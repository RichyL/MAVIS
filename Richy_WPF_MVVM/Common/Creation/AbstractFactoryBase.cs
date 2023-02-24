using Richy_WPF_MVVM.Common.Navigation;
using System.Windows.Controls;


namespace Richy_WPF_MVVM.Common.Creation
{
    /// <summary>
    /// Implementing classes are reponsible for producing the objects used in the application. The INavigationService implemention will use this class to get the Views and ViewModels
    /// which are used in the Views shown to the user. 
    /// 
    /// The life-time of the View and ViewModels is the responsability of the AbstractFactoryBase implementation
    /// </summary>
    public abstract class AbstractFactoryBase
    {
        /// <summary>
        /// Reference to the INavigationService
        /// </summary>
        protected INavigationService _navigationService;

        public AbstractFactoryBase(INavigationService navigationService)
        {
            _navigationService= navigationService;
        }

        /// <summary>
        /// Return an appropriate View for the route specified 
        /// </summary>
        /// <param name="routeName">Specifies the route for which a View is required</param>
        /// <returns>UserControl to be shown within the main application window</returns>
        public abstract UserControl GetViewForRoute(NavigationRoutes route);

        /// <summary>
        /// Return an appropriate ViewModel for the route specified
        /// </summary>
        /// <param name="route">Specifies the route for which a ViewModel is required</param>
        /// <param name="message">An optional object which can be passed to the new ViewModel</param>
        /// <returns>ViewModelBase to be used for the view shown in the main application window</returns>
        public abstract ViewModelBase GetViewModelForRoute(NavigationRoutes route, object? message=null);

        /// <summary>
        /// Return an appropriate View for the popup specified 
        /// </summary>
        /// <param name="routeName">Specifies the popup for which a View is required</param>
        /// <returns>UserControl to be shown within a modal window above the main application window</returns>
        public abstract UserControl GetViewForPopup(ComplexDialogRoute popup);

        /// <summary>
        /// Return an appropriate ViewModel for the popup specified
        /// </summary>
        /// <param name="route">Specifies the popup for which a ViewModel is required</param>
        /// <param name="message">An optional object which can be passed to the new ViewModel</param>
        /// <returns>ViewModelBase to be used withn a modal window above the main application window</returns>
        public abstract ViewModelBase GetViewModelForPopup(ComplexDialogRoute popup, object? message = null);
    }
}