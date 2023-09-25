using Microsoft.WindowsAPICodePack.Dialogs;
using MinecraftDatapackEditor.Data;
using MinecraftDatapackEditor.Data.Dimensions.Generation;
using MinecraftDatapackEditor.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;
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
            InfoDataGrid.Visibility = Visibility.Hidden;
            SidebarGrid.Visibility = Visibility.Hidden;
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
            if (ValidDatapack)
            {
                RefreshView();
                SidebarGrid.Visibility = Visibility.Visible;
                InfoDataGrid.Visibility = Visibility.Hidden;
            }
            
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

        private async void FileExplorerView_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            var newVal = e.NewValue;
            DataView? view = null;
            object? obj = null;

            try
            {
                if (newVal == null)
                    return;

                var tvi = (TreeViewItem)newVal;
                obj = tvi.Tag;


            }
            catch (Exception)
            {
                return;
            }

            var type = obj.GetType();

            if (type.IsEnumerable() && (type.GetArrayType()?.Inherits<Tableable>() ?? false))
            {
                var elType = type.GetArrayType();
                var arr = obj.GetArray();
                ValueTxt.Text = "Loading...";

                await Task.Run(() =>
                {
                    DataTable? tb = null;
                    foreach (Tableable item in arr)
                    {
                        tb = item.GetRow(tb);
                    }

                    //var newView = new DataView(tb);

                    //tb = newView.ToTable(true, "Biome Id");

                    view = tb.DefaultView;
                    //view = new DataView(tb);
                });

                ValueTxt.Text = $"{elType.Name}[{arr.Length}]";
            }

            if (view != null)
            {
                InfoDataGrid.DataContext = view;
                InfoDataGrid.Visibility = Visibility.Visible;
            }
            else if (obj != null)
            {
                ValueTxt.Text = obj.ToString();
                InfoDataGrid.Visibility = Visibility.Hidden;
            }
            
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

        private void InfoDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var row = (DataRowView)InfoDataGrid.SelectedItem;
            ValueTxt.Text = row["Biome Id"].ToString();
        }
    }
}
