using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
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
    /// Interaction logic for AboutDialog.xaml
    /// </summary>
    public partial class AboutDialog : Window
    {

        public string versionText
        {
            get
            {
                return Assembly.GetExecutingAssembly().GetName().Version.ToString();
            }
        }
        public string authorText
        {
            get
            {
                return "Oliver Matle"; // so geht das nicht: Assembly.GetExecutingAssembly()
            }
        }

        public string descriptionText
        {
            get {
                return Properties.Resource.M321_ABOUTDESCRIPTION;
            }
        }
        public string labelAuthor
        {
            get
            {
                return Properties.Resource.L147_AUTHOR;
            }
        }
        public string labelVersion
        {
            get
            {
                return Properties.Resource.L148_VERSION;
            }
        }

        public string dialogTitle
        {
            get
            {
                return Properties.Resource.L215_WINDOWTITLEABOUT;
            }
        }


        public AboutDialog()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
