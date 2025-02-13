using BC.Kernel;
using BCMaster.Domain.Domain.Rotas;

namespace BCMaster.Domain.Inteface.Rotas
{
    public interface IRotaRepository
    {
        Task<Result<Exception, Rota>> Adicionar(Rota rota);
        Result<Exception, IQueryable<Rota>> Listar();
        Task<Result<Exception, Rota>> Atualizar(Rota rota);
        Task<Result<Exception, Rota>> Deletar(Rota rota);
    }
}
