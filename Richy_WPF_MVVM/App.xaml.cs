using System.Windows;
using Richy_WPF_MVVM.Common.Navigation;
using Richy_WPF_MVVM.User;

namespace Richy_WPF_MVVM
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            //The NavigationService is a Mediator. It will tie all the other elements together. 
            //To avoid dependency problems (because where possible it is nice to use constructor injection to create items so that the item is created immediately along with all its
            //dependencies) then this is made first and is dependent on nothing
            INavigationService navigation = new NavigationService();

            //The Factory is this example of an IoC container. It will produce items for the Navigation Service when this is given a route. The comment for the NavigationService says that it is nice if an object
            //is constructed with all its requirements via constructor injection (as opposed to creating the object and then using property injection or method injection to add things).
            //The Factory is given a reference to the NavigationService so that it can be injected into the ViewModel constructors as they will use this object for navigation
            AppFactory factory = new AppFactory(navigation);

            /*This is the graphical parent of this application. Originally thought it may be worth having a MainView of type Usercontrol which would be the logical parent of the application.
            In this latter case there would be the Window would be still present and MainView would be added to it. This may change but currently MainWindow only has a ContentControl in it and 
            the content of this will be set by the NavigationService.
             */
            MainWindow window = new MainWindow();

            /*
             * The NavigationService will need to know about the MainWindow and so it is registered here.
             */
            navigation.RegisterMainWindow(window);

            /*
             * As the NavigationService will need to navigate to new ViewModels by creating the View (and setting this in MainWindow.MainView) and it's associated ViewModel then it will
             * need to know about the Factory and this is done here.
             */
            navigation.RegisterFactory(factory);

            /*
             * Set the start up page - is it better if this is an enum? At least it is a type safe way of identifying a route?
             */
            navigation.NavigateTo( NavigationRoutes.Home );

            //Let's rock!
            window.Show();
        }

    }
}
