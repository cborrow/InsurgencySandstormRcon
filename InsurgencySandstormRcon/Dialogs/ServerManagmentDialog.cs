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
    public partial class ServerManagmentDialog : Form
    {
        RconServer selectedServer;
        int selectedIndex = 0;

        public ServerManagmentDialog()
        {
            InitializeComponent();
        }

        protected override void OnLoad(EventArgs e)
        {
            listBox1.Items.Clear();
            listBox1.Items.AddRange(RconServerManager.Instance.Servers.ToArray());
            base.OnLoad(e);
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBox1.SelectedItem != null)
            {
                RconServer server = (RconServer)listBox1.SelectedItem;
                selectedServer = server;
                selectedIndex = listBox1.SelectedIndex;

                textBox1.Text = server.Name;
                textBox2.Text = server.Host;
                numericUpDown1.Value = server.GamePort;
                numericUpDown2.Value = server.QueryPort;
                numericUpDown3.Value = server.RconPort;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if(selectedServer != null && selectedIndex != 0)
            {
                int index = listBox1.SelectedIndex;
                selectedServer.Name = textBox1.Text;
                selectedServer.Host = textBox2.Text;
                selectedServer.GamePort = (int)numericUpDown1.Value;
                selectedServer.QueryPort = (int)numericUpDown2.Value;
                selectedServer.RconPort = (int)numericUpDown3.Value;

                if (!string.IsNullOrEmpty(textBox6.Text))
                    selectedServer.RconPassword = Security.EncryptPassword(textBox6.Text);

                if(index >= 0 && index < RconServerManager.Instance.Servers.Count)
                    RconServerManager.Instance.Servers[index] = selectedServer;
            }
            else
            {
                RconServer server = new RconServer();
                server.Name = textBox1.Text;
                server.Host = textBox2.Text;
                server.GamePort = (int)numericUpDown1.Value;
                server.QueryPort = (int)numericUpDown2.Value;
                server.RconPort = (int)numericUpDown3.Value;

                if (!string.IsNullOrEmpty(textBox6.Text))
                    server.RconPassword = Security.EncryptPassword(textBox6.Text);

                listBox1.Items.Add(server);
                RconServerManager.Instance.Servers.Add(server);
            }

            RconServerManager.Instance.Save();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            selectedIndex = 0;
            selectedServer = null;
            textBox1.Text = string.Empty;
            textBox2.Text = string.Empty;
            numericUpDown1.Value = 0;
            numericUpDown2.Value = 0;
            numericUpDown3.Value = 0;
            textBox6.Text = string.Empty;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (listBox1.SelectedItem != null)
            {
                RconServer server = (RconServer)listBox1.SelectedItem;
                RconServerManager.Instance.Servers.Remove(server);
                listBox1.Items.Remove(listBox1.SelectedItem);
            }
        }
    }
}
