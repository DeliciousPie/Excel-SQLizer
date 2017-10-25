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
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var fileDialog = new System.Windows.Forms.OpenFileDialog();
            fileDialog.Filter = "Excel Workbooks (*.xlsx)|*.xlsx";
            var result = fileDialog.ShowDialog();
            if (result == System.Windows.Forms.DialogResult.OK)
            {
                this.messages.Foreground = Brushes.Black;
                this.messages.Text = "Working...";

                SQLizer sqlizer = new SQLizer(fileDialog.FileName);
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
    }
}
