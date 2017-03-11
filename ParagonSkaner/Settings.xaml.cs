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
using WIA;

namespace ParagonSkaner
{
    
    /// <summary>
    /// Interaction logic for Settings.xaml
    /// </summary>
    public partial class Settings : Window
    {
        public string path;
        public bool needed = true;
        public WIA.Device default_device;
        string source="set.dat";
        string defaulte = null;
        //FileStream fs = null;
        string[] lines = null;
        public Settings()
        {
            InitializeComponent();
            if (!File.Exists(source))
            {
                
                defaulte = System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDoc‌​uments), "Paragons\\");
                File.AppendAllText(source, defaulte);
                if (!Directory.Exists(defaulte))
                {
                    Directory.CreateDirectory(defaulte);
                }
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
                Path_Source.Text = defaulte;
            }

            
        }

        private void Submit_Click(object sender, RoutedEventArgs e)
        {
            path=Path_Source.Text;
            if (!path.EndsWith(System.IO.Path.DirectorySeparatorChar.ToString()))
            {
                path += System.IO.Path.DirectorySeparatorChar;
            }
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
        private void set_scaner_Click(object sender, RoutedEventArgs e)
        {
            CommonDialog dialog = new WIA.CommonDialog();
            default_device = dialog.ShowSelectDevice(WiaDeviceType.ScannerDeviceType, true, false);
            DeviceInfo dev_ifo = null;
            //dev_ifo.Properties.
            //Device.Text = 
        }
    }
}
