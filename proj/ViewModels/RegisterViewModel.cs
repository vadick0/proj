using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace proj.ViewModels
{
    public class RegisterViewModel
    {
        [Required(ErrorMessage = "{0} обязательно для заполнения.")]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Год рождения обязателен для заполнения.")]
        [Display(Name = "Год рождения")]
        public int? Year { get; set; }

        [Required(ErrorMessage = "{0} обязательно для заполнения.")]
        [DataType(DataType.Password)]
        [StringLength(100, ErrorMessage = "Поле {0} должно иметь минимум {2} и максимум {1} символов.", MinimumLength = 8)]
        [Display(Name = "Пароль")]
        public string Password { get; set; }

        [Required(ErrorMessage = "{0} обязательно для продолжения регистрации.")]
        [Compare("Password", ErrorMessage = "Пароли не совпадают")]
        [DataType(DataType.Password)]
        [Display(Name = "Подтвердить пароль")]
        public string PasswordConfirm { get; set; }
    }
}
