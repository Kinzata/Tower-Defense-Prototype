using System.Collections;
using System.Collections.Generic;
using Kinzata.TowerDefense.Util;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    public bool isDeveloperToolsEnabled = false;

    [Header("Tile / Selection")]
    [SerializeField] GameObject SelectedOverlayPrefab;
    private GameObject selectedOverlay;

    [Header("Tower Placement")]
    private TowerInventoryController towerInventoryController;
    [SerializeField] TowerWeapon WeaponPrefab;

    [Header("Player Resources")]
    [SerializeField] int CollectedCurrency = 500;

    private PlaceableTile selectedTile;
    private Camera mainCamera;
    // Start is called before the first frame update
    void Start()
    {
        mainCamera = Camera.main;
        selectedOverlay = Instantiate(SelectedOverlayPrefab, Vector3.zero, Quaternion.identity);
        selectedOverlay.SetActive(false);

        towerInventoryController = GameObject.FindObjectOfType<TowerInventoryController>();
    }

    // Update is called once per frame
    void Update()
    {
        // Disable PlayerController Input events when developer tools is enabled, to change behavior - Probably a better way to do this
        if(isDeveloperToolsEnabled) { return; }

        var selectTowerButtonDown = Input.GetButtonUp("LeftClick");
        if( selectTowerButtonDown ) {
            SelectTile();
        }
    }

    public void SelectTile(){
        var objectSelected = GetClickedGameObject();
        if( objectSelected == null ) {
            DeselectTile();
            return;
        }

        var tile = objectSelected.GetComponent<PlaceableTile>();
        if(tile) {
            DeselectTile();
            selectedTile = tile;
            selectedOverlay.transform.position = selectedTile.GetPlacementPosition() + new Vector3(0, 0.0002f, 0);
            selectedOverlay.SetActive(true);
        }
    }

    public void PlaceTower(){
        if( selectedTile == null || !towerInventoryController.CanBuyTower(CollectedCurrency)) { return; }
        else {
            var costOfTower = towerInventoryController.GetCurrentTowerCost();
            towerInventoryController.BuildSelectedTower(selectedTile);
            towerInventoryController.BuyTower();
            CollectedCurrency -= costOfTower;
        }
    }

    public void PlaceTowerWeapon() {
        if( selectedTile == null ) { return; }
        var tower = selectedTile.GetPlacedTower();
        if( tower == null ) { return; }

        tower.BuildWeapon(WeaponPrefab.gameObject);
    }

    private void DeselectTile() {
        if( selectedOverlay == null ) { return; }
        selectedOverlay.SetActive(false);
        selectedTile = null;
    }

    private GameObject GetClickedGameObject(){
        return MouseUtility.SelectGameObjectWithRayCast(mainCamera, Input.mousePosition, 100);
    }
}
