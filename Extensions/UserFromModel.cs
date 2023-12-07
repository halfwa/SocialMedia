using WebApp.Models.Entities.Users;
using WebApp.Models.ViewModels.Account;

namespace WebApp.Models.Users
{
    public static class UserFromModel
    {
        public static User Convert(this User user, UserEditViewModel userEditeVm)
        {
            user.Image = userEditeVm.Image;
            user.FirstName = userEditeVm.FirstName;
            user.LastName = userEditeVm.LastName;
            user.MiddleName = userEditeVm.MiddleName;
            user.Email = userEditeVm.Email;
            user.Status = userEditeVm.Status;
            user.About = userEditeVm.About;
            user.BirthDate = userEditeVm.BirthDate;
            
            return user;
        }
    }
}
