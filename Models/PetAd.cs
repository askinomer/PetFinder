using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PetFinder.Models;

public class PetAd
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Evcil hayvan adı gereklidir."), StringLength(100)]
    [Display(Name = "İsim")]
    public string Name { get; set; } = string.Empty;

    [Required(ErrorMessage = "Tür bilgisi gereklidir."), StringLength(50)]
    [Display(Name = "Tür")]
    public string Species { get; set; } = string.Empty; // "Cat" or "Dog"

    [Required(ErrorMessage = "Şehir bilgisi gereklidir."), StringLength(100)]
    [Display(Name = "Şehir")]
    public string City { get; set; } = string.Empty;

    [StringLength(2000)]
    [Display(Name = "Açıklama")]
    public string Description { get; set; } = string.Empty;

    // VARBINARY(MAX) in SQL Server, byte[] in C#
    [Column(TypeName = "VARBINARY(MAX)")]
    public byte[]? ImageBytes { get; set; }
}
