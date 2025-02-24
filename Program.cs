using Demo.Data;
using Demo.Interfaces;
using Demo.Repositories.LocalRepository;
using Demo.Repositories.RemoteRepository;
using Demo.Services;
using Hangfire;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddControllers();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAngularApp",
        policy =>
        {
            policy.WithOrigins("http://localhost:4200") 
                .AllowAnyMethod()
                .AllowAnyHeader();
        });
});

builder.Services.AddScoped<SynchronizationService>();

builder.Services.AddDbContext<RemoteDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddDbContext<LocalDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("SqliteConnection")));

builder.Services.AddHangfire(x => x.UseSqlServerStorage(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddHangfireServer();

builder.Services.AddMediatR(configuration =>
{
    configuration.RegisterServicesFromAssembly(typeof(Program).Assembly);
});


// Register repositories
builder.Services.AddScoped<IProductRepositoryLocal, LocalRepository>(); 
builder.Services.AddScoped<IProductRepositoryRemote, RemoteRepository>();

var app = builder.Build();

app.UseCors("AllowAngularApp");
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}


app.UseHttpsRedirection();
app.MapControllers();
app.UseHangfireDashboard();

app.Run();
