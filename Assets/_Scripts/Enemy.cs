using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] float maxHealth = 2f;
    [SerializeField] int currencyOnDrop = 20;
    private float health;
    [SerializeField] float SpawnHeightAdjustment = 0f;
    private Waypoint currentWaypoint;
    private Vector3 currentTargetPosition;
    private float moveSpeed = 0f;
    private EnemyHealthBarController healthBarController;
    private PlayerController playerController;

    // Start is called before the first frame update
    void Start()
    {
        AdjustStartPosition();
        healthBarController = GetComponent<EnemyHealthBarController>();
        playerController = GameObject.FindObjectOfType<PlayerController>();
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
    public void SetCurrentWaypoint(Waypoint waypoint) {
        currentWaypoint = waypoint;
        if(currentWaypoint != null){
            currentTargetPosition = currentWaypoint.point + new Vector3(0, SpawnHeightAdjustment, 0);
        }
    }

    public void AdjustWaypointTargetPosition(){
        var offset = Random.insideUnitSphere;
        offset.Scale(new Vector3(0.25f,0.25f,0.25f));
        currentTargetPosition = currentTargetPosition + offset;
    }

    public void SetMoveSpeed(float speed) { moveSpeed = speed; }

    private void AdjustStartPosition() {
        transform.position = new Vector3(transform.position.x, SpawnHeightAdjustment, transform.position.z);
    }

    private void MoveTowardsWaypoint(){
        	var currentPos = transform.position;
			var targetPos = currentTargetPosition;

			var moveThisFrame = moveSpeed * Time.deltaTime;
			transform.position = Vector3.MoveTowards(currentPos, targetPos, moveThisFrame);

			if( Vector3.Distance(currentPos,targetPos) < 0.1f) {
				SetCurrentWaypoint(currentWaypoint.GetNextWaypoint());
                AdjustWaypointTargetPosition();
			}
    }

    private void HandlePathComplete() {
        playerController.TakeDamage(10);
        Destroy(gameObject);
    }

    private void Die() {
        playerController.RewardCurrency(currencyOnDrop);
        Destroy(gameObject);
    }

}
