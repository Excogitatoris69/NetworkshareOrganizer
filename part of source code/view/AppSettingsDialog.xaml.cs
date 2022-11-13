using NetworkShareOrganizer.src.viewmodel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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
    /// Interaction logic for AppSettingsDialog.xaml
    /// </summary>
    public partial class AppSettingsDialog : Window
    {
        public AppSettingsDialog()
        {
            InitializeComponent();
            AppSettingsViewmodel vm = getViewmodel();

            vm.onWindowClosingEvent += WindowClosingEventHandler;
            vm.PropertyChanged += OnViewmodelDataChangedEvent;

            //view ist fertig, jetzt model informieren
            Loaded += delegate { vm.OnLoaded(); };
        }

        public void WindowClosingEventHandler(object sender, WindowClosingEventArgs e)
        {
            DialogResult = e.dialogResult;
            Close();
        }

        private void OnViewmodelDataChangedEvent(object sender, PropertyChangedEventArgs args)
        {
            AppSettingsViewmodel vm = getViewmodel();
            if (args.PropertyName.Equals(nameof(vm.preSelectedLanguage)))
            {
                comboLanguage_setSelection(vm.preSelectedLanguage);
            }

        }


        private void comboLanguage_setSelection(LanguageListItemModel item)
        {
            comboLanguage.SelectedItem = item;
        }


        /// <summary>
        /// Liefert das Viewmodel.
        /// </summary>
        /// <returns></returns>
        private AppSettingsViewmodel getViewmodel()
        {
            return (AppSettingsViewmodel)DataContext;
        }

        private void comboLanguage_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            AppSettingsViewmodel vm = getViewmodel();
            vm.selectedLanguage = (LanguageListItemModel)comboLanguage.SelectedItem;
        }
    }
}
