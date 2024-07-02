using TaskHub.Models;

namespace TaskHub.DTO
{
    public class RelatorioMediaTarefasPorUsuarioDTO
    {
        public int NumeroTarefas { get; set; }
        public Usuario Usuario { get; set; }
    }
}
