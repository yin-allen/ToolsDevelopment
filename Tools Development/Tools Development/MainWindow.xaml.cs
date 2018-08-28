using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace Tools_Development
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private List<Dictionary<string, string>> jsonResult;

        public MainWindow()
        {
            InitializeComponent();

            Button checkButton = new Button();
            checkButton.Name = "checkButton";
            checkButton.Click += Check;

            SetSaveButton();
        }

        private void SetSaveButton()
        {
            Button saveButton = new Button();
            saveButton.Name = "showButton";
            saveButton.Click += Save;
        }

        private void SetTextBox(List<Dictionary<string, string>> jsonSchema)
        {

            int teams = jsonSchema.Count;
            string limit = "";

            foreach (var listDictionary in jsonSchema)
            {
                foreach (string key in listDictionary.Keys)
                {
                    var value = listDictionary[key];
                    var stackPanel = new StackPanel()
                    {
                        Orientation = Orientation.Horizontal
                    };

                    Label dynamicLabel = new Label();
                    int start = listDictionary[key].IndexOf('[');
                    int end = listDictionary[key].LastIndexOf(']');
                    limit = listDictionary[key].Substring(start + 1, end - start - 1);

                    dynamicLabel.Name = key;
                    dynamicLabel.Content = key + "(請輸入" + limit + "碼)";
                    dynamicLabel.Width = 240;
                    dynamicLabel.Height = 30;

                    var dynamicTxt = new TextBox();
                    dynamicTxt.HorizontalAlignment = HorizontalAlignment.Left;
                    dynamicTxt.Width = 240;
                    dynamicTxt.Height = 30;
                    dynamicTxt.Name = key;
                    dynamicTxt.MaxLength = int.Parse(limit);
                    dynamicTxt.PreviewTextInput += TextBoxPreviewTextInput;

                    stackPanel.Children.Add(dynamicLabel);
                    stackPanel.Children.Add(dynamicTxt);
                    listView.Items.Add(stackPanel);
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
            //  string position = @"D:\Visual Studio\ToolsDevelopmentNew\Tools Development\Tools Development\assets\json\schema.json";
            if (System.IO.File.Exists(position))
            {
                using (StreamReader r = new StreamReader(@position))
                {
                    jsonResult = GetJson(position);
                    SetTextBox(jsonResult);
                }
            }
            else
            {
                MessageBox.Show(" 檔案不存在");
            }

        }

        private void Save(object sender, RoutedEventArgs e)
        {
            List<string> labelContents = new List<string>();
            List<string> textboxTexts = new List<string>();
            Dictionary<string, string> allContents = new Dictionary<string, string>();

            if (listView.Items.Count > 0)
            {
                foreach (var item in listView.Items)
                {
                    if (item is Label)
                    {
                        labelContents.Add((item as Label).Content.ToString());
                    }
                    else if (item is TextBox)
                    {
                        textboxTexts.Add((item as TextBox).Text);
                    }
                }
                for (int i = 0; i < labelContents.Count; i++)
                {
                    allContents.Add(labelContents[i], textboxTexts[i]);
                }
            }
            Console.WriteLine(allContents);
        }

        private void TextBoxPreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !IsValid(((TextBox)sender).Name, ((TextBox)sender).Text + e.Text);
        }

        public bool IsValid(string name, string str)
        {
            string dataType = "";
            foreach (var dictionaryItem in jsonResult)
            {
                foreach (var item in dictionaryItem)
                {
                    if (item.Key.Equals(name))
                        dataType = item.Value;
                }
            }
            return true;
        }

        private bool IsUshort(string value)
        {
            ushort dateValue;
            if (ushort.TryParse(value, out dateValue))
                return true;
            else
                return false;
        }

        private bool IsUlong(string value)
        {
            ulong dateValue;
            if (ulong.TryParse(value, out dateValue))
                return true;
            else
                return false;
        }

        private bool IsLong(string value)
        {
            long dateValue;
            if (long.TryParse(value, out dateValue))
                return true;
            else
                return false;
        }

        private bool IsShort(string value)
        {
            short dateValue;
            if (short.TryParse(value, out dateValue))
                return true;
            else
                return false;
        }

        private bool IsSbyte(string value)
        {
            sbyte dateValue;
            if (sbyte.TryParse(value, out dateValue))
                return true;
            else
                return false;
        }

        private bool IsInt(string value)
        {
            int dateValue;
            if (int.TryParse(value, out dateValue))
                return true;
            else
                return false;
        }

        private bool IsUint(string value)
        {
            uint dateValue;
            if (uint.TryParse(value, out dateValue))
                return true;
            else
                return false;
        }
    }
}
