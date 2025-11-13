using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

[ApiController]
[Route("api/[controller]/[action]")]
public class AccountController : ControllerBase
{
    [HttpPost]
    public string Login([FromBody] LoginModel model)
    {
        if (model == null)
            return "";

        if (!(model.UserName == "ladminl" && model.Password == "#lpasswordl#"))
            return "";
        
        var key = Encoding.ASCII.GetBytes("your_secret_key_here"); // Same secret key as used in the Startup.cs
        var tokenHandler = new JwtSecurityTokenHandler();

        // Define the claims for the user
        var claims = new[]
        {
            new Claim(ClaimTypes.Name, "Admin"), // Replace with actual user data
            new Claim(ClaimTypes.Email, "ladminl@op.com"),
        };

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.UtcNow.AddDays(7), // Set token expiration time
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };

        var token = tokenHandler.CreateToken(tokenDescriptor);
        var tokenString = tokenHandler.WriteToken(token);

        return tokenString;
    }
}
