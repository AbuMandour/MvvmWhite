using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AppCenter.Crashes;
using WhiteMvvm.Services.Dialog;
using Xamarin.Forms;

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
            System.Diagnostics.Debug.WriteLine(exception.ToString());
            var message = "";
            message = !string.IsNullOrEmpty(exception.Message) ? exception.Message : "exception without message";
            return _dialogService.ShowErrorAsync(message);
#endif
            Crashes.TrackError(exception);
            return Task.CompletedTask;
        }

        public T Benchmark<T>(Func<T> function, string message = "duration: ", bool withPopup = true)
        {
            var sw = Stopwatch.StartNew();
            var result = function.Invoke();
            sw.Stop();
            var durationMessage = $"{message}: {sw.ElapsedMilliseconds} milliseconds";
            if (withPopup)
                _dialogService.ShowErrorAsync(durationMessage);
            System.Diagnostics.Debug.WriteLine(durationMessage);
            return result;
        }

        public void Benchmark(Action function, string message = "duration: ", bool withPopup = true)
        {
            var sw = Stopwatch.StartNew();
            function.Invoke();
            sw.Stop();
            var durationMessage = $"{message}: {sw.ElapsedMilliseconds} milliseconds";
            if (withPopup)
                _dialogService.ShowErrorAsync(durationMessage);
            System.Diagnostics.Debug.WriteLine(durationMessage);
        }
    }
}