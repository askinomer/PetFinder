using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PetFinder.Models;

public class PetAd
{
    public int Id { get; set; }

    [Required, StringLength(100)]
    [Display(Name = "Pet Name")]
    public string Name { get; set; } = string.Empty;

    [Required, StringLength(50)]
    [Display(Name = "Species")]
    public string Species { get; set; } = string.Empty; // "Cat" or "Dog"

    [Required, StringLength(100)]
    [Display(Name = "City")]
    public string City { get; set; } = string.Empty;

    [StringLength(2000)]
    [Display(Name = "Description")]
    public string Description { get; set; } = string.Empty;

    // VARBINARY(MAX) in SQL Server, byte[] in C#
    [Column(TypeName = "VARBINARY(MAX)")]
    public byte[]? ImageBytes { get; set; }
}
