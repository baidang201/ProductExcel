using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;

namespace Signalway.CommThemes.Commands
{
    /// <summary>
    /// 向后命令
    /// </summary>
    public class BackwardCommand
    {
        private static RoutedUICommand backward;

        static BackwardCommand()
        {
            InputGestureCollection inputs = new InputGestureCollection();
            inputs.Add(new KeyGesture(Key.Left, ModifierKeys.Control, "Ctrl+Left"));
            backward = new RoutedUICommand("Backward", "Backward", typeof(BackwardCommand), inputs);
        }

        public static RoutedUICommand Backward
        {
            get { return backward; }
        }
    }
}
