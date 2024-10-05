using System.ComponentModel.DataAnnotations;
namespace Banking.Data.Models;

public class Client
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Email is required.")]
    [EmailAddress(ErrorMessage = "Invalid email format.")]
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
    [RegularExpression(@"^\+\d{1,3}\s?\d{4,14}(?:x.+)?$", ErrorMessage = "Invalid mobile number format. Include country code.")]
    public string MobileNumber { get; set; }

    [Required(ErrorMessage = "Sex is required.")]
    [StringLength(6)]
    public string Sex { get; set; }

    public bool? IsDeleted { get; set; } = false;

    // Relationship with Address
    public int? AddressId { get; set; } = 1;
    public Address Address { get; set; }

    public List<Account> Accounts { get; set; }
}