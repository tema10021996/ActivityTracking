using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Data.Entity;
using ActivityTracking.DAL.EntityFramework;
using ActivityTracking.DomainModel;
using System;
using System.Linq;

namespace ActivityTracking.WebClient.Models
{
    public class DbInitializer :  DropCreateDatabaseAlways<ApplicationContext>
    {
        protected override void Seed(ApplicationContext context)
        {
            var userManager = new ApplicationUserManager(new UserStore<ApplicationUser>(context));

            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));
            #region Create roles
            // Create roles
            var roleAdmin = new IdentityRole { Id = "1", Name = "admin" };
            var roleUser = new IdentityRole { Id = "2", Name = "user" };
            var rolemanager = new IdentityRole { Id = "3", Name = "manager" };
            #endregion

            #region Add roles to BD
            roleManager.Create(roleAdmin);
            roleManager.Create(roleUser);
            roleManager.Create(rolemanager);
            #endregion

            #region Create users
            // Create Admin
            var admin = new ApplicationUser { UserName = "Admin" };
            string adminPassword = "123456";
            userManager.Create(admin, adminPassword);
            userManager.AddToRole(admin.Id, roleAdmin.Name);

            //Create User NikitaMaltsev
            var NikitaMaltsev = new ApplicationUser { UserName = "NikitaMaltsev" };
            string NikitaMaltsevPassword = "123456";
            userManager.Create(NikitaMaltsev, NikitaMaltsevPassword);
            userManager.AddToRole(NikitaMaltsev.Id, roleUser.Name);

            //Create User AlexandrTkachuk
            var AlexandrTkachuk = new ApplicationUser { UserName = "AlexandrTkachuk" };
            string AlexandrTkachukPassword = "123456";
            userManager.Create(AlexandrTkachuk, AlexandrTkachukPassword);
            userManager.AddToRole(AlexandrTkachuk.Id, roleUser.Name);

            //Create User ArtemChuhalo
            var ArtemChuhalo = new ApplicationUser { UserName = "ArtemChuhalo" };
            string ArtemChuhaloPassword = "123456";
            userManager.Create(ArtemChuhalo, ArtemChuhaloPassword);
            userManager.AddToRole(ArtemChuhalo.Id, roleUser.Name);


            var IvanIvanov = new ApplicationUser { UserName = "IvanIvanov" };
            string IvanIvanovPassword = "123456";
            userManager.Create(IvanIvanov, IvanIvanovPassword);
            userManager.AddToRole(IvanIvanov.Id, roleUser.Name);

            var MaxMaximov = new ApplicationUser { UserName = "MaxMaximov" };
            string MaxMaximovPassword = "123456";
            userManager.Create(MaxMaximov, MaxMaximovPassword);
            userManager.AddToRole(MaxMaximov.Id, roleUser.Name);

            //Create manager AlexandraMorozova
            var AlexandraMorozova = new ApplicationUser { UserName = "AlexandraMorozova" };
            string AlexandraMorozovaPassword = "123456";
            userManager.Create(AlexandraMorozova, AlexandraMorozovaPassword);
            userManager.AddToRole(AlexandraMorozova.Id, rolemanager.Name);
            #endregion            
           
            #region Add reasons
            //Add Reasons
            Repository<Reason> reasonRepository = new Repository<Reason>(context);
            reasonRepository.Create(new Reason { Name = "Meeting", AddingTime = DateTime.Now, Color = "#FF0000" });
            reasonRepository.Create(new Reason { Name = "Consultation", AddingTime = DateTime.Now, Color = "#FFFF00" });
            reasonRepository.Create(new Reason { Name = "English", AddingTime = DateTime.Now, Color = "#008000" });
            reasonRepository.Create(new Reason { Name = "Other", AddingTime = DateTime.Now, Color = "#808080" });
            #endregion

            #region Add Groups
            ////Add Groups
            //Repository<Group> groupRepository = new Repository<Group>(context);
            //groupRepository.Create(new Group { Name = "Group1", MayAbsentTime = new TimeSpan(00,01,00)});
            //groupRepository.Create(new Group { Name = "Group2", MayAbsentTime = new TimeSpan(00, 02, 00) });
            //groupRepository.Create(new Group { Name = "Group3", MayAbsentTime = new TimeSpan(00, 03, 00) });
            //groupRepository.Create(new Group { Name = "Group4", MayAbsentTime = new TimeSpan(00, 04, 00) });
            #endregion 

            #region Set reasons to groups
            //Set reasons to groups
            //Group group1 = groupRepository.GetList().First(g => g.Name == "Group1");
            //group1.Reasons.Add(reasonRepository.GetList().First(r => r.Name == "Meeting"));
            //group1.Reasons.Add(reasonRepository.GetList().First(r => r.Name == "Consultation"));
            //group1.Reasons.Add(reasonRepository.GetList().First(r => r.Name == "English"));
            //group1.Reasons.Add(reasonRepository.GetList().First(r => r.Name == "Other"));
            //groupRepository.Update(group1);

            //Group group2 = groupRepository.GetList().First(g => g.Name == "Group2");
            //group2.Reasons.Add(reasonRepository.GetList().First(r => r.Name == "Meeting"));
            //group2.Reasons.Add(reasonRepository.GetList().First(r => r.Name == "Consultation"));
            //group2.Reasons.Add(reasonRepository.GetList().First(r => r.Name == "English"));
            //group2.Reasons.Add(reasonRepository.GetList().First(r => r.Name == "Other"));
            //groupRepository.Update(group2);

            //Group group3 = groupRepository.GetList().First(g => g.Name == "Group3");
            //group3.Reasons.Add(reasonRepository.GetList().First(r => r.Name == "Meeting"));
            //group3.Reasons.Add(reasonRepository.GetList().First(r => r.Name == "Consultation"));
            //group3.Reasons.Add(reasonRepository.GetList().First(r => r.Name == "English"));
            //group3.Reasons.Add(reasonRepository.GetList().First(r => r.Name == "Other"));
            //groupRepository.Update(group3);

            //Group group4 = groupRepository.GetList().First(g => g.Name == "Group4");
            //group4.Reasons.Add(reasonRepository.GetList().First(r => r.Name == "Meeting"));
            //group4.Reasons.Add(reasonRepository.GetList().First(r => r.Name == "Consultation"));
            //group4.Reasons.Add(reasonRepository.GetList().First(r => r.Name == "English"));
            //group4.Reasons.Add(reasonRepository.GetList().First(r => r.Name == "Other"));
            //groupRepository.Update(group4);

            #endregion

            #region Add users to Group1

            //Repository<ApplicationUser> applicationUserRepository = new Repository<ApplicationUser>(context);
            //var ggroup1 = groupRepository.GetList().First(g => g.Name == "Group1");
            //ggroup1.Users.Add(applicationUserRepository.GetList().First(u=>u.UserName == "NikitaMaltsev"));
            //ggroup1.Users.Add(applicationUserRepository.GetList().First(u => u.UserName == "AlexandrTkachuk"));
            //ggroup1.Users.Add(applicationUserRepository.GetList().First(u => u.UserName == "ArtemChuhalo"));
            //ggroup1.Users.Add(applicationUserRepository.GetList().First(u => u.UserName == "AlexandraMorozova"));
            //ggroup1.Users.Add(applicationUserRepository.GetList().First(u => u.UserName == "IvanIvanov"));
            //ggroup1.Users.Add(applicationUserRepository.GetList().First(u => u.UserName == "MaxMaximov"));
            //groupRepository.Update(ggroup1);
            #endregion

            #region Add Absences
            Repository<ApplicationUser> applicationUserRepository = new Repository<ApplicationUser>(context);
            //Reasons
            var reasonMeeting = reasonRepository.GetList().First(r => r.Name == "Meeting");
            var reasonConsultation = reasonRepository.GetList().First(r => r.Name == "Consultation");
            var reasonEnglish = reasonRepository.GetList().First(r => r.Name == "English");
            var reasonOther = reasonRepository.GetList().First(r => r.Name == "Other");
            //users
            var userAlexandrTkachuk = applicationUserRepository.GetList().First(u => u.UserName == "AlexandrTkachuk");
            var userNikitaMaltsev = applicationUserRepository.GetList().First(u => u.UserName == "NikitaMaltsev");
            var userArtemChuhalo = applicationUserRepository.GetList().First(u => u.UserName == "ArtemChuhalo");
            var userAlexandraMorozova = applicationUserRepository.GetList().First(u => u.UserName == "AlexandraMorozova");
            var userIvanIvanov = applicationUserRepository.GetList().First(u => u.UserName == "IvanIvanov");
            var userMaxMaximov = applicationUserRepository.GetList().First(u => u.UserName == "MaxMaximov");

            #region Add Absences and to user Alexandr


            //Add Absences for AlexandrTkachuk
            Repository<Absence> absenceRepository = new Repository<Absence>(context);

            //for 21 April
            absenceRepository.Create(new Absence { StartAbsence = new DateTime(2017, 4, 21, 9, 10, 0), EndAbsence = new DateTime(2017, 4, 21, 9, 40, 0), Date = new DateTime(2017, 4, 21), Reason = reasonConsultation, User = userAlexandrTkachuk });
            absenceRepository.Create(new Absence { StartAbsence = new DateTime(2017, 4, 21, 11, 0, 0), EndAbsence = new DateTime(2017, 4, 21, 11, 35, 0), Date = new DateTime(2017, 4, 21), Reason = reasonMeeting, User = userAlexandrTkachuk });
            absenceRepository.Create(new Absence { StartAbsence = new DateTime(2017, 4, 21, 13, 0, 0), EndAbsence = new DateTime(2017, 4, 21, 14, 20, 0), Date = new DateTime(2017, 4, 21), Reason = reasonEnglish, User = userAlexandrTkachuk });
            absenceRepository.Create(new Absence { StartAbsence = new DateTime(2017, 4, 21, 15, 0, 0), EndAbsence = new DateTime(2017, 4, 21, 15, 20, 0), Date = new DateTime(2017, 4, 21), Reason = reasonConsultation, User = userAlexandrTkachuk });

            //for 22 April
            absenceRepository.Create(new Absence { StartAbsence = new DateTime(2017, 4, 22, 9, 20, 0), EndAbsence = new DateTime(2017, 4, 22, 9, 30, 0), Date = new DateTime(2017, 4, 22), Reason = reasonMeeting, User = userAlexandrTkachuk });
            absenceRepository.Create(new Absence { StartAbsence = new DateTime(2017, 4, 22, 10, 50, 0), EndAbsence = new DateTime(2017, 4, 22, 11, 5, 0), Date = new DateTime(2017, 4, 22), Reason = reasonConsultation, User = userAlexandrTkachuk });
            absenceRepository.Create(new Absence { StartAbsence = new DateTime(2017, 4, 22, 12, 0, 0), EndAbsence = new DateTime(2017, 4, 22, 12, 20, 0), Date = new DateTime(2017, 4, 22), Reason = reasonConsultation, User = userAlexandrTkachuk });
            absenceRepository.Create(new Absence { StartAbsence = new DateTime(2017, 4, 22, 15, 0, 0), EndAbsence = new DateTime(2017, 4, 22, 15, 50, 0), Date = new DateTime(2017, 4, 22), Reason = reasonOther, User = userAlexandrTkachuk, Comment = "It was а queue at the toilet" });

            //for 23 April
            absenceRepository.Create(new Absence { StartAbsence = new DateTime(2017, 4, 23, 9, 40, 0), EndAbsence = new DateTime(2017, 4, 23, 9, 55, 0), Date = new DateTime(2017, 4, 23), Reason = reasonEnglish, User = userAlexandrTkachuk });
            absenceRepository.Create(new Absence { StartAbsence = new DateTime(2017, 4, 23, 11, 25, 0), EndAbsence = new DateTime(2017, 4, 23, 11, 55, 0), Date = new DateTime(2017, 4, 23), Reason = reasonMeeting, User = userAlexandrTkachuk });
            absenceRepository.Create(new Absence { StartAbsence = new DateTime(2017, 4, 23, 15, 5, 0), EndAbsence = new DateTime(2017, 4, 23, 15, 35, 0), Date = new DateTime(2017, 4, 23), Reason = reasonConsultation, User = userAlexandrTkachuk });


            // for 24 march
            absenceRepository.Create(new Absence { StartAbsence = new DateTime(2017, 4, 24, 9, 50, 0), EndAbsence = new DateTime(2017, 4, 24, 10, 20, 0), Date = new DateTime(2017, 4, 24), Reason = reasonMeeting, User = userAlexandrTkachuk });
            absenceRepository.Create(new Absence { StartAbsence = new DateTime(2017, 4, 24, 10, 50, 0), EndAbsence = new DateTime(2017, 4, 24, 11, 20, 0), Date = new DateTime(2017, 4, 24), Reason = reasonConsultation, User = userAlexandrTkachuk });
            absenceRepository.Create(new Absence { StartAbsence = new DateTime(2017, 4, 24, 15, 0, 0), EndAbsence = new DateTime(2017, 4, 24, 15, 25, 0), Date = new DateTime(2017, 4, 24), Reason = reasonConsultation, User = userAlexandrTkachuk });

            //for 25 march
            absenceRepository.Create(new Absence { StartAbsence = new DateTime(2017, 4, 25, 9, 10, 0), EndAbsence = new DateTime(2017, 4, 25, 9, 40, 0), Date = new DateTime(2017, 4, 25), Reason = reasonConsultation, User = userAlexandrTkachuk });
            absenceRepository.Create(new Absence { StartAbsence = new DateTime(2017, 4, 25, 11, 0, 0), EndAbsence = new DateTime(2017, 4, 25, 11, 35, 0), Date = new DateTime(2017, 4, 25), Reason = reasonMeeting, User = userAlexandrTkachuk });
            absenceRepository.Create(new Absence { StartAbsence = new DateTime(2017, 4, 25, 13, 0, 0), EndAbsence = new DateTime(2017, 4, 25, 14, 20, 0), Date = new DateTime(2017, 4, 25), Reason = reasonEnglish, User = userAlexandrTkachuk });
            absenceRepository.Create(new Absence { StartAbsence = new DateTime(2017, 4, 25, 15, 0, 0), EndAbsence = new DateTime(2017, 4, 25, 15, 20, 0), Date = new DateTime(2017, 4, 25), Reason = reasonConsultation, User = userAlexandrTkachuk });

            //for 26 march
            absenceRepository.Create(new Absence { StartAbsence = new DateTime(2017, 4, 26, 9, 20, 0), EndAbsence = new DateTime(2017, 4, 26, 9, 30, 0), Date = new DateTime(2017, 4, 26), Reason = reasonMeeting, User = userAlexandrTkachuk });
            absenceRepository.Create(new Absence { StartAbsence = new DateTime(2017, 4, 26, 10, 50, 0), EndAbsence = new DateTime(2017, 4, 26, 11, 5, 0), Date = new DateTime(2017, 4, 26), Reason = reasonConsultation, User = userAlexandrTkachuk });
            absenceRepository.Create(new Absence { StartAbsence = new DateTime(2017, 4, 26, 12, 0, 0), EndAbsence = new DateTime(2017, 4, 26, 12, 20, 0), Date = new DateTime(2017, 4, 26), Reason = reasonConsultation, User = userAlexandrTkachuk });
            absenceRepository.Create(new Absence { StartAbsence = new DateTime(2017, 4, 26, 15, 10, 0), EndAbsence = new DateTime(2017, 4, 26, 16, 30, 0), Date = new DateTime(2017, 4, 26), Reason = reasonOther, User = userAlexandrTkachuk, Comment = "It was а queue at the toilet" });

            //for 27 march
            absenceRepository.Create(new Absence { StartAbsence = new DateTime(2017, 4, 27, 9, 0, 0), EndAbsence = new DateTime(2017, 4, 27, 9, 25, 0), Date = new DateTime(2017, 4, 27), Reason = reasonOther, User = userAlexandrTkachuk, Comment = "I don't want to explain(" });
            absenceRepository.Create(new Absence { StartAbsence = new DateTime(2017, 4, 27, 10, 15, 0), EndAbsence = new DateTime(2017, 4, 27, 10, 45, 0), Date = new DateTime(2017, 4, 27), Reason = reasonMeeting, User = userAlexandrTkachuk });
            absenceRepository.Create(new Absence { StartAbsence = new DateTime(2017, 4, 27, 14, 0, 0), EndAbsence = new DateTime(2017, 4, 27, 14, 40, 0), Date = new DateTime(2017, 4, 27), Reason = reasonConsultation, User = userAlexandrTkachuk });

            //for 28 march
            absenceRepository.Create(new Absence { StartAbsence = new DateTime(2017, 4, 28, 9, 40, 0), EndAbsence = new DateTime(2017, 4, 28, 9, 55, 0), Date = new DateTime(2017, 4, 28), Reason = reasonEnglish, User = userAlexandrTkachuk });
            absenceRepository.Create(new Absence { StartAbsence = new DateTime(2017, 4, 28, 11, 25, 0), EndAbsence = new DateTime(2017, 4, 28, 11, 55, 0), Date = new DateTime(2017, 4, 28), Reason = reasonMeeting, User = userAlexandrTkachuk });
            absenceRepository.Create(new Absence { StartAbsence = new DateTime(2017, 4, 28, 15, 5, 0), EndAbsence = new DateTime(2017, 4, 28, 15, 35, 0), Date = new DateTime(2017, 4, 28), Reason = reasonConsultation, User = userAlexandrTkachuk });



            #endregion


            #region Add Absences and for Nikita

            //Add Absences for Nikita

            //for 21 April
            absenceRepository.Create(new Absence { StartAbsence = new DateTime(2017, 4, 21, 9, 40, 0), EndAbsence = new DateTime(2017, 4, 21, 9, 55, 0), Date = new DateTime(2017, 4, 21), Reason = reasonConsultation, User = userNikitaMaltsev });
            absenceRepository.Create(new Absence { StartAbsence = new DateTime(2017, 4, 21, 11, 5, 0), EndAbsence = new DateTime(2017, 4, 21, 11, 55, 0), Date = new DateTime(2017, 4, 21), Reason = reasonMeeting, User = userNikitaMaltsev });
            absenceRepository.Create(new Absence { StartAbsence = new DateTime(2017, 4, 21, 13, 20, 0), EndAbsence = new DateTime(2017, 4, 21, 14, 30, 0), Date = new DateTime(2017, 4, 21), Reason = reasonEnglish, User = userNikitaMaltsev });
            absenceRepository.Create(new Absence { StartAbsence = new DateTime(2017, 4, 21, 16, 0, 0), EndAbsence = new DateTime(2017, 4, 21, 16, 20, 0), Date = new DateTime(2017, 4, 21), Reason = reasonConsultation, User = userNikitaMaltsev });

            //for 22 April
            absenceRepository.Create(new Absence { StartAbsence = new DateTime(2017, 4, 22, 8, 20, 0), EndAbsence = new DateTime(2017, 4, 22, 9, 30, 0), Date = new DateTime(2017, 4, 22), Reason = reasonMeeting, User = userNikitaMaltsev });
            absenceRepository.Create(new Absence { StartAbsence = new DateTime(2017, 4, 22, 10, 50, 0), EndAbsence = new DateTime(2017, 4, 22, 11, 5, 0), Date = new DateTime(2017, 4, 22), Reason = reasonConsultation, User = userNikitaMaltsev });
            absenceRepository.Create(new Absence { StartAbsence = new DateTime(2017, 4, 22, 12, 40, 0), EndAbsence = new DateTime(2017, 4, 22, 13, 10, 0), Date = new DateTime(2017, 4, 22), Reason = reasonConsultation, User = userNikitaMaltsev });
            absenceRepository.Create(new Absence { StartAbsence = new DateTime(2017, 4, 22, 15, 20, 0), EndAbsence = new DateTime(2017, 4, 22, 15, 30, 0), Date = new DateTime(2017, 4, 22), Reason = reasonOther, User = userNikitaMaltsev, Comment = "It was а queue at the toilet" });

            //for 23 April
            absenceRepository.Create(new Absence { StartAbsence = new DateTime(2017, 4, 23, 9, 0, 0), EndAbsence = new DateTime(2017, 4, 23, 9, 55, 0), Date = new DateTime(2017, 4, 23), Reason = reasonMeeting, User = userNikitaMaltsev });
            absenceRepository.Create(new Absence { StartAbsence = new DateTime(2017, 4, 23, 11, 25, 0), EndAbsence = new DateTime(2017, 4, 23, 11, 55, 0), Date = new DateTime(2017, 4, 23), Reason = reasonEnglish, User = userNikitaMaltsev });
            absenceRepository.Create(new Absence { StartAbsence = new DateTime(2017, 4, 23, 15, 5, 0), EndAbsence = new DateTime(2017, 4, 23, 15, 35, 0), Date = new DateTime(2017, 4, 23), Reason = reasonConsultation, User = userNikitaMaltsev });


            // for 24 march
            absenceRepository.Create(new Absence { StartAbsence = new DateTime(2017, 4, 24, 9, 50, 0), EndAbsence = new DateTime(2017, 4, 24, 10, 20, 0), Date = new DateTime(2017, 4, 24), Reason = reasonMeeting, User = userNikitaMaltsev });
            absenceRepository.Create(new Absence { StartAbsence = new DateTime(2017, 4, 24, 10, 50, 0), EndAbsence = new DateTime(2017, 4, 24, 11, 20, 0), Date = new DateTime(2017, 4, 24), Reason = reasonConsultation, User = userNikitaMaltsev });
            absenceRepository.Create(new Absence { StartAbsence = new DateTime(2017, 4, 24, 15, 0, 0), EndAbsence = new DateTime(2017, 4, 24, 15, 25, 0), Date = new DateTime(2017, 4, 24), Reason = reasonConsultation, User = userNikitaMaltsev });

            //for 25 march
            absenceRepository.Create(new Absence { StartAbsence = new DateTime(2017, 4, 25, 9, 10, 0), EndAbsence = new DateTime(2017, 4, 25, 9, 40, 0), Date = new DateTime(2017, 4, 25), Reason = reasonMeeting, User = userNikitaMaltsev });
            absenceRepository.Create(new Absence { StartAbsence = new DateTime(2017, 4, 25, 11, 0, 0), EndAbsence = new DateTime(2017, 4, 25, 11, 35, 0), Date = new DateTime(2017, 4, 25), Reason = reasonConsultation, User = userNikitaMaltsev });
            absenceRepository.Create(new Absence { StartAbsence = new DateTime(2017, 4, 25, 13, 0, 0), EndAbsence = new DateTime(2017, 4, 25, 14, 20, 0), Date = new DateTime(2017, 4, 25), Reason = reasonEnglish, User = userNikitaMaltsev });
            absenceRepository.Create(new Absence { StartAbsence = new DateTime(2017, 4, 25, 15, 0, 0), EndAbsence = new DateTime(2017, 4, 25, 15, 20, 0), Date = new DateTime(2017, 4, 25), Reason = reasonConsultation, User = userNikitaMaltsev });

            //for 26 march
            absenceRepository.Create(new Absence { StartAbsence = new DateTime(2017, 4, 26, 8, 20, 0), EndAbsence = new DateTime(2017, 4, 26, 9, 0, 0), Date = new DateTime(2017, 4, 26), Reason = reasonMeeting, User = userNikitaMaltsev });
            absenceRepository.Create(new Absence { StartAbsence = new DateTime(2017, 4, 26, 10, 30, 0), EndAbsence = new DateTime(2017, 4, 26, 11, 5, 0), Date = new DateTime(2017, 4, 26), Reason = reasonConsultation, User = userNikitaMaltsev });
            absenceRepository.Create(new Absence { StartAbsence = new DateTime(2017, 4, 26, 12, 0, 0), EndAbsence = new DateTime(2017, 4, 26, 12, 20, 0), Date = new DateTime(2017, 4, 26), Reason = reasonConsultation, User = userNikitaMaltsev });
            absenceRepository.Create(new Absence { StartAbsence = new DateTime(2017, 4, 26, 15, 20, 0), EndAbsence = new DateTime(2017, 4, 26, 15, 30, 0), Date = new DateTime(2017, 4, 26), Reason = reasonOther, User = userNikitaMaltsev, Comment = "It was а queue at the toilet" });

            //for 27 march
            absenceRepository.Create(new Absence { StartAbsence = new DateTime(2017, 4, 27, 9, 0, 0), EndAbsence = new DateTime(2017, 4, 27, 10, 25, 0), Date = new DateTime(2017, 4, 27), Reason = reasonConsultation, User = userNikitaMaltsev, Comment = "I don't want to explain(" });
            absenceRepository.Create(new Absence { StartAbsence = new DateTime(2017, 4, 27, 10, 45, 0), EndAbsence = new DateTime(2017, 4, 27, 11, 55, 0), Date = new DateTime(2017, 4, 27), Reason = reasonMeeting, User = userNikitaMaltsev });
            absenceRepository.Create(new Absence { StartAbsence = new DateTime(2017, 4, 27, 14, 0, 0), EndAbsence = new DateTime(2017, 4, 27, 14, 40, 0), Date = new DateTime(2017, 4, 27), Reason = reasonConsultation, User = userNikitaMaltsev });

            //for 28 march
            absenceRepository.Create(new Absence { StartAbsence = new DateTime(2017, 4, 28, 9, 30, 0), EndAbsence = new DateTime(2017, 4, 28, 9, 55, 0), Date = new DateTime(2017, 4, 28), Reason = reasonEnglish, User = userNikitaMaltsev });
            absenceRepository.Create(new Absence { StartAbsence = new DateTime(2017, 4, 28, 12 ,0, 0), EndAbsence = new DateTime(2017, 4, 28, 12, 45, 0), Date = new DateTime(2017, 4, 28), Reason = reasonMeeting, User = userNikitaMaltsev });
            absenceRepository.Create(new Absence { StartAbsence = new DateTime(2017, 4, 28, 15, 5, 0), EndAbsence = new DateTime(2017, 4, 28, 15, 35, 0), Date = new DateTime(2017, 4, 28), Reason = reasonConsultation, User = userNikitaMaltsev });

            #endregion


            #region Add Abences to user ArtemChuhalo

            //for 21 April
            absenceRepository.Create(new Absence { StartAbsence = new DateTime(2017, 4, 21, 9, 10, 0), EndAbsence = new DateTime(2017, 4, 21, 9, 40, 0), Date = new DateTime(2017, 4, 21), Reason = reasonConsultation, User = userArtemChuhalo });
            absenceRepository.Create(new Absence { StartAbsence = new DateTime(2017, 4, 21, 11, 0, 0), EndAbsence = new DateTime(2017, 4, 21, 11, 35, 0), Date = new DateTime(2017, 4, 21), Reason = reasonMeeting, User = userArtemChuhalo });
            absenceRepository.Create(new Absence { StartAbsence = new DateTime(2017, 4, 21, 12, 0, 0), EndAbsence = new DateTime(2017, 4, 21, 14, 40, 0), Date = new DateTime(2017, 4, 21), Reason = reasonEnglish, User = userArtemChuhalo });
            absenceRepository.Create(new Absence { StartAbsence = new DateTime(2017, 4, 21, 15, 0, 0), EndAbsence = new DateTime(2017, 4, 21, 15, 20, 0), Date = new DateTime(2017, 4, 21), Reason = reasonConsultation, User = userArtemChuhalo });

            //for 22 April
            absenceRepository.Create(new Absence { StartAbsence = new DateTime(2017, 4, 22, 9, 20, 0), EndAbsence = new DateTime(2017, 4, 22, 9, 30, 0), Date = new DateTime(2017, 4, 22), Reason = reasonMeeting, User = userArtemChuhalo });
            absenceRepository.Create(new Absence { StartAbsence = new DateTime(2017, 4, 22, 10, 50, 0), EndAbsence = new DateTime(2017, 4, 22, 11, 5, 0), Date = new DateTime(2017, 4, 22), Reason = reasonConsultation, User = userArtemChuhalo });
            absenceRepository.Create(new Absence { StartAbsence = new DateTime(2017, 4, 22, 12, 0, 0), EndAbsence = new DateTime(2017, 4, 22, 12, 20, 0), Date = new DateTime(2017, 4, 22), Reason = reasonConsultation, User = userArtemChuhalo });
            absenceRepository.Create(new Absence { StartAbsence = new DateTime(2017, 4, 22, 15, 20, 0), EndAbsence = new DateTime(2017, 4, 22, 15, 30, 0), Date = new DateTime(2017, 4, 22), Reason = reasonOther, User = userArtemChuhalo, Comment = "It was а queue at the toilet" });

            //for 23 April
            absenceRepository.Create(new Absence { StartAbsence = new DateTime(2017, 4, 23, 9, 40, 0), EndAbsence = new DateTime(2017, 4, 23, 9, 55, 0), Date = new DateTime(2017, 4, 23), Reason = reasonEnglish, User = userArtemChuhalo });
            absenceRepository.Create(new Absence { StartAbsence = new DateTime(2017, 4, 23, 11, 25, 0), EndAbsence = new DateTime(2017, 4, 23, 11, 55, 0), Date = new DateTime(2017, 4, 23), Reason = reasonMeeting, User = userArtemChuhalo });
            absenceRepository.Create(new Absence { StartAbsence = new DateTime(2017, 4, 23, 15, 5, 0), EndAbsence = new DateTime(2017, 4, 23, 15, 35, 0), Date = new DateTime(2017, 4, 23), Reason = reasonConsultation, User = userArtemChuhalo });


            // for 24 march
            absenceRepository.Create(new Absence { StartAbsence = new DateTime(2017, 4, 24, 9, 50, 0), EndAbsence = new DateTime(2017, 4, 24, 10, 20, 0), Date = new DateTime(2017, 4, 24), Reason = reasonMeeting, User = userArtemChuhalo });
            absenceRepository.Create(new Absence { StartAbsence = new DateTime(2017, 4, 24, 10, 50, 0), EndAbsence = new DateTime(2017, 4, 24, 11, 20, 0), Date = new DateTime(2017, 4, 24), Reason = reasonConsultation, User = userArtemChuhalo });
            absenceRepository.Create(new Absence { StartAbsence = new DateTime(2017, 4, 24, 15, 0, 0), EndAbsence = new DateTime(2017, 4, 24, 15, 25, 0), Date = new DateTime(2017, 4, 24), Reason = reasonConsultation, User = userArtemChuhalo });

            //for 25 march
            absenceRepository.Create(new Absence { StartAbsence = new DateTime(2017, 4, 25, 9, 10, 0), EndAbsence = new DateTime(2017, 4, 25, 9, 40, 0), Date = new DateTime(2017, 4, 25), Reason = reasonConsultation, User = userArtemChuhalo });
            absenceRepository.Create(new Absence { StartAbsence = new DateTime(2017, 4, 25, 11, 0, 0), EndAbsence = new DateTime(2017, 4, 25, 11, 35, 0), Date = new DateTime(2017, 4, 25), Reason = reasonMeeting, User = userArtemChuhalo });
            absenceRepository.Create(new Absence { StartAbsence = new DateTime(2017, 4, 25, 11, 55, 0), EndAbsence = new DateTime(2017, 4, 25, 14, 20, 0), Date = new DateTime(2017, 4, 25), Reason = reasonEnglish, User = userArtemChuhalo });
            absenceRepository.Create(new Absence { StartAbsence = new DateTime(2017, 4, 25, 15, 0, 0), EndAbsence = new DateTime(2017, 4, 25, 15, 20, 0), Date = new DateTime(2017, 4, 25), Reason = reasonConsultation, User = userArtemChuhalo });

            //for 26 march
            absenceRepository.Create(new Absence { StartAbsence = new DateTime(2017, 4, 26, 9, 20, 0), EndAbsence = new DateTime(2017, 4, 26, 9, 30, 0), Date = new DateTime(2017, 4, 26), Reason = reasonMeeting, User = userArtemChuhalo });
            absenceRepository.Create(new Absence { StartAbsence = new DateTime(2017, 4, 26, 10, 50, 0), EndAbsence = new DateTime(2017, 4, 26, 11, 5, 0), Date = new DateTime(2017, 4, 26), Reason = reasonConsultation, User = userArtemChuhalo });
            absenceRepository.Create(new Absence { StartAbsence = new DateTime(2017, 4, 26, 12, 0, 0), EndAbsence = new DateTime(2017, 4, 26, 12, 20, 0), Date = new DateTime(2017, 4, 26), Reason = reasonConsultation, User = userArtemChuhalo });
            absenceRepository.Create(new Absence { StartAbsence = new DateTime(2017, 4, 26, 15, 20, 0), EndAbsence = new DateTime(2017, 4, 26, 15, 30, 0), Date = new DateTime(2017, 4, 26), Reason = reasonOther, User = userArtemChuhalo, Comment = "It was а queue at the toilet" });

            //for 27 march
            absenceRepository.Create(new Absence { StartAbsence = new DateTime(2017, 4, 27, 9, 0, 0), EndAbsence = new DateTime(2017, 4, 27, 9, 25, 0), Date = new DateTime(2017, 4, 27), Reason = reasonOther, User = userArtemChuhalo, Comment = "I don't want to explain(" });
            absenceRepository.Create(new Absence { StartAbsence = new DateTime(2017, 4, 27, 10, 15, 0), EndAbsence = new DateTime(2017, 4, 27, 10, 45, 0), Date = new DateTime(2017, 4, 27), Reason = reasonMeeting, User = userArtemChuhalo });
            absenceRepository.Create(new Absence { StartAbsence = new DateTime(2017, 4, 27, 14, 0, 0), EndAbsence = new DateTime(2017, 4, 27, 14, 40, 0), Date = new DateTime(2017, 4, 27), Reason = reasonConsultation, User = userArtemChuhalo });

            //for 28 march
            absenceRepository.Create(new Absence { StartAbsence = new DateTime(2017, 4, 28, 9, 40, 0), EndAbsence = new DateTime(2017, 4, 28, 9, 55, 0), Date = new DateTime(2017, 4, 28), Reason = reasonEnglish, User = userArtemChuhalo });
            absenceRepository.Create(new Absence { StartAbsence = new DateTime(2017, 4, 28, 11, 25, 0), EndAbsence = new DateTime(2017, 4, 28, 11, 55, 0), Date = new DateTime(2017, 4, 28), Reason = reasonMeeting, User = userArtemChuhalo });
            absenceRepository.Create(new Absence { StartAbsence = new DateTime(2017, 4, 28, 15, 5, 0), EndAbsence = new DateTime(2017, 4, 28, 15, 35, 0), Date = new DateTime(2017, 4, 28), Reason = reasonConsultation, User = userArtemChuhalo });


            #endregion

            #region Add Absences to user MaxMaximov
            //for 21 April
            absenceRepository.Create(new Absence { StartAbsence = new DateTime(2017, 4, 21, 9, 40, 0), EndAbsence = new DateTime(2017, 4, 21, 9, 55, 0), Date = new DateTime(2017, 4, 21), Reason = reasonConsultation, User = userMaxMaximov });
            absenceRepository.Create(new Absence { StartAbsence = new DateTime(2017, 4, 21, 11, 5, 0), EndAbsence = new DateTime(2017, 4, 21, 11, 55, 0), Date = new DateTime(2017, 4, 21), Reason = reasonMeeting, User = userMaxMaximov });
            absenceRepository.Create(new Absence { StartAbsence = new DateTime(2017, 4, 21, 13, 20, 0), EndAbsence = new DateTime(2017, 4, 21, 14, 30, 0), Date = new DateTime(2017, 4, 21), Reason = reasonEnglish, User = userMaxMaximov });
            absenceRepository.Create(new Absence { StartAbsence = new DateTime(2017, 4, 21, 16, 0, 0), EndAbsence = new DateTime(2017, 4, 21, 16, 20, 0), Date = new DateTime(2017, 4, 21), Reason = reasonConsultation, User = userMaxMaximov });

            //for 22 April
            absenceRepository.Create(new Absence { StartAbsence = new DateTime(2017, 4, 22, 8, 20, 0), EndAbsence = new DateTime(2017, 4, 22, 9, 30, 0), Date = new DateTime(2017, 4, 22), Reason = reasonMeeting, User = userMaxMaximov });
            absenceRepository.Create(new Absence { StartAbsence = new DateTime(2017, 4, 22, 10, 50, 0), EndAbsence = new DateTime(2017, 4, 22, 11, 5, 0), Date = new DateTime(2017, 4, 22), Reason = reasonConsultation, User = userMaxMaximov });
            absenceRepository.Create(new Absence { StartAbsence = new DateTime(2017, 4, 22, 12, 40, 0), EndAbsence = new DateTime(2017, 4, 22, 13, 10, 0), Date = new DateTime(2017, 4, 22), Reason = reasonConsultation, User = userMaxMaximov });
            absenceRepository.Create(new Absence { StartAbsence = new DateTime(2017, 4, 22, 15, 20, 0), EndAbsence = new DateTime(2017, 4, 22, 15, 30, 0), Date = new DateTime(2017, 4, 22), Reason = reasonOther, User = userMaxMaximov, Comment = "It was а queue at the toilet" });

            //for 23 April
            absenceRepository.Create(new Absence { StartAbsence = new DateTime(2017, 4, 23, 9, 0, 0), EndAbsence = new DateTime(2017, 4, 23, 9, 55, 0), Date = new DateTime(2017, 4, 23), Reason = reasonMeeting, User = userMaxMaximov });
            absenceRepository.Create(new Absence { StartAbsence = new DateTime(2017, 4, 23, 11, 25, 0), EndAbsence = new DateTime(2017, 4, 23, 11, 55, 0), Date = new DateTime(2017, 4, 23), Reason = reasonEnglish, User = userMaxMaximov });
            absenceRepository.Create(new Absence { StartAbsence = new DateTime(2017, 4, 23, 15, 5, 0), EndAbsence = new DateTime(2017, 4, 23, 15, 35, 0), Date = new DateTime(2017, 4, 23), Reason = reasonConsultation, User = userMaxMaximov });


            // for 24 march
            absenceRepository.Create(new Absence { StartAbsence = new DateTime(2017, 4, 24, 9, 50, 0), EndAbsence = new DateTime(2017, 4, 24, 10, 20, 0), Date = new DateTime(2017, 4, 24), Reason = reasonMeeting, User = userMaxMaximov });
            absenceRepository.Create(new Absence { StartAbsence = new DateTime(2017, 4, 24, 10, 50, 0), EndAbsence = new DateTime(2017, 4, 24, 11, 20, 0), Date = new DateTime(2017, 4, 24), Reason = reasonConsultation, User = userMaxMaximov });
            absenceRepository.Create(new Absence { StartAbsence = new DateTime(2017, 4, 24, 15, 0, 0), EndAbsence = new DateTime(2017, 4, 24, 15, 25, 0), Date = new DateTime(2017, 4, 24), Reason = reasonConsultation, User = userMaxMaximov });

            //for 25 march
            absenceRepository.Create(new Absence { StartAbsence = new DateTime(2017, 4, 25, 9, 10, 0), EndAbsence = new DateTime(2017, 4, 25, 9, 40, 0), Date = new DateTime(2017, 4, 25), Reason = reasonMeeting, User = userMaxMaximov });
            absenceRepository.Create(new Absence { StartAbsence = new DateTime(2017, 4, 25, 11, 0, 0), EndAbsence = new DateTime(2017, 4, 25, 11, 35, 0), Date = new DateTime(2017, 4, 25), Reason = reasonConsultation, User = userMaxMaximov });
            absenceRepository.Create(new Absence { StartAbsence = new DateTime(2017, 4, 25, 13, 0, 0), EndAbsence = new DateTime(2017, 4, 25, 14, 20, 0), Date = new DateTime(2017, 4, 25), Reason = reasonEnglish, User = userMaxMaximov });
            absenceRepository.Create(new Absence { StartAbsence = new DateTime(2017, 4, 25, 15, 0, 0), EndAbsence = new DateTime(2017, 4, 25, 15, 20, 0), Date = new DateTime(2017, 4, 25), Reason = reasonConsultation, User = userMaxMaximov });

            //for 26 march
            absenceRepository.Create(new Absence { StartAbsence = new DateTime(2017, 4, 26, 8, 20, 0), EndAbsence = new DateTime(2017, 4, 26, 9, 0, 0), Date = new DateTime(2017, 4, 26), Reason = reasonMeeting, User = userMaxMaximov });
            absenceRepository.Create(new Absence { StartAbsence = new DateTime(2017, 4, 26, 10, 30, 0), EndAbsence = new DateTime(2017, 4, 26, 11, 5, 0), Date = new DateTime(2017, 4, 26), Reason = reasonConsultation, User = userMaxMaximov });
            absenceRepository.Create(new Absence { StartAbsence = new DateTime(2017, 4, 26, 12, 0, 0), EndAbsence = new DateTime(2017, 4, 26, 12, 20, 0), Date = new DateTime(2017, 4, 26), Reason = reasonConsultation, User = userMaxMaximov });
            absenceRepository.Create(new Absence { StartAbsence = new DateTime(2017, 4, 26, 15, 20, 0), EndAbsence = new DateTime(2017, 4, 26, 15, 30, 0), Date = new DateTime(2017, 4, 26), Reason = reasonOther, User = userMaxMaximov, Comment = "It was а queue at the toilet" });

            //for 27 march
            absenceRepository.Create(new Absence { StartAbsence = new DateTime(2017, 4, 27, 9, 0, 0), EndAbsence = new DateTime(2017, 4, 27, 10, 25, 0), Date = new DateTime(2017, 4, 27), Reason = reasonConsultation, User = userMaxMaximov, Comment = "I don't want to explain(" });
            absenceRepository.Create(new Absence { StartAbsence = new DateTime(2017, 4, 27, 10, 45, 0), EndAbsence = new DateTime(2017, 4, 27, 11, 55, 0), Date = new DateTime(2017, 4, 27), Reason = reasonMeeting, User = userMaxMaximov });
            absenceRepository.Create(new Absence { StartAbsence = new DateTime(2017, 4, 27, 14, 0, 0), EndAbsence = new DateTime(2017, 4, 27, 14, 40, 0), Date = new DateTime(2017, 4, 27), Reason = reasonConsultation, User = userMaxMaximov });

            //for 28 march
            absenceRepository.Create(new Absence { StartAbsence = new DateTime(2017, 4, 28, 9, 30, 0), EndAbsence = new DateTime(2017, 4, 28, 9, 55, 0), Date = new DateTime(2017, 4, 28), Reason = reasonEnglish, User = userMaxMaximov });
            absenceRepository.Create(new Absence { StartAbsence = new DateTime(2017, 4, 28, 12, 0, 0), EndAbsence = new DateTime(2017, 4, 28, 12, 45, 0), Date = new DateTime(2017, 4, 28), Reason = reasonMeeting, User = userMaxMaximov });
            absenceRepository.Create(new Absence { StartAbsence = new DateTime(2017, 4, 28, 15, 5, 0), EndAbsence = new DateTime(2017, 4, 28, 15, 35, 0), Date = new DateTime(2017, 4, 28), Reason = reasonConsultation, User = userMaxMaximov });


            #endregion


            #region Add absences for AlexandraMorozova


            // for 21 march
            absenceRepository.Create(new Absence { StartAbsence = new DateTime(2017, 4, 21, 9, 0, 0), EndAbsence = new DateTime(2017, 4, 21, 10, 15, 0), Date = new DateTime(2017, 4, 21), Reason = reasonConsultation, User = userAlexandraMorozova });
            absenceRepository.Create(new Absence { StartAbsence = new DateTime(2017, 4, 21, 11, 0, 0), EndAbsence = new DateTime(2017, 4, 21, 12, 40, 0), Date = new DateTime(2017, 4, 21), Reason = reasonMeeting, User = userAlexandraMorozova });
            absenceRepository.Create(new Absence { StartAbsence = new DateTime(2017, 4, 21, 13, 20, 0), EndAbsence = new DateTime(2017, 4, 21, 14, 30, 0), Date = new DateTime(2017, 4, 21), Reason = reasonEnglish, User = userAlexandraMorozova });
            absenceRepository.Create(new Absence { StartAbsence = new DateTime(2017, 4, 21, 16, 0, 0), EndAbsence = new DateTime(2017, 4, 21, 16, 20, 0), Date = new DateTime(2017, 4, 21), Reason = reasonConsultation, User = userAlexandraMorozova });

            //for 22 April
            absenceRepository.Create(new Absence { StartAbsence = new DateTime(2017, 4, 22, 9, 20, 0), EndAbsence = new DateTime(2017, 4, 22, 9, 30, 0), Date = new DateTime(2017, 4, 22), Reason = reasonMeeting, User = userAlexandraMorozova });
            absenceRepository.Create(new Absence { StartAbsence = new DateTime(2017, 4, 22, 10, 20, 0), EndAbsence = new DateTime(2017, 4, 22, 11, 45, 0), Date = new DateTime(2017, 4, 22), Reason = reasonConsultation, User = userAlexandraMorozova });
            absenceRepository.Create(new Absence { StartAbsence = new DateTime(2017, 4, 22, 12, 40, 0), EndAbsence = new DateTime(2017, 4, 22, 13, 40, 0), Date = new DateTime(2017, 4, 22), Reason = reasonConsultation, User = userAlexandraMorozova });
            absenceRepository.Create(new Absence { StartAbsence = new DateTime(2017, 4, 22, 15, 50, 0), EndAbsence = new DateTime(2017, 4, 22, 16, 20, 0), Date = new DateTime(2017, 4, 22), Reason = reasonOther, User = userAlexandraMorozova, Comment = "It was а queue at the toilet" });

            //for 23 April
            absenceRepository.Create(new Absence { StartAbsence = new DateTime(2017, 4, 23, 9, 40, 0), EndAbsence = new DateTime(2017, 4, 23, 9, 55, 0), Date = new DateTime(2017, 4, 23), Reason = reasonMeeting, User = userAlexandraMorozova });
            absenceRepository.Create(new Absence { StartAbsence = new DateTime(2017, 4, 23, 11, 25, 0), EndAbsence = new DateTime(2017, 4, 23, 11, 55, 0), Date = new DateTime(2017, 4, 23), Reason = reasonEnglish, User = userAlexandraMorozova });
            absenceRepository.Create(new Absence { StartAbsence = new DateTime(2017, 4, 23, 15, 5, 0), EndAbsence = new DateTime(2017, 4, 23, 15, 35, 0), Date = new DateTime(2017, 4, 23), Reason = reasonConsultation, User = userAlexandraMorozova });


            // for 24 march
            absenceRepository.Create(new Absence { StartAbsence = new DateTime(2017, 4, 24, 9, 50, 0), EndAbsence = new DateTime(2017, 4, 24, 10, 20, 0), Date = new DateTime(2017, 4, 24), Reason = reasonMeeting, User = userAlexandraMorozova });
            absenceRepository.Create(new Absence { StartAbsence = new DateTime(2017, 4, 24, 10, 50, 0), EndAbsence = new DateTime(2017, 4, 24, 11, 20, 0), Date = new DateTime(2017, 4, 24), Reason = reasonConsultation, User = userAlexandraMorozova });
            absenceRepository.Create(new Absence { StartAbsence = new DateTime(2017, 4, 24, 15, 0, 0), EndAbsence = new DateTime(2017, 4, 24, 15, 25, 0), Date = new DateTime(2017, 4, 24), Reason = reasonConsultation, User = userAlexandraMorozova });

            //for 25 march
            absenceRepository.Create(new Absence { StartAbsence = new DateTime(2017, 4, 25, 9, 10, 0), EndAbsence = new DateTime(2017, 4, 25, 9, 40, 0), Date = new DateTime(2017, 4, 25), Reason = reasonMeeting, User = userAlexandraMorozova });
            absenceRepository.Create(new Absence { StartAbsence = new DateTime(2017, 4, 25, 11, 0, 0), EndAbsence = new DateTime(2017, 4, 25, 11, 35, 0), Date = new DateTime(2017, 4, 25), Reason = reasonConsultation, User = userAlexandraMorozova });
            absenceRepository.Create(new Absence { StartAbsence = new DateTime(2017, 4, 25, 13, 0, 0), EndAbsence = new DateTime(2017, 4, 25, 14, 20, 0), Date = new DateTime(2017, 4, 25), Reason = reasonEnglish, User = userAlexandraMorozova });
            absenceRepository.Create(new Absence { StartAbsence = new DateTime(2017, 4, 25, 15, 0, 0), EndAbsence = new DateTime(2017, 4, 25, 15, 20, 0), Date = new DateTime(2017, 4, 25), Reason = reasonConsultation, User = userAlexandraMorozova });

            //for 26 march
            absenceRepository.Create(new Absence { StartAbsence = new DateTime(2017, 4, 26, 8, 20, 0), EndAbsence = new DateTime(2017, 4, 26, 9, 0, 0), Date = new DateTime(2017, 4, 26), Reason = reasonMeeting, User = userAlexandraMorozova });
            absenceRepository.Create(new Absence { StartAbsence = new DateTime(2017, 4, 26, 10, 30, 0), EndAbsence = new DateTime(2017, 4, 26, 11, 5, 0), Date = new DateTime(2017, 4, 26), Reason = reasonConsultation, User = userAlexandraMorozova });
            absenceRepository.Create(new Absence { StartAbsence = new DateTime(2017, 4, 26, 12, 0, 0), EndAbsence = new DateTime(2017, 4, 26, 12, 20, 0), Date = new DateTime(2017, 4, 26), Reason = reasonConsultation, User = userAlexandraMorozova });
            absenceRepository.Create(new Absence { StartAbsence = new DateTime(2017, 4, 26, 15, 20, 0), EndAbsence = new DateTime(2017, 4, 26, 15, 30, 0), Date = new DateTime(2017, 4, 26), Reason = reasonOther, User = userAlexandraMorozova, Comment = "It was а queue at the toilet" });

            //for 27 march
            absenceRepository.Create(new Absence { StartAbsence = new DateTime(2017, 4, 27, 10, 0, 0), EndAbsence = new DateTime(2017, 4, 27, 10, 25, 0), Date = new DateTime(2017, 4, 27), Reason = reasonConsultation, User = userAlexandraMorozova, Comment = "I don't want to explain(" });
            absenceRepository.Create(new Absence { StartAbsence = new DateTime(2017, 4, 27, 11, 15, 0), EndAbsence = new DateTime(2017, 4, 27, 11, 45, 0), Date = new DateTime(2017, 4, 27), Reason = reasonMeeting, User = userAlexandraMorozova });
            absenceRepository.Create(new Absence { StartAbsence = new DateTime(2017, 4, 27, 14, 0, 0), EndAbsence = new DateTime(2017, 4, 27, 14, 40, 0), Date = new DateTime(2017, 4, 27), Reason = reasonConsultation, User = userAlexandraMorozova });

            //for 28 march
            absenceRepository.Create(new Absence { StartAbsence = new DateTime(2017, 4, 28, 9, 30, 0), EndAbsence = new DateTime(2017, 4, 28, 9, 55, 0), Date = new DateTime(2017, 4, 28), Reason = reasonEnglish, User = userAlexandraMorozova });
            absenceRepository.Create(new Absence { StartAbsence = new DateTime(2017, 4, 28, 12, 0, 0), EndAbsence = new DateTime(2017, 4, 28, 12, 45, 0), Date = new DateTime(2017, 4, 28), Reason = reasonMeeting, User = userAlexandraMorozova });
            absenceRepository.Create(new Absence { StartAbsence = new DateTime(2017, 4, 28, 15, 5, 0), EndAbsence = new DateTime(2017, 4, 28, 15, 35, 0), Date = new DateTime(2017, 4, 28), Reason = reasonConsultation, User = userAlexandraMorozova });


            #endregion
            

            #region Add Abences to user IvanIvanov

            //for 21 April
            absenceRepository.Create(new Absence { StartAbsence = new DateTime(2017, 4, 21, 9, 10, 0), EndAbsence = new DateTime(2017, 4, 21, 9, 40, 0), Date = new DateTime(2017, 4, 21), Reason = reasonConsultation, User = userIvanIvanov });
            absenceRepository.Create(new Absence { StartAbsence = new DateTime(2017, 4, 21, 11, 0, 0), EndAbsence = new DateTime(2017, 4, 21, 11, 35, 0), Date = new DateTime(2017, 4, 21), Reason = reasonMeeting, User = userIvanIvanov });
            absenceRepository.Create(new Absence { StartAbsence = new DateTime(2017, 4, 21, 12, 0, 0), EndAbsence = new DateTime(2017, 4, 21, 14, 40, 0), Date = new DateTime(2017, 4, 21), Reason = reasonEnglish, User = userIvanIvanov });
            absenceRepository.Create(new Absence { StartAbsence = new DateTime(2017, 4, 21, 15, 0, 0), EndAbsence = new DateTime(2017, 4, 21, 15, 20, 0), Date = new DateTime(2017, 4, 21), Reason = reasonConsultation, User = userIvanIvanov });

            //for 22 April
            absenceRepository.Create(new Absence { StartAbsence = new DateTime(2017, 4, 22, 9, 20, 0), EndAbsence = new DateTime(2017, 4, 22, 9, 30, 0), Date = new DateTime(2017, 4, 22), Reason = reasonMeeting, User = userIvanIvanov });
            absenceRepository.Create(new Absence { StartAbsence = new DateTime(2017, 4, 22, 10, 50, 0), EndAbsence = new DateTime(2017, 4, 22, 11, 5, 0), Date = new DateTime(2017, 4, 22), Reason = reasonConsultation, User = userIvanIvanov });
            absenceRepository.Create(new Absence { StartAbsence = new DateTime(2017, 4, 22, 12, 0, 0), EndAbsence = new DateTime(2017, 4, 22, 12, 20, 0), Date = new DateTime(2017, 4, 22), Reason = reasonConsultation, User = userIvanIvanov });
            absenceRepository.Create(new Absence { StartAbsence = new DateTime(2017, 4, 22, 15, 20, 0), EndAbsence = new DateTime(2017, 4, 22, 15, 30, 0), Date = new DateTime(2017, 4, 22), Reason = reasonOther, User = userIvanIvanov, Comment = "It was а queue at the toilet" });

            //for 23 April
            absenceRepository.Create(new Absence { StartAbsence = new DateTime(2017, 4, 23, 9, 40, 0), EndAbsence = new DateTime(2017, 4, 23, 9, 55, 0), Date = new DateTime(2017, 4, 23), Reason = reasonEnglish, User = userIvanIvanov });
            absenceRepository.Create(new Absence { StartAbsence = new DateTime(2017, 4, 23, 11, 25, 0), EndAbsence = new DateTime(2017, 4, 23, 11, 55, 0), Date = new DateTime(2017, 4, 23), Reason = reasonMeeting, User = userIvanIvanov });
            absenceRepository.Create(new Absence { StartAbsence = new DateTime(2017, 4, 23, 15, 5, 0), EndAbsence = new DateTime(2017, 4, 23, 15, 35, 0), Date = new DateTime(2017, 4, 23), Reason = reasonConsultation, User = userIvanIvanov });


            // for 24 march
            absenceRepository.Create(new Absence { StartAbsence = new DateTime(2017, 4, 24, 9, 50, 0), EndAbsence = new DateTime(2017, 4, 24, 10, 20, 0), Date = new DateTime(2017, 4, 24), Reason = reasonMeeting, User = userIvanIvanov });
            absenceRepository.Create(new Absence { StartAbsence = new DateTime(2017, 4, 24, 10, 50, 0), EndAbsence = new DateTime(2017, 4, 24, 11, 20, 0), Date = new DateTime(2017, 4, 24), Reason = reasonConsultation, User = userIvanIvanov });
            absenceRepository.Create(new Absence { StartAbsence = new DateTime(2017, 4, 24, 15, 0, 0), EndAbsence = new DateTime(2017, 4, 24, 15, 25, 0), Date = new DateTime(2017, 4, 24), Reason = reasonConsultation, User = userIvanIvanov });

            //for 25 march
            absenceRepository.Create(new Absence { StartAbsence = new DateTime(2017, 4, 25, 9, 10, 0), EndAbsence = new DateTime(2017, 4, 25, 9, 40, 0), Date = new DateTime(2017, 4, 25), Reason = reasonConsultation, User = userIvanIvanov });
            absenceRepository.Create(new Absence { StartAbsence = new DateTime(2017, 4, 25, 11, 0, 0), EndAbsence = new DateTime(2017, 4, 25, 11, 35, 0), Date = new DateTime(2017, 4, 25), Reason = reasonMeeting, User = userIvanIvanov });
            absenceRepository.Create(new Absence { StartAbsence = new DateTime(2017, 4, 25, 11, 55, 0), EndAbsence = new DateTime(2017, 4, 25, 14, 20, 0), Date = new DateTime(2017, 4, 25), Reason = reasonEnglish, User = userIvanIvanov });
            absenceRepository.Create(new Absence { StartAbsence = new DateTime(2017, 4, 25, 15, 0, 0), EndAbsence = new DateTime(2017, 4, 25, 15, 20, 0), Date = new DateTime(2017, 4, 25), Reason = reasonConsultation, User = userIvanIvanov });

            //for 26 march
            absenceRepository.Create(new Absence { StartAbsence = new DateTime(2017, 4, 26, 9, 20, 0), EndAbsence = new DateTime(2017, 4, 26, 9, 30, 0), Date = new DateTime(2017, 4, 26), Reason = reasonMeeting, User = userIvanIvanov });
            absenceRepository.Create(new Absence { StartAbsence = new DateTime(2017, 4, 26, 10, 50, 0), EndAbsence = new DateTime(2017, 4, 26, 11, 5, 0), Date = new DateTime(2017, 4, 26), Reason = reasonConsultation, User = userIvanIvanov });
            absenceRepository.Create(new Absence { StartAbsence = new DateTime(2017, 4, 26, 12, 0, 0), EndAbsence = new DateTime(2017, 4, 26, 12, 20, 0), Date = new DateTime(2017, 4, 26), Reason = reasonConsultation, User = userIvanIvanov });
            absenceRepository.Create(new Absence { StartAbsence = new DateTime(2017, 4, 26, 15, 20, 0), EndAbsence = new DateTime(2017, 4, 26, 15, 30, 0), Date = new DateTime(2017, 4, 26), Reason = reasonOther, User = userIvanIvanov, Comment = "It was а queue at the toilet" });

            //for 27 march
            absenceRepository.Create(new Absence { StartAbsence = new DateTime(2017, 4, 27, 9, 0, 0), EndAbsence = new DateTime(2017, 4, 27, 9, 25, 0), Date = new DateTime(2017, 4, 27), Reason = reasonOther, User = userIvanIvanov, Comment = "I don't want to explain(" });
            absenceRepository.Create(new Absence { StartAbsence = new DateTime(2017, 4, 27, 10, 15, 0), EndAbsence = new DateTime(2017, 4, 27, 10, 45, 0), Date = new DateTime(2017, 4, 27), Reason = reasonMeeting, User = userIvanIvanov });
            absenceRepository.Create(new Absence { StartAbsence = new DateTime(2017, 4, 27, 14, 0, 0), EndAbsence = new DateTime(2017, 4, 27, 14, 40, 0), Date = new DateTime(2017, 4, 27), Reason = reasonConsultation, User = userIvanIvanov });

            //for 28 march
            absenceRepository.Create(new Absence { StartAbsence = new DateTime(2017, 4, 28, 9, 40, 0), EndAbsence = new DateTime(2017, 4, 28, 9, 55, 0), Date = new DateTime(2017, 4, 28), Reason = reasonEnglish, User = userIvanIvanov });
            absenceRepository.Create(new Absence { StartAbsence = new DateTime(2017, 4, 28, 11, 25, 0), EndAbsence = new DateTime(2017, 4, 28, 11, 55, 0), Date = new DateTime(2017, 4, 28), Reason = reasonMeeting, User = userIvanIvanov });
            absenceRepository.Create(new Absence { StartAbsence = new DateTime(2017, 4, 28, 15, 5, 0), EndAbsence = new DateTime(2017, 4, 28, 15, 35, 0), Date = new DateTime(2017, 4, 28), Reason = reasonConsultation, User = userIvanIvanov });


            #endregion
            #endregion
            //Repository<UserLogin> UserLoginRepository = new Repository<UserLogin>(appContext);
            //UserLoginRepository.Create(new UserLogin { Login = "svezho" });
            //UserLoginRepository.Create(new UserLogin { Login = "Alexandr" });
            base.Seed(context);
        }
    }
 
}