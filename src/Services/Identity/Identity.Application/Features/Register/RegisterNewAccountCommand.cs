namespace Identity.Application.Features.Register;
public class RegisterNewAccountCommand : IRequest<JsonHttpResponse<Unit>>
{
    public string Email { get; set; }
    public string Password { get; set; }
    public string Gender { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }

    public RegisterNewAccountCommand(string email, string password, string gender, string firstName, string lastName)
    {
        Email = email;
        Password = password;
        Gender = gender;
        FirstName = firstName;
        LastName = lastName;
    }
}
