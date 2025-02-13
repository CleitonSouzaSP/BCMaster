using BCMaster.Domain.Domain.Rotas;
using BCMaster.Infra.Context.Rotas;
using BCMaster.Rotas.WebAPI.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");


// Configuração do banco de dados
// Usa banco em memória apenas no ambiente de desenvolvimento/teste
if (builder.Environment.IsDevelopment())
{
    builder.Services.AddDbContext<RotaDbContext>(options =>
        options.UseInMemoryDatabase("TestDatabaseMaster"));
}
else
{
    builder.Services.AddDbContext<RotaDbContext>(options =>
        options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
}

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "BC.Rotas.WebAPI", Version = "v1" });
    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    c.IncludeXmlComments(xmlPath);
});

builder.Services.AddHttpClient();
builder.Services.AddHttpContextAccessor();
builder.Services.AddLogging(config =>
{
    config.AddDebug();
    config.AddConsole();
});

builder.Services.AddControllers();
builder.Services.AddCors();
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

//Resolução de dependencias, adicionado no metodo de extensão
builder.Services.AddRepositories();

builder.Services.AddServices();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI();
}

//Implementação para adicionar dados iniciais no banco em memoria
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<RotaDbContext>();

    //Adiciona dados iniciais ao banco em memória
    if (!context.Rotas.Any())
    {
        context.Rotas.AddRange(new List<Rota>
        {
            new Rota { Id = Guid.NewGuid(), Origem = "GRU", Destino = "BRC", Valor = 10 },
            new Rota { Id = Guid.NewGuid(), Origem = "BRC", Destino = "SCL", Valor = 5 },
            new Rota { Id = Guid.NewGuid(), Origem = "GRU", Destino = "CDG", Valor = 75 },
            new Rota { Id = Guid.NewGuid(), Origem = "GRU", Destino = "SCL", Valor = 20 },
            new Rota { Id = Guid.NewGuid(), Origem = "GRU", Destino = "ORL", Valor = 56 },
            new Rota { Id = Guid.NewGuid(), Origem = "ORL", Destino = "CDG", Valor = 5 },
            new Rota { Id = Guid.NewGuid(), Origem = "SCL", Destino = "ORL", Valor = 20 },
        });

        context.SaveChanges();
    }
}

app.UseHsts();
app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
