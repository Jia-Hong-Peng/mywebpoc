using SecretManagementPortal.Blob;

var builder = WebApplication.CreateBuilder(args);

// �q�����ܼƤ�����s�u�r��A�p�G�����ܼƤ��s�b�h�^�h��t�m���
var connectionString = Environment.GetEnvironmentVariable("StorageConnectionString") ?? builder.Configuration["AzureStorageConfig:ConnectionString"];
var containerName = builder.Configuration["AzureStorageConfig:ContainerName"];

var blobService = new BlobService(
            connectionString,
            containerName
          );    

builder.Services.AddScoped<BlobService>(serviceProvider => blobService);

builder.WebHost.ConfigureKestrel(options =>
{
    options.Limits.MaxRequestBodySize = 52428800; // �Ҧp�A�o�̳]�m��50MB
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

app.MapControllers(); // �T�O������ѳQ���U

app.Run();

