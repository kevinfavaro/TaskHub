# TaskHub

> É um projeto simples de controle de Projetos e Tarefas.
> Desenvolvido com o intuito de aplicação de teste.

### Ajustes e melhorias

O projeto ainda está em desenvolvimento e as próximas atualizações serão voltadas nas seguintes tarefas:

- [x] Definição de Banco de Dados
- [x] Criação da API e Testes
- [x] Ajustes e inclusão de relatório
- [x] Readme
- [x] Docker

## 💻 Pré-requisitos

Antes de começar, verifique se você atendeu aos seguintes requisitos:

- Você tem o `.NET 8`
- Você tem o `SQL Server`
- Você instalou a versão mais recente do `Docker`
- (opicional) ter o `Docker Desktop`

## 🚀 Instalando TaskHub

Para instalar o TaskHub, siga estas etapas:

- Vá para o diretório do TaskHub pelo terminal e execute:
```
docker build -t taskhub .
docker-compose up
```
- Após subir o docker, conecte ao banco de dados (localhost,1533)
- E executar o comando do arquivo `CreateDatabase.sql`
- O ambiente já vai estar pronto para ser executado

## ☕ Usando TaskHub

Para usar TaskHub, você pode seguir com o postman ou similar.

## 📝 Licença

Esse projeto está sob licença. Veja o arquivo [LICENÇA](LICENSE) para mais detalhes.