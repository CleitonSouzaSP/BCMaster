using BCMaster.Domain.Domain.Rotas;
using BCMaster.Domain.Inteface.Rotas;
using BCMaster.Rotas.WebAPI.Controller.ViweModels;
using BCMaster.Services.Services.Rotas;
using Microsoft.AspNetCore.Mvc;

namespace BCMaster.Rotas.WebAPI.Controller.Rotas
{
    /// <summary>
    /// Controlador de Rotas
    /// Responsável por prover dados relacionados as rotas
    /// </summary>
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [Route("api/[controller]")]
    public class RotasController : ControllerBase
    {
        private readonly ILogger _logger;
        private readonly IRotaRepository _rotaRepository;
        private readonly IRotaService _rotaService;
        public RotasController(ILogger<RotasController> logger, IRotaRepository rotaRepository, IRotaService rotaService)
        {
            _logger = logger;
            _rotaRepository = rotaRepository;
            _rotaService = rotaService;
        }

        /// <summary>
        /// EndPoint responsável por adicionar uma rota
        /// </summary>
        /// <param name="rota"></param>
        /// <returns></returns>
        [ProducesResponseType(typeof(int), StatusCodes.Status200OK)]
        [Route("insert")]
        [HttpPost]
        public async Task<IActionResult> Adicionar([FromBody] Rota rota)
        {
            try
            {
                if (rota == null)
                    throw new ArgumentNullException(nameof(rota));

                var result = await _rotaRepository.Adicionar(rota);

                if (!result.IsSuccess)
                {
                    _logger.LogError(result.Failure.Message);
                    return BadRequest($"Erro ao adicionar a rota. Consulte o administrador do sistema!");
                }

                return Ok(result);

            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "Erro inesperado");
                return StatusCode(500, "Ocorreu um erro interno");
            }
        }

        /// <summary>
        /// EndPoint responsavel por atualizar uma rota
        /// </summary>
        /// <param name="rota"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        [ProducesResponseType(typeof(RotasViewModel), StatusCodes.Status200OK)]
        [Route("update")]
        [HttpPut]
        public async Task<IActionResult> Atualizar ([FromBody]Rota rota)
        {
            try
            {
                if (rota == null)
                    throw new ArgumentNullException(nameof(rota));

                var result = await _rotaRepository.Atualizar(rota);

                if (!result.IsSuccess)
                {
                    _logger?.LogError(result.Failure.Message);
                    return BadRequest($"Erro ao atualizar a rota. Consulte o administrador do sistema!");
                }
                return Ok(result);
            }
            catch(Exception ex) 
            {
                _logger.LogError(ex, "Erro inesperado");
                return StatusCode(500, "Ocorreu um erro interno");
            }
        }

        /// <summary>
        /// EndPoint responsavel por excluir uma rota
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [ProducesResponseType(typeof(RotasViewModel), StatusCodes.Status200OK)]
        [Route("{id}")]
        [HttpDelete]
        public async Task<IActionResult> Excluir([FromBody] Rota rota)
        {
            try
            {
                if (rota is null || rota.Id == Guid.Empty)
                    return BadRequest("Rota inválida para exclusão.");

                var result = await _rotaRepository.Deletar(rota);

                if (!result.IsSuccess)
                {
                    _logger?.LogError($"Erro ao excluir a rota {rota.Id}", result.Failure.Message);
                    return BadRequest($"Erro ao excluir a rota { rota.Id }");
                }

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Erro inesperado ao excluir a rota { rota?.Id }");
                return StatusCode(500, "Ocorreu um erro interno");
            }
        }

        /// <summary>
        /// EndPoint Responsável por retornar uma lista simples de todas rotas
        /// </summary>
        /// <returns>Lista Simples de Produtos</returns>
        [ProducesResponseType(typeof(RotasViewModel), StatusCodes.Status200OK)]
        [HttpGet]
        public async Task<IActionResult> ListarRotas(CancellationToken cancellationToken = default)
        {
            var result = await Task.FromResult(_rotaRepository.Listar());
            if (result.Success?.ToList() is null)
            {
                _logger.LogInformation($"Nenhuma rota cadastrada - { DateTime.Now }");
                return NotFound("Nenhuma rota cadastrada!");
            }
            return Ok(result);
        }

        /// <summary>
        /// EndPoint responsavel por obter a melhor rota
        /// Utilizando algoritimo de caminho minimo/shortest path algorithm Dijkstra
        /// </summary>
        /// <param name="origem"></param>
        /// <param name="destino"></param>
        /// <returns></returns>
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [HttpGet("melhor")]
        public async Task<IActionResult> ObterMelhorRota([FromQuery] string origem, [FromQuery] string destino)
        {
            if (string.IsNullOrEmpty(origem) || string.IsNullOrEmpty(destino))
                return BadRequest("Origem e destino são obrigatórios.");

            var melhorRotaObtida = _rotaService.ObterMelhorRota(origem, destino);

            return melhorRotaObtida is null ? NotFound("Nenhuma rota foi encontrada.") : Ok(melhorRotaObtida);
        }
    }
}
