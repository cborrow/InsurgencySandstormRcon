using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace InsurgencySandstormRcon.Controls
{
    public partial class ToggleablePasswordBox : UserControl
    {
        public new event EventHandler TextChanged;
        char passwordChar = '*';

        public string Text
        {
            get { return textBox1.Text; }
            set { textBox1.Text = value; }
        }

        bool passwordVisible = false;
        public bool PasswordVisible
        {
            get { return passwordVisible; }
            set
            {
                passwordVisible = value;

                if (!passwordVisible)
                    textBox1.PasswordChar = passwordChar;
                else
                    textBox1.PasswordChar = (char)0;
            }
        }
        
        public ToggleablePasswordBox()
        {
            InitializeComponent();

            textBox1.TextChanged += TextBox1_TextChanged;
            TextChanged += OnTextChanged;
        }

        protected virtual void OnTextChanged(object sender, EventArgs e)
        {

        }

        private void TextBox1_TextChanged(object sender, EventArgs e)
        {
            TextChanged(sender, e);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            PasswordVisible = !passwordVisible;
        }
    }
}
