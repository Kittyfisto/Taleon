namespace Assets
{
	public sealed class MouseButtons
	{
		public static readonly MouseButtons Left = new MouseButtons(0);
		public static readonly MouseButtons Right = new MouseButtons(1);
		public static readonly MouseButtons Middle = new MouseButtons(2);
		private readonly int _value;

		private MouseButtons(int value)
		{
			_value = value;
		}

		public static implicit operator int(MouseButtons btn)
		{
			return btn._value;
		}
	}
}