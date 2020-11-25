using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class LevelPath : MonoBehaviour {

	private List<GameObject> waypointObjects;
	private Waypoint[] waypoints;

	// Use this for initialization
	void Start () {
		Debug.Log("Setup: LevelPath - Starting");
		InitializeWaypointObjects();
		waypoints = BuildWaypointArray();
		Debug.Log("Setup: LevelPath - Complete");
	}

	public Waypoint GetWaypointAt(int index) {
		var waypoint = waypoints.ElementAtOrDefault(index);
		return waypoint;
	}

	private void InitializeWaypointObjects(){
		waypointObjects = new List<GameObject>();
		foreach( Transform child in transform) {
			waypointObjects.Add(child.gameObject);
		}
	}
	
	private Waypoint[] BuildWaypointArray() {
		return waypointObjects
		.OrderBy(obj => obj.name)
		.Select((obj, index) => new Waypoint(index, obj.transform.position, this))
		.ToArray();	
	} 
}