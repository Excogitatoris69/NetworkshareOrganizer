using NetworkShareOrganizer.src.common.model;
using NetworkShareOrganizer.src.viewmodel;
using System;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace NetworkShareOrganizer.src.view
{


    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private MainWindowViewmodel myViewmodel = null;
        private bool isGrouping = false;
        private ICollectionView cvTableNetshare = null;
        public MainWindow()
        {
            InitializeComponent();
            MainWindowViewmodel vm = getViewmodel();

            //register eventhandler
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
            Loaded += delegate { vm.OnLoaded(); };
            cvTableNetshare = CollectionViewSource.GetDefaultView(TableNetshare.ItemsSource);
            
            vm.initialize();


        }

        

        /// <summary>
        /// Liefert das Viewmodel.
        /// </summary>
        private MainWindowViewmodel getViewmodel()
        {
            myViewmodel = (MainWindowViewmodel)DataContext;
            return myViewmodel;
        }

        private void OnViewmodelDataChangedEvent(object sender, PropertyChangedEventArgs args)
        {
            MainWindowViewmodel vm = getViewmodel();
            if (args.PropertyName.Equals(nameof(vm.preSelectedGroupingCatergoryItem)))
            {
                comboGroupingCategory_setSelection(
                   myViewmodel.preSelectedGroupingCatergoryItem
                );
            }
        }
       


        /// <summary>
        /// Setzt einen Wert in der Dropdownliste
        /// </summary>
        /// <param name="item"></param>
        private void comboGroupingCategory_setSelection(GroupingListItemModel item)
        {
            comboGroupingCategory.SelectedItem = item;
            refresh_groupingCollectionView();
        }

        //private void toggleGrouping_Click(object sender, RoutedEventArgs e)
        //{
        //    if (isGrouping)
        //    {
        //        isGrouping = false;
        //        ICollectionView cvTableNetshare = CollectionViewSource.GetDefaultView(TableNetshare.ItemsSource);
        //        if (cvTableNetshare != null)
        //        {
        //            cvTableNetshare.GroupDescriptions.Clear();
        //        }
        //    }
        //    else
        //    {
        //        isGrouping = true;
        //        ICollectionView cvTableNetshare = CollectionViewSource.GetDefaultView(TableNetshare.ItemsSource);
        //        if (cvTableNetshare != null && cvTableNetshare.CanGroup)
        //        {
        //            cvTableNetshare.GroupDescriptions.Clear();
        //            cvTableNetshare.GroupDescriptions.Add(new PropertyGroupDescription(nameof(NetshareTableRowModel.GroupName)));
        //            //cvTableNetshare.GroupDescriptions.Add(new PropertyGroupDescription(nameof(NetshareTableRowModel.ConnectionStatus)));
                    
        //        }
        //    }
        //}

        /// <summary>
        /// Wird ausgeführt, wenn das Viewmodel die View schließen will.
        /// Is called if ViewModel send Event to close view.
        /// </summary>
        public void WindowClosingEventHandler(object sender, WindowClosingEventArgs e)
        {
            Close();
        }

        private void TableNetshare_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            MainWindowViewmodel vm = getViewmodel();
            vm.selectedNetshareTableRowList.Clear();
            foreach (NetshareTableRowModel item in TableNetshare.SelectedItems)
            {
                vm.selectedNetshareTableRowList.Add(item);
            }
            //vm.EventHandler_TableNetshare_SelectionChanged();//Viewmodel benachrichtigen
            vm.selectedNetshareTableRowIndex = TableNetshare.SelectedIndex;
        }

        private void refresh_groupingCollectionView()
        {
            //ICollectionView cvTableNetshare = CollectionViewSource.GetDefaultView(TableNetshare.ItemsSource);
            GroupingListItemModel cat = (GroupingListItemModel)comboGroupingCategory.SelectedItem;
            
            if (cvTableNetshare != null) 
            {
                //clean
                cvTableNetshare.GroupDescriptions.Clear();
                cvTableNetshare.SortDescriptions.Add(new SortDescription("Title", ListSortDirection.Ascending));
                if (cat.groupingCategory == EGroupingCategory.GroupName)
                {
                    cvTableNetshare.GroupDescriptions.Add(new PropertyGroupDescription(nameof(NetshareTableRowModel.GroupName)));
                }
                else if (cat.groupingCategory == EGroupingCategory.ConnectionStatus)
                {
                    cvTableNetshare.GroupDescriptions.Add(new PropertyGroupDescription(nameof(NetshareTableRowModel.ConnectionStatusText)));
                }
            }

        }

        private void comboGroupingCategory_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (TableNetshare != null)
            {
                GroupingListItemModel cat = (GroupingListItemModel)comboGroupingCategory.SelectedItem;
                Properties.Settings1.Default.S203_SelectedGrouping = cat.groupingCategory.ToString();
                //MainWindowViewmodel vm = getViewmodel();
                //vm.selectedGroupingCatergoryItem = cat;
                refresh_groupingCollectionView();
                //if (cat.groupingCategory == EGroupingCategory.None)
                //{
                //    ICollectionView cvTableNetshare = CollectionViewSource.GetDefaultView(TableNetshare.ItemsSource);
                //    if (cvTableNetshare != null)
                //    {
                //        cvTableNetshare.GroupDescriptions.Clear();
                //    }
                //}
                //else if (cat.groupingCategory == EGroupingCategory.GroupName)
                //{
                //    ICollectionView cvTableNetshare = CollectionViewSource.GetDefaultView(TableNetshare.ItemsSource);
                //    if (cvTableNetshare != null && cvTableNetshare.CanGroup)
                //    {
                //        cvTableNetshare.GroupDescriptions.Clear();
                //        cvTableNetshare.GroupDescriptions.Add(new PropertyGroupDescription(nameof(NetshareTableRowModel.GroupName)));

                //    }
                //}
                //else if (cat.groupingCategory == EGroupingCategory.ConnectionStatus)
                //{
                //    ICollectionView cvTableNetshare = CollectionViewSource.GetDefaultView(TableNetshare.ItemsSource);
                //    if (cvTableNetshare != null && cvTableNetshare.CanGroup)
                //    {
                //        cvTableNetshare.GroupDescriptions.Clear();
                //        cvTableNetshare.GroupDescriptions.Add(new PropertyGroupDescription(nameof(NetshareTableRowModel.ConnectionStatus)));

                //    }
                //}
            }
        }
    }
}
