using System;
using Microsoft.AspNetCore.Mvc;
using Moq;
using TaskHub.Controllers;
using TaskHub.DTO;
using TaskHub.Models;
using TaskHub.Services.Interfaces;
using Xunit;

namespace TaskHub.Tests
{
    public class ComentariosControllerTests
    {
        [Fact]
        public void CriarComentario_DeveRetornar201Created()
        {
            // Arrange
            var comentarioServiceMock = new Mock<IComentarioService>();
            comentarioServiceMock.Setup(s => s.Criar(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<ComentarioCreateDTO>()))
                .Returns(new Comentario { ComentarioId = 1 });

            var controller = new ComentariosController(comentarioServiceMock.Object);
            var projetoId = 1;
            var tarefaId = 42;
            var comentario = new ComentarioCreateDTO { Texto = "Meu comentário" };

            // Act
            var result = controller.CriarComentario(projetoId, tarefaId, comentario) as CreatedResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(201, result.StatusCode);
            Assert.Equal("/api/projetos/1/tarefas/42/comentarios/1", result.Location);
        }
    }
}
