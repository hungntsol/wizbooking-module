namespace EFCore.Persistence.Abstracts;

public interface IAsyncRepository<T> : IAsyncBaseRepository<T>, IAsyncReadRepository<T> where T : class
{
}
