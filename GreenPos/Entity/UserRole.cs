namespace GreenPOS.Entity
{
    public class UserRole : BaseEntity<long>
    {
        public long UserId { get; set; }
        public long RoleId { get; set; }
    }
}
