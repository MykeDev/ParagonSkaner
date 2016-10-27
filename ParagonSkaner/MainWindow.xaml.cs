using System;
using System.Collections.Generic;
using System.ComponentModel;
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
using WIA;


namespace ParagonSkaner
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        String destination;
        Settings Set = null;
        public MainWindow()
        {
            InitializeComponent();
            Set = new Settings();
        }

        void LogContent(String name,String str)
        {
            Log.Text += name + ": " + str + "\n";
        }


        ImageFile Scan(String str)
        {
            LogContent("process", "Scanning");
            CommonDialog dialog = new WIA.CommonDialog();
            ImageFile scannedImage = null;
           // Device device = dialog.ShowSelectDevice(WiaDeviceType.ScannerDeviceType,false,false);
            //string comm = CommandID.wiaCommandTakePicture;
           // string comm = "{AF933CAC-ACAD-11D2-A093-00C04F72DC3C}";
            //Item scannedImage = device.ExecuteCommand(comm);
            //scannedImage =scannedImage_raw.t
            //LogContent("commands", device.Commands);
            scannedImage = dialog.ShowAcquireImage(
                                WiaDeviceType.ScannerDeviceType,
                                WiaImageIntent.UnspecifiedIntent,
                                WiaImageBias.MaximizeQuality,
                                //FormatID.wiaFormatPNG,
                                EnvFormatID.wiaFormatPNG,
                                true, true, false);
            String path = Set.path;
            //"C:\\Users\\Michal\\Dysk Google\\Finanse\\paragony_trial\\"
            destination = path +  str + "_" + Note.Text + ".png";
            LogContent("destination", destination);
            try
            {
                //scannedImage.SaveFile(destination);
                //return scannedImage;
            }
            catch(Exception e)
            {
                MessageBox.Show("Error "+e.Message);
            }
            finally
            {
                
            }
            return null;
        }

        public abstract class EnvFormatID
        {
            public const string wiaFormatBMP = "{B96B3CAB-0728-11D3-9D7B-0000F81EF32E}";
            public const string wiaFormatGIF = "{B96B3CB0-0728-11D3-9D7B-0000F81EF32E}";
            public const string wiaFormatJPEG = "{B96B3CAE-0728-11D3-9D7B-0000F81EF32E}";
            public const string wiaFormatPNG = "{B96B3CAF-0728-11D3-9D7B-0000F81EF32E}";
            public const string wiaFormatTIFF = "{B96B3CB1-0728-11D3-9D7B-0000F81EF32E}";
        }

        private void button_scam_Click(object sender, RoutedEventArgs e)
        {
            String str;
            if (calCalendar.SelectedDate.HasValue)
            {
               // string format = "yyyyMMdd";

                DateTime? myDate = calCalendar.SelectedDate;
                str = (myDate.Value.ToString("yyyy-MM-dd"));

            }
            else
            {
                DateTime myDate = DateTime.Today;
                str = (myDate.ToString("yyyy-MM-dd"));
            }
            textBlock.Text = str;
            ImageFile par = Scan(str);
            //ImageSource imSource = new BitmapImage(new Uri(destination));
            //image_show.Source = imSource;
        }





        //showing selected date
       void calCalendar_changed(object sender,SelectionChangedEventArgs e)
        {
            DateTime? myDate = calCalendar.SelectedDate;
            string str = (myDate.Value.ToString("yyyy-MM-dd"));
            textBlock.Text = str;
        }


        //show settings
        private void settings_Click(object sender, RoutedEventArgs e)
        {
            Set.Show();
        }
        //destroy instance of settings
        void Window_Closing(object sender, CancelEventArgs e)
        {
            Set.needed = false;
            Set.Close();
        }
    }
}
