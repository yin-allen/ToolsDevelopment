﻿using Newtonsoft.Json;
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
            string limit = "";


            foreach (var listDictionary in jsonSchema)
            {
                foreach (string key in listDictionary.Keys)
                {
                    var value = listDictionary[key];
                    Label dynamicLabel = new Label();
                    int start = listDictionary[key].IndexOf('[');
                    int end = listDictionary[key].LastIndexOf(']');
                    limit = listDictionary[key].Substring(start + 1, end - start - 1);

                    dynamicLabel.Name = key;
                    dynamicLabel.Content = key + "(請輸入" + limit + "碼)";
                    dynamicLabel.Width = 240;
                    dynamicLabel.Height = 30;
                    dynamicLabel.Margin = new Thickness(0, location + startLocation, 0, 0);
                    location += 38;

                    var dynamicTxt = new TextBox();
                    dynamicTxt.HorizontalAlignment = HorizontalAlignment.Left;
                    dynamicTxt.Width = 240;
                    dynamicTxt.Height = 30;
                    dynamicTxt.Name = key;
                    dynamicTxt.MaxLength = int.Parse(limit);
                    dynamicTxt.Margin = new Thickness(200, textboxLocation + startLocation, 0, 10);
                    textboxLocation += 38;


                    showGrid.Children.Add(dynamicLabel);
                    showGrid.Children.Add(dynamicTxt);
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
            string position = @"C:\Users\allen\Documents\ToolsDevelopment\Tools Development\Tools Development\assets\json\schema.json";
            // string position = @"D:\Visual Studio\ToolsDevelopmentNew\Tools Development\Tools Development\assets\json\schema.json";
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
