using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;

namespace Signalway.CommThemes.Commands
{
    /// <summary>
    /// 向前命令
    /// </summary>
    public class ForwardCommand
    {
        private static RoutedUICommand forward;

        static ForwardCommand()
        {
            InputGestureCollection inputs = new InputGestureCollection(); 
            inputs.Add(new KeyGesture(Key.Right, ModifierKeys.Control, "Ctrl+Right"));
            forward = new RoutedUICommand("Forward", "Forward", typeof(ForwardCommand), inputs);
        }

        public static RoutedUICommand Forward
        {
            get { return forward; }
        }
    }
}
