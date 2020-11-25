using UnityEngine;
using UnityEngine.Events;
using System.Collections.Generic;

public class ArcedProjectile : Projectile {

	[Tooltip("How high the arc should be, in units")]
	public float arcHeight = 1;

	public override void ChildStart() {

	}

	public override void ChildUpdate() {
		if( !hasBeenFired || target == null ){ return; }
		// Compute the next position, with arc added in
		float x0 = startPos.x;
		float x1 = target.transform.position.x;
        float z0 = startPos.z;
		float z1 = target.transform.position.z;

		// Something something this allows it to arc...
        float distanceBetweenStartAndTarget = Mathf.Sqrt(Mathf.Pow(x1 - x0, 2) + Mathf.Pow(z1 - z0, 2));
        var next = Vector3.MoveTowards(transform.position, target.transform.position, speed * Time.deltaTime);

        float movedDistance = Mathf.Sqrt(Mathf.Pow(next.x - x0, 2) + Mathf.Pow(next.z - z0, 2));
		float distanceFromTarget = Mathf.Sqrt(Mathf.Pow(transform.position.x - x1, 2) + Mathf.Pow(transform.position.z - z1, 2));
		float baseY = Mathf.Lerp(startPos.y, target.transform.position.y, (distanceBetweenStartAndTarget - distanceFromTarget) / distanceBetweenStartAndTarget);

        float remainingDistance = Mathf.Sqrt(Mathf.Pow(next.x - x1, 2) + Mathf.Pow(next.z - z1, 2));
		float arc = arcHeight * movedDistance * remainingDistance / ( distanceBetweenStartAndTarget * distanceBetweenStartAndTarget);
		var nextPos = new Vector3(next.x, baseY + arc , next.z);

		// Rotate to face the next position, and then move there
		transform.rotation = Quaternion.LookRotation(nextPos - transform.position);
		transform.position = nextPos;
	}

	public void OnCollisionEnter(Collision other) {
		var enemy = other.gameObject.GetComponent<Enemy>();
		enemy.Hit(projectileDamage);
		Arrived();
	}

	void Arrived() {
		Destroy(gameObject);
	}
}