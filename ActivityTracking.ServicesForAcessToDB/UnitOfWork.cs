using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ActivityTracking.DAL.EntityFramework;
using System.Data.Entity;
using ActivityTracking.DomainModel;

namespace ActivityTracking.ServicesForAcessToDB
{
    public class UnitOfWork
    {
        private ApplicationContext context = new ApplicationContext();
        private DBAcessService<Absence> absenceRepository;
        private DBAcessService<ApplicationUser> applicationUserRepository;
        private DBAcessService<DivisionManager> divisionManagerRepository;
        private DBAcessService<Reason> reasonRepository;
        private DBAcessService<WeekBeginningDay> weekBeginningDayRepository;

        public DBAcessService<Absence> AbsenceService
        {
            get
            {
                if (absenceRepository == null)
                    absenceRepository = new DBAcessService<Absence>(context);
                return absenceRepository;
            }
        }
        public DBAcessService<ApplicationUser> ApplicationUserService
        {
            get
            {
                if (applicationUserRepository == null)
                    applicationUserRepository = new DBAcessService<ApplicationUser>(context);
                return applicationUserRepository;
            }
        }
        public DBAcessService<DivisionManager> DivisionManagerService
        {
            get
            {
                if (divisionManagerRepository == null)
                    divisionManagerRepository = new DBAcessService<DivisionManager>(context);
                return divisionManagerRepository;
            }
        }
        public DBAcessService<Reason> ReasonService
        {
            get
            {
                if (reasonRepository == null)
                    reasonRepository = new DBAcessService<Reason>(context);
                return reasonRepository;
            }
        }
        public DBAcessService<WeekBeginningDay> WeekBeginningDayService
        {
            get
            {
                if (weekBeginningDayRepository == null)
                    weekBeginningDayRepository = new DBAcessService<WeekBeginningDay>(context);
                return weekBeginningDayRepository;
            }
        }
    }
}
