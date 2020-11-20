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
        System.Windows.Forms.Timer passwordUpdatedTimer;

        RconServer selectedServer;
        int selectedIndex = 0;

        public ServerManagmentDialog()
        {
            InitializeComponent();

            passwordUpdatedTimer = new Timer();
            passwordUpdatedTimer.Interval = 500;
            passwordUpdatedTimer.Tick += PasswordUpdatedTimer_Tick;
        }

        protected override void OnLoad(EventArgs e)
        {
            ClearFields();
            listBox1.DataSource = RconServerManager.Instance.Servers;
            base.OnLoad(e);
        }

        protected RconServer GetSelectedServer()
        {
            if (listBox1.SelectedItems.Count > 0)
                return (RconServer)listBox1.SelectedItems[0];
            return null;
        }

        protected int GetSelectedServerIndex()
        {
            if (listBox1.SelectedIndices.Count > 0)
                return listBox1.SelectedIndices[0];
            return -1;
        }

        protected void ClearFields()
        {
            textBox1.Text = string.Empty;
            textBox2.Text = string.Empty;
            numericUpDown1.Value = 0;
            numericUpDown2.Value = 0;
            numericUpDown3.Value = 0;
        }

        protected void ServerListUpdated()
        {
            listBox1.DataSource = null;
            listBox1.DataSource = RconServerManager.Instance.Servers;
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBox1.SelectedItem != null)
            {
                int index = GetSelectedServerIndex();
                selectedServer = RconServerManager.Instance.Servers[index];
                selectedIndex = index;

                textBox1.Text = selectedServer.Name;
                textBox2.Text = selectedServer.Host;
                numericUpDown1.Value = selectedServer.GamePort;
                numericUpDown2.Value = selectedServer.QueryPort;
                numericUpDown3.Value = selectedServer.RconPort;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            RconServerManager.Instance.Save();
            listBox1.SelectedIndex = -1;
            DialogResult = DialogResult.OK;
            this.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            selectedIndex = -1;
            selectedServer = null;
            textBox1.Text = string.Empty;
            textBox2.Text = string.Empty;
            numericUpDown1.Value = 0;
            numericUpDown2.Value = 0;
            numericUpDown3.Value = 0;
            textBox6.Text = string.Empty;

            DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (listBox1.SelectedIndex >= 0)
            {
                int index = GetSelectedServerIndex();
                RconServerManager.Instance.Servers.RemoveAt(index);
                RconServerManager.Instance.Save();
                ServerListUpdated();
            }
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            RconServer server = new RconServer();
            server.GamePort = 0;
            server.RconPort = 0;
            server.QueryPort = 0;
            server.Name = "Unknown server";
            server.Host = "NO HOST SPECIFIED";
            RconServerManager.Instance.Servers.Add(server);
            ServerListUpdated();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            int index = GetSelectedServerIndex();

            if (index >= 0)
            {
                RconServerManager.Instance.Servers[index].Name = textBox1.Text;
                ServerListUpdated();
            }
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            int index = GetSelectedServerIndex();

            if (index >= 0)
            {
                RconServerManager.Instance.Servers[index].Host = textBox2.Text;
                ServerListUpdated();
            }
        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            int index = GetSelectedServerIndex();

            if (index >= 0)
            {
                RconServerManager.Instance.Servers[index].GamePort = (int)numericUpDown1.Value;
                ServerListUpdated();
            }
        }

        private void numericUpDown2_ValueChanged(object sender, EventArgs e)
        {
            int index = GetSelectedServerIndex();

            if (index >= 0)
            {
                RconServerManager.Instance.Servers[index].QueryPort = (int)numericUpDown2.Value;
                ServerListUpdated();
            }
        }

        private void numericUpDown3_ValueChanged(object sender, EventArgs e)
        {
            int index = GetSelectedServerIndex();

            if (index >= 0)
            {
                RconServerManager.Instance.Servers[index].RconPort = (int)numericUpDown3.Value;
                ServerListUpdated();
            }
        }

        private void PasswordUpdatedTimer_Tick(object sender, EventArgs e)
        {
            
        }
    }
}
