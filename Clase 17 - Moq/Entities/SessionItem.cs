using Entities.User;

namespace Entities
{
    public class SessionItem
    {
        public long Id { get; set; }
        public long UserId { get; set; }
        public virtual UserItem? User { get; set; }
        public DateTime OpenedAt { get; set; }
    }
}
