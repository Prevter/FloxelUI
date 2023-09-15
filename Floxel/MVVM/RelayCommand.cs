using System;
using System.Windows.Input;

namespace FloxelLib.MVVM;

[AttributeUsage(AttributeTargets.Method)]
public sealed class RelayCommand : Attribute, ICommand
{
	readonly Action<object?>? _execute;
	readonly bool _hasArg;
	readonly Action? _executeNoArgs;
	readonly Predicate<object?>? _canExecute;

	public RelayCommand() : this((e) => { }, null) { }

	public RelayCommand(Action<object?> execute) : this(execute, null)
	{
	}
	
	public RelayCommand(Action execute) : this(execute, null)
	{
	}
	
	public RelayCommand(Action<object?> execute, Predicate<object?>? canExecute)
	{
		_execute = execute ?? throw new ArgumentNullException(nameof(execute));
		_canExecute = canExecute;
		_hasArg = true;
	}

	public RelayCommand(Action execute, Predicate<object?>? canExecute)
	{
		_executeNoArgs = execute ?? throw new ArgumentNullException(nameof(execute));
		_canExecute = canExecute;
	}

	public bool CanExecute(object? parameters)
	{
		return _canExecute == null || _canExecute(parameters);
	}

	public event EventHandler? CanExecuteChanged
	{
		add { CommandManager.RequerySuggested += value; }
		remove { CommandManager.RequerySuggested -= value; }
	}

	public void Execute(object? parameters)
	{
		if (_hasArg) _execute!(parameters);
		else if (_executeNoArgs is not null) _executeNoArgs();
	}
}