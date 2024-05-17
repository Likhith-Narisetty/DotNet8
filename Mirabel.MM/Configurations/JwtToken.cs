using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.Data.SqlClient;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

namespace Mirabel.MM.Configurations
{
    public class JWTtoken
    {
        public static string GenerateJSONWebToken()
        {
            IConfigurationBuilder builder = new ConfigurationBuilder().AddJsonFile("appsettings.json", false, true);
            IConfigurationRoot root = builder.Build();
            string Key = "Jwt:Key";
            string JwtKey = root[Key].ToString();
            string Issuer = "Jwt:Issuer";
            string JwtIssuer = root[Issuer].ToString();

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(JwtKey));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(JwtIssuer,
             JwtIssuer,
              null,
              expires: DateTime.Now.AddMinutes(120),
              signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }



    }
}
 