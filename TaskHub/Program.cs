using Microsoft.EntityFrameworkCore;
using TaskHub.Data;
using TaskHub.DTO;
using TaskHub.Mapper;
using TaskHub.Middleware;
using TaskHub.Models;
using TaskHub.Provider;
using TaskHub.Services;

var builder = WebApplication.CreateBuilder(args);

var services = builder.Services;

services.AddHttpContextAccessor();

services.AddSingleton<UsuarioProvider>();

services.AddScoped<UsuarioService>();
services.AddScoped<ProjetoService>();
services.AddScoped<TarefaService>();
services.AddScoped<HistoricoService>();
services.AddScoped<ComentarioService>();

services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
services.Configure<AppSettings>(builder.Configuration.GetSection("AppSettings"));

services.AddEndpointsApiExplorer();
services.AddSwaggerGen();

services.AddAutoMapper(typeof(AutoMapperProfile));

services.AddRouting(options => options.LowercaseUrls = true);
services.AddControllers();

var app = builder.Build();

app.UseMiddleware<UsuarioMiddleware>();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapControllers();

app.Run();