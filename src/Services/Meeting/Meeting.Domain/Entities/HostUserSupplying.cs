using SharedCommon.Utilities;

namespace Meeting.Domain.Entities;

[BsonCollection("userHostSupply")]
public class HostUserSupplying : DocumentEntityBase<string>
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
	public List<string> Tags { get; set; } = Utils.List.Empty<string>();

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

	public static HostUserSupplying New(ulong hostId, string name, string? description, IList<string>? tags)
	{
		var newObj = new HostUserSupplying(hostId, name, description, true);
		if (tags is not null)
		{
			newObj.AppendTags(tags);
		}

		return newObj;
	}
}
