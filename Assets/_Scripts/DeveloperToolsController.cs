using System.Collections;
using System.Collections.Generic;
using Kinzata.TowerDefense.Util;
using UnityEngine;

public class DeveloperToolsController : MonoBehaviour
{
    // This is a temp tower for testing
    [SerializeField] GameObject TowerPrefab;

    private PlayerController playerController;
    private Camera mainCamera;
    [SerializeField] bool isDeveloperToolsEnabled = false;

    // Start is called before the first frame update
    void Start()
    {
        mainCamera = Camera.main;
        playerController = FindObjectOfType<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKey(KeyCode.LeftShift) && Input.GetKeyDown(KeyCode.BackQuote)){ ToggleDeveloperTools(); }
        if( !isDeveloperToolsEnabled ) { return; }

        var placeTowerButtonDown = Input.GetButtonUp("LeftClick");
        if( placeTowerButtonDown ) {
            PlaceTower();
        }
    }

    private void ToggleDeveloperTools() {
        isDeveloperToolsEnabled = !isDeveloperToolsEnabled;
        playerController.isDeveloperToolsEnabled = isDeveloperToolsEnabled;
        Debug.Log($"Developer tools set to {isDeveloperToolsEnabled}!");
    }

    private void PlaceTower(){
        var objectSelected = GetClickedGameObject();
        if( objectSelected == null ) { return; }

        var placeableTile = objectSelected.GetComponent<PlaceableTile>();
        if(placeableTile) {
            placeableTile.PlaceTower(TowerPrefab);
        }
    }

    private GameObject GetClickedGameObject(){
        return MouseUtility.SelectGameObjectWithRayCast(mainCamera, Input.mousePosition, 100);
    }
}
