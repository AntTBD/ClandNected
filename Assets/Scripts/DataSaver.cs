using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DataSaver : MonoBehaviour
{
    private static bool created = false;

    [Header("GameObjects")]
    [SerializeField] private GameObject moneyValueGO;
    [SerializeField] private GameObject datacentersValueGO;
    [SerializeField] private GameObject housesValueGO;
    [SerializeField] private GameObject hiddenElement;
    [Header("Values")]
    [SerializeField] private int moneyValue;
    [SerializeField] private int datacentersValue;
    [SerializeField] private int housesValue;

    [SerializeField] RenderTexture renderTexture;

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
        if (moneyValueGO == null || datacentersValueGO == null || housesValueGO == null || hiddenElement == null)
        {
            SetAllGameObjects();
        }
    }

    private void SetAllGameObjects()
    {
        Debug.Log("Set All GameObjects");
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
        if (housesValueGO == null)
        {
            housesValueGO = GameObject.Find("houseNumberValue");
            if (housesValueGO == null) Debug.LogWarning("House : Can't find houseNumberValue !");
        }
        if (hiddenElement == null)
        {
            hiddenElement = GameObject.Find("InGameUI");
            if (hiddenElement == null) Debug.LogWarning("HiddenElement : Can't find InGameUI !");
        }
    }

    public void SaveValues()
    {
        if (moneyValueGO == null || datacentersValueGO == null || housesValueGO == null || hiddenElement == null)
        {
            SetAllGameObjects();
        }

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
        if (housesValueGO != null)
        {
            string result = housesValueGO.GetComponent<TextMeshProUGUI>().text;
            housesValue = int.Parse(result);
        }

        // get last render image : https://stackoverflow.com/questions/56783654/how-to-capture-frames-from-the-unity3d-camera-and-display-them-on-another-rawima
        CaptureScreen();
    }

    public RenderTexture LoadValues(TextMeshProUGUI money, TextMeshProUGUI datacenters, TextMeshProUGUI houses)
    {
        if (money != null)
            money.text = moneyValue.ToString() + " $";
        if (datacenters != null)
            datacenters.text = datacentersValue.ToString();
        if (houses != null)
            houses.text = housesValue.ToString();

        return GetSnapShot();
    }

    // ============================================================================


    // https://answers.unity.com/questions/850451/capturescreenshot-without-ui.html
    public void CaptureScreen()
    {
        // Wait till the last possible moment before screen rendering to hide the UI
        if (hiddenElement != null) hiddenElement.SetActive(false);

        // Wait for screen rendering to complete
        new WaitForEndOfFrame();

        // reset cam position
        Camera.main.GetComponent<CameraMovements>().ResetCam();

        // Take screenshot
        SaveScreenshot();

        // Show UI after we're done
        if (hiddenElement != null) hiddenElement.SetActive(true);
    }

    // https://answers.unity.com/questions/22954/how-to-save-a-picture-take-screenshot-from-a-camer.html
    private void SaveScreenshot()
    {
        Camera cam = Camera.main;
        float height = Screen.height;
        float width = Screen.width;

        // creates off-screen render texture that can rendered into
        renderTexture = new RenderTexture((int)width, (int)height, 24);
        
        // manually render scene into rt
        cam.targetTexture = renderTexture;
        cam.Render();

        // read pixels will read from the currently active render texture so make our offscreen 
        // render texture active and then read the pixels
        RenderTexture.active = renderTexture;

        // reset active camera texture and render texture
        cam.targetTexture = null;
        RenderTexture.active = null; // JC: added to avoid errors

    }

    private RenderTexture GetSnapShot()
    {
        return renderTexture;
    }
}