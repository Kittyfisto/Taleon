namespace Assets.Scripts.CutScene
{
	/// <summary>
	///     Represents a line of dialog of one speaker in a cut scene, speech or dialog.
	/// </summary>
	public sealed class DialogAction
		: ICutSceneAction
	{
		/// <summary>
		///     The amount of seconds this line should be visible on screen.
		/// </summary>
		public readonly float Length;

		public readonly string Speaker;
		public readonly string Text;

		public DialogAction(float length, string speaker, string text)
		{
			Length = length;
			Speaker = speaker;
			Text = text;
		}

		public override string ToString()
		{
			return string.Format("{0}: {1}", Speaker, Text);
		}

		float ICutSceneAction.Length
		{
			get { return Length; }
		}
	}
}