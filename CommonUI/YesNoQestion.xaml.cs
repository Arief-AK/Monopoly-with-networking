﻿using System;
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
    /// Interaction logic for YesNoQestion.xaml
    /// </summary>
    public partial class YesNoQestion : Window
    {
        private String ButtonResult;
        public YesNoQestion(String question, ref String result)
        {
            InitializeComponent();

            Run myRun = new Run(question);
            Paragraph paragraph = new Paragraph();
            paragraph.Inlines.Add(myRun);

            TextBoxForQuestion.Document.Blocks.Add(paragraph);
        }

        private void NoButton_OnClick(object sender, RoutedEventArgs e)
        {
            ButtonResult = "N";
            Close();
        }

        private void YesButton_OnClick(object sender, RoutedEventArgs e)
        {
            ButtonResult = "Y";
            Close();
        }
    }
}