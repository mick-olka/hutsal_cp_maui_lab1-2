using Microsoft.Maui.Controls;
using Microsoft.Maui.Graphics;

namespace Lab_2.Controls
{
    public class BorderedEntry : Entry
    {
        public BorderedEntry()
        {
            // Set default values 
            BackgroundColor = Color.FromArgb("#5f5");
            FontSize = 18;
            TextColor = Color.FromArgb("#333");
            PlaceholderColor = Color.FromArgb("#666");
        }
    }
}

