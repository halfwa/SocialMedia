using WebApp.Models.Entities.Users;

namespace WebApp.Models.ViewModels.Account
{
    public class UserWithFriendExt : User
    {
        public bool IsFriendWithCurrent { get; set; }
    }
}
