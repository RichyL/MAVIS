using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Richy_WPF_MVVM.Common
{
    public class ViewModelBase : INotifyPropertyChanged
    {
        #region INotifyPropertyChanged

        public event PropertyChangedEventHandler PropertyChanged;

        //https://docs.microsoft.com/en-us/dotnet/api/system.componentmodel.inotifypropertychanged?view=dotnet-uwp-10.0&preserve-view=true
        protected void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion

        /// <summary>
        /// Should the ViewModel accept a message then this method would need to be overrideen. This method is called 
        /// prior to the ViewModel being bound to the View by the Navigation Service.
        /// </summary>
        /// <param name="message"></param>
        public virtual void SetMessage(object? message) { }
    }
}
