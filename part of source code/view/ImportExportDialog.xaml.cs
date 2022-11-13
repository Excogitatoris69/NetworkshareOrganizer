using NetworkShareOrganizer.src.viewmodel;
using System;
using System.ComponentModel;
using System.Windows;

namespace NetworkShareOrganizer.src.view
{
    /// <summary>
    /// Interaction logic for ImportExportDialog.xaml
    /// </summary>
    public partial class ImportExportDialog : Window
    {
        public ImportExportDialog()
        {
            InitializeComponent();
            ImportExportDialogViewmodel vm = getViewmodel();
            vm.PropertyChanged += OnViewmodelDataChangedEvent;
            
            //Closing-Mechanismus
            //Die View kennt sein ViewModel, aber nicht umgekehrt.Ein Viewmodel kann die View nicht schließen, nur die View kann das.
            //Deshalb registrieren wir einen Eventhandler namens 'WindowClosingEventHandler' beim Viewmodel an 'onWindowClosingEvent',
            //um über den Wunsch einer Fensterschließung informiert zu werden.
            //Wir lassen dann durch die in der View definierten EventHandler die View schließen.
            vm.onWindowClosingEvent += WindowClosingEventHandler;
            //Ruft innerhalb des Viewmodel OnWindowClosing auf, wenn X-Close gedrückt wurde.
            //Dort wird und kann nur entschieden werden, ob das window geschlossen werden darf.
            Closing += vm.OnWindowClosing;

            Loaded += delegate { vm.OnLoaded(); };
        }

        private void OnViewmodelDataChangedEvent(object sender, PropertyChangedEventArgs args)
        {
            ImportExportDialogViewmodel vm = getViewmodel();
            if (args.PropertyName.Equals(nameof(vm.path)))
            {
                tbPath.Text = vm.path;
            }
        }

        private void WindowClosingEventHandler(object sender, WindowClosingEventArgs e)
        {
            Close();
        }

       

        /// <summary>
        /// Liefert das Viewmodel.
        /// </summary>
        /// <returns></returns>
        private ImportExportDialogViewmodel getViewmodel()
        {
            return (ImportExportDialogViewmodel)DataContext;
        }

        private void tbPath_LostFocus(object sender, RoutedEventArgs e)
        {
            ImportExportDialogViewmodel vm = getViewmodel();
            vm.path = tbPath.Text;
        }

        private void tbPath_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            //scroll to last position
            tbPath.CaretIndex = tbPath.Text.Length;
            var rect = tbPath.GetRectFromCharacterIndex(tbPath.CaretIndex);
            tbPath.ScrollToHorizontalOffset(rect.Right);
        }
    }
}
