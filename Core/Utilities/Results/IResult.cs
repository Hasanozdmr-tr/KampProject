using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Utilities.Results
{
    public interface IResult
    {
        // Temel Void İşlemleri için
        bool Success { get; } //sadece işlem sonucu, get (okuma) işlemi olacak. (set olsaydı yazma işlemi de yapılabilirdi.)
        string Message { get;  }

    }
}
