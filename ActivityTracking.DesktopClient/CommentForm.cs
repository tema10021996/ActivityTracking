using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net.Http;
using System.Net.Http.Headers;


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
            PutAbsence("Other", commentTextBox.Text);
            questionForm.FormAppearanceTimer.Start();
            this.Close();
            questionForm.KeyboardHook.Install();
            questionForm.MouseHook.Install();
        }

        async void PutAbsence(string reasonName, string comment)
        {
            PutModel putModel = new PutModel { EndAbsence = DateTime.Now, UserName = Environment.UserName, ReasonName = reasonName, Comment = comment };
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:14110/");
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                HttpResponseMessage response = await client.PutAsJsonAsync("api/Desktop/UpdateAbsence", putModel);
            }

        }
    }
}
