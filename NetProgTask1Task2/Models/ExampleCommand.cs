using System;
using System.Windows.Input;

namespace NetProgTask1Task2.Models;

// пример реализации команды, общего назначения, определяемой программистом
public class ExampleCommand : ICommand
{
    // Делегат - полезное действие команды
    readonly Action<object> _execute;

    // Делегат - проверка возможности выполнения команды
    readonly Predicate<object> _canExecute;


    public ExampleCommand(Action<object> execute) : this(execute, null!) { }

    public ExampleCommand(Action<object> execute, Predicate<object> canExecute) {

        _execute = execute ?? throw new ArgumentNullException(nameof(execute));
        _canExecute = canExecute;

    } // ExampleCommand


    public bool CanExecute(object parameter) => _canExecute?.Invoke(parameter) ?? true;


    public event EventHandler CanExecuteChanged {

        add => CommandManager.RequerySuggested += value;
        remove => CommandManager.RequerySuggested -= value;

    } // CanExecuteChanged


    public void Execute(object parameter) => _execute.Invoke(parameter);

} // class ExampleCommand