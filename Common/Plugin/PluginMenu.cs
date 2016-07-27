﻿using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using HDT.Plugins.Common.Util;

namespace HDT.Plugins.Common.Plugin
{
	/// <summary>
	/// Helper class to create a MenuItem one level deep
	/// </summary>
	public class PluginMenu
	{
		public MenuItem Menu { get; private set; }

		public PluginMenu(string header)
		{
			Menu = new MenuItem();
			Menu.Header = header;
		}

		public PluginMenu(string header, string icon)
			: this(header)
		{
			Menu.Icon = CreateIcon(icon);
		}

		public void Append(string header, ICommand command)
		{
			Append(header, null, command);
		}

		public void Append(string header, string icon, ICommand command)
		{
			Menu.Items.Add(new MenuItem() {
				Header = header,
				Icon = CreateIcon(icon),
				Command = command
			});
		}

		private TextBlock CreateIcon(string name)
		{
			if (string.IsNullOrWhiteSpace(name))
				return null;

			var block = new TextBlock();
			block.FontFamily = new FontFamily(
				new Uri("pack://application:,,,/HDT.Plugins.Common;component/Resources/"), "./#IcoMoon-Free");
			block.FontSize = 18;
			block.Margin = new Thickness(5, 0, 0, 0);
			block.Text = IcoMoon.Get(name);
			return block;
		}
	}
}