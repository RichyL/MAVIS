using Richy_WPF_MVVM.Common.Creation;

namespace Richy_WPF_MVVM.Common.Navigation
{
    /// <summary>
    /// This enumeration would need to be modified on a by-application basis to specify the Views to be shown to the user.
    /// </summary>
    public enum NavigationRoutes { Home, FirstView, SecondView };

    /// <summary>
    /// This enumeration would need to be modified on a by-application basis to specify the modal popups to be shown to the user.
    /// </summary
    public enum ComplexDialogRoute { Message }

    /// <summary>
    /// Classes implementing this interface are responsible for the navigation within the app. Navigation in this case being the View shown to the user in the main
    /// application window.
    /// 
    /// Implementations of this class should also handle the display of modal dialogues to the user. 
    /// 
    /// All navigation is specified by entries added to the NavigationRoutes enum which provides some type checking cover on navigation routes. Popups are shown depending 
    /// on values in the Popups enum.
    /// </summary>
    public interface INavigationService
    {
        /// <summary>
        /// Causes the navigation service to change the currently displayed View to match that matching the NavigationRoute specified by route
        /// </summary>
        /// <param name="route">Specifies the View to be shown to the user</param>
        /// <param name="message">Used to allow the current ViewModel to pass data to the new ViewModel used on navigating</param>
        /// <exception cref="ViewModelNotFoundException">Thrown when navigation occurs and the navigation service cannot get an instance of the ViewModel from the AbstractFactoryBase</exception>
        /// <exception cref="ViewNotFoundException">Thrown when navigation occurs and the navigation service cannot get an instance of the View from the AbstractFactoryBase</exception>
        void NavigateTo(NavigationRoutes route, object? message = null);

        /// <summary>
        /// The navigation service will use an instance of AbstractFactoryBase to get the Views and ViewModels to be shown to the user. 
        /// </summary>
        /// <param name="factory">Implementation of AbstractFactoryBase which will provide new objects to be shown to the user</param>
        void RegisterFactory(AbstractFactoryBase factory);

        /// <summary>
        /// The navigation service requires a reference to the main application window in order to show required View to the user
        /// </summary>
        /// <param name="window">Reference to the main application window</param>
        /// <exception cref="NavigationServiceException">Thrown if no reference to the main application window exists</exception>
        void RegisterMainWindow(MainWindow window);
        //void ShowDialog(string message, string title = "", MessageBoxButton messageBoxButtons = MessageBoxButton.OK, MessageBoxImage messageBoxImage = MessageBoxImage.None, MessageBoxResult messageBoxResult = MessageBoxResult.None, DialogResultAvailable? callback = null);


        /// <summary>
        /// Shows a predefined modal box in order to pose simple questions to the user.
        /// </summary>
        /// <param name="message">Message to show in the dialogue box</param>
        /// <param name="title">Title of the dialog box (be default this is blank)</param>
        /// <param name="messageBoxImage">Specifies if a warning, error or no icon is shown on the dialogue</param>
        /// <param name="messageBoxButtons">Specifies which selection of buttons is shown from the choices OK or OK/Cancel. Default is OK.</param>
        /// <param name="callback">Code to be called when the Dialog is closed</param>
        void ShowDialog(string message,
                               string title = "",
                               CommonDialogIcon messageBoxImage = CommonDialogIcon.None,
                               CommonDialogButton messageBoxButtons = CommonDialogButton.OK,
                               DialogResultAvailable? callback = null);

        /// <summary>
        /// Shows a Window inside which is View specified by the user using the ComplexDialogRoute. The intent of this class is for a ViewModel to use itself
        /// for the specified View and in doing so can easily get information back from the ComplexDialog. 
        /// </summary>
        /// <param name="dialogRoute">Specifies the View to be shown within the Complex Dialog</param>
        /// <param name="viewModel">The ViewModel to be used for this View.</param>
        void ShowComplexDialog(ComplexDialogRoute dialogRoute, ViewModelBase viewModel);

        /// <summary>
        /// Will close, if set, the current ComplexDialog being managed by the Navigation Service.
        /// </summary>
        void CloseComplexDialog();
    }
}