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
using System.Windows.Input;
using System.IO;
using Microsoft.Win32;
using System.ComponentModel;

namespace spr
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private bool zapisane = false;
        private string sciezka = "";
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Wytnij_Click(object sender, EventArgs e)
        {
            Text.Cut();
        }

        private void Kopiuj_Click(object sender, EventArgs e)
        {
            Text.Copy();
        }

        private void Wklej_Click(object sender, EventArgs e)
        {
            Text.Paste();
        }

        private void Usun_Click(object sender, EventArgs e)
        {
            int poczatekZaznaczenia = Text.SelectionStart;
            Text.Text = Text.Text.Remove(poczatekZaznaczenia, Text.SelectionLength);
            Text.Select(poczatekZaznaczenia, 0);
        }

        private void Otworz_Click(object sender, EventArgs e)
        {
            Otworz();
        }

        private void ZapiszJako_Click(object sender, EventArgs e)
        {
            ZapiszJako();
        }

        private void Zapisz_Click(object sender, EventArgs e)
        {
            Zapisz();
        }

        private void Otworz()
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Filter = "txt files (*.txt)|*.txt";
            dialog.Multiselect = false;
            dialog.Title = "Otwórz";
            if (dialog.ShowDialog() == true)
            {
                Text.Text = File.ReadAllText(dialog.FileName);
                sciezka = dialog.FileName;
                zapisane = true;
            }
        }

        private bool ZapiszJako()
        {
            SaveFileDialog dialog = new SaveFileDialog();
            dialog.Title = "Zapisz jako";
            dialog.Filter = "txt files (*.txt)|*.txt";
            dialog.AddExtension = true;
            if (dialog.ShowDialog() == true)
            {
                File.WriteAllText(dialog.FileName, Text.Text);
                sciezka = dialog.FileName;
                zapisane = true;
                return true;
            }
            return false;
        }

        private void Zapisz()
        {
            if (sciezka == "")
            {
                ZapiszJako();
            }
            else
            {
                File.WriteAllText(sciezka, Text.Text);
                zapisane = true;
            }
        }

        private void Zakoncz_Click(object sender, EventArgs e)
        {
            Close();
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            if (!zapisane)
            {
                MessageBoxResult odpowiedz = MessageBox.Show("Czy chcesz zapisać zmiany w pliku?", "Notatnik", MessageBoxButton.YesNoCancel);
                if (odpowiedz == MessageBoxResult.Yes)
                {
                    if (sciezka == "")
                    {
                        if (!ZapiszJako())
                            e.Cancel = true;
                    }
                    else
                        Zapisz();
                }
                else if (odpowiedz == MessageBoxResult.Cancel)
                    e.Cancel = true;
            }
            base.OnClosing(e);
        }

        private void ZmianaTextu_Click(object sender, EventArgs e)
        {
            if (sciezka != "")
                if (File.ReadAllText(sciezka) != Text.Text)
                    zapisane = false;
        }

        private void WcisnieciePrzycisku_Click(object sender, KeyEventArgs e)
        {
            if (Keyboard.IsKeyDown(Key.LeftCtrl) || Keyboard.IsKeyDown(Key.RightCtrl))
            {
                if (e.Key == Key.O)
                {
                    Otworz();
                    Keyboard.ClearFocus();
                }
                if (e.Key == Key.S)
                {
                    Zapisz();
                    Keyboard.ClearFocus();
                }
                if (Keyboard.IsKeyDown(Key.LeftShift) || Keyboard.IsKeyDown(Key.RightShift))
                    if (e.Key == Key.S)
                    {
                        ZapiszJako();
                        Keyboard.ClearFocus();
                    }

            }
        }
    }
}
