using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerWeapon : MonoBehaviour
{
    private Enemy targetEnemy;
    [SerializeField] Projectile projectilePrefab;
    [SerializeField] Vector3 projectilePosition;
    private Projectile projectile;

    private TowerData towerData;
    private WeaponData weaponData;
    [SerializeField] float weaponRange = 2f;
    [SerializeField] float reloadSpeed = 3f;
    [SerializeField] float weaponDamage = 1f;
    private float reloadCountDown = 0f;

    void Start()
    {
        projectile = GetComponentInChildren<Projectile>();
        reloadCountDown = GetWeaponFireRate();
    }

    void Update()
    {
        // Aim and Fire at target, or retarget if no target
        if (targetEnemy == null) { Retarget(); }
        else
        {
            AimAtTarget();
            if (projectile)
            {
                FireAtTarget();
            }
        }

        // Reload only once timer is complete and we don't already have a projectile
        if (projectile == null)
        {
            if (GetWeaponFireRate() % reloadCountDown == 0)
            {
                var iPosition = transform.position;
                projectile = Instantiate(projectilePrefab, iPosition, transform.rotation, transform) as Projectile;
                projectile.transform.localPosition = projectilePosition;
            }
            else
            {
                reloadCountDown += Time.deltaTime;
                reloadCountDown = Mathf.Min(reloadCountDown, GetWeaponFireRate());
            }
        }
    }

    public void SetTowerData(TowerData data)
    {
        towerData = data;
    }

    public void SetWeaponData(WeaponData data)
    {
        weaponData = data;
    }

    public float GetWeaponRange()
    {
        return weaponRange * (towerData?.GetRangeModifier() ?? 1f);
    }

    public float GetWeaponDamage()
    {
        return weaponDamage * (towerData?.GetDamageModifier() ?? 1f);
    }

    public float GetWeaponFireRate()
    {
        return reloadSpeed * (towerData?.GetReloadModifier() ?? 1f);
    }
    private void FireAtTarget()
    {
        if (!isCurrentTargetInRange()) { targetEnemy = null; }
        else if (projectile)
        {
            projectile.FireProjectile(GetWeaponDamage());
            projectile = null;
            reloadCountDown = 0f;
        }
    }

    private void AimAtTarget()
    {
        var targetVector = targetEnemy.transform.position - transform.position;

        var rotation = Vector3.RotateTowards(transform.forward, targetVector, 4, 0.0f);
        Debug.DrawRay(transform.position, rotation, Color.red);

        // Move our position a step closer to the target.
        transform.rotation = Quaternion.LookRotation(rotation);
        if (projectile)
        {
            projectile.target = targetEnemy.gameObject;
        }
    }

    private void Retarget()
    {
        var layer = LayerMask.NameToLayer("Enemy");
        var allTargetsInRange = Physics.OverlapSphere(transform.position, GetWeaponRange(), 1 << layer, QueryTriggerInteraction.Collide);
        if (allTargetsInRange.Length > 0)
            targetEnemy = allTargetsInRange[0].gameObject.GetComponent<Enemy>();
    }

    private bool isCurrentTargetInRange()
    {
        if (targetEnemy == null) { return false; }
        var distance = Vector3.Distance(transform.position, targetEnemy.transform.position);
        return distance <= GetWeaponRange();
    }
}
