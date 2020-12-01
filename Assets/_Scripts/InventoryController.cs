using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryController : MonoBehaviour
{

    [SerializeField] TowerDataConfig[] towerDataConfigurations = new TowerDataConfig[0];
    [SerializeField] WeaponDataConfig[] weaponDataConfigurations = new WeaponDataConfig[0];

    [SerializeField] int BaseTowerCost = 100;
    [SerializeField] float TowerCostInflation = 1.1f;
    private int CurrentTowerCost = 0;
    [SerializeField] int BaseWeaponCost = 100;
    [SerializeField] float WeaponsCostInflation = 1.1f;
    private int CurrentWeaponCost = 0;


    void Start()
    {
        CurrentTowerCost = BaseTowerCost;
        CurrentWeaponCost = BaseWeaponCost;
    }

    public int GetCurrentTowerCost(){
        return CurrentTowerCost;
    }

    public bool CanBuyTower(int currency) {
        return CurrentTowerCost < currency;
    }

    public void BuyTower() {
        CurrentTowerCost = (int)(CurrentTowerCost * TowerCostInflation);
    }

    public TowerDataConfig GetSelectedTowerDataConfig() {
        if( towerDataConfigurations == null || towerDataConfigurations.Length == 0 )
        {
            return null;
        }

        // Mocking the selection of a tower for the time being.  This will eventually be chosen via the UI.
        return towerDataConfigurations[0];
    }

    public void BuildSelectedTower(PlaceableTile tile){
        var towerDataConfig = GetSelectedTowerDataConfig();

        if( towerDataConfig == null ){
            Debug.Log("TowerData not configured!  Skipping place tower.");
            return;
        }

        var tower = tile.PlaceTower(towerDataConfig.GetTowerPrefab());
        if( tower != null ){
            var towerComponent = tower.GetComponent<Tower>();
            towerComponent.SetTowerData(towerDataConfig.CreateTowerDataFromConfig());
        }
    }

     public int GetCurrentWeaponCost(){
        return CurrentWeaponCost;
    }

    public bool CanBuyWeapon(int currency) {
        return CurrentWeaponCost < currency;
    }

    public void BuyWeapon() {
        CurrentWeaponCost = (int)(CurrentWeaponCost * WeaponsCostInflation);
    }

    public WeaponDataConfig GetSelectedWeaponDataConfig() {
        if( weaponDataConfigurations == null || weaponDataConfigurations.Length == 0 )
        {
            return null;
        }

        // Mocking the selection of a tower for the time being.  This will eventually be chosen via the UI.
        return weaponDataConfigurations[0];
    }

    public void BuildSelectedWeapon(Tower tower){
        var weaponDataConfig = GetSelectedWeaponDataConfig();

        if( weaponDataConfig == null ){
            Debug.Log("WeaponData not configured!  Skipping place weapon.");
            return;
        }

        tower.BuildWeapon(weaponDataConfig.GetWeaponPrefab());
    }

}
