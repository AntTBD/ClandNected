using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/*
Maison :
 - la maison cr�e des data pour les envoyer vers un datacenter                                              // TODO
 - une maison peut �tre ou non satisfaite de sa connexion aux dataCenters ( d�cid� par les data )
 - les maisons sont g�n�r�es al�atoirement autour des datacenters                                           // In spawn generator
 - une maison a besoin d'�tre connect�e � un c�ble pour envoyer des donn�es                                 // if OK

Attributs du MaisonController :
 - bool�en de satisfaction                                                                                  // OK
 - ref portion de c�ble                                                                                     // OK
 - DeltaTime pour l'envoi de donn�es                                                                        // Added
 - Visiblement c'est tout ?..

Methods :
 - G�n�ration des data                                                                                      // TODO
 - Get/Set satisfaction                                                                                     // Added
 - Set Connected Cable                                                                                      // Added

Note :
 - Penser � lister les maisons dans un seul GameObject pour calculer la satisfaction g�n�rale
*/

public class HouseController : MonoBehaviour
{
    private bool isSatisfied;
    [SerializeField] private GameObject connectedCable;

    [SerializeField] private float sendDeltaTimeSeconds;

    [SerializeField] private GameObject dataPrefab;
    // Start is called before the first frame update
    void Start()
    {
        isSatisfied = false;
        StartCoroutine(SendDatas());
    }

    public void SetIsSatified(bool etat)
    {
        var bar = GameObject.Find("Slider").GetComponent<SatisfactionBar>();
        if (!etat) bar.removeSatisfaction();
        else bar.addSatisfaction();
        isSatisfied = etat;
    }
    public bool IsSatified()
    {
        return isSatisfied;
    }
    public void ConnectTo(GameObject cable)
    {
        connectedCable = cable;
    }


    public GameObject GetConnectedCable()
    {
        return connectedCable;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator SendDatas()
    {
        Debug.Log("Waiting 5s ...");
        yield return new WaitForSeconds(sendDeltaTimeSeconds);
        Debug.LogWarning(connectedCable);
        if (connectedCable == null) yield break;
        Debug.Log("Creating a new Data...");
        CreateNewData();
        StartCoroutine(SendDatas());
    }

    /// <summary>
    /// La maison cr�e des data pour les envoyer vers un datacenter
    /// </summary>
    private void CreateNewData()
    { 
        Instantiate(dataPrefab, Vector3.zero, Quaternion.identity,transform);
        Debug.Log("Data created");
    }
}
