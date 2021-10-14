using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class DataSaver : MonoBehaviour
{
    private static bool created = false;

    [SerializeField] private GameObject moneyValueGO;
    [SerializeField] private GameObject datacentersValueGO;
    private int moneyValue;
    private int datacentersValue;   

    void Awake()
    {
        // Ensure the script is not deleted while loading.
        if (!created)
        {
            DontDestroyOnLoad(this.gameObject);
            created = true;
        }
        else
        {
            Destroy(this.gameObject);
        }

    }

    // Start is called before the first frame update
    void Start()
    {
        if (moneyValueGO == null)
        {
            moneyValueGO = GameObject.Find("moneyValue");
            if (moneyValueGO == null) Debug.LogWarning("DataSaver : Can't find moneyValue !");
        }
        if (datacentersValueGO == null)
        {
            datacentersValueGO = GameObject.Find("datacenterNumberValue");
            if (datacentersValueGO == null) Debug.LogWarning("DataSaver : Can't find datacenterNumberValue !");
        }
    }

    public void SaveValues()
    {
        if (moneyValueGO != null)
        {
            string s = moneyValueGO.GetComponent<TextMeshProUGUI>().text;
            string result = s.Remove(s.Length - 2); // remove " $"
            moneyValue = int.Parse(result);
        }
        if (datacentersValueGO != null)
        {
            string result = datacentersValueGO.GetComponent<TextMeshProUGUI>().text;
            datacentersValue = int.Parse(result);
        }
    }

    public void LoadValues(TextMeshProUGUI money, TextMeshProUGUI datacenters)
    {
        if (money != null)
            money.text = moneyValue.ToString() + " $";
        if (datacenters != null)
            datacenters.text = datacentersValue.ToString();
    }
}
