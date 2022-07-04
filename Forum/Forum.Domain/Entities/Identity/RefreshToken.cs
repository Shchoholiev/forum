namespace Forum.Domain.Entities.Identity
{
    public class RefreshToken
    {
        public string Token { get; set; }

        public DateTime ExpiryDate { get; set; }
    }
}
