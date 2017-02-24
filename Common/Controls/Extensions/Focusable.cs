/*
 *  Focusing a UIElement from a view model
 *  Source: http://stackoverflow.com/a/1356781
 */

namespace HDT.Plugins.Common.Controls.Extensions
{
	using System.Windows;

	public static class Focusable
	{
		public static bool GetIsFocused(DependencyObject obj)
		{
			return (bool)obj.GetValue(IsFocusedProperty);
		}

		public static void SetIsFocused(DependencyObject obj, bool value)
		{
			obj.SetValue(IsFocusedProperty, value);
		}

		public static readonly DependencyProperty IsFocusedProperty =
			DependencyProperty.RegisterAttached(
				"IsFocused", typeof(bool), typeof(Focusable),
				new UIPropertyMetadata(false, OnIsFocusedPropertyChanged));

		private static void OnIsFocusedPropertyChanged(
			DependencyObject d,
			DependencyPropertyChangedEventArgs e)
		{
			var uie = (UIElement)d;
			if ((bool)e.NewValue)
			{
				uie.Focus(); // false values don't matter in this case
			}
		}
	}
}