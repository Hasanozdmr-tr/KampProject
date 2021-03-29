using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Utilities.Security.JWT
{
    public class AccessToken //Erişim Anahtarı, kullanıcı adı ve şifre ile beraber gönderilir. Client tan Web Api ye.
    {
        //JWT için utiliti ler bunlar.

        public string Token { get; set; }
        public DateTime Expiration { get; set; }

    }
}
