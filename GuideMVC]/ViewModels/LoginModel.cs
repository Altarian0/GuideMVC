using System.ComponentModel.DataAnnotations;

namespace GuideMVC_.ViewModels
{
    public class LoginModel
    {
        [Required(ErrorMessage = "Не указан логин")]
        public string Login { get; set; }

        [Required(ErrorMessage = "Не указан пароль")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Display(Name = "Запомнить меня")]
        [DataType(DataType.Password)]
        public bool RememberMe { get; set; }
        public string ReturnUrl{ get; set; }
    }
}
