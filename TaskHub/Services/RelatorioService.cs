using Microsoft.EntityFrameworkCore;
using System.Transactions;
using TaskHub.Data;
using TaskHub.DTO;
using TaskHub.Services.Interfaces;

namespace TaskHub.Services
{
    public class RelatorioService : IRelatorioService
    {
        private readonly ApplicationDbContext _context;

        public RelatorioService(ApplicationDbContext context)
        {
            _context = context;
        }

        public IEnumerable<RelatorioMediaTarefasPorUsuarioDTO> MediaTarefasConcluidas(DateTime inicio, DateTime fim, int[]? usuarioId = null)
        {
            using (var scope = new TransactionScope(TransactionScopeOption.Required, new TransactionOptions { IsolationLevel = IsolationLevel.ReadUncommitted }))
            {
                var result = _context.Tarefas
                    .Where(t => (usuarioId != null ? usuarioId.Contains(t.UsuarioId) : true) && t.Ativa && t.Status == Enums.StatusTarefa.Concluída && t.DataConcluido >= inicio.Date && t.DataConcluido <= fim.Date.AddSeconds(86399))
                    .GroupBy(t => t.UsuarioId)
                    .Select(g => new RelatorioMediaTarefasPorUsuarioDTO
                    {
                        NumeroTarefas = g.Count(),
                        Usuario = g.FirstOrDefault().Usuario ?? _context.Usuarios.FirstOrDefault(u => u.UsuarioId == g.Key)
                    }).ToList();

                scope.Complete();

                return result;
            }
        }

        public IEnumerable<RelatorioMediaTarefasPorUsuarioDTO> MediaTarefasConcluidas(int dias, int[]? usuarioId = null)
        {
            return MediaTarefasConcluidas(DateTime.Now.AddDays(-dias), DateTime.Now, usuarioId);
        }

        public RelatorioMediaTarefasPorUsuarioDTO MediaTarefasConcluidasPorUsuario(int usuarioId, DateTime inicio, DateTime fim)
        {
            return MediaTarefasConcluidas(inicio, fim, new int[] { usuarioId }).FirstOrDefault();
        }

        public RelatorioMediaTarefasPorUsuarioDTO MediaTarefasConcluidasPorUsuario(int usuarioId, int dias)
        {
            return MediaTarefasConcluidas(dias, new int[] { usuarioId }).FirstOrDefault();
        }
    }
}
