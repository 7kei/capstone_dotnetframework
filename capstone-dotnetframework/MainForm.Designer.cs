namespace capstone_dotnetframework
{
    partial class MainForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.MainOutputBox = new System.Windows.Forms.RichTextBox();
            this.FallbackModeOnButton = new System.Windows.Forms.Button();
            this.FallbackModeOffButton = new System.Windows.Forms.Button();
            this.ExportAttendanceButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // MainOutputBox
            // 
            this.MainOutputBox.BackColor = System.Drawing.Color.Black;
            this.MainOutputBox.ForeColor = System.Drawing.Color.White;
            this.MainOutputBox.Location = new System.Drawing.Point(12, 12);
            this.MainOutputBox.Name = "MainOutputBox";
            this.MainOutputBox.ReadOnly = true;
            this.MainOutputBox.Size = new System.Drawing.Size(366, 424);
            this.MainOutputBox.TabIndex = 0;
            this.MainOutputBox.Text = "";
            // 
            // FallbackModeOnButton
            // 
            this.FallbackModeOnButton.Location = new System.Drawing.Point(384, 12);
            this.FallbackModeOnButton.Name = "FallbackModeOnButton";
            this.FallbackModeOnButton.Size = new System.Drawing.Size(152, 67);
            this.FallbackModeOnButton.TabIndex = 1;
            this.FallbackModeOnButton.Text = "Fallback Mode ON";
            this.FallbackModeOnButton.UseVisualStyleBackColor = true;
            this.FallbackModeOnButton.Click += new System.EventHandler(this.FallbackModeOnButton_Click);
            // 
            // FallbackModeOffButton
            // 
            this.FallbackModeOffButton.Location = new System.Drawing.Point(384, 85);
            this.FallbackModeOffButton.Name = "FallbackModeOffButton";
            this.FallbackModeOffButton.Size = new System.Drawing.Size(152, 61);
            this.FallbackModeOffButton.TabIndex = 2;
            this.FallbackModeOffButton.Text = "Fallback Mode OFF";
            this.FallbackModeOffButton.UseVisualStyleBackColor = true;
            this.FallbackModeOffButton.Click += new System.EventHandler(this.FallbackModeOffButton_Click);
            // 
            // ExportAttendanceButton
            // 
            this.ExportAttendanceButton.Location = new System.Drawing.Point(384, 152);
            this.ExportAttendanceButton.Name = "ExportAttendanceButton";
            this.ExportAttendanceButton.Size = new System.Drawing.Size(152, 61);
            this.ExportAttendanceButton.TabIndex = 3;
            this.ExportAttendanceButton.Text = "Export Attendance";
            this.ExportAttendanceButton.UseVisualStyleBackColor = true;
            this.ExportAttendanceButton.Click += new System.EventHandler(this.ExportAttendanceButton_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(806, 448);
            this.Controls.Add(this.ExportAttendanceButton);
            this.Controls.Add(this.FallbackModeOffButton);
            this.Controls.Add(this.FallbackModeOnButton);
            this.Controls.Add(this.MainOutputBox);
            this.Name = "MainForm";
            this.Text = "MainForm";
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.RichTextBox MainOutputBox;
        private System.Windows.Forms.Button FallbackModeOnButton;
        private System.Windows.Forms.Button FallbackModeOffButton;
        private System.Windows.Forms.Button ExportAttendanceButton;
    }
}