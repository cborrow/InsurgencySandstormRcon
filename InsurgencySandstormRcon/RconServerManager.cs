using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Windows.Forms;

using Newtonsoft.Json;

namespace InsurgencySandstormRcon
{
    public class RconServerManager
    {
        string rconDataFilePath;

        static RconServerManager instance;
        public static RconServerManager Instance
        {
            get
            {
                if (instance == null)
                    instance = new RconServerManager();
                return instance;
            }
        }

        RconServerCollection previouslySavedServers;
        RconServerCollection servers;
        public RconServerCollection Servers
        {
            get { return servers; }
            set { servers = value; }
        }

        RconServer activeServer;
        public RconServer ActiveServer
        {
            get { return activeServer; }
            set { activeServer = value; }
        }

        bool unsavedChanges = false;
        public bool UnsavedChanges
        {
            get { return unsavedChanges; }
            set { unsavedChanges = value; }
        }

        public RconServerManager()
        {
            instance = this;
            servers = new RconServerCollection();
            previouslySavedServers = servers;

            rconDataFilePath = Path.Combine(Application.UserAppDataPath, "RconServers.json");
        }

        public void Load()
        {
            if (!File.Exists(rconDataFilePath))
            {
                //Test();
                return;
            }

            string data = File.ReadAllText(rconDataFilePath);
            RconServer[] rconServers = JsonConvert.DeserializeObject<RconServer[]>(data);
            servers.AddRange(rconServers);
            previouslySavedServers = servers;
        }

        public void Save()
        {
            string data = JsonConvert.SerializeObject(servers);
            File.WriteAllText(rconDataFilePath, data);
            previouslySavedServers = servers;
            unsavedChanges = false;
        }

        public void RevertUnsavedChanges()
        {
            servers = previouslySavedServers;
        }
    }
}
