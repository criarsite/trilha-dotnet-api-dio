using Microsoft.EntityFrameworkCore;
using ApiVendas.src.Models;

namespace ApiVendas.src.Data
{
    public class Contexto : DbContext
    {
        public Contexto(DbContextOptions<Contexto> options) : base(options)
        {

        }
        public DbSet<Vendedor> Vendedores { get; set; }
        public DbSet<Pedido> Pedidos { get; set; }
    }
}