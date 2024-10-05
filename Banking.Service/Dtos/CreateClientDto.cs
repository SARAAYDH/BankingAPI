using Banking.Data.Models;
using System.ComponentModel.DataAnnotations;

namespace Banking.Service.Dtos;

public class CreateClientDto
{
    [Required(ErrorMessage = "Email is required.")]
    [RegularExpression(@"^[a-zA-Z0-9]+@BankAPI\.COM$", ErrorMessage = "Invalid email format. Only letters and numbers are allowed, and the email must end with '@BankAPI.COM'.")]
    public string Email { get; set; }

    [Required(ErrorMessage = "First name is required.")]
    [StringLength(60, ErrorMessage = "First name cannot be longer than 60 characters.")]
    public string FirstName { get; set; }

    [Required(ErrorMessage = "Last name is required.")]
    [StringLength(60, ErrorMessage = "Last name cannot be longer than 60 characters.")]
    public string LastName { get; set; }

    [Required(ErrorMessage = "Personal ID is required.")]
    [StringLength(11, MinimumLength = 11, ErrorMessage = "Personal ID must be exactly 11 characters.")]
    public string PersonalId { get; set; }

    public string? ProfilePhoto { get; set; }

    [Required(ErrorMessage = "Mobile number is required.")]
    public string MobileNumber { get; set; }

    [Required(ErrorMessage = "Sex is required.")]
    [RegularExpression("(?i)^(Male|Female)$", ErrorMessage = "Value must be either 'Male' or 'Female'.")]
    public string Sex { get; set; }

    // Relationship with Address
    public int AddressId { get; set; } = 1;

    public List<AccountDto>? Accounts { get; set; }

}
