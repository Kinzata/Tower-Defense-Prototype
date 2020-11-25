using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Tower Data Config")]
public class TowerDataConfig : ScriptableObject {

	[SerializeField] GameObject TowerPrefab;
	[SerializeField] float RangeModifier = 1f;
	[SerializeField] float DamageModifier = 1f;
	[SerializeField] float ReloadModifier = 1f;

	public GameObject GetTowerPrefab() { return TowerPrefab; }

	public float GetRangeModifier() { return RangeModifier; }

	public float GetDamageModifier() { return DamageModifier; }

	public float GetReloadModifier() { return ReloadModifier; }

    public TowerData CreateTowerDataFromConfig() {
        return new TowerData(
            GetRangeModifier(),
            GetDamageModifier(),
            GetReloadModifier()
        );
    }

}
