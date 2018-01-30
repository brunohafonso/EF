using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using PrimeiroEF.Dados;
using PrimeiroEF.Models;

namespace PrimeiroEF.Controllers
{
    //rota padrão da aplicação
    [Route("API/[Controller]")]
    public class ClienteController : Controller
    {
        //instanciando classe cliente
        Cliente cliente = new Cliente();

        //definindo que o contexto é de leitura
        readonly ClienteContexto contexto;


        //criando contexto
        public ClienteController(ClienteContexto contexto)
        {
            this.contexto = contexto;
        }


        //listando atraves do metodo get
        [HttpGet]
        public IEnumerable<Cliente> listar()
        {
            return contexto.ClienteNaBase.ToList().OrderByDescending(c=>c.Id);
        }


        //listando com id
        [HttpGet("{Id}")]
        public Cliente listar(int Id)
        {
            return contexto.ClienteNaBase.Where(x => x.Id == Id).FirstOrDefault();
        }


        //cadastrando novos registros
        [HttpPost]
        public IActionResult cadastro([FromBody] Cliente cliente)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            cliente.DataCadastro = DateTime.Now;

            contexto.ClienteNaBase.Add(cliente);
            int x = contexto.SaveChanges();
            if (x > 0)
            {
                return Ok();
            }
            else
            {
                return BadRequest();
            }
        }


        //atualizando registros
        [HttpPut("{Id}")]
        public IActionResult editar(int Id, [FromBody] Cliente cliente)
        {
            if (cliente == null || cliente.Id != Id)
            {
                return BadRequest();
            }

            var cli = contexto.ClienteNaBase.Where(x => x.Id == Id).FirstOrDefault();

            if (cli == null)
            {
                return NotFound();
            }

            cli.Id = cliente.Id;
            cli.Nome = cliente.Nome;
            cli.Email = cliente.Email;
            cli.Idade = cliente.Idade;
            cli.DataCadastro = DateTime.Now;

            contexto.ClienteNaBase.Update(cli);
            int rs = contexto.SaveChanges();

            if (rs > 0)
            {
                return Ok();
            }
            else
            {
                return BadRequest();
            }
        }


        //deletando
        [HttpDelete("{Id}")]
        public IActionResult deletar(int Id)
        {
            var cliente = contexto.ClienteNaBase.Where(x => x.Id == Id).FirstOrDefault();
            if (cliente == null)
            {
                return NotFound();
            }

            contexto.ClienteNaBase.Remove(cliente);

            int rs = contexto.SaveChanges();
            if (rs > 0)
            {
                return Ok();
            }
            else
            {
                return BadRequest();
            }
        }
    }
}