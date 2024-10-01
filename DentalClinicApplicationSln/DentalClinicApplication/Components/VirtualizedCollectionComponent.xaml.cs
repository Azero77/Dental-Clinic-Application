using DentalClinicApp.Models;
using DentalClinicApplication.Commands;
using DentalClinicApplication.ComponentsViewModels;
using DentalClinicApplication.VirtualizationCollections;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
        public ObservableCollection<DataGridColumn> Columns
        {
            get { return (ObservableCollection<DataGridColumn>)GetValue(ColumnsProperty); }
            set { SetValue(ColumnsProperty, value); }
        }

        // Using a DependencyProperty as the backing store for MyProperty.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ColumnsProperty =
            DependencyProperty.Register("MyProperty", typeof(ObservableCollection<DataGridColumn>), typeof(VirtualizedCollectionComponent), new PropertyMetadata(new ObservableCollection<DataGridColumn>()));

        public VirtualizedCollectionComponent()
        {
            InitializeComponent();
        }

        private void SetUpDataGridColumns()
        {
            CollectionDataGrid.Columns.Clear();
            foreach (DataGridColumn column in Columns)
            {
                try
                {

                    CollectionDataGrid.Columns.Add(column);
                }
                catch (Exception)
                {
                }
            }
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            SetUpDataGridColumns();
        }
    }

}
