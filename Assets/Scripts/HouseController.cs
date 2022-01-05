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
    [SerializeField] private bool isSatisfied;
    [SerializeField] private GameObject connectedCable;

    [SerializeField] private float sendDeltaTimeSeconds;

    [SerializeField] private GameObject dataPrefab;

    [SerializeField] private bool useSpritesIndicateSatisfaction;
    [SerializeField] private Sprite satisfiedSprite, unsatisfiedSprite;


    // Start is called before the first frame update
    void Start()
    {
        name = "House " + Random.Range(0, 1000).ToString();
        isSatisfied = false;
        if (useSpritesIndicateSatisfaction)
        {
            if (satisfiedSprite == null) Debug.LogWarning(name + " No satisfied sprite set !");
            if (unsatisfiedSprite == null) Debug.LogWarning(name + " No unsatisfied sprite set !");
        }

        // add little random
        sendDeltaTimeSeconds = Random.Range(sendDeltaTimeSeconds, sendDeltaTimeSeconds + 3);

        StartCoroutine(SendDatas());
    }

    public void SetIsSatified(bool etat)
    {
        SatisfactionBar bar = GameObject.Find("Slider").GetComponent<SatisfactionBar>();
        if (!etat) bar.removeSatisfaction();
        else bar.addSatisfaction();
        isSatisfied = etat;

        if (useSpritesIndicateSatisfaction)
        {
            transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = isSatisfied ? satisfiedSprite : unsatisfiedSprite;
        }
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

    IEnumerator SendDatas()
    {
        while (true)
        {
            yield return new WaitForSeconds(sendDeltaTimeSeconds);
            CreateNewData();
        }
    }

    /// <summary>
    /// La maison cr�e des data pour les envoyer vers un datacenter
    /// </summary>
    private void CreateNewData()
    {
        if (connectedCable != null)
        {
            Instantiate(dataPrefab, Vector3.zero, Quaternion.identity, transform);
        }
        else
        {
            SetIsSatified(false);// if not connected, decreases satisfaction
        }
    }
    private void OnDestroy()
    {
        StopCoroutine(SendDatas());
    }

}
