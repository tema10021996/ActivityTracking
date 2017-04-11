using Microsoft.AspNet.Identity.EntityFramework;

namespace ActivityTracking.DomainModel
{
    public class ApplicationUser : IdentityUser
    {

        public int? GroupId { get; set; }
        public virtual Group Group { get; set; }

        public ApplicationUser()
        {

        }
    }
}
