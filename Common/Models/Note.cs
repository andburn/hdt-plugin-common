using System.Text.RegularExpressions;

namespace HDT.Plugins.Common.Models
{
	public class Note
	{
		private static readonly Regex _noteRegex =
			new Regex(@"^\s*\[(?<tag>([A-Za-z0-9\s_\-',]+))\]\s*(?<note>(.*))$",
				RegexOptions.Compiled);

		private string _text;

		public string Text
		{
			get
			{
				return _text;
			}
			set
			{
				SetText(value);
			}
		}

		public string Archetype { get; set; }

		public bool HasArchetype
		{
			get
			{
				return !string.IsNullOrWhiteSpace(Archetype);
			}
		}

		public Note()
		{
			_text = string.Empty;
			Archetype = string.Empty;
		}

		public Note(string text)
			: this()
		{
			SetText(text);
		}

		private void SetText(string text)
		{
			if (!string.IsNullOrWhiteSpace(text))
			{
				var match = _noteRegex.Match(text);
				if (match.Success)
				{
					Archetype = match.Groups["tag"].Value;
					_text = match.Groups["note"].Value;
				}
				else
				{
					_text = text;
				}
			}
		}
	}
}