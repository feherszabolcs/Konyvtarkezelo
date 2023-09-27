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
using Path = System.IO.Path;
using WinForms = System.Windows.Forms;

namespace Könyvtárak
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private string pathL = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
        private string pathR = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
        public MainWindow()
        {
            InitializeComponent();
            UpdateListBox(pathL, lboxKönyvtárListLeft);
            UpdateListBox(pathR, lboxKönyvtárListRight);
        }
        private void UpdateListBox(string path, ListBox lbx)
        {
            List<string> list = Directory.GetFileSystemEntries(path, "*", SearchOption.TopDirectoryOnly).ToList();
            lbx.Items.Clear();
            foreach (var file in list)
            {
                lbx.Items.Add(Path.GetFileName(file));
            }
        }
        private bool CheckIfExists(string name, ListBox lbx, string path)
        {
            if (lbx.Items.Count != 0 && !Directory.Exists(path + "\\" + name)) return false;
            return true;
        }
        private void btnOpenKönyvtárLeft_Click(object sender, RoutedEventArgs e)
        {
            WinForms.FolderBrowserDialog dialog = new WinForms.FolderBrowserDialog();
            WinForms.DialogResult result = dialog.ShowDialog();
            if (result == WinForms.DialogResult.OK)
            {
                pathL = dialog.SelectedPath;
                UpdateListBox(pathL, lboxKönyvtárListLeft);
            }
            else MessageBox.Show("Könyvtár kiválasztás sikertelen", "Hiba", MessageBoxButton.OK, MessageBoxImage.Error);
        }
        private void btnCreateMappaLeft_Click(object sender, RoutedEventArgs e)
        {
            if (!String.IsNullOrWhiteSpace(tbMappaLeft.Text))
            {
                if (!CheckIfExists(tbMappaLeft.Text, lboxKönyvtárListLeft, pathL))
                {
                    try
                    {
                        DirectoryInfo directoryInfo = Directory.CreateDirectory($"{pathL}\\{tbMappaLeft.Text}");
                        MessageBox.Show("Mappa létrehozva", "Info", MessageBoxButton.OK, MessageBoxImage.Information);
                        UpdateListBox(pathL, lboxKönyvtárListLeft);
                        UpdateListBox(pathR, lboxKönyvtárListRight);
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
        private void btnFileCreate_Click(object sender, RoutedEventArgs e)
        {
            if (!String.IsNullOrWhiteSpace(tbFileName.Text))
            {
                try
                {
                    StreamWriter sw = new StreamWriter($"{pathL}\\{tbFileName.Text}");
                    sw.Close();
                    MessageBox.Show("Létrejött a fájl.", "Infó", MessageBoxButton.OK, MessageBoxImage.Information);

                    var fileDest = $"{pathL}\\{tbFileName.Text}";
                    string? dir = System.IO.Path.GetDirectoryName(fileDest);
                    string fullName = System.IO.Path.GetFileName(fileDest);
                    string fullNameNoExt = System.IO.Path.GetFileNameWithoutExtension(fileDest);
                    string extension = System.IO.Path.GetExtension(fileDest);

                    lblFileData.Content = $"File adatok: \n\t" +
                        $"Könyvtár: {dir} \n\t" +
                        $"Teljes név: {fullName} \n\t" +
                        $"Kiterjesztés nélküli név: {fullNameNoExt} \n\t" +
                        $"Kiterjesztés: {extension}";
                    UpdateListBox(pathL, lboxKönyvtárListLeft);
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

        private void btnCreateMappaRight_Click(object sender, RoutedEventArgs e)
        {
            if (!String.IsNullOrWhiteSpace(tbMappaRight.Text))
            {
                if (!CheckIfExists(tbMappaRight.Text, lboxKönyvtárListRight, pathR))
                {
                    try
                    {
                        DirectoryInfo directoryInfo = Directory.CreateDirectory($"{pathR}\\{tbMappaRight.Text}");
                        MessageBox.Show("Mappa létrehozva", "Info", MessageBoxButton.OK, MessageBoxImage.Information);
                        UpdateListBox(pathL, lboxKönyvtárListLeft);
                        UpdateListBox(pathR, lboxKönyvtárListRight);
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

        private void btnOpenKönyvtárRight_Click(object sender, RoutedEventArgs e)
        {
            WinForms.FolderBrowserDialog dialog = new WinForms.FolderBrowserDialog();
            WinForms.DialogResult result = dialog.ShowDialog();
            if (result == WinForms.DialogResult.OK)
            {
                pathR = dialog.SelectedPath;
                UpdateListBox(pathR, lboxKönyvtárListRight);
            }
            else MessageBox.Show("Könyvtár kiválasztása sikertelen", "Hiba", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            if (lboxKönyvtárListLeft.SelectedIndex > -1) //bal oldal aktív
            {
                FileAttributes fAttr = File.GetAttributes(pathL + "\\" + lboxKönyvtárListLeft.SelectedItem.ToString());

                if (fAttr.HasFlag(FileAttributes.Directory)) //Flag vizsgálat: Mappa-e?
                {
                    try
                    {
                        Directory.Delete(pathL + "\\" + lboxKönyvtárListLeft.SelectedItem.ToString());
                        UpdateListBox(pathL, lboxKönyvtárListLeft);
                        UpdateListBox(pathR, lboxKönyvtárListRight);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Nem sikerült a mappa törlése! " + ex.Message, "Hiba", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
                else
                {
                    File.Delete(pathL + "\\" + lboxKönyvtárListLeft.SelectedItem.ToString());
                    UpdateListBox(pathL, lboxKönyvtárListLeft);
                    UpdateListBox(pathR, lboxKönyvtárListRight);
                }
            }
            else if(lboxKönyvtárListRight.SelectedIndex > -1)
            {
                FileAttributes fAttr = File.GetAttributes(pathR + "\\" + lboxKönyvtárListRight.SelectedItem.ToString());

                if (fAttr.HasFlag(FileAttributes.Directory))
                {
                    try
                    {
                        Directory.Delete(pathR + "\\" + lboxKönyvtárListRight.SelectedItem.ToString());
                        UpdateListBox(pathR, lboxKönyvtárListRight);
                        UpdateListBox(pathL, lboxKönyvtárListLeft);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Nem sikerült a mappa törlése! " + ex.Message, "Hiba", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
                else
                {
                    File.Delete(pathR + "\\" + lboxKönyvtárListRight.SelectedItem.ToString());
                    UpdateListBox(pathR, lboxKönyvtárListRight);
                    UpdateListBox(pathL, lboxKönyvtárListLeft);
                }

            }
            
        }

        //copy files
        private void btnToLeft_Click(object sender, RoutedEventArgs e)
        {
            if (lboxKönyvtárListRight.SelectedIndex > -1)
            {
                FileAttributes fileAttributes = File.GetAttributes(pathR + "\\" + lboxKönyvtárListRight.SelectedItem.ToString());
                if (fileAttributes.HasFlag(FileAttributes.Directory))
                {
                    try
                    {
                        DirectoryInfo source = new DirectoryInfo($"{pathR}\\{lboxKönyvtárListRight.SelectedItem}");
                        DirectoryInfo dest = new DirectoryInfo($"{pathL}\\{lboxKönyvtárListRight.SelectedItem}");
                        CopyFolder(source, dest);
                        UpdateListBox(pathL, lboxKönyvtárListLeft);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Nem sikerült a mappa másolása! " + ex.Message, "Hiba", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
                else
                {
                    CopyFile(lboxKönyvtárListRight.SelectedItem.ToString(), pathL, pathR);
                    UpdateListBox(pathL, lboxKönyvtárListLeft);
                }
            }
        }

        private void btnToRight_Click(object sender, RoutedEventArgs e)
        {
            if (lboxKönyvtárListLeft.SelectedIndex > -1)
            {
                FileAttributes fileAttributes = File.GetAttributes(pathL + "\\" + lboxKönyvtárListLeft.SelectedItem.ToString());
                if (fileAttributes.HasFlag(FileAttributes.Directory))
                {
                    try
                    {
                        DirectoryInfo source = new DirectoryInfo($"{pathL}\\{lboxKönyvtárListLeft.SelectedItem}");
                        DirectoryInfo dest = new DirectoryInfo($"{pathR}\\{lboxKönyvtárListLeft.SelectedItem}");
                        CopyFolder(source, dest);
                        UpdateListBox(pathR, lboxKönyvtárListRight);
                    }
                    catch (Exception)
                    {
                        MessageBox.Show("Nem sikerült a mappa másolása! ", "Hiba", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
                else
                {
                    CopyFile(lboxKönyvtárListLeft.SelectedItem.ToString(), pathR, pathL);
                    UpdateListBox(pathR, lboxKönyvtárListRight);
                }
            }
        }

        private void CopyFile(string name, string to, string destPath)
        {
            try
            {
                FileInfo fileInfo = new FileInfo($"{destPath}\\{name}");
                fileInfo.CopyTo($"{to}\\{name}");
            }
            catch (Exception)
            {
                MessageBox.Show("Nem sikerült a fájl másolása: Már létezik. ", "Hiba", MessageBoxButton.OK, MessageBoxImage.Error);
            }


        }
        private void CopyFolder(DirectoryInfo src, DirectoryInfo dest)
        {
            Directory.CreateDirectory(dest.FullName);
            foreach (var dir in src.GetDirectories())
            {
                CopyFolder(dir, dest.CreateSubdirectory(dir.Name));
            }
            foreach (var file in src.GetFiles())
            {
                file.CopyTo(Path.Combine(dest.FullName, file.Name));
            }

        }

        private void lboxKönyvtárListLeft_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (lboxKönyvtárListLeft.SelectedIndex > -1)
            {
                btnDelete.IsEnabled = true;
                lboxKönyvtárListRight.SelectedIndex = -1;
            }
        }

        private void lboxKönyvtárListRight_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (lboxKönyvtárListRight.SelectedIndex > -1)
            {
                btnDelete.IsEnabled = true;
                lboxKönyvtárListLeft.SelectedIndex = -1;
            }
        }
    }
}