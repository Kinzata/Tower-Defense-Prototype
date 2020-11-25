using UnityEngine;

namespace Kinzata.TowerDefense.Util
{
    public static class MouseUtility
    {
        public static GameObject SelectGameObjectWithRayCast(Camera camera, Vector3 mousePosition, float distance)
        {
            Ray ray = camera.ScreenPointToRay( mousePosition );
            RaycastHit hit;
        
            if( Physics.Raycast( ray, out hit, distance ) )
            {
                return hit.transform.gameObject;
            }

            return null;
        }
    }
}

