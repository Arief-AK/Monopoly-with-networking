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
    }
}
