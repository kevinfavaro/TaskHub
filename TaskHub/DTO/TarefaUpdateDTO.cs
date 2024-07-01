using TaskHub.Enums;

namespace TaskHub.DTO
{
    public class TarefaUpdateDTO
    {
        public string Titulo { get; set; }
        public string Descricao { get; set; }
        public DateTime? DataVencimento { get; set; }
        public StatusTarefa? Status { get; set; }
    }
}
