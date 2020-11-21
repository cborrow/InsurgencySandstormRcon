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

            toggleablePasswordBox1.TextChanged += ToggleablePasswordBox1_TextChanged;
        }

        protected override void OnLoad(EventArgs e)
        {
            ClearFields();
            ServerListUpdated();

            base.OnLoad(e);
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            listBox1.SelectedIndex = -1;
            ClearFields();
            base.OnClosing(e);
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
            toggleablePasswordBox1.Text = string.Empty;
        }

        protected void ServerListUpdated()
        {
            listBox1.DataSource = null;
            listBox1.DataSource = RconServerManager.Instance.Servers;
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBox1.SelectedIndex >= 0)
            {
                int index = GetSelectedServerIndex();
                selectedServer = RconServerManager.Instance.Servers[index];
                selectedIndex = index;

                textBox1.Text = selectedServer.Name;
                textBox2.Text = selectedServer.Host;
                numericUpDown1.Value = selectedServer.GamePort;
                numericUpDown2.Value = selectedServer.QueryPort;
                numericUpDown3.Value = selectedServer.RconPort;
                toggleablePasswordBox1.Text = Security.DecryptPassword(selectedServer.RconPassword);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            RconServerManager.Instance.Save();
            listBox1.SelectedIndex = -1;
            DialogResult = DialogResult.OK;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            listBox1.SelectedIndex = -1;
            DialogResult = DialogResult.Cancel;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (listBox1.SelectedIndex >= 0)
            {
                int index = GetSelectedServerIndex();
                RconServerManager.Instance.Servers.RemoveAt(index);
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
            RconServerManager.Instance.UnsavedChanges = true;
            ServerListUpdated();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            int index = GetSelectedServerIndex();

            if (index >= 0)
            {
                RconServerManager.Instance.Servers[index].Name = textBox1.Text;
                RconServerManager.Instance.UnsavedChanges = true;
                ServerListUpdated();
            }
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            int index = GetSelectedServerIndex();

            if (index >= 0)
            {
                RconServerManager.Instance.Servers[index].Host = textBox2.Text;
                RconServerManager.Instance.UnsavedChanges = true;
                ServerListUpdated();
            }
        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            int index = GetSelectedServerIndex();

            if (index >= 0)
            {
                RconServerManager.Instance.Servers[index].GamePort = (int)numericUpDown1.Value;
                RconServerManager.Instance.UnsavedChanges = true;
                ServerListUpdated();
            }
        }

        private void numericUpDown2_ValueChanged(object sender, EventArgs e)
        {
            int index = GetSelectedServerIndex();

            if (index >= 0)
            {
                RconServerManager.Instance.Servers[index].QueryPort = (int)numericUpDown2.Value;
                RconServerManager.Instance.UnsavedChanges = true;
                ServerListUpdated();
            }
        }

        private void numericUpDown3_ValueChanged(object sender, EventArgs e)
        {
            int index = GetSelectedServerIndex();

            if (index >= 0)
            {
                RconServerManager.Instance.Servers[index].RconPort = (int)numericUpDown3.Value;
                RconServerManager.Instance.UnsavedChanges = true;
                ServerListUpdated();
            }
        }

        private void ToggleablePasswordBox1_TextChanged(object sender, EventArgs e)
        {
            int index = GetSelectedServerIndex();

            if (index > 0)
            {
                RconServerManager.Instance.Servers[index].RconPassword = Security.EncryptPassword(toggleablePasswordBox1.Text);
                RconServerManager.Instance.UnsavedChanges = true;
            }
        }
    }
}
