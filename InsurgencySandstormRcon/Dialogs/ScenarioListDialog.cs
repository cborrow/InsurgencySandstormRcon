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
    public partial class ScenarioListDialog : Form
    {
        List<string> scenarioNameTable;

        string selectedScenario = string.Empty;
        public string SelectedScenario
        {
            get { return selectedScenario; }
            set { selectedScenario = value; }
        }

        public ScenarioListDialog()
        {
            InitializeComponent();

            scenarioNameTable = new List<string>();
        }

        public DialogResult ShowDialog(string[] scenarios)
        {
            scenarioNameTable.AddRange(scenarios);

            foreach(string s in scenarios)
            {
                listBox1.Items.Add(FormatScenarioName(s));
            }

            return this.ShowDialog();
        }

        protected string FormatScenarioName(string scenario)
        {
            if (scenario.Length > 0 && scenario.Contains("_"))
            {
                string[] parts = scenario.Split('_');

                if (parts.Length == 3)
                    return string.Format("{0} on {1}", parts[2], parts[1]);
                else if (parts.Length == 4)
                    return string.Format("{0} {1} on {2}", parts[2], parts[3], parts[1]);
            }
            return string.Empty;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            int index = listBox1.SelectedIndex;

            if(index > 0 && index < scenarioNameTable.Count)
            {
                selectedScenario = scenarioNameTable[index];
                DialogResult = DialogResult.OK;
            }
            else
                DialogResult = DialogResult.Cancel;

            this.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }
    }
}
