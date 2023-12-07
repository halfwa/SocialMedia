using WebApp.Models.Entities.Users;

namespace WebApp.Data
{
    public class GenerateUsers
    {
        public readonly string[] maleNames = new string[] { "Александро", "Борис", "Василий", "Игорь", "Даниил", "Сергей", "Евгений", "Алексей", "Геогрий", "Валентин" };
        public readonly string[] femaleNames = new string[] { "Анна", "Мария", "Станислава", "Елена" };
        public readonly string[] lastNames = new string[] { "Тестов", "Титов", "Потапов", "Джабаев", "Иванов" };

        public List<User> Populate(int count)
        {
            var users = new List<User>();
            for (int i = 1; i < count; i++)
            {
                string firstName;
                var rnd = new Random();

                var male = rnd.Next(1, 2) == 1;

                var lastName = lastNames[rnd.Next(0, lastNames.Length - 1)];
                if (male)
                {
                    firstName = maleNames[rnd.Next(0, maleNames.Length - 1)];
                }
                else
                {
                    lastName = lastName + "a";
                    firstName = femaleNames[rnd.Next(0, femaleNames.Length - 1)];
                }

                var item = new User()
                {
                    FirstName = firstName,
                    LastName = lastName,
                    BirthDate = DateTime.Now.AddDays(-rnd.Next(1, (DateTime.Now - DateTime.Now.AddYears(-25)).Days)).ToUniversalTime(),
                    Email = "test" + rnd.Next(0, 1204) + "@test.com",
                };

                item.UserName = item.Email;
                item.Image = "https://thispersondoesnotexist.com/";

                users.Add(item);
            }

            return users;
        }
    }
}
