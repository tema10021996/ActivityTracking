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
    public partial class QuestionForm : Form
    {
        KeyboardHook keyboardHook;
        MouseHook mouseHook;
        ApplicationUser user;
        DAL.EntityFramework.ApplicationContext context;

        Timer timer;
        public Timer Timer
        {
            get { return timer; }
        }

        public QuestionForm()
        {
            InitializeComponent();
            TopMost = true;
            context = new DAL.EntityFramework.ApplicationContext();

            InitializeUser();

            InitializeHooks();

            InitializeTimer();
           
        }


        void InitializeUser()
        {

            Repository <ApplicationUser> usersRepository = new Repository<ApplicationUser>(context);
            user = usersRepository.GetList().First(u =>u.UserName == Environment.UserName);
        }
        private void InitializeTimer()
        {
            timer = new Timer();
            timer.Interval = user.Group.MayAbsentTime.Hours * 216000000 + user.Group.MayAbsentTime.Minutes * 60000 + user.Group.MayAbsentTime.Seconds * 1000;
            timer.Start();
            timer.Tick += timerTick; ;
        }



        private void InitializeHooks()
        {
            keyboardHook = new KeyboardHook();
            keyboardHook.KeyDown += new KeyboardHook.KeyboardHookCallback(HookKeyDown);
            keyboardHook.Install();

            mouseHook = new MouseHook();
            mouseHook.MouseMove += new MouseHook.MouseHookCallback(HookMouseActivity);
            mouseHook.MouseWheel += new MouseHook.MouseHookCallback(HookMouseActivity);
            mouseHook.LeftButtonDown += new MouseHook.MouseHookCallback(HookMouseActivity);
            mouseHook.MiddleButtonDown += new MouseHook.MouseHookCallback(HookMouseActivity);
            mouseHook.RightButtonDown += new MouseHook.MouseHookCallback(HookMouseActivity);
            mouseHook.Install(); 
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
            //TODO
            //Проверка в здании ли пользователь 

            Repository<Absenсe> absenseRepository = new Repository<Absenсe>(context);
            absenseRepository.Create(new Absenсe { StartAbsence = DateTime.Now, User = user, Date = DateTime.Today });

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
            if (((Button)sender).Text == "Meeting")
            {
                Repository<Reason> reasonsRepository = new Repository<Reason>(context);
                Reason reason = reasonsRepository.GetList().First(r => r.Name == "Meeting");
                Repository<Absenсe> absenseRepository = new Repository<Absenсe>(context);
                Absenсe absence = absenseRepository.GetList().Last(a => a.User.UserName == user.UserName);
                absence.Reason = reason;
                absence.EndAbsence = DateTime.Now;
                absenseRepository.Update(absence);
            }

            HideForm();
            timer.Start();
        }

        
        private void OtherButton_Click(object sender, EventArgs e)
        {
            timer.Stop();
            HideForm();
            CommentForm comentForm = new CommentForm(this, user, context);
            comentForm.Show();
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

        
        
    }
}
