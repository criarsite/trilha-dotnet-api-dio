namespace ApiVendas.src.Models
{
    public class Pedido
    {
        public int Id { get; set; }
        public DateTime DataPedido { get; set; }
        public int VendedorId { get; set; }
        public string Itens { get; set; }
        public string StatusDisponiveis { get; set; }
        public string StatusPedido { get; set; }
        public DateTime DataUltimaAtualizacao { get; set; }
    }
}

