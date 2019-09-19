using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InsurgencySandstormRcon
{
    public class RconPacketLog
    {
        static RconPacketLog instance;
        public static RconPacketLog Instance
        {
            get
            {
                if (instance == null)
                    instance = new RconPacketLog();
                return instance;
            }
        }

        List<RconDebugPacket> packets;
        public List<RconDebugPacket> Packets
        {
            get { return packets; }
        }

        public RconPacketLog()
        {
            instance = this;
            packets = new List<RconDebugPacket>();
        }
    }
}
