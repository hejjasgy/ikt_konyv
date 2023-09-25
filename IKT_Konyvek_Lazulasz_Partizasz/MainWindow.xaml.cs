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
            DeleteButton.Click += torlesGomb;
            NewCheckbox.Checked += ujGomb;
        }

        void ujGomb(Object sender, RoutedEventArgs e){
            if(TitleTextBox.Text.Length > 0 || WriterTextBox.Text.Length > 0 || PublisherTextBox.Text.Length > 0 || PriceTextBox.Text.Length > 0 || StockTextBox.Text.Length > 0){
                MessageBoxResult messageBoxResult = MessageBox.Show("Valóban új könyvet szeretnél hozzáadni az adatbázishoz? Ezzel törlöd a jelenleg módosítás alatt lévő könyv módosításait és a jelenlegi kijelölést. Szeretnéd a bevitt adatokat kitörölni?", "Új?", MessageBoxButton.YesNoCancel);
                if(messageBoxResult==MessageBoxResult.Cancel){
                    NewCheckbox.IsChecked = false;
                    return;
                }else if(messageBoxResult== MessageBoxResult.Yes){
                    TitleTextBox.Text = "";
                    WriterTextBox.Text = "";
                    PublisherTextBox.Text = "";
                    PriceTextBox.Text = "";
                    StockTextBox.Text = "";
                }
        
                MainDataGrid.ItemsSource = konyvekLIST;
            
                StatusTextView.Content = konyvekLIST.Count > 0?"":"Nincs egy adat se";
                switchIsLoadedState(true);
            }
       
        }
        
        void torlesGomb(Object sender, RoutedEventArgs e){
            if(isLoaded && MainDataGrid.SelectedIndex > 0){
                MessageBoxResult messageBoxResult = MessageBox.Show("Valóban törölni akarod a(z) " + konyvekLIST[MainDataGrid.SelectedIndex].Cim + " nevű könyvet?", "Törlés?", MessageBoxButton.YesNo);
                if(messageBoxResult!=MessageBoxResult.Yes){
                    return;
                }
                switchIsLoadedState(false);
                Adatbazis.DELETE(konyvekLIST[MainDataGrid.SelectedIndex].getID());
                betoltes();
                switchIsLoadedState(true);
               
            }else{
                MessageBox.Show("Nincs kijelölt könyv");
                changeStatusText("Nincs kijelölve semmi");
            }
        }
        
        void mentesGomb(Object sender, RoutedEventArgs e){
            switchIsLoadedState(false);
            if(NewCheckbox.IsChecked== true){
                ujRekord();
            }else modositas();
        }
        
        void switchIsLoadedState(Boolean state){
            isLoaded = state;
            disableAndEnableThings();
        }

        void disableAndEnableThings(){

            if(isLoaded) StatusTextView.Content = "";
            
            NewCheckbox.IsEnabled = isLoaded;
            SaveButton.IsEnabled = isLoaded;
            DeleteButton.IsEnabled = isLoaded;
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
            if(MainDataGrid.SelectedIndex < 0){
                MessageBox.Show("Nincs kijelölt könyv");
                changeStatusText("Nincs kijelölve semmi");
                switchIsLoadedState(true);
                return;
            };
            Konyv konyv = konyvekLIST[MainDataGrid.SelectedIndex];

            if(TitleTextBox.Text.Length < 1 || WriterTextBox.Text.Length < 1 || PublisherTextBox.Text.Length < 1 || PriceTextBox.Text.Length < 1 || StockTextBox.Text.Length < 1){
                changeStatusText("Nem jó adatok");
                switchIsLoadedState(true);
            }else{
                if(Int32.TryParse(PriceTextBox.Text.ToString(), out int ar) && Int32.TryParse(StockTextBox.Text.ToString(), out int raktaron)){
                    konyv.Cim = TitleTextBox.Text;
                    konyv.Szerzo = WriterTextBox.Text;
                    konyv.Kiado = PublisherTextBox.Text;
                    konyv.Ar = ar;
                    konyv.Raktaron = raktaron;
                    Adatbazis.UPDATE(konyv.getID(), konyv);
                    StatusTextView.Content = "Siker!";
                    betoltes();
                }else{
                    changeStatusText("Az adatok nincsenek jól formázva!");
                }
                switchIsLoadedState(true);
            }
        }      
        
        void ujRekord(){
            if(TitleTextBox.Text.Length < 1 || WriterTextBox.Text.Length < 1 || PublisherTextBox.Text.Length < 1 || PriceTextBox.Text.Length < 1 || StockTextBox.Text.Length < 1){
                changeStatusText("Nem jó adatok");
                switchIsLoadedState(true);
            }else{
                if(Int32.TryParse(PriceTextBox.Text.ToString(), out int ar) && Int32.TryParse(StockTextBox.Text.ToString(), out int raktaron)){
                    Konyv konyv = new Konyv(-1,TitleTextBox.Text, WriterTextBox.Text, PublisherTextBox.Text, ar, raktaron);
                    StatusTextView.Content = Adatbazis.INSERT(konyv);
                    betoltes();
                    NewCheckbox.IsEnabled = false;
                }else changeStatusText("Az adatok nincsenek jól formázva!");
                switchIsLoadedState(true);
            }
        }

        void changeStatusText(String text){
            StatusTextView.Content = text;
            MessageBox.Show(text);
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
