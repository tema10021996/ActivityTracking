using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace ActivityTracking.DesktopClient
{
    public partial class CommentForm : Form
    {
        QuestionForm questionForm;

        public CommentForm(QuestionForm questionForm)
        {
            TopMost = true;
            InitializeComponent();
            this.questionForm = questionForm;          
        }

        private void OkButton_Click(object sender, EventArgs e)
        {
            questionForm.Timer.Start();
            this.Close();
            questionForm.KeyboardHook.Install();
            questionForm.MouseHook.Install();
        }
    }
}
