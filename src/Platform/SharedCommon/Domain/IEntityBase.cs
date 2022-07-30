namespace SharedCommon.Domain;
public interface IEntityBase<TKey>
{
    TKey Id { get; set; }

    DateTime CreatedAt { get; set; }

    DateTime ModifiedAt { get; set; }
}
