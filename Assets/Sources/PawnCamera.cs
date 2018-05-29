/*******************************************************
 * Copyright (C) 2016 Raving Bots - All Rights Reserved
 * 
 * ravingbots.com
 * contact@ravingbots.com
 *
 * This file is part of Character Example project.
 * 
 * Unauthorized distributing and/or copying of this file
 * without the express permission of the author
 * is strictly prohibited.
 *******************************************************/

using UnityEngine;

[RequireComponent(typeof(Camera))]
public class PawnCamera : MonoBehaviour
{
	public Vector3 AttachOffset = new Vector3(0, 2, -4);
	public float FollowSmooth = 0.1f;
	public float LookSmooth = 0.1f;

	public Pawn Pawn;
	public Camera Camera { get; private set; }

	Vector3 _followPositon;
	Vector3 _followVelocity;
	Vector2 _lookAngle;
	Vector2 _lookVelocity;

	void Awake()
	{
		Camera = GetComponent<Camera>();
    }

	public void ResetView()
	{
		if (Pawn)
		{
			_followPositon = Pawn.transform.position;
			_followVelocity = Vector3.zero;
			_lookAngle = Pawn.InputLook;
			_lookVelocity = Vector2.zero;

			UpdateView();
        }
	}

	void LateUpdate()
	{
		if (!Pawn || Time.timeScale <= 0f)
			return;

		_followPositon = Vector3.SmoothDamp(_followPositon, Pawn.transform.position, ref _followVelocity, FollowSmooth);
		_lookAngle.x = Mathf.SmoothDampAngle(_lookAngle.x, Pawn.InputLook.x, ref _lookVelocity.x, LookSmooth);
		_lookAngle.y = Mathf.SmoothDampAngle(_lookAngle.y, Pawn.InputLook.y, ref _lookVelocity.y, LookSmooth);

		UpdateView();
    }

	void UpdateView()
	{
		var xRot = Quaternion.Euler(-_lookAngle.y, 0f, 0f);
        var yRot = Quaternion.Euler(0f, _lookAngle.x, 0f);

		var offset = Quaternion.Euler(Mathf.Clamp(-_lookAngle.y, 0f, 90f), 0f, 0f) * AttachOffset;

		transform.position = _followPositon + yRot * offset;
		transform.rotation = yRot * xRot;
	}
}
