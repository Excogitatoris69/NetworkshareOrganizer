using NetworkShareOrganizer.src.viewmodel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace NetworkShareOrganizer.src.view
{
    /// <summary>
    /// Interaction logic for ChangeMasterPasswordDialog.xaml
    /// </summary>
    public partial class ChangeMasterPasswordDialog : Window
    {
        public ChangeMasterPasswordDialog()
        {
            InitializeComponent();

            //Closing-Mechanismus
            //Die View kennt sein ViewModel, aber nicht umgekehrt.Ein Viewmodel kann die View nicht schließen, nur die View kann das.
            //Deshalb registrieren wir einen Eventhandler namens 'WindowClosingEventHandler' beim Viewmodel an 'onWindowClosingEvent',
            //um über den Wunsch einer Fensterschließung informiert zu werden.
            //Wir lassen dann durch die in der View definierten EventHandler die View schließen.
            ((ChangeMasterPasswordDialogViewmodel)DataContext).onWindowClosingEvent += WindowClosingEventHandler;
            //Ruft innerhalb des Viewmodel OnWindowClosing auf, wenn X-Close gedrückt wurde.
            //Dort wird und kann nur entschieden werden, ob das window geschlossen werden darf.
            Closing += ((ChangeMasterPasswordDialogViewmodel)DataContext).OnWindowClosing;
            //tbPasswordOld.PasswordChanged += delegate { ((ChangeMasterPasswordDialogViewmodel)DataContext).onPasswordChanged(); };
            //tbPasswordNew.PasswordChanged += delegate { ((ChangeMasterPasswordDialogViewmodel)DataContext).onPasswordChanged(); };
        }

        /// <summary>
        /// Wird ausgeführt, wenn das Viewmodel die View schließen will.
        /// Is called if ViewModel send Event to close view.
        /// </summary>
        public void WindowClosingEventHandler(object sender, WindowClosingEventArgs e)
        {
            Close();
        }

        private void tbPasswordNew_PasswordChanged(object sender, RoutedEventArgs e)
        {
            ((ChangeMasterPasswordDialogViewmodel)DataContext).secPasswordNew = tbPasswordNew.SecurePassword;
        }

        private void tbPasswordOld_PasswordChanged(object sender, RoutedEventArgs e)
        {
            ((ChangeMasterPasswordDialogViewmodel)DataContext).secPasswordOld = tbPasswordOld.SecurePassword;
        }
    }
}
