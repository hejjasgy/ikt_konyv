using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace IKT_Konyvek_Lazulasz_Partizasz{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow{

        static List<Konyv> eredetiKonyvekLIST = new List<Konyv>();
        static List<Konyv> konyvekLIST = new List<Konyv>();
        static Boolean isLoaded = false;
        public static Label StatusTextBlock;
        RadioButton SelectedRadioButton;
        
        public MainWindow(){
            InitializeComponent();

            StatusTextBlock = StatusTextView;
            switchIsLoadedState(false);
            betoltes();
            MainDataGrid.SelectionChanged += new SelectionChangedEventHandler(MainDataGrid_SelectionChanged);
            SaveButton.Click += mentesGomb;
            DeleteButton.Click += torlesGomb;
            FilteringRadioButton.Checked += switchRadioButtonMode;
            ModifyRadioButton.Checked += switchRadioButtonMode;
            NewRadioButton.Checked += switchRadioButtonMode;
            SelectedRadioButton = FilteringRadioButton;
        }
        

        void ujGomb(){
            if(TitleTextBox.Text.Length > 0 || WriterTextBox.Text.Length > 0 || PublisherTextBox.Text.Length > 0 || PriceTextBox.Text.Length > 0 || StockTextBox.Text.Length > 0){
                MessageBoxResult messageBoxResult = MessageBox.Show("Valóban új könyvet szeretnél hozzáadni az adatbázishoz? Alább eldöntheted, hogy töröljük-e a beviteli mezők tartalmát, vagy ezekkel az adatokkal dolgozzunk.", "Új?", MessageBoxButton.YesNoCancel);
                if(messageBoxResult==MessageBoxResult.Cancel){
                    SelectedRadioButton.IsChecked = true;
                    restoreRadioButton();
                    return;
                }else if(messageBoxResult== MessageBoxResult.Yes) clearEditTextViews();
                
                MainDataGrid.ItemsSource = konyvekLIST;
            
                StatusTextView.Content = konyvekLIST.Count > 0?"":"Nincs egy adat se";
                switchIsLoadedState(true);
            }
            
            SelectedRadioButton = NewRadioButton;
            radioButtonCheck();
        }
        
        void torlesGomb(Object sender, RoutedEventArgs e){
            if(isLoaded && MainDataGrid.SelectedIndex > -1){
                MessageBoxResult messageBoxResult = MessageBox.Show("Valóban törölni akarod a(z) " + konyvekLIST[MainDataGrid.SelectedIndex].Cim + " nevű könyvet?", "Törlés?", MessageBoxButton.YesNo);
                if(messageBoxResult!=MessageBoxResult.Yes){
                    return;
                }
                switchIsLoadedState(false);
                Adatbazis.DELETE(konyvekLIST[MainDataGrid.SelectedIndex].getID());
                betoltes();
                switchIsLoadedState(true);
                SelectedRadioButton = NewRadioButton;
                radioButtonCheck();
            }else{
                changeStatusText("Nincs kijelölt könyv");
            }
        }
        
        void mentesGomb(Object sender, RoutedEventArgs e){
            switchIsLoadedState(false);
            if(SelectedRadioButton == NewRadioButton){
                ujRekord();
            }else modositas();
        }
        
        void switchIsLoadedState(Boolean state){
            isLoaded = state;
            disableAndEnableThings();
        }

        void disableAndEnableThings(){

            if(isLoaded) StatusTextView.Content = "";

            FilteringRadioButton.IsEnabled = isLoaded;
            ModifyRadioButton.IsEnabled = isLoaded && MainDataGrid.SelectedIndex>-1;
            NewRadioButton.IsEnabled = isLoaded;
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
            eredetiKonyvekLIST = Adatbazis.SELECT("*");
            konyvekLIST = eredetiKonyvekLIST;
            MainDataGrid.ItemsSource = konyvekLIST;

            if(SelectedRadioButton == FilteringRadioButton){
                loadByFilter();
            }
            
            if(konyvekLIST[0].getID() == -1){
                StatusTextView.Content = "Hiba történt! " + konyvekLIST[0].Cim;
            }
            switchIsLoadedState(true);
        }

        void modositas(){
            if(MainDataGrid.SelectedIndex < 0){
                changeStatusText("Nincs kijelölt könyv");
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
                    int selectedIndex = MainDataGrid.SelectedIndex;
                    Adatbazis.UPDATE(konyv.getID(), konyv);
                    StatusTextView.Content = "Siker!";
                    betoltes();
                    MainDataGrid.SelectedIndex = selectedIndex;
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
                    ModifyRadioButton.IsChecked = true;
                    MainDataGrid.SelectedIndex = konyvekLIST.Count - 1;
                    SelectedRadioButton = ModifyRadioButton;
                    radioButtonCheck();
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

            SelectedRadioButton = ModifyRadioButton;
            restoreRadioButton();
            
            Konyv konyv = konyvekLIST[MainDataGrid.SelectedIndex];
            TitleTextBox.Text = konyv.Cim;
            WriterTextBox.Text = konyv.Szerzo;
            PublisherTextBox.Text = konyv.Kiado;
            PriceTextBox.Text = konyv.Ar + "";
            StockTextBox.Text = konyv.Raktaron + "";
            SelectedRadioButton = ModifyRadioButton;
            radioButtonCheck();
        }

        void switchRadioButtonMode(Object sender, RoutedEventArgs e){
            if((Boolean)FilteringRadioButton.IsChecked) SelectedRadioButton = FilteringRadioButton;
            if((Boolean)ModifyRadioButton.IsChecked) SelectedRadioButton = ModifyRadioButton;
           radioButtonCheck();
        }

        void radioButtonCheck(){
            ModifyRadioButton.IsEnabled = isLoaded && MainDataGrid.SelectedIndex>-1;
            
            if((Boolean)NewRadioButton.IsChecked && SelectedRadioButton !=NewRadioButton){
                ujGomb();
            }

            if(SelectedRadioButton == FilteringRadioButton) loadByFilter();
            
            if(SelectedRadioButton == NewRadioButton){
                SaveButton.Content = "Hozzáadás";
            }else if(SelectedRadioButton==ModifyRadioButton){
                SaveButton.Content = "Módosítás";
            }
            
            
            SaveButton.Visibility = (SelectedRadioButton == ModifyRadioButton||SelectedRadioButton==NewRadioButton)?Visibility.Visible:Visibility.Collapsed;
            DeleteButton.Visibility = SelectedRadioButton == ModifyRadioButton?Visibility.Visible:Visibility.Collapsed;
        }       
        
        void restoreRadioButton(){
            FilteringRadioButton.IsChecked = SelectedRadioButton == FilteringRadioButton;
            ModifyRadioButton.IsChecked = SelectedRadioButton == ModifyRadioButton;
            NewRadioButton.IsChecked = false;
        }

        private void TitleTextBox_TextChanged(object sender, TextChangedEventArgs e){
            writeCheck();
        }

        private void PublisherTextBox_TextChanged(object sender, TextChangedEventArgs e) {
            writeCheck();
        }

        private void StockTextBox_TextChanged(object sender, TextChangedEventArgs e) {
            writeCheck();
        }

        private void WriterTextBox_TextChanged(object sender, TextChangedEventArgs e) {
            writeCheck();
        }

        private void PriceTextBox_TextChanged(object sender, TextChangedEventArgs e) {
            writeCheck();
        }

        void loadByFilter(){
            switchIsLoadedState(false);
            List<Konyv> filteredKonyvs = new List<Konyv>();

            if(TitleTextBox.Text.Length < 1 && WriterTextBox.Text.Length < 1 && PublisherTextBox.Text.Length < 1 && PriceTextBox.Text.Length < 1 && StockTextBox.Text.Length < 1){
                filteredKonyvs = eredetiKonyvekLIST;
            }else{
                foreach(Konyv konyv in eredetiKonyvekLIST){
                    Boolean matches = true;
                    if(TitleTextBox.Text.Length > 0){
                        if(!konyv.Cim.ToLower().Contains(TitleTextBox.Text.ToLower())) matches = false;
                    }
                    if(WriterTextBox.Text.ToLower().Length > 0){
                        if(!konyv.Szerzo.ToLower().Contains(WriterTextBox.Text.ToLower())) matches = false;
                    }
                    if(PublisherTextBox.Text.ToLower().Length > 0){
                        if(!konyv.Kiado.ToLower().Contains(PublisherTextBox.Text.ToLower())) matches = false;
                    }  
                    if(PriceTextBox.Text.ToLower().Length > 0){
                        if(konyv.Ar!=Int32.Parse(PriceTextBox.Text.ToLower())) matches = false;
                    } 
                    if(StockTextBox.Text.ToLower().Length > 0){
                        if(konyv.Raktaron!=Int32.Parse(StockTextBox.Text.ToLower())) matches = false;
                    }

                    if(matches)filteredKonyvs.Add(konyv);
                }
            }
            
            konyvekLIST = filteredKonyvs;
            StatusTextView.Content = konyvekLIST.Count > 0?"":"Nincs ilyen könyv";

            try{
                MainDataGrid.ItemsSource = konyvekLIST;
                MainDataGrid.SelectedIndex = konyvekLIST.Count - 1;
            }catch(Exception e){
                Console.WriteLine(e);
                StatusTextView.Content = "Nincs ilyen könyv";
            }
            switchIsLoadedState(true);
        }
        
        void writeCheck(){
            if(SelectedRadioButton == FilteringRadioButton){
                loadByFilter();
            }
        }
        
        private void tbNumber_PreviewTextInput(object sender, TextCompositionEventArgs e){
            if(!char.IsDigit(e.Text[0])){
                e.Handled = true;
            }
        }

        void clearEditTextViews(){
            TitleTextBox.Text = "";
            WriterTextBox.Text = "";
            PublisherTextBox.Text = "";
            PriceTextBox.Text = "";
            StockTextBox.Text = "";
        }

        
    }
}
