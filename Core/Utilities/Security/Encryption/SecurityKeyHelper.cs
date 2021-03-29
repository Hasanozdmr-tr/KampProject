using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Utilities.Security.Encryption
{
    public class SecurityKeyHelper // şifrelenen her şeyi byte array ile vermemiz gerekiyor. Çevirimi bu yapacak
                                   // Api nin anlaması için. String ile olmuyor.

    {
        public static SecurityKey CreateSecurityKey (string securityKey)
        {
            //tokenoptions daki securitykey i yani appsettings.json daki keyi kullanarak yeni bir nesne üretecek.

            return new SymmetricSecurityKey(Encoding.UTF8.GetBytes(securityKey));

        }

    }
}
