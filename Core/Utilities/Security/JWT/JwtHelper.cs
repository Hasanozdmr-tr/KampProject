using Core.Entities.Concrete;
using Core.Extensions;
using Core.Utilities.Security.Encryption;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;


namespace Core.Utilities.Security.JWT
{
    
        public class JwtHelper : ITokenHelper
        {
            public IConfiguration Configuration { get; }   //Conf. appsetting deki verileri okuyacak. Set edilemeyecek.
            private TokenOptions _tokenOptions;
            private DateTime _accessTokenExpiration;
            public JwtHelper(IConfiguration configuration)
            {
                Configuration = configuration;   //injection yapıyor confg ile.
                _tokenOptions = Configuration.GetSection("TokenOptions").Get<TokenOptions>();
               //tokenoption ı appsetting içindeki get section yani token option alanını al. Tokenoptions a ata diyor.
            }
            public AccessToken CreateToken(User user, List<OperationClaim> operationClaims)
            {
            _accessTokenExpiration = DateTime.Now.AddMinutes(_tokenOptions.AccessTokenExpiration);
            // now dan itibaren dk ekle diyor. Ne kadar dk ? Appsettings deki tokenexpiration yani 10 dk kadar.
            var securityKey = SecurityKeyHelper.CreateSecurityKey(_tokenOptions.SecurityKey);
            // tokenoption daki sec.key i kullanarak security key i create et diyorum.
                var signingCredentials = SigningCredentialsHelper.CreateSigningCredentials(securityKey);
            var jwt = CreateJwtSecurityToken(_tokenOptions, user, signingCredentials, operationClaims);
            //option ları kullanarak claimleri (yetkileri) hangi kullanıcıyı da içeren bi metod la ya create etmişim.
                var jwtSecurityTokenHandler = new JwtSecurityTokenHandler();
                var token = jwtSecurityTokenHandler.WriteToken(jwt);

                return new AccessToken
                {
                    Token = token,
                    Expiration = _accessTokenExpiration
                };

            }

            public JwtSecurityToken CreateJwtSecurityToken(TokenOptions tokenOptions, User user,
                SigningCredentials signingCredentials, List<OperationClaim> operationClaims)
            {
                var jwt = new JwtSecurityToken(  //token oluşturuyor.
                    issuer: tokenOptions.Issuer,
                    audience: tokenOptions.Audience,
                    expires: _accessTokenExpiration,
                    notBefore: DateTime.Now,
                    claims: SetClaims(user, operationClaims),  //claimler için de ayrı bir metod yapılmış.
                    signingCredentials: signingCredentials
                );
                return jwt;
            }

            private IEnumerable<Claim> SetClaims(User user, List<OperationClaim> operationClaims)
            {      //claim aslında yetki ama daha fazlası da var. Userid, email, isim bla bla.
                var claims = new List<Claim>();
            // claim nesnesi normalde microsoft tarafından oluşturulmuş. Ama buna sen yeni özellikler ekleyebiliyorsun.
            //Örn: claims yazınca birsürü operasyon geliyor. Ama AddEmail yok. Bunu biz yazacağız. buna Extension deniyor.
                claims.AddNameIdentifier(user.Id.ToString());
                claims.AddEmail(user.Email);
                claims.AddName($"{user.FirstName} {user.LastName}");  // başına dolar yaparsan 2 stringi + ile yazmak yerine birlikte yazıyorsun.
                claims.AddRoles(operationClaims.Select(c => c.Name).ToArray());

                return claims;
            }
        }
    
}
