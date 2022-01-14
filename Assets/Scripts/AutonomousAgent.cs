using Assets.Scripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutonomousAgent : Agent
{
	[SerializeField] Perception flockPerception;
	[SerializeField] Perception perception;
	[SerializeField] ObstaclePerception obstaclePerception;
	[SerializeField] Steering steering;
	[SerializeField] AutonomousAgentData AgentData;

	public float maxSpeed { get { return AgentData.maxSpeed; } }
	public float maxForce { get { return AgentData.maxForce; } }

	public Vector3 velocity { get; set; } = Vector3.zero;

	// Update is called once per frame
	void Update()
	{
		Vector3 acceleration = Vector3.zero;

		GameObject[] gameObjects = perception.GetGameObjects();
		if (gameObjects.Length != 0)
		{
			Vector3 force = Vector3.zero;
			force += steering.Flee(this, gameObjects[0]) * AgentData.fleeWeight;
			force += steering.Seek(this, gameObjects[0]) * AgentData.seekWeight;
			acceleration += force;
		}
		else
		{
			acceleration += steering.Wander(this);
		}
		// obstacle avoidance
		if (obstaclePerception.IsObstacleInFront())
		{
			Vector3 direction = obstaclePerception.GetOpenDirection();
			acceleration += steering.CalculateSteering(this, direction) * AgentData.obstacleWeight;
		}

		gameObjects = flockPerception.GetGameObjects();
		if (gameObjects.Length != 0)
		{
			acceleration += steering.Cohesion(this, gameObjects) * AgentData.cohesionWeight;
			//acceleration += steering.Separation(this, gameObjects) * AgentData.separationWeight;
			//acceleration += steering.Alignment(this, gameObjects) * AgentData.alignmentWeight;
		}

		velocity += acceleration * Time.deltaTime;

		velocity = Vector3.ClampMagnitude(velocity, maxSpeed);

		transform.position = transform.position + velocity * Time.deltaTime;

		if (velocity.sqrMagnitude > 0.1f)
		{
			transform.rotation = Quaternion.LookRotation(velocity);
		}
		transform.position = Utilities.Wrap(transform.position, new Vector3(-10, -10, -10), new Vector3(10, 10, 10));
	}
}
