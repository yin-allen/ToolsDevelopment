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


        private string GetDataType(string name)
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
            return dataType;
        }

        private bool BoolValid(string data)
        {
            bool result = false;
            if (data.Equals("true") || data.Equals("false"))
                result = true;
            return result;
        }

        private int GetLimit(string dataType)
        {
            string limit = "";
            int start = dataType.IndexOf('[');
            int end = dataType.LastIndexOf(']');
            limit = dataType.Substring(start + 1, end - start - 1);
            int length = int.Parse(limit);
            return length;
        }

        private int GetTextLength(string data)
        {
            string dataType = GetDataType(data);
            int charLength = GetLimit(dataType);
            return charLength;
        }

        private int ByteLength(string name)
        {
            int charLength = GetTextLength(name);
            int bit = 8 * charLength;
            double numberRange = Math.Pow(2, bit);
            string strNumber = Convert.ToString(numberRange);
            return strNumber.Length;
        }

        private bool IsByte(string value)
        {
            byte dateValue;
            if (byte.TryParse(value, out dateValue))
                return true;
            else
                return false;
        }

        private bool IsDecimal(string value)
        {
            decimal dateValue;
            if (decimal.TryParse(value, out dateValue))
                return true;
            else
                return false;
        }

        private bool IsDouble(string value)
        {
            double dateValue;
            if (double.TryParse(value, out dateValue))
                return true;
            else
                return false;
        }

        private bool IsFloat(string value)
        {
            float dateValue;
            if (float.TryParse(value, out dateValue))
                return true;
            else
                return false;
        }

        private bool CharValid(string name, string str)
        {
            int charLength = GetTextLength(name);
            if (str.Length <= charLength)
                return true;
            else
                return false;
        }

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
            int limit = 0;

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
                    limit = GetLimit(listDictionary[key]);

                    dynamicLabel.Name = key;
                    dynamicLabel.Content = key + "(請輸入" + limit + "碼)";
                    dynamicLabel.Width = 240;
                    dynamicLabel.Height = 30;

                    var dynamicTxt = new TextBox();
                    dynamicTxt.HorizontalAlignment = HorizontalAlignment.Left;
                    dynamicTxt.Width = 240;
                    dynamicTxt.Height = 30;
                    dynamicTxt.Name = key;

                    string dataType = GetDataType(key);
                    if (GetDataType(key).Contains("byte"))
                        dynamicTxt.MaxLength = ByteLength(key);
                    else
                        dynamicTxt.MaxLength = limit;

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
            //string position = @"C:\Users\allen\Documents\ToolsDevelopment\Tools Development\Tools Development\assets\json\schema.json";
              string position = @"D:\Visual Studio\ToolsDevelopmentNew\Tools Development\Tools Development\assets\json\schema.json";
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
            int textBoxCount = 0;
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
                        ++textBoxCount;
                        string dataType = GetDataType((item as TextBox).Name);
                        string errorMessage = ValidData(dataType, textBoxCount, item);
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

        private string ValidData(string dataType, int textBoxCount, object item)
        {
            string errorMessage = "";
            if (dataType.Contains("bool"))
                if (!BoolValid((item as TextBox).Text))
                    errorMessage += "第" + textBoxCount + "筆格式不正確請輸入true 或 false";
            if (dataType.Contains("byte"))
                if (!IsByte((item as TextBox).Name))
                    errorMessage += "第" + textBoxCount + "筆格式不正確請輸入數字";
            if (dataType.Contains("decimal"))
                if (!IsDecimal((item as TextBox).Name))
                    errorMessage += "第" + textBoxCount + "筆格式不正確請輸入數字";
            if (dataType.Contains("double"))
                if (!IsDouble((item as TextBox).Name))
                    errorMessage += "第" + textBoxCount + "筆格式不正確請輸入數字";
            if (dataType.Contains("float"))
                if (!IsFloat((item as TextBox).Name))
                    errorMessage += "第" + textBoxCount + "筆格式不正確請輸入數字";
            if (dataType.Contains("int"))
                if (!IsInt((item as TextBox).Name))
                    errorMessage += "第" + textBoxCount + "筆格式不正確請輸入數字";
            if (dataType.Contains("uint"))
                if (!IsUint((item as TextBox).Name))
                    errorMessage += "第" + textBoxCount + "筆格式不正確請輸入數字";
            if (dataType.Contains("long"))
                if (!IsLong((item as TextBox).Name))
                    errorMessage += "第" + textBoxCount + "筆格式不正確請輸入數字";
            if (dataType.Contains("ulong"))
                if (!IsUlong((item as TextBox).Name))
                    errorMessage += "第" + textBoxCount + "筆格式不正確請輸入數字";
            if (dataType.Contains("short"))
                if (!IsShort((item as TextBox).Name))
                    errorMessage += "第" + textBoxCount + "筆格式不正確請輸入數字";
            if (dataType.Contains("ushort"))
                if (!IsUshort((item as TextBox).Name))
                    errorMessage += "第" + textBoxCount + "筆格式不正確請輸入數字";
            if (dataType.Contains("sbyte"))
                if (!IsSbyte((item as TextBox).Name))
                    errorMessage += "第" + textBoxCount + "筆格式不正確請輸入數字";
            return errorMessage;
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
