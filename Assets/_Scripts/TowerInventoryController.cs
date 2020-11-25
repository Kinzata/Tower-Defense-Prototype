using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerInventoryController : MonoBehaviour
{

    [SerializeField] TowerDataConfig[] towerDataConfigurations = new TowerDataConfig[0];

    [SerializeField] int BaseTowerCost = 100;
    [SerializeField] int BaseWeaponCost = 100;


    void Start()
    {

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

}
