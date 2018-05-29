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

[RequireComponent(typeof(Rigidbody))]
public class Pawn : MonoBehaviour
{
	public float InitialHealth = 1f;
	public float Speed = 2f;
	public float Acceleration = 100f;
	public float ReverseScale = 0.5f;
	public float JumpForce = 10f;
	public float FlyScale = 0.05f;

	public Collider BottomCollider;

	public float Health { get; private set; }
	public bool IsAlive { get { return Health > 0f; } }

	public bool IsSupported { get; private set; }
	const float SupportTolerance = 0.05f;
	Vector3 _lastSupport;

	Vector2 _inputLook;
	public Vector2 InputLook
	{
		get { return _inputLook; }
		set
		{
			value.y = Mathf.Clamp(value.y, -90f, 90f);
			_inputLook = value;
        }
	}

	Vector2 _inputMove;
	public Vector2 InputMove
	{
		get { return _inputMove; }
		set
		{
			var m = value.magnitude;
			if (m > 1f)
				value /= m;

			_inputMove = value;
        }
	}

	public Rigidbody Rigidbody { get; private set; }

	protected virtual void Awake()
	{
		Rigidbody = GetComponent<Rigidbody>();
		transform.rotation = Quaternion.identity;
    }

	void OnEnable()
	{
		Health = InitialHealth;
    }

	public void Jump()
	{
		if (IsSupported && IsAlive)
			Rigidbody.AddForce(transform.up * JumpForce, ForceMode.Impulse);
    }

   // private Transform test;

	public virtual bool Kill()
	{
        //test = Rigidbody.transform;
		if (Health > 0f)
		{
			Health = 0f;
			Rigidbody.isKinematic = true;
			Rigidbody.detectCollisions = false;
            return true;
		}
		return false;
	}

    public virtual bool Revive()
    {
        //Rigidbody.transform.SetPositionAndRotation(test.position, test.rotation);
        //test = null;
        if (Health == 0f)
        {
            Health = InitialHealth;
            Rigidbody.isKinematic = false;
            Rigidbody.detectCollisions = true;
            return true;
        }
        return false;
    }

    void FixedUpdate()
	{
		if (Time.timeScale <= 0f && IsAlive)
			return;

		if (IsSupported)
			IsSupported = (transform.position - _lastSupport).sqrMagnitude < SupportTolerance;

		ProcessMotion();
	}

	void ProcessMotion()
	{
		if (InputMove == Vector2.zero)
			return;

		var direction = Quaternion.Euler(0f, InputLook.x, 0f) * new Vector3(InputMove.x, 0, InputMove.y >= 0 ? InputMove.y : ReverseScale * InputMove.y);
		if (!IsSupported)
			direction *= FlyScale;

		Rigidbody.AddForce(Rigidbody.GetPushForce(direction, Acceleration, Speed));
	}

	void OnCollisionEnter(Collision collision)
	{
		ProcessCollision(collision);
    }

	void OnCollisionStay(Collision collision)
	{
		ProcessCollision(collision);
	}

	void ProcessCollision(Collision collision)
	{
		foreach (var contact in collision.contacts)
		{
			if (contact.thisCollider == BottomCollider)
			{
				_lastSupport = transform.position;
				IsSupported = true;
				break;
			}
		}
	}
}
