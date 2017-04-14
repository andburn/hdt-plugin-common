namespace HDT.Plugins.Common.Models
{
	public class Note
	{
		public string Text { get; set; }
		public string Archetype { get; set; }

		public Note()
		{
		}

		public Note(string text)
			: this()
		{
			Text = text;
			// TODO parse out archetype if any...optional param...
		}
	}
}