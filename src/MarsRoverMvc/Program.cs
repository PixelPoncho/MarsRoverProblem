using MarsRoverMvc.Services;

var builder = WebApplication.CreateBuilder(args);

// Add MVC services to support Controllers and Views
builder.Services.AddControllersWithViews();

// Register the HTTP client for calling the Web API
// This allows the MVC app to communicate with the Web Service
builder.Services.AddHttpClient<IRoverApiService, RoverApiService>(client =>
{
    // Configure the base address for API calls
    // In development, the API runs on port 5002
    client.BaseAddress = new Uri("https://localhost:5002");
    // Disable SSL verification for development (remove in production)
    client.DefaultRequestHeaders.Add("Accept", "application/json");
}).ConfigureHttpMessageHandlerBuilder(builder => {
     // Also register HttpClientHandler to ignore SSL for development

     builder.PrimaryHandler = new HttpClientHandler
    {
        ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => true
    };
});

var app = builder.Build();

// Configure the HTTP request pipeline
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

// Configure default route
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
