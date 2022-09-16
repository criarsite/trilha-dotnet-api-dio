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
    public class VendedoresController : ControllerBase
    {
        private readonly Contexto _context;

        public VendedoresController(Contexto context)
        {
            _context = context;
        }

        // GET: api/Vendedores
        [HttpGet("ListarTodos")]
        public async Task<ActionResult<IEnumerable<Vendedor>>> GetVendedores()
        {
            return await _context.Vendedores.ToListAsync();
        }

        // GET: api/Vendedores/5
        [HttpGet("BuscarPor{id}")]
        public async Task<ActionResult<Vendedor>> GetVendedor(int id)
        {
            var vendedor = await _context.Vendedores.FindAsync(id);

            if (vendedor == null)
            {
                return NotFound();
            }

            return vendedor;
        }

        [HttpPost("NovoVendedor")]
        public async Task<ActionResult<Vendedor>> PostVendedor(Vendedor vendedor, string nome, string cpf, string telefone)
        {
            if (nome == null) return BadRequest("Informe o nome do vendedor!");
            if (cpf == null) return BadRequest("Informe o CPF do vendedor!");
            if (telefone == null) return BadRequest("Informe o telefone!");
            vendedor.Cpf = cpf;
            vendedor.Nome = nome;
            vendedor.Telefone = telefone;

            try
            {
                _context.Vendedores.Add(vendedor);
                await _context.SaveChangesAsync();

                return CreatedAtAction("GetVendedor", new { id = vendedor.Id }, vendedor);

            }
            catch (DbUpdateConcurrencyException ex)
            {
                return BadRequest(ex.ToString());
            }

        }

        private bool VendedorExists(int id)
        {
            return _context.Vendedores.Any(e => e.Id == id);
        }
    }
}