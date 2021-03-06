﻿using UnityEngine;

namespace Assets.Scripts.CutScene
{
	public sealed class CameraCutAction
		: ICutSceneAction
	{
		private readonly GameObject _anchor;
		private readonly float _length;

		public CameraCutAction(GameObject anchor, float length)
		{
			_anchor = anchor;
			_length = length;
		}

		public Transform Transform
		{
			get { return _anchor != null ? _anchor.transform : null; }
		}

		public float Length
		{
			get { return _length; }
		}
	}
}