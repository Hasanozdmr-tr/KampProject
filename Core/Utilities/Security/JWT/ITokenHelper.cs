
using Core.Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Utilities.Security.JWT
{
    public interface ITokenHelper  //ileride JWT kullanılmayabilir oyüzden interface e yazarız bu operasyonu.
    {
        AccessToken CreateToken(User user, List<OperationClaim> operationClaims);
        // ilgili kullanıcının claimlerini içeren bir token üretecek bu.
    }
}
