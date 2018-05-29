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

public class CameraManager : MonoBehaviour
{
	Animator _introAnimator;
	Camera _mainCamera;
	PawnCamera _pawnCamera;
	
	void Awake()
	{
		_introAnimator = GetComponentInChildren<Animator>();
		_mainCamera = Camera.main;
		_pawnCamera = _mainCamera.GetComponent<PawnCamera>();
		
		_pawnCamera.ResetView();
        _introAnimator.transform.position = _pawnCamera.transform.position;
		_introAnimator.transform.rotation = _pawnCamera.transform.rotation;

		_mainCamera.gameObject.SetActive(false);
    }

	public void OnAnimationEnd()
	{
		_mainCamera.gameObject.SetActive(true);
		_introAnimator.gameObject.SetActive(false);
    }
}
