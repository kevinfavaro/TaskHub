using TaskHub.Enums;

namespace TaskHub.Models
{
    public class Tarefa
    {
        public int TarefaId { get; set; }
        public string Titulo { get; set; }
        public string? Descricao { get; set; }
        public DateTime? DataVencimento { get; set; }
        public DateTime? DataConcluido { get; set; }
        public StatusTarefa? Status { get; set; }
        public PrioridadeTarefa Prioridade { get; set; }
        public int ProjetoId { get; set; }
        public int UsuarioId { get; set; }
        public bool Ativa { get; set; } = true;
        public virtual Projeto Projeto { get; set; }
        public virtual Usuario Usuario { get; set; }
    }
}
