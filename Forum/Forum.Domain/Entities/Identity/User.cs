using Forum.Domain.Common;

namespace Forum.Domain.Entities.Identity
{
    public class User : EntityBase
    {
        public string Nickname { get; set; }

        public string Email { get; set; }

        public string Bio { get; set; }

        public int Reputation { get; set; }

        public string PasswordHash { get; set; }

        public RefreshToken RefreshToken { get; set; }
    }
}
