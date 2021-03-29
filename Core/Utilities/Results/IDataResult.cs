using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Utilities.Results
{
    public interface IDataResult<T>:IResult
    {
        //IResult ın yaptığı şeyleri bu da zaten yapacak. Ek olarak Data da göndürecek.
        T Data { get; }
    }
}
