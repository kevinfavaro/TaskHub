using Microsoft.AspNetCore.Mvc;
using TaskHub.DTO;
using TaskHub.Services.Interfaces;

namespace TaskHub.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RelatoriosController : ControllerBase
    {
        private readonly IRelatorioService _relatorioService;
        public RelatoriosController(IRelatorioService relatorioService)
        {
            _relatorioService = relatorioService;
        }

        [HttpGet]
        [Route("tarefas/medias/concluidas/usuarios")]
        public IActionResult MediaTarefasConcluidasUsuarios(int dias = 30)
        {
            var relatorio = _relatorioService.MediaTarefasConcluidas(dias);
            return Ok(relatorio);
        }

        [HttpGet]
        [Route("tarefas/medias/concluidas/usuarios/{usuarioId}")]
        public IActionResult MediaTarefasConcluidasPorUsuario(int usuarioId, int dias = 30)
        {
            var relatorio = _relatorioService.MediaTarefasConcluidasPorUsuario(usuarioId, dias);
            return Ok(relatorio);
        }
    }
}
