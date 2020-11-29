using TMPro;
using UnityEngine;

public class CurrencyDisplay : MonoBehaviour
{

    private TextMeshProUGUI currencyText;

    private string currencyPrefix = "Coins: ";


    // Start is called before the first frame update
    void Start()
    {
        currencyText = gameObject.GetComponent<TextMeshProUGUI>();
        currencyText.text = currencyPrefix + "0000";
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void UpdateText(int currency){
        currencyText.text = currencyPrefix + currency.ToString();
    }
}
