using System;

namespace HDT.Plugins.Common.Plugin
{
	[AttributeUsage(System.AttributeTargets.Class)]
	public class Name : Attribute
	{
		private string _text;

		public Name(string text)
		{
			_text = text;
		}

		public string Get()
		{
			return _text;
		}
	}

	[AttributeUsage(System.AttributeTargets.Class)]
	public class Description : Attribute
	{
		private string _text;

		public Description(string text)
		{
			_text = text;
		}

		public string Get()
		{
			return _text;
		}
	}

	[AttributeUsage(System.AttributeTargets.Class)]
	public class Author : Attribute
	{
		private string _text;

		public Author(string text)
		{
			_text = text;
		}

		public string Get()
		{
			return _text;
		}
	}

	[AttributeUsage(System.AttributeTargets.Class)]
	public class ButtonText : Attribute
	{
		private string _text;

		public ButtonText(string text)
		{
			_text = text;
		}

		public string Get()
		{
			return _text;
		}
	}

	[AttributeUsage(System.AttributeTargets.Class)]
	public class PluginVersion : Attribute
	{
		private string _text;

		public PluginVersion(string text)
		{
			_text = text;
		}

		public string Get()
		{
			return _text;
		}
	}
}