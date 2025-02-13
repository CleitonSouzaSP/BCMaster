using BCMaster.Infra.Context.Rotas;

namespace BCMaster.Services.Services.Rotas
{
    public class RotaService : IRotaService
    {
        private readonly RotaDbContext _context;

        public RotaService(RotaDbContext context)
        {
            _context = context;
        }

        public string ObterMelhorRota(string origem, string destino)
        {
            //Criando um grafo de rotas
            //Lê todas as rotas do banco e cria um grafo com as conexões
            var rotas = _context.Rotas.ToList();

            var grafo = new Dictionary<string, List<(string, double)>>();

            foreach (var rota in rotas)
            {
                if (!grafo.ContainsKey(rota.Origem))
                    grafo[rota.Origem] = new List<(string, double)>();

                grafo[rota.Origem].Add((rota.Destino, rota.Valor));
            }
            /////

            //Armazena o menor custo
            var custos = new Dictionary<string, double> { [origem] = 0 };

            //GUarda o caminho percorrido para a reconstrução da rota
            var anteriores = new Dictionary<string, string>();

            //Mantes controle dos locais já processados
            var rotasVisitadas = new HashSet<string>();

            //Escolhe o local atual com o menor custo já acumulado
            //Para cada iteração (vizinho) verifica se o novo custo é menor que o custo já registrado
            //Se for menor atualiza o custo e já define o caminho anterior
            //Repete o processo até todos os nós serem processados ou o destino alcançado
            while (true)
            {
                string posAtual = custos
                    .Where(n => !rotasVisitadas.Contains(n.Key))
                    .OrderBy(n => n.Value)
                    .Select(n => n.Key)
                    .FirstOrDefault();

                if (posAtual == null || posAtual == destino) break;

                rotasVisitadas.Add(posAtual);

                if (!grafo.ContainsKey(posAtual)) continue;

                foreach (var (vizinho, custo) in grafo[posAtual])
                {
                    double novoCusto = custos[posAtual] + custo;
                    if (!custos.ContainsKey(vizinho) || novoCusto < custos[vizinho])
                    {
                        custos[vizinho] = novoCusto;
                        anteriores[vizinho] = posAtual;
                    }
                }
            }

            if (!custos.ContainsKey(destino)) return null;

            var caminho = new List<string>();
            for (var pos = destino; pos != null; pos = anteriores.GetValueOrDefault(pos))
                caminho.Add(pos);
            caminho.Reverse();

            return string.Join(" - ", caminho) + $" ao custo de ${custos[destino]}";

        }
    }
}
