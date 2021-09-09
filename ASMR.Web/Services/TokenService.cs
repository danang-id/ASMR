using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using ASMR.Core.Entities;
using ASMR.Web.Configurations;
using ASMR.Web.Data;
using ASMR.Web.Services.Generic;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace ASMR.Web.Services
{
	public interface ITokenService : IServiceBase
	{
		public NormalizedUserWithToken BuildToken(User user, IEnumerable<UserRole> userRoles);
		public bool ValidateToken(string token);
	}
	
	public class TokenService : ServiceBase, ITokenService
	{
		private const double ExpiryDurationMinutes = 30;
		private readonly string _issuer;
		private readonly string _key;
		
		public TokenService(ApplicationDbContext dbContext,
			IOptions<JsonWebTokenOptions> options) : base(dbContext)
		{
			_issuer = options.Value.Issuer;
			_key = options.Value.Issuer;
		}

		public NormalizedUserWithToken BuildToken(User user, IEnumerable<UserRole> userRoles)
		{
			var roleList = userRoles.ToList();
			var claims = new List<Claim>
			{
				new(ClaimTypes.NameIdentifier, user.Id),
				new(ClaimTypes.Name, user.UserName),
				new("FirstName", user.FirstName),
				new("LastName", user.LastName),
				new("Image", user.Image)
			};
			claims.AddRange(roleList.Select(userRole => new Claim(ClaimTypes.Role, userRole.Name)));

			var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_key));        
			var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256Signature);           
			var tokenDescriptor = new JwtSecurityToken(_issuer, _issuer, claims, 
				expires: DateTime.Now.AddMinutes(ExpiryDurationMinutes), signingCredentials: credentials);        
			var token = new JwtSecurityTokenHandler().WriteToken(tokenDescriptor);

			return user.ToNormalizedUserWithToken(token, roleList);
		}

		public bool ValidateToken(string token)
		{
			var mySecret = Encoding.UTF8.GetBytes(_key);           
			var mySecurityKey = new SymmetricSecurityKey(mySecret);
			var tokenHandler = new JwtSecurityTokenHandler();
			var tokenValidationParameters = new TokenValidationParameters
			{
				ValidateIssuerSigningKey = true,
				ValidateIssuer = true,
				ValidateAudience = true,
				ValidIssuer = _issuer,
				ValidAudience = _issuer,
				IssuerSigningKey = mySecurityKey,
			};
			try 
			{
				tokenHandler.ValidateToken(token, tokenValidationParameters, out var _);            
			}
			catch
			{
				return false;
			}
			
			return true;    
		}
	}
}