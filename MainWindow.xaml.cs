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
        bool nowy = true, modified = false;

        /*clear*/
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
            db = new Database();

        }

        /*zmina czczionki*/
        private void ehCBFontsChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                UDrawing.fontFamily = new FontFamily(e.AddedItems[0].ToString());
                
               
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }
        /*zmiana rozmiaru*/
        private void ehcbfSizeChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                UDrawing.fontsize = (int)e.AddedItems[0];
                
               
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        /*dodanie sekwencio*/
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
            Console.WriteLine(UDrawing.sA.Length);
            UDrawing.sOp = au.rbSr.IsChecked == true ? " ; " : " , ";

            btnRedraw_Click(sender, e);

            modified = true;
            
        }


        private void btnAddEl_Click(object sender, RoutedEventArgs e)
        {
            AddElem ae = new AddElem();

            ae.ShowDialog();

            if (ae.tbA.Text.Length > 250 || ae.tbB.Text.Length > 250 || ae.tbC.Text.Length > 250)
            {
                MessageBox.Show("Zbyt długi tekst!\n Maksymalna długość tekstu to 250 znaków!", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            UDrawing.eA = ae.tbA.Text;
            UDrawing.eB = ae.tbB.Text;
            UDrawing.eC = ae.tbC.Text;
            
            btnRedraw_Click(sender, e);
            modified = true;
        }


        private void btnRedraw_Click(object sender, RoutedEventArgs e)
        {
            cDrawing.ClearAll();

            DrawingVisual dv = new DrawingVisual();
            using (DrawingContext dc = dv.RenderOpen())
            {
                UDrawing md = new UDrawing(dc);

                md.Redraw();
                dc.Close();
            }
            cDrawing.AddElement(dv);

        }

        private void btnSpare_Click(object sender, RoutedEventArgs e)
        {

            char operacja = 'X';
            switch (MessageBox.Show("Co zamienić?\n [Tak]==A, [Nie]==B", "Zamień", MessageBoxButton.YesNoCancel, MessageBoxImage.Question))
            {
                case MessageBoxResult.Yes:
                    operacja = 'A';
                    break;
                case MessageBoxResult.No:
                    operacja = 'B';
                    break;
                case MessageBoxResult.Cancel: return;
            }

            cDrawing.ClearAll();
            UDrawing.oper = operacja;
            btnRedraw_Click(sender, e);
            modified = true;
        }

        private void ehNowyClick(object sender, RoutedEventArgs e)
        {
            UDrawing.ClearAll();
            cDrawing.ClearAll();
            nowy = true;
            modified = false;
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            Save save = new Save();
            save.ShowDialog();
           
            Console.WriteLine(save.a);
            if (save.a)
            {
                try
                {

                    string sql = "insert into uniterms values('{0}','{1}','{2}','{3}','{4}','{5}','{6}',''{7}','{8}',{9},'{10}','{11}');";
                    if (nowy)
                    {
                        sql = "insert into uniterms values('" + save.tbName.Text + "','" + save.tbDescription.Text + "','" +
                            UDrawing.sA + "','" + UDrawing.sB + "','" + UDrawing.sOp + "','" + UDrawing.eA + "','" +
                            UDrawing.eB + "','" + UDrawing.eC + "'," + UDrawing.fontsize + ",'" + UDrawing.fontFamily + "','" + UDrawing.oper + "');";
                    }
                    else
                    {
                        sql = "UPDATE uniterms SET " +
          "description = '" + save.tbDescription.Text +
          "',sA = '" + UDrawing.sA +
          "',sB ='" + UDrawing.sB +
          "',sOp ='" + UDrawing.sOp +
          "',eA = '" + UDrawing.eA +
          "',eB = '" + UDrawing.eB +
          "',eC = '" + UDrawing.eC +
          "',fontSize =" + UDrawing.fontsize +
          ",fontFamily = '" + UDrawing.fontFamily +
          "',switched ='" + UDrawing.oper +
            "' WHERE name ='" + save.tbName.Text + "';";
                    }
                    db.RunQuery(sql);

                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Wystąpił błąd", MessageBoxButton.OK, MessageBoxImage.Error);
                }

                Window_Loaded(sender, e);

                // lbUniterms.SelectionChanged -= ehlbUNitermsSelectionChanged;
                // lbUniterms.SelectedValue = tbName.Text;
                // lbUniterms.SelectionChanged += ehlbUNitermsSelectionChanged;
            }
        }

        private void MenuItem_Click_1(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void btnClear_Click(object sender, RoutedEventArgs e)
        {
            cDrawing.ClearAll();
        }


        private bool CheckSave()
        {

            if (!modified)
                return true;
            else
            {
                switch (MessageBox.Show("Chcesz zapisać?", "Zapis", MessageBoxButton.YesNoCancel, MessageBoxImage.Question))
                {
                    case MessageBoxResult.Yes:
                        {
                            MenuItem_Click_1(null, null); 
                            modified = false;
                            nowy = false;
                            return true;
                        }
                    case MessageBoxResult.No:
                        {
                            modified = false;
                            nowy = false;
                            return true;
                        }
                    case MessageBoxResult.Cancel: return false;
                    default: return false;
                }
            }

        }


        private void Load_Click(object sender, RoutedEventArgs e)
        {

            Load load = new Load();
            load.ShowDialog();

            
            if (load.sel.Length != 0)
            {
                if (CheckSave())
                {
                    DataRow dr;
                    try
                    {
                        dr = db.CreateDataRow(String.Format("select * from uniterms where name = '{0}';", load.sel));


                        UDrawing.eA = (string)dr["eA"];
                        UDrawing.eB = (string)dr["eB"];
                        UDrawing.eC = (string)dr["eC"];
                       
                        UDrawing.sA = (string)dr["sA"];
                        UDrawing.sB = (string)dr["sB"];
                        UDrawing.sOp = (string)dr["sOp"];
                        Console.WriteLine(UDrawing.sA.Length);
                        UDrawing.fontFamily = new FontFamily((string)dr["fontFamily"]);
                        UDrawing.fontsize = (Int16)dr["fontSize"];
                        UDrawing.oper = ((string)dr["switched"])[0]; ;

                        

                        cbFonts.SelectedValue = UDrawing.fontFamily;
                        cbfSize.SelectedValue = (int)UDrawing.fontsize;

                        cDrawing.ClearAll();



                        DrawingVisual dv = new DrawingVisual();
                     

                        btnRedraw_Click(sender, e);
                        nowy = false;
                        modified = false;
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }

                }
            }

        }

        private void HorScroll_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
    {
        TranslateTransform tt = new TranslateTransform();
        tt.X = -HorScroll.Value;
        tt.Y = -VerScroll.Value;

        cDrawing.RenderTransform = tt;
    }

    }


    
}
