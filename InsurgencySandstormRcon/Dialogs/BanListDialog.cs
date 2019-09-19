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

        private void button1_Click(object sender, EventArgs e)
        {
            string selectedPlayer = (string)listBox1.SelectedItem;

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
