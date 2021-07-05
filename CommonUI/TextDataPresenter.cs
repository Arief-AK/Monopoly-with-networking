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
        }

        public String YesOrNo(String question)
        {
            YesNoQestion window = new YesNoQestion(question);
            window.ShowDialog();

            if ((window.DialogResult is not null) && (window.DialogResult == true))
                return "Y";
            else
                return "N";
        }

        public String PropertyRequest(String question)
        {
            PropertyForm window = new PropertyForm(question);
            window.ShowDialog();

            if (window.DialogResult is null)
                return "0";
            else if (window.DialogResult == false)
                return "1";
            else
                return "2";
        }
    }
}
