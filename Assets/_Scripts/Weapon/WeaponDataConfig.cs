using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Weapon Data Config")]
public class WeaponDataConfig : ScriptableObject {

	[SerializeField] GameObject WeaponPrefab;
	[SerializeField] float RangeModifier = 1f;
	[SerializeField] float DamageModifier = 1f;
	[SerializeField] float ReloadModifier = 1f;

	public GameObject GetWeaponPrefab() { return WeaponPrefab; }

	public float GetRangeModifier() { return RangeModifier; }

	public float GetDamageModifier() { return DamageModifier; }

	public float GetReloadModifier() { return ReloadModifier; }

    public WeaponData CreateWeaponDataFromConfig() {
        return new WeaponData(
            GetRangeModifier(),
            GetDamageModifier(),
            GetReloadModifier()
        );
    }

}
