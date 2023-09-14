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
            DirectoryInfo directoryInfo = Directory.CreateDirectory($"{path}\\{tbMappa.Text}");
            MessageBox.Show("Mappa létrehozva", "Info", MessageBoxButton.OK, MessageBoxImage.Information);
            UpdateListBox();
        }
    }
}
