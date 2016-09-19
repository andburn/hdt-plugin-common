namespace HDT.Plugins.Common.Settings
{
	public class SettingValue
	{
		public static readonly SettingValue Empty = new SettingValue(null);

		private string _value;

		public string Value
		{
			get { return _value; }
		}

		public bool Bool
		{
			get
			{
				bool b;
				bool.TryParse(_value, out b);
				return b;
			}
		}

		public int Int
		{
			get
			{
				int i;
				int.TryParse(_value, out i);
				return i;
			}
		}

		public double Double
		{
			get
			{
				double d;
				double.TryParse(_value, out d);
				return d;
			}
		}

		public SettingValue()
		{
			_value = string.Empty;
		}

		public SettingValue(string value)
		{
			_value = value;
		}

		public override string ToString()
		{
			return Value;
		}

		public override bool Equals(object obj)
		{
			if (obj == null)
				return false;

			var value = obj as SettingValue;
			if (value == null)
				return false;

			return Value == value.Value;
		}

		public override int GetHashCode()
		{
			return Value.GetHashCode();
		}

		public static implicit operator string(SettingValue s)
		{
			return s.Value;
		}
	}
}