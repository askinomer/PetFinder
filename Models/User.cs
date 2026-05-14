using System.ComponentModel.DataAnnotations;

namespace PetFinder.Models;

public class User
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Kullanıcı adı zorunludur.")]
    [StringLength(50)]
    [Display(Name = "Kullanıcı Adı")]
    public string Username { get; set; } = string.Empty;

    [Required(ErrorMessage = "Şifre zorunludur.")]
    [StringLength(200)]
    [Display(Name = "Şifre")]
    public string Password { get; set; } = string.Empty;
}
