using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace EnglishBySongs.Controls
{
    public class CustomView : ContentView
    {
        public event EventHandler<EventArgs> LongPressEvent;

        public void RaiseLongPressEvent()
        {
            if (IsEnabled)
                LongPressEvent?.Invoke(this, EventArgs.Empty);
        }
    }
}
