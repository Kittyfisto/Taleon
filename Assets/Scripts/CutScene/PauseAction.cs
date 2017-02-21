namespace Assets.Scripts.CutScene
{
	public sealed class PauseAction
		: ICutSceneAction
	{
		public float Length;

		public PauseAction(float length)
		{
			Length = length;
		}

		float ICutSceneAction.Length
		{
			get { return Length; }
		}
	}
}