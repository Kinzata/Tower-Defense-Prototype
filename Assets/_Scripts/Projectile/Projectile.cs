using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [Tooltip("Position we want to hit")]
	public GameObject target;
    protected Vector3 startPos;
	
	[Tooltip("Horizontal speed, in units/sec")]
	public float speed = 10;

    protected bool hasBeenFired = false;
    protected float projectileDamage = 1f;

    // Start is called before the first frame update
    void Start()
    {
        ChildStart();
    }

    // Update is called once per frame
    void Update()
    {
        if( hasBeenFired && target == null) { Destroy(gameObject); }
        ChildUpdate();
    }

    public void FireProjectile(float damage) {
		startPos = transform.position;
        hasBeenFired = true;
        gameObject.transform.parent = null;
        projectileDamage = damage;
    }


    public virtual void ChildStart() {
        // Override in child class
    }
    public virtual void ChildUpdate() {
        // Override in child class
    }
}
