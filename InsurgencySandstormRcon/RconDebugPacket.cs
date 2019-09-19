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
    }
}
