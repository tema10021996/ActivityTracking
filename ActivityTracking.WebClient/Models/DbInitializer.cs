using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Data.Entity;
using ActivityTracking.DAL.EntityFramework;
using ActivityTracking.DomainModel;
using System;
using System.Linq;

namespace ActivityTracking.WebClient.Models
{
    public class DbInitializer :  DropCreateDatabaseIfModelChanges<ApplicationContext>
    {
        protected override void Seed(ApplicationContext context)
        {
            var userManager = new ApplicationUserManager(new UserStore<ApplicationUser>(context));

            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));

            // Create roles
            var roleAdmin = new IdentityRole { Id = "1", Name = "admin" };
            var roleUser = new IdentityRole { Id = "2", Name = "user" };
            var rolemanager = new IdentityRole { Id = "3", Name = "manager" };

            // add roles to BD
            roleManager.Create(roleAdmin);
            roleManager.Create(roleUser);
            roleManager.Create(rolemanager);

            // Create Admin
            var admin = new ApplicationUser { UserName = "Admin" };
            string adminPassword = "123456";
            userManager.Create(admin, adminPassword);

            // Create role to Admin
            userManager.AddToRole(admin.Id, roleAdmin.Name);

            //Create User NikitaMaltsev
            var NikitaMaltsev = new ApplicationUser { UserName = "Nikita Maltsev" };
            string NikitaMaltsevPassword = "123456";
            userManager.Create(NikitaMaltsev, NikitaMaltsevPassword);
            userManager.AddToRole(NikitaMaltsev.Id, roleUser.Name);

            //Create User AlexandrTkachuk
            var AlexandrTkachuk = new ApplicationUser { UserName = "Alexandr Tkachuk" };
            string AlexandrTkachukPassword = "123456";
            userManager.Create(AlexandrTkachuk, AlexandrTkachukPassword);
            userManager.AddToRole(AlexandrTkachuk.Id, roleUser.Name);

            //Create User ArtemChuhalo
            var ArtemChuhalo = new ApplicationUser { UserName = "Artem Chuhalo" };
            string ArtemChuhaloPassword = "123456";
            userManager.Create(ArtemChuhalo, ArtemChuhaloPassword);
            userManager.AddToRole(ArtemChuhalo.Id, roleUser.Name);

            //Create manager AlexandraMorozova
            var AlexandraMorozova = new ApplicationUser { UserName = "Alexandra Morozova" };
            string AlexandraMorozovaPassword = "123456";
            userManager.Create(AlexandraMorozova, AlexandraMorozovaPassword);
            userManager.AddToRole(AlexandraMorozova.Id, rolemanager.Name);

            ApplicationContext appContext = new ApplicationContext();
            Repository<UserLogin> UserLoginRepository = new Repository<UserLogin>(appContext);
            UserLoginRepository.Create(new UserLogin { Login = "svezho" });
            UserLoginRepository.Create(new UserLogin { Login = "Alexandr" });
            //Add Reasons
            Repository<Reason> reasonRepository = new Repository<Reason>(appContext);
            reasonRepository.Create(new Reason { Name = "Meeting", AddingTime = DateTime.Now });
            reasonRepository.Create(new Reason { Name = "Consultation", AddingTime = DateTime.Now });
            reasonRepository.Create(new Reason { Name = "English", AddingTime = DateTime.Now });
            reasonRepository.Create(new Reason { Name = "Worked without PC", AddingTime = DateTime.Now });
            reasonRepository.Create(new Reason { Name = "Other", AddingTime = DateTime.Now });

            #region Add Groups
            //Add Groups
            Repository<Group> groupRepository = new Repository<Group>(appContext);
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

            base.Seed(context);
        }
    }
 
}