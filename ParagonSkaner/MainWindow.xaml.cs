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
        BitmapImage image;
        Uri imageSource;
        string[] Scans;
        String destination;
        Settings Set = null;
        int view;
        Device WiaDev = null;
        public MainWindow()
        {
            InitializeComponent();
            Set = new Settings();
            WiaDev = Set.default_device;
            ShowPreview(true);           
        }

        void LogContent(String name,String str)
        {
            Log.Text += name + ": " + str + "\n";
        }

        void ShowPreview(bool last=false)
        {
            Scans = Directory.GetFileSystemEntries(Set.path);
            if(last==true)
            {
                view = Scans.Length - 1;
            }
            if (view<0)
            {
                view = 0;
            }
            if (Scans.Length > 0)
            {
                image = new BitmapImage();
                image.BeginInit();
                image.CacheOption = BitmapCacheOption.OnLoad;
                imageSource = new Uri(Scans[view]);
                image.UriSource = imageSource;
                image.EndInit();
                image_show.Source = image;
                File_show.Text = Scans[view];
            }
            else
            {
                LogContent("Preview", "Folder nie zawiera plików");
                File_show.Text = "";
            }

        }
            

        ImageFile Scan(String str)
        {
            LogContent("process", "Scanning");
            CommonDialog dialog = new WIA.CommonDialog();
            ImageFile scannedImage = null;
            if (Set.default_device==null)
            {
                WiaDev = dialog.ShowSelectDevice(WiaDeviceType.ScannerDeviceType, true, false);
                MessageBoxResult result = MessageBox.Show("Ustawić urządzenie jako domyślne?", "Potwierdzenie", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if(result==MessageBoxResult.Yes)
                {
                    Set.default_device = WiaDev;
                }
            }
            else
            {
                WiaDev = Set.default_device;
            }

            //Start Scan

            WIA.Item Item = WiaDev.Items[1] as WIA.Item;
            
            try
            {
                scannedImage = (ImageFile)dialog.ShowTransfer(Item, EnvFormatID.wiaFormatPNG, false);
                String path = Set.path;
                destination = path + str + "_" + Note.Text + ".png";
                LogContent("destination", destination);
                scannedImage.SaveFile(destination);
                return scannedImage;
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
            
            ShowPreview(true);
        }

        //Obsługa podglądu paragonów
        private void button_previous_Click(object sender, RoutedEventArgs e)
        {
            if (view == 0)
            {
                view = Scans.Length - 1;
            }
            else
            {
                view -= 1;
            }
            ShowPreview();
        }

        private void button_next_Click(object sender, RoutedEventArgs e)
        {
            if (view == (Scans.Length - 1))
            {
                view = 0;
            }
            else
            {
                view += 1;
            }
            ShowPreview();
        }

        private void button_del_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Na pewno chcesz usunąć ten skan?", "Potwierdzenie", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
            {
                try
                {
                    File.Delete(Scans[view]);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message);
                }
                finally
                {
                    view -= 1;
                    ShowPreview();
                }
            }
        }
        //zooming
        //private void image_MouseWheel(object sender, MouseWheelEventArgs e)
        //{
        //    image_show.ren
        //    var st = (ScaleTransform)image_show.RenderTransform;
        //    double zoom = e.Delta > 0 ? .2 : -.2;
        //    st.ScaleX += zoom;
        //    st.ScaleY += zoom;
        //}
        private void image_MouseWheel(object sender, MouseWheelEventArgs e) { }


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
