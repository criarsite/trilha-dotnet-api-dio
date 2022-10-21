using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ApiVendas.src.Data;
using ApiVendas.src.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ApiVendas.src.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PedidosController : ControllerBase
    {
        private readonly Contexto _context;

        public PedidosController(Contexto context)
        {
            _context = context;
        }

        [HttpGet("ListarTodos")]
        public async Task<ActionResult<IEnumerable<Pedido>>> GetPedidos()
        {
            return await _context.Pedidos.ToListAsync();
        }

        [HttpGet("BuscarPor{id}")]
        public async Task<ActionResult<Pedido>> GetPedido(int id)
        {
            var pedido = await _context.Pedidos.FindAsync(id);

            if (pedido == null)
            {
                return NotFound();
            }

            return pedido;
        }


        [HttpPut("AlterarStatus{id}")]
        public async Task<IActionResult> PutPedido(int id, string status, Pedido pedido)
        {

            if (id == 0)
                return BadRequest("Campo id é necessário para buscar o pedido!");
            if (status == null)
                return BadRequest("Informe o novo status do pedido!");
            string[] statusBd1 = new string[2];

            Pedido pedidoBd = await _context.Pedidos.FindAsync(id);

            if (pedidoBd.StatusDisponiveis.ToUpper() == "ENVIADO PARA TRANSPORTADORA" || pedidoBd.StatusDisponiveis.ToUpper() == "CANCELADO")
            {
                return BadRequest($"O pedido em tela não aceita modificações no seu status: {pedidoBd.StatusPedido}");
            }
            statusBd1 = pedidoBd.StatusDisponiveis.Split(",");
            statusBd1[0].Trim();
            statusBd1[1].Trim();


            if (status.ToUpper() != statusBd1[0].ToUpper() && status.ToUpper() != statusBd1[1].ToUpper())
            {
                return BadRequest($"O pedido em tela só aceita um dos status: {statusBd1[0]} ou {statusBd1[1]}");
            }
            DateTime data = DateTime.Now;


            data.ToShortDateString();
            pedidoBd.StatusPedido = status;
            pedidoBd.DataUltimaAtualizacao = data;
            if (status.ToUpper() == "PAGAMENTO APROVADO")
            {
                pedidoBd.StatusDisponiveis = "Enviado para Transportadora, Cancelado";
            }
            else if (status.ToUpper() == "ENVIADO PARA TRANSPORTADORA")
            {
                pedidoBd.StatusDisponiveis = "Entregue";
            }

            _context.Entry(pedidoBd).State = EntityState.Modified;


            try
            {
                await _context.SaveChangesAsync();
                return CreatedAtAction("GetPedido", new { id = pedido.Id }, pedido);

            }
            catch (DbUpdateConcurrencyException ex)
            {
                if (!PedidoExists(id))
                {
                    return NotFound();
                }
                else
                {
                    return BadRequest(ex.ToString());
                }
                
            }

            //return NoContent() ;
        }



        // POST: api/Pedidos

        [HttpPost("NovoPedido")]
        public async Task<ActionResult<Pedido>> PostPedido(Pedido pedido, int vendedorId, string itens)
        {
            if (vendedorId == 0)
            {
                return BadRequest("Campo Id do vendedor é necessário!");
            }

            DateTime data = DateTime.Now;
            data.ToShortDateString();
            pedido.VendedorId = vendedorId;
            pedido.StatusPedido = "Aguardando Pagamento";
            pedido.StatusDisponiveis = "Pagamento Aprovado, Cancelado";
            pedido.DataPedido = data;
            pedido.DataUltimaAtualizacao = data;
            pedido.Itens = itens;

            try
            {
                _context.Pedidos.Add(pedido);
                await _context.SaveChangesAsync();
                return CreatedAtAction("GetPedido", new { id = pedido.Id }, pedido);
            }
            catch (DbUpdateConcurrencyException ex)
            {
                return BadRequest(ex.ToString());
            }

         }


        private bool PedidoExists(int id)
        {
            return _context.Pedidos.Any(e => e.Id == id);
        }

         [HttpDelete("{id}")]
        public IActionResult Deletar(int id)
        {
            var pedidoBd = _context.Pedidos.Find(id);
            if (pedidoBd == null) return NotFound();
            _context.Pedidos.Remove(pedidoBd);
            _context.SaveChanges();
            return NoContent();
        }
    }
}