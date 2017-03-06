namespace Assets.Scripts.CutScene
{
	public sealed class PauseDialogAction
		: ICutSceneAction
	{
		public float Length;

		public PauseDialogAction(float length)
		{
			Length = length;
		}

		float ICutSceneAction.Length
		{
			get { return Length; }
		}
	}
}