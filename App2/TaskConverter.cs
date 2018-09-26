using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Globalization.DateTimeFormatting;

namespace IntoTheBrain
{
    internal class TaskConverter : Windows.UI.Xaml.Data.IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string culture)
        {
            // подробнее на странице http://msdn.microsoft.com/ru-ru/library/windows/apps/br211380.aspx искать слово DataConverter
            if (value == null)
                throw new ArgumentNullException("value", "Значение Тайтла задачи не должно быть null");

            if (!(value is uint))
                throw new ArgumentException("Value must be of type DateTime.", "value");

            uint numTask = (uint) value;
            return "Задача № " + numTask;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            string strValue = value as string;
            uint resultTitle;
            if (uint.TryParse(strValue, out resultTitle))
            {
                return resultTitle;
            }
            return Windows.UI.Xaml.DependencyProperty.UnsetValue;
        }
    }
}
