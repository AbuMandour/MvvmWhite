using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace WhiteMvvm.Services.Logging
{
    public interface ILoggerService
    {
        Task LogException(Exception exception);
        T Benchmark<T>(Func<T> function, string message = "duration: ", bool withPopup = true);
        void Benchmark(Action function, string message = "duration: ", bool withPopup = true);
    }
}
