using System.Security.Cryptography;
using System.Text;

namespace ControleDeContatos.Helper
{
    public static class Criptografia
    {
        public static string GerarHash(this string valor)
        {
            using var hash = SHA256.Create();
            var bytes = Encoding.UTF8.GetBytes(valor);
            var hashBytes = hash.ComputeHash(bytes);

            var strHexa = new StringBuilder();
            foreach (var item in hashBytes)
            {
                strHexa.Append(item.ToString("x2"));
            }

            return strHexa.ToString();
        }
    }
}
