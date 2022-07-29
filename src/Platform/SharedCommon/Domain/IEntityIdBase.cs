namespace SharedCommon.Domain;
public interface IEntityIdBase<TKey>
{
    TKey Id { get; set; }
}
