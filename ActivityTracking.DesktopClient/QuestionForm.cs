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
    public partial class QuestionForm : Form
    {
        Timer timer;
        UserActivityHook hook;
        public Timer Timer
        {
            get { return timer; }
        }

        public QuestionForm()
        {
            InitializeComponent();
            TopMost = true;
            hook = new UserActivityHook();
            hook.KeyDown += HookKeyDown;
            hook.OnMouseActivity += HookMouseActivity;

            timer = new Timer();
            timer.Interval = 5000;
            timer.Start();
            timer.Tick += timerTick;
        }

        private void HookMouseActivity(object sender, MouseEventArgs e)
        {
            timer.Stop();
            timer.Start();
        }

        private void HookKeyDown(object sender, KeyEventArgs e)
        {
            timer.Stop();
            timer.Start();
        }

        private void timerTick(object sender, EventArgs e)
        {
            ShowForm();
            timer.Stop();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            notifyIcon1.Icon = Properties.Resources.lines;
            HideForm();
        }
         

        private void notifyIcon1_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Normal;
            this.ShowInTaskbar = true;
        }  

         private void ReasonButton_Click(object sender, EventArgs e)
        {
            HideForm();
            timer.Start();
        }

        private void ShowForm()
        {
            this.WindowState = FormWindowState.Normal;
            this.ShowInTaskbar = true;
        }
        private void HideForm()
        {
            this.WindowState = FormWindowState.Minimized;
            this.ShowInTaskbar = false;

        }

        private void OtherButton_Click(object sender, EventArgs e)
        {
            timer.Stop();
            HideForm();
            CommentForm comentForm = new CommentForm(this);
            comentForm.Show();
        }
    }
}
