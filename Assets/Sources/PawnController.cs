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
using UnityEngine.SceneManagement;

public class PawnController : MonoBehaviour
{
	public Vector2 Sensitivity = new Vector2(1f, 1f);
	public Pawn Pawn;

	void Update()
	{
		ProcessInput();
	}

	void ProcessInput()
	{
		if (!Pawn)
			return;

		Pawn.InputLook += Vector2.Scale(Sensitivity, new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y")));
		Pawn.InputMove = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));

		if (Input.GetButtonDown("Jump"))
			Pawn.Jump();

		if (Input.GetButtonDown("Fire1"))
			Pawn.Kill();

        if (Input.GetKeyDown(KeyCode.F))
            Pawn.Revive();

        if (Input.GetKeyDown(KeyCode.R))
        {
			int scene = SceneManager.GetActiveScene().buildIndex;
			SceneManager.LoadScene(scene, LoadSceneMode.Single);
        }
    }
}
