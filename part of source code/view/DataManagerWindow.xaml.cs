using NetworkShareOrganizer.src.common.model;
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
    /// Interaction logic for DataManagerWindow.xaml
    /// </summary>
    public partial class DataManagerWindow : Window
    {
        public DataManagerWindow()
        {
            InitializeComponent();
            DataManagerWindowViewmodel vm = getViewmodel();

            //register event handler
            vm.editCredentialViewSettings.PropertyChanged += OnEditCredentialViewEvent;
            vm.editGroupViewSettings.PropertyChanged += OnEditGroupViewEvent;
            vm.editNetshareViewSettings.PropertyChanged += OnEditNetshareViewEvent;
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

            //view ist fertig, jetzt model informieren
            //vm.initialize();

            Loaded += delegate { vm.OnLoaded(); };
            Unloaded += delegate { vm.OnUnloaded(); };

            btnTogglePassword.IsChecked = true;
            passwordClearText.Width = 0;
            passwordBox.Width = passwordBoxWidth;
        }

        /// <summary>
        /// Liefert das Viewmodel.
        /// </summary>
        /// <returns></returns>
        private DataManagerWindowViewmodel getViewmodel()
        {
            return (DataManagerWindowViewmodel)DataContext;
        }

        public int passwordBoxWidth { 
            get { 
                return 200; 
            } 
        }

        /// <summary>
        /// Wir reagieren auf Events vom Viewmodel.
        /// Wenn dort ein Element einer Combobox als preselect definiert wird,
        /// muss dies hier auch gesetzt werden.
        /// </summary>
        private void OnViewmodelDataChangedEvent(object sender, PropertyChangedEventArgs args)
        {
            DataManagerWindowViewmodel vm = getViewmodel();
            if (args.PropertyName.Equals(nameof(vm.preSelectedCredentialDtoList)))
            {
                //Selektion in der Liste hat sich geändert.
                lvCredential_setSelections(vm.preSelectedCredentialDtoList);
            }
            if (args.PropertyName.Equals(nameof(vm.preSelectedGroupDtoList)))
            {
                lvGroup_setSelections(vm.preSelectedGroupDtoList);
            }
            if (args.PropertyName.Equals(nameof(vm.preSelectedNetshareDtoList)))
            {
                lvNetshare_setSelections(vm.preSelectedNetshareDtoList);
            }
            if (args.PropertyName.Equals(nameof(vm.preSelectedDriveletter)))
            {
                comboDriveletter_setSelection(vm.preSelectedDriveletter);
            }
            if (args.PropertyName.Equals(nameof(vm.preSelectedGroupComboItem)))
            {
                comboGroup_setSelection(vm.preSelectedGroupComboItem);
            }
            if (args.PropertyName.Equals(nameof(vm.preSelectedCredentialComboItem)))
            {
                comboCredential_setSelection(vm.preSelectedCredentialComboItem);
            }

            if (args.PropertyName.Equals(nameof(vm.selectedNetshareDtoList)))
            {
                lvNetshare_setSelections(vm.selectedNetshareDtoList);
            }

            //if (args.PropertyName.Equals(nameof(vm.selectedCredentialDtoList)))
            //{
            //    int x = lvCredentials.SelectedIndex;
            //}
        }

        /// <summary>
        /// Wir reagieren auf Events vom Viewmodel-Teil credentials
        /// </summary>
        private void OnEditCredentialViewEvent(object sender, PropertyChangedEventArgs args)
        {
            DataManagerWindowViewmodel vm = getViewmodel();
            if (args.PropertyName.Equals(nameof(vm.editCredentialViewSettings.editStatus)))
            {
                if (vm.editCredentialViewSettings.editStatus == EEditstatus.EditMode
                    || vm.editCredentialViewSettings.editStatus == EEditstatus.AddNewMode)
                {
                    tbCredentialTitle.Focus();
                    tbCredentialTitle.Select(0, tbCredentialTitle.Text.Length);
                }
                if (vm.editCredentialViewSettings.editStatus == EEditstatus.ReadOnly)
                {
                    lvCredentials.Focus();
                    //selektion des zuvor selektierten Elements
                    //zuerst auf -1 setzen, damit der SelectionChangedEventtrigger ausgelöst wird.
                    lvCredentials.SelectedIndex = -1;
                    lvCredential_setSelections(vm.preSelectedCredentialDtoList);
                }

            }
        }

        /// <summary>
        /// Wir reagieren auf Events vom Viewmodel-Teil groups
        /// </summary>
        private void OnEditGroupViewEvent(object sender, PropertyChangedEventArgs args)
        {
            DataManagerWindowViewmodel vm = getViewmodel();
            if (args.PropertyName.Equals(nameof(vm.editGroupViewSettings.editStatus)))
            {
                if (vm.editGroupViewSettings.editStatus == EEditstatus.EditMode
                    || vm.editGroupViewSettings.editStatus == EEditstatus.AddNewMode)
                {
                    tbGroupTitle.Focus();
                    tbGroupTitle.Select(0, tbGroupTitle.Text.Length);
                }
                if (vm.editGroupViewSettings.editStatus == EEditstatus.ReadOnly)
                {
                    lvGroups.Focus();
                    //selektion des zuvor selektierten Elements
                    //zuerst auf -1 setzen, damit der SelectionChangedEventtrigger ausgelöst wird.
                    lvGroups.SelectedIndex = -1;
                    lvGroup_setSelections(vm.preSelectedGroupDtoList);
                }

            }
        }

        /// <summary>
        /// Wir reagieren auf Events vom Viewmodel-Teil netshares.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        private void OnEditNetshareViewEvent(object sender, PropertyChangedEventArgs args)
        {
            DataManagerWindowViewmodel vm = getViewmodel();
            if (args.PropertyName.Equals(nameof(vm.editNetshareViewSettings.editStatus)))
            {
                if (vm.editNetshareViewSettings.editStatus == EEditstatus.EditMode
                    || vm.editNetshareViewSettings.editStatus == EEditstatus.AddNewMode)
                {
                    tbNetshareTitle.Focus();
                    tbNetshareTitle.Select(0, tbNetshareTitle.Text.Length);
                }
                if (vm.editNetshareViewSettings.editStatus == EEditstatus.ReadOnly)
                {
                    lvNetshare.Focus();
                    //selektion des zuvor selektierten Elements
                    //zuerst auf -1 setzen, damit der SelectionChangedEventtrigger ausgelöst wird.
                    lvNetshare.SelectedIndex = -1;
                    lvNetshare_setSelections(vm.preSelectedNetshareDtoList);

                    comboDriveletter.SelectedIndex = -1;
                    comboDriveletter_setSelection(vm.preSelectedDriveletter);

                    comboGroup.SelectedIndex = -1;
                    comboGroup_setSelection(vm.preSelectedGroupComboItem);

                    comboCredential.SelectedIndex = -1;
                    comboCredential_setSelection(vm.preSelectedCredentialComboItem);
                }
            }
            else if (args.PropertyName.Equals(nameof(vm.preSelectedNetshareDtoList)))
            {
                lvNetshare.SelectedIndex = -1;
                lvNetshare_setSelections(vm.preSelectedNetshareDtoList);
                int x = 0;
            }

        }

        /// <summary>
        /// Selektiert in der listview alle Items, die in der Suchliste übergeben werden.
        /// </summary>
        /// <param name="searchList"></param>
        private void lvCredential_setSelections(List<CredentialListItemModel> searchList)
        {
            //Iteration über alle items und selektieren, wenn in der suchliste.
            //Somit verhindern wir Selektionen, die es nicht mehr gibt.
            foreach (CredentialListItemModel listItem in lvCredentials.Items)
            {
                foreach (CredentialListItemModel searchItem in searchList)
                {
                    if (searchItem.credentialDto.Id.Equals(listItem.credentialDto.Id))
                    {
                        lvCredentials.SelectedItems.Add(listItem);
                    }
                }
            }
        }

        /// <summary>
        /// Selektiert in der listview alle Items, die in der Suchliste übergeben werden.
        /// </summary>
        /// <param name="searchList"></param>
        private void lvGroup_setSelections(List<GroupListItemModel> searchList)
        {
            //Iteration über alle items und selektieren, wenn in der suchliste.
            //Somit verhindern wir Selektionen, die es nicht mehr gibt.
            foreach (GroupListItemModel listItem in lvGroups.Items)
            {
                foreach (GroupListItemModel searchItem in searchList)
                {
                    if (searchItem.groupDto.Id.Equals(listItem.groupDto.Id))
                    {
                        lvGroups.SelectedItems.Add(listItem);
                    }
                }
            }
        }

        /// <summary>
        /// Selektiert in der listview alle Items, die in der Suchliste übergeben werden.
        /// </summary>
        /// <param name="searchList"></param>
        private void lvNetshare_setSelections(List<NetshareListItemModel> searchList)
        {
            //Iteration über alle items und selektieren, wenn in der suchliste.
            //Somit verhindern wir Selektionen, die es nicht mehr gibt.
            foreach (NetshareListItemModel listItem in lvNetshare.Items)
            {
                foreach (NetshareListItemModel searchItem in searchList)
                {
                    if (searchItem.netshareDto.Id.Equals(listItem.netshareDto.Id))
                    {
                        lvNetshare.SelectedItems.Add(listItem);
                    }
                }
            }
        }

        /// <summary>
        /// Selektiert in der Combobox das übergebene Element
        /// </summary>
        /// <param name="item"></param>
        private void comboDriveletter_setSelection(DriveletterListItemModel item)
        {
            comboDriveletter.SelectedItem = item;
        }

        /// <summary>
        /// Selektiert in der Combobox das übergebene Element
        /// </summary>
        /// <param name="item"></param>
        private void comboGroup_setSelection(GroupListItemModel item)
        {
            comboGroup.SelectedItem = item;
        }

        /// <summary>
        /// Selektiert in der Combobox das übergebene Element
        /// </summary>
        /// <param name="item"></param>
        private void comboCredential_setSelection(CredentialListItemModel item)
        {
            comboCredential.SelectedItem = item;
        }
        /// <summary>
        /// Wird aufgerufen aus Xaml bei Änderungen der Selektion in einer Listview.
        /// Es werden alle selektierten Items im ViewModel gespeichert.
        /// </summary>
        private void lvCredentials_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            DataManagerWindowViewmodel vm = getViewmodel();
            vm.selectedCredentialDtoList.Clear();
            foreach (CredentialListItemModel item in lvCredentials.SelectedItems)
            {
                vm.selectedCredentialDtoList.Add(item);

            }
            vm.EventHandler_lvCredentials_SelectionChanged();//Viewmodel benachrichtigen
            vm.selectedCredentialIndex = lvCredentials.SelectedIndex;
            passwordBox.Password = "xxxxxxxxx";
            btnTogglePassword.IsChecked = true;
            ToggleButton_Click(sender,e);
            
        }

        /// <summary>
        /// Wird aufgerufen aus Xaml bei Änderungen der Selektion in einer Listview.
        /// Es werden alle selektierten Items im ViewModel gespeichert.
        /// </summary>
        private void lvGroups_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            DataManagerWindowViewmodel vm = getViewmodel();
            vm.selectedGroupDtoList.Clear();
            foreach (GroupListItemModel item in lvGroups.SelectedItems)
            {
                vm.selectedGroupDtoList.Add(item);

            }
            vm.EventHandler_lvGroups_SelectionChanged();//Viewmodel benachrichtigen
            vm.selectedGroupIndex = lvGroups.SelectedIndex;
        }

        /// <summary>
        /// Wird aufgerufen aus Xaml bei Änderungen der Selektion in einer Listview.
        /// Es werden alle selektierten Items im ViewModel gespeichert.
        /// </summary>
        private void lvNetshare_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            DataManagerWindowViewmodel vm = getViewmodel();
            vm.selectedNetshareDtoList.Clear();
            foreach (NetshareListItemModel item in lvNetshare.SelectedItems)
            {
                vm.selectedNetshareDtoList.Add(item);

            }
            vm.EventHandler_lvNetshare_SelectionChanged();//Viewmodel benachrichtigen
            vm.selectedNetshareIndex = lvNetshare.SelectedIndex;
        }

        /// <summary>
        /// Wird aufgerufen aus Xaml bei Änderungen der Selektion in einer Listview.
        /// Es wird das selektierten Item im ViewModel gespeichert.
        /// </summary>
        private void comboDriveletter_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            DataManagerWindowViewmodel vm = getViewmodel();
            vm.selectedDriveletter = (DriveletterListItemModel)comboDriveletter.SelectedItem;
        }

        private void comboCredential_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            DataManagerWindowViewmodel vm = getViewmodel();
            vm.selectedCredentialComboItem = (CredentialListItemModel)comboCredential.SelectedItem;
        }

        private void comboGroup_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            DataManagerWindowViewmodel vm = getViewmodel();
            vm.selectedGroupComboItem = (GroupListItemModel)comboGroup.SelectedItem;
        }

        /// <summary>
        /// Wird ausgeführt, wenn das Viewmodel die View schließen will.
        /// Is called if ViewModel send Event to close view.
        /// </summary>
        public void WindowClosingEventHandler(object sender, WindowClosingEventArgs e)
        {
            Close();
        }

        private void ToggleButton_Click(object sender, RoutedEventArgs e)
        {
            DataManagerWindowViewmodel vm = getViewmodel();
            if ((bool)btnTogglePassword.IsChecked)//maskiert/verdeckt
            {
                passwordClearText.Text = "";
                passwordBox.Width = passwordBoxWidth;
                passwordClearText.Width = 0;
                vm.editCredentialViewSettings.setTogglePwModeMasked();
            }
            else//klartext wir angezeigt
            {
                    //p => string.IsNullOrEmpty(p)? passwordClearText.Text=passwordBox.Password:passwordClearText.Text = p 
                vm.getClearPassword( p => setPassword(p));
                passwordBox.Width = 0;
                passwordClearText.Width = passwordBoxWidth;
                vm.editCredentialViewSettings.setTogglePwModeClear();
            }
        }

        public void setPassword(string password)
        {
            //passwordClearText.Text = password;
            if (string.IsNullOrEmpty(password))
                passwordClearText.Text = passwordBox.Password;
            else
                passwordClearText.Text = password;
        }

        private void passwordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            DataManagerWindowViewmodel vm = getViewmodel();
            vm.editCredentialViewSettings.secPasswordDto = new PasswordDto(passwordBox.SecurePassword);
        }

      

        private void passwordBox_GotFocus(object sender, RoutedEventArgs e)
        {
            passwordBox.Password = "";
        }

        private void tbGroupTitle_LostFocus(object sender, RoutedEventArgs e)
        {
            DataManagerWindowViewmodel vm = getViewmodel();
            vm.editGroupViewSettings.dataGroupname = tbGroupTitle.Text;
        }

        private void tbGroupTitle_TextChanged(object sender, TextChangedEventArgs e)
        {
            DataManagerWindowViewmodel vm = getViewmodel();
            vm.editGroupViewSettings.dataGroupname = tbGroupTitle.Text;
        }

        private void tbNetshareTitle_TextChanged(object sender, TextChangedEventArgs e)
        {
            DataManagerWindowViewmodel vm = getViewmodel();
            vm.editNetshareViewSettings.dataTitle = tbNetshareTitle.Text;
        }

        private void tbNetshareTitle_LostFocus(object sender, RoutedEventArgs e)
        {
            DataManagerWindowViewmodel vm = getViewmodel();
            vm.editNetshareViewSettings.dataTitle = tbNetshareTitle.Text;
        }

        private void tbCredentialTitle_LostFocus(object sender, RoutedEventArgs e)
        {
            DataManagerWindowViewmodel vm = getViewmodel();
            vm.editCredentialViewSettings.dataTitle = tbCredentialTitle.Text;
        }

        private void tbCredentialTitle_TextChanged(object sender, TextChangedEventArgs e)
        {
            DataManagerWindowViewmodel vm = getViewmodel();
            vm.editCredentialViewSettings.dataTitle = tbCredentialTitle.Text;
        }
    }
}
