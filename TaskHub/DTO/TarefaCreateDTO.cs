using TaskHub.Enums;

namespace TaskHub.DTO
{
    public class TarefaCreateDTO
    {
        public string Titulo { get; set; }
        public string Descricao { get; set; }
        public DateTime? DataVencimento { get; set; }
        public PrioridadeTarefa Prioridade { get; set; }
    }
}
