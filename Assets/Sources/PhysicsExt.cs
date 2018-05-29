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

public static class PhysicsExt
{
	public static Vector3 GetPushForce(this Rigidbody rigidbody, Vector3 moveDirection, float accelerationForce, float maxVelocity)
	{
		var velocity = Vector3.Dot(rigidbody.velocity, moveDirection.normalized);
		if (velocity > maxVelocity)
			return Vector3.zero;

		var dt = Time.fixedDeltaTime;

		var deltaVelocity = dt * accelerationForce / rigidbody.mass;
		if (velocity + deltaVelocity > maxVelocity)
			deltaVelocity = maxVelocity - velocity;

		var deltaForce = rigidbody.mass * deltaVelocity / dt;
		return deltaForce * moveDirection;
	}
}
