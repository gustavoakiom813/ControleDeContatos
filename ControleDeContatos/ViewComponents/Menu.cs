using ControleDeContatos.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace ControleDeContatos.ViewComponents
{
    public class Menu : ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync()
        {
            string sessaoUsuario = HttpContext.Session.GetString("sessaoUsuarioLogado");

            // Em vez de null, retorna um conteúdo de texto vazio
            if (string.IsNullOrEmpty(sessaoUsuario))
            {
                return Content("");
            }

            UsuarioModel usuario = JsonConvert.DeserializeObject<UsuarioModel>(sessaoUsuario);

            // Se a sua View precisa dos dados do usuário, lembre-se de passá-lo aqui
            return View(usuario);
        }
    }
}
