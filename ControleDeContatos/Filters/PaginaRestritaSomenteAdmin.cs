using ControleDeContatos.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Newtonsoft.Json;

namespace ControleDeContatos.Filters
{
    public class PaginaRestritaSomenteAdmin : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            string sessaoUsuario = context.HttpContext.Session.GetString("sessaoUsuarioLogado");
            UsuarioModel? usuario = null;

            if (!string.IsNullOrEmpty(sessaoUsuario))
            {
                try
                {
                    usuario = JsonConvert.DeserializeObject<UsuarioModel>(sessaoUsuario);
                }
                catch
                {
                    usuario = null;
                }
            }

            if (usuario == null)
            {
                context.Result = new RedirectToRouteResult(new RouteValueDictionary
                {
                    { "controller", "Login" },
                    { "action", "Index" }
                });
                return;
            }

            if (usuario.Perfil != Enum.PerfilEnum.Admin)
            {
                context.Result = new RedirectToRouteResult(new RouteValueDictionary
                {
                    { "controller", "Restrito" },
                    { "action", "Index" }
                });
                return;
            }

            base.OnActionExecuting(context);
        }
    }
}
