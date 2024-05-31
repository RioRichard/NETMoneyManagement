using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using NETAPI.Models;

namespace NETAPI.Helper;

public static class SessionHelper
{
    public static string GenerateToken(Account account, string secret = "")
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(secret);

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new[] { new Claim(ClaimTypes.NameIdentifier, account.Id.ToString()) }),
            Expires = DateTime.UtcNow.AddDays(30), // Token expiration time
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };

        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }

    public static bool GetUserId(HttpContext context, out int id)
    {
        id = -1;
        var user = context.User;
        var check = user.Claims.FirstOrDefault(p => p.Type == ClaimTypes.NameIdentifier);
        if (check == null)
            return false;
        var checkParse = int.TryParse(check.Value, out id);
        return checkParse;
    }
}