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

        public KeyboardHook KeyboardHook
        {
            get { return keyboardHook; }
        }

        public MouseHook MouseHook
        {
            get { return mouseHook; }
        }

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
            timer.Interval = 10000; //user.Group.MayAbsentTime.Hours * 216000000 + user.Group.MayAbsentTime.Minutes * 60000 + user.Group.MayAbsentTime.Seconds * 1000;
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


        //--------------------
        //this.MeetingButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
        //    this.MeetingButton.Location = new System.Drawing.Point(12, 46);
        //    this.MeetingButton.Name = "MeetingButton";
        //    this.MeetingButton.Size = new System.Drawing.Size(352, 33);
        //    this.MeetingButton.TabIndex = 1;
        //    this.MeetingButton.Text = "Meeting";
        //    this.MeetingButton.UseVisualStyleBackColor = true;
        //    this.MeetingButton.Click += new System.EventHandler(this.ReasonButton_Click);
        //=========================


        private async void timerTick(object sender, EventArgs e)
        {
            //TODO
            //Проверка в здании ли пользователь 

            timer.Stop();
            keyboardHook.Uninstall();
            mouseHook.Uninstall();


            PostModel nn = new PostModel { Start = DateTime.Now, Date = DateTime.Today, UserName = "AlexandrTkachuk" };
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:14110/");
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                this.Width += 100;
                HttpResponseMessage response = await client.PostAsJsonAsync("api/Desktop", nn);

                if (response.IsSuccessStatusCode)
                {

                    bool result = await response.Content.ReadAsAsync<bool>();
                    if (result)
                    {
                        this.ForeColor = Color.AliceBlue;
                    }
                    else
                        this.ForeColor = Color.Brown;
                }
            }

            ShowForm();
            //timer.Stop();
        }

        //static async Task GetRequest(string ID)
        //{
        //    using (var client = new HttpClient())
        //    {
        //        client.BaseAddress = new Uri("http://localhost:14110/");
        //        client.DefaultRequestHeaders.Accept.Clear();
        //        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

        //        HttpResponseMessage response;

        //        string id = "aaa";


        //        response = await client.GetAsync("api/Desktop/" + id);
        //        if (response.IsSuccessStatusCode)
        //        {

        //        }

        //    }
        //}
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
            keyboardHook.Install();
            mouseHook.Install();
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