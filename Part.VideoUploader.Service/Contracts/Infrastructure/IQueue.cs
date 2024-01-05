namespace Part.VideoUploader.Service.Contracts.Infrastructure;

public interface IQueue<T>
{
    Task EnqueueAsync(T item);
    Task<T?> DequeueAsync();
}