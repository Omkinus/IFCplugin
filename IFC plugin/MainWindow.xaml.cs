using Microsoft.Win32;
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
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace IFC_plugin
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            this.MouseDown += delegate { DragMove(); };
        }


        string filepath = ""; // на самом деле эта переменная означает имя файла а не путь
        string outputfolder = "";
        string filename = "";
        
        //кнопка STARTCREATION
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (filepath == "")
            {
                System.Windows.MessageBox.Show("Input file not selected");
                return;
            }
            if (outputfolder == "")
            {
                System.Windows.MessageBox.Show("The path to the folder is not specified");
                return;
            }

                System.IO.StreamReader streamReader;
                streamReader = new System.IO.StreamReader(filepath);
                string alltext = streamReader.ReadToEnd();
                string[] alltextarray = alltext.Split('\n');

                for (int i = 0; i < alltextarray.Length; i++)
                {
                    if (alltextarray[i].Contains("IFCELEMENTASSEMBLY"))
                    {
                        string[] itemsplitted = alltextarray[i].Split(',');
                        itemsplitted[2] = (itemsplitted[7] + "-" + itemsplitted[2]).Replace('"', ' ').Replace("'-'", "-");
                        string newstring = string.Join(",", itemsplitted);
                        alltextarray[i] = newstring;
                    }
                }

                string alltextformatted = string.Join("\n", alltextarray);
                if (outputfilenametextbox.Text == "" || outputfilenametextbox.Text == " ")
                {
                    StreamWriter sw = new StreamWriter(outputfolder + "\\" + filename + "Modified_by_IFCPlugin.ifc");
                    sw.Write(alltextformatted);
                    sw.Close();
                }
                else
                {
                    StreamWriter sw = new StreamWriter(outputfolder + "\\" + outputfilenametextbox.Text + ".ifc");
                    sw.Write(alltextformatted);
                    sw.Close();
                }
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        // CLOSE BUTTON
        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            Close();
        }

        //кнопка INPUTFILE
        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            System.Windows.Forms.OpenFileDialog openFileDialog = new System.Windows.Forms.OpenFileDialog();
            openFileDialog.Filter = "IFC files (*.ifc)|*.ifc|All files (*.*)|*.*";
            openFileDialog.ShowDialog();
            filepath = openFileDialog.FileName;
            filename = System.IO.Path.GetFileNameWithoutExtension(openFileDialog.FileName);

        }

        //кнопка OUTPUT FOLDER
        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog();
            folderBrowserDialog.ShowDialog();
            outputfolder = folderBrowserDialog.SelectedPath;
        }
    }
}
