namespace WebApp.Models.Entities.Users
{
    public class Friend
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public User User { get; set; }

        public string CurrentFriendId { get; set; }

        public User CurrentFriend { get; set; }
    }
}
