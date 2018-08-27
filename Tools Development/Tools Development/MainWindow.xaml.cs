using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

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

        private void SetTextBox(List<Dictionary<string, string>> jsonSchema)
        {
            int startLocation = -600;
            int location = 0;
            int textboxLocation = 0;

            int teams = jsonSchema.Count;
            TextBox[] txtTeamNames = new TextBox[teams];
            string text = "";
            for (int i = 0; i < txtTeamNames.Length; i++)
            {
                var jsonValue = jsonSchema[i].Values;
                foreach (var item in jsonValue)
                {
                    int start = item.IndexOf('[');
                    int end = item.LastIndexOf(']');
                    text = item.Substring(start + 1, end - start - 1);
                }
                var txt = new TextBox();
                txtTeamNames[i] = txt;
                txtTeamNames[i].HorizontalAlignment = HorizontalAlignment.Left;
                txtTeamNames[i].Width = Convert.ToInt32(text) * 10;
                txtTeamNames[i].Height = 50;
                txtTeamNames[i].Margin = new Thickness(200, textboxLocation + startLocation, 0, 0);
                textboxLocation += 38;
                showGrid.Children.Add(txtTeamNames[i]);
            }

            foreach (var listDictionary in jsonSchema)
            {
                foreach (string key in listDictionary.Keys)
                {
                    var value = listDictionary[key];
                    Label dynamicLabel = new Label();

                    dynamicLabel.Name = key;
                    dynamicLabel.Content = key;
                    dynamicLabel.Width = 240;
                    dynamicLabel.Height = 30;
                    dynamicLabel.Margin = new Thickness(0, location + startLocation, 0, 0);
                    //dynamicLabel.Foreground = new SolidColorBrush(Colors.White);
                    //dynamicLabel.Background = new SolidColorBrush(Colors.Black);
                    location += 38;
                    showGrid.Children.Add(dynamicLabel);
                }
            }
        }


        private List<Dictionary<string, string>> GetJson(string position)
        {
            List<Dictionary<string, string>> jsonResult = new List<Dictionary<string, string>>();
            //   string position = @"D:\Visual Studio\ToolsDevelopmentNew\Tools Development\Tools Development\assets\json\schema.json";
            if (System.IO.File.Exists(position))
            {
                using (StreamReader r = new StreamReader(@position))
                {
                    var json = r.ReadToEnd();
                    dynamic stuff = JsonConvert.DeserializeObject(json);
                    Dictionary<string, object> values = new Dictionary<string, object>();
                    foreach (var item in stuff.schema)
                    {
                        Object temp = item;
                        var jsonSerializer = JsonConvert.SerializeObject(temp);
                        values = JsonConvert.DeserializeObject<Dictionary<string, object>>(jsonSerializer);
                        jsonResult.Add(JsonConvert.DeserializeObject<Dictionary<string, string>>(jsonSerializer));
                    }
                }
            }
            return jsonResult;
        }

        private void Check(object sender, RoutedEventArgs e)
        {
            //C:\Users\allen\Documents\ToolsDevelopment\Tools Development\Tools Development\assets\json\schema.json
            //string position = textBox.Text;
            //string position = @"C:\Users\allen\Documents\ToolsDevelopment\Tools Development\Tools Development\assets\json\schema.json";
            string position = @"D:\Visual Studio\ToolsDevelopmentNew\Tools Development\Tools Development\assets\json\schema.json";
            if (System.IO.File.Exists(position))
            {
                using (StreamReader r = new StreamReader(@position))
                {
                    List<Dictionary<string, string>> jsonResult = GetJson(position);
                    SetTextBox(jsonResult);
                }
            }
            else
            {
                MessageBox.Show(" 檔案不存在");
            }

        }

    }
}
