using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SatisfactionBar : MonoBehaviour
{
    private const int SATISFIED_VAL = 1;
    private const int UNSATISFIED_VAL = 4;

    private const int STARTHEALTH = 50;

    public const int MAXHEALTH = 100;

    private Slider slider;

    private int curSatisfaction;

    public Image Fill; // assign in the editor the "Fill"

    public Color maxHealthColor = Color.green;

    public Color minHealthColor = Color.red;

    private void Awake()
    {
        slider = gameObject.GetComponent<Slider>();
        curSatisfaction = STARTHEALTH; // just for testing purposes
    }

    private void Start()
    {
        slider.wholeNumbers = true; // I dont want 3.543 Health but 3 or 4
        slider.minValue = 0f;
        slider.maxValue = MAXHEALTH;
        slider.value = STARTHEALTH; // start with half health
    }

        public bool removeSatisfaction (int cost = UNSATISFIED_VAL) {
        if (curSatisfaction - cost >= 0) {
            this.curSatisfaction -= cost;
            UpdateHealthBar();
            return true;
        }
        // TODO : Implement game over here
        Debug.Log("GAME OVER !!!!");
        return false;
    }

    public bool addSatisfaction (int income = SATISFIED_VAL) {
        if(curSatisfaction + income <= 100)
            curSatisfaction += income;
        UpdateHealthBar();
        return true;
    }

    //TODO Remove this !
    private void Update () {
        if (Input.GetKeyDown (KeyCode.RightArrow))
            this.addSatisfaction ();
        if (Input.GetKeyDown (KeyCode.LeftArrow))
            this.removeSatisfaction ();
    }

    private void UpdateHealthBar()
    {
        slider.value = curSatisfaction;
        Fill.color =
            Color.Lerp(minHealthColor, maxHealthColor, (float) curSatisfaction / MAXHEALTH);
    }
}
