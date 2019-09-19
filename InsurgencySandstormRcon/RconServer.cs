using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Security;
using System.Security.Cryptography;

using Newtonsoft.Json;

namespace InsurgencySandstormRcon
{
    public class RconServer
    {
        
        SuperSimpleRconLib rcon;
        [JsonIgnore]
        public SuperSimpleRconLib Rcon
        {
            get { return rcon; }
        }

        string name;
        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        string host;
        public string Host
        {
            get { return host; }
            set
            {
                host = value;
                rcon.Host = host;
            }
        }

        string rconPwd;
        public string RconPassword
        {
            get { return rconPwd; }
            set
            {
                rconPwd = value;
                rcon.Password = rconPwd;
            }
        }

        string guid;
        public string Guid
        {
            get { return guid; }
            internal set { guid = value; }
        }

        int gamePort;
        public int GamePort
        {
            get { return gamePort; }
            set { gamePort = value; }
        }

        int queryPort;
        public int QueryPort
        {
            get { return queryPort; }
            set { queryPort = value; }
        }

        int rconPort;
        public int RconPort
        {
            get { return rconPort; }
            set
            {
                rconPort = value;
                rcon.Port = rconPort;
            }
        }

        public RconServer()
        {
            rcon = new SuperSimpleRconLib();
        }

        public void Send(string command)
        {
            rcon.Host = host;
            rcon.Port = rconPort;
            rcon.Password = rconPwd;
            //rcon.Password = GetDecryptedPassword();
            //TODO : Change to require password each request so it's not saved in the SuperSimpleRconLib instance

            try
            {
                rcon.SendCommand(command);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception : {0}", ex.Message);
            }
        }

        /*public string GetDecryptedPassword()
        {
            try
            {
                string pwd = Encoding.Unicode.GetString(
                    ProtectedData.Unprotect(Convert.FromBase64String(rconPwd), null, DataProtectionScope.CurrentUser));
                return pwd;
            }
            catch(Exception ex)
            {
                Console.WriteLine("Failed to decrypt rcon password : " + ex.Message);
            }
            return string.Empty;
        }

        public bool SetEncryptedPassword(string pwd)
        {
            try
            {
                string encPwd = Convert.ToBase64String(ProtectedData.Protect(
                    Encoding.Unicode.GetBytes(pwd), null, DataProtectionScope.CurrentUser));
                return true;
            }
            catch(Exception ex)
            {
                Console.WriteLine("Failed to encrypt rcon password : " + ex.Message);
            }
            //TODO : Decide if we should use unprotected password, notify user, give user option to not encrypt passwords?
            return false;
        }*/

        public override string ToString()
        {
            string output = string.Format("{0} ({1}:{2})", name, host, rconPort);
            return output;
        }
    }
}
