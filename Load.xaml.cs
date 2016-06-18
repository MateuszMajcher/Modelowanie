using System;
using System.Collections.Generic;
using System.Data;
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

namespace Modelowanie
{
    /// <summary>
    /// Interaction logic for Load.xaml
    /// </summary>
    public partial class Load : Window
    {
        Database db;
        public String sel
        {
            set; get;
        }

        void setO(Database o)
        {
            db = o;
        }

        public Load()
        {
            InitializeComponent();
            btnLoad.IsEnabled = false;
            sel = "";
            Database db = new Database();

            lbUniterms.Items.Clear();
            DataTable dt = db.CreateDataTable("select name from uniterms;");
            foreach (DataRow dr in dt.Rows)
            {
                lbUniterms.Items.Add(dr["name"]);
            }


           
        }


        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            sel = lbUniterms.SelectedItem.ToString();
            Console.WriteLine(sel);
            Close();
        }

        private void lbUniterms_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            btnLoad.IsEnabled = true;
            
        }
    }
}
