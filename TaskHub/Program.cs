using Microsoft.EntityFrameworkCore;
using TaskHub.Data;
using TaskHub.Mapper;
using TaskHub.Middleware;
using TaskHub.Models;
using TaskHub.Provider;
using TaskHub.Services;
using TaskHub.Services.Interfaces;

var builder = WebApplication.CreateBuilder(args);

var services = builder.Services;

services.AddHttpContextAccessor();

services.AddScoped<IUsuarioProvider, UsuarioProvider>();
services.AddScoped<IUsuarioService, UsuarioService>();
services.AddScoped<IProjetoService, ProjetoService>();
services.AddScoped<ITarefaService, TarefaService>();
services.AddScoped<IHistoricoService, HistoricoService>();
services.AddScoped<IComentarioService, ComentarioService>();

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