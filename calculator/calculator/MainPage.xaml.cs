

using System.Diagnostics;
using System.Security.Cryptography.X509Certificates;
using System.Text.RegularExpressions;

namespace calculator
{
    public partial class MainPage : ContentPage
    {
        String screenText;
        int count = 0;
        public MainPage()
        {
            InitializeComponent();
        }
        private void ChangeSign(object sender, EventArgs e)
        {
            if (screenText == "")
            {

            }
            else
            {
                int position = myLabel.Text.LastIndexOf(screenText);
                string beforePosition = myLabel.Text.Substring(0, position);  // Part before "*"
                string afterPosition = myLabel.Text.Substring(position);
                string newPosition = "hi";

                if (afterPosition.Contains(")"))
                {
                    position = myLabel.Text.LastIndexOf("(");
                    beforePosition = myLabel.Text.Substring(0, position);  // Part before "*"
                    afterPosition = screenText;
                    newPosition = newPosition.Replace("(", "").Replace(")", "").Replace("-", "");
                    myLabel.Text = beforePosition + afterPosition;
                }


                else
                {
                    afterPosition = "(-" + afterPosition + ")";
                    myLabel.Text = beforePosition + afterPosition;
                }

            }
        }
        private void CLEAR(object sender, EventArgs e)
        {
            screenText = "";
            myLabel.Text = screenText;
        }
        private void OnNumberClicked(object sender, EventArgs e)
        {
            screenText += ((Button)sender).Text;
            myLabel.Text += ((Button)sender).Text;
        }

        private void OnSymClicked(object sender, EventArgs e)
        {
            screenText = "";
            myLabel.Text += ((Button)sender).Text;
        }


        private void OnButtonClicked(object sender, EventArgs e)
        {
            string inputText = ((Button)sender).Text;
            myLabel.Text = giveNum(myLabel.Text);
        }

        private string giveNum(String nums)
        {
            this.screenText = "";
            bool containsPlus = nums.Contains("+");
            bool containsMin = Regex.IsMatch(nums, @"(?<!\()\-");
            bool containsDiv = nums.Contains(" / ");
            bool containsTime = nums.Contains("x");

            // Handle multiplication and division first (PEMDAS)
            while (containsTime || containsDiv)
            {
                if (containsTime)
                {
                    Match match = Regex.Match(nums, @"\(?(-?\d+)\)?\s*x\s*\(?(-?\d+)\)?");
                    if (match.Success)
                    {
                        string num1 = match.Groups[1].Value;  // Number before 'x'
                        string num2 = match.Groups[2].Value;  // Number after 'x'

                        int result = (int.Parse(num1) * int.Parse(num2));  // Perform multiplication
                        int endOfNum2 = match.Index + match.Length;  // Position after 'num2'
                        nums = nums.Substring(0, match.Index) + "(" + result.ToString() + ")" + nums.Substring(endOfNum2);
                        containsTime = nums.Contains("x");  // Recheck for more multiplications
                    }
                }

                if (containsDiv)
                {
                    Match match = Regex.Match(nums, @"\(?(-?\d+)\)?\s*/\s*\(?(-?\d+)\)?");
                    if (match.Success)
                    {
                        string num1 = match.Groups[1].Value;  // Number before '/'
                        string num2 = match.Groups[2].Value;  // Number after '/'

                        int result = (int.Parse(num1) / int.Parse(num2));  // Perform division
                        int endOfNum2 = match.Index + match.Length;  // Position after 'num2'
                        nums = nums.Substring(0, match.Index) + "(" + result.ToString() + ")" + nums.Substring(endOfNum2);
                        containsDiv = nums.Contains("/");  // Recheck for more divisions
                    }
                }
            }

            // Handle addition and subtraction next
            while (containsPlus || containsMin)
            {
                if (containsPlus)
                {
                    Match match = Regex.Match(nums, @"\(?(-?\d+)\)?\s*\+\s*\(?(-?\d+)\)?");
                    if (match.Success)
                    {
                        string num1 = match.Groups[1].Value;  // Number before '+'
                        string num2 = match.Groups[2].Value;  // Number after '+'

                        int result = (int.Parse(num1) + int.Parse(num2));  // Perform addition
                        int endOfNum2 = match.Index + match.Length;  // Position after 'num2'
                        nums = nums.Substring(0, match.Index) + "(" + result.ToString() + ")" + nums.Substring(endOfNum2);
                        containsPlus = nums.Contains("+");  // Recheck for more additions
                    }
  
                }

                if (containsMin)
                {
                    Match match = Regex.Match(nums, @"\(?(-?\d+)\)?\s*-\s*\(?(-?\d+)\)?");
                    if (match.Success)
                    {
                        string num1 = match.Groups[1].Value;  // Number before '-'
                        string num2 = match.Groups[2].Value;  // Number after '-'

                        int result = (int.Parse(num1) - int.Parse(num2));  // Perform subtraction
                        int endOfNum2 = match.Index + match.Length;  // Position after 'num2'
                        nums = nums.Substring(0, match.Index) + "(" + result.ToString() + ")" + nums.Substring(endOfNum2);
                        containsMin = Regex.IsMatch(nums, @"(?<!\()\-");
                    }
                }
            }

            return nums;
        }
    }

}
