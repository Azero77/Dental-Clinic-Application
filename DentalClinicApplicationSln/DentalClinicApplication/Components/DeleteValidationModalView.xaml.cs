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
using Xceed.Wpf.Toolkit.PropertyGrid;

namespace DentalClinicApplication.Components
{
    /// <summary>
    /// Interaction logic for DeleteValidationModalView.xaml
    /// </summary>
    public partial class DeleteValidationModalView : UserControl
    {


        public TextBlock Message
        {
            get { return (TextBlock)GetValue(MessageProperty); }
            set { SetValue(MessageProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Message.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty MessageProperty =
            DependencyProperty.Register("Message", typeof(TextBlock), typeof(DeleteValidationModalView), new PropertyMetadata());


        public object BindedObject
        {
            get { return (object)GetValue(BindedObjectProperty); }
            set { SetValue(BindedObjectProperty, value); }
        }

        // Using a DependencyProperty as the backing store for BindedObject.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty BindedObjectProperty =
            DependencyProperty.Register("BindedObject", typeof(object), typeof(DeleteValidationModalView), new PropertyMetadata(new object()));


        public DeleteValidationModalView()
        {
            InitializeComponent();
            Container.Width = App.Current.MainWindow.Width / 2;
            Container.Height = App.Current.MainWindow.Height / 2;
            DockPanelContainer.Height = Container.Height / 4;
        }
    }
}
