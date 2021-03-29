using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Utilities.Results
{
    public class Result : IResult
    {
        public bool Success { get; }

        public string Message { get; }

        public Result(bool success,string message):this(success) //-> This bu classı kastediyor.() demek constructure ı çağırmak demek.
                                                                   //costructure ıma success i gönder diyor.
        { //yukarıda property tanımlarken set tanımlamamamızın sebebi kodları standart hale getirmek.
          //Constructure ile set işlemini yaptıracağız şuan
            Message = message;
        }
        public Result(bool success)
        { //2 tane constructure ımız oldu. Overload etmiş olduk. Birden fazla kez çağırdık.
            Success = success;
        }



    }
}
