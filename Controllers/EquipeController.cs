using MeuPrimeiroMvc.Contexts;
using MeuPrimeiroMvc.Models;
using Microsoft.AspNetCore.Mvc;

namespace MeuPrimeiroMvc.Controllers
{
    [Route("[controller]")]
    public class EquipeController : Controller
    {
        //Criar uma referência (instância )sobre a conmunicação do meu banco de dados
        ProjetoTesteContext context = new ProjetoTesteContext();

        public IActionResult Index()
        {
            //Forma de listar todos os items da tabela de (Equipe)
            var listaEquipes = context.Equipes.ToList();
            //Passar para a tela (em forma de memória) os dados das equipes cadastradas
            ViewBag.listaEquipes = listaEquipes;
            return View();
        }

        [Route("cadastrar")]
        public IActionResult CadastrarEquipe(IFormCollection formEquipe) //Recebendo dados no padrão FormData - para trabalhar com arquivos
        {
            if (formEquipe.Files.Count > 0)
            {
                //Recebendo o arquivo anexado
                var arquivosAnexados = formEquipe.Files[0]; //Dentro da possibilidade de receber vários arquivos, estamos recebendo apenas o primeiro(único)
                var pastaArmazenamento = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/img/equipe");
                //Directory.GetCurrentDirectory -->função pra pegar o caminho(localização) da pasta do projeto
                //Criar a pasta "wwwroot/" --> é o local configurado para acessar arquivos doi navegador
                if (!Directory.Exists(pastaArmazenamento)) //Verifiquei se a pasta de armazenamento não existe, e caso não exista automaticamente o projeto fica responsável por criar essa pasta
                {
                    Directory.CreateDirectory(pastaArmazenamento);

                    //Passando a localização da pasta de armazenamento+o nome do arquivo a ser salvo
                    var arquivoArmazenado = Path.Combine(pastaArmazenamento, arquivosAnexados.FileName);
                    //Chamamos uma função do c# para criação de arquivo - dentro da pasta de armazenamento
                    using (var stream = new FileStream(arquivoArmazenado, FileMode.Create))
                    {
                        //Para esse novo arquivo, copiamos o conteudo do arquivo anexado
                        arquivosAnexados.CopyTo(stream);
                    }
                    //Criando o objeto de equipe para cadastro
                    Equipe equipe = new Equipe()
                    {
                        Nome = formEquipe["Nome"],
                        Imagem = arquivosAnexados.FileName
                    };
                    //Armazenar a equipe no banco de dados
                    context.Add(equipe);
                }
                else
                {
                    Equipe equipe = new Equipe()
                    {
                        Nome = formEquipe["Nome"],
                        Imagem = "padrão.jpg"
                    };
                    //Armazenar a equipe 
                    context.Add(equipe);
                }
            }
            //Registrar as alterações no banco de dados
            context.SaveChanges();

            return RedirectToAction("Index");
        }
        //Na rota de excluir, vamos capturar o id que vem na url
        [Route("ExcluirEquipe/{idEquipe}")]
        public IActionResult ExcluirEquipe(int idEquipe)
        {
            //Verificar se existe jogadores que contenham o vínculo com a equipe
            List<Jogador> listaJogadores = context.Jogadors.Where(x => x.IdEquipe == idEquipe).ToList();

            if (listaJogadores.Count > 0)
            {
                //Remove todos os j0pgadores vinculados
                foreach (Jogador jogador in listaJogadores)
                {
                    context.Remove(jogador);
                }
                context.SaveChanges();
            }
            //Pegar o id de referência, e vou procurar a equipe no banco de dados. FirstOrDefault retorna nulo ou a primeira opção
            Equipe equipe = context.Equipes.FirstOrDefault(x => x.Id == idEquipe);// select*from EQUIPE WHERE ID==(VALOR DA EQUIPE DA TABELA) -->LAMBIDA É COMO ISSO DO SELECT
                                                                                  //lambida é uma condição dentro do item ,laço de repetição numa unica linha e fazer certas validação
            context.Remove(equipe);
            context.SaveChanges();
            return RedirectToAction("Index");
        }
        //Na rota de excluir, vamos capturar o id que vem na url
        [Route("Atualizar/{idEquipe}")]
        public IActionResult Atualizar(int idEquipe)
        {
            //Pegar o id de referência, e vou procurar a equipe no banco de dados. FirstOrDefault retorna nulo ou a primeira opção
            Equipe equipe = context.Equipes.FirstOrDefault(x => x.Id == idEquipe);// select*from EQUIPE WHERE ID==(VALOR DA EQUIPE DA TABELA) -->LAMBIDA É COMO ISSO DO SELECT
                                                                                  //lambida é uma condição dentro do item ,laço de repetição numa unica linha e fazer certas validação
            ViewBag.Equipe = equipe;
            return View();
        }
        [Route("AtualizarEquipe")]
        public IActionResult AtualizarEquipe(Equipe equipe)
        {
            context.Equipes.Update(equipe);
            context.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}