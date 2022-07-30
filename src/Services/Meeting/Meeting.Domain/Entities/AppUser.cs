using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SharedCommon.Domain;

namespace Meeting.Domain.Entities;

[Index(nameof(Email), Name = "IX_User_Email", IsUnique = true)]
[Index(nameof(LastName), Name = "IX_User_LastName", IsUnique = false)]
[Index(nameof(FirstName), Name = "IX_User_FirstName", IsUnique = false)]
public class AppUser : EntityBase<ulong>
{
    public string Email { get; set; } = null!;
    public string NormalizeEmail { get; set; }
    public string HashPassword { get; set; } = null!;
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public string? Bio { get; set; }
    public DateTime? DateOfBirth { get; set; }
    public string? PhoneNumber { get; set; }
    public string? Address { get; set; }
    public string Gender { get; set; }
    public string? ImageUrl { get; set; }
    public bool IsEmailVerified { get; set; }
    public bool IsActive { get; set; }
    public bool IsLocked { get; set; }
    public DateTime? LastLogin { get; set; }
    public string? LastLoginIpv4 { get; set; }
    public string? LastLoginIpv6 { get; set; }

    public AppUser(string email,
        string rawPassword,
        string firstName,
        string lastName,
        string? bio = null,
        DateTime? dateOfBirth = null,
        string? phoneNumber = null,
        string? address = null,
        UserGender gender = default,
        string? imageUrl = null,
        bool isEmailVerified = false,
        DateTime? lastLogin = null,
        string? lastLoginIpv4 = null,
        string? lastLoginIpv6 = null) : this(email, rawPassword, firstName, lastName, gender)
    {
        Bio = bio;
        DateOfBirth = dateOfBirth;
        PhoneNumber = phoneNumber;
        Address = address;
        ImageUrl = imageUrl;
        IsEmailVerified = isEmailVerified;
        LastLogin = lastLogin;
        LastLoginIpv4 = lastLoginIpv4;
        LastLoginIpv6 = lastLoginIpv6;
    }

    public AppUser(string email, string rawPassword, string firstName, string lastName, UserGender gender)
    {
        Email = email;
        NormalizeEmail = email.ToUpper();
        FirstName = firstName;
        LastName = lastName;
        this.SetPassword(rawPassword);
        Gender = gender.Mapping();
    }

    public string Fullname => string.Format("{0} {1}", this.FirstName, this.LastName);

    /// <summary>
    /// Set hashPassword for user
    /// </summary>
    /// <param name="raw"></param>
    public void SetPassword(string raw)
    {
        var hashPassword = new PasswordHasher<AppUser>().HashPassword(this, raw);
        this.HashPassword = hashPassword;
    }

    /// <summary>
    /// Validate raw password with hash one
    /// </summary>
    /// <param name="raw"></param>
    /// <returns></returns>
    public bool ValidatePassword(string raw)
    {
        var verify = new PasswordHasher<AppUser>().VerifyHashedPassword(this, this.HashPassword, raw);
        return verify == PasswordVerificationResult.Success;
    }
}

public enum UserGender
{
    MALE,
    FEMALE,
    OTHER
}

public static class UserGenderExtension
{
    public static string Mapping(this UserGender gender)
    {
        return gender switch
        {
            UserGender.MALE => nameof(UserGender.MALE),
            UserGender.FEMALE => nameof(UserGender.FEMALE),
            UserGender.OTHER => nameof(UserGender.OTHER),
            _ => nameof(UserGender.MALE),
        };
    }
}
