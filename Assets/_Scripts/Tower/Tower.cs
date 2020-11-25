using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tower : MonoBehaviour
{
    public TowerWeapon towerWeapon;
    public Vector3 towerWeaponPlacementPositionAdjustment = Vector3.zero;

    private TowerData towerData;

    public void SetTowerData(TowerData data)
    {
        towerData = data;
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public TowerWeapon GetTowerWeapon() {
        return towerWeapon;
    }

    public void BuildWeapon(GameObject weaponToBuild) {
        if( GetTowerWeapon() ) { return; }

        towerWeapon = Instantiate(weaponToBuild, transform.position + (towerWeaponPlacementPositionAdjustment * gameObject.transform.localScale.y), Quaternion.identity).GetComponent<TowerWeapon>();
        var scale = towerWeapon.transform.localScale;
        towerWeapon.gameObject.transform.parent = gameObject.transform;
        towerWeapon.transform.localScale = scale;

        towerWeapon.SetTowerData(towerData);
    }

    void OnDrawGizmos()
    {
        var drawPosition = gameObject.transform.position + (towerWeaponPlacementPositionAdjustment * gameObject.transform.localScale.y);
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(drawPosition, 0.05f);
    }
}
