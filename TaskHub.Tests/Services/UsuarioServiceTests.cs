using Microsoft.EntityFrameworkCore;
using TaskHub.Data;
using TaskHub.Models;
using TaskHub.Services;

namespace TaskHub.Tests
{
    public class UsuarioServiceTests
    {
        [Fact]
        public void Obter_DeveRetornarUsuarioAtivo()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDb")
                .Options;

            using (var context = new ApplicationDbContext(options))
            {
                var usuarioService = new UsuarioService(context);

                var usuario = new Usuario
                {
                    UsuarioId = 1,
                    Funcao = "Gerente",
                    Ativo = true
                };

                context.Usuarios.Add(usuario);
                context.SaveChanges();

                // Act
                var usuarioObtido = usuarioService.Obter(1);

                // Assert
                Assert.NotNull(usuarioObtido);
                Assert.Equal(usuario.UsuarioId, usuarioObtido.UsuarioId);
                Assert.True(usuarioObtido.Ativo);
            }
        }

        [Fact]
        public void ehGerente_DeveRetornarVerdadeiroSeFuncaoGerente()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDb")
                .Options;

            using (var context = new ApplicationDbContext(options))
            {
                var usuarioService = new UsuarioService(context);

                var usuario = new Usuario
                {
                    UsuarioId = 2,
                    Funcao = "Gerente",
                    Ativo = true
                };

                context.Usuarios.Add(usuario);
                context.SaveChanges();

                // Act
                var resultado = usuarioService.ehGerente(2);

                // Assert
                Assert.True(resultado);
            }
        }
    }
}
