using Richy_WPF_MVVM.Common.Creation;
using Richy_WPF_MVVM.Common.Exceptions;
using System;
using System.Windows;

namespace Richy_WPF_MVVM.Common.Navigation
{
    public enum CommonDialogIcon { None, Error, Warning }

    public enum CommonDialogButton { OK, OkCancel }

    public enum CommonDialogResult { OK, Cancel, NoButtonPressed }


    public class NavigationService : INavigationService
    {
        

        private MainWindow _mainWindow;

        private AbstractFactoryBase? _factory;


        public void RegisterMainWindow(MainWindow window)
        {
            ArgumentNullException.ThrowIfNull(window, "The main window has not been created");
            _mainWindow = window;
        }


        public void RegisterFactory(AbstractFactoryBase factory)
        {
            _factory = factory;
        }

        public void NavigateTo(NavigationRoutes route,object? message=null)
        {
            CheckNavigationService();

            #pragma warning disable CS8602 // Dereference of a possibly null reference.
            var view = _factory.GetViewForRoute(route);


            var viewmodel = _factory.GetViewModelForRoute(route, message);

            view.DataContext = viewmodel;

            _mainWindow.MainView.Content = view;
            #pragma warning restore CS8602 // Dereference of a possibly null reference.

        }



        private void CheckNavigationService()
        {
            if (_factory == null) throw new NavigationServiceException("The navigation service has no factory registered");
            if (_mainWindow == null) throw new NavigationServiceException("The navigation service has no window registered");
        }

        #region DialogueCode

        SimpleDialog? _commonDialog;


        DialogResultAvailable? _dialogClosedCallback;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        /// <param name="title"></param>
        /// <param name="messageBoxButtons"></param>
        /// <param name="messageBoxImage"></param>
        /// <param name="messageBoxResult"></param>
        /// <param name="callback">It is the caller's responsability to remove this event handler from the code</param>
        public void ShowDialog(string message,
                               string title = "",
                               CommonDialogIcon messageBoxImage = CommonDialogIcon.None,
                               CommonDialogButton messageBoxButtons = CommonDialogButton.OK,
                               DialogResultAvailable? callback = null)
        {
            _commonDialog = new SimpleDialog(_mainWindow, message, title, messageBoxImage, messageBoxButtons);

            _dialogClosedCallback = callback;
            _commonDialog.DialogResultEvent += DialogClosed;

            _commonDialog.ShowDialog();

            System.Diagnostics.Debug.WriteLine(message);

        }

        private void DialogClosed(DialogResultEvent e)
        {

            if(_commonDialog != null)
            {
                _commonDialog.DialogResultEvent -= DialogClosed;
                _commonDialog = null;
            }

            if (_dialogClosedCallback != null)
            {
                _dialogClosedCallback(e);
            }

        }

        ComplexDialog _complexDialog;
        ComplexDialogClosed? _complexDialogClosedCallback;

        public void ShowComplexDialog(ComplexDialogRoute dialogRoute, ViewModelBase viewModel)
        {
            var view = _factory.GetViewForPopup(dialogRoute);

            view.DataContext = viewModel;

            _complexDialog = new ComplexDialog();
            _complexDialog.Owner = _mainWindow;

            _complexDialog.Closed += ComplexDialogClosed;
            _complexDialog.ComplexDialogContent.Content = view;

            _complexDialog.ShowDialog();
            
        }

        private void ComplexDialogClosed(object? sender, EventArgs e)
        {

            if (_complexDialog!= null)
            {
                _complexDialog.Closed -= ComplexDialogClosed;
                _complexDialog = null;
            }

        }

        public void CloseComplexDialog()
        {
            if( _complexDialog != null ) { _complexDialog.Close(); }
        }


        #endregion

    }

}
