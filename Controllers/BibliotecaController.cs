using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using webAPI.Data;
using webAPI.Models;

namespace webAPI.Controllers
{
    [ApiController]
    [Route("[Controller]")]
    public class BibliotecaController:Controller
    {
        private DatabaseContext _context;

        public BibliotecaController(DatabaseContext context)
        {
            _context=context;
        }

        [HttpGet]
        public async Task<ActionResult<List<Biblioteca>>> GetBiblioteca()
        {
            var bibliotecas = await _context.Bibliotecas.ToListAsync();
            return bibliotecas;
        }

        
        [HttpGet("{id}")]
        public async Task<ActionResult<Biblioteca>> GetBibliotecaByID(int id)
        {
            var bibliotecas = await _context.Bibliotecas.FindAsync();
            if(bibliotecas==null)
            {
                return NotFound();
            }
            return bibliotecas;
        }

        [HttpPost]
        public async Task<ActionResult<Biblioteca>> PostBiblioteca(Biblioteca biblioteca)
        {
            _context.Bibliotecas.Add(biblioteca);
            await _context.SaveChangesAsync();
            return CreatedAtAction("GetBibliotecaByID", new{id=biblioteca.BibliotecaID}, biblioteca);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<Biblioteca>> PutBiblioteca(int id, Biblioteca biblioteca)
        {
            if(id != biblioteca.BibliotecaID)
            {
                return BadRequest();
            }
            _context.Entry(biblioteca).State= EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();

            }
            catch(DbUpdateConcurrencyException)
            {
                if(!BibliotecaExists(id))
                {
                    return NotFound();

                }
                else
                {
                    throw;
                }

            }
            return CreatedAtAction("GetBibliotecaByID", new{id=biblioteca.BibliotecaID}, biblioteca);

        }
        [HttpDelete("{id}")]
        public async Task<ActionResult<Biblioteca>> DeleteBiblioteca(int id)
        {
            var bibliotecas = await _context.Bibliotecas.FindAsync(id);
            
            if(bibliotecas==null)
            {
                return NotFound();
            }
            _context.Bibliotecas.Remove(bibliotecas);
            await _context.SaveChangesAsync();
            return bibliotecas;

        }

        private bool BibliotecaExists(int id)
        {
            return _context.Bibliotecas.Any(d=>d.BibliotecaID==id);
        }
    }
}