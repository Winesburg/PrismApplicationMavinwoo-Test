﻿using System;
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

namespace Module.Views
{
    /// <summary>
    /// Interaction logic for Data.xaml
    /// </summary>
    public partial class Data : UserControl
    {
        public Data()
        {
            InitializeComponent();
    }

        
        private void SfDatePicker_To_ValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {

        }

        private void ComboBoxItem_Selected(object sender, RoutedEventArgs e)
        {

        }
    }
}
