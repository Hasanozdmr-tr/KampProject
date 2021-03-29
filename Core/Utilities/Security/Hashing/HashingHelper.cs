using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Utilities.Security.Hashing
{
    public class HashingHelper
    {
        public static void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt )
        {
            using(var hmac = new System.Security.Cryptography.HMACSHA512()) //bunun ile hash ve salt oluşturacağız.
            {
                passwordSalt = hmac.Key; // her kullanıcı için farklı bir key hatta salt oluşur.Oldukça güvenlidir.
                passwordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password)); //password u byte e çeviriyor hash oluşturuyor.
            }

        }

        public static bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
        {   // kullanıcının gönderdiği password ile hash lediği key i karşılaştırıp doğru mu diye kontrol eder.

            using (var hmac = new System.Security.Cryptography.HMACSHA512(passwordSalt)) //bunun ile hash ve salt oluşturacağız.
            {
                var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
                for (int i=0; i< computedHash.Length;i++)
                {
                    if(computedHash[i]!=passwordHash[i])
                    {
                        return false;
                    }
                }
                return true;
            }
            
        }
    }
}
