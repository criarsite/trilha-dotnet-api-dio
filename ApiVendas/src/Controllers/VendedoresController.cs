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


        [HttpPut("{id}")]
        public IActionResult Atualizar(int id, Vendedor vendedor)
        {
            var vendedorBd = _context.Vendedores.Find(id);
            if (id == null) return NotFound();
            vendedorBd.Nome = vendedor.Nome;
            vendedorBd.Cpf = vendedor.Cpf;
            vendedorBd.Telefone = vendedor.Telefone;
            _context.Vendedores.Update(vendedorBd);
            _context.SaveChanges();
            return Ok(vendedorBd);

        }

        [HttpDelete("{id}")]
        public IActionResult Deletar(int id)
        {
            var vendedorBd = _context.Vendedores.Find(id);
            if (vendedorBd == null) return NotFound();
            _context.Vendedores.Remove(vendedorBd);
            _context.SaveChanges();
            return NoContent();
        }
    }
}