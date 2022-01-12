namespace EmptyPlatform.Auth.Db.Entities
{
    public class Session
    {
        public int SessionId { get; set; }

        public string UserId { get; set; }

        public string Device { get; set; }
    }
}
