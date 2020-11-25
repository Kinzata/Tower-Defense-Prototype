using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerData
{
    public TowerData(float rangeModifier, float damageModifier, float reloadModifier)
    {
        this.rangeModifier = rangeModifier;
        this.damageModifier = damageModifier;
        this.reloadModifier = reloadModifier;
    }

    protected float rangeModifier = 1f;
    public float GetRangeModifier() { return rangeModifier; }
    public void SetRangeModifier(float value) { rangeModifier = value; }

    protected float damageModifier = 1f;
    public float GetDamageModifier() { return damageModifier; }
    public void SetDamageModifier(float value) { damageModifier = value; }

    protected float reloadModifier = 1f;
    public float GetReloadModifier() { return reloadModifier; }
    public void SetReloadModifier(float value) { reloadModifier = value; }
}
