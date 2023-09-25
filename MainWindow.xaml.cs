using Microsoft.WindowsAPICodePack.Dialogs;
using MinecraftDatapackEditor.Data;
using MinecraftDatapackEditor.Data.Dimensions.Generation;
using MinecraftDatapackEditor.Interfaces;
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

        private async void LoadBtn_Click(object sender, RoutedEventArgs e)
        {
            LoadBtn.Content = "Loading...";
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
            LoadBtn.Content = "LOAD";
        }

        private void RefreshView()
        {
            if (!ValidDatapack)
                return;

            FileExplorerView.Items.Clear();

            foreach (var ns in Datapack.Namespaces)
            {
                FileExplorerView.Render(ns);
            }
        }

        private void FileExplorerView_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            try
            {
                if (e.NewValue == null)
                    return;

                var tvi = (TreeViewItem)e.NewValue;

                ValueTxt.Text = tvi.Tag.ToString();
            }
            catch (Exception) { }
        }

        private void DeleteSelectionBtn_Click(object sender, RoutedEventArgs e)
        {
            var tvi = (TreeViewItem)FileExplorerView.SelectedItem;
            var obj = tvi.Tag;

            if (obj is DistinctedList<IRenderDistinctable<string>, string>)
            {
                var list = (DistinctedList<IRenderDistinctable<string>, string>)obj;
                var t = list.OriginType;

                var others = new List<IRenderDistinctable<string>>(list.Origin);
                others.RemoveWhere(x => list.Any(y => list.KeySelector.Invoke(x) == list.KeySelector.Invoke(y)));

                MessageBox.Show($"Distincted List. {list.Count()}/{list.Origin.Count()} (subset of {others.Count()}) of type {t.Name}");

                list.Origin.RemoveWhere(x => list.Any(y => list.KeySelector.Invoke(x) == list.KeySelector.Invoke(y)));
                RefreshView();
            }
            else
                MessageBox.Show(obj.ToString());
        }
    }
}
