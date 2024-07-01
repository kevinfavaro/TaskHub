using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TaskHub.DTO;
using TaskHub.Services.Interfaces;

namespace TaskHub.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProjetosController : ControllerBase
    {
        private readonly IProjetoService _projetoService;

        public ProjetosController(IProjetoService projetoService)
        {
            _projetoService = projetoService;
        }

        [HttpGet]
        [Route("{projetoId}")]
        public IActionResult ObterProjeto(int projetoId)
        {
            return Ok(_projetoService.Obter(projetoId));
        }

        [HttpGet]
        public IActionResult ListarProjetos()
        {
            var projetos = _projetoService.ObterTodos();
            return Ok(projetos);
        }

        [HttpGet]
        [Route("{projetoId}/tarefas")]
        public IActionResult ListarTarefasProjeto(int projetoId)
        {
            var tarefas = _projetoService.ObterTarefas(projetoId);
            return Ok(tarefas);
        }

        [HttpPost]
        public IActionResult CriarProjeto(ProjetoCreateDTO novoProjeto)
        {
            var projetoCriado = _projetoService.Criar(novoProjeto);
            return Created("/api/projetos/" + projetoCriado.ProjetoId, projetoCriado);
        }

        [HttpDelete]
        [Route("{projetoId}")]
        public IActionResult RemoverProjeto(int projetoId)
        {
            _projetoService.Remover(projetoId);
            return NoContent();
        }
    }
}
