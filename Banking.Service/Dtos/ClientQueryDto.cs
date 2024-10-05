using System.ComponentModel.DataAnnotations;
namespace Banking.Service.Dtos;

public class ClientQueryDto
{
    
    [StringLength(60, ErrorMessage = "SearchTerm cannot be longer than 60 characters.")]
    public string? SearchTerm { get; set; } 
    
    [StringLength(10, ErrorMessage = "sort by field cannot be longer than 10 characters.")]
    [RegularExpression("(?i)^(FirstName|LastName|Email)$", ErrorMessage = "Value must be either 'FirstName', 'LastName' or 'Email'.")]
    public string? SortBy { get; set; } = "FirstName";

    [StringLength(10, ErrorMessage = "Filter by field cannot be longer than 10 characters.")]
    [RegularExpression("(?i)^(FirstName|LastName|Email|MobileNumber)$", ErrorMessage = "Value must be either 'FirstName', 'LastName', 'Email', or 'MobileNumber'.")]
    public string? FilterBy { get; set; } = "all";

    [RegularExpression("(?i)^(Asc|Desc)$", ErrorMessage = "Value must be either 'asc' or 'desc'.")]
    public string? SortDirection { get; set; } = "Asc";

    [Range(1, int.MaxValue, ErrorMessage = "Page must be greater than 0.")]
    public int Page { get; set; } = 1; 

    [Range(1, int.MaxValue, ErrorMessage = "PageSize must be greater than 0.")]
    public int PageSize { get; set; } = 10; 
}
