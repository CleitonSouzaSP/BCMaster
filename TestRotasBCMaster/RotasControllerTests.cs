using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BC.Kernel;
using BCMaster.Rotas.WebAPI.Controller.Rotas;
using BCMaster.Rotas.WebAPI.Domain.Rotas;
using BCMaster.Rotas.WebAPI.Inteface.Rotas;
using BCMaster.Rotas.WebAPI.Services.Rotas;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

public class RotasControllerTests
{
    private readonly Mock<ILogger<RotasController>> _loggerMock;
    private readonly Mock<IRotaRepository> _rotaRepositoryMock;
    private readonly Mock<IRotaService> _rotaServiceMock;
    private readonly RotasController _controller;

    public RotasControllerTests()
    {
        _loggerMock = new Mock<ILogger<RotasController>>();
        _rotaRepositoryMock = new Mock<IRotaRepository>();
        _rotaServiceMock = new Mock<IRotaService>();
        _controller = new RotasController(_loggerMock.Object, _rotaRepositoryMock.Object, _rotaServiceMock.Object);
    }

    [Fact]
    public async Task Adicionar_DeveRetornarOk_QuandoRotaForValida()
    {
        var rota = new Rota { Id = Guid.NewGuid(), Origem = "GRU", Destino = "CDG", Valor = 40 };
        _rotaRepositoryMock.Setup(r => r.Adicionar(It.IsAny<Rota>()))
            .ReturnsAsync(Result<Exception, Rota>.Of(rota));

        var result = await _controller.Adicionar(rota);

        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(StatusCodes.Status200OK, okResult.StatusCode);
    }

    [Fact]
    public async Task Adicionar_DeveRetornarBadRequest_QuandoRotaForNula()
    {
        var result = await _controller.Adicionar(null);

        var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
        Assert.Equal(StatusCodes.Status400BadRequest, badRequestResult.StatusCode);
    }

    [Fact]
    public async Task Atualizar_DeveRetornarOk_QuandoAtualizacaoForBemSucedida()
    {
        var rota = new Rota { Id = Guid.NewGuid(), Origem = "BRC", Destino = "SCL", Valor = 99 };
        _rotaRepositoryMock.Setup(r => r.Atualizar(It.IsAny<Rota>()))
            .ReturnsAsync(Result<Exception, Rota>.Of(rota));

        var result = await _controller.Atualizar(rota);

        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(StatusCodes.Status200OK, okResult.StatusCode);
    }

    [Fact]
    public async Task Atualizar_DeveRetornarBadRequest_QuandoRotaForNula()
    {
        var result = await _controller.Atualizar(null);

        var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
        Assert.Equal(StatusCodes.Status400BadRequest, badRequestResult.StatusCode);
    }

    [Fact]
    public async Task Excluir_DeveRetornarNoContent_QuandoExclusaoForBemSucedida()
    {
        var rota = new Rota { Id = Guid.NewGuid(), Origem = "BRC", Destino = "SCL", Valor = 20 };
        _rotaRepositoryMock.Setup(r => r.Deletar(It.IsAny<Rota>()))
            .ReturnsAsync(Result<Exception, Rota>.Of(rota));

        var result = await _controller.Excluir(rota);

        Assert.IsType<NoContentResult>(result);
    }

    [Fact]
    public async Task Excluir_DeveRetornarBadRequest_QuandoRotaForNula()
    {
        var result = await _controller.Excluir(null);

        var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
        Assert.Equal(StatusCodes.Status400BadRequest, badRequestResult.StatusCode);
    }

    [Fact]
    public async Task ListarRotas_DeveRetornarOk_QuandoHouverRotasCadastradas()
    {
        var rotas = new List<Rota> { new Rota { Id = Guid.NewGuid(), Origem = "ORIG", Destino = "DEST", Valor = 125 } };
        _rotaRepositoryMock.Setup(r => r.Listar())
            .Returns(Result<Exception, IQueryable<Rota>>.Of(rotas.AsQueryable()));

        var result = await _controller.ListarRotas();

        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(StatusCodes.Status200OK, okResult.StatusCode);
    }

    [Fact]
    public async Task ListarRotas_DeveRetornarNotFound_QuandoNaoHouverRotas()
    {
        _rotaRepositoryMock.Setup(r => r.Listar())
            .Returns(Result<Exception, IQueryable<Rota>>.Of(new List<Rota>().AsQueryable()));

        var result = await _controller.ListarRotas();

        var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
        Assert.Equal(StatusCodes.Status404NotFound, notFoundResult.StatusCode);
    }

    [Fact]
    public async Task ObterMelhorRota_DeveRetornarOk_QuandoRotaForEncontrada()
    {
        string origem = "GRU", destino = "CDG", melhorRota = "GRU -> CDG (Valor: 40)";
        _rotaServiceMock.Setup(s => s.ObterMelhorRota(origem, destino))
            .Returns(melhorRota);

        var result = await _controller.ObterMelhorRota(origem, destino);

        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(StatusCodes.Status200OK, okResult.StatusCode);
    }

    [Fact]
    public async Task ObterMelhorRota_DeveRetornarNotFound_QuandoNenhumaRotaForEncontrada()
    {
        string origem = "A", destino = "B";
        _rotaServiceMock.Setup(s => s.ObterMelhorRota(origem, destino))
            .Returns((string)null);

        var result = await _controller.ObterMelhorRota(origem, destino);

        var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
        Assert.Equal(StatusCodes.Status404NotFound, notFoundResult.StatusCode);
    }
}
