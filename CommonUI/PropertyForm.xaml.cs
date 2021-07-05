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
using System.Windows.Shapes;

namespace CommonUI
{
    /// <summary>
    /// Interaction logic for PropertyForm.xaml
    /// </summary>
    public partial class PropertyForm : Window
    {
        private String ButtonResult;

        public PropertyForm(String question, ref String result)
        {
            InitializeComponent();

            Run myRun = new Run(question);
            Paragraph paragraph = new Paragraph();
            paragraph.Inlines.Add(myRun);

            TextBoxForQuestion.Document.Blocks.Add(paragraph);
        }

        private void HotelButton_OnClick(object sender, RoutedEventArgs e)
        {
            ButtonResult = "2";
            Close();
        }

        private void HouseButton_OnClick(object sender, RoutedEventArgs e)
        {
            ButtonResult = "1";
            Close();
        }

        private void NoButton_OnClick(object sender, RoutedEventArgs e)
        {
            ButtonResult = "0";
            Close();
        }
    }
}
