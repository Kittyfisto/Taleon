namespace Assets.Scripts.CutScene
{
	public sealed class LoadSceneAction
		: ICutSceneAction
	{
		private readonly string _sceneName;

		public LoadSceneAction(string sceneName)
		{
			_sceneName = sceneName;
		}

		public string SceneName { get { return _sceneName; } }

		public float Length
		{
			get { return 0; }
		}
	}
}