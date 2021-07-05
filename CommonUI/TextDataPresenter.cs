using System;
using System.Windows.Controls;
using System.Windows.Documents;

namespace CommonUI
{
    public class TextDataPresenter
    {
        public RichTextBox TextBoxForMessages { get; set; }
        public void WriteLine(string textLine)
        {
            Run myRun = new Run(textLine);
            //Bold myBold = new Bold(new Run("edit me!"));
            Paragraph paragraph = new Paragraph();
            paragraph.Inlines.Add(myRun);
            //paragraph.Inlines.Add(myBold);

            TextBoxForMessages.Document.Blocks.Add(paragraph);
            TextBoxForMessages.ScrollToEnd();
        }

        public String YesOrNo(String question)
        {
            YesNoQestion window = new YesNoQestion(question);
            window.ShowDialog();

            String res = String.Empty;

            if ((window.DialogResult is not null) && (window.DialogResult == true))
                res = "Y";
            else
                res = "N";

            WriteLine("Answer: " + res);
            return res;
        }

        public String PropertyRequest(String question)
        {
            PropertyForm window = new PropertyForm(question);
            window.ShowDialog();

            String res = String.Empty;

            if (window.DialogResult is null)
                res = "0";
            else if (window.DialogResult == false)
                res = "1";
            else
                res = "2";

            WriteLine("Answer: " + res);
            return res;
        }
    }
}
