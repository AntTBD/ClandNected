using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
Datacenter :
 - le datacenter accueille les data pour générer de l'argent au joueur                                          // TODO
 - il a une vitesse de traitement des data lorsqu'il les reçoit                                                 // OK
   -> empêche de traiter des données à volonté
 - il est améliorable  (vitesse de traitement, nombre de port 1-4 ...)                                          // OK vitesse (SetProcessingSpeed) / nb ports (UpgradePortsMax)
 - le joueur pourra demander à avoir un autre datacenter mais ce dernier apparait aléatoirement sur la carte !

Attributs du controller :
 - Vitesse de traitement                                                                                        // ADDED
 - Nb port max                                                                                                  // OK
 - Nb port utilisées                                                                                            // OK
 - booléen pour savoir si on peut tirer un câble ou non ( calculé à partir des deux au dessus )                 // OK
 - file de traitement des data                                                                                  // OK
   -> si pleine -> on détruit les data qui arrivent                                                             // OK AddNewDataToWaitingList (on ne l'ajoute pas à la list) TODO delete prefab
 - capacitée max de la list                                                                                     // ADDED
 - Autres aux besoins 

Methodes:
 - Update booléen pour savoir si on peut tirer un câble ou non                                                  // OK SetCanPullCable()
 - Améliorer la vitesse de traitement                                                                           // OK SetProcessingSpeed(float deltaTime = 0.5f)
 - Améliroer le nb de ports                                                                                     // OK UpgradePortsMax(int deltaUpgrade = 1)
 - Connecter un nouveau cable                                                                                   // OK ConnectNewCable(CableManager cable)
 - Traitement de la list toute les X secondes                                                                   // OK DatasProcessing()
 - Traitement d'une donnée                                                                                      // TODO TOCOMPLET OneDataProcessing(DataManager data)
 - Ajouter une nouvelle list                                                                                    // TODO TOCOMPLET AddNewDataToWaitingList(DataManager data)

Note :
 - ne pas oublier la gestion de l'économie et de la satisfaction lorsque l'on traite une data
*/

public class DatacenterController : MonoBehaviour
{
    [SerializeField] private float processingSpeed;

    [SerializeField] private int nbPortsMax;
    private int nbPortsUsed;
    private bool canPullCable;
    private List<CableManager> connectedCables;
    private List<DataManager> waitingLine;
    [SerializeField] private int waitingLineMaxCapacity;



    // Start is called before the first frame update
    void Start()
    {
        nbPortsUsed = 0;
        SetCanPullCable();
        connectedCables = new List<CableManager>();
        waitingLine = new List<DataManager>(waitingLineMaxCapacity);
    }

    /// <summary>
    /// Set canPullCable pour savoir si on peut tirer un câble ou non ( calculé à partir du nbOutputMax et nbOutputUsed )
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
    /// Vitesse de traitement améliorable (-0.5 par defaut)<br/>
    /// SI supérieur à 0s entre chaque traitement
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
    /// Amelioration du nb max de ports (par défaut : +1) <br/>
    /// Update canPullCable
    /// </summary>
    /// <param name="deltaUpgrade"></param>
    void UpgradePortsMax(int deltaUpgrade = 1)
    {
        nbPortsMax += deltaUpgrade;
        SetCanPullCable();
    }

    void ConnectNewCable(CableManager cable)
    {
        connectedCables.Add(cable);
        nbPortsUsed++;
        SetCanPullCable();
    }

    // Update is called once per frame
    void Update()
    {
        DatasProcessing();
    }

    /// <summary>
    /// Traitement des données toute les [processingSpeed] secondes
    /// </summary>
    /// <returns></returns>
    IEnumerator DatasProcessing()
    {
        if (waitingLine.Count > 0) // Si on a des datas
        {
            OneDataProcessing(waitingLine[0]);
        }

        yield return new WaitForSeconds(processingSpeed);// il a une vitesse de traitement des data lorsqu'il les reçoit
    }

    /// <summary>
    /// Traitement d'une donnée (house satisfied + player money)<br/>
    /// Supprime la data de la list d'attente
    /// Supprime la data
    /// </summary>
    /// <param name="data"></param>
    void OneDataProcessing(DataManager data)
    {
        /// TODO : complete to affect data + its house satisfaction (+) + player money (+)
        // ...
        // data.GetHouse().SetIsSatified(true);
        // playerManager.AddMoney(...);



        // remove data from list
        waitingLine.RemoveAt(0);

        /// TODO : delete prefab data
        // ...
    }

    /// <summary>
    /// Ajoute une data à la liste d'attente <br/>
    /// SI la list n'a pas atteint ça capacité max <br/>
    /// SINON affect house satisfaction (TODO) + supprime la data (TODO)
    /// </summary>
    /// <param name="data"></param>
    void AddNewDataToWaitingList(DataManager data)
    {
        if (waitingLine.Count <= waitingLineMaxCapacity)
        {
            waitingLine.Add(data);
        }
        else
        {
            /// TODO : affect data + its house satisfaction (-)
            // data.GetHouse().SetIsSatified(false);

            /// TODO : delete prefab data
            // ...
        }
    }
}
