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
    public partial class KickPlayersDialog : Form
    {
        public KickPlayersDialog()
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
            string reason = textBox1.Text;
            reason = Encoding.ASCII.GetString(Encoding.ASCII.GetBytes(reason));

            foreach(ListViewItem lvi in listView1.Items)
            {
                string id = (string)lvi.Tag;
                RconServerManager.Instance.ActiveServer.Rcon.SendCommand(string.Format("kick {0} {1}", id, reason));
            }

            MessageBox.Show("Kick request has been sent to the server for each of the selected players");
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
