using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.CutScene
{
	public sealed class CutSceneController
		: MonoBehaviour
	{
		private ICutSceneAction[] _lines;
		private int _currentLineIndex;
		private float _currentLineElapsed;
		private float _length;

		/// <summary>
		/// The text component that shall display the dialog.
		/// </summary>
		public Text TextBox;

		public void Play(IEnumerable<ICutSceneAction> actions)
		{
			gameObject.SetActive(true);

			_lines = actions.ToArray();
			_currentLineIndex = -1;
			_length = _lines.Sum(x => x.Length);

			Debug.LogFormat("Starting cut scene {0}seconds, {1} actions", _length, _lines.Length);

			ExecuteNextAction();
		}

		private void Update()
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
				TextBox.text = dialog.ToString();
			else
				TextBox.text = null;
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
}