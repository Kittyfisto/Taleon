using Assets.Scripts.CutScene;
using UnityEngine;

namespace Assets.Scripts.Chapters.Chapter01
{
	/// <summary>
	///     Responsible for controlling the entire cut scene that forms the introduction.
	///     Controls:
	///     - Camera movement and -cuts
	///     - Dialog (both voice and subtitle)
	/// </summary>
	public class IntroCutScene
		: MonoBehaviour
	{
		public CutSceneController Panel;

		private void Start()
		{
			var lines = new[]
			{
				new DialogLine(2f, "XO", "That was the last batch. We're fully loaded and ready to fly, ma'am"),
				new DialogLine(2f, "Captain", "Thank you, <insert player name>. Let's go."),
				new DialogLine(2f, "Captain", "This is the commander. Man all stations and prepare for immediate undocking."),
				new DialogLine(2f, "Captain", "Coms, request permission to undock."),
				new DialogLine(2f, "Communication Officer", "Aye, aye ma'am."),
				new DialogLine(2f, "Communication Officer", "NTC, this is Support Frigate Osiris requesting permission to undock, over."),
				new DialogLine(2f, "Station", "Osiris, this is Nestor Traffic Control, permission granted. Godspeed."),
				new DialogLine(2f, "Captian", "Take us out <insert player name>, slow and steady."),
				new DialogLine(2f, "XO", "Release docking clamps."),
				new DialogLine(2f, "Navigation Officer", "Releasing docking clamps."),
				new DialogLine(2f, "XO", "Take us out, <insert nav name>."),
				new DialogLine(2f, "Navigation Officer", "Yessir."),
				new DialogLine(2f, "Captain", "XO, Set course to intercept the <Sister Ship Name>. We will support her in patrolling LTO."),
				new DialogLine(2f, "XO", "Aye Aye, Cpt.")
			};

			Panel.Play(lines);
		}
	}
}