using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] float maxHealth = 2f;
    private float health;
    public float SpawnHeightAdjustment = 0f;
    private Waypoint currentWaypoint;
    private float moveSpeed = 0f;
    private EnemyHealthBarController healthBarController;

    // Start is called before the first frame update
    void Start()
    {
        AdjustStartPosition();
        healthBarController = GetComponent<EnemyHealthBarController>();
        health = maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        if(currentWaypoint != null) {
            MoveTowardsWaypoint();
        }
        else {
            HandlePathComplete();
        }
    }

    public void Hit(float damage){
        health -= damage;
        if( health <= 0f ) {
            Die();
        }
        healthBarController.UpdateHealthBar(maxHealth, health);
    }
    public void SetCurrentWaypoint(Waypoint waypoint) { currentWaypoint = waypoint; }
    public void SetMoveSpeed(float speed) { moveSpeed = speed; }

    private void AdjustStartPosition() {
        transform.position = new Vector3(transform.position.x, SpawnHeightAdjustment, transform.position.z);
    }

    private void MoveTowardsWaypoint(){
        	var currentPos = transform.position;
			var targetPos = currentWaypoint.point + new Vector3(0, SpawnHeightAdjustment, 0);

			var moveThisFrame = moveSpeed * Time.deltaTime;
			transform.position = Vector3.MoveTowards(currentPos, targetPos, moveThisFrame);

			if( Vector3.Distance(currentPos,targetPos) < 0.1f) {
				currentWaypoint = currentWaypoint.GetNextWaypoint();
			}
    }

    private void HandlePathComplete() {
        Destroy(gameObject);
    }

    private void Die() {
        Destroy(gameObject);
    }
}
