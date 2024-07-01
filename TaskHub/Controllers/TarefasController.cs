using Microsoft.AspNetCore.Mvc;
using TaskHub.DTO;
using TaskHub.Services.Interfaces;

namespace TaskHub.Controllers
{
    [Route("api/projetos/{projetoId}/[controller]")]
    [ApiController]
    public class TarefasController : ControllerBase
    {
        private readonly ITarefaService _tarefaService;
        public TarefasController(ITarefaService tarefaService)
        {
            _tarefaService = tarefaService;
        }

        [HttpGet]
        [Route("{tarefaId}")]
        public IActionResult ObterTarefa(int projetoId, int tarefaId)
        {
            return Ok(_tarefaService.Obter(projetoId, tarefaId));
        }

        [HttpPost]
        public IActionResult CriarTarefa(int projetoId, TarefaCreateDTO novaTarefa)
        {
            var tarefaCriada = _tarefaService.Criar(projetoId, novaTarefa);
            return Created("/api/projetos/" + projetoId + "/tarefas/" + tarefaCriada.TarefaId, tarefaCriada);
        }

        [HttpPut]
        [Route("{tarefaId}")]
        public IActionResult AtualizarTarefa(int projetoId, int tarefaId, TarefaUpdateDTO tarefaAtualizada)
        {
            _tarefaService.Atualizar(projetoId, tarefaId, tarefaAtualizada);
            return NoContent();
        }

        [HttpDelete]
        [Route("{tarefaId}")]
        public IActionResult RemoverTarefa(int projetoId, int tarefaId)
        {
            _tarefaService.Remover(projetoId, tarefaId);
            return NoContent();
        }
    }
}
