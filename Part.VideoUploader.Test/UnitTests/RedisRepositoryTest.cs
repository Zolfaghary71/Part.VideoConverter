using Xunit;
using Moq;
using Microsoft.Extensions.Caching.Distributed;
using System.Threading.Tasks;
using Part.VideoUploader.Infrastructure.Redis;
using System;
using System.Text.Json;

public class RedisRepositoryTests
{
    private class SampleEntity
    {
        public Guid Id { get; set; }
    }

    [Fact]
    public async Task Get_WhenCalled_ReturnsEntity()
    {
        var sampleEntity = new SampleEntity { Id = Guid.NewGuid() };
        var serializedEntity = JsonSerializer.SerializeToUtf8Bytes(sampleEntity);
        var mockCache = new Mock<IDistributedCache>();
        mockCache.Setup(x => x.GetAsync(sampleEntity.Id.ToString(), default))
                 .ReturnsAsync(serializedEntity);

        var repository = new RedisRepository<SampleEntity>(mockCache.Object);

        var result = await repository.Get(sampleEntity.Id);

        Assert.NotNull(result);
        Assert.Equal(sampleEntity.Id, result.Id);
    }

    [Fact]
    public async Task Set_WhenCalled_SavesEntity()
    {
        var sampleEntity = new SampleEntity { Id = Guid.NewGuid() };
        var mockCache = new Mock<IDistributedCache>();
        var repository = new RedisRepository<SampleEntity>(mockCache.Object);

        await repository.Set(sampleEntity.Id, sampleEntity);

        mockCache.Verify(x => x.SetAsync(sampleEntity.Id.ToString(), 
                                         It.IsAny<byte[]>(), 
                                         It.IsAny<DistributedCacheEntryOptions>(), 
                                         default), 
                                         Times.Once);
    }

    [Fact]
    public async Task Delete_WhenCalled_RemovesEntity()
    {
        var entityId = Guid.NewGuid();
        var mockCache = new Mock<IDistributedCache>();
        var repository = new RedisRepository<SampleEntity>(mockCache.Object);

        await repository.Delete(entityId);

        mockCache.Verify(x => x.RemoveAsync(entityId.ToString(), default), Times.Once);
    }
}
