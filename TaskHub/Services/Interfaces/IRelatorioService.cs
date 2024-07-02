using TaskHub.DTO;

namespace TaskHub.Services.Interfaces
{
    public interface IRelatorioService
    {
        public IEnumerable<RelatorioMediaTarefasPorUsuarioDTO> MediaTarefasConcluidas(DateTime inicio, DateTime fim, int[]? usuarioId = null);
        public IEnumerable<RelatorioMediaTarefasPorUsuarioDTO> MediaTarefasConcluidas(int dias, int[]? usuarioId = null);
        public RelatorioMediaTarefasPorUsuarioDTO MediaTarefasConcluidasPorUsuario(int usuarioId, DateTime inicio, DateTime fim);
        public RelatorioMediaTarefasPorUsuarioDTO MediaTarefasConcluidasPorUsuario(int usuarioId, int dias);
    }
}
