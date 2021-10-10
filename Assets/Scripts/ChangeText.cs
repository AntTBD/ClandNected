 using UnityEngine;
 using UnityEngine.EventSystems;
 using UnityEngine.UI;
 
 public class ChangeText : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler {
 
     private Text myText;
 
     void Start (){
         myText = GetComponentInChildren<Text>();
     }
 
     public void OnPointerEnter (PointerEventData eventData)
     {
        myText.color = new Color(0.5f,0.5f,0.5f,1f);
     }
 
     public void OnPointerExit (PointerEventData eventData)
     {
        myText.color = new Color(0,0,0);
     }
 }