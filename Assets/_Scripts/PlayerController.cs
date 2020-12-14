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

    [Header("Controls")]
    public float scrollWheelScale = .3f;
    public float scrollScale = .05f;
    public float rotateScale = 2f;

    private PlayerBase PlayerBase;

    private Camera mainCamera;
    private GameObject InGameUI;
    private GameObject GameOverUI;

    void Start()
    {
        mainCamera = Camera.main;
        selectedOverlay = Instantiate(SelectedOverlayPrefab, Vector3.zero, Quaternion.identity);
        selectedOverlay.SetActive(false);

        inventoryController = GameObject.FindObjectOfType<InventoryController>();
        PlayerBase = GameObject.FindObjectOfType<PlayerBase>();

        if (currencyDisplay == null)
        {
            currencyDisplay = GameObject.FindObjectOfType<CurrencyDisplay>();
        }
        currencyDisplay.UpdateText(CollectedCurrency);

        InGameUI = GameObject.FindGameObjectWithTag("InGameUI");
        InGameUI.SetActive(true);
        GameOverUI = GameObject.FindGameObjectWithTag("GameOverUI");
        GameOverUI.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        // Disable PlayerController Input events when developer tools is enabled, to change behavior - Probably a better way to do this
        if (isDeveloperToolsEnabled) { return; }

        var selectTowerButtonDown = Input.GetButtonUp("LeftClick");
        if (selectTowerButtonDown)
        {
            SelectTile();
        }


        // Camera Controls
        // Zoom In/Out
        if (Input.mouseScrollDelta.y != 0)
        {
            var forward = mainCamera.transform.forward;
            var scrollWheelDelta = Input.mouseScrollDelta.y * scrollWheelScale;
            var translate = forward * scrollWheelDelta;
            mainCamera.transform.Translate(translate, Space.World);
        }
        // Zoom In
        if (Input.GetKey(KeyCode.W))
        {
            var forward = mainCamera.transform.forward;
            var translate = forward * scrollScale;
            mainCamera.transform.Translate(translate, Space.World);
        }
        // Zoom Out
        if (Input.GetKey(KeyCode.S))
        {
            var backward = mainCamera.transform.forward * -1;
            var translate = backward * scrollScale;
            mainCamera.transform.Translate(translate, Space.World);
        }
        // Left
        if (Input.GetKey(KeyCode.A))
        {
            var left = mainCamera.transform.right * -1;
            var translate = left * scrollScale;
            mainCamera.transform.Translate(translate, Space.World);
        }
        // Right
        if (Input.GetKey(KeyCode.D))
        {
            var right = mainCamera.transform.right;
            var translate = right * scrollScale;
            mainCamera.transform.Translate(translate, Space.World);
        }
        // Up
        if (Input.GetKey(KeyCode.E))
        {
            var up = mainCamera.transform.up;
            var translate = up * scrollScale;
            mainCamera.transform.Translate(translate, Space.World);
        }
        // Down
        if (Input.GetKey(KeyCode.Q))
        {
            var down = mainCamera.transform.up * -1;
            var translate = down * scrollScale;
            mainCamera.transform.Translate(translate, Space.World);
        }
        // Rotate
        if (Input.GetMouseButton(1))
        {
            var mouseX = Input.GetAxisRaw("Mouse X") * rotateScale;
            var mouseY = Input.GetAxisRaw("Mouse Y") * -rotateScale;
            mainCamera.transform.Rotate(new Vector3(mouseY, 0, 0), Space.Self);
            mainCamera.transform.Rotate(new Vector3(0, mouseX, 0), Space.World);
        }
    }

    public void SelectTile()
    {
        var objectSelected = GetClickedGameObject();
        if (objectSelected == null)
        {
            DeselectTile();
            return;
        }

        var tile = objectSelected.GetComponent<PlaceableTile>();
        if (tile)
        {
            DeselectTile();
            selectedTile = tile;
            selectedOverlay.transform.position = selectedTile.GetPlacementPosition() + new Vector3(0, 0.0002f, 0);
            selectedOverlay.SetActive(true);
        }
    }

    public void PlaceTower()
    {
        if (selectedTile == null || !inventoryController.CanBuyTower(CollectedCurrency)) { return; }
        else
        {
            var costOfTower = inventoryController.GetCurrentTowerCost();
            inventoryController.BuildSelectedTower(selectedTile);
            inventoryController.BuyTower();
            CollectedCurrency -= costOfTower;
            currencyDisplay.UpdateText(CollectedCurrency);
        }
    }

    public void PlaceTowerWeapon()
    {
        if (selectedTile == null || !inventoryController.CanBuyWeapon(CollectedCurrency)) { return; }
        var tower = selectedTile.GetPlacedTower();
        if (tower == null) { return; }

        var costOfWeapon = inventoryController.GetCurrentWeaponCost();
        inventoryController.BuildSelectedWeapon(tower);
        inventoryController.BuyWeapon();
        CollectedCurrency -= costOfWeapon;
        currencyDisplay.UpdateText(CollectedCurrency);
    }

    public void RewardCurrency(int rewardValue)
    {
        CollectedCurrency += rewardValue;
        currencyDisplay.UpdateText(CollectedCurrency);
    }

    public void TakeDamage(int damage)
    {
        BaseHealth -= damage;

        if (BaseHealth <= 0 && PlayerBase != null)
        {
            Destroy(PlayerBase.gameObject);
            GameOver();
        }
    }

    private void GameOver()
    {
        InGameUI.SetActive(false);
        GameOverUI.SetActive(true);
    }

    private void DeselectTile()
    {
        if (selectedOverlay == null) { return; }
        selectedOverlay.SetActive(false);
        selectedTile = null;
    }

    private GameObject GetClickedGameObject()
    {
        return MouseUtility.SelectGameObjectWithRayCast(mainCamera, Input.mousePosition, 100);
    }
}
