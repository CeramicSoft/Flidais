using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Flidais.Helper
{
	public static class UIHelper
	{
		private static IEnumerable<T> FindVisualChildren<T>(DependencyObject parent) where T : DependencyObject
		{
			if (parent == null)
			{
				yield break;
			}

			for (int i = 0; i < VisualTreeHelper.GetChildrenCount(parent); i++)
			{
				DependencyObject child = VisualTreeHelper.GetChild(parent, i);
				if (child is T t)
				{
					yield return t;
				}

				foreach (T childOfChild in FindVisualChildren<T>(child))
				{
					yield return childOfChild;
				}
			}
		}
		public static void UpdateControlColors(Control control, Brush background, Brush text, Brush foreground)
		{
			control.Background = background;
			foreach (Control innerControl in UIHelper.FindVisualChildren<Control>(control))
			{
				innerControl.Margin = new Thickness(2);
				if (innerControl is TextBox textBox)
				{
					textBox.Foreground = text;
					textBox.Background = foreground;
				}
				if (innerControl is Label label)
				{
					label.Foreground = text;
				}
				if (innerControl is ComboBox combobox)
				{
					combobox.Background = foreground;
				}
				if (innerControl is CheckBox checkBox)
				{
					checkBox.Foreground = text;
				}
				if (innerControl is Button button)
				{
					button.Background = foreground;
					button.Foreground = text;
				}
			}
		}
	}
}
