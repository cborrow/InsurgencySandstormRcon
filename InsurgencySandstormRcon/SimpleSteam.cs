using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.IO;
using System.Net;
using System.Text.RegularExpressions;

namespace InsurgencySandstormRcon
{
    public class SimpleSteam
    {
        public SimpleSteam()
        {

        }

        public SteamUser GetUserByID(string steamID64)
        {
            Regex steamIDRegex = new Regex("([0-9]{17})");

            if (steamIDRegex.IsMatch(steamID64))
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(string.Format("https://steamcommunity.com/profiles/{0}?xml=1", steamID64));

                using(HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                {
                    Stream s = response.GetResponseStream();
                    string xmlData = string.Empty;

                    using(StreamReader sr = new StreamReader(s))
                    {
                        xmlData = sr.ReadToEnd();
                        sr.Close();
                    }

                    if(!string.IsNullOrEmpty(xmlData))
                    {
                        XmlDocument xml = new XmlDocument();
                        xml.LoadXml(xmlData);

                        XmlNode root = xml.DocumentElement;
                        SteamUser steamUser = new SteamUser();
                        steamUser.SteamID64 = steamID64;

                        foreach(XmlNode item in root.ChildNodes)
                        {
                            if(item.Name == "steamID")
                            {
                                steamUser.SteamID = item.InnerText;
                            }
                            else if(item.Name == "avatarIcon")
                            {
                                steamUser.AvatarIconURL = item.InnerText;
                            }
                            else if(item.Name == "customURL")
                            {
                                steamUser.CustomURL = "https://steamcommunity.com/id/" + item.InnerText;
                            }
                        }

                        return steamUser;
                    }

                    if (s != null)
                        s.Close();
                }
            }

            return null;
        }
    }
}
