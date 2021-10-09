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
