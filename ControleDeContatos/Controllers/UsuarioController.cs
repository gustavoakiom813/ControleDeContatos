using ControleDeContatos.Filters;
using ControleDeContatos.Models;
using ControleDeContatos.Repositorio;
using Microsoft.AspNetCore.Mvc;
using System.Text.RegularExpressions;

namespace ControleDeContatos.Controllers
{
    [PaginaRestritaSomenteAdmin]
    public class UsuarioController : Controller
    {
        private readonly IUsuarioRepositorio _usuarioRepositorio;

        public UsuarioController(IUsuarioRepositorio usuarioRepositorio)
        {
            _usuarioRepositorio = usuarioRepositorio;
        }

        public IActionResult Index()
        {
            List<UsuarioModel> usuarios = _usuarioRepositorio.BuscarTodos();

            return View(usuarios);
        }

        public IActionResult Criar()
        {
            return View();
        }

        public IActionResult Editar(int id)
        {
            UsuarioModel usuario = _usuarioRepositorio.ListarPorId(id);

            return View(usuario);
        }

        public IActionResult ApagarConfirmacao(int id)
        {
            UsuarioModel usuario = _usuarioRepositorio.ListarPorId(id);

            return View(usuario);
        }

        public IActionResult Apagar(int id)
        {
            try
            {
                bool apagado = _usuarioRepositorio.Apagar(id);

                if (apagado == true)
                {
                    TempData["MensagemSucesso"] = "Usuário excluído com sucesso!";
                }

                else
                {
                    TempData["MensagemErro"] = $"Erro ao excluir o usuário!";
                }
                return RedirectToAction("Index");
            }
            catch (Exception erro)
            {
                TempData["MensagemErro"] = $"Erro ao excluir o usuário, detalhe do erro: {erro.Message}";
                return RedirectToAction("Index");
            }
        }

        [HttpPost]
        public IActionResult Criar(UsuarioModel usuario)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    // 3. Salva no banco de dados
                    usuario = _usuarioRepositorio.Adicionar(usuario);

                    TempData["MensagemSucesso"] = "Usuário cadastrado com sucesso!";

                    return RedirectToAction("Index");
                }
                return View(usuario);
            }
            catch (Exception erro)
            {
                TempData["MensagemErro"] = $"Erro ao cadastrar o usuário, detalhe do erro: {erro.Message}";

                return RedirectToAction("Index");
            }
        }

        [HttpPost]
        public IActionResult Alterar(UsuarioSemSenhaModel usuarioSemSenhaModel)
        {
            try
            {
                UsuarioModel usuario = null;

                if (ModelState.IsValid)
                {
                    usuario = new UsuarioModel()
                    {
                        Id = usuarioSemSenhaModel.Id,
                        Nome = usuarioSemSenhaModel.Nome,
                        Login = usuarioSemSenhaModel.Login,
                        Email = usuarioSemSenhaModel.Email,
                        Perfil = usuarioSemSenhaModel.Perfil
                    };

                    // 1. Salva no banco de dados com o número limpo
                    usuario = _usuarioRepositorio.Atualizar(usuario);

                    TempData["MensagemSucesso"] = "Usuário alterado com sucesso!";

                    return RedirectToAction("Index");
                }
                return View("Editar", usuario);
            }
            catch (Exception erro)
            {
                TempData["MensagemErro"] = $"Erro ao editar o usuário, detalhe do erro: {erro.Message}";

                return RedirectToAction("Index");
            }
        }
    }
}
