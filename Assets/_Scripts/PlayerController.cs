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
    private PlaceableTile selectedTile;

    [Header("Tower Placement")]
    [SerializeField] TowerWeapon WeaponPrefab;
    private InventoryController inventoryController;

    [Header("Player Resources")]
    [SerializeField] CurrencyDisplay currencyDisplay;
    [SerializeField] int CollectedCurrency = 500;
    [SerializeField] int BaseHealth = 100;

    private PlayerBase PlayerBase;

    private Camera mainCamera;

    void Start()
    {
        mainCamera = Camera.main;
        selectedOverlay = Instantiate(SelectedOverlayPrefab, Vector3.zero, Quaternion.identity);
        selectedOverlay.SetActive(false);

        inventoryController = GameObject.FindObjectOfType<InventoryController>();
        PlayerBase = GameObject.FindObjectOfType<PlayerBase>();

        currencyDisplay.UpdateText(CollectedCurrency);
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
        if( selectedTile == null || !inventoryController.CanBuyTower(CollectedCurrency)) { return; }
        else {
            var costOfTower = inventoryController.GetCurrentTowerCost();
            inventoryController.BuildSelectedTower(selectedTile);
            inventoryController.BuyTower();
            CollectedCurrency -= costOfTower;
            currencyDisplay.UpdateText(CollectedCurrency);
        }
    }

    public void PlaceTowerWeapon() {
        if( selectedTile == null || !inventoryController.CanBuyWeapon(CollectedCurrency)) { return; }
        var tower = selectedTile.GetPlacedTower();
        if( tower == null ) { return; }

        var costOfWeapon = inventoryController.GetCurrentWeaponCost();
        inventoryController.BuildSelectedWeapon(tower);
        inventoryController.BuyWeapon();
        CollectedCurrency -= costOfWeapon;
        currencyDisplay.UpdateText(CollectedCurrency);
    }

    public void RewardCurrency(int rewardValue){
        CollectedCurrency += rewardValue;
        currencyDisplay.UpdateText(CollectedCurrency);
    }

    public void TakeDamage(int damage){
        BaseHealth -= damage;

        if( BaseHealth <= 0 ){
            Destroy(PlayerBase.gameObject);

            // End score screen transition
        }
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
