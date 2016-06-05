using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Data;
using System.Windows.Markup;
using System.Xml.Serialization;
using System.Xml;
using System.IO;


namespace Modelowanie
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        Database db;
        bool nowy = false, modified = false;

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            cDrawing.ClearAll();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            foreach (FontFamily f in System.Windows.Media.Fonts.SystemFontFamilies)
            {
                cbFonts.Items.Add(f);
            }
            if (cbFonts.Items.Count > 0)
                cbFonts.SelectedIndex = 0;

            for (int i = 8; i <= 40; i++)
            {
                cbfSize.Items.Add(i);
            }
            cbfSize.SelectedIndex = 4;

          
        }


        private void ehCBFontsChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                UDrawing.fontFamily = new FontFamily(e.AddedItems[0].ToString());
                modified = true;
                System.Console.WriteLine("Hello world!");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        private void ehcbfSizeChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                UDrawing.fontsize = (int)e.AddedItems[0];
                modified = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }


        private void Button_Click(object sender, RoutedEventArgs e)
        {
            AddUniterm au = new AddUniterm();

            au.ShowDialog();

            if (au.tbA.Text.Length > 250 || au.tbB.Text.Length > 250)
            {
                MessageBox.Show("Zbyt długi tekst!\n Maksymalna długość tekstu to 250 znaków!", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            UDrawing.sA = au.tbA.Text;
            UDrawing.sB = au.tbB.Text;

            UDrawing.sOp = au.rbSr.IsChecked == true ? " ; " : " , ";

            //btnRedraw_Click(sender, e);

            modified = true;

        }


    }
}
