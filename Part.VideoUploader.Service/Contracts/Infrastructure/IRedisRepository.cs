namespace Part.VideoUploader.Service.Contracts.Infrastructure;

public interface IRedisRepository<T>
{
    public Task<T> Get(Guid id);
    public Task<bool> Set(Guid id, T @object);
    public Task<bool> Delete(Guid id);
}