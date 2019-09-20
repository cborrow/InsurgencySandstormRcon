using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InsurgencySandstormRcon
{
    public enum PacketDirection
    {
        Sent,
        Received
    };

    public class RconDebugPacket
    {
        int size;
        public int Size
        {
            get { return size; }
            set { size = value; }
        }

        int id;
        public int Id
        {
            get { return id; }
            set { id = value; }
        }

        int type;
        public int Type
        {
            get { return type; }
            set { type = value; }
        }

        PacketDirection direction;
        public PacketDirection Direction
        {
            get { return direction; }
            set { direction = value; }
        }

        long packetTime;
        public long Time
        {
            get { return packetTime; }
            set { packetTime = value; }
        }

        string data;
        public string Data
        {
            get { return data; }
            set { data = value; }
        }

        public RconDebugPacket()
        {

        }

        public string DirectionToString()
        {
            if (direction == PacketDirection.Received)
                return "Received";
            else if (direction == PacketDirection.Sent)
                return "Sent";
            return "Unknown";
        }

        public string TypeToString()
        {
            if (direction == PacketDirection.Sent)
            {
                if (type == 0x03)
                    return "SERVERDATA_AUTH";
                else if (type == 0x02)
                    return "SERVERDATA_EXECCOMMAND";
                else if (type == 0x00)
                    return "SERVERDATA_EMPTY_PACKET";
            }
            else
            {
                if (type == 0x02)
                    return "SERVERDATA_AUTH_RESPONSE";
                else if (type == 0x00)
                    return "SERVERDATA_RESPONSE_VALUE";
                else
                    return "SERVERDATA_VALUE_CONT";
            }
            return "Unknown";
        }
    }
}
