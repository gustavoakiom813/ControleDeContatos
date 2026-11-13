using ControleDeContatos.Filters;
using ControleDeContatos.Helper;
using ControleDeContatos.Models;
using ControleDeContatos.Repositorio;
using Microsoft.AspNetCore.Mvc;
using System.Text.RegularExpressions;

namespace ControleDeContatos.Controllers
{
    [PaginaUsuarioLogado]
    public class ContatoController : Controller
    {
        private readonly IContatoRepositorio _contatoRepositorio;
        private readonly ISessao _sessao;
        public ContatoController(IContatoRepositorio contatoRepositorio, ISessao sessao)
        {
            _contatoRepositorio = contatoRepositorio;
            _sessao = sessao;
        }

        public IActionResult Index()
        {
            UsuarioModel usuarioLogado = _sessao.BuscarSessaoUsuario();
            List<ContatoModel> contatos = _contatoRepositorio.BuscarTodos(usuarioLogado.Id);

            return View(contatos);
        }

        public IActionResult Criar()
        {
            return View();
        }

        public IActionResult Editar(int id)
        {
            ContatoModel contato = _contatoRepositorio.ListarPorId(id);

            return View(contato);
        }

        public IActionResult ApagarConfirmacao(int id)
        {
            ContatoModel contato = _contatoRepositorio.ListarPorId(id);

            return View(contato);
        }

        public IActionResult Apagar(int id)
        {
            try
            {
                bool apagado = _contatoRepositorio.Apagar(id);

                if(apagado == true)
                {
                    TempData["MensagemSucesso"] = "Contato excluído com sucesso!";
                }

                else
                {
                    TempData["MensagemErro"] = $"Erro ao excluir o contato!";
                }
                    return RedirectToAction("Index");
            }
            catch (Exception erro)
            {
                TempData["MensagemErro"] = $"Erro ao excluir o contato, detalhe do erro: {erro.Message}";
                return RedirectToAction("Index");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Criar(ContatoModel contato)
        {
            try
            {
                ModelState.Remove("Usuario");

                if (ModelState.IsValid)
                {
                    UsuarioModel usuarioLogado = _sessao.BuscarSessaoUsuario();
                    if (usuarioLogado == null)
                    {
                        TempData["MensagemErro"] = "Sua sessão expirou. Faça login novamente.";
                        return RedirectToAction("Index", "Login");
                    }

                    if (!string.IsNullOrEmpty(contato.Celular))
                    {
                        contato.Celular = Regex.Replace(contato.Celular, @"\D", "");
                    }

                    contato.UsuarioId = usuarioLogado.Id;
                    contato = _contatoRepositorio.Adicionar(contato);

                    TempData["MensagemSucesso"] = "Contato cadastrado com sucesso!";
                    return RedirectToAction("Index");
                }

                return View(contato);
            }
            catch (Exception erro)
            {
                TempData["MensagemErro"] = $"Erro ao cadastrar o contato, detalhe do erro: {erro.Message}";
                return RedirectToAction("Index");
            }
        }

        //[HttpPost]
        //public IActionResult Alterar(ContatoModel contato)
        //{
        //    try
        //    {
        //        // 1. Valida o modelo com a máscara ainda ativa (garante que o formato digitado está correto)
        //        if (ModelState.IsValid)
        //        {
        //            // 2. Remove tudo o que NÃO for número (\D) antes de mandar para o banco
        //            if (!string.IsNullOrEmpty(contato.Celular))
        //            {
        //                contato.Celular = Regex.Replace(contato.Celular, @"\D", "");
        //                // Exemplo: "(11) 99999-9999" vira "11999999999"                        
        //            }

        //            UsuarioModel usuarioLogado = _sessao.BuscarSessaoUsuario();
        //            contato.UsuarioId = usuarioLogado.Id;

        //            // 3. Salva no banco de dados com o número limpo
        //            _contatoRepositorio.Atualizar(contato);

        //            TempData["MensagemSucesso"] = "Contato alterado com sucesso!";

        //            return RedirectToAction("Index");
        //        }
        //        return View("Editar", contato);
        //    }
        //    catch (Exception erro)
        //    {
        //        TempData["MensagemErro"] = $"Erro ao editar o contato, detalhe do erro: {erro.Message}";

        //        return RedirectToAction("Index");
        //    }
        //}
        [HttpPost]
        public IActionResult Alterar(ContatoModel contato)
        {
            try
            {
                // Ignora a validação da propriedade de navegação Usuario
                ModelState.Remove("Usuario");

                if (ModelState.IsValid)
                {
                    if (!string.IsNullOrEmpty(contato.Celular))
                    {
                        contato.Celular = Regex.Replace(contato.Celular, @"\D", "");
                    }

                    UsuarioModel usuarioLogado = _sessao.BuscarSessaoUsuario();
                    contato.UsuarioId = usuarioLogado.Id;

                    _contatoRepositorio.Atualizar(contato);

                    TempData["MensagemSucesso"] = "Contato alterado com sucesso!";
                    return RedirectToAction("Index");
                }

                return View("Editar", contato);
            }
            catch (Exception erro)
            {
                TempData["MensagemErro"] = $"Erro ao editar o contato, detalhe do erro: {erro.Message}";
                return RedirectToAction("Index");
            }
        }

    }
}
