namespace HDT.Plugins.Common.Settings
{
	public class SettingValue
	{
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
			return _value;
		}

		public static implicit operator string(SettingValue s)
		{
			return s._value;
		}
	}
}