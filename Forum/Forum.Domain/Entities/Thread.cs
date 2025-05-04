using Forum.Domain.Common;
using Forum.Domain.Entities.Identity;

namespace Forum.Domain.Entities
{
    public class Thread : EntityBase
    {
        public string Name { get; set; }

        public DateTime DatePosted { get; set; }

        public int Rating { get; set; } = 0;

        public User Author { get; set; }
    }
}
