using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace InsurgencySandstormRcon.Dialogs
{
    public partial class BanPlayersDialog : Form
    {
        public BanPlayersDialog()
        {
            InitializeComponent();
        }

        public DialogResult ShowDialog(ListViewItem[] players)
        {
            listView1.Items.AddRange(players);

            return this.ShowDialog();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string time = numericUpDown1.Value.ToString();
            string reason = textBox1.Text;
            reason = Encoding.ASCII.GetString(Encoding.ASCII.GetBytes(reason));

            if (RconServerManager.Instance.ActiveServer != null)
            {
                foreach (ListViewItem lvi in listView1.Items)
                {
                    string id = (string)lvi.Tag;
                    if (checkBox1.Checked)
                    {
                        RconServerManager.Instance.ActiveServer.Rcon.SendCommand(string.Format("permban {0} {1}", id, reason));
                        MessageBox.Show("A permban request has been sent to the server for each of the selected players");
                    }
                    else
                    {
                        RconServerManager.Instance.ActiveServer.Rcon.SendCommand(string.Format("ban {0} {1} {2}", id, time, reason));
                        MessageBox.Show("A ban request for " + time + " minutes has been sent to the server for each of the selected players");
                    }
                }
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
