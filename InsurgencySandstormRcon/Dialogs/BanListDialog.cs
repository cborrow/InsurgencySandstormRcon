using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace InsurgencySandstormRcon
{
    public partial class BanListDialog : Form
    {
        public BanListDialog()
        {
            InitializeComponent();
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            listBox1.Items.Clear();
            if(RconServerManager.Instance.ActiveServer != null)
            {
                string result = RconServerManager.Instance.ActiveServer.Rcon.SendCommand("listbans");
                ParseBanListString(result);
            }
        }

        protected void ParseBanListString(string banlist)
        {
            string[] bannedUsers = banlist.Split(')');

            foreach(string banEntry in bannedUsers)
            {
                if (string.IsNullOrEmpty(banEntry))
                    continue;

                string[] parts = banEntry.Split(' ');

                if (parts.Length > 1 && banEntry.Contains("("))
                {
                    string user = parts[0];
                    string dateStr = parts[1];
                    DateTime dt = DateTime.Now;

                    if (DateTime.TryParse(parts[1], out dt))
                    {
                        dateStr = dt.ToString();
                    }

                    int reasonIndex = banEntry.IndexOf("(", 20) + 1;

                    string reason = banEntry.Substring(reasonIndex, (banEntry.Length - reasonIndex));

                    listBox1.Items.Add(string.Format("{0} on {1} | {2}", user, dateStr, reason));
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string selectedPlayer = (string)listBox1.SelectedItem;
            string id = selectedPlayer.Split(' ')[0]; //Find a better way to get ID

            if(selectedPlayer != null)
            {
                RconServerManager.Instance.ActiveServer.Rcon.SendCommand(string.Format("unban {0}", selectedPlayer));
            }

            DialogResult = DialogResult.OK;
            this.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            this.Close();
        }
    }
}
