using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
Datacenter :
 - le datacenter accueille les data pour g�n�rer de l'argent au joueur                                          // TODO
 - il a une vitesse de traitement des data lorsqu'il les re�oit                                                 // OK
   -> emp�che de traiter des donn�es � volont�
 - il est am�liorable  (vitesse de traitement, nombre de port 1-4 ...)                                          // OK vitesse (SetProcessingSpeed) / nb ports (UpgradePortsMax)
 - le joueur pourra demander � avoir un autre datacenter mais ce dernier apparait al�atoirement sur la carte !

Attributs du controller :
 - Vitesse de traitement                                                                                        // ADDED
 - Nb port max                                                                                                  // OK
 - Nb port utilis�es                                                                                            // OK
 - bool�en pour savoir si on peut tirer un c�ble ou non ( calcul� � partir des deux au dessus )                 // OK
 - file de traitement des data                                                                                  // OK
   -> si pleine -> on d�truit les data qui arrivent                                                             // OK AddNewDataToWaitingList (on ne l'ajoute pas � la list) TODO delete prefab
 - capacit�e max de la list                                                                                     // ADDED
 - Autres aux besoins 

Methodes:
 - Update bool�en pour savoir si on peut tirer un c�ble ou non                                                  // OK SetCanPullCable()
 - Am�liorer la vitesse de traitement                                                                           // OK SetProcessingSpeed(float deltaTime = 0.5f)
 - Am�liroer le nb de ports                                                                                     // OK UpgradePortsMax(int deltaUpgrade = 1)
 - Connecter un nouveau cable                                                                                   // OK ConnectNewCable(CableManager cable)
 - Traitement de la list toute les X secondes                                                                   // OK DatasProcessing()
 - Traitement d'une donn�e                                                                                      // TODO TOCOMPLET OneDataProcessing(DataManager data)
 - Ajouter une nouvelle list                                                                                    // TODO TOCOMPLET AddNewDataToWaitingList(DataManager data)

Note :
 - ne pas oublier la gestion de l'�conomie et de la satisfaction lorsque l'on traite une data
*/

public class DatacenterController : MonoBehaviour
{
    [SerializeField] private float processingSpeed;

    [SerializeField] private int nbPortsMax;
    private int nbPortsUsed;
    private bool canPullCable;
    [SerializeField]
    private List<CableController> connectedCables;
    [SerializeField]
    private List<DataController> waitingLine;
    [SerializeField] private int waitingLineMaxCapacity;



    // Start is called before the first frame update
    void Start()
    {
        nbPortsUsed = 0;
        SetCanPullCable();
        connectedCables = new List<CableController>();
        waitingLine = new List<DataController>(waitingLineMaxCapacity);
        StartCoroutine(DatasProcessing());
    }

    /// <summary>
    /// Set canPullCable pour savoir si on peut tirer un c�ble ou non ( calcul� � partir du nbOutputMax et nbOutputUsed )
    /// </summary>
    private void SetCanPullCable()
    {
        if (nbPortsUsed < nbPortsMax)
        {
            canPullCable = true;
        }
        else
        {
            canPullCable = false;
        }
    }

    /// <summary>
    /// Vitesse de traitement am�liorable (-0.5 par defaut)<br/>
    /// SI sup�rieur � 0s entre chaque traitement
    /// </summary>
    /// <param name="deltaTime"></param>
    void SetProcessingSpeed(float deltaTime = 0.5f)
    {
        if (processingSpeed - deltaTime >= 0f)
        {
            processingSpeed -= deltaTime;
        }

    }

    /// <summary>
    /// Amelioration du nb max de ports (par d�faut : +1) <br/>
    /// Update canPullCable
    /// </summary>
    /// <param name="deltaUpgrade"></param>
    void UpgradePortsMax(int deltaUpgrade = 1)
    {
        nbPortsMax += deltaUpgrade;
        SetCanPullCable();
    }

    public void ConnectNewCable(CableController cable)
    {
        connectedCables.Add(cable);
        nbPortsUsed++;
        SetCanPullCable();
    }
    
    /// <summary>
    /// Traitement des donn�es toute les [processingSpeed] secondes
    /// </summary>
    /// <returns></returns>
    IEnumerator DatasProcessing()
    {
        yield return new WaitForSeconds(processingSpeed);// il a une vitesse de traitement des data lorsqu'il les re�oit
        if (waitingLine.Count > 0)
        {
            Debug.Log("Process"+waitingLine[0]);
            OneDataProcessing(waitingLine[0]);
        }
        StartCoroutine(DatasProcessing());

    }

    /// <summary>
    /// Traitement d'une donn�e (house satisfied + player money)<br/>
    /// Supprime la data de la list d'attente
    /// Supprime la data
    /// </summary>
    /// <param name="data"></param>
    void OneDataProcessing(DataController data)
    {
        //affect data + its house satisfaction (+) + player money (+)
        GameObject.Find("MoneyManager").GetComponent<MoneyManager>().addMoney();
        // remove data from list
        waitingLine.Remove(data.GetComponent<DataController>());
        // delete prefab data + affect satisfaction(+)
        data.Delete(true);


    }

    /// <summary>
    /// Ajoute une data � la liste d'attente <br/>
    /// SI la list n'a pas atteint �a capacit� max <br/>
    /// SINON affect house satisfaction (TODO) + supprime la data (TODO)
    /// </summary>
    /// <param name="data"></param>
    public void AddNewDataToWaitingList(DataController data)
    {
        if (waitingLine.Count <= waitingLineMaxCapacity)
        {
            waitingLine.Add(data);
        }
        else
        {
            data.Delete(false);
        }
    }
}
