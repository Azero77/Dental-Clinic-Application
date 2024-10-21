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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace DentalClinicApplication.Components
{
    /// <summary>
    /// Interaction logic for DeleteValidationModalView.xaml
    /// </summary>
    public partial class DeleteValidationModalView : UserControl
    {
        public DeleteValidationModalView()
        {
            InitializeComponent();
            Container.Width = App.Current.MainWindow.Width / 2;
            Container.Height = App.Current.MainWindow.Height / 2;
            DockPanelContainer.Height = Container.Height / 4;
        }
    }
}
