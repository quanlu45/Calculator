using System;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.IO;
using System.Windows.Markup;
using System.Xml;

namespace Calculator
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {

        private double  res=0;    //操作数
        private string op="=";    //操作符
        private string num ="";   //操作数
        private StringBuilder formula = new StringBuilder("");   //式子
        public MainWindow()
        {
            InitializeComponent();
            label1.Content = "0";
            label2.Content = "";

        }

        /// <summary>
        /// 加减乘除得计算
        /// </summary>
        private void Calculate()
        {
            double tmp=0;
            try
            {
              tmp = double.Parse(num);
               
               
            }catch(Exception)
            {
                return;
            }

            switch (op)
            {
                case "add":
                    res += tmp;
                    break;
                case "sub":
                    res -= tmp;
                    break;
                case "mul":
                    res *= tmp;
                    break;
                case "div":
                    res /= tmp;
                    break;
                case "=":
                    res = tmp;
                    break;
            }
            label1.Content = res;


        }

        /// <summary>
        /// 序列化反序列化深度拷贝resultLable
        /// </summary>
        private void SaveResult()
        {
            Label tmp = null;
            string labelXaml = XamlWriter.Save(resultlable);
            StringReader stringReader = new StringReader(labelXaml);
            XmlReader xmlReader = XmlReader.Create(stringReader);
            tmp = XamlReader.Load(xmlReader) as Label;
            resultListBox.Items.Insert(0, tmp);

        }

        private void ButtonClick(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;
        
            switch (button.Name)
            {
                case "equal":

                    if (!op.Equals("sqrt") && !op.Equals("square") && !op.Equals("reci") && !op.Equals("percent") && !op.Equals("negate"))
                        formula.Append(label1.Content.ToString()+"=");
                    Calculate();
                    label2.Content = formula.ToString();
                    SaveResult();
                    formula.Clear();
                    num = "";
                    op = "=";
                    break;
                    
                case "add":case "sub":case "mul":case "div":
                    //替换上一次得操作符
                    if (!op.Equals("sqrt") && !op.Equals("square") && !op.Equals("reci") && !op.Equals("percent"))
                    {
                        formula.Append(label1.Content.ToString());
                        try
                        {
                            double.Parse(formula[formula.Length - 1].ToString());
                        }
                        catch (FormatException)
                        {
                            if (op.Equals("div"))
                                formula.Remove(formula.Length - 2, 2);
                            else
                                formula = formula.Remove(formula.Length - 1, 1);
                        }


                    }

                    formula.Append(button.Content.ToString());
                    Calculate();
                    num = "";
                    op = button.Name;
                    break;
                case "zero":case "one":case "two":case "three":case "four":case "five":case "six":case "seven":case "eight":case "nine":
                case "dot":
                    if (op.Equals("sqrt") ||op.Equals("square") ||op.Equals("reci") ||op.Equals("percent"))
                    {
                        num = "";
                        formula.Clear();
                        op = "=";
                    }

                    num = num + button.Content;
                    label1.Content = num; 
                    break;
                case "sqrt":
                    num = Math.Sqrt(double.Parse(label1.Content.ToString())).ToString();
                    formula.Append(button.Content.ToString() + "(" + label1.Content.ToString() + ")");
                    Calculate();
                    op = "sqrt";
                    label1.Content = num;
                    break;
                case "square":
                    num = Math.Pow(double.Parse(label1.Content.ToString()), 2).ToString();
                    formula.Append( "sqr"+"(" + label1.Content.ToString() + ")");
                    Calculate();
                    op = "square";
                    label1.Content = num;
                    break;
                case "reci":
                    num = (1 / double.Parse(label1.Content.ToString())).ToString();
                    formula.Append(1 + "/(" + label1.Content.ToString() + ")");
                    Calculate();
                    op = "reci";
                    label1.Content = num;
                    break;
                case "percent":
                    num = (double.Parse(label1.Content.ToString())/100).ToString();
                    formula.Append(button.Content.ToString() + "(" + label1.Content.ToString() + ")");
                    Calculate();
                    op = "percent";
                    label1.Content = num;
                    break;
                case "negate":
                    num = (-double.Parse(label1.Content.ToString())).ToString();
                    formula.Append("negate" + "(" + label1.Content.ToString() + ")");
                    Calculate();
                    op = "percent";
                    label1.Content = num;
                    break;
                case "C":
                    num = "";
                    res = 0;
                    formula.Clear();
                    label1.Content = "0";
                    op = "=";
                    break;
                case "CE":
                    label1.Content = "0";

                    num = "";
                    break;
                case "del":
                    try
                    {
                        num = num.Substring(0, num.ToString().Length - 1);
                        label1.Content = num;
                    }
                    catch(ArgumentOutOfRangeException)
                    {
                        label1.Content = "0";
                    }
                    break;
            }
            label2.Content = formula.ToString();
            
        }
    }
}
