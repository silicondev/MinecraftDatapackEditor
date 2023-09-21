using Microsoft.WindowsAPICodePack.Dialogs;
using MinecraftDatapackEditor.Data;
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

namespace MinecraftDatapackEditor
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public string DatapackDirectory
        {
            get => DatapackFolderTxt.Text ?? "";
            set => DatapackFolderTxt.Text = value;
        }
        public bool ValidDatapack { get; private set; }

        public DataPack? Datapack { get; private set; } = null;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void DatapackFolderSelectBtn_Click(object sender, RoutedEventArgs e)
        {
            using (var fd = new CommonOpenFileDialog())
            {
                fd.IsFolderPicker = true;
                fd.InitialDirectory = "C:\\";
                if (fd.ShowDialog() == CommonFileDialogResult.Ok)
                    DatapackDirectory = fd.FileName;
            }
        }

        private void LoadBtn_Click(object sender, RoutedEventArgs e)
        {
            Datapack = null;
            if (!File.Exists(Path.Combine(DatapackDirectory, "pack.mcmeta")))
            {
                if (MessageBox.Show("No Datapack at your selected folder exists! Would you like to create one?", "Create New?", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                    Datapack = DataPack.CreateNew(DatapackDirectory);
            }
            else
            {
                Datapack = new DataPack(DatapackDirectory);
                Datapack.Load();
            }

            ValidDatapack = Datapack != null && !string.IsNullOrEmpty(Datapack.Name) && Datapack.Pack != null;
            RefreshView();
        }

        private void RefreshView()
        {
            if (!ValidDatapack)
                return;

            foreach (var ns in Datapack.Namespaces)
            {
                var tvNs = new TreeViewItem()
                {
                    Header = ns.Name
                };

                var tvDimH = new TreeViewItem()
                {
                    Header = "Dimensions"
                };

                foreach (var dim in ns.Dimensions)
                {
                    var tvDim = new TreeViewItem()
                    {
                        Header = dim.Name
                    };

                    tvDimH.Items.Add(tvDim);
                }

                tvNs.Items.Add(tvDimH);

                FileExplorerView.Items.Add(tvNs);

            }
        }
    }
}
