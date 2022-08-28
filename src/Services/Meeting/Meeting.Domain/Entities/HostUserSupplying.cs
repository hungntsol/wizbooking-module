namespace Meeting.Domain.Entities;

[BsonCollection("userHostSupply")]
public class HostUserSupplying : DocumentBase<string>
{
	public HostUserSupplying(ulong hostId, string name, string? description, bool isActive)
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
	public List<string> Tags { get; set; } = Array.Empty<string>().ToList();

	public bool IsValid()
	{
		return IsActive;
	}

	public void AppendTag(string tag)
	{
		Tags.Add(tag);
	}

	public void AppendTags(IList<string> tags)
	{
		Tags.AddRange(tags);
	}
}
