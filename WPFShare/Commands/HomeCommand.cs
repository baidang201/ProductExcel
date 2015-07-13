using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;

namespace Signalway.CommThemes.Commands
{
    /// <summary>
    /// 返回主窗口命令
    /// </summary>
    public class HomeCommand
    {
        private static RoutedUICommand home;

        static HomeCommand()
        {
            InputGestureCollection inputs = new InputGestureCollection();
            inputs.Add(new KeyGesture(Key.Home, ModifierKeys.Control, "Ctrl+Home"));
            home = new RoutedUICommand("Home", "Home", typeof(HomeCommand), inputs);
        }

        public static RoutedUICommand Home
        {
            get { return home; }
        }
    }
}
