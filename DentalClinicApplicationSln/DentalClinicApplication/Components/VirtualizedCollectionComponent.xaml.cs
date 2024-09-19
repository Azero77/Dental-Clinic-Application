using DentalClinicApp.Models;
using DentalClinicApplication.Commands;
using DentalClinicApplication.ComponentsViewModels;
using DentalClinicApplication.VirtualizationCollections;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
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
    public partial class VirtualizedCollectionComponent : UserControl
    {
        public VirtualizedCollectionComponent()
        {
            InitializeComponent();
        }
    }

    /// <summary>
    /// Interaction logic for VirtualizedCollectionComponent.xaml
    /// </summary>
    public partial class VirtualizedCollectionComponent<T> : VirtualizedCollectionComponent
    {
        public VirtualizedCollectionComponent() : base()
        {
            this.DataContext = new VirtualizedCollectionComponentViewModel<T>();
        }



        public IEnumerable Collection
        {
            get { return (IEnumerable)GetValue(CollectionProperty); }
            set { SetValue(CollectionProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Collection.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CollectionProperty =
            DependencyProperty.Register("Collection", typeof(IEnumerable), typeof(VirtualizedCollectionComponent<T>), new PropertyMetadata(Enumerable.Empty<T>(),OnCollectionChanged));

        private static void OnCollectionChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            VirtualizedCollectionComponent<T> userControlComponent = (VirtualizedCollectionComponent<T>)d;
            userControlComponent.DataContext =
                VirtualizedCollectionComponentViewModel<T>
                .GetVirtualizedCollectionComponentViewModel((IEnumerable) e.NewValue);
        }
    }

    public partial class ClientVirtualizedCollectionComponent : VirtualizedCollectionComponent<Client>
    {
        public ClientVirtualizedCollectionComponent()
        {
        }
    }
}
