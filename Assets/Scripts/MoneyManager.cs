using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MoneyManager : MonoBehaviour {
    private const int CABLECOST = 10;

    private const int HOUSEINCOME = 2;
    private const int STARTINCOME = 40;

    [SerializeField]
    private GameObject moneyValue;

    private int moneyAvailable;

    // Start is called before the first frame update
    private void Start () {
        moneyAvailable = STARTINCOME;
        if (!moneyValue)
            moneyValue = GameObject.Find ("moneyValue");
        displayMoney ();
    }

    public bool removeMoney (int cost = CABLECOST) {
        if (moneyAvailable - cost >= 0) {
            this.moneyAvailable -= cost;
            displayMoney ();
            return true;
        }
        return false;
    }

    public bool addMoney (int income = HOUSEINCOME) {
        moneyAvailable += income;
        displayMoney ();
        return true;
    }

    private void displayMoney () {
        this.moneyValue.GetComponent<Text> ().text = this.ToString ();
    }

    public override string ToString () {
        return this.moneyAvailable + " $";
    }

    //TODO Remove this !
    private void Update () {
        if (Input.GetKeyDown (KeyCode.UpArrow))
            this.addMoney ();
        if (Input.GetKeyDown (KeyCode.DownArrow))
            this.removeMoney ();
    }
}