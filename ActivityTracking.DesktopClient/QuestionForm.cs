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
        KeyboardHook keyboardHook;
        MouseHook mouseHook;

        Timer timer;
        public Timer Timer
        {
            get { return timer; }
        }

        public QuestionForm()
        {
            InitializeComponent();
            TopMost = true;

            keyboardHook = new KeyboardHook();
            keyboardHook.KeyDown += new KeyboardHook.KeyboardHookCallback(HookKeyDown);
            keyboardHook.Install();

            mouseHook = new MouseHook();
            mouseHook.MouseMove += new MouseHook.MouseHookCallback(HookMouseActivity);
            mouseHook.MouseWheel += new MouseHook.MouseHookCallback(HookMouseActivity);
            mouseHook.LeftButtonDown += new MouseHook.MouseHookCallback(HookMouseActivity);
            mouseHook.MiddleButtonDown += new MouseHook.MouseHookCallback(HookMouseActivity);
            mouseHook.RightButtonDown += new MouseHook.MouseHookCallback(HookMouseActivity);
            mouseHook.Install(); ;

            timer = new Timer();
            timer.Interval = 5000;
            timer.Start();
            timer.Tick += timerTick;
        }

        private void HookMouseActivity(MouseHook.MSLLHOOKSTRUCT mouseStruc)
        {
            timer.Stop();
            timer.Start();
        }

        private void HookKeyDown(KeyboardHook.VKeys key)
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
