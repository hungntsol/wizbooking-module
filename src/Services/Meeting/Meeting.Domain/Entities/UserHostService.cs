namespace Meeting.Domain.Entities;

[BsonCollection("userHostService")]
public class UserHostService : DocumentBase<string>
{
	public UserHostService(ulong hostId, string name, string? description, bool isActive)
	{
		HostId = hostId;
		Name = name;
		Description = description;
		IsActive = isActive;
	}

	public ulong HostId { get; set; }
	public string Name { get; set; }
	public string? Description { get; set; }
	public bool IsActive { get; set; }

	public bool IsValid()
	{
		return IsActive;
	}
}
