using Microsoft.Identity.Client;
using System.ComponentModel.DataAnnotations;

namespace ControleDeContatos.Models
{
    public class ContatoModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Digite o nome do contato")]
        public string Nome { get; set; }

        [Required(ErrorMessage = "Digite o e-mail do contato")]
        [EmailAddress(ErrorMessage = "O e-mail informado não é válido!")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Digite o celular do contato")]
        [Phone(ErrorMessage = "O celular informado não é válido!")]
        // Esta Regex agora aceita os formatos: (11) 99999-9999 ou (11) 9999-9999
        [RegularExpression(@"^\(\d{2}\)\s\d{4,5}-\d{4}$", ErrorMessage = "O celular deve estar no formato (11) 99999-9999")]
        public string Celular { get; set; }

        public int? UsuarioId { get; set; }

        public UsuarioModel Usuario { get; set; }
    }
}
