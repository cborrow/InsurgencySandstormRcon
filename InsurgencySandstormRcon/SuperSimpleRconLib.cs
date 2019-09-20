using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;

namespace InsurgencySandstormRcon
{
    public class SuperSimpleRconLib
    {
        int connID;
        bool authenticated = false;

        const int SERVERDATA_AUTH = 0x03;
        const int SERVERDATA_AUTH_RESPONSE = 0x02;
        const int SERVERDATA_EXECCOMMAND = 0x02;
        const int SERVERDATA_RESPONSE_VALUE = 0x00;
        const int SERVERDATA_NONE = -1;

        const int PACKET_HEADER_LENGTH = 12;
        const int SIZE_LENGTH = 4;
        const int ID_LENGTH = 4;
        const int TYPE_LENGTH = 4;

        /*
         * Source Rcon Packet Structure
         * Size     Int32       4 Bytes
         * ID       Int32       4 Bytes
         * Type     Int32       4 Bytes
         * Body     String      Varies, Null-terminated
         * Empty    String      Null-terminated string (0x00)
         * 
         * Multi-part packets for some servers, the first packet is normal and the rest are just data
         * no packet information just all bytes of data.
         */

        string host;
        public string Host
        {
            get { return host; }
            set { host = value; }
        }

        int port;
        public int Port
        {
            get { return port; }
            set { port = value; }
        }

        string password;
        public string Password
        {
            get { return password; }
            set { password = value; }
        }

        public SuperSimpleRconLib()
        {
            Random r = new Random();
            connID = r.Next(1000, 10000);
        }

        public string SendCommand(string command)
        {
            string output = string.Empty;

            using (Socket s = new Socket(SocketType.Stream, ProtocolType.Tcp))
            {
                s.ReceiveTimeout = 5000;
                s.Connect(new IPEndPoint(IPAddress.Parse(host), port));
                SendLogin(s);
                output = SendServerCommand(s, command);
                s.Close();

                authenticated = false;
            }

            return output;
        }

        public void GenerateDebugInfo(byte[] packet, PacketDirection dir)
        {
            long time = DateTime.Now.Ticks;

            RconDebugPacket debugPacket = new RconDebugPacket();
            debugPacket.Size = GetPacketLength(packet);
            debugPacket.Id = GetPacketId(packet);
            debugPacket.Type = GetPacketType(packet);
            if (dir == PacketDirection.Sent && debugPacket.Type == SERVERDATA_AUTH)
                debugPacket.Data = "********";
            else
                debugPacket.Data = GetPacketData(packet);
            debugPacket.Direction = dir;

            RconPacketLog.Instance.Packets.Add(debugPacket);
        }

        protected void SendLogin(Socket s)
        {
            byte[] packet = GetPacketBytes(SERVERDATA_AUTH, Security.DecryptPassword(password));
            byte[] buffer = new byte[4096];
            int length = 0;

            if(s.Connected)
            {
                GenerateDebugInfo(packet, PacketDirection.Sent);
                length = s.Send(packet);
                Console.WriteLine("Sent {0} bytes to host.", length);

                length = s.Receive(buffer, SocketFlags.None);
                Console.WriteLine("Received {0} bytes from host.", length);

                if (GetPacketType(buffer) == SERVERDATA_RESPONSE_VALUE)
                {
                    length = s.Receive(buffer, buffer.Length - 1, SocketFlags.None);
                    Console.WriteLine("Received {0} bytes from host.", length);
                    GenerateDebugInfo(buffer, PacketDirection.Received);

                    if (GetPacketType(buffer) == SERVERDATA_AUTH_RESPONSE)
                    {
                        int id = GetPacketId(buffer);

                        if (id == connID)
                        {
                            authenticated = true;
                            Console.WriteLine("Authentication successful");
                        }
                        else
                        {
                            authenticated = false;
                            Console.WriteLine("Authentication failed");
                        }
                    }
                }
                else if (GetPacketType(buffer) == SERVERDATA_AUTH_RESPONSE)
                {
                    int id = GetPacketId(buffer);

                    if (id == connID)
                    {
                        authenticated = true;
                        Console.WriteLine("Authentication successful");
                    }
                    else
                    {
                        authenticated = false;
                        Console.WriteLine("Authentication failed");
                    }
                }
            }
        }

        protected string SendServerCommand(Socket s, string command)
        {
            byte[] packet = GetPacketBytes(SERVERDATA_EXECCOMMAND, command);
            byte[] multiPartPacket = new byte[122880]; //Enough space for 30 4096b packets
            int bytesRead = 0;

            if (s.Connected && authenticated)
            {
                byte[] buffer = new byte[4096];
                int length = 0;

                GenerateDebugInfo(packet, PacketDirection.Sent);
                length = s.Send(packet);
                Console.WriteLine("Sent {0} bytes to host.", length);

                //Send second empty packet for multi-packet responses
                byte[] cPacket = GetPacketBytes(SERVERDATA_RESPONSE_VALUE, "");
                GenerateDebugInfo(cPacket, PacketDirection.Sent);
                s.Send(cPacket);

                //Read first packet
                length = s.Receive(buffer, SocketFlags.None);
                Console.WriteLine("Received {0} bytes from host.", length);
                GenerateDebugInfo((byte[])buffer.Clone(), PacketDirection.Received);
                buffer.CopyTo(multiPartPacket, 0);
                bytesRead += length;

                //Read any additional packets. At the minimum we should recieve 
                //one empty packet signafying the end of response packets
                while (true)
                {
                    buffer = new byte[4096];
                    try
                    {
                        Console.WriteLine("Reading packet...");
                        length = s.Receive(buffer, SocketFlags.None);
                        Console.WriteLine("Received {0} bytes from host.", length);
                        GenerateDebugInfo((byte[])buffer.Clone(), PacketDirection.Received);

                        //We've gotten to the last packet. Break out of the loop
                        int id = GetPacketId(buffer);
                        if (id == connID)
                            break;

                        int size = GetPacketLength(buffer);

                        if (size == 10)
                            continue; //Skip empty packets

                        if(size < 4096)
                        {
                            //This packet is a normal packet, but we only need the data from it
                            Buffer.BlockCopy(buffer, PACKET_HEADER_LENGTH, multiPartPacket, bytesRead, (size - 9));
                            bytesRead += (size - 9); //Size of packet minus id, type fields and final null-terminated string
                        }
                        else
                        {
                            buffer.CopyTo(multiPartPacket, bytesRead);
                            bytesRead += length;
                        }

                        /*
                         * Slow down read requests from server. 
                         * Going to fast was causing 4096b packets followed by 100-2000b packets followed by 4096b again.
                         * Either not all data was being read or all data was read but the bytes read returned was not correct
                         * This can likely be changed to 5 or lower and still work. 
                         * But 15 works without any really noticable time delay
                         */
                        System.Threading.Thread.Sleep(15);
                    }
                    catch(TimeoutException tex)
                    {
                        Console.WriteLine("Receving from server timed out. Avaliable data will be returned");
                        break;
                    } 
                    catch(Exception ex)
                    {
                        Console.WriteLine("An exception occured while reading from server {0}", ex.Message);
                        Console.WriteLine("Avaliable data will be returned");
                        break;
                    }
                }

                string data = GetPacketData(multiPartPacket);
                return data;
            }
            else
            {
                Console.WriteLine("Not connected or not authenticated. Unable to send command.");
            }
            return string.Empty;
        }

        byte[] GetPacketBytes(int dataType, string command)
        {
            byte[] packetData = new byte[PACKET_HEADER_LENGTH + command.Length + 2]; //PACKET_HEADER_LENGTH is the length of SIZE, ID, and TYPE fields
            byte[] size;
            byte[] id;
            byte[] type;
            byte[] data;
            byte[] empty = new byte[] { 0x00 };

            connID++; //Increase id for use with determining the end of multi-part packets
            size = BitConverter.GetBytes(packetData.Length - 4);
            id = BitConverter.GetBytes(connID);
            type = BitConverter.GetBytes(dataType);
            //data = UTF8Encoding.Default.GetBytes(command + '\0'); 
            data = Encoding.ASCII.GetBytes(command + '\0'); //Add null-terminator to end of string

            size.CopyTo(packetData, 0);
            id.CopyTo(packetData, SIZE_LENGTH);
            type.CopyTo(packetData, (SIZE_LENGTH + ID_LENGTH));
            data.CopyTo(packetData, PACKET_HEADER_LENGTH);
            empty.CopyTo(packetData, packetData.Length - 1);

            return packetData;
        }

        int GetPacketType(byte[] packet)
        {
            if(packet.Length >= PACKET_HEADER_LENGTH)
            {
                int type = BitConverter.ToInt32(packet, 8);
                return type;
            }
            return SERVERDATA_NONE;
        }

        int GetPacketId(byte[] packet)
        {
            if (packet.Length >= PACKET_HEADER_LENGTH)
            {
                int id = BitConverter.ToInt32(packet, 4);
                return id;
            }
            return SERVERDATA_NONE;
        }

        int GetPacketLength(byte[] packet)
        {
            if(packet.Length > 4)
            {
                int length = BitConverter.ToInt32(packet, 0);
                return length;
            }
            return 0;
        }

        void SetPacketLength(ref byte[] packet, int newLength)
        {
            byte[] newLengthBytes = BitConverter.GetBytes(newLength);
            packet[0] = newLengthBytes[0];
            packet[1] = newLengthBytes[1];
            packet[2] = newLengthBytes[2];
            packet[3] = newLengthBytes[3];
        }

        string GetPacketData(byte[] packet)
        {
            //Instead of using packet length go by where packet data should start to the length - packet header - 1 for null-terminated string
            //Some multi-part packets do not include the total length of all of the packets and rather the length of the first complete packet
            //However, that also requires that we filter out null terminators / empty bytes
            //string data = UTF8Encoding.ASCII.GetString(packet, 12, packet.Length - 13);
            string data = string.Empty;

            //Encoding.

            if (GetPacketId(packet) <= connID)
            {
                int length = GetPacketLength(packet);
                length = Math.Min(length, 4084);
                data = Encoding.UTF8.GetString(packet, PACKET_HEADER_LENGTH, packet.Length - (PACKET_HEADER_LENGTH + 1));
            }
            else
                data = Encoding.UTF8.GetString(packet, 0, packet.Length - 1);

            //Filter out null terminators and the empty end of the packet
            data = data.Replace('\0', '^');
            data = data.Replace("^", string.Empty);

            return data;
        }
    }
}
