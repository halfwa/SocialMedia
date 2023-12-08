using Microsoft.EntityFrameworkCore;
using WebApp.Data.ApplicationContext;
using WebApp.Models.Entities.Users;

namespace WebApp.Data.Repository
{
    public class MessageRepository: Repository<Message>
    {
        public MessageRepository(ApplicationDbContext db)
            : base(db) { }


        public List<Message> GetMessages(User sender, User recipient)
        {
            Set.Include(x => x.Sender);
            Set.Include(x => x.Recipient);

            var from = Set.AsEnumerable().Where(x => x.SenderId == sender.Id && x.RecipientId == recipient.Id).ToList();
            var to = Set.AsEnumerable().Where(x => x.SenderId == recipient.Id && x.RecipientId == sender.Id).ToList();

            var messages = new List<Message>(); 
            messages.AddRange(from);
            messages.AddRange(to);
            messages.OrderBy(x => x.Id);

            return messages;
        }
    }
}
    