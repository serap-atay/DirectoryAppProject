using System.ComponentModel.DataAnnotations;

namespace DirectoryApp.ViewModels
{
    public class ContactViewModel
    {
        public int id { get; set; }
        [Required(ErrorMessage = "Ad alanı gereklidir.")]
        [Display(Name = "Ad")]
        [StringLength(50)]
        public string Name { get; set; }
        [Required(ErrorMessage = "Soyad alanı gereklidir.")]
        [Display(Name = "Soyad")]
        [StringLength(50)]
        public string Surname { get; set; }
        [Display(Name = "Telefon")]
        [Required(ErrorMessage = "Numara alanı gereklidir.")]
        public string Phone { get; set; }
    }
}
