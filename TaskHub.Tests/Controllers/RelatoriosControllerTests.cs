using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Net;
using TaskHub.Controllers;
using TaskHub.DTO;
using TaskHub.Services.Interfaces;

namespace TaskHub.Tests
{
    public class RelatoriosControllerTests
    {
        [Fact]
        public void MediaTarefasConcluidasUsuarios_DeveRetornarOk()
        {
            // Arrange
            var relatorioServiceMock = new Mock<IRelatorioService>();
            relatorioServiceMock.Setup(s => s.MediaTarefasConcluidas(30, It.IsAny<int[]>()))
                .Returns(
                new List<RelatorioMediaTarefasPorUsuarioDTO>
                    {
                        new RelatorioMediaTarefasPorUsuarioDTO { NumeroTarefas = 2 },
                        new RelatorioMediaTarefasPorUsuarioDTO { NumeroTarefas = 3 }
                    }
                );

            var controller = new RelatoriosController(relatorioServiceMock.Object);

            // Act
            var result = controller.MediaTarefasConcluidasUsuarios() as OkObjectResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(HttpStatusCode.OK, (HttpStatusCode)result.StatusCode);
        }

        [Fact]
        public void MediaTarefasConcluidasPorUsuario_DeveRetornarOk()
        {
            // Arrange
            var usuarioId = 42;
            var relatorioServiceMock = new Mock<IRelatorioService>();
            relatorioServiceMock.Setup(s => s.MediaTarefasConcluidasPorUsuario(usuarioId, It.IsAny<int>()))
                .Returns(new RelatorioMediaTarefasPorUsuarioDTO { NumeroTarefas = 1 });

            var controller = new RelatoriosController(relatorioServiceMock.Object);

            // Act
            var result = controller.MediaTarefasConcluidasPorUsuario(usuarioId) as OkObjectResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(HttpStatusCode.OK, (HttpStatusCode)result.StatusCode);
        }
    }
}
