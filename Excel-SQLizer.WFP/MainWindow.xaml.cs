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
using Excel_SQLizer.SQLizers;

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
        private bool _insertOrUpdateMode;

        public MainWindow()
        {
            //default insertOrUpdateMode
            _insertOrUpdateMode = true;
            _insertMode         = false;
            _updateMode         = false;
            _deleteMode         = false;
            InitializeComponent();
        }

        private void SelectFileClick(object sender, RoutedEventArgs e)
        {
            var fileDialog = new System.Windows.Forms.OpenFileDialog
            {
                Filter = "Excel Workbooks (*.xlsx)|*.xlsx"
            };
            var result = fileDialog.ShowDialog();
            if (result == System.Windows.Forms.DialogResult.OK)
            {
                this.messages.Foreground = Brushes.Black;
                this.messages.Text = "Working...";


                BaseSQLizer sqlizer = CreateSQLizer(fileDialog.FileName);
                try
                {
                    sqlizer.GenerateSQLScripts();
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

        private BaseSQLizer CreateSQLizer(string fileName)
        {
            BaseSQLizer sqlizer = null;
            if (_insertMode)
            {
                sqlizer = SQLizerFactory.Create(SQLizerType.Insert, fileName);
            }
            else if (_updateMode)
            {
                sqlizer = SQLizerFactory.Create(SQLizerType.Update, fileName);
            }
            else if (_deleteMode)
            {
                sqlizer = SQLizerFactory.Create(SQLizerType.Delete, fileName);
            }
            else if (_insertOrUpdateMode)
            {
                sqlizer = SQLizerFactory.Create(SQLizerType.InsertOrUpdate, fileName);
            }

            return sqlizer;
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
            _insertMode          = true;
            _updateMode          = false;
            _deleteMode          = false;
            _insertOrUpdateMode  = false;
            this.modeTxt.Content = "Insert Mode";
        }

        private void UpdateMode(object sender, RoutedEventArgs e)
        {
            _insertMode          = false;
            _updateMode          = true;
            _deleteMode          = false;
            _insertOrUpdateMode  = false;
            this.modeTxt.Content = "Update mode";
        }

        private void DeleteMode(object sender, RoutedEventArgs e)
        {
            _insertMode          = false;
            _updateMode          = false;
            _deleteMode          = true;
            _insertOrUpdateMode  = false;
            this.modeTxt.Content = "Delete mode";
        }

        private void InsertOrUpdateMode(object send, RoutedEventArgs e)
        {
            _insertMode         = false;
            _updateMode         = false;
            _deleteMode         = false;
            _insertOrUpdateMode = true;
            this.modeTxt.Content = "Insert or Update mode";
        }
    }
}
