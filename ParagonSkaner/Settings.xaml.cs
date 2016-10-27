using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
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

namespace ParagonSkaner
{
    
    /// <summary>
    /// Interaction logic for Settings.xaml
    /// </summary>
    public partial class Settings : Window
    {
        public string path;
        public bool needed = true;
        string source="set.dat";
        //FileStream fs = null;
        string[] lines = null;
        public Settings()
        {
            InitializeComponent();
            if (!File.Exists(source))
            {
                File.AppendAllText(source,"paragon\\");
            }
            //fs = new FileStream(source, FileMode.Open, FileAccess.ReadWrite);
            try
            {
                //StreamReader sr = new StreamReader(fs);
                //path = sr.ReadLine();
                lines = File.ReadAllLines(source);
                path = lines[0];
                Path_Source.Text = path;
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.ToString());
                Path_Source.Text = "paragon";
            }
            if (!Directory.Exists("paragon"))
            {
                Directory.CreateDirectory("paragon");
            }
        }

        private void Submit_Click(object sender, RoutedEventArgs e)
        {
            path=Path_Source.Text;
            lines[0] = path;
            File.WriteAllLines(source, lines);
        }

        //public void Close()
        //{
        //    this.Hide();
        //}
        void Settings_Closing(object sender, CancelEventArgs e)
        {
            if(needed==true)
            {
                e.Cancel = true;
                this.Hide();
            }
        }
    }
}
