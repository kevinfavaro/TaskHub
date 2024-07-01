using AutoMapper;
using Microsoft.Extensions.Options;
using TaskHub.Data;
using TaskHub.DTO;
using TaskHub.Models;
using TaskHub.Provider;
using TaskHub.Services.Interfaces;

namespace TaskHub.Services
{
    public class TarefaService : ITarefaService
    {
        private readonly ApplicationDbContext _context;
        private readonly IHistoricoService _historicoService;
        private readonly IUsuarioProvider _usuarioProvider;
        private readonly IMapper _mapper;

        private readonly int _maxTarefas;

        public TarefaService(ApplicationDbContext context,
            IHistoricoService historicoService,
            IUsuarioProvider usuarioIdProvider,
            IOptions<AppSettings> appSettings,
            IMapper mapper)
        {
            _context = context;
            _historicoService = historicoService;
            _usuarioProvider = usuarioIdProvider;
            _mapper = mapper;
            _maxTarefas = appSettings.Value.MaxTarefas;
        }

        public Tarefa Obter(int projetoId, int tarefaId)
        {
            return _context.Tarefas.FirstOrDefault(t => t.ProjetoId == projetoId && t.TarefaId == tarefaId);
        }

        public bool Existe(int projetoId, int tarefaId)
        {
            return _context.Tarefas.FirstOrDefault(t => t.ProjetoId == projetoId && t.TarefaId == tarefaId && t.Ativa) != null;
        }

        public IEnumerable<Tarefa> ObterPorProjetoId(int projetoId)
        {
            return _context.Tarefas.Where(t => t.ProjetoId == projetoId && t.Ativa == true).ToList();
        }

        public bool TemPendentePorProjetoId(int projetoId)
        {
            return _context.Tarefas.FirstOrDefault(t => t.ProjetoId == projetoId && t.Ativa && t.Status != Enums.StatusTarefa.Concluída) != null;
        }

        public Tarefa Criar(int projetoId, TarefaCreateDTO novaTarefa)
        {
            if (_usuarioProvider.UsuarioId == null)
            {
                throw new Exception("Necessário identificar o usuário para a operação.");
            }

            var CountTarefas = _context.Tarefas.Count(t =>  t.ProjetoId == projetoId);
            if (CountTarefas >= _maxTarefas)
            {
                throw new Exception($"Não é possível adicionar mais de {_maxTarefas} tarefas no projeto");
            }

            var tarefa = _mapper.Map<Tarefa>(novaTarefa);

            tarefa.UsuarioId = (int)_usuarioProvider.UsuarioId;
            tarefa.ProjetoId = projetoId;
            tarefa.Status = tarefa.Status ?? Enums.StatusTarefa.Pendente;

            _context.Tarefas.Add(tarefa);
            _context.SaveChanges();

            _historicoService.Criar((int)tarefa.TarefaId, "Tarefa criada", (int)tarefa.UsuarioId);

            return tarefa;
        }

        public void Atualizar(int projetoId, int tarefaId, TarefaUpdateDTO tarefaAtualizada)
        {
            if (_usuarioProvider.UsuarioId == null)
            {
                throw new Exception("Necessário identificar o usuário para a operação.");
            }

            var tarefaExistente = _context.Tarefas.FirstOrDefault(t => t.TarefaId == tarefaId && t.ProjetoId == projetoId);
            if (tarefaExistente != null)
            {
                string historicoMessage = (tarefaAtualizada.Titulo != tarefaExistente.Titulo ? ($"Titulo - De: {tarefaExistente.Titulo} | Para: {tarefaAtualizada.Titulo}\n") : "") +
                    (tarefaAtualizada.Descricao != tarefaExistente.Descricao ? ($"Descricao - De: {tarefaExistente.Descricao} | Para: {tarefaAtualizada.Descricao}\n") : "") +
                    (tarefaAtualizada.DataVencimento != tarefaExistente.DataVencimento ? ($"DataVencimento - De: {tarefaExistente.DataVencimento} | Para: {tarefaAtualizada.DataVencimento}\n") : "") +
                    (tarefaAtualizada.Status != tarefaExistente.Status ? ($"Status - De: {tarefaExistente.Status} | Para: {tarefaAtualizada.Status}\n") : "");

                tarefaExistente.Titulo = tarefaAtualizada.Titulo ?? tarefaExistente.Titulo;
                tarefaExistente.Descricao = tarefaAtualizada.Descricao ?? tarefaExistente.Descricao;
                tarefaExistente.DataVencimento = tarefaAtualizada.DataVencimento ?? tarefaExistente.DataVencimento;
                tarefaExistente.Status = tarefaAtualizada.Status ?? tarefaExistente.Status;

                _context.SaveChanges();

                _historicoService.Criar(tarefaId, historicoMessage, (int)_usuarioProvider.UsuarioId);
            }
        }

        public void Remover(int projetoId, int tarefaId)
        {
            if (_usuarioProvider.UsuarioId == null)
            {
                throw new Exception("Necessário identificar o usuário para a operação.");
            }

            var tarefaExistente = _context.Tarefas.FirstOrDefault(t => t.TarefaId == tarefaId && t.ProjetoId == projetoId);
            if (tarefaExistente != null)
            {
                tarefaExistente.Ativa = false;
                _context.SaveChanges();

                _historicoService.Criar(tarefaId, "Tarefa excluída", (int)_usuarioProvider.UsuarioId);
            }
        }
    }
}
