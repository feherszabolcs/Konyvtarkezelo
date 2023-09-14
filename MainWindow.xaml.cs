using System;
using System.Collections.Generic;
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
using WinForms = System.Windows.Forms;

namespace Könyvtárak
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private string? path = null;
        public MainWindow()
        {
            InitializeComponent();
            path = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            UpdateListBox();
        }
        private void UpdateListBox()
        {
            List<string> list = Directory.GetFileSystemEntries(path, "*", SearchOption.TopDirectoryOnly).ToList();
            lboxKönyvtárList.Items.Clear();
            foreach (var file in list)
            {
                lboxKönyvtárList.Items.Add(System.IO.Path.GetFileName(file));
            }
        }

        private void btnOpenKönyvtár_Click(object sender, RoutedEventArgs e)
        {
            WinForms.FolderBrowserDialog dialog = new WinForms.FolderBrowserDialog();
            WinForms.DialogResult result = dialog.ShowDialog();
            if (result == WinForms.DialogResult.OK) {
                path = dialog.SelectedPath;
                UpdateListBox();
            } 
            else MessageBox.Show("Könyvtár kiválasztás sikertelen", "Hiba", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        private void btnCreateMappa_Click(object sender, RoutedEventArgs e)
        {
            if (!String.IsNullOrWhiteSpace(tbMappa.Text))
            {
                if (CheckIfExists(tbMappa.Text))
                {
                    try
                    {
                        DirectoryInfo directoryInfo = Directory.CreateDirectory($"{path}\\{tbMappa.Text}");
                        MessageBox.Show("Mappa létrehozva", "Info", MessageBoxButton.OK, MessageBoxImage.Information);
                        UpdateListBox();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Nem sikerült a mappa létrehozása: " + ex.Message, "Hiba", MessageBoxButton.OK, MessageBoxImage.Error);
                        throw;
                    }
                }
                else
                {
                    MessageBox.Show("Nem sikerült a mappa létrehozása: már létezik ", "Hiba", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
            {
                MessageBox.Show("Nem sikerült a mappa létrehozása: hibás név", "Hiba", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private bool CheckIfExists(string name)
        {
            if (lboxKönyvtárList.Items.Count != 0 && !Directory.Exists(path + "\\" + name)) return false;
            return true;
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            FileAttributes fAttr = File.GetAttributes(path + "\\" + lboxKönyvtárList.SelectedItem.ToString());

            if (fAttr.HasFlag(FileAttributes.Directory)) //Flag vizsgálat: Mappa-e?
            {
                    try
                    {
                        Directory.Delete(path + "\\" + lboxKönyvtárList.SelectedItem.ToString());
                        UpdateListBox();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Nem sikerült a mappa törlése! " + ex.Message, "Hiba", MessageBoxButton.OK, MessageBoxImage.Error);
                        throw;
                    }
            }
            else
            {
                File.Delete(path + "\\" + lboxKönyvtárList.SelectedItem.ToString());
                UpdateListBox();
            }
            
        }

        private void lboxKönyvtárList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (lboxKönyvtárList.SelectedIndex != -1) btnDelete.IsEnabled = true;
        }

        private void btnFileCreate_Click(object sender, RoutedEventArgs e)
        {
            if (!String.IsNullOrWhiteSpace(tbFileName.Text))
            {
                try
                {
                    StreamWriter sw = new StreamWriter($"{path}\\{tbFileName.Text}");
                    sw.Close();
                    MessageBox.Show("Létrejött a fájl.", "Infó", MessageBoxButton.OK, MessageBoxImage.Information);

                    var fileDest = $"{path}\\{tbFileName.Text}";
                    string? dir = System.IO.Path.GetDirectoryName(fileDest);
                    string fullName = System.IO.Path.GetFileName(fileDest);
                    string fullNameNoExt = System.IO.Path.GetFileNameWithoutExtension(fileDest);
                    string extension = System.IO.Path.GetExtension(fileDest);

                    lblFileData.Content = $"File adatok: \n\t" +
                        $"Könyvtár: {dir} \n\t" +
                        $"Teljes név: {fullName} \n\t" +
                        $"Kiterjesztés nélküli név: {fullNameNoExt} \n\t" +
                        $"Kiterjesztés: {extension}";
                    UpdateListBox();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Nem sikerült a fájl létrehozása! " + ex.Message, "Hiba", MessageBoxButton.OK, MessageBoxImage.Error);
                    throw;
                }
               
            }
            else
            {
                MessageBox.Show("Nem sikerült a fájl létrehozása: nem adott meg nevet!", "Hiba", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }

    }