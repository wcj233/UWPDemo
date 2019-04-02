using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace UWPDemo
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
        }

        private void clickToChangeBg(object sender, RoutedEventArgs e)
        {
            //AppBarButton cb = sender as AppBarButton;
            Random ro = new Random(10);
            long tick = DateTime.Now.Ticks;
            Random ran = new Random((int)(tick & 0xffffffffL) | (int)(tick >> 32));

            int R = ran.Next(255);
            int G = ran.Next(255);
            int B = ran.Next(255);
            B = (R + G > 400) ? R + G - 400 : B;//0 : 380 - R - G;
            B = (B > 255) ? 255 : B;

            Color LightBlue = Color.FromArgb(255, (byte)R, (byte)G, (byte)B);
            this.Background = new SolidColorBrush(LightBlue);
        }

        private void firstNum_TextChanged(object sender, TextChangedEventArgs e)
        {
            int transFirstNum = 0;
            bool isRight = int.TryParse(firstNum.Text, out transFirstNum);
            if (isRight == false && firstNum.Text.Length > 0)
            {
                errorStr.Text = "FirstNum is not an integer.";
                return;
            }
            else {
                errorStr.Text = "";
            }
            if (firstNum.Text.Length > 0 && secondNum.Text.Length > 0 && calculate.SelectedValue != null)
            {
                calculateResult(int.Parse(firstNum.Text), int.Parse(secondNum.Text), calculate.SelectedIndex);
            }
            else {
                result.Text = "";
            }
        }

        private void calculate_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string calType = e.AddedItems[0].ToString();
            if (firstNum.Text.Length > 0 && secondNum.Text.Length > 0 && calculate.SelectedValue != null)
            {
                calculateResult(int.Parse(firstNum.Text), int.Parse(secondNum.Text), calculate.SelectedIndex);
            }
        }

        private void secondNum_TextChanged(object sender, TextChangedEventArgs e)
        {
            int transSecondNum = 0;
            bool isRight = int.TryParse(secondNum.Text, out transSecondNum);
            if (isRight == false && secondNum.Text.Length > 0)
            {
                errorStr.Text = "SecondNum is not an integer.";
                return;
            }
            else
            {
                errorStr.Text = "";
            }
            if (firstNum.Text.Length > 0 && secondNum.Text.Length > 0 && calculate.SelectedValue != null)
            {
                calculateResult(int.Parse(firstNum.Text), int.Parse(secondNum.Text), calculate.SelectedIndex);
            }
            else
            {
                result.Text = "";
            }
        }

        private void calculateResult (int firstNum, int secondNum, int calTypeIndex) {
            switch (calTypeIndex) {
                case 0:
                    int resultNumP = firstNum + secondNum;
                    result.Text = resultNumP.ToString();
                    break;
                case 1:
                    int resultNumM = firstNum - secondNum;
                    result.Text = resultNumM.ToString();
                    break;
                case 2:
                    int resultNumMM = firstNum * secondNum;
                    result.Text = resultNumMM.ToString();
                    break;
                case 3:
                    if (secondNum == 0)
                    {
                        errorStr.Text = "0 cannot be used as divisor";
                    }
                    else {
                        int resultNum = firstNum / secondNum;
                        result.Text = resultNum.ToString();
                    }
                    break;
            }
        }
        
    }
}
