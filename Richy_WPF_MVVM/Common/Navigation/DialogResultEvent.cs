using System;

namespace Richy_WPF_MVVM.Common.Navigation
{
    public delegate void DialogResultAvailable(DialogResultEvent e);

    public delegate void ComplexDialogClosed(object? o);

    public class DialogResultEvent : EventArgs
    {
        public CommonDialogResult Result { get; set; }
    }
}
