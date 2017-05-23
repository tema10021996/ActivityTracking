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

            InitializeUser();

            InitializeHooks();

            InitializeTimer();
           
        }


        void InitializeUser()
        {

            //Repository <ApplicationUser> usersRepository = new Repository<ApplicationUser>(context);
            //user = usersRepository.GetList().First(u =>u.UserName == Environment.UserName);
        }
        private void InitializeTimer()
        {
            timer = new Timer();
            timer.Interval = 1000; //user.Group.MayAbsentTime.Hours * 216000000 + user.Group.MayAbsentTime.Minutes * 60000 + user.Group.MayAbsentTime.Seconds * 1000;
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

        private async void timerTick(object sender, EventArgs e)
        {
            //TODO
            //Проверка в здании ли пользователь 


            //using (var client = new HttpClient())
            //{
            //    PostModel postModel = new PostModel { Start = DateTime.Now, UserName = Environment.UserName, Date = DateTime.Now.Date };

            //    client.BaseAddress = new Uri("http://localhost:14110/");
            //    client.DefaultRequestHeaders.Accept.Clear();
            //    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            //    HttpResponseMessage response = await client.PostAsJsonAsync("api/Desktop", postModel);

            //    if (response.IsSuccessStatusCode)
            //    {
            //        this.ForeColor = Color.AliceBlue;
            //    }
            //}

            GetRequest("aaa");
            

            ShowForm();
            timer.Stop();
        }

        static async Task GetRequest(string ID)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:14110/");
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                HttpResponseMessage response;

                string id = "aaa";

                response = await client.GetAsync("api/Desktop/" + id);
                if (response.IsSuccessStatusCode)
                {
                   
                }

            }
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
            
        //        Repository<Reason> reasonsRepository = new Repository<Reason>(context);
        //        Reason reason = reasonsRepository.GetList().First(r => r.Name == "Meeting");
        //        Repository<Absence> absenseRepository = new Repository<Absence>(context);
        //        Absence absence = absenseRepository.GetList().Last(a => a.User.UserName == user.UserName);
        //        absence.Reason = reason;
        //        absence.EndAbsence = DateTime.Now;
        //        absenseRepository.Update(absence);
        //    }

        //    HideForm();
        //    timer.Start();
        }

        
        private void OtherButton_Click(object sender, EventArgs e)
        {
        //    timer.Stop();
        //    HideForm();
        //    CommentForm comentForm = new CommentForm(this, user, context);
        //    comentForm.Show();
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
