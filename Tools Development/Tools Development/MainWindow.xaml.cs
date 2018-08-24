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

namespace Tools_Development
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            Button checkButton = new Button();
            checkButton.Name = "checkButton";
            checkButton.Click += Check;

        }

        private void Check(object sender, RoutedEventArgs e)
        {
            JObject o1 = JObject.Parse(File.ReadAllText(@"c:\videogames.json"));
        }
    }
}
