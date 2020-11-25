using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class HealthBar : MonoBehaviour
{
    private float startingWidth;
    private Image image;
    private RectTransform rect;
    // Start is called before the first frame update
    void Start()
    {
        image = GetComponentInChildren<Image>();
        rect = image.gameObject.GetComponent<RectTransform>();
        startingWidth = rect.rect.width;
        gameObject.SetActive(false);
    }

    public void UpdateScaling(float percentHealth) {
        var width = startingWidth * percentHealth;
        rect.sizeDelta = new Vector2(width, rect.rect.height);
        if( percentHealth < 1f ) { gameObject.SetActive(true);}
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
