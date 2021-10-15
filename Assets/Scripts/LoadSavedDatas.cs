using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LoadSavedDatas : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI moneyText, datacentersText;
    [SerializeField] private RenderTexture mapTexture;
    [SerializeField] private MiniMap miniMap;
    // Start is called before the first frame update
    void Start()
    {
        GameObject dataSaver = GameObject.Find("DataSaver");
        if (dataSaver != null) mapTexture = dataSaver.GetComponent<DataSaver>().LoadValues(moneyText, datacentersText);

        if (miniMap != null && mapTexture != null) miniMap.SetTexture(mapTexture);
    }
}
