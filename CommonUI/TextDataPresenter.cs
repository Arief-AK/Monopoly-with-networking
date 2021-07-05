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
            String result = String.Empty;
            YesNoQestion window = new YesNoQestion(question, ref result);
            window.ShowDialog();
            return result;
        }

        public String PropertyRequest(String question)
        {
            String result = String.Empty;
            PropertyForm window = new PropertyForm(question, ref result);
            window.ShowDialog();
            return result;
        }
    }
}
