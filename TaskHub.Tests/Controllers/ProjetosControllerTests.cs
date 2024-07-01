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
    public class ProjetosControllerTests
    {
        [Fact]
        public void ObterProjeto_DeveRetornarOkObjectResult()
        {
            // Arrange
            var projetoServiceMock = new Mock<IProjetoService>();
            projetoServiceMock.Setup(s => s.Obter(It.IsAny<int>()))
                .Returns(new Projeto { ProjetoId = 1, Nome = "Meu Projeto" });

            var controller = new ProjetosController(projetoServiceMock.Object);
            var projetoId = 1;

            // Act
            var result = controller.ObterProjeto(projetoId) as OkObjectResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(200, result.StatusCode);
            Assert.IsType<Projeto>(result.Value);
            Assert.Equal("Meu Projeto", (result.Value as Projeto).Nome);
        }

        [Fact]
        public void ListarProjetos_DeveRetornarOkObjectResult()
        {
            // Arrange
            var projetoServiceMock = new Mock<IProjetoService>();
            projetoServiceMock.Setup(s => s.ObterTodos())
                .Returns(new[] { new Projeto { ProjetoId = 1, Nome = "Projeto 1" }, new Projeto { ProjetoId = 2, Nome = "Projeto 2" } });

            var controller = new ProjetosController(projetoServiceMock.Object);

            // Act
            var result = controller.ListarProjetos() as OkObjectResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(200, result.StatusCode);
            Assert.IsType<Projeto[]>(result.Value);
            Assert.Equal(2, (result.Value as Projeto[]).Length);
        }

        [Fact]
        public void ListarTarefasProjeto_DeveRetornarOkObjectResult()
        {
            // Arrange
            var projetoServiceMock = new Mock<IProjetoService>();
            projetoServiceMock.Setup(s => s.ObterTarefas(It.IsAny<int>()))
                .Returns(new[] { new Tarefa { TarefaId = 1, Titulo = "Tarefa 1" }, new Tarefa { TarefaId = 2, Titulo = "Tarefa 2" } });

            var controller = new ProjetosController(projetoServiceMock.Object);
            var projetoId = 1;

            // Act
            var result = controller.ListarTarefasProjeto(projetoId) as OkObjectResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(200, result.StatusCode);
            Assert.IsType<Tarefa[]>(result.Value);
            Assert.Equal(2, (result.Value as Tarefa[]).Length);
        }

        [Fact]
        public void CriarProjeto_DeveRetornar201Created()
        {
            // Arrange
            var projetoServiceMock = new Mock<IProjetoService>();
            projetoServiceMock.Setup(s => s.Criar(It.IsAny<ProjetoCreateDTO>()))
                .Returns(new Projeto { ProjetoId = 1 });

            var controller = new ProjetosController(projetoServiceMock.Object);
            var novoProjeto = new ProjetoCreateDTO { Nome = "Novo Projeto" };

            // Act
            var result = controller.CriarProjeto(novoProjeto) as CreatedResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(201, result.StatusCode);
            Assert.Equal("/api/projetos/1", result.Location);
        }

        [Fact]
        public void RemoverProjeto_DeveRetornarNoContent()
        {
            // Arrange
            var projetoServiceMock = new Mock<IProjetoService>();
            var controller = new ProjetosController(projetoServiceMock.Object);
            var projetoId = 1;

            // Act
            var result = controller.RemoverProjeto(projetoId) as NoContentResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(204, result.StatusCode);
        }
    }
}