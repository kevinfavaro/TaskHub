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
    public class TarefasControllerTests
    {
        [Fact]
        public void ObterTarefa_DeveRetornarOkObjectResult()
        {
            // Arrange
            var tarefaServiceMock = new Mock<ITarefaService>();
            tarefaServiceMock.Setup(s => s.Obter(It.IsAny<int>(), It.IsAny<int>()))
                .Returns(new Tarefa { TarefaId = 42, Titulo = "Minha Tarefa" });

            var controller = new TarefasController(tarefaServiceMock.Object);
            var projetoId = 1;
            var tarefaId = 42;

            // Act
            var result = controller.ObterTarefa(projetoId, tarefaId) as OkObjectResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(200, result.StatusCode);
            Assert.IsType<Tarefa>(result.Value);
            Assert.Equal("Minha Tarefa", (result.Value as Tarefa).Titulo);
        }


        [Fact]
        public void CriarTarefa_DeveRetornar201Created()
        {
            // Arrange
            var tarefaServiceMock = new Mock<ITarefaService>();
            tarefaServiceMock.Setup(s => s.Criar(It.IsAny<int>(), It.IsAny<TarefaCreateDTO>()))
                .Returns(new Tarefa { TarefaId = 1 });

            var controller = new TarefasController(tarefaServiceMock.Object);
            var projetoId = 1;
            var novaTarefa = new TarefaCreateDTO { Titulo = "Nova Tarefa" };

            // Act
            var result = controller.CriarTarefa(projetoId, novaTarefa) as CreatedResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(201, result.StatusCode);
            Assert.Equal("/api/projetos/1/tarefas/1", result.Location);
        }

        [Fact]
        public void AtualizarTarefa_DeveRetornarNoContent()
        {
            // Arrange
            var tarefaServiceMock = new Mock<ITarefaService>();
            var controller = new TarefasController(tarefaServiceMock.Object);
            var projetoId = 1;
            var tarefaId = 42;
            var tarefaAtualizada = new TarefaUpdateDTO { Titulo = "Tarefa Atualizada" };

            // Act
            var result = controller.AtualizarTarefa(projetoId, tarefaId, tarefaAtualizada) as NoContentResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(204, result.StatusCode);
        }

        [Fact]
        public void RemoverTarefa_DeveRetornarNoContent()
        {
            // Arrange
            var tarefaServiceMock = new Mock<ITarefaService>();
            var controller = new TarefasController(tarefaServiceMock.Object);
            var projetoId = 1;
            var tarefaId = 42;

            // Act
            var result = controller.RemoverTarefa(projetoId, tarefaId) as NoContentResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(204, result.StatusCode);
        }
    }
}