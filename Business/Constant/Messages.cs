using Core.Entities.Concrete;
using Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace Business.Constant
{
    public static class Messages
    //static yaparsan classın hiç newlenmesine gerek kalmaz.
    {
        public static string ProductAdded = "Ürün Eklendi.";
        public static string ProductNameInValid = "Ürün ismi geçersiz.";

        public static string MaintenenceTime = " Sistem bakımda olduğundan işleminize devam edilemiyor.";
        public static string ProductsListed = " Ürünler Listelendi. ";

        public static string UnitPRiceInValid = "Ürün birim fiyatı geçersiz.";

        public static string ProductCountOfCategoryError = "Bir kategoride en fazla 10 ürün olabilir.";

        public static string ProductNameAlreadyExists = "Bu isimde başka bir kayıt var.";

        public static string CategoryLimitExceded = "Kategori limiti aşıldığı için yeni ürün eklenemiyor.";
        public static string  AuthorizationDenied = "Yetkiniz yok.";

        public static string UserRegistered = "Kayıt Oldu.";
        public static string UserNotFound = "Kullanıcı bulunamadı.";

        public static string PasswordError = "Parola Hatası.";

        public static string SuccessfulLogin = "Başarılı Giriş";

        public static string UserAlreadyExists = "Kullanıcı mevcut.";

        public static string AccessTokenCreated = "Token oluşturuldu.";
    }
}
