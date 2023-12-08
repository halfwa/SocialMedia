using System.ComponentModel.DataAnnotations;

namespace WebApp.Models.ViewModels.Account
{
    public class UserEditViewModel
    {
        [Display(Name = "Id")]
        public string UserId { get; set; }

        [Required(ErrorMessage = "Поле Имя обязательно для заполнения")]
        [DataType(DataType.Text)]
        [Display(Name = "Имя", Prompt = "Введите имя")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Поле Фамилия обязательно для заполнения")]
        [DataType(DataType.Text)]
        [Display(Name = "Фамилия", Prompt = "Введите фамилию")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Поле Отчество обязательно для заполнения")]
        [DataType(DataType.Text)]
        [Display(Name = "Отчество", Prompt = "Введите новое отчество")]
        public string MiddleName { get; set; }

        [Required(ErrorMessage = "Поле Дата рождения обязательно для заполнения")]
        public DateTime BirthDate { get; set; }

        [Required(ErrorMessage = "Поле Аватар обязательно для заполнения")]
        [Display(Name = "Аватар", Prompt = "Ссылка на аватарку")]
        public string Image { get; set; }

        [Required(ErrorMessage = "Поле Никнэйм обязательно для заполнения")]
        [Display(Name = "Никнэйм", Prompt = "Введите новый никнэйм")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Поле Email обязательно для заполнения")]
        [EmailAddress]
        [Display(Name = "Email", Prompt = "Введите новый Email")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Поле Статус обязательно для заполнения")]
        [Display(Name = "Статус", Prompt = "Новый статус")]
        public string Status { get; set; }

        [Required(ErrorMessage = "Поле Обо мне обязательно для заполнения")]
        [Display(Name = "Обо мне", Prompt = "Информация обо мне")]
        public string About { get; set; }

    }
}
