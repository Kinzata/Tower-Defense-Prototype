using UnityEngine;
using System.Linq;
using System.Collections.Generic;

public class LevelPath : MonoBehaviour
{
    private Waypoint[] waypoints;

    // Use this for initialization
    void Start()
    {
    }

    public void SetupWaypoints(Vector3[] points)
    {
		var waypointsList = new List<Waypoint>();
        for (int i = 0; i < points.Length; i++)
        {
            var waypoint = new Waypoint(i, points[i], this);
			waypointsList.Add(waypoint);
		}
		waypoints = waypointsList.ToArray();
    }

    public Waypoint GetWaypointAt(int index)
    {
        var waypoint = waypoints.ElementAtOrDefault(index);
        return waypoint;
    }

    void OnDrawGizmos()
    {
        for (int i = 0; i < waypoints.Length; i++)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawSphere(waypoints[i].point, 0.05f);
        }
    }
}