using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace MilitaryApp
{
    public static class NavigationService
    {
        public static void NavigateTo<T>() where T : Window, new()
        {
            var currentWindow = Application.Current.Windows.OfType<Window>().FirstOrDefault(w => w.IsActive);
            var newWindow = new T();

            if (currentWindow != null)
            {
                currentWindow.Close();
            }

            newWindow.Show();
        }
    }
}
