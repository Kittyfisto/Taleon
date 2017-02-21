using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.CutScene
{
	public sealed class CutSceneController
		: MonoBehaviour
	{
		private DialogLine[] _lines;
		private int _currentLineIndex;
		private float _currentLineElapsed;

		/// <summary>
		/// The text component that shall display the dialog.
		/// </summary>
		public Text TextBox;

		public void Play(DialogLine[] lines)
		{
			gameObject.SetActive(true);

			_lines = lines;
			_currentLineIndex = -1;
			ShowNextLine();
		}

		private void Update()
		{
			var currentLine = GetCurrentLine();
			if (currentLine == null)
				return;

			if (_currentLineElapsed > currentLine.Length)
			{
				ShowNextLine();
			}
			else
			{
				_currentLineElapsed += Time.deltaTime;
			}
		}

		private void ShowNextLine()
		{
			++_currentLineIndex;
			_currentLineElapsed = 0;

			var currentLine = GetCurrentLine();
			if (currentLine == null)
				return;

			Show(currentLine);
		}

		private void Show(DialogLine currentLine)
		{
			TextBox.text = currentLine.ToString();
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

		private DialogLine GetCurrentLine()
		{
			if (IsFinished)
				return null;

			var currentLine = _lines[_currentLineIndex];
			return currentLine;
		}
	}
}