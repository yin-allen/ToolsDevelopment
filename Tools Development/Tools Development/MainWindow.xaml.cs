using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
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
            List<Dictionary<string,string>> jsonResult = GetJson();
            SetTextBox(jsonResult);
        }

        private void SetTextBox(List<Dictionary<string,string>> jsonSchema)
        {
            int teams = jsonSchema.Count;
            TextBox[] txtTeamNames = new TextBox[teams];
            string value = "";
            for (int i = 0; i < txtTeamNames.Length; i++)
            {
                var jsonValue = jsonSchema[i].Values;
                foreach (var item in jsonValue)
                {
                    int start = item.IndexOf('[');
                    int end = item.LastIndexOf(']');
                    value = item.Substring(start + 1, end - start - 1);
                }
                var txt = new TextBox();
                txtTeamNames[i] = txt;
                txt.Width = Convert.ToInt32(value) * 10;
                txt.Height = 50;
                txt.Margin = new Thickness(-250 + i /5 * 35, 0 + 50 * (i % 5), 0, 0);
                txt.BorderThickness = new Thickness(5, 5, 5, 5);
                toolsLine.Children.Add(txt);
            }
        }


        private List<Dictionary<string, string>> GetJson()
        {
            List<Dictionary<string, string>> jsonResult = new List<Dictionary<string, string>>();
            string position = @"D:\Visual Studio\ToolsDevelopmentNew\Tools Development\Tools Development\assets\json\schema.json";
            if (System.IO.File.Exists(position))
            {
                using (StreamReader r = new StreamReader(@position))
                {
                    var json = r.ReadToEnd();
                    dynamic stuff = JsonConvert.DeserializeObject(json);
                    foreach (var item in stuff.schema)
                    {
                        Object temp = item;
                        var jsonSerializer = JsonConvert.SerializeObject(temp);
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
            string position = @"D:\Visual Studio\ToolsDevelopmentNew\Tools Development\Tools Development\assets\json\schema.json";
            if (System.IO.File.Exists(position))
            {
                using (StreamReader r = new StreamReader(@position))
                {
                    var json = r.ReadToEnd();
                    dynamic stuff = JsonConvert.DeserializeObject(json);
                    int i = 5;
                }
            }
            else
            {
                MessageBox.Show(" 檔案不存在");
            }
           
        }

    }
}
