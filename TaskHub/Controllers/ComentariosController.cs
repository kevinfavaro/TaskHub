using Microsoft.AspNetCore.Mvc;
using TaskHub.DTO;
using TaskHub.Services.Interfaces;

namespace TaskHub.Controllers
{
    [Route("api/projetos/{projetoId}/tarefas/{tarefaId}/[controller]")]
    [ApiController]
    public class ComentariosController : ControllerBase
    {
        private readonly IComentarioService _comentarioService;
        public ComentariosController(IComentarioService comentarioService)
        {
            _comentarioService = comentarioService;
        }

        [HttpPost]
        public IActionResult CriarComentario(int projetoId, int tarefaId, ComentarioCreateDTO comentario)
        {
            var comentarioCriado = _comentarioService.Criar(projetoId, tarefaId, comentario);
            return Created("/api/projetos/" + projetoId + "/tarefas/" + tarefaId + "/comentarios/" + comentarioCriado.ComentarioId, comentarioCriado);
        }
    }
}
