using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace IKT_Konyvek_Lazulasz_Partizasz{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow{

        static List<Konyv> konyvekLIST = new List<Konyv>();
        static Boolean isLoaded = false;
        
        public MainWindow(){
            InitializeComponent();

            betoltes();
            MainDataGrid.SelectionChanged += new SelectionChangedEventHandler(MainDataGrid_SelectionChanged);
            SaveButton.Click += mentesGomb;
        }

        void mentesGomb(Object sender, RoutedEventArgs e){
            modositas();
        }
        
        void switchIsLoadedState(Boolean state){
            isLoaded = state;
            disableAndEnableThings();
        }

        void disableAndEnableThings(){
            NewButton.IsEnabled = isLoaded;
            SaveButton.IsEnabled = isLoaded;
            TitleTextBox.IsEnabled = isLoaded;
            WriterTextBox.IsEnabled = isLoaded;
            PublisherTextBox.IsEnabled = isLoaded;
            PriceTextBox.IsEnabled = isLoaded;
            StockTextBox.IsEnabled = isLoaded;

            ProgressBar.Visibility = isLoaded?Visibility.Collapsed:Visibility.Visible;

        }

        void betoltes(){
            MainDataGrid.ItemsSource = null;
            switchIsLoadedState(false);
            konyvekLIST = Adatbazis.SELECT("*");
            MainDataGrid.ItemsSource = konyvekLIST;
            
            StatusTextView.Content = konyvekLIST.Count > 0?"":"Nincs egy adat se";
            switchIsLoadedState(true);
        }

        void modositas(){
            Konyv konyv = konyvekLIST[MainDataGrid.SelectedIndex];

            if(TitleTextBox.Text.Length < 0 || WriterTextBox.Text.Length < 0 || PublisherTextBox.Text.Length < 0 || PriceTextBox.Text.Length < 0 || StockTextBox.Text.Length < 0){
                changeStatusText("Nem jó adatok");
            }else{
                if(double.TryParse(PriceTextBox.Text.ToString(), out double ar) && Int32.TryParse(StockTextBox.Text.ToString(), out int raktaron)){
                    switchIsLoadedState(false);
                    konyv.Cim = TitleTextBox.Text;
                    konyv.Szerzo = WriterTextBox.Text;
                    konyv.Kiado = PublisherTextBox.Text;
                    konyv.Ar = ar;
                    konyv.Raktaron = raktaron;
                    
                    if(Adatbazis.UPDATE(konyv.getID(), konyv)){
                        StatusTextView.Content = "Siker!";
                        betoltes();
                    }else{
                        changeStatusText("Sikertelen művelet!");
                    }
                    switchIsLoadedState(true);
                }else changeStatusText("Az adatok nincsenek jól formázva!");
            }
            
        }

        void changeStatusText(String text){
            StatusTextView.Content = text;
        }

        void MainDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e){
            if(!isLoaded) return;

            Konyv konyv = konyvekLIST[MainDataGrid.SelectedIndex];
            TitleTextBox.Text = konyv.Cim;
            WriterTextBox.Text = konyv.Szerzo;
            PublisherTextBox.Text = konyv.Kiado;
            PriceTextBox.Text = konyv.Ar + "";
            StockTextBox.Text = konyv.Raktaron + "";
        }
    }
}
