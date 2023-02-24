namespace MAVIS_Source_Generators
{
    [Generator]
    public class GenericCommandGenerator : IIncrementalGenerator
    {
        public void Initialize(IncrementalGeneratorInitializationContext context)
        {
            var hint = "GenericCommand.g.cs";
            var markerSource = @"

using System;
using System.Windows.Input;

namespace  Richy_WPF_MVVM.Common
{
    /// <summary>
    /// Generic can execute handler
    /// </summary>
    /// <param name=""parameter""></param>
    /// <returns></returns>
    public delegate bool GenericCommandCanExecuteHandler(object parameter);

    /// <summary>
    /// Generic execute handler
    /// </summary>
    /// <param name=""parameter""></param>
    public delegate void GenericCommandExecuteHandler(object parameter);

    /// <summary>
    /// Generic command processing
    /// </summary>
    public class GenericCommand : ICommand
    {
     

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name=""canExec""></param>
        /// <param name=""doExec""></param>
        public GenericCommand(Func<object, bool> canExec, Action<object> doExec)
        {
            CanExecuteDelegate += p => canExec(p);
            Executed += p => doExec(p);
        }

        /// <summary>
        /// Returns result of invoked CanExecute method
        /// </summary>
        /// <param name=""parameter""></param>
        /// <returns></returns>
        public bool CanExecute(object parameter)
        {
            return CanExecuteDelegate.Invoke(parameter);
        }

        private EventHandler? _internalCanExecuteChanged;

        /// <summary>
        /// Can execute changed event handler
        /// </summary>
        public event EventHandler? CanExecuteChanged
        {
            add
            {
                _internalCanExecuteChanged += value;
                CommandManager.RequerySuggested += value;
            }
            remove
            {
                _internalCanExecuteChanged -= value;
                CommandManager.RequerySuggested -= value;
            }
        }

        /// <summary>
        /// This method can be used to raise the CanExecuteChanged handler.
        /// This will force WPF to re-query the status of this command directly.
        /// </summary>
        public void RaiseCanExecuteChanged()
        {
            OnCanExecuteChanged();
        }

        /// <summary>
        /// This method is used to walk the delegate chain and well WPF that
        /// our command execution status has changed.
        /// </summary>
        protected virtual void OnCanExecuteChanged()
        {
            _internalCanExecuteChanged?.Invoke(this, EventArgs.Empty);
        }

        /// <summary>
        /// Can execute handler
        /// </summary>
        public event GenericCommandCanExecuteHandler? CanExecuteDelegate;

        /// <summary>
        /// Execute handler
        /// </summary>
        public event GenericCommandExecuteHandler? Executed;

        /// <summary>
        /// Execute
        /// </summary>
        /// <param name=""parameter"">Object reference</param>
        public void Execute(object parameter)
        {
            if (Executed != null)
            {
                Executed.Invoke(parameter);
            }
        }
    }
}
";

            context.RegisterPostInitializationOutput(ctx => { ctx.AddSource(hint, markerSource); });
        }
    }
}
