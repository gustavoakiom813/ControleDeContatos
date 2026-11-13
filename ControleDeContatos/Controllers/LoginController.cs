using ControleDeContatos.Helper;
using ControleDeContatos.Models;
using ControleDeContatos.Repositorio;
using Microsoft.AspNetCore.Mvc;

namespace ControleDeContatos.Controllers
{
    public class LoginController : Controller
    {
        private readonly IUsuarioRepositorio _usuarioRepositorio;
        private readonly ISessao _sessao;
        private readonly IEmail _email;

        public LoginController(IUsuarioRepositorio usuarioRepositorio, ISessao sessao, IEmail email)
        {
            _usuarioRepositorio = usuarioRepositorio;
            _sessao = sessao;
            _email = email;
        }

        public IActionResult Index()
        {
            // Se o usuário estiver logado, redirecionar para a Home
            if (_sessao.BuscarSessaoUsuario() != null) return RedirectToAction("Index", "Home");

            return View();
        }

        [HttpPost]
        public IActionResult Logar(LoginModel loginModel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    UsuarioModel usuario = _usuarioRepositorio.BuscarPorLogin(loginModel.Login);

                    // Verifica se o usuário existe E se a senha é válida
                    if (usuario != null && usuario.SenhaValida(loginModel.Senha))
                    {
                        _sessao.CriarSessaoUsuario(usuario);

                        return RedirectToAction("Index", "Home");
                    }

                    // Mensagem genérica por motivos de segurança (evita enumeração de usuários)
                    TempData["MensagemErro"] = "Usuário e/ou senha inválido(s). Por favor, tente novamente.";
                }

                return View("Index", loginModel);
            }
            catch (Exception erro)
            {
                TempData["MensagemErro"] = $"Erro ao logar no sistema. Detalhe: {erro.Message}";
                return RedirectToAction("Index");
            }
        }

        [HttpPost]
        public IActionResult EnviarLinkParaRedefinirSenha(RedefinirSenhaModel redefinirSenhaModel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    UsuarioModel usuario = _usuarioRepositorio.BuscarPorEmailLogin(redefinirSenhaModel.Email, redefinirSenhaModel.Login);

                    if (usuario != null)
                    {
                        string novaSenha = usuario.GerarNovaSenha();
                        string mensagem = $"Sua nova senha é: {novaSenha}";
                        string assunto = "Sistema de Contatos - Nova Senha";

                        bool emailEnviado = _email.Enviar(usuario.Email, assunto, mensagem);

                        if (emailEnviado)
                        {
                            _usuarioRepositorio.Atualizar(usuario);
                            TempData["MensagemSucesso"] = "Enviamos para o seu e-mail cadastrado uma nova senha.";
                        }
                        else
                        {
                            TempData["MensagemErro"] = "Não foi possível enviar o e-mail, por favor tente novamente.";
                        }
                        return RedirectToAction("Index", "Login");
                    }
                    TempData["MensagemErro"] = "Erro ao redefinir a senha. Por favor, verifique os dados informados.";
                }

                return View("Index");
            }
            catch (Exception erro)
            {
                TempData["MensagemErro"] = $"Erro ao redefinir a senha. Detalhe: {erro.Message}";
                return RedirectToAction("Index");
            }
        }



        public IActionResult RedefinirSenha()
        {
            return View();
        }

        public IActionResult Sair()
        {
            _sessao.RemoverSessaoUsuario();

            return RedirectToAction("Index", "Login");
        }
    }
}
