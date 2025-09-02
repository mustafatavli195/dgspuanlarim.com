using System.ComponentModel.DataAnnotations;

namespace dgspuanlarim.Models
{
    public class ContactFormModel
    {
        [Required(ErrorMessage = "Adınızı giriniz")]
        [StringLength(50, ErrorMessage = "Adınız en fazla 50 karakter olabilir")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Email adresinizi giriniz")]
        [EmailAddress(ErrorMessage = "Geçerli bir email giriniz")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Mesajınızı yazınız")]
        [StringLength(500, ErrorMessage = "Mesaj 500 karakteri aşamaz")]
        public string Message { get; set; }
    }
}
