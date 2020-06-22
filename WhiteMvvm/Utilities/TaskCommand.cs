using System;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows.Input;
using WhiteMvvm.Exceptions;
using Xamarin.Forms;

namespace WhiteMvvm.Utilities
{
    /// <summary>
    /// An implmentation of ITaskCommand. Allows Commands to safely be used asynchronously with Task.
    /// </summary>
    public sealed class TaskCommand<T> : ITaskCommand<T>
    {
        private readonly Func<T, Task> _execute;
        private readonly Func<object?, bool> _canExecute;
        private readonly Action<Exception>? _onException;
        private readonly bool _continueOnCapturedContext;
        private readonly WhiteMvvm.WeakEventManager.WeakEventManager _weakEventManager = new WhiteMvvm.WeakEventManager.WeakEventManager();
        private volatile bool _inProgress;

        /// <summary>
        /// Initializes a new instance of the <see cref="T:WhiteMvvm.Utilities.TaskCommand`1"/> class.
        /// </summary>
        /// <param name="execute">The Function executed when Execute or ExecuteAysnc is called. This does not check canExecute before executing and will execute even if canExecute is false</param>
        /// <param name="canExecute">The Function that verifies whether or not TaskCommand should execute.</param>
        /// <param name="onException">If an exception is thrown in the Task, <c>onException</c> will execute. If onException is null, the exception will be re-thrown</param>
        /// <param name="continueOnCapturedContext">If set to <c>true</c> continue on captured context; this will ensure that the Synchronization Context returns to the calling thread. If set to <c>false</c> continue on a different context; this will allow the Synchronization Context to continue on a different thread</param>
        public TaskCommand(Func<T, Task> execute,
                            Func<object?, bool>? canExecute = null,
                            Action<Exception>? onException = null,
                            bool continueOnCapturedContext = false)
        {
            _execute = execute ?? throw new ArgumentNullException(nameof(execute), $"{nameof(execute)} cannot be null");
            _canExecute = canExecute ?? (_ => true);
            _onException = onException;
            _continueOnCapturedContext = continueOnCapturedContext;
        }

        /// <summary>
        /// Occurs when changes occur that affect whether or not the command should execute
        /// </summary>
        public event EventHandler CanExecuteChanged
        {
            add => _weakEventManager.AddEventHandler(value);
            remove => _weakEventManager.RemoveEventHandler(value);
        }

        /// <summary>
        /// Determines whether the command can execute in its current state
        /// </summary>
        /// <returns><c>true</c>, if this command can be executed; otherwise, <c>false</c>.</returns>
        /// <param name="parameter">Data used by the command. If the command does not require data to be passed, this object can be set to null.</param>
        public bool CanExecute(object? parameter)
        {
            if (_inProgress)
            {
                return false;
            }
            if (_canExecute != null)
            {
                return _canExecute(parameter);
            }
            return true;
        }

        /// <summary>
        /// Raises the CanExecuteChanged event.
        /// </summary>
        public void RaiseCanExecuteChanged() => _weakEventManager.HandleEvent(this, EventArgs.Empty, nameof(CanExecuteChanged));

        /// <summary>
        /// Executes the Command as a Task
        /// </summary>
        /// <returns>The executed Task</returns>
        /// <param name="parameter">Data used by the command. If the command does not require data to be passed, this object can be set to null.</param>
        public async Task ExecuteAsync(T parameter)
        {
            _inProgress = true;
            RaiseCanExecuteChanged();
            await Device.InvokeOnMainThreadAsync((() =>
            {
                _execute.Invoke(parameter)?.ContinueWith((task) =>
                {
                    _inProgress = false;
                    RaiseCanExecuteChanged();
                });
            }));

        }

        void ICommand.Execute(object parameter)
        {
            switch (parameter)
            {
                case T validParameter:
                    ExecuteAsync(validParameter).SafeFireAndForget(_continueOnCapturedContext, _onException);
                    break;

#pragma warning disable CS8601 //Possible null reference assignment
                case null when !typeof(T).GetTypeInfo().IsValueType:
                    ExecuteAsync((T)parameter).SafeFireAndForget(_continueOnCapturedContext, _onException);
                    break;
#pragma warning restore CS8601

                case null:
                    throw new InvalidCommandParameterException(typeof(T));

                default:
                    throw new InvalidCommandParameterException(typeof(T), parameter.GetType());
            }
        }
    }

    /// <summary>
    /// An implmentation of ITaskCommand. Allows Commands to safely be used asynchronously with Task.
    /// </summary>
    public sealed class TaskCommand : ITaskCommand
    {
        private readonly Func<Task> _execute;
        private volatile bool _inProgress;
        private readonly Func<object?, bool> _canExecute;
        private readonly Action<Exception>? _onException;
        private readonly bool _continueOnCapturedContext;
        private readonly WhiteMvvm.WeakEventManager.WeakEventManager _weakEventManager = new WhiteMvvm.WeakEventManager.WeakEventManager();

        /// <summary>
        /// Initializes a new instance of the <see cref="T:TaskExtensions.MVVM.TaskCommand`1"/> class.
        /// </summary>
        /// <param name="execute">The Function executed when Execute or ExecuteAysnc is called. This does not check canExecute before executing and will execute even if canExecute is false</param>
        /// <param name="canExecute">The Function that verifies whether or not TaskCommand should execute.</param>
        /// <param name="onException">If an exception is thrown in the Task, <c>onException</c> will execute. If onException is null, the exception will be re-thrown</param>
        /// <param name="continueOnCapturedContext">If set to <c>true</c> continue on captured context; this will ensure that the Synchronization Context returns to the calling thread. If set to <c>false</c> continue on a different context; this will allow the Synchronization Context to continue on a different thread</param>
        public TaskCommand(Func<Task> execute,
                            Func<object?, bool>? canExecute = null,
                            Action<Exception>? onException = null,
                            bool continueOnCapturedContext = false)
        {
            _execute = execute ?? throw new ArgumentNullException(nameof(execute), $"{nameof(execute)} cannot be null");
            _canExecute = canExecute ?? (_ => true);
            _onException = onException;
            _continueOnCapturedContext = continueOnCapturedContext;
        }

        /// <summary>
        /// Occurs when changes occur that affect whether or not the command should execute
        /// </summary>
        public event EventHandler CanExecuteChanged
        {
            add => _weakEventManager.AddEventHandler(value);
            remove => _weakEventManager.RemoveEventHandler(value);
        }

        /// <summary>
        /// Determines whether the command can execute in its current state
        /// </summary>
        /// <returns><c>true</c>, if this command can be executed; otherwise, <c>false</c>.</returns>
        /// <param name="parameter">Data used by the command. If the command does not require data to be passed, this object can be set to null.</param>
        public bool CanExecute(object? parameter)
        {
            if (_inProgress)
            {
                return false;
            }
            if (_canExecute != null)
            {
                return _canExecute(parameter);
            }
            return true;
        }

        /// <summary>
        /// Raises the CanExecuteChanged event.
        /// </summary>
        public void RaiseCanExecuteChanged() => _weakEventManager.HandleEvent(this, EventArgs.Empty, nameof(CanExecuteChanged));

        /// <summary>
        /// Executes the Command as a Task
        /// </summary>
        /// <returns>The executed Task</returns>
        public async Task ExecuteAsync()
        {
            _inProgress = true;
            RaiseCanExecuteChanged();
            await _execute()?.ContinueWith((task) =>
            {
                _inProgress = false;
                RaiseCanExecuteChanged();
            });
        }

        void ICommand.Execute(object parameter)
        {
            _execute().SafeFireAndForget(_continueOnCapturedContext, _onException);
        }
    }
}
