using MeuPrimeiroMvc.Contexts;
using MeuPrimeiroMvc.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MeuPrimeiroMvc.Controllers
{
    [Route("[controller]")]
    public class JogadorController : Controller
    {
        //Criar uma referência (instância )sobre a conmunicação do meu banco de dados
        ProjetoTesteContext context = new ProjetoTesteContext();

        public IActionResult Index()
        {
            //Forma de listar todos os items da tabela de (Equipe)
            var listaJogador = context.Jogadors.Include("IdEquipeNavigation").ToList(); //.Include() - trago os dados das tabelas relacionadas. principalmente em tabelas de n pra n esse é op meio correto
            //Passar para a tela (em forma de memória) os dados das jogadores cadastradas
            ViewBag.listaJogador = listaJogador;

            //Forma de listar todos os items da tabela de (Equipe)
            var listaEquipes = context.Equipes.ToList();
            //Passar para a tela (em forma de memória) os dados das equipes cadastradas
            ViewBag.listaEquipes = listaEquipes;
            return View();
        }

        [Route("cadastrar")]
        public IActionResult CadastrarJogador(Jogador jogador)
        {
            //Armazenar o jogador 
            context.Add(jogador);
            //Registrar as alterações no banco de dados
            context.SaveChanges();

            return RedirectToAction("Index");
        }

        //Na rota de excluir, vamos capturar o id que vem na url
        [Route("ExcluirJogador/{idJogador}")]
        public IActionResult ExcluirJogador(int idJogador)
        {
            //Pegar o id de referência, e vou procurar a equipe no banco de dados. FirstOrDefault retorna nulo ou a primeira opção
            Jogador jogador = context.Jogadors.FirstOrDefault(x => x.Id == idJogador);// select*from EQUIPE WHERE ID==(VALOR DA EQUIPE DA TABELA) -->LAMBIDA É COMO ISSO DO SELECT
                                                                                      //lambida é uma condição dentro do item ,laço de repetição numa unica linha e fazer certas validação
            context.Remove(jogador);
            context.SaveChanges();
            return RedirectToAction("Index");
        }

         //Na rota de excluir, vamos capturar o id que vem na url
        [Route("Atualizar/{idJogador}")]
        public IActionResult Atualizar(int idJogador)
        {
             //Forma de listar todos os items da tabela de (Equipe)
            var listaEquipes = context.Equipes.ToList();
            //Passar para a tela (em forma de memória) os dados das equipes cadastradas
            ViewBag.listaEquipes = listaEquipes;
            
            //Pegar o id de referência, e vou procurar a equipe no banco de dados. FirstOrDefault retorna nulo ou a primeira opção
            Jogador jogador = context.Jogadors.FirstOrDefault(x => x.Id == idJogador);// select*from EQUIPE WHERE ID==(VALOR DA EQUIPE DA TABELA) -->LAMBIDA É COMO ISSO DO SELECT
                                                                                  //lambida é uma condição dentro do item ,laço de repetição numa unica linha e fazer certas validação
            ViewBag.Jogador = jogador;
            return View();
        }

        [Route("AtualizarJogador")]
        public IActionResult AtualizarEquipe(Jogador jogador)
        {
            context.Jogadors.Update(jogador);
            context.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}