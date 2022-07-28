namespace EFCore.Persistence.Abstracts
{
    internal interface IAsyncRepository<T> : IAsyncBaseRepository<T>, IAsyncReadRepository<T> where T : class
    {
    }
}
