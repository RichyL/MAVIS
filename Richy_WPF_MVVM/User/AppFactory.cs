using Richy_WPF_MVVM.Common.Creation;
using Richy_WPF_MVVM.Common.Exceptions;
using Richy_WPF_MVVM.Common.Navigation;
using System.Windows.Controls;
using Richy_WPF_MVVM.User.Views;
using Richy_WPF_MVVM.User.ViewModels;
using Richy_WPF_MVVM.Common;
using Richy_WPF_MVVM.User.Services;
using Richy_WPF_MVVM.User.ComplexDialogs;

namespace Richy_WPF_MVVM.User;

public class AppFactory : AbstractFactoryBase
{
    //Example of a Service being instantiated which can then be injected into ViewModels as required
    ReverseStringService _reverseStringService = new ReverseStringService();

    public AppFactory(INavigationService navigationService) : base(navigationService)
    {
    }

    public override ViewModelBase GetViewModelForRoute(NavigationRoutes routeName,object? message=null)
    {
        switch (routeName)
        {
            case NavigationRoutes.Home:
            case NavigationRoutes.FirstView:
                return new FirstViewModel(_navigationService);
            case NavigationRoutes.SecondView:
                SecondViewModel secondViewModel = new SecondViewModel(_navigationService, _reverseStringService);
                secondViewModel.SetMessage(message);
                return secondViewModel;
            default:
                throw new ViewModelNotFoundException();
        }
    }


    public override UserControl GetViewForRoute(NavigationRoutes routeName)
    {
        switch (routeName)
        {
            case NavigationRoutes.Home:
            case NavigationRoutes.FirstView:
                return new FirstView();
            case NavigationRoutes.SecondView:
                return new SecondView();
            default:
                throw new ViewNotFoundException();
        }
    }

    public override ViewModelBase GetViewModelForPopup(ComplexDialogRoute popup, object? message = null)
    {
        switch (popup)
        {
            case ComplexDialogRoute.Message:
                return new FirstViewModel(_navigationService);
            default:
                throw new ViewModelNotFoundException();
        }
    }


    public override UserControl GetViewForPopup(ComplexDialogRoute popup)
    {
        switch (popup)
        {
            case ComplexDialogRoute.Message:
                return new GetInputDialog();
            default:
                throw new ViewNotFoundException();
        }
    }
}

