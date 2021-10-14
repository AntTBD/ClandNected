using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Utils;

/*
Cable :
 - le c�ble est un �l�ment c�ur du gameplay, il s'agit du seul �l�ment manipulable par le joueur
 - le joueur pourra tirer des c�bles depuis des "points de tirage" ( datacenter, maison, routeurs, autres c�bles )
 - le c�ble est un objet regroupant toutes les portions de c�bles formant ce dernier ( une portion = une case )
 - un c�ble poss�de une capacit� maximale de data transportables
   -> un c�ble peut �tre am�lior� pour augmenter sa capacit� de transport
   -> la capacit� de transport du c�ble est calcul�e en fonction de sa longueur
      -> �vite de tricher en faisant se succ�der des petits c�bles pouvant transporter plus de donn�es qu'un seul long
      -> utiliser un multiplicateur et le nb de portions de c�ble contenu par le c�ble
   -> Un c�ble satur� ne peut accueillir aucune data de plus

Attributs du CableController :
 - ref ObjDepart ( en vrai le sens n'a pas d'importance )                                                                       // OK
 - ref ObjArrivee ( en vrai le sens n'a toujours pas d'importance )                                                             // OK
 - Niveau d'am�lioration                                                                                                        // OK
 - Nombre de data max dans le cable                                                                                             // OK
   -> am�liorable
 - Nombre de data actuellement dans le cable                                                                                    // ADDED
 - float poid du cable ( le poid nous aide � calculer les chemins les plus courts vers les datacenters )                        // OK
   - Deux options ici :
   -> ( 1 ) On calcul simplement en fonction de la taille de c�ble (nb de portions de c�ble)
   -> ( 2 ) On calcul en fonction de la taille du c�ble et de sa saturation                                                     // OK (could be improve)
      -> permet d'avoir un r�seau plus intelligent et �quilibr� qui essaie d'�viter les bouchons de data
      -> peut �tre impl�ment� via une am�lioration pour que le joueur le d�bloque en cours de jeu
 - operationnel                                                                                                                 // OK
   -> dans le cas d'un c�ble cass� qui foutrait la merde :)
 - list de sections                                                                                                             // ADDED
 - list de datas => dans les enfants du cable

Methodes :
SetBegin(GameObject begin)                                                                                                      // OK
SetEnd(GameObject end)                                                                                                          // OK
CheckAndUpdateMaxData()                                                                                                         // TODO : TO IMPROVE
UpgradeLevel()                                                                                                                  // OK : Call section upgrade
GetWeight()                                                                                                                     // OK
IsOperational()                                                                                                                 // OK

UpdateOperational()                                                                                                             // OK
UpdateWeight()                                                                                                                  // TODO : to improve
AddSection(CableSectionController section)                                                                                      // OK
Delete()                                                                                                                        // OK
AddData(GameObject data)                                                                                                        // OK : to check
RemoveData(GameObject data)                                                                                                     // TODO
CheckSaturation()                 change color of cable                                                                         // TODO
Diviser(GameObject router)                                                                                                      // OK : to check

Note :
 - Un cable doit pouvoir se dessiner et se supprimer facilement sur la grille                                                   // TODO: delete TO CHECK
 - g�rer la section d'un c�ble en deux c�bles lors de la connexion d'un autre � celui-ci (cf-> routeur)                         // OK : to check
 - Un c�ble satur� devra changer de couleur et tirer vers le rouge, un c�ble au repos sera bleu ou vert                         // TODO
 - on pourra �ventuellement ajouter un syst�me d'usure du c�ble                                                                 // TODO
*/

public class CableController : MonoBehaviour
{
    [SerializeField]
    private GameObject objBegin;
    [SerializeField] private GameObject objEnd;
    [SerializeField] private int level;
    const int LEVEL_MAX = 4;
    [SerializeField]
    private int nbMaxDatas = 10;
    [SerializeField]
    private int nbDatas;
    [SerializeField] private float weight;
    private bool operational = true;
    private List<DataController> datas;

    private bool previousOperational = false;

    private Grid<GameObject> grid;
    private const int CABLECOST = 2;

    // Start is called before the first frame update
    void Awake()
    {
        name = "Cable " + Random.Range(0, 10000).ToString();
        level = 1;
        CheckAndUpdateMaxData();
        weight = 0f;
        datas = new List<DataController>();

        grid = GameObject.Find("GridManager").GetComponent<GridManager>().GetGrid();
    }

    public void SetBegin(GameObject begin)
    {
        objBegin = begin;
    }
    public void SetEnd(GameObject end)
    {
        objEnd = end;
    }

    public GameObject GetBegin()
    {
        return objBegin;
    }
    public GameObject GetEnd()
    {
        return objEnd;
    }

    public void SetOperational(bool test)
    {
        operational = test;
    }

    /// <summary>
    /// Upgrade max capacity of data<br/>
    /// level * nb enfants
    /// </summary>
    void CheckAndUpdateMaxData()
    {
        /// TODO : to improve
        nbMaxDatas = level * 5;
    }

    public void UpgradeLevel()
    {
        bool upgraded = false;
        if (level <= LEVEL_MAX && GameObject.Find("MoneyManager").GetComponent<MoneyManager>().removeMoney(CABLECOST * level * (transform.childCount + 1)))
        {
            foreach (Transform section in transform)
            {
                /// call function to upgarde section
                upgraded = section.GetComponent<CableSectionController>().Upgrade();
                if (upgraded == false)
                    break;
            }
            if (upgraded)
            {
                level++;
                CheckAndUpdateMaxData();

                UpdateWeight();
            }
        }
    }
    public float GetWeight()
    {
        return weight;
    }
    bool IsOperational()
    {
        return operational;
    }

    // Update is called once per frame
    void Update()
    {
        CheckSaturation();
        CheckForUpgrade();
        UpdateWeight();
    }

    void CheckForUpgrade()
    {
        if (Input.GetMouseButtonDown(1)) // right clic
        {
            if (grid != null)
            {
                Vector3 mousePos = grid.GetGridPosition(GetMouseWorldPosition());
                if (grid.IsInGrid(mousePos))
                {
                    foreach (Transform section in transform)
                    {
                        if (mousePos == section.position)
                        {
                            UpgradeLevel();
                            break;
                        }
                    }

                }
            }
        }
    }

    void CheckSaturation()
    {
        if (IsOperational() != previousOperational)// change color if state is different
        {
            previousOperational = operational;
            // change color foreach sections
            foreach (Transform section in transform)
            {
                section.GetComponent<CableSectionController>().SetSatured(! operational);
            }
        }
    }

    void UpdateOperational()
    {
        operational = (nbDatas < nbMaxDatas);
    }

    void UpdateWeight()
    {
        /// TODO : calcul du poid
        ///  On calcul en fonction de la taille du c�ble et de sa saturation 
        //weight = transform.childCount * ((float)nbMaxDatas - (float)nbDatas);
        weight = (transform.childCount+1 + nbDatas)*(LEVEL_MAX-(level-1));
    }

    public void AddSection(CableSectionController section)
    {
        section.transform.parent = transform; // add section as a child
        section.name = name + "_" + transform.childCount;
        UpdateWeight();
    }

    public bool AddData(GameObject data)
    {
        if (IsOperational())
        {
            nbDatas++;
            datas.Add(data.GetComponent<DataController>());
            UpdateOperational();
            UpdateWeight();
            return true;
        }
        else
        {
            // delete data
            data.GetComponent<DataController>().Delete(false);
            return false;
        }
    }

    public void RemoveData(GameObject data)
    {
        if (datas.Count > 0)
        {
            datas.RemoveAt(0);
            nbDatas--;
        }
        UpdateOperational();
        UpdateWeight();

        /// TODO : Delete data or transmit to objEnd or objBegin
        /// ...

    }

    /// <summary>
    /// Delete sections + datas
    /// </summary>
    public void Delete()
    {
        // delete sections
        foreach (Transform section in transform)
        {
            /// TODO : call function to remove section
            section.GetComponent<CableSectionController>().Delete();

        }
        // delete datas
        foreach (DataController data in datas)
        {
            data.Delete(false);
        }
        // delete cable prefab
        Destroy(gameObject);
    }

    public bool Diviser(GameObject router, GameObject prefabCableController)
    {
        // get size of cable to compare success or not
        int cableSizeTemp = transform.childCount;
        // create new cable
        GameObject newCable = Instantiate(prefabCableController, Vector3.zero, Quaternion.identity);
        // parcourir le cable actuel
        bool firstCable = true;
        Transform middleSection = null;
        List<CableSectionController> listTemp = new List<CableSectionController>();
        foreach (Transform section in transform)
        {
            if (firstCable == true)
            {
                if (section.transform.position == router.transform.position)
                {
                    // suprime la section correspondant au router et changer de cable
                    middleSection = section;
                    firstCable = false;
                }
            }
            else
            {
                // add section to new cable (auto change parent)
                listTemp.Add(section.GetComponent<CableSectionController>());
            }
        }
        // on change le parent apres
        foreach (CableSectionController temp in listTemp)
        {
            newCable.GetComponent<CableController>().AddSection(temp);
        }
        middleSection.GetComponent<CableSectionController>().Delete();
        newCable.GetComponent<CableController>().SetBegin(router);// set begin of new cable
        newCable.GetComponent<CableController>().SetEnd(objEnd);// set end of new cable
        objEnd = router; // set end of this cable

        router.GetComponent<RouterController>().addPort(newCable);// newCable first section
        router.GetComponent<RouterController>().addPort(gameObject);// thisCable last section

        return newCable.transform.childCount + transform.childCount == cableSizeTemp-1;
    }
}
