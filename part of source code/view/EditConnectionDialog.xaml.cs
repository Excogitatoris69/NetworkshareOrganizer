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
    /// Interaction logic for EditConnectionDialog.xaml
    /// </summary>
    public partial class EditConnectionDialog : Window
    {
        
        public EditConnectionDialog()
        {
            InitializeComponent();
            EditConnectionDialogViewmodel vm = getViewmodel();

            ((EditConnectionDialogViewmodel)DataContext).onWindowClosingEvent += WindowClosingEventHandler;
            vm.PropertyChanged += OnViewmodelDataChangedEvent;
            comboCredential.Focus();
        }

        public void WindowClosingEventHandler(object sender, WindowClosingEventArgs e)
        {
            DialogResult = e.dialogResult;
            Close();
        }

        

        private void OnViewmodelDataChangedEvent(object sender, PropertyChangedEventArgs args)
        {
            EditConnectionDialogViewmodel vm = getViewmodel();
            if (args.PropertyName.Equals(nameof(vm.preSelectedCredentialListItem)))
            {
                comboCredential_setSelection(vm.preSelectedCredentialListItem);
            }
            if (args.PropertyName.Equals(nameof(vm.preSelectedDriveletter)))
            {
                comboDriveletter_setSelection(vm.preSelectedDriveletter);
            }
        }

        /// <summary>
        /// Liefert das Viewmodel.
        /// </summary>
        /// <returns></returns>
        private EditConnectionDialogViewmodel getViewmodel()
        {
            return (EditConnectionDialogViewmodel)DataContext;
        }

        private void comboCredential_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            EditConnectionDialogViewmodel vm = getViewmodel();
            vm.selectedCredentialListItem = (CredentialListItemModel)comboCredential.SelectedItem;
        }

        private void comboDrive_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            EditConnectionDialogViewmodel vm = getViewmodel();
            vm.selectedDriveletter = (DriveletterListItemModel)comboDrive.SelectedItem;
        }

        /// <summary>
        /// Selektiert in der Combobox das übergebene Element
        /// </summary>
        /// <param name="item"></param>
        private void comboDriveletter_setSelection(DriveletterListItemModel item)
        {
            comboDrive.SelectedItem = item;
        }

        /// <summary>
        /// Selektiert in der Combobox das übergebene Element
        /// </summary>
        /// <param name="item"></param>
        private void comboCredential_setSelection(CredentialListItemModel item)
        {
            comboCredential.SelectedItem = item;
        }


    }
}
