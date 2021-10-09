using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/*
Maison :
 - la maison crée des data pour les envoyer vers un datacenter                                              // TODO
 - une maison peut être ou non satisfaite de sa connexion aux dataCenters ( décidé par les data )
 - les maisons sont générées aléatoirement autour des datacenters                                           // In spawn generator
 - une maison a besoin d'être connectée à un câble pour envoyer des données                                 // if OK

Attributs du MaisonController :
 - booléen de satisfaction                                                                                  // OK
 - ref portion de câble                                                                                     // OK
 - DeltaTime pour l'envoi de données                                                                        // Added
 - Visiblement c'est tout ?..

Methods :
 - Génération des data                                                                                      // TODO
 - Get/Set satisfaction                                                                                     // Added
 - Set Connected Cable                                                                                      // Added

Note :
 - Penser à lister les maisons dans un seul GameObject pour calculer la satisfaction générale
*/

public class HouseController : MonoBehaviour
{
    bool isSatisfied;
    CableController connectedCable; // TODO : add Cable script

    [SerializeField] float sendDeltaTimeSeconds;

    // Start is called before the first frame update
    void Start()
    {
        isSatisfied = false;
        connectedCable = null;
    }

    public void SetIsSatified(bool etat)
    {
        isSatisfied = etat;
    }
    public bool IsSatified()
    {
        return isSatisfied;
    }
    public void ConnectTo(CableController cable)
    {
        connectedCable = cable;
    }

    // Update is called once per frame
    void Update()
    {
        SendDatas();
    }

    IEnumerator SendDatas()
    {
        if(connectedCable != null) // if connected
        {
            CreateNewData();
        }

        yield return new WaitForSeconds(sendDeltaTimeSeconds);
    }

    void CreateNewData()
    {
        /// TODO : Create data prefab
        /// Send data
    }
}
