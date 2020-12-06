using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaceableTile : MonoBehaviour
{
    private Tower placedTower;

    private bool isSelected = false;
    [SerializeField] Vector3 placementPositionAdjustment = new Vector3(0,0.2f,0);
    public int materialIndexForHover = 1;
    public Color highlightMaterialColor = new Color();
    private Color originalMaterialColor;
    private MeshRenderer rend;


    // Start is called before the first frame update
    void Start()
    {
        rend = gameObject.GetComponent<MeshRenderer>();
        originalMaterialColor = rend.materials[materialIndexForHover].color;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public Vector3 GetPlacementPosition(){
        return gameObject.transform.position + placementPositionAdjustment;
    }

    public GameObject PlaceTower(GameObject objectToPlace) {
        if( placedTower == null ) {
            var newObject = Instantiate(objectToPlace, GetPlacementPosition(), Quaternion.identity);
            placedTower = newObject.GetComponent<Tower>();
            return newObject;
        }
        else return null;
    }

    public Tower GetPlacedTower(){
        return placedTower;
    }

    public void OnMouseEnter(){
        rend.materials[materialIndexForHover].color = highlightMaterialColor;
    }

    public void OnMouseExit(){
        rend.materials[materialIndexForHover].color = originalMaterialColor;
    }

    void OnDrawGizmos()
    {
        var drawPosition = gameObject.transform.position + placementPositionAdjustment;
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(drawPosition, 0.05f);
    }
}
