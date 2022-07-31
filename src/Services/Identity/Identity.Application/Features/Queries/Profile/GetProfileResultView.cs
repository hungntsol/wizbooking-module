namespace Identity.Application.Features.Queries.Profile;
public class GetProfileResultView
{
    public string Email { get; set; } = null!;
    public string NormalizeEmail { get; set; } = null!;
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public string Fullname { get; set; } = null!;
    public string? Bio { get; set; }
    public DateTime? DateOfBirth { get; set; }
    public string? PhoneNumber { get; set; }
    public string? Address { get; set; }
    public string Gender { get; set; } = null!;
    public string? ImageUrl { get; set; }
    public DateTime? LastLogin { get; set; }
}
