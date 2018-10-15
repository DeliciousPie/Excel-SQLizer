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
using System.IO;

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

        private string _fileName;

        public bool OneScript { get; set; }

        public MainWindow()
        {
            //default insertOrUpdateMode
            _insertOrUpdateMode = true;
            _insertMode         = false;
            _updateMode         = false;
            _deleteMode         = false;
            OneScript           = true;

            InitializeComponent();
            // After component is initialized, set up checkbox data context
            oneScriptCheckBox.DataContext = this;
        }

        private void SelectFileClick(object sender, RoutedEventArgs e)
        {
            var fileDialog = new System.Windows.Forms.OpenFileDialog
            {
                Filter = "All (*.xlsx;*.xls;*csv)|*.xlsx;*.xls;*csv|Excel Workbooks (*.xlsx;*.xls)|*.xlsx;*.xls|CSV (*.csv)|*.csv"
            };
            var result = fileDialog.ShowDialog();
            if (result == System.Windows.Forms.DialogResult.OK)
            {
                this.messages.Foreground = Brushes.Black;
                this.messages.Text = "Working...";

                // Save file name
                _fileName = fileDialog.SafeFileName;

                // Determine the selected file type - includes the '.'
                string extension = System.IO.Path.GetExtension(fileDialog.FileName);
                // Open the selected file as a file stream
                using (FileStream fileStream = File.Open(fileDialog.FileName, FileMode.Open, FileAccess.Read))
                {

                    MemoryStream memStream = new MemoryStream();
                    // Copy to a memory stream (the format SQLizer works with)
                    fileStream.CopyTo(memStream);
                    ISQLizer sqlizer = CreateSQLizer(fileDialog.FileName, extension, memStream);
                    try
                    {
                        Dictionary<string, List<string>> sqlResults = sqlizer.GetSQLStatements();
                        string outPath = System.IO.Path.GetDirectoryName(fileDialog.FileName);
                        WriteSqlScripts(sqlResults, outPath);
                        this.messages.Foreground = Brushes.Green;
                        this.messages.Text = "Successfully created script(s) in " + System.IO.Path.GetDirectoryName(fileDialog.FileName);
                        memStream.Dispose();
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
            }
            else
            {
                this.messages.Foreground = Brushes.Black;
                this.messages.Text = "";
            }
        }

        private void WriteSqlScripts(Dictionary<string, List<string>> sqlResults, string outPath)
        {
            // TODO: Refactor to be more DRY
            if (OneScript)
            {
                // Remove spaces from file name
                string name     = _fileName.Replace(' ', '_');
                // Find the index of the extension so we can strip the extension
                int extIdx      = name.LastIndexOf('.');
                name            = name.Substring(0, extIdx);
                string filePath = $"{outPath}\\{GetScriptName(name)}";
                if (File.Exists(filePath))
                {
                    File.Delete(filePath);
                }
                using (StreamWriter sw = File.CreateText(filePath))
                {
                    foreach (KeyValuePair<string, List<string>> kv in sqlResults)
                    {
                        // Each item in the list is a new line of SQL to write
                        foreach (string statement in kv.Value)
                        {
                            sw.WriteLine(statement);
                        }
                    }
                }
            }
            else
            {
                foreach (KeyValuePair<string, List<string>> kv in sqlResults)
                {
                    string filePath = $"{outPath}\\{GetScriptName(kv.Key)}";
                    // If the file already exists, delete it
                    if (File.Exists(filePath))
                    {
                        File.Delete(filePath);
                    }
                    // Create the file to write too
                    using (StreamWriter sw = File.CreateText(filePath))
                    {
                        // Each item in the list is a new line of SQL to write
                        foreach (string statement in kv.Value)
                        {
                            sw.WriteLine(statement);
                        }
                    }
                }
            }
        }

        private ISQLizer CreateSQLizer(string filePath, string extension, MemoryStream memStream)
        {
            FileType fileType = extension.ToLower() == ".csv"
                            ? FileType.CSV
                            : FileType.Excel;
            string fileName = System.IO.Path.GetFileNameWithoutExtension(filePath);

            ISQLizer sqlizer = null;
            if (_insertOrUpdateMode)
            {
                sqlizer = SQLizerFactory.Create(SQLizerType.InsertOrUpdate, fileType, memStream, fileName);
            }
            else if (_insertMode)
            {
                sqlizer = SQLizerFactory.Create(SQLizerType.Insert, fileType, memStream, fileName);
            }
            else if (_updateMode)
            {
                sqlizer = SQLizerFactory.Create(SQLizerType.Update, fileType, memStream, fileName);
            }
            else if (_deleteMode)
            {
                sqlizer = SQLizerFactory.Create(SQLizerType.Delete, fileType, memStream, fileName);
            }

            return sqlizer;
        }

        /// <summary>
        /// Gets the name of the script, based on the table name and type of SQL generated.
        /// </summary>
        /// <param name="tableName">Name of the table.</param>
        /// <returns></returns>
        private string GetScriptName(string tableName)
        {
            string result = tableName.ToUpper();
            if (_insertOrUpdateMode)
            {
                result += "_INSERT_OR_UPDATE_STATEMENTS.sql";
            }
            else if (_updateMode)
            {
                result += "_UPDATE_STATEMENTS.sql";
            }
            else if (_deleteMode)
            {
                result += "_DELETE_STATEMENTS.sql";
            }
            else if (_insertMode)
            {
                result += "_INSERT_STATEMENTS.sql";
            }

            return result;
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
