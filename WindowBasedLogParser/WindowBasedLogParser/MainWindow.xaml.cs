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
using System.IO;


namespace WindowBasedLogParser
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

        /// <summary>
        /// This will Browse the Input File 
        /// eg : *.log file
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BrowseInputFileClicked(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog fileDialog = GetInputFileDialog();

            Nullable<bool> result = fileDialog.ShowDialog();

            if (result == true)
            {
                this.inputFileTextBox.Text = fileDialog.FileName;
            }
        }

        /// <summary>
        /// This will Browse for Output file.
        /// eg. *.csv file
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BrowseOutputFileClicked(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog fileDialog = GetOutputFileDialog();

            Nullable<bool> result = fileDialog.ShowDialog();

            if (result == true)
            {
                this.outputFileTextBox.Text = fileDialog.FileName;
            }
        }


        /// <summary>
        /// This will start parsing the file.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OkButtonClicked(object sender, RoutedEventArgs e)
        {
            if (CheckInputFilePath())
            {
                if (CheckOutputFilePath())
                {
                    new Parser(this.inputFileTextBox.Text);

                    try
                    {
                        this.WriteStatistics();

                        this.DoYouWantToContinue();

                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Exception while printing Stats : " + ex.Message);
                    }
                }
            }

        }

        /// <summary>
        /// This will display Parsing completed information and will ask the user 
        /// whether s/he want to continue using the application or not?
        /// </summary>
        private void DoYouWantToContinue()
        {
            MessageBoxResult result = MessageBox.Show("Parsing Completed.\nDo You Want Continue?", "", MessageBoxButton.YesNo);

            if (result == MessageBoxResult.No)
            {
                Application.Current.Shutdown();
            }
        }

        /// <summary>
        /// This will write parsed result(statistics) into an output file.
        /// </summary>
        private void WriteStatistics()
        {
            //Get output file.
            System.IO.StreamWriter resultFile = new System.IO.StreamWriter(this.outputFileTextBox.Text);

            string line = "RequestCount\tMethodType\tStatusCount200\tStatusCount500\tMaximum Request Time\tMinimum Request Time\tRequest Name\n";

            resultFile.WriteLine(line);

            foreach (KeyValuePair<string, RequestInformation> requestInfo in Parser.RequestDictionary)
            {
                RequestInformation requestinformation = requestInfo.Value;

                line = requestinformation.RequestCount + "\t"
                                + requestinformation.MethodType + "\t"
                                + requestinformation.StatusCount200 + "\t"
                                + requestinformation.StatusCount500 + "\t"
                                + ((!requestinformation.MaxRequestTime.Equals(null) && !requestinformation.MaxRequestTime.Equals("")) ? (requestinformation.MaxRequestTime) : "null") + "\t"
                                + ((!requestinformation.MinRequestTime.Equals(null) && !requestinformation.MinRequestTime.Equals("")) ? (requestinformation.MinRequestTime) : "null") + "\t"
                                + requestInfo.Key + "\n"
                              ;
                resultFile.WriteLine(line);
            }
            resultFile.Close();
        }

        /// <summary>
        /// This will return File Dialog Object with below Properties set.
        /// 1 : Title  : title to be displayed on file dialog box.
        /// 2 : Filter : Files to be filtered. 
        /// </summary>
        /// <returns></returns>
        private Microsoft.Win32.OpenFileDialog GetInputFileDialog()
        {
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();

            dlg.Title = "Select a Log file.";
            dlg.Filter = "Log Files(*.log)|*.log";

            return dlg;
        }

        /// <summary>
        /// This will return File Dialog Object with below Properties set.
        /// 1 : Title  : title to be displayed on file dialog box.
        /// 2 : Filter : Files to be filtered. 
        /// </summary>
        /// <returns></returns>
        private Microsoft.Win32.OpenFileDialog GetOutputFileDialog()
        {
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();

            dlg.Title = "Select an Output file.";
            dlg.Filter = "Csv Files(*.csv)|*.csv";

            return dlg;
        }

        /// <summary>
        /// This will check Input File Path and returns true if exists.
        /// If User manually entered the File name i.e. file already not exists then check
        /// whether the directory is present or not.
        /// </summary>
        /// <returns></returns>
        private bool CheckOutputFilePath()
        {
            if (String.IsNullOrEmpty(this.outputFileTextBox.Text))
            {
                this.DisplayInformation("No output file entered.\nPlease enter valid file name");
                return false;
            }
            if (File.Exists(this.outputFileTextBox.Text))
            {
                return true;
            }
            else
            {
                if ((this.outputFileTextBox.Text).Contains('\\'))
                {
                    string directoryName = (this.outputFileTextBox.Text).Substring(0, ((this.outputFileTextBox.Text).LastIndexOf('\\')));
                    string fileName = (this.outputFileTextBox.Text).Substring(((this.outputFileTextBox.Text).LastIndexOf('\\')));

                    if (Directory.Exists(directoryName) && fileName.Contains(".csv"))
                        return true;
                }
            }

            this.DisplayInformation("Invalid output file.\nPlease enter valid file name");
            return false;
        }

        /// <summary>
        /// This will check Output File path and returms true if exists.
        /// </summary>
        /// <returns></returns>
        private bool CheckInputFilePath()
        {
            if (String.IsNullOrEmpty(this.inputFileTextBox.Text))
            {
                this.DisplayInformation("No Input file entered.\nPlease enter valid file name");
                return false;
            }

            if (File.Exists(this.inputFileTextBox.Text))
            {
                return true;
            }
            else
            {
                this.DisplayInformation("Invalid input file.\nPlease enter valid file name");
            }
            return false;
        }

        /// <summary>
        /// This will display a useful Information to user.
        /// </summary>
        /// <param name="information"></param>
        private void DisplayInformation(string information)
        {
            MessageBox.Show(information, "", MessageBoxButton.OK, MessageBoxImage.Warning);
        }
    }
}
