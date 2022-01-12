using System.ComponentModel.DataAnnotations;

namespace EmptyPlatform.Auth.Api.User
{
    public class UpdateRequest : CreateRequest
    {
        [Required]
        public string UserId { get; set; }

        public override Db.User Map() => new()
        {
            UserId = UserId,
            Email = Email,
            FirstName = FirstName,
            SecondName = SecondName,
            Birthday = Birthday
        };
    }
}
