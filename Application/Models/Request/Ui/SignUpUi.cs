using System.ComponentModel.DataAnnotations;

namespace Application.Models.Request.Ui
{
    public class SignUpUi
    {
        [Required]
        public string Msisdn { get; set; }
        [Required]
        [MinLength(6)]
        public string Password { get; set; }
    }
}
