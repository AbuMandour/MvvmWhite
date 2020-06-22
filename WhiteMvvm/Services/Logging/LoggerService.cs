using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AppCenter.Crashes;
using WhiteMvvm.Services.Dialog;
using WhiteMvvm.Utilities;

namespace WhiteMvvm.Services.Logging
{
    public class LoggerService : ILoggerService
    {
        private readonly IDialogService _dialogService;

        public LoggerService(IDialogService dialogService)
        {
            _dialogService = dialogService;
        }
        public Task LogException(Exception exception)
        {
#if DEBUG
            Console.WriteLine(exception.ToString());
            return _dialogService.ShowErrorAsync(exception.ToString());
#endif
            Crashes.TrackError(exception);
            return Task.CompletedTask;
        }

        public T Benchmark<T>(Func<T> function, string message)
        {
            var sw = Stopwatch.StartNew();
            var result = function.Invoke();
            sw.Stop();
            _dialogService.ShowErrorAsync($"{message}: {sw.ElapsedMilliseconds} milliseconds");
            return result;
        }

        public void Benchmark(Action function, string message)
        {
            var sw = Stopwatch.StartNew();
            function.Invoke();
            sw.Stop();
            _dialogService.ShowErrorAsync($"{message}: {sw.ElapsedMilliseconds} milliseconds");
        }
    }
}
