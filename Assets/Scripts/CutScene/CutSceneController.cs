using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.UI;
using UnityEngine;
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
			private readonly ICutSceneAction[] _lines;
			private int _currentLineIndex;
			private float _currentLineElapsed;
			private readonly float _length;
			private readonly Text _textBox;

			public CutSceneActionList(Text textBox, IEnumerable<ICutSceneAction> actions)
			{
				_textBox = textBox;
				_lines = actions.ToArray();
				_currentLineIndex = -1;
				_length = _lines.Sum(x => x.Length);

				ExecuteNextAction();
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

			private void ExecuteNextAction()
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
				var dialog = currentAction as DialogAction;
				if (dialog != null)
					_textBox.text = dialog.ToString();
				else
					_textBox.text = null;
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

		private QuitGameComponent _quitDialog;

		public void Play(params IEnumerable<ICutSceneAction>[] actions)
		{
			gameObject.SetActive(true);

			_quitDialog = FindObjectOfType<QuitGameComponent>();

			_actionLists = new CutSceneActionList[actions.Length];
			var length = 0f;
			int count = 0;
			for (int i = 0; i < actions.Length; ++i)
			{
				_actionLists[i] = new CutSceneActionList(TextBox, actions[i]);
				length = Mathf.Max(length, _actionLists[i].Length);
				count += _actionLists[i].Count;
			}

			Debug.LogFormat("Starting cut scene {0}seconds, {1} actions", length, count);
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