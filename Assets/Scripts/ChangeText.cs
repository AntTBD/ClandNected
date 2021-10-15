using TMPro;
using UnityEngine;
 using UnityEngine.EventSystems;
 using UnityEngine.UI;
 
 public class ChangeText : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler {

    [SerializeField] private Text myText;
    [SerializeField] private TextMeshProUGUI myTextMesh;

    void Start (){
        myText = GetComponentInChildren<Text>();
        if(myText == null)
            myTextMesh = GetComponentInChildren<TextMeshProUGUI>();
    }
 
     public void OnPointerEnter (PointerEventData eventData)
     {
        if (myText != null)
            myText.color = new Color(0.5f,0.5f,0.5f,1f);
        if (myTextMesh != null)
            myTextMesh.color = new Color(0.5f, 0.5f, 0.5f, 1f);
    }
 
     public void OnPointerExit (PointerEventData eventData)
    {
        if (myText != null)
            myText.color = new Color(0,0,0);
        if (myTextMesh != null)
            myTextMesh.color = new Color(0, 0, 0);
    }
 }