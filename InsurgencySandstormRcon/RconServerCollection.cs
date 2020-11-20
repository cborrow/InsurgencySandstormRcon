using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InsurgencySandstormRcon
{
    public class RconServerCollection : Collection<RconServer>
    {
        public RconServerCollection() : base()
        {

        }

        public new void Add(RconServer server)
        {
            if(server.Guid == string.Empty)
            {
                server.Guid = Guid.NewGuid().ToString();
            }
            this.Items.Add(server);
        }

        public void AddRange(IEnumerable<RconServer> servers)
        {
            foreach(RconServer rs in servers)
            {
                this.Items.Add(rs);
            }
        }

        public bool Contains(string guid)
        {
            foreach(RconServer server in this.Items)
            {
                if (server.Guid == guid)
                    return true;
            }
            return false;
        }

        public new bool Contains(RconServer server)
        {
            if (this.Items.Contains(server))
                return true;
            return Contains(server.Guid);
        }
    }
}
