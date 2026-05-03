using System.ComponentModel.DataAnnotations;

namespace PetFinder.Models;

public class User
{
    public int Id { get; set; }

    [Required, StringLength(50)]
    public string Username { get; set; } = string.Empty;

    [Required, StringLength(200)]
    public string Password { get; set; } = string.Empty;
}
