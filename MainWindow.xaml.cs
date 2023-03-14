using System;
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
using System.Windows.Controls.Ribbon;
using Microsoft.WindowsAPICodePack.Dialogs;
using System.IO;
using System.Windows.Threading;

namespace OAP_Ex3_1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public string[] files;
        public string[] files1;
        int i = 0;
        int mix = 0;  //если перемешали то 1
        public MainWindow()
        {       
  
            InitializeComponent();

        }

        private void Pusk_Click(object sender, RoutedEventArgs e)
        {
            Media.Play();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            CommonOpenFileDialog dialog = new CommonOpenFileDialog { IsFolderPicker=true};
            var result = dialog.ShowDialog();
            if (result == CommonFileDialogResult.Ok)
            {
                files = Directory.GetFiles(dialog.FileName,"*.mp3");
                if (mix==0) files1 = files;  // на случай перемешивания
                Mlist.ItemsSource= files;
                Media.Source = new Uri(files[0]); 
                Media.Position = new TimeSpan(Convert.ToInt64(myslider.Value));
                Media.Volume= 0.01;
                Media.Play();
            }
        }
        
        private void myslider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
           Media.Position = new TimeSpan(Convert.ToInt64(myslider.Value));
           Mytime.Text = Media.Position.ToString(@"hh\:mm\:ss") +"/"+ Media.NaturalDuration.TimeSpan.ToString(@"hh\:mm\:ss"); 
        }



        private void Volume_ValueChanged_1(object sender, RoutedPropertyChangedEventArgs<double> e)

        {
            
            Media.Volume= Volume.Value/100;

        }


        private void ChangeMediaVolume(object sender, RoutedPropertyChangedEventArgs<double> args)
        {
            Media.Volume = (double)Volume.Value;
        }

        private void Play_Click(object sender, RoutedEventArgs e)
        {
            Media.Pause();
        }

        private void Media_MediaOpened(object sender, RoutedEventArgs e)
        {
            myslider.Maximum= Media.NaturalDuration.TimeSpan.Ticks;
            Mytime.Text = TimeSpan.FromSeconds(myslider.Value) + "/" + Media.NaturalDuration.TimeSpan.ToString(@"hh\:mm\:ss");
            myslider.Value= 0;
        }

        private void Media_MediaEnded(object sender, RoutedEventArgs e) // песня закончилась
        {

            MessageBox.Show("asdfdasdfasdf");
            if (i == files.Length - 1)
                i = 0;
            else
            {
                i++;
            }
            Media.Source = new Uri(files[i]);
            Media.Play();

        }



        private void GO_Click(object sender, RoutedEventArgs e)
        {
            
            
            Media.Stop();
            if (i==files.Length-1)
                i = 0;
            else {
                i++;
                 }
            Media.Source = new Uri(files[i]);
            Media.Play();



        }

        private void Replay_Click(object sender, RoutedEventArgs e)
        {
            Media.Stop();
            if (i==0)
                i = files.Length-1;
            else
            {
                i--;
            }
            Media.Source = new Uri(files[i]);
            Media.Play();

        }

        private void Accidentally_Click(object sender, RoutedEventArgs e)
        {

            if (mix == 0) // перемешиваем
            {
                var rand = new Random();
                for (int k = files.Length - 1; k > 0; k--)
                {
                    int j = rand.Next(i);
                    var t = files[k];
                    files[k] = files[j];
                    files[j] = t;
                }
                mix = 1; // перемешали
                Mlist.Items.Refresh();
                Media.Stop();
                i = 0;
                Media.Source = new Uri(files[i]);
                Media.Play();
               
            }
            else // возвращаем по алфавиту
            {
                files = files1;
                mix = 0; 
                Mlist.Items.Refresh();
                Media.Stop();
                i = 0;
                Media.Source = new Uri(files[i]);
                Media.Play();
            }


        }
    }
}
