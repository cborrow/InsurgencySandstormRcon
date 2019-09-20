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
    public partial class DebugPacketDialog : Form
    {
        public DebugPacketDialog()
        {
            InitializeComponent();
        }

        public DialogResult ShowDialog(RconDebugPacket packet)
        {
            idLabel.Text = "Packet Id : " + packet.Id.ToString();
            typeLabel.Text = "Type : " + packet.TypeToString();
            lengthLabel.Text = "Length : " + packet.Size.ToString();
            directionLabel.Text = "Direction : " + packet.DirectionToString();
            richTextBox1.Text = packet.Data;

            return this.ShowDialog();
        }
    }
}
