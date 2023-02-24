using System;
using System.Windows;
using System.Windows.Controls;


namespace Richy_WPF_MVVM.Common.Navigation
{

    public partial class SimpleDialog : Window
    {

        public event DialogResultAvailable? DialogResultEvent;

    
        public SimpleDialog(Window owner,
                            string text,
                            string? title,
                            CommonDialogIcon image,
                            CommonDialogButton messageBoxButton = CommonDialogButton.OK)
        {
            InitializeComponent();
            this.Owner = owner;
            DialogText.Content = text;
            this.Title = title;

            SetupButtons(messageBoxButton);
            SetupIcons(image);

            this.Closed += CommonDialog_Closed;

        }

        private void CommonDialog_Closed(object? sender, EventArgs e)
        {
            if (sender is Button) return;
            //otherwise need to raise an event if someone closes the dialog wihout using a button (i.e. Dialog Close button)
            DialogResultEvent?.Invoke(new Navigation.DialogResultEvent() { Result = CommonDialogResult.NoButtonPressed});
        }

        private void SetupIcons(CommonDialogIcon icon)
        {
            string content = string.Empty;

            
            if (icon == CommonDialogIcon.Warning) content = "\uE7BA";
            if (icon == CommonDialogIcon.Error) content = "\uE783";

            DialogIcon.Content = content;
        }

        private void SetupButtons(CommonDialogButton messageBoxButton)
        {
            switch(messageBoxButton)
            {
                case CommonDialogButton.OK:
                    Button1.Content = "OK";

                    Button2.Visibility = Visibility.Collapsed;
                    break;
                case CommonDialogButton.OkCancel:
                    _button1Result = MessageBoxResult.OK;
                    Button1.Content = "OK";

                    Button2.Visibility = Visibility.Visible;
                        Button2.Content = "Cancel";
                    _button2Result = MessageBoxResult.Cancel;
                    break;
            }
            
        }

        private MessageBoxResult _button1Result;
        private MessageBoxResult _button2Result;


        private void SelectionMade(object sender, RoutedEventArgs e)
        {
            DialogResultEvent resultEvent = new DialogResultEvent();

            Button clickedButton = (Button)sender;
            if (clickedButton == Button1) resultEvent.Result = CommonDialogResult.OK;
            if (clickedButton == Button2) resultEvent.Result = CommonDialogResult.Cancel;

            DialogResultEvent?.Invoke(resultEvent);
            this.Close();
        }
    }
}
