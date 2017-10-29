using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using cntrl = System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Forms;
using Excel_SQLizer;
using Excel_SQLizer.Exceptions;

namespace Excel_SQLizer.WFP
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        //There is probably a better WPF pattern than this
        private bool _insertMode;
        private bool _updateMode;
        private bool _deleteMode;

        public MainWindow()
        {
            //default insertMode
            _insertMode = true;
            _updateMode = false;
            _deleteMode = false;
            InitializeComponent();
        }

        private void SelectFileClick(object sender, RoutedEventArgs e)
        {
            var fileDialog = new System.Windows.Forms.OpenFileDialog();
            fileDialog.Filter = "Excel Workbooks (*.xlsx)|*.xlsx";
            var result = fileDialog.ShowDialog();
            if (result == System.Windows.Forms.DialogResult.OK)
            {
                this.messages.Foreground = Brushes.Black;
                this.messages.Text = "Working...";

                OLD_SQLizer sqlizer = new OLD_SQLizer(fileDialog.FileName);
                try
                {
                    sqlizer.GenerateInsertScript();
                    this.messages.Foreground = Brushes.Green;
                    this.messages.Text = "Successfully created script(s) in " + System.IO.Path.GetDirectoryName(fileDialog.FileName);
                }
                catch (WorkbookOpenException)
                {
                    this.messages.Foreground = Brushes.Red;
                    this.messages.Text = "Error - workbook is opened by another process.";
                }
                catch (Exception exception)
                {
                    //update error message
                    this.messages.Foreground = Brushes.Red;
                    this.messages.Text = exception.Message;
                }
            }
            else
            {
                this.messages.Foreground = Brushes.Black;
                this.messages.Text = "";
            }
        }

        private void SelectModeClick(object sender, RoutedEventArgs e)
        {
            //Inspired by https://dotnetlearning.wordpress.com/2011/02/20/dropdown-menu-in-wpf/
            (sender as cntrl.Button).ContextMenu.IsEnabled       = true;
            (sender as cntrl.Button).ContextMenu.PlacementTarget = (sender as cntrl.Button);
            (sender as cntrl.Button).ContextMenu.Placement       = cntrl.Primitives.PlacementMode.Bottom;
            (sender as cntrl.Button).ContextMenu.IsOpen          = true;
        }

        private void InsertMode(object sender, RoutedEventArgs e)
        {
            _insertMode = true;
            _updateMode = false;
            _deleteMode = false;
        }

        private void UpdateMode(object sender, RoutedEventArgs e)
        {
            _insertMode = false;
            _updateMode = true;
            _deleteMode = false;
        }

        private void DeleteMode(object sender, RoutedEventArgs e)
        {
            _insertMode = false;
            _updateMode = false;
            _deleteMode = true;
        }
    }
}
