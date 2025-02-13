using BC.Kernel;
using BCMaster.Domain.Domain.Rotas;
using BCMaster.Domain.Inteface.Rotas;
using BCMaster.Infra.Context.Rotas;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace BCMaster.Infra.Infra.Rotas
{
    public class RotaRepository : IRotaRepository
    {
        private readonly RotaDbContext _context;

        public RotaRepository(RotaDbContext context)
        {
            _context = context;
        }

        public async Task<Result<Exception, Rota>> Adicionar(Rota rota)
        {
            try
            {
                var rotaAdicionada = _context.Add(rota).Entity;
                await _context.SaveChangesAsync();
                return rotaAdicionada;
            }
            catch (Exception ex)
            {
                return ex;
            }
        }

        public async Task<Result<Exception, Rota>> Atualizar(Rota rota)
        {
            try
            {
                var rotaAtualizada = _context.Update(rota).Entity;
                await _context.SaveChangesAsync();
                return rotaAtualizada;
            }
            catch (Exception ex)
            {
                return ex;
            }
        }

        public async Task<Result<Exception, Rota>> Deletar(Rota rota)
        {
            try
            {
                var rotaExcluida = _context.Remove(rota).Entity;
                await _context.SaveChangesAsync();
                return rotaExcluida;
            }
            catch (Exception ex)
            {
                return ex;
            }
        }

        public Result<Exception, IQueryable<Rota>> Listar()
        {
            try
            {
                var listaRotas = _context.Rotas.AsNoTracking();

                //return listaRotas.AsResult(); //Retorno utilizando o padrão Result<T>
                return listaRotas.AsQueryable().AsResult(); //Retorno utilizando o padrão Result<T>
            }
            catch (Exception ex)
            {
                return ex;
            }
        }
    }
}
