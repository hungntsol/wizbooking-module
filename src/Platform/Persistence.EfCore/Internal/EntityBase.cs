using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using SharedCommon.Domain;

namespace Persistence.EfCore.Internal;

public abstract class EntityBase<TKey> : SupportPayloadEvent, IEntityBase<TKey>, ISupportPayloadEvent
{
	[Key]
	[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
	[Required]
	public TKey Id { get; set; } = default!;

	public DateTime CreatedAt { get; set; }
	public DateTime ModifiedAt { get; set; }
}
