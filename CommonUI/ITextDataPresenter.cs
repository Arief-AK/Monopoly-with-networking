using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace CommonUI
{
    interface ITextDataPresenter
    {
        RichTextBox TextBoxForMessages { get; set; }
        void WriteLine(String textLine);
    }
}
