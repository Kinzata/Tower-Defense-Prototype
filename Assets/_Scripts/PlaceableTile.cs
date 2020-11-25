﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaceableTile : MonoBehaviour
{
    private Tower placedTower;

    private bool IsSelected = false;
    [SerializeField] Vector3 placementPositionAdjustment = new Vector3(0,0.2f,0);

    // Start is called before the first frame update
    void Start()
    {
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

    void OnDrawGizmos()
    {
        var drawPosition = gameObject.transform.position + placementPositionAdjustment;
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(drawPosition, 0.05f);
    }
}
