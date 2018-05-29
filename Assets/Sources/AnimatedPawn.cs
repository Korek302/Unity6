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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatedPawn : Pawn
{
	public float RotateSmooth = 0.1f;
	
	public Animator Animator { get; private set; }

	Rigidbody[] _limbs;
	float _rotateVelocity;

    private List<Vector3> limbPositionList;
    private List<Quaternion> limbRotationList;

	protected override void Awake()
	{
		base.Awake();

		Animator = GetComponentInChildren<Animator>();
		_limbs = Animator.GetComponentsInChildren<Rigidbody>();
        limbPositionList = new List<Vector3>();
        limbRotationList = new List<Quaternion>();
    }

	void Update()
	{
		if (!IsAlive)
			return;

		Animator.transform.eulerAngles = 
			new Vector3(0f, Mathf.SmoothDampAngle(Animator.transform.eulerAngles.y, InputLook.x, ref _rotateVelocity, RotateSmooth), 0f);
        
		Animator.SetFloat("MotionBlendX", IsSupported ? InputMove.x : 0f);
        Animator.SetFloat("MotionBlendY", IsSupported ? InputMove.y : 0f);
		Animator.SetFloat("MotionSpeed", Mathf.Sign(InputMove.y));
    }

    public override bool Kill()
	{
		var velocity = Rigidbody.velocity;

        if (base.Kill())
		{
			Animator.enabled = false;
			//Animator.transform.parent = null;

			foreach (var limb in _limbs)
			{
                limbPositionList.Add(limb.gameObject.transform.localPosition);
                limbRotationList.Add(limb.gameObject.transform.localRotation);

                limb.GetComponent<Collider>().enabled = true;
				limb.isKinematic = false;
				limb.useGravity = true;
				limb.interpolation = RigidbodyInterpolation.Interpolate;
				limb.velocity = velocity;
            }

			return true;
		}

		return false;
	}

    public override bool Revive()
    {
        var velocity = Rigidbody.velocity;

        if (base.Revive())
        {
            Animator.enabled = true;
            StartCoroutine("MoveLimbs");

            return true;
        }

        return false;
    }

    IEnumerator MoveLimbs()
    {
        float time = 0;
        while (time <= 1)
        {
            int i = 0;
            foreach (Rigidbody limb in _limbs)
            {
                limb.useGravity = false;
                limb.interpolation = RigidbodyInterpolation.None;
                limb.GetComponent<Collider>().enabled = false;
                limb.isKinematic = true;
                limb.transform.localPosition = Vector3.Lerp(limb.transform.localPosition, limbPositionList[i], time);
                limb.transform.localRotation = Quaternion.Lerp(limb.transform.localRotation, limbRotationList[i], time);
                i++;
            }
            time += 0.01f;
            yield return new WaitForSeconds(0.02f);
        }
    }
}
