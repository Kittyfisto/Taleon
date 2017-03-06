using System;
using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.UI;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Assets.Scripts.CutScene
{
	public sealed class CutSceneController
		: MonoBehaviour
	{
		/// <summary>
		/// Responsible for executing a list of serial actions where each action
		/// is executed for a certain amount of time.
		/// </summary>
		private sealed class CutSceneActionList
		{
			private readonly Dictionary<Type, Action<ICutSceneAction>> _actions;
			private readonly ICutSceneAction[] _lines;
			private readonly float _length;
			private readonly Text _textBox;
			private readonly Camera _camera;

			private int _currentLineIndex;
			private float _currentLineElapsed;

			public CutSceneActionList(Text textBox, Camera camera, IEnumerable<ICutSceneAction> actions)
			{
				_actions = new Dictionary<Type, Action<ICutSceneAction>>();
				_actions.Add(typeof(DialogAction), ShowDialog);
				_actions.Add(typeof(PauseDialogAction), PauseDialog);
				_actions.Add(typeof(CameraCutAction), MoveCamera);
				_actions.Add(typeof(LoadSceneAction), LoadScene);

				_textBox = textBox;
				_camera = camera;

				_lines = actions.ToArray();
				_currentLineIndex = -1;
				_length = _lines.Sum(x => x.Length);
			}

			private void LoadScene(ICutSceneAction action)
			{
				var name = ((LoadSceneAction) action).SceneName;
				Debug.LogFormat("Loading scene '{0}'...", name);
				SceneManager.LoadScene(name, LoadSceneMode.Single);
			}

			private void MoveCamera(ICutSceneAction action)
			{
				var cut = (CameraCutAction) action;
				var transform = cut.Transform;

				if (transform != null && _camera != null)
				{
					var cameraTransform = _camera.transform;

					cameraTransform.position = transform.position;
					cameraTransform.rotation = transform.rotation;

					Debug.LogFormat("Camera position/rotation changed to {0}/{1}",
						transform.position,
						transform.rotation);
				}
				else
				{
					Debug.LogWarning("No transform given - camera will not change position/rotation");
				}
			}

			private void PauseDialog(ICutSceneAction action)
			{
				if (_textBox == null)
					return;

				_textBox.text = null;
			}

			private void ShowDialog(ICutSceneAction action)
			{
				if (_textBox == null)
					return;

				_textBox.text = ((DialogAction)action).ToString();
			}

			public float Length
			{
				get { return _length; }
			}

			public int Count
			{
				get { return _lines.Length; }
			}

			public void Update()
			{
				var currentLine = GetCurrentAction();
				if (currentLine == null)
					return;

				if (_currentLineElapsed > currentLine.Length)
				{
					ExecuteNextAction();
				}
				else
				{
					_currentLineElapsed += Time.deltaTime;
				}
			}

			internal void ExecuteNextAction()
			{
				++_currentLineIndex;
				_currentLineElapsed = 0;

				var currentAction = GetCurrentAction();
				if (currentAction == null)
					return;

				Execute(currentAction);
			}

			private void Execute(ICutSceneAction currentAction)
			{
				var type = currentAction.GetType();
				Action<ICutSceneAction> fn;
				if (_actions.TryGetValue(type, out fn))
				{
					fn(currentAction);
				}
				else
				{
					Debug.LogWarningFormat("Unknown cut scene action: {0}", type);
				}
			}

			public bool IsFinished
			{
				get
				{
					if (_lines == null || _currentLineIndex < 0 || _currentLineIndex >= _lines.Length)
						return true;

					return false;
				}
			}

			private ICutSceneAction GetCurrentAction()
			{
				if (IsFinished)
					return null;

				var currentLine = _lines[_currentLineIndex];
				return currentLine;
			}
		}

		private CutSceneActionList[] _actionLists;

		/// <summary>
		/// The text component that shall display the dialog.
		/// </summary>
		public Text TextBox;

		public Camera Camera;

		private QuitGameComponent _quitDialog;

		public void Play(params IEnumerable<ICutSceneAction>[] actions)
		{
			gameObject.SetActive(true);

			_quitDialog = FindObjectOfType<QuitGameComponent>();
			_actionLists = new CutSceneActionList[actions.Length];

			if (TextBox == null)
				Debug.LogWarning("No TextBox assigned - subtitles will not be displayed");
			if (Camera == null)
				Debug.LogWarning("No Camera assigned - camera will not change position during cutscene");

			var length = 0f;
			int count = 0;
			for (int i = 0; i < actions.Length; ++i)
			{
				_actionLists[i] = new CutSceneActionList(TextBox, Camera, actions[i]);
				length = Mathf.Max(length, _actionLists[i].Length);
				count += _actionLists[i].Count;
			}

			Debug.LogFormat("Starting cut scene {0}seconds, {1} actions", length, count);

			foreach (var action in _actionLists)
			{
				action.ExecuteNextAction();
			}
		}

		private void Update()
		{
			// We want to pause playback until the quit dialog is closed (again)
			if (_quitDialog.IsShowingQuitDIalog)
				return;

			if (_actionLists == null)
				return;

			foreach (var actionList in _actionLists)
			{
				actionList.Update();
			}
		}
	}
}