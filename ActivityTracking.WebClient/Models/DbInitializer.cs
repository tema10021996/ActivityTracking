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
            reasonRepository.Create(new Reason { Name = "Meeting", AddingTime = DateTime.Now });
            reasonRepository.Create(new Reason { Name = "Consultation", AddingTime = DateTime.Now });
            reasonRepository.Create(new Reason { Name = "English", AddingTime = DateTime.Now });
            reasonRepository.Create(new Reason { Name = "Worked without PC", AddingTime = DateTime.Now });
            reasonRepository.Create(new Reason { Name = "Other", AddingTime = DateTime.Now });
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
            group1.Reasons.Add(reasonRepository.GetList().First(r => r.Name == "Worked without PC"));
            group1.Reasons.Add(reasonRepository.GetList().First(r => r.Name == "Other"));
            groupRepository.Update(group1);

            Group group2 = groupRepository.GetList().First(g => g.Name == "Group2");
            group2.Reasons.Add(reasonRepository.GetList().First(r => r.Name == "Meeting"));
            group2.Reasons.Add(reasonRepository.GetList().First(r => r.Name == "Consultation"));
            group2.Reasons.Add(reasonRepository.GetList().First(r => r.Name == "English"));
            group2.Reasons.Add(reasonRepository.GetList().First(r => r.Name == "Worked without PC"));
            group2.Reasons.Add(reasonRepository.GetList().First(r => r.Name == "Other"));
            groupRepository.Update(group2);

            Group group3 = groupRepository.GetList().First(g => g.Name == "Group3");
            group3.Reasons.Add(reasonRepository.GetList().First(r => r.Name == "Meeting"));
            group3.Reasons.Add(reasonRepository.GetList().First(r => r.Name == "Consultation"));
            group3.Reasons.Add(reasonRepository.GetList().First(r => r.Name == "English"));
            group3.Reasons.Add(reasonRepository.GetList().First(r => r.Name == "Worked without PC"));
            group3.Reasons.Add(reasonRepository.GetList().First(r => r.Name == "Other"));
            groupRepository.Update(group3);

            Group group4 = groupRepository.GetList().First(g => g.Name == "Group4");
            group4.Reasons.Add(reasonRepository.GetList().First(r => r.Name == "Meeting"));
            group4.Reasons.Add(reasonRepository.GetList().First(r => r.Name == "Consultation"));
            group4.Reasons.Add(reasonRepository.GetList().First(r => r.Name == "English"));
            group4.Reasons.Add(reasonRepository.GetList().First(r => r.Name == "Worked without PC"));
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

            #region Add Absences to user Alexandr

            //Reasons
            var reasonMeeting = reasonRepository.GetList().First(r => r.Name == "Meeting");
            var reasonConsultation = reasonRepository.GetList().First(r => r.Name == "Consultation");
            var reasonEnglish = reasonRepository.GetList().First(r => r.Name == "English");
            var reasonWorkedWithoutPC = reasonRepository.GetList().First(r => r.Name == "Worked without PC");
            var reasonOther = reasonRepository.GetList().First(r => r.Name == "Other");
            //users
            var userNikitaMaltsev = applicationUserRepository.GetList().First(u => u.UserName == "NikitaMaltsev");
            var userAlexandrTkachuk = applicationUserRepository.GetList().First(u => u.UserName == "AlexandrTkachuk");
            var userArtemChuhalo = applicationUserRepository.GetList().First(u => u.UserName == "ArtemChuhalo");
            var userAlexandraMorozova = applicationUserRepository.GetList().First(u => u.UserName == "AlexandraMorozova");
            //Add Absences
            Repository<Absenсe> absenceRepository = new Repository<Absenсe>(context);
            Absenсe absense1 = new Absenсe { StartAbsence = new DateTime(2017, 3, 5, 0, 0, 0), EndAbsence = new DateTime(2017, 3, 5, 8, 15, 0), Date = new DateTime(2017, 3, 5), Reason =  };

            //#endregion
            //Repository<UserLogin> UserLoginRepository = new Repository<UserLogin>(appContext);
            //UserLoginRepository.Create(new UserLogin { Login = "svezho" });
            //UserLoginRepository.Create(new UserLogin { Login = "Alexandr" });
            base.Seed(context);
        }
    }
 
}