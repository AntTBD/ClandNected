using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LoadSavedDatas : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI moneyText, datacentersText;
    // Start is called before the first frame update
    void Start()
    {
        GameObject dataSaver = GameObject.Find("DataSaver");
        if (dataSaver != null) dataSaver.GetComponent<DataSaver>().LoadValues(moneyText, datacentersText);
    }
}
