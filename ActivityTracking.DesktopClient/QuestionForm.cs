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
        List<string> reasonsNames;
        int MayAbsentMinutes;
        Timer formAppearanceTimer;
        Timer formStartTimer;
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
        public Timer FormAppearanceTimer
        {
            get { return formAppearanceTimer; }
        }

        #region QuestionForm() Constructor
        public QuestionForm()
        {
            MayAbsentMinutes = 5;
            reasonsNames = new List<string>() { "Meeting", "English", "Other" };
            InitializeComponent();
            InitializeHooks();
            InitializeTimers();
            TopMost = true;          
            ChecktMayAbsentMinutes();
            CheckDepartmentReasonsChanging();
        }
        #endregion

        #region InitializeTimer
        private void InitializeTimers()
        {
            formAppearanceTimer = new Timer();
            //TODO *60 - minutes
            formAppearanceTimer.Interval = MayAbsentMinutes * 1000; //*60;      
            formAppearanceTimer.Tick += FormAppearanceTimer_Tick;
            formAppearanceTimer.Start();

            formStartTimer = new Timer();
            formStartTimer.Interval = 3000;
            formStartTimer.Tick += FormStartTimer_Tick;
            formStartTimer.Start();
        }
        #endregion

        #region InitializeHooks
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
        #endregion

        #region HookMouseActivity
        private void HookMouseActivity(MouseHook.MSLLHOOKSTRUCT mouseStruc)
        {
            formAppearanceTimer.Stop();
            formAppearanceTimer.Start();
        }
        #endregion'

        #region HookKeyDown
        private void HookKeyDown(KeyboardHook.VKeys key)
        {
            formAppearanceTimer.Stop();
            formAppearanceTimer.Start();
        }
        #endregion

        #region FormAppearanceTimer_Tick
        private void FormAppearanceTimer_Tick(object sender, EventArgs e)
        {
            //TODO
            //Проверка в здании ли пользователь 
            
            formAppearanceTimer.Stop();            
            keyboardHook.Uninstall();
            mouseHook.Uninstall();
            PostAbsence();
            ShowForm();
        }
        #endregion

        private void FormStartTimer_Tick(object sender, EventArgs e)
        {
            formStartTimer.Stop();
            formStartTimer.Dispose();
            this.Hide();
        }

            #region PostAbsence()
            async void PostAbsence()
        {
            PostModel postModel = new PostModel { StartAbsence = DateTime.Now, Date = DateTime.Today, UserName = "AlexandrTkachuk" };
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:14110/");
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                HttpResponseMessage response = await client.PostAsJsonAsync("api/Desktop/CreateAbsence", postModel);
            }
        }
        #endregion

        #region PutAbsence()
        async void PutAbsence(string reasonName, string comment)
        {
            PutModel putModel = new PutModel { EndAbsence = DateTime.Now, UserName = "AlexandrTkachuk", ReasonName = reasonName, Comment = comment };
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:14110/");
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                HttpResponseMessage response = await client.PutAsJsonAsync("api/Desktop/UpdateAbsence", putModel);
            }

        }
        #endregion

        //TODO DeLETE!
        private void notifyIcon1_Click(object sender, EventArgs e)
        {
            this.Show();
            //this.WindowState = FormWindowState.Normal;
            //this.ShowInTaskbar = true;
        }

        #region ReasonButton_Click
        private void ReasonButton_Click(object sender, EventArgs e)
        {
            string reasonName = ((Button)sender).Text;
            PutAbsence(reasonName, null);
            this.Hide();
            formAppearanceTimer.Start();
            keyboardHook.Install();
            mouseHook.Install();
        }
        #endregion

        #region OtherButton_Click
        private void OtherButton_Click(object sender, EventArgs e)
        {
            this.Hide();
            CommentForm comentForm = new CommentForm(this);
            comentForm.Show();

        }
        #endregion

        #region ShowForm
        private void ShowForm()
        {
            ChecktMayAbsentMinutes();
            CheckDepartmentReasonsChanging();
            this.Controls.Clear();
            CreateButtonsAndLabel();
            this.Show();
        }
        #endregion

        #region CheckDepartmentReasonsChanging
        private async void CheckDepartmentReasonsChanging()
        {
            List<string> requestReasonsNames = null;
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:14110/");
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                string a = "AlexandrTkachuk";
                HttpResponseMessage response = await client.GetAsync("api/Desktop/GetReasonsNames/" + a);
                if (response.IsSuccessStatusCode)
                {
                    requestReasonsNames = await response.Content.ReadAsAsync<List<string>>();
                    if (!CompareReasonsNames(reasonsNames, requestReasonsNames))
                    {
                        reasonsNames = requestReasonsNames;                      
                    }

                }
            }
        }
        #endregion

        #region CreateButtonsAndLabel
        private void CreateButtonsAndLabel()
        {
            this.Height = 120;
            Label label = new Label();
            label.Size = new Size(227, 20);
            label.Location = new Point(12, 19);
            label.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            label.Text = "Choose the reason of abence: ";
            this.Controls.Add(label);

            foreach (var reasonName in reasonsNames)
            {
                this.Height += 39;
                Button reasonButton = new Button();
                reasonButton.Text = reasonName;
                reasonButton.Size = new Size(352, 33);
                reasonButton.Location = new Point(12, this.Height - 92);
                if (reasonName == "Other")
                {
                    reasonButton.Click += new System.EventHandler(OtherButton_Click);
                }
                else
                {
                    reasonButton.Click += new System.EventHandler(ReasonButton_Click);                  
                }
                reasonButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
                this.Controls.Add(reasonButton);
            }
        }
        #endregion

        #region CompareReasonsNames
        bool CompareReasonsNames(List<string> oldReasonsNames, List<string> newReasonsNames)
        {
            if (oldReasonsNames.Count == newReasonsNames.Count)
            {
                var oldReasonsNamesToarray = oldReasonsNames.ToArray();
                var newReasonsNamesToarray = newReasonsNames.ToArray();
                for (int i = 0; i < oldReasonsNamesToarray.Length; i++)
                {
                    if (oldReasonsNamesToarray[i] != newReasonsNamesToarray[i])
                    {
                        return false;
                    }
                }
                return true;
            }
            else
            {
                return false;
            }
        }
        #endregion

        #region ChecktMayAbsentMinutes
        private async void ChecktMayAbsentMinutes()
        {

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:14110/");
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

               string userName = "AlexandrTkachuk";
                HttpResponseMessage response = await client.GetAsync("api/Desktop/GetMayAbsentMinutes/" + userName);
                if (response.IsSuccessStatusCode)
                {
                    MayAbsentMinutes = await response.Content.ReadAsAsync<int>();
                    formAppearanceTimer.Interval = MayAbsentMinutes * 1000;
                }               
            }

        }
        #endregion

        #region QuestionForm_Load
        private void QuestionForm_Load(object sender, EventArgs e)
        {
            notifyIcon1.Icon = Properties.Resources.lines;
        }
        #endregion

    }
}
