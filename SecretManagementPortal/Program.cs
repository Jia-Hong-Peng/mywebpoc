using SecretManagementPortal.Blob;

var builder = WebApplication.CreateBuilder(args);

// 從環境變數中獲取連線字串，如果環境變數不存在則回退到配置文件
var connectionString = Environment.GetEnvironmentVariable("StorageConnectionString") ?? builder.Configuration["AzureStorageConfig:ConnectionString"];
var containerName = builder.Configuration["AzureStorageConfig:ContainerName"];

var blobService = new BlobService(
            connectionString,
            containerName
          );    

builder.Services.AddScoped<BlobService>(serviceProvider => blobService);

builder.WebHost.ConfigureKestrel(options =>
{
    options.Limits.MaxRequestBodySize = 52428800; // 例如，這裡設置為50MB
});


// Add services to the container.
builder.Services.AddRazorPages();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapRazorPages();

app.MapControllers(); // 確保控制器路由被註冊

app.Run();

