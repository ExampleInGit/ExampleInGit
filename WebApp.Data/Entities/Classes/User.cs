using WebApp.Data.Entities.Interfaces;

namespace WebApp.Data.Entities.Classes
{
    public class User : BaseEntity, ITrack, IDeletable
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
        public int Activated { get; set; }
        public bool IsDeleted { get; set; }
        public int RoleId { get; set; }
        public Role Role { get; set; }
    }
}
