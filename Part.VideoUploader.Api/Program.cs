using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Part.VideoUploader.Infrastructure.Db;
using Part.VideoUploader.Infrastructure.Repository;
using Part.VideoUploader.Infrastructure.Storage;
using Part.VideoUploader.Service.Contracts.Infrastructure;
using Part.VideoUploader.Service.Features.Storage.Commands;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddMediatR(cfg =>
{
    cfg.RegisterServicesFromAssemblies(typeof(Program).Assembly, typeof(UploadFileCommandHandler).Assembly);
});
builder.Services.AddDbContext<VideoUploaderDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddDbContext<VideoUploaderDbContext>(options =>
    options.UseInMemoryDatabase("InMemoryPartVideoUploaderDb"));

builder.Services.AddScoped<IFileUploadInfoRepository, FileUploadInfoRepository>();
builder.Services.AddScoped<IStorageService, StorageService>();



builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo 
    { 
        Title = "Part Uploader API", 
        Version = "v1",
        Description = "An API for uploading and managing files"
    });
    
});

builder.Services.AddStackExchangeRedisCache(options =>
{
    options.Configuration = "localhost"; 
    options.InstanceName = "SampleInstance";
});


builder.Services.AddControllers();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/error"); 
}

app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
    c.RoutePrefix = string.Empty;
});

app.UseCors();

app.UseRouting();

app.UseAuthorization();

app.MapControllers();

app.Run();
