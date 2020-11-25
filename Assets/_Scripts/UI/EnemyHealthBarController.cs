using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealthBarController : MonoBehaviour
{
    public Canvas canvas;
    public GameObject healthSpritePrefab;
    public float healthbarOffset = 0.3f;
    private GameObject healthSprite;
    private HealthBar healthBar;
    // Start is called before the first frame update
    void Start()
    {
        canvas = FindObjectOfType<HealthBarCanvas>().gameObject.GetComponent<Canvas>();
        healthSprite = Instantiate(healthSpritePrefab);
        healthSprite.transform.SetParent(canvas.transform);
        healthBar = healthSprite.GetComponent<HealthBar>();
    }

    public void UpdateHealthBar(float maxHealth, float health){
        var percentHealth = health / maxHealth;
        healthBar.UpdateScaling(percentHealth);
    }

    // Update is called once per frame
    void Update()
    {
        var wantedPos = Camera.main.WorldToScreenPoint (new Vector3(transform.position.x, transform.position.y + healthbarOffset, transform.position.z));
        healthSprite.transform.position = wantedPos;
    }

    void OnDestroy() {
        Destroy(healthSprite);
    }
}
