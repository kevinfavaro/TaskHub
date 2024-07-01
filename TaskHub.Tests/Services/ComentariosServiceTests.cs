using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Moq;
using TaskHub.Data;
using TaskHub.DTO;
using TaskHub.Mapper;
using TaskHub.Provider;
using TaskHub.Services;
using TaskHub.Services.Interfaces;

namespace TaskHub.Tests
{
    public class ComentarioServiceTests
    {
        [Fact]
        public void CriarComentario_DeveRetornarComentarioCriado()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDb")
                .Options;

            using (var context = new ApplicationDbContext(options))
            {
                var usuarioProviderMock = new Mock<IUsuarioProvider>();
                usuarioProviderMock.SetupGet(p => p.UsuarioId).Returns(1);

                var tarefaServiceMock = new Mock<ITarefaService>();
                tarefaServiceMock.Setup(s => s.Existe(It.IsAny<int>(), It.IsAny<int>())).Returns(true);

                var historicoServiceMock = new Mock<IHistoricoService>();

                var mapperConfig = new MapperConfiguration(cfg => cfg.AddProfile(new AutoMapperProfile()));
                var mapper = mapperConfig.CreateMapper();

                var comentarioService = new ComentarioService(
                    context,
                    tarefaServiceMock.Object,
                    historicoServiceMock.Object,
                    usuarioProviderMock.Object,
                    mapper
                );

                var projetoId = 1;
                var tarefaId = 42;
                var comentarioCreate = new ComentarioCreateDTO { Texto = "Meu comentário" };

                // Act
                var comentarioCriado = comentarioService.Criar(projetoId, tarefaId, comentarioCreate);

                // Assert
                Assert.NotNull(comentarioCriado);
                Assert.Equal(comentarioCreate.Texto, comentarioCriado.Texto);
                Assert.Equal(1, comentarioCriado.UsuarioId);
                Assert.Equal(tarefaId, comentarioCriado.TarefaId);
                Assert.True(comentarioCriado.DataComentario > DateTime.MinValue);
            }
        }
    }
}
