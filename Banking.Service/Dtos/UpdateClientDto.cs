using Banking.Data.Models;
using System.ComponentModel.DataAnnotations;

namespace Banking.Service.Dtos;

public class UpdateClientDto
{
    [Required]
    [StringLength(60, ErrorMessage = "First name cannot be longer than 60 characters.")]
    public string FirstName { get; set; }

    [Required]
    [StringLength(60, ErrorMessage = "Last name cannot be longer than 60 characters.")]
    public string LastName { get; set; }

    [Required]
    public string ProfilePhoto { get; set; }

    [Required]
    public string MobileNumber { get; set; }

}
