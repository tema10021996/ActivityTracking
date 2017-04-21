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
            //Add Groups
            Repository<Group> groupRepository = new Repository<Group>(context);
            groupRepository.Create(new Group { Name = "Group1", MayAbsentTime = new TimeSpan(00,01,00)});
            groupRepository.Create(new Group { Name = "Group2", MayAbsentTime = new TimeSpan(00, 02, 00) });
            groupRepository.Create(new Group { Name = "Group3", MayAbsentTime = new TimeSpan(00, 03, 00) });
            groupRepository.Create(new Group { Name = "Group4", MayAbsentTime = new TimeSpan(00, 04, 00) });
            #endregion 

            #region Set reasons to groups
            //Set reasons to groups
            Group group1 = groupRepository.GetList().First(g=>g.Name == "Group1");
            group1.Reasons.Add(reasonRepository.GetList().First(r => r.Name == "Meeting"));
            group1.Reasons.Add(reasonRepository.GetList().First(r => r.Name == "Consultation"));
            group1.Reasons.Add(reasonRepository.GetList().First(r => r.Name == "English"));
            group1.Reasons.Add(reasonRepository.GetList().First(r => r.Name == "Other"));
            groupRepository.Update(group1);

            Group group2 = groupRepository.GetList().First(g => g.Name == "Group2");
            group2.Reasons.Add(reasonRepository.GetList().First(r => r.Name == "Meeting"));
            group2.Reasons.Add(reasonRepository.GetList().First(r => r.Name == "Consultation"));
            group2.Reasons.Add(reasonRepository.GetList().First(r => r.Name == "English"));
            group2.Reasons.Add(reasonRepository.GetList().First(r => r.Name == "Other"));
            groupRepository.Update(group2);

            Group group3 = groupRepository.GetList().First(g => g.Name == "Group3");
            group3.Reasons.Add(reasonRepository.GetList().First(r => r.Name == "Meeting"));
            group3.Reasons.Add(reasonRepository.GetList().First(r => r.Name == "Consultation"));
            group3.Reasons.Add(reasonRepository.GetList().First(r => r.Name == "English"));
            group3.Reasons.Add(reasonRepository.GetList().First(r => r.Name == "Other"));
            groupRepository.Update(group3);

            Group group4 = groupRepository.GetList().First(g => g.Name == "Group4");
            group4.Reasons.Add(reasonRepository.GetList().First(r => r.Name == "Meeting"));
            group4.Reasons.Add(reasonRepository.GetList().First(r => r.Name == "Consultation"));
            group4.Reasons.Add(reasonRepository.GetList().First(r => r.Name == "English"));
            group4.Reasons.Add(reasonRepository.GetList().First(r => r.Name == "Other"));
            groupRepository.Update(group4);
            #endregion

            #region Add users to Group1

            Repository<ApplicationUser> applicationUserRepository = new Repository<ApplicationUser>(context);
            var ggroup1 = groupRepository.GetList().First(g => g.Name == "Group1");
            ggroup1.Users.Add(applicationUserRepository.GetList().First(u=>u.UserName == "NikitaMaltsev"));
            ggroup1.Users.Add(applicationUserRepository.GetList().First(u => u.UserName == "AlexandrTkachuk"));
            ggroup1.Users.Add(applicationUserRepository.GetList().First(u => u.UserName == "ArtemChuhalo"));
            ggroup1.Users.Add(applicationUserRepository.GetList().First(u => u.UserName == "AlexandraMorozova"));
            groupRepository.Update(ggroup1);
            #endregion


            #region Add Absences
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

            #region Add Absences and times to user Alexandr
            //Times for AlexandrTkachuk
            Repository<Time> timeRepository = new Repository<Time>(context);

            timeRepository.Create(new Time { Date = new DateTime(2017, 4, 13), TimeIn = new DateTime(2017, 4, 13, 8, 40, 0), TimeOut = new DateTime(2017, 4, 13, 10, 20, 0), User = userAlexandrTkachuk });
            timeRepository.Create(new Time { Date = new DateTime(2017, 4, 13), TimeIn = new DateTime(2017, 4, 13, 10, 40, 0), TimeOut = new DateTime(2017, 4, 13, 17, 20, 0), User = userAlexandrTkachuk });


            timeRepository.Create(new Time { Date = new DateTime(2017, 4, 14), TimeIn = new DateTime(2017, 4, 14, 8, 0, 0), TimeOut = new DateTime(2017, 4, 14, 14, 0, 0), User = userAlexandrTkachuk });
            timeRepository.Create(new Time { Date = new DateTime(2017, 4, 14), TimeIn = new DateTime(2017, 4, 14, 15, 0, 0), TimeOut = new DateTime(2017, 4, 14, 16, 50, 0), User = userAlexandrTkachuk });


            timeRepository.Create(new Time { Date = new DateTime(2017, 4, 15), TimeIn = new DateTime(2017, 4, 15, 8, 55, 0), TimeOut = new DateTime(2017, 4, 15, 10, 0, 0), User = userAlexandrTkachuk });
            timeRepository.Create(new Time { Date = new DateTime(2017, 4, 15), TimeIn = new DateTime(2017, 4, 15, 10, 55, 0), TimeOut = new DateTime(2017, 4, 15, 12, 50, 0), User = userAlexandrTkachuk });
            timeRepository.Create(new Time { Date = new DateTime(2017, 4, 15), TimeIn = new DateTime(2017, 4, 15, 13, 0, 0), TimeOut = new DateTime(2017, 4, 15, 16, 50, 0), User = userAlexandrTkachuk });

            timeRepository.Create(new Time { Date = new DateTime(2017, 4, 16), TimeIn = new DateTime(2017, 4, 16, 8, 15, 0), TimeOut = new DateTime(2017, 4, 16, 12, 20, 0), User = userAlexandrTkachuk });
            timeRepository.Create(new Time { Date = new DateTime(2017, 4, 16), TimeIn = new DateTime(2017, 4, 16, 12, 50, 0), TimeOut = new DateTime(2017, 4, 16, 16, 20, 0), User = userAlexandrTkachuk });

            timeRepository.Create(new Time { Date = new DateTime(2017, 4, 17), TimeIn = new DateTime(2017, 4, 17, 8, 40, 0), TimeOut = new DateTime(2017, 4, 17, 10, 20, 0), User = userAlexandrTkachuk });
            timeRepository.Create(new Time { Date = new DateTime(2017, 4, 17), TimeIn = new DateTime(2017, 4, 17, 10, 40, 0), TimeOut = new DateTime(2017, 4, 17, 17, 20, 0), User = userAlexandrTkachuk });

            timeRepository.Create(new Time { Date = new DateTime(2017, 4, 18), TimeIn = new DateTime(2017, 4, 18, 8, 0, 0), TimeOut = new DateTime(2017, 4, 18, 14, 0, 0), User = userAlexandrTkachuk });
            timeRepository.Create(new Time { Date = new DateTime(2017, 4, 18), TimeIn = new DateTime(2017, 4, 18, 15, 0, 0), TimeOut = new DateTime(2017, 4, 18, 16, 50, 0), User = userAlexandrTkachuk });

            timeRepository.Create(new Time { Date = new DateTime(2017, 4, 19), TimeIn = new DateTime(2017, 4, 19, 8, 50, 0), TimeOut = new DateTime(2017, 4, 19, 17, 40, 0), User = userAlexandrTkachuk });;

            timeRepository.Create(new Time { Date = new DateTime(2017, 4, 20), TimeIn = new DateTime(2017, 4, 20, 8, 55, 0), TimeOut = new DateTime(2017, 4, 20, 10, 0, 0), User = userAlexandrTkachuk });
            timeRepository.Create(new Time { Date = new DateTime(2017, 4, 20), TimeIn = new DateTime(2017, 4, 20, 10, 55, 0), TimeOut = new DateTime(2017, 4, 20, 12, 50, 0), User = userAlexandrTkachuk });
            timeRepository.Create(new Time { Date = new DateTime(2017, 4, 20), TimeIn = new DateTime(2017, 4, 20, 13, 0, 0), TimeOut = new DateTime(2017, 4, 20, 16, 50, 0), User = userAlexandrTkachuk });

            //Add Absences for AlexandrTkachuk
            Repository<Absence> absenceRepository = new Repository<Absence>(context);

            //for 13 April
            absenceRepository.Create(new Absence { StartAbsence = new DateTime(2017, 4, 13, 9, 10, 0), EndAbsence = new DateTime(2017, 4, 13, 9, 40, 0), Date = new DateTime(2017, 4, 13), Reason = reasonConsultation, User = userAlexandrTkachuk });
            absenceRepository.Create(new Absence { StartAbsence = new DateTime(2017, 4, 13, 11, 0, 0), EndAbsence = new DateTime(2017, 4, 13, 11, 35, 0), Date = new DateTime(2017, 4, 13), Reason = reasonMeeting, User = userAlexandrTkachuk });
            absenceRepository.Create(new Absence { StartAbsence = new DateTime(2017, 4, 13, 13, 0, 0), EndAbsence = new DateTime(2017, 4, 13, 14, 20, 0), Date = new DateTime(2017, 4, 13), Reason = reasonEnglish, User = userAlexandrTkachuk });
            absenceRepository.Create(new Absence { StartAbsence = new DateTime(2017, 4, 13, 15, 0, 0), EndAbsence = new DateTime(2017, 4, 13, 15, 20, 0), Date = new DateTime(2017, 4, 13), Reason = reasonConsultation, User = userAlexandrTkachuk });

            //for 14 April
            absenceRepository.Create(new Absence { StartAbsence = new DateTime(2017, 4, 14, 9, 20, 0), EndAbsence = new DateTime(2017, 4, 14, 9, 30, 0), Date = new DateTime(2017, 4, 14), Reason = reasonMeeting, User = userAlexandrTkachuk });
            absenceRepository.Create(new Absence { StartAbsence = new DateTime(2017, 4, 14, 10, 50, 0), EndAbsence = new DateTime(2017, 4, 14, 11, 5, 0), Date = new DateTime(2017, 4, 14), Reason = reasonConsultation, User = userAlexandrTkachuk });
            absenceRepository.Create(new Absence { StartAbsence = new DateTime(2017, 4, 14, 12, 0, 0), EndAbsence = new DateTime(2017, 4, 14, 12, 20, 0), Date = new DateTime(2017, 4, 14), Reason = reasonConsultation, User = userAlexandrTkachuk });
            absenceRepository.Create(new Absence { StartAbsence = new DateTime(2017, 4, 14, 15, 20, 0), EndAbsence = new DateTime(2017, 4, 14, 15, 30, 0), Date = new DateTime(2017, 4, 14), Reason = reasonOther, User = userAlexandrTkachuk, Comment = "It was а queue at the toilet" });

            //for 15 April
            absenceRepository.Create(new Absence { StartAbsence = new DateTime(2017, 4, 15, 9, 40, 0), EndAbsence = new DateTime(2017, 4, 15, 9, 55, 0), Date = new DateTime(2017, 4, 15), Reason = reasonEnglish, User = userAlexandrTkachuk });
            absenceRepository.Create(new Absence { StartAbsence = new DateTime(2017, 4, 15, 11, 25, 0), EndAbsence = new DateTime(2017, 4, 15, 11, 55, 0), Date = new DateTime(2017, 4, 15), Reason = reasonMeeting, User = userAlexandrTkachuk });
            absenceRepository.Create(new Absence { StartAbsence = new DateTime(2017, 4, 15, 15, 5, 0), EndAbsence = new DateTime(2017, 4, 15, 15, 35, 0), Date = new DateTime(2017, 4, 15), Reason = reasonConsultation, User = userAlexandrTkachuk });


            // for 5 march
            absenceRepository.Create(new Absence { StartAbsence = new DateTime(2017, 4, 16, 9, 50, 0), EndAbsence = new DateTime(2017, 4, 16, 10, 20, 0), Date = new DateTime(2017, 4, 16), Reason = reasonMeeting, User = userAlexandrTkachuk });
            absenceRepository.Create(new Absence { StartAbsence = new DateTime(2017, 4, 16, 10, 50, 0), EndAbsence = new DateTime(2017, 4, 16, 11, 20, 0), Date = new DateTime(2017, 4, 16), Reason = reasonConsultation, User = userAlexandrTkachuk });
            absenceRepository.Create(new Absence { StartAbsence = new DateTime(2017, 4, 16, 15, 0, 0), EndAbsence = new DateTime(2017, 4, 16, 15, 25, 0), Date = new DateTime(2017, 4, 16), Reason = reasonConsultation, User = userAlexandrTkachuk });

            //for 6 march
            absenceRepository.Create(new Absence { StartAbsence = new DateTime(2017, 4, 17, 9, 10, 0), EndAbsence = new DateTime(2017, 4, 17, 9, 40, 0), Date = new DateTime(2017, 4, 17), Reason = reasonConsultation, User = userAlexandrTkachuk });
            absenceRepository.Create(new Absence { StartAbsence = new DateTime(2017, 4, 17, 11, 0, 0), EndAbsence = new DateTime(2017, 4, 17, 11, 35, 0), Date = new DateTime(2017, 4, 17), Reason = reasonMeeting, User = userAlexandrTkachuk });
            absenceRepository.Create(new Absence { StartAbsence = new DateTime(2017, 4, 17, 13, 0, 0), EndAbsence = new DateTime(2017, 4, 17, 14, 20, 0), Date = new DateTime(2017, 4, 17), Reason = reasonEnglish, User = userAlexandrTkachuk });
            absenceRepository.Create(new Absence { StartAbsence = new DateTime(2017, 4, 17, 15, 0, 0), EndAbsence = new DateTime(2017, 4, 17, 15, 20, 0), Date = new DateTime(2017, 4, 17), Reason = reasonConsultation, User = userAlexandrTkachuk });

            //for 7 march
            absenceRepository.Create(new Absence { StartAbsence = new DateTime(2017, 4, 18, 9, 20, 0), EndAbsence = new DateTime(2017, 4, 18, 9, 30, 0), Date = new DateTime(2017, 4, 18), Reason = reasonMeeting, User = userAlexandrTkachuk });
            absenceRepository.Create(new Absence { StartAbsence = new DateTime(2017, 4, 18, 10, 50, 0), EndAbsence = new DateTime(2017, 4, 18, 11, 5, 0), Date = new DateTime(2017, 4, 18), Reason = reasonConsultation, User = userAlexandrTkachuk });
            absenceRepository.Create(new Absence { StartAbsence = new DateTime(2017, 4, 18, 12, 0, 0), EndAbsence = new DateTime(2017, 4, 18, 12, 20, 0), Date = new DateTime(2017, 4, 18), Reason = reasonConsultation, User = userAlexandrTkachuk });
            absenceRepository.Create(new Absence { StartAbsence = new DateTime(2017, 4, 18, 15, 20, 0), EndAbsence = new DateTime(2017, 4, 18, 15, 30, 0), Date = new DateTime(2017, 4, 18), Reason = reasonOther, User = userAlexandrTkachuk, Comment = "It was а queue at the toilet" });

            //for 8 march
            absenceRepository.Create(new Absence { StartAbsence = new DateTime(2017, 4, 19, 9, 0, 0), EndAbsence = new DateTime(2017, 4, 19, 9, 25, 0), Date = new DateTime(2017, 4, 19), Reason = reasonOther, User = userAlexandrTkachuk, Comment = "I don't want to explain(" });
            absenceRepository.Create(new Absence { StartAbsence = new DateTime(2017, 4, 19, 10, 15, 0), EndAbsence = new DateTime(2017, 4, 19, 10, 45, 0), Date = new DateTime(2017, 4, 19), Reason = reasonMeeting, User = userAlexandrTkachuk });
            absenceRepository.Create(new Absence { StartAbsence = new DateTime(2017, 4, 19, 14, 0, 0), EndAbsence = new DateTime(2017, 4, 19, 14, 40, 0), Date = new DateTime(2017, 4, 19), Reason = reasonConsultation, User = userAlexandrTkachuk });

            //for 9 march
            absenceRepository.Create(new Absence { StartAbsence = new DateTime(2017, 4, 20, 9, 40, 0), EndAbsence = new DateTime(2017, 4, 20, 9, 55, 0), Date = new DateTime(2017, 4, 20), Reason = reasonEnglish, User = userAlexandrTkachuk });
            absenceRepository.Create(new Absence { StartAbsence = new DateTime(2017, 4, 20, 11, 25, 0), EndAbsence = new DateTime(2017, 4, 20, 11, 55, 0), Date = new DateTime(2017, 4, 20), Reason = reasonMeeting, User = userAlexandrTkachuk });
            absenceRepository.Create(new Absence { StartAbsence = new DateTime(2017, 4, 20, 15, 5, 0), EndAbsence = new DateTime(2017, 4, 20, 15, 35, 0), Date = new DateTime(2017, 4, 20), Reason = reasonConsultation, User = userAlexandrTkachuk });



            #endregion


            #region Add Absences and times for Nikita
            timeRepository.Create(new Time { Date = new DateTime(2017, 4, 13), TimeIn = new DateTime(2017, 4, 13, 8, 40, 0), TimeOut = new DateTime(2017, 4, 13, 10, 20, 0), User = userNikitaMaltsev });
            timeRepository.Create(new Time { Date = new DateTime(2017, 4, 13), TimeIn = new DateTime(2017, 4, 13, 10, 40, 0), TimeOut = new DateTime(2017, 4, 13, 17, 20, 0), User = userNikitaMaltsev });


            timeRepository.Create(new Time { Date = new DateTime(2017, 4, 14), TimeIn = new DateTime(2017, 4, 14, 8, 0, 0), TimeOut = new DateTime(2017, 4, 14, 14, 0, 0), User = userNikitaMaltsev });
            timeRepository.Create(new Time { Date = new DateTime(2017, 4, 14), TimeIn = new DateTime(2017, 4, 14, 15, 0, 0), TimeOut = new DateTime(2017, 4, 14, 16, 50, 0), User = userNikitaMaltsev });


            timeRepository.Create(new Time { Date = new DateTime(2017, 4, 15), TimeIn = new DateTime(2017, 4, 15, 8, 55, 0), TimeOut = new DateTime(2017, 4, 15, 10, 0, 0), User = userNikitaMaltsev });
            timeRepository.Create(new Time { Date = new DateTime(2017, 4, 15), TimeIn = new DateTime(2017, 4, 15, 10, 55, 0), TimeOut = new DateTime(2017, 4, 15, 12, 50, 0), User = userNikitaMaltsev });
            timeRepository.Create(new Time { Date = new DateTime(2017, 4, 15), TimeIn = new DateTime(2017, 4, 15, 13, 0, 0), TimeOut = new DateTime(2017, 4, 15, 16, 50, 0), User = userNikitaMaltsev });

            timeRepository.Create(new Time { Date = new DateTime(2017, 4, 16), TimeIn = new DateTime(2017, 4, 16, 8, 15, 0), TimeOut = new DateTime(2017, 4, 16, 12, 20, 0), User = userNikitaMaltsev });
            timeRepository.Create(new Time { Date = new DateTime(2017, 4, 16), TimeIn = new DateTime(2017, 4, 16, 12, 50, 0), TimeOut = new DateTime(2017, 4, 16, 16, 20, 0), User = userNikitaMaltsev });

            timeRepository.Create(new Time { Date = new DateTime(2017, 4, 17), TimeIn = new DateTime(2017, 4, 17, 8, 40, 0), TimeOut = new DateTime(2017, 4, 17, 10, 20, 0), User = userNikitaMaltsev });
            timeRepository.Create(new Time { Date = new DateTime(2017, 4, 17), TimeIn = new DateTime(2017, 4, 17, 10, 40, 0), TimeOut = new DateTime(2017, 4, 17, 17, 20, 0), User = userNikitaMaltsev });

            timeRepository.Create(new Time { Date = new DateTime(2017, 4, 18), TimeIn = new DateTime(2017, 4, 18, 8, 0, 0), TimeOut = new DateTime(2017, 4, 18, 14, 0, 0), User = userNikitaMaltsev });
            timeRepository.Create(new Time { Date = new DateTime(2017, 4, 18), TimeIn = new DateTime(2017, 4, 18, 15, 0, 0), TimeOut = new DateTime(2017, 4, 18, 16, 50, 0), User = userNikitaMaltsev });

            timeRepository.Create(new Time { Date = new DateTime(2017, 4, 19), TimeIn = new DateTime(2017, 4, 19, 8, 50, 0), TimeOut = new DateTime(2017, 4, 19, 17, 40, 0), User = userNikitaMaltsev }); ;

            timeRepository.Create(new Time { Date = new DateTime(2017, 4, 20), TimeIn = new DateTime(2017, 4, 20, 8, 55, 0), TimeOut = new DateTime(2017, 4, 20, 10, 0, 0), User = userNikitaMaltsev });
            timeRepository.Create(new Time { Date = new DateTime(2017, 4, 20), TimeIn = new DateTime(2017, 4, 20, 10, 55, 0), TimeOut = new DateTime(2017, 4, 20, 12, 50, 0), User = userNikitaMaltsev });
            timeRepository.Create(new Time { Date = new DateTime(2017, 4, 20), TimeIn = new DateTime(2017, 4, 20, 13, 0, 0), TimeOut = new DateTime(2017, 4, 20, 16, 50, 0), User = userNikitaMaltsev });

            //Add Absences for Nikita

            //for 13 April
            absenceRepository.Create(new Absence { StartAbsence = new DateTime(2017, 4, 13, 9, 10, 0), EndAbsence = new DateTime(2017, 4, 13, 9, 40, 0), Date = new DateTime(2017, 4, 13), Reason = reasonConsultation, User = userNikitaMaltsev });
            absenceRepository.Create(new Absence { StartAbsence = new DateTime(2017, 4, 13, 11, 0, 0), EndAbsence = new DateTime(2017, 4, 13, 11, 35, 0), Date = new DateTime(2017, 4, 13), Reason = reasonMeeting, User = userNikitaMaltsev });
            absenceRepository.Create(new Absence { StartAbsence = new DateTime(2017, 4, 13, 13, 0, 0), EndAbsence = new DateTime(2017, 4, 13, 14, 20, 0), Date = new DateTime(2017, 4, 13), Reason = reasonEnglish, User = userNikitaMaltsev });
            absenceRepository.Create(new Absence { StartAbsence = new DateTime(2017, 4, 13, 15, 0, 0), EndAbsence = new DateTime(2017, 4, 13, 15, 20, 0), Date = new DateTime(2017, 4, 13), Reason = reasonConsultation, User = userNikitaMaltsev });

            //for 14 April
            absenceRepository.Create(new Absence { StartAbsence = new DateTime(2017, 4, 14, 9, 20, 0), EndAbsence = new DateTime(2017, 4, 14, 9, 30, 0), Date = new DateTime(2017, 4, 14), Reason = reasonMeeting, User = userNikitaMaltsev });
            absenceRepository.Create(new Absence { StartAbsence = new DateTime(2017, 4, 14, 10, 50, 0), EndAbsence = new DateTime(2017, 4, 14, 11, 5, 0), Date = new DateTime(2017, 4, 14), Reason = reasonConsultation, User = userNikitaMaltsev });
            absenceRepository.Create(new Absence { StartAbsence = new DateTime(2017, 4, 14, 12, 0, 0), EndAbsence = new DateTime(2017, 4, 14, 12, 20, 0), Date = new DateTime(2017, 4, 14), Reason = reasonConsultation, User = userNikitaMaltsev });
            absenceRepository.Create(new Absence { StartAbsence = new DateTime(2017, 4, 14, 15, 20, 0), EndAbsence = new DateTime(2017, 4, 14, 15, 30, 0), Date = new DateTime(2017, 4, 14), Reason = reasonOther, User = userNikitaMaltsev, Comment = "It was а queue at the toilet" });

            //for 15 April
            absenceRepository.Create(new Absence { StartAbsence = new DateTime(2017, 4, 15, 9, 40, 0), EndAbsence = new DateTime(2017, 4, 15, 9, 55, 0), Date = new DateTime(2017, 4, 15), Reason = reasonEnglish, User = userNikitaMaltsev });
            absenceRepository.Create(new Absence { StartAbsence = new DateTime(2017, 4, 15, 11, 25, 0), EndAbsence = new DateTime(2017, 4, 15, 11, 55, 0), Date = new DateTime(2017, 4, 15), Reason = reasonMeeting, User = userNikitaMaltsev });
            absenceRepository.Create(new Absence { StartAbsence = new DateTime(2017, 4, 15, 15, 5, 0), EndAbsence = new DateTime(2017, 4, 15, 15, 35, 0), Date = new DateTime(2017, 4, 15), Reason = reasonConsultation, User = userNikitaMaltsev });


            // for 5 march
            absenceRepository.Create(new Absence { StartAbsence = new DateTime(2017, 4, 16, 9, 50, 0), EndAbsence = new DateTime(2017, 4, 16, 10, 20, 0), Date = new DateTime(2017, 4, 16), Reason = reasonMeeting, User = userNikitaMaltsev });
            absenceRepository.Create(new Absence { StartAbsence = new DateTime(2017, 4, 16, 10, 50, 0), EndAbsence = new DateTime(2017, 4, 16, 11, 20, 0), Date = new DateTime(2017, 4, 16), Reason = reasonConsultation, User = userNikitaMaltsev });
            absenceRepository.Create(new Absence { StartAbsence = new DateTime(2017, 4, 16, 15, 0, 0), EndAbsence = new DateTime(2017, 4, 16, 15, 25, 0), Date = new DateTime(2017, 4, 16), Reason = reasonConsultation, User = userNikitaMaltsev });

            //for 6 march
            absenceRepository.Create(new Absence { StartAbsence = new DateTime(2017, 4, 17, 9, 10, 0), EndAbsence = new DateTime(2017, 4, 17, 9, 40, 0), Date = new DateTime(2017, 4, 17), Reason = reasonConsultation, User = userNikitaMaltsev });
            absenceRepository.Create(new Absence { StartAbsence = new DateTime(2017, 4, 17, 11, 0, 0), EndAbsence = new DateTime(2017, 4, 17, 11, 35, 0), Date = new DateTime(2017, 4, 17), Reason = reasonMeeting, User = userNikitaMaltsev });
            absenceRepository.Create(new Absence { StartAbsence = new DateTime(2017, 4, 17, 13, 0, 0), EndAbsence = new DateTime(2017, 4, 17, 14, 20, 0), Date = new DateTime(2017, 4, 17), Reason = reasonEnglish, User = userNikitaMaltsev });
            absenceRepository.Create(new Absence { StartAbsence = new DateTime(2017, 4, 17, 15, 0, 0), EndAbsence = new DateTime(2017, 4, 17, 15, 20, 0), Date = new DateTime(2017, 4, 17), Reason = reasonConsultation, User = userNikitaMaltsev });

            //for 7 march
            absenceRepository.Create(new Absence { StartAbsence = new DateTime(2017, 4, 18, 9, 20, 0), EndAbsence = new DateTime(2017, 4, 18, 9, 30, 0), Date = new DateTime(2017, 4, 18), Reason = reasonMeeting, User = userNikitaMaltsev });
            absenceRepository.Create(new Absence { StartAbsence = new DateTime(2017, 4, 18, 10, 50, 0), EndAbsence = new DateTime(2017, 4, 18, 11, 5, 0), Date = new DateTime(2017, 4, 18), Reason = reasonConsultation, User = userNikitaMaltsev });
            absenceRepository.Create(new Absence { StartAbsence = new DateTime(2017, 4, 18, 12, 0, 0), EndAbsence = new DateTime(2017, 4, 18, 12, 20, 0), Date = new DateTime(2017, 4, 18), Reason = reasonConsultation, User = userNikitaMaltsev });
            absenceRepository.Create(new Absence { StartAbsence = new DateTime(2017, 4, 18, 15, 20, 0), EndAbsence = new DateTime(2017, 4, 18, 15, 30, 0), Date = new DateTime(2017, 4, 18), Reason = reasonOther, User = userNikitaMaltsev, Comment = "It was а queue at the toilet" });

            //for 8 march
            absenceRepository.Create(new Absence { StartAbsence = new DateTime(2017, 4, 19, 9, 0, 0), EndAbsence = new DateTime(2017, 4, 19, 9, 25, 0), Date = new DateTime(2017, 4, 19), Reason = reasonOther, User = userNikitaMaltsev, Comment = "I don't want to explain(" });
            absenceRepository.Create(new Absence { StartAbsence = new DateTime(2017, 4, 19, 10, 15, 0), EndAbsence = new DateTime(2017, 4, 19, 10, 45, 0), Date = new DateTime(2017, 4, 19), Reason = reasonMeeting, User = userNikitaMaltsev });
            absenceRepository.Create(new Absence { StartAbsence = new DateTime(2017, 4, 19, 14, 0, 0), EndAbsence = new DateTime(2017, 4, 19, 14, 40, 0), Date = new DateTime(2017, 4, 19), Reason = reasonConsultation, User = userNikitaMaltsev });

            //for 9 march
            absenceRepository.Create(new Absence { StartAbsence = new DateTime(2017, 4, 20, 9, 40, 0), EndAbsence = new DateTime(2017, 4, 20, 9, 55, 0), Date = new DateTime(2017, 4, 20), Reason = reasonEnglish, User = userNikitaMaltsev });
            absenceRepository.Create(new Absence { StartAbsence = new DateTime(2017, 4, 20, 11, 25, 0), EndAbsence = new DateTime(2017, 4, 20, 11, 55, 0), Date = new DateTime(2017, 4, 20), Reason = reasonMeeting, User = userNikitaMaltsev });
            absenceRepository.Create(new Absence { StartAbsence = new DateTime(2017, 4, 20, 15, 5, 0), EndAbsence = new DateTime(2017, 4, 20, 15, 35, 0), Date = new DateTime(2017, 4, 20), Reason = reasonConsultation, User = userNikitaMaltsev });

            #endregion


            #region Add Abences and time to user ArtemChuhalo
            timeRepository.Create(new Time { Date = new DateTime(2017, 3, 5), TimeIn = new DateTime(2017, 3, 5, 8, 0, 0), TimeOut = new DateTime(2017, 3, 5, 16, 20, 0), User = userArtemChuhalo });
            timeRepository.Create(new Time { Date = new DateTime(2017, 3, 6), TimeIn = new DateTime(2017, 3, 6, 9, 0, 0), TimeOut = new DateTime(2017, 3, 6, 17, 20, 0), User = userArtemChuhalo });
            timeRepository.Create(new Time { Date = new DateTime(2017, 3, 7), TimeIn = new DateTime(2017, 3, 7, 8, 30, 0), TimeOut = new DateTime(2017, 3, 7, 16, 0, 0), User = userArtemChuhalo });
            timeRepository.Create(new Time { Date = new DateTime(2017, 3, 8), TimeIn = new DateTime(2017, 3, 8, 8, 50, 0), TimeOut = new DateTime(2017, 3, 8, 16, 40, 0), User = userArtemChuhalo });
            timeRepository.Create(new Time { Date = new DateTime(2017, 3, 9), TimeIn = new DateTime(2017, 3, 9, 8, 30, 0), TimeOut = new DateTime(2017, 3, 9, 18, 20, 0), User = userArtemChuhalo });

            // for 5 march
            absenceRepository.Create(new Absence { StartAbsence = new DateTime(2017, 3, 5, 9, 50, 0), EndAbsence = new DateTime(2017, 3, 5, 10, 20, 0), Date = new DateTime(2017, 3, 5), Reason = reasonMeeting, User = userArtemChuhalo });
            absenceRepository.Create(new Absence { StartAbsence = new DateTime(2017, 3, 5, 10, 50, 0), EndAbsence = new DateTime(2017, 3, 5, 11, 20, 0), Date = new DateTime(2017, 3, 5), Reason = reasonConsultation, User = userArtemChuhalo });
            absenceRepository.Create(new Absence { StartAbsence = new DateTime(2017, 3, 5, 15, 0, 0), EndAbsence = new DateTime(2017, 3, 5, 15, 25, 0), Date = new DateTime(2017, 3, 5), Reason = reasonConsultation, User = userArtemChuhalo });

            //for 6 march
            absenceRepository.Create(new Absence { StartAbsence = new DateTime(2017, 3, 6, 9, 10, 0), EndAbsence = new DateTime(2017, 3, 6, 9, 40, 0), Date = new DateTime(2017, 3, 6), Reason = reasonConsultation, User = userArtemChuhalo });
            absenceRepository.Create(new Absence { StartAbsence = new DateTime(2017, 3, 6, 11, 0, 0), EndAbsence = new DateTime(2017, 3, 6, 11, 35, 0), Date = new DateTime(2017, 3, 6), Reason = reasonMeeting, User = userArtemChuhalo });
            absenceRepository.Create(new Absence { StartAbsence = new DateTime(2017, 3, 6, 13, 0, 0), EndAbsence = new DateTime(2017, 3, 6, 14, 20, 0), Date = new DateTime(2017, 3, 6), Reason = reasonConsultation, User = userArtemChuhalo });
            absenceRepository.Create(new Absence { StartAbsence = new DateTime(2017, 3, 6, 15, 0, 0), EndAbsence = new DateTime(2017, 3, 6, 15, 20, 0), Date = new DateTime(2017, 3, 6), Reason = reasonEnglish, User = userArtemChuhalo });

            //for 7 march
            absenceRepository.Create(new Absence { StartAbsence = new DateTime(2017, 3, 7, 9, 20, 0), EndAbsence = new DateTime(2017, 3, 7, 9, 30, 0), Date = new DateTime(2017, 3, 7), Reason = reasonMeeting, User = userArtemChuhalo });
            absenceRepository.Create(new Absence { StartAbsence = new DateTime(2017, 3, 7, 10, 50, 0), EndAbsence = new DateTime(2017, 3, 7, 11, 5, 0), Date = new DateTime(2017, 3, 7), Reason = reasonConsultation, User = userArtemChuhalo });
            absenceRepository.Create(new Absence { StartAbsence = new DateTime(2017, 3, 7, 12, 0, 0), EndAbsence = new DateTime(2017, 3, 7, 12, 20, 0), Date = new DateTime(2017, 3, 7), Reason = reasonConsultation, User = userArtemChuhalo });
            absenceRepository.Create(new Absence { StartAbsence = new DateTime(2017, 3, 7, 15, 0, 0), EndAbsence = new DateTime(2017, 3, 7, 15, 15, 0), Date = new DateTime(2017, 3, 7), Reason = reasonOther, User = userArtemChuhalo, Comment = "It was а queue at the toilet" });

            //for 8 march
            absenceRepository.Create(new Absence { StartAbsence = new DateTime(2017, 3, 8, 9, 0, 0), EndAbsence = new DateTime(2017, 3, 8, 9, 25, 0), Date = new DateTime(2017, 3, 8), Reason = reasonOther, User = userArtemChuhalo, Comment = "I don't want to explain(" });
            absenceRepository.Create(new Absence { StartAbsence = new DateTime(2017, 3, 8, 10, 15, 0), EndAbsence = new DateTime(2017, 3, 8, 10, 45, 0), Date = new DateTime(2017, 3, 8), Reason = reasonMeeting, User = userArtemChuhalo });
            absenceRepository.Create(new Absence { StartAbsence = new DateTime(2017, 3, 8, 14, 0, 0), EndAbsence = new DateTime(2017, 3, 8, 14, 40, 0), Date = new DateTime(2017, 3, 8), Reason = reasonConsultation, User = userArtemChuhalo });

            //for 9 march
            absenceRepository.Create(new Absence { StartAbsence = new DateTime(2017, 3, 9, 9, 40, 0), EndAbsence = new DateTime(2017, 3, 9, 9, 55, 0), Date = new DateTime(2017, 3, 9), Reason = reasonConsultation, User = userArtemChuhalo });
            absenceRepository.Create(new Absence { StartAbsence = new DateTime(2017, 3, 9, 11, 25, 0), EndAbsence = new DateTime(2017, 3, 9, 11, 55, 0), Date = new DateTime(2017, 3, 9), Reason = reasonMeeting, User = userArtemChuhalo });
            absenceRepository.Create(new Absence { StartAbsence = new DateTime(2017, 3, 9, 15, 5, 0), EndAbsence = new DateTime(2017, 3, 9, 15, 35, 0), Date = new DateTime(2017, 3, 9), Reason = reasonConsultation, User = userArtemChuhalo });

            #endregion



            #region Add absences and times for AlexandraMorozova

            timeRepository.Create(new Time { Date = new DateTime(2017, 3, 5), TimeIn = new DateTime(2017, 3, 5, 8, 0, 0), TimeOut = new DateTime(2017, 3, 5, 16, 20, 0), User = userAlexandraMorozova });
            timeRepository.Create(new Time { Date = new DateTime(2017, 3, 6), TimeIn = new DateTime(2017, 3, 6, 9, 0, 0), TimeOut = new DateTime(2017, 3, 6, 17, 20, 0), User = userAlexandraMorozova });
            timeRepository.Create(new Time { Date = new DateTime(2017, 3, 7), TimeIn = new DateTime(2017, 3, 7, 8, 30, 0), TimeOut = new DateTime(2017, 3, 7, 16, 0, 0), User = userAlexandraMorozova });
            timeRepository.Create(new Time { Date = new DateTime(2017, 3, 8), TimeIn = new DateTime(2017, 3, 8, 8, 50, 0), TimeOut = new DateTime(2017, 3, 8, 16, 40, 0), User = userAlexandraMorozova });
            timeRepository.Create(new Time { Date = new DateTime(2017, 3, 9), TimeIn = new DateTime(2017, 3, 9, 9, 5, 0), TimeOut = new DateTime(2017, 3, 9, 18, 20, 0), User = userAlexandraMorozova });

            // for 5 march
            absenceRepository.Create(new Absence { StartAbsence = new DateTime(2017, 3, 5, 9, 50, 0), EndAbsence = new DateTime(2017, 3, 5, 10, 20, 0), Date = new DateTime(2017, 3, 5), Reason = reasonMeeting, User = userAlexandraMorozova });
            absenceRepository.Create(new Absence { StartAbsence = new DateTime(2017, 3, 5, 10, 50, 0), EndAbsence = new DateTime(2017, 3, 5, 11, 20, 0), Date = new DateTime(2017, 3, 5), Reason = reasonConsultation, User = userAlexandraMorozova });
            absenceRepository.Create(new Absence { StartAbsence = new DateTime(2017, 3, 5, 15, 0, 0), EndAbsence = new DateTime(2017, 3, 5, 15, 25, 0), Date = new DateTime(2017, 3, 5), Reason = reasonConsultation, User = userAlexandraMorozova });

            //for 6 march
            absenceRepository.Create(new Absence { StartAbsence = new DateTime(2017, 3, 6, 9, 10, 0), EndAbsence = new DateTime(2017, 3, 6, 9, 40, 0), Date = new DateTime(2017, 3, 6), Reason = reasonConsultation, User = userAlexandraMorozova });
            absenceRepository.Create(new Absence { StartAbsence = new DateTime(2017, 3, 6, 11, 0, 0), EndAbsence = new DateTime(2017, 3, 6, 11, 35, 0), Date = new DateTime(2017, 3, 6), Reason = reasonMeeting, User = userAlexandraMorozova });
            absenceRepository.Create(new Absence { StartAbsence = new DateTime(2017, 3, 6, 13, 0, 0), EndAbsence = new DateTime(2017, 3, 6, 14, 20, 0), Date = new DateTime(2017, 3, 6), Reason = reasonConsultation, User = userAlexandraMorozova });
            absenceRepository.Create(new Absence { StartAbsence = new DateTime(2017, 3, 6, 15, 0, 0), EndAbsence = new DateTime(2017, 3, 6, 15, 20, 0), Date = new DateTime(2017, 3, 6), Reason = reasonEnglish, User = userAlexandraMorozova });

            //for 7 march
            absenceRepository.Create(new Absence { StartAbsence = new DateTime(2017, 3, 7, 9, 20, 0), EndAbsence = new DateTime(2017, 3, 7, 9, 30, 0), Date = new DateTime(2017, 3, 7), Reason = reasonMeeting, User = userAlexandraMorozova });
            absenceRepository.Create(new Absence { StartAbsence = new DateTime(2017, 3, 7, 10, 50, 0), EndAbsence = new DateTime(2017, 3, 7, 11, 5, 0), Date = new DateTime(2017, 3, 7), Reason = reasonConsultation, User = userAlexandraMorozova });
            absenceRepository.Create(new Absence { StartAbsence = new DateTime(2017, 3, 7, 12, 0, 0), EndAbsence = new DateTime(2017, 3, 7, 12, 20, 0), Date = new DateTime(2017, 3, 7), Reason = reasonConsultation, User = userAlexandraMorozova });
            absenceRepository.Create(new Absence { StartAbsence = new DateTime(2017, 3, 7, 15, 0, 0), EndAbsence = new DateTime(2017, 3, 7, 15, 15, 0), Date = new DateTime(2017, 3, 7), Reason = reasonOther, User = userAlexandraMorozova, Comment = "It was а queue at the toilet" });

            //for 8 march
            absenceRepository.Create(new Absence { StartAbsence = new DateTime(2017, 3, 8, 9, 0, 0), EndAbsence = new DateTime(2017, 3, 8, 9, 25, 0), Date = new DateTime(2017, 3, 8), Reason = reasonOther, User = userAlexandraMorozova, Comment = "I don't want to explain(" });
            absenceRepository.Create(new Absence { StartAbsence = new DateTime(2017, 3, 8, 10, 15, 0), EndAbsence = new DateTime(2017, 3, 8, 10, 45, 0), Date = new DateTime(2017, 3, 8), Reason = reasonMeeting, User = userAlexandraMorozova });
            absenceRepository.Create(new Absence { StartAbsence = new DateTime(2017, 3, 8, 14, 0, 0), EndAbsence = new DateTime(2017, 3, 8, 14, 40, 0), Date = new DateTime(2017, 3, 8), Reason = reasonConsultation, User = userAlexandraMorozova });

            //for 9 march
            absenceRepository.Create(new Absence { StartAbsence = new DateTime(2017, 3, 9, 9, 40, 0), EndAbsence = new DateTime(2017, 3, 9, 9, 55, 0), Date = new DateTime(2017, 3, 9), Reason = reasonConsultation, User = userAlexandraMorozova });
            absenceRepository.Create(new Absence { StartAbsence = new DateTime(2017, 3, 9, 11, 25, 0), EndAbsence = new DateTime(2017, 3, 9, 11, 55, 0), Date = new DateTime(2017, 3, 9), Reason = reasonMeeting, User = userAlexandraMorozova });
            absenceRepository.Create(new Absence { StartAbsence = new DateTime(2017, 3, 9, 15, 5, 0), EndAbsence = new DateTime(2017, 3, 9, 15, 35, 0), Date = new DateTime(2017, 3, 9), Reason = reasonConsultation, User = userAlexandraMorozova });



            #endregion
            #endregion
            //Repository<UserLogin> UserLoginRepository = new Repository<UserLogin>(appContext);
            //UserLoginRepository.Create(new UserLogin { Login = "svezho" });
            //UserLoginRepository.Create(new UserLogin { Login = "Alexandr" });
            base.Seed(context);
        }
    }
 
}