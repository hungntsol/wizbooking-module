﻿using Identity.Domain.Common;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Persistence.EfCore.Internal;

namespace Identity.Domain.Entities;

[Index(nameof(Email), Name = "IX_User_Email", IsUnique = true)]
[Index(nameof(LastName), Name = "IX_User_LastName", IsUnique = false)]
[Index(nameof(FirstName), Name = "IX_User_FirstName", IsUnique = false)]
[Index(nameof(Role), Name = "IX_User_Role", IsUnique = false)]
public class UserAccount : EntityBase<ulong>
{
	public UserAccount(string email,
		string normalizeEmail,
		string firstName,
		string lastName,
		string? bio,
		DateTime? dateOfBirth,
		string? phoneNumber,
		string? address,
		string gender,
		string? imageUrl,
		string role,
		DateTime? lastLogin,
		string? lastLoginIpv4,
		string? lastLoginIpv6)
	{
		Email = email;
		NormalizeEmail = normalizeEmail ?? email.ToUpper();
		FirstName = firstName;
		LastName = lastName;
		Bio = bio;
		DateOfBirth = dateOfBirth;
		PhoneNumber = phoneNumber;
		Address = address;
		Gender = gender;
		ImageUrl = imageUrl;
		Role = role;
		LastLogin = lastLogin;
		LastLoginIpv4 = lastLoginIpv4;
		LastLoginIpv6 = lastLoginIpv6;
	}

	internal UserAccount()
	{
	}

	public string Email { get; set; } = null!;
	public string NormalizeEmail { get; set; } = null!;
	public string HashPassword { get; set; } = null!;
	public string FirstName { get; set; } = null!;
	public string LastName { get; set; } = null!;
	public string? Bio { get; set; }
	public DateTime? DateOfBirth { get; set; }
	public string? PhoneNumber { get; set; }
	public string? Address { get; set; }
	public string Gender { get; set; } = null!;
	public string? ImageUrl { get; set; }
	public bool IsEmailVerified { get; set; }
	public bool IsActive { get; set; }
	public bool IsLocked { get; set; }
	public string Role { get; set; } = null!;
	public DateTime? LastLogin { get; set; }
	public string? LastLoginIpv4 { get; set; }
	public string? LastLoginIpv6 { get; set; }

	public string Fullname => $"{FirstName} {LastName}";

	/// <summary>
	/// Create a basic user instance. All of this information will not be changed overtime
	/// </summary>
	/// <param name="email"></param>
	/// <param name="rawPassword"></param>
	/// <param name="firstName"></param>
	/// <param name="lastName"></param>
	/// <param name="gender"></param>
	/// <param name="role"></param>
	/// <returns></returns>
	public static UserAccount New(string email, string rawPassword, string firstName, string lastName, string gender,
		string role)
	{
		var user = new UserAccount
		{
			Email = email,
			NormalizeEmail = email.ToUpper(),
			FirstName = firstName,
			LastName = lastName
		};

		user.SetPassword(rawPassword);
		user.Gender = UserGenderTypes.IsDefined(gender) ? gender : UserGenderTypes.DEFAULT;
		user.Role = UserRoleTypes.IsDefined(role) ? role : UserRoleTypes.DEFAULT;

		return user;
	}

	/// <summary>
	/// Set hashPassword for user
	/// </summary>
	/// <param name="raw"></param>
	public void SetPassword(string raw)
	{
		var hashPassword = new PasswordHasher<UserAccount>().HashPassword(this, raw);
		HashPassword = hashPassword;
	}

	/// <summary>
	/// Validate raw password with hash one
	/// </summary>
	/// <param name="raw"></param>
	/// <returns></returns>
	public bool ValidatePassword(string raw)
	{
		var verify = new PasswordHasher<UserAccount>().VerifyHashedPassword(this, HashPassword, raw);
		return verify == PasswordVerificationResult.Success;
	}

	/// <summary>
	/// Activate account
	/// </summary>
	public void Activate()
	{
		IsActive = true;
		ConfirmEmail();
	}

	/// <summary>
	/// DeActive account
	/// </summary>
	public void DeActive()
	{
		IsActive = false;
	}

	/// <summary>
	/// Lock account
	/// </summary>
	public void Lock()
	{
		IsLocked = true;
	}

	public void UnLock()
	{
		IsLocked = false;
	}

	/// <summary>
	/// Email is confirm
	/// </summary>
	public void ConfirmEmail()
	{
		IsEmailVerified = true;
	}

	/// <summary>
	/// Whether this account is valid for end-user
	/// </summary>
	/// <returns></returns>
	public bool IsValid()
	{
		return IsEmailVerified && IsActive && !IsLocked;
	}

	public bool HasRole(string role)
	{
		return Role.Equals(role);
	}
}
