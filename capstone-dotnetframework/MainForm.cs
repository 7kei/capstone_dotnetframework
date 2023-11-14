using System;
using System.Windows.Forms;
using Microsoft.VisualBasic;    

namespace capstone_dotnetframework
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            Program.Initialize();
        }

        public void WriteToOutputWindow(string output)
        {
            if (!output.EndsWith("\n"))
                WriteTextSafe(output + "\n");
            else
                WriteTextSafe(output);
        }

        public void WriteTextSafe(string text)
        {
            if (MainOutputBox.InvokeRequired)
            {
                Action safeWrite = delegate { WriteTextSafe($"{text}"); };
                MainOutputBox.Invoke(safeWrite);
            }
            else
                MainOutputBox.AppendText(text);
        }

        private void FallbackModeOnButton_Click(object sender, EventArgs e)
        {
            Program.turnOnFallbackMode();
        }

        private void FallbackModeOffButton_Click(object sender, EventArgs e)
        {
            Program.turnOffFallbackMode();
        }

        private void ExportAttendanceButton_Click(object sender, EventArgs e)
        {
            var date = DateTime.Now.ToString("yyyy-MM-dd");
            var section = "12 - HELIUM";

            section = Interaction.InputBox($"Input Grade and Section \nFormat: GRADELEVEL - SECTION \n(example: 12 - HELIUM)\nLeave blank for all users entered regardless of section.", "Export Attendance", section);
            date = Interaction.InputBox($"Input Date \nFormat: yyyy-MM-dd \n(example: 2023-05-04)\nLeave blank for the current date", "Export Attendance", date);

            SaveFileDialog saveFileDialog1 = new SaveFileDialog();
            saveFileDialog1.Filter = "CSV File|*.csv";
            saveFileDialog1.Title = "Save CSV";
            DialogResult result = saveFileDialog1.ShowDialog();
            if (result == DialogResult.OK)
            {
                if (saveFileDialog1.FileName != "")
                    Program.exportAttendanceFile(saveFileDialog1.FileName, date, section);

            }
            
        }
    }
}
