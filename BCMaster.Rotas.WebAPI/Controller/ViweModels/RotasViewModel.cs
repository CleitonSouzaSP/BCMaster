namespace BCMaster.Rotas.WebAPI.Controller.ViweModels
{
    public class RotasViewModel
    {
        public Guid Id { get; set; }
        public string Origem { get; set; } = string.Empty;
        public string Destino { get; set; } = string.Empty;
        public double Valor { get; set; } = 0;
    }
}
