using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ActivityTracking.DomainModel;
using ActivityTracking.DAL.EntityFramework;

namespace ActivityTracking.DesktopClient
{
    public partial class CommentForm : Form
    {
        QuestionForm questionForm;
        ApplicationUser user;
        DAL.EntityFramework.ApplicationContext context;

        public CommentForm(QuestionForm questionForm, ApplicationUser user, DAL.EntityFramework.ApplicationContext context)
        {
            TopMost = true;
            InitializeComponent();
            this.questionForm = questionForm;
            this.user = user;
            this.context = context;
        }

        private void OkButton_Click(object sender, EventArgs e)
        {
            questionForm.Timer.Start();
            this.Close();
        }
    }
}
