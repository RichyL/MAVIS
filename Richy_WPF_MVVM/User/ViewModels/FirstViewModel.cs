using Richy_WPF_MVVM.Common.Navigation;
using Richy_WPF_MVVM.Common;
using System;

namespace Richy_WPF_MVVM.User.ViewModels
{
    public partial class FirstViewModel : ViewModelBase
    {
        private readonly INavigationService _navigationService;

        public FirstViewModel(INavigationService navigationService)
        {
            ArgumentException.ThrowIfNullOrEmpty(nameof(navigationService), "The Navigation Service has not been passed to this ViewModel");
            _navigationService = navigationService;

            InitiateMAVISCommands();
        }


        [MAVIS_Property]
        string viewModelMessage = "This text will be shown on Page 2";


        [MAVIS_Method]
        private void GotoView2()
        {
            _navigationService.NavigateTo(NavigationRoutes.SecondView, viewModelMessage);
        }


        [MAVIS_Property]
        string dialogueBoxResult = "The dialogue box has not been opened";


        [MAVIS_Method]
        private void OpenDialog()
        {
            _navigationService.ShowDialog("This diaglog shows a warning icon", "Page 1 Dialogue Box", CommonDialogIcon.Warning, CommonDialogButton.OkCancel, DialogClosed);
        }


        private void DialogClosed(DialogResultEvent e)
        {
            if (e.Result == CommonDialogResult.NoButtonPressed) DialogueBoxResultProperty = "The dialog was closed without the button";
            if (e.Result == CommonDialogResult.OK) DialogueBoxResultProperty = "The user clicked the OK button";
            if (e.Result== CommonDialogResult.Cancel) DialogueBoxResultProperty = "The user clicked the cancel button";
        }

        #region ComplexDialog

        [MAVIS_Property]
        string complexDialogText = "This text can only be edited by the dialog";

        [MAVIS_Method]
        private void OpenComplexDialog()
        {
            _navigationService.ShowComplexDialog(ComplexDialogRoute.Message, this);
        }

        [MAVIS_Method]
        private void CloseComplexDialog()
        {
            _navigationService?.CloseComplexDialog();
        }

        #endregion

    }
}
