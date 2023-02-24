using Richy_WPF_MVVM.Common.Navigation;
using Richy_WPF_MVVM.Common;
using Richy_WPF_MVVM.User.Services;

namespace Richy_WPF_MVVM.User.ViewModels
{
    public partial class SecondViewModel : ViewModelBase
    {
        private readonly INavigationService _navigationService;
        private readonly ReverseStringService _reverseStringService;

        public SecondViewModel(INavigationService navigationService, ReverseStringService reverseStringService)
        {
            _navigationService = navigationService;
            _reverseStringService = reverseStringService;
            InitiateMAVISCommands();
        }

        [MAVIS_Property]
        string originalMessage = "Second View Model";

        [MAVIS_Property]
        string reversedMessage;

        public override void SetMessage(object? message)
        {
            if(message == null || message is not string) 
            { reversedMessage = "No message to reverse!";
                return;
            }

            var messageAsString = (string)message;

            originalMessage = messageAsString;
            reversedMessage = _reverseStringService.ReverseString(messageAsString);

        }

        [MAVIS_Method]
        private void GotoView1()
        {
            _navigationService.NavigateTo(NavigationRoutes.FirstView);
        }

  
    }
}
