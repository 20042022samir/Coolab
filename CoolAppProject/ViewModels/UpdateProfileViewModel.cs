using System.ComponentModel.DataAnnotations;

namespace CoolAppProject.ViewModels
{
    public class UpdateProfileViewModel
    {

        public string? Name { get; set; }
        public string? Surname { get; set; }
        public string? UserName { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string? NewPassword { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Compare("NewPassword")]
        public string? CurrecntPassword { get; set; }
    }
}
