using System;
using System.Collections.Generic;
using System.Windows;

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
            switchIsLoadedState(false);
            konyvekLIST = Adatbazis.SELECT("*");
            MainDataGrid.ItemsSource = konyvekLIST;
            
            StatusTextView.Content = konyvekLIST.Count > 0?"":"Nincs egy adat se";
            switchIsLoadedState(true);
        }
    }
}
