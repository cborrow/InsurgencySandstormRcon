using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InsurgencySandstormRcon
{
    public class SteamUser
    {
        string steamID64;
        public string SteamID64
        {
            get { return steamID64; }
            set { steamID64 = value; }
        }

        string steamID;
        public string SteamID
        {
            get { return steamID; }
            set { steamID = value; }
        }

        public string DisplayName
        {
            get { return steamID; }
        }

        string avatarIconURL;
        public string AvatarIconURL
        {
            get { return avatarIconURL; }
            set { avatarIconURL = value; }
        }

        string customURL;
        public string CustomURL
        {
            get { return customURL; }
            set { customURL = value; }
        }
    }
}
