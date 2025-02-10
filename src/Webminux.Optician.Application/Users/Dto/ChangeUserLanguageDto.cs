using System.ComponentModel.DataAnnotations;

namespace Webminux.Optician.Users.Dto
{
    public class ChangeUserLanguageDto
    {
        [Required]
        public string LanguageName { get; set; }
    }
}