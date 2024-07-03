using Microsoft.IdentityModel.Tokens;
using System.Security.Cryptography;

namespace DTO.API.Auth
{
    public class SigningConfigurations
    {
        public SecurityKey Key { get; }
        public SigningCredentials SigningCredentials { get; }

        public SigningConfigurations()
        {
            using (var provider = new RSACryptoServiceProvider(2048))
                Key = new RsaSecurityKey(provider.ExportParameters(true));
            SigningCredentials = new SigningCredentials(Key, SecurityAlgorithms.RsaSha256Signature);

            // Caso a aplicação for colocada em um load balancer tem que usar uma security key estática, se não vai ficar deslogando o client
            // Abaixo comentado um código de exemplo:
            // Key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(AppConfig.StaticAppConfig.SecurityKey));
            // SigningCredentials = new SigningCredentials(Key, SecurityAlgorithms.HmacSha256);
        }
    }
}
