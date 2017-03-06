using Assets.Scripts.CutScene;
using UnityEngine;

namespace Assets.Scripts.Chapters.Chapter01
{
	/// <summary>
	///     Defines the entire intro cut scene of chapter 01.
	/// </summary>
	public class IntroCutScene
		: MonoBehaviour
	{
		public GameObject Panel;

		private void Start()
		{
			var lines = new ICutSceneAction[]
			{
				new DialogAction(3f,   "XO", "That was the last batch. We're fully loaded and ready to fly, ma'am"),
				new DialogAction(2.5f, "Captain", "Thank you, <insert player name>. Let's go."),
				new PauseDialogAction(2f),
				new DialogAction(3.5f, "Captain", "This is the commander. Man all stations and prepare for immediate undocking."),
				new PauseDialogAction(1f),
				new DialogAction(2f,   "Captain", "Coms, request permission to undock."),
				new DialogAction(2f,   "Communication Officer", "Aye, aye ma'am."),
				new PauseDialogAction(1f),
				new DialogAction(4f,   "Communication Officer", "NTC, this is Support Frigate Osiris requesting permission to undock, over."),
				new DialogAction(3.5f, "Station", "Osiris, this is Nestor Traffic Control, permission granted. Godspeed."),
				new PauseDialogAction(1f),
				new DialogAction(3f,   "Captian", "Take us out <insert player name>, slow and steady."),
				new DialogAction(2f,   "XO", "Release docking clamps."),
				new DialogAction(3f,   "Navigation Officer", "Releasing docking clamps."),
				new PauseDialogAction(1f),
				new DialogAction(2.5f, "XO", "Take us out, <insert nav name>."),
				new DialogAction(2.5f, "Navigation Officer", "Yessir."),
				new PauseDialogAction(10f),
				new DialogAction(4f,   "Captain", "XO, Set course to intercept the <Sister Ship Name>. We will support her in patrolling LTO."),
				new DialogAction(2f,   "XO", "Aye Aye, Cpt.")
			};
			var cameraCuts = new[]
			{
				new CameraCutAction(GameObject.Find("Position1"), 7.5f),
				new CameraCutAction(GameObject.Find("Position2"), 13.5f),
				new CameraCutAction(GameObject.Find("Position3"), 10),
				new CameraCutAction(GameObject.Find("Position4"), 20)
			};

			var controller = Panel.GetComponent<CutSceneController>();
			controller.Play(lines, cameraCuts);
		}
	}
}