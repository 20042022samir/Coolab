using CoolApp.Core.Entities;
using System.ComponentModel.DataAnnotations;

namespace CoolAppProject.ViewModels
{
    public class RegisterViewModel
    {
        public bool IsStudent { get; set; }
        public string SchoolUniName { get; set; }
        public bool IsTeacher { get; set; }
        public bool IsEmployee { get; set; } 
        [Required]
        public string Name { get; set; }
        [Required]
        public string Surname { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        [DataType(DataType.Password)] public string Password { get; set; }
        [Required]
        [DataType(DataType.Password)]
        [Compare("Password")] 
        public string ConfirmPassword { get; set; }
        public string UserName { get; set; }
    }
}
