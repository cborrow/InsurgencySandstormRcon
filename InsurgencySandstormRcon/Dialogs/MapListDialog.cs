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
    public partial class MapListDialog : Form
    {
        string selectedMap;
        public string SelectedMap
        {
            get { return selectedMap; }
        }

        public MapListDialog()
        {
            InitializeComponent();
        }

        public DialogResult ShowDialog(string[] maps, bool changeMap)
        {
            listBox1.Items.Clear();
            listBox1.Items.AddRange(maps);

            if(changeMap)
            {
                button1.Text = "Change";
                button2.Enabled = true;
                button2.Visible = true;
                this.Text = "Select a map to move to";
            }
            else
            {
                button1.Text = "Close";
                button2.Enabled = false;
                button2.Visible = false;
                this.Text = "Listing server maps";
            }

            return this.ShowDialog();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
            this.Close();
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(listBox1.SelectedItem != null)
            {
                selectedMap = (string)listBox1.SelectedItem;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            this.Close();
        }
    }
}
