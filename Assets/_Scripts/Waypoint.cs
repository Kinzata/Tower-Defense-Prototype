using UnityEngine;

public class Waypoint
{
    private int index;
    public Vector3 point;

    LevelPath levelPath;

    public Waypoint(int index, Vector3 point, LevelPath levelPath)
    {
        this.index = index;
        this.point = point;
        this.levelPath = levelPath;
    }

    public Waypoint GetNextWaypoint()
    {
        return levelPath.GetWaypointAt(index + 1);
    }
}
