﻿using System;
using System.Collections.Generic;
using System.Text;

namespace WhiteMvvm.Utilities
{
    /// <summary>
    /// An Async implementation of ICommand for Task
    /// </summary>
    public interface ITaskCommand<T> : System.Windows.Input.ICommand
    {
        /// <summary>
        /// Executes the Command as a Task
        /// </summary>
        /// <returns>The Task to execute</returns>
        /// <param name="parameter">Data used by the command. If the command does not require data to be passed, this object can be set to null.</param>
        System.Threading.Tasks.Task ExecuteAsync(T parameter);

        /// <summary>
        /// Raises the CanExecuteChanged event.
        /// </summary>
        void RaiseCanExecuteChanged();
    }

    /// <summary>
    /// An Async implementation of ICommand for Task
    /// </summary>
    public interface ITaskCommand : System.Windows.Input.ICommand
    {
        /// <summary>
        /// Executes the Command as a Task
        /// </summary>
        /// <returns>The Task to execute</returns>
        System.Threading.Tasks.Task ExecuteAsync();

        /// <summary>
        /// Raises the CanExecuteChanged event.
        /// </summary>
        void RaiseCanExecuteChanged();
    }
}

