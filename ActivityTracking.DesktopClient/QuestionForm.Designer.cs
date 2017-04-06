namespace ActivityTracking.DesktopClient
{
    partial class QuestionForm
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
            this.components = new System.ComponentModel.Container();
            this.label1 = new System.Windows.Forms.Label();
            this.MeetingButton = new System.Windows.Forms.Button();
            this.ConsultationButton = new System.Windows.Forms.Button();
            this.EnglishButton = new System.Windows.Forms.Button();
            this.WithoutPCButton = new System.Windows.Forms.Button();
            this.OtherButton = new System.Windows.Forms.Button();
            this.notifyIcon1 = new System.Windows.Forms.NotifyIcon(this.components);
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label1.Location = new System.Drawing.Point(12, 19);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(227, 20);
            this.label1.TabIndex = 0;
            this.label1.Text = "Choose the reason of abence: ";
            // 
            // MeetingButton
            // 
            this.MeetingButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.MeetingButton.Location = new System.Drawing.Point(12, 46);
            this.MeetingButton.Name = "MeetingButton";
            this.MeetingButton.Size = new System.Drawing.Size(352, 33);
            this.MeetingButton.TabIndex = 1;
            this.MeetingButton.Text = "Meeting";
            this.MeetingButton.UseVisualStyleBackColor = true;
            this.MeetingButton.Click += new System.EventHandler(this.ReasonButton_Click);
            // 
            // ConsultationButton
            // 
            this.ConsultationButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.ConsultationButton.Location = new System.Drawing.Point(12, 85);
            this.ConsultationButton.Name = "ConsultationButton";
            this.ConsultationButton.Size = new System.Drawing.Size(352, 33);
            this.ConsultationButton.TabIndex = 2;
            this.ConsultationButton.Text = "Consultation";
            this.ConsultationButton.UseVisualStyleBackColor = true;
            this.ConsultationButton.Click += new System.EventHandler(this.ReasonButton_Click);
            // 
            // EnglishButton
            // 
            this.EnglishButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.EnglishButton.Location = new System.Drawing.Point(12, 124);
            this.EnglishButton.Name = "EnglishButton";
            this.EnglishButton.Size = new System.Drawing.Size(352, 33);
            this.EnglishButton.TabIndex = 3;
            this.EnglishButton.Text = "English";
            this.EnglishButton.UseVisualStyleBackColor = true;
            this.EnglishButton.Click += new System.EventHandler(this.ReasonButton_Click);
            // 
            // WithoutPCButton
            // 
            this.WithoutPCButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.WithoutPCButton.Location = new System.Drawing.Point(12, 163);
            this.WithoutPCButton.Name = "WithoutPCButton";
            this.WithoutPCButton.Size = new System.Drawing.Size(352, 33);
            this.WithoutPCButton.TabIndex = 4;
            this.WithoutPCButton.Text = "Worked without PC";
            this.WithoutPCButton.UseVisualStyleBackColor = true;
            this.WithoutPCButton.Click += new System.EventHandler(this.ReasonButton_Click);
            // 
            // OtherButton
            // 
            this.OtherButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.OtherButton.Location = new System.Drawing.Point(12, 202);
            this.OtherButton.Name = "OtherButton";
            this.OtherButton.Size = new System.Drawing.Size(352, 33);
            this.OtherButton.TabIndex = 5;
            this.OtherButton.Text = "Other";
            this.OtherButton.UseVisualStyleBackColor = true;
            this.OtherButton.Click += new System.EventHandler(this.OtherButton_Click);
            // 
            // notifyIcon1
            // 
            this.notifyIcon1.Text = "notifyIcon1";
            this.notifyIcon1.Visible = true;
            this.notifyIcon1.Click += new System.EventHandler(this.notifyIcon1_Click);
            // 
            // QuestionForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(378, 261);
            this.Controls.Add(this.OtherButton);
            this.Controls.Add(this.WithoutPCButton);
            this.Controls.Add(this.EnglishButton);
            this.Controls.Add(this.ConsultationButton);
            this.Controls.Add(this.MeetingButton);
            this.Controls.Add(this.label1);
            this.Name = "QuestionForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "QuestionForm";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button MeetingButton;
        private System.Windows.Forms.Button ConsultationButton;
        private System.Windows.Forms.Button EnglishButton;
        private System.Windows.Forms.Button WithoutPCButton;
        private System.Windows.Forms.Button OtherButton;
        private System.Windows.Forms.NotifyIcon notifyIcon1;
    }
}

