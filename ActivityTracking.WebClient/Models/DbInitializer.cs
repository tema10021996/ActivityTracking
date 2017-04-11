using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Data.Entity;
using ActivityTracking.DAL.EntityFramework;
using ActivityTracking.DomainModel;
using System;

namespace ActivityTracking.WebClient.Models
{
    public class DbInitializer :  DropCreateDatabaseIfModelChanges<ApplicationContext>
    {
        protected override void Seed(ApplicationContext context)
        {
            var userManager = new ApplicationUserManager(new UserStore<ApplicationUser>(context));

            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));

            // создаем две роли
            var role1 = new IdentityRole { Name = "admin" };
            var role2 = new IdentityRole { Name = "user" };
            var role3 = new IdentityRole { Name = "manager" };

            // добавляем роли в бд
            roleManager.Create(role1);
            roleManager.Create(role2);
            roleManager.Create(role3);

            // создаем админа
            var admin = new ApplicationUser { UserName = "Admin" };
            string password = "123456";
            var result = userManager.Create(admin, password);

            
            if (result.Succeeded)
            {
                // добавляем для админа роль
                userManager.AddToRole(admin.Id, role1.Name);
                userManager.AddToRole(admin.Id, role2.Name);
                userManager.AddToRole(admin.Id, role3.Name);
            }

            ApplicationContext appContext = new ApplicationContext();
            Repository<UserLogin> UserLoginRepository = new Repository<UserLogin>(appContext);
            UserLoginRepository.Create(new UserLogin { Login = "svezho" });
            UserLoginRepository.Create(new UserLogin { Login = "Alexandr" });

            Repository<Reason> reasonRepository = new Repository<Reason>(appContext);
            reasonRepository.Create(new Reason { Name = "Meeting", AddingTime = DateTime.Now });
            reasonRepository.Create(new Reason { Name = "Consultation", AddingTime = DateTime.Now });
            reasonRepository.Create(new Reason { Name = "English", AddingTime = DateTime.Now });
            reasonRepository.Create(new Reason { Name = "Worked without PC", AddingTime = DateTime.Now });
            reasonRepository.Create(new Reason { Name = "Other", AddingTime = DateTime.Now });

            Repository<Group> groupRepository = new Repository<Group>(appContext);
            groupRepository.Create(new Group { Name = "Group1", MayAbsentTime = new TimeSpan(00,01,00)});
            groupRepository.Create(new Group { Name = "Group2", MayAbsentTime = new TimeSpan(00, 02, 00) });
            groupRepository.Create(new Group { Name = "Group3", MayAbsentTime = new TimeSpan(00, 03, 00) });
            groupRepository.Create(new Group { Name = "Group4", MayAbsentTime = new TimeSpan(00, 04, 00) });

            base.Seed(context);
        }
    }
 
}