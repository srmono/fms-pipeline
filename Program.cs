using FastEndpoints;
using Microsoft.EntityFrameworkCore;
using fleetmanagement.config;
using fleetmanagement.repositories;
using fleetmanagement.middleware;
using System.Net;
using System.Threading;

var builder = WebApplication.CreateBuilder(args);

//  Explicitly configure Kestrel to listen on port 5222
builder.WebHost.ConfigureKestrel(serverOptions =>
{
    serverOptions.ListenAnyIP(5222);
});


//  Read connection string from environment variable (Docker support)
var connectionString = Environment.GetEnvironmentVariable("ConnectionStrings__DefaultConnection") 
    ?? builder.Configuration.GetConnectionString("DefaultConnection");

//  Log connection string to verify it is set correctly
Console.WriteLine($"[DEBUG] Using Connection String: {connectionString}");

//  Add database context with MySQL and retry logic
builder.Services.AddDbContext<TruckDbContext>(options =>
    options.UseMySql(connectionString, new MySqlServerVersion(new Version(8, 0, 25)),
        mySqlOptions => mySqlOptions.EnableRetryOnFailure() //  Enables retries if MySQL isn't ready
    )
);

builder.Services.AddScoped<ITruckRepository, TruckRepository>();
builder.Services.AddFastEndpoints();  // Register FastEndpoints

var app = builder.Build();

//  Retry logic for database connection (handles MySQL readiness)
int maxRetries = 5;
for (int retry = 1; retry <= maxRetries; retry++)
{
    try
    {
        using (var scope = app.Services.CreateScope())
        {
            var dbContext = scope.ServiceProvider.GetRequiredService<TruckDbContext>();

            Console.WriteLine("[INFO] Dropping existing database...");
            dbContext.Database.EnsureDeleted();

            Console.WriteLine("[INFO] Creating new database...");
            dbContext.Database.EnsureCreated(); // Recreate DB every time for local development
            
            Console.WriteLine("[SUCCESS] Database setup completed.");
        }
        break;  //  Exit retry loop on success
    }
    catch (Exception ex)
    {
        Console.WriteLine($"[ERROR] Database setup failed. Retrying in 5 seconds... (Attempt {retry}/{maxRetries})");
        Console.WriteLine($"Exception: {ex.Message}");
        Thread.Sleep(5000);  // Wait before retrying
    }
}

//  Use global exception handler middleware
app.UseMiddleware<CustomExceptionHandlerMiddleware>();

//  Use FastEndpoints for handling API routes
app.UseFastEndpoints();

//  Start the application
app.Run();




// using FastEndpoints;
// using Microsoft.EntityFrameworkCore;
// using fleetmanagement.config;
// using fleetmanagement.repositories;
// using fleetmanagement.middleware;
// using System.Net;
// using System.Threading;

// var builder = WebApplication.CreateBuilder(args);

// //  Read connection string from environment variable (Docker support)
// var connectionString = Environment.GetEnvironmentVariable("ConnectionStrings__DefaultConnection") 
//     ?? builder.Configuration.GetConnectionString("DefaultConnection");

// //  Log connection string to verify it is set correctly
// Console.WriteLine($"[DEBUG] Using Connection String: {connectionString}");

// //  Add database context with MySQL
// builder.Services.AddDbContext<TruckDbContext>(options =>
//     options.UseMySql(connectionString, new MySqlServerVersion(new Version(8, 0, 25))));

// builder.Services.AddScoped<ITruckRepository, TruckRepository>();
// builder.Services.AddFastEndpoints();  // Register FastEndpoints

// var app = builder.Build();

// //  Retry logic for database connection (handles MySQL readiness)
// int maxRetries = 5;
// for (int retry = 1; retry <= maxRetries; retry++)
// {
//     try
//     {
//         using (var scope = app.Services.CreateScope())
//         {
//             var dbContext = scope.ServiceProvider.GetRequiredService<TruckDbContext>();

//             Console.WriteLine("[INFO] Dropping existing database...");
//             dbContext.Database.EnsureDeleted();

//             Console.WriteLine("[INFO] Creating new database...");
//             dbContext.Database.EnsureCreated(); // Recreate DB every time for local development
            
//             Console.WriteLine("[SUCCESS] Database setup completed.");
//         }
//         break;  //  Exit retry loop on success
//     }
//     catch (Exception ex)
//     {
//         Console.WriteLine($"[ERROR] Database setup failed. Retrying in 5 seconds... (Attempt {retry}/{maxRetries})");
//         Console.WriteLine($"Exception: {ex.Message}");
//         Thread.Sleep(5000);  // Wait before retrying
//     }
// }

// //  Use global exception handler middleware
// app.UseMiddleware<CustomExceptionHandlerMiddleware>();

// //  Use FastEndpoints for handling API routes
// app.UseFastEndpoints();

// //  Start the application
// app.Run();



// using FastEndpoints;
// using Microsoft.EntityFrameworkCore;
// using fleetmanagement.config;
// using fleetmanagement.repositories;
// using fleetmanagement.middleware;
// using System.Net;

// var builder = WebApplication.CreateBuilder(args);

// // Add services to the container
// builder.Services.AddDbContext<TruckDbContext>(options =>
//     options.UseMySql(builder.Configuration.GetConnectionString("DefaultConnection"),
//         new MySqlServerVersion(new Version(8, 0, 25))));

// builder.Services.AddScoped<ITruckRepository, TruckRepository>();
// builder.Services.AddFastEndpoints();  // Register FastEndpoints

// var app = builder.Build();

// // Ensure the database and tables are created automatically (for development environment)
// using (var scope = app.Services.CreateScope())
// {
//     var dbContext = scope.ServiceProvider.GetRequiredService<TruckDbContext>();
//     dbContext.Database.EnsureDeleted();
//     dbContext.Database.EnsureCreated(); // This will create the database and tables if they don't exist
// }

// // Use global exception handler middleware and FastEndpoints
// // app.UseMiddleware<CustomExceptionHandlerMiddleware>()  // Custom exception handler
// //    .UseFastEndpoints();  // Use FastEndpoints once to handle routes
// app.UseMiddleware<CustomExceptionHandlerMiddleware>();

// //app.UseDefaultExceptionHandler();
// app.UseFastEndpoints();

// // Run the application
// app.Run();
