using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
CheckSaturation()                                                                                                               // TODO
Diviser(GameObject router)                                                                                                      // OK : to check

Note :
 - Un cable doit pouvoir se dessiner et se supprimer facilement sur la grille                                                   // TODO delete TO CHECK
 - g�rer la section d'un c�ble en deux c�bles lors de la connexion d'un autre � celui-ci (cf-> routeur)                         // OK : to check
 - Un c�ble satur� devra changer de couleur et tirer vers le rouge, un c�ble au repos sera bleu ou vert                         // TODO
 - on pourra �ventuellement ajouter un syst�me d'usure du c�ble                                                                 // TODO
*/

public class CableController : MonoBehaviour
{
    private GameObject objBegin;
    private GameObject objEnd;
    private int level;
    private int nbMaxDatas;
    private int nbDatas;
    private float weight;
    private bool operational;


    // Start is called before the first frame update
    void Start()
    {
        level = 0;
        CheckAndUpdateMaxData();
        UpdateOperational();
        weight = 0f;

    }

    void SetBegin(GameObject begin)
    {
        objBegin = begin;
    }
    void SetEnd(GameObject end)
    {
        objEnd = end;
    }

    /// <summary>
    /// Upgrade max capacity of data<br/>
    /// level * nb enfants
    /// </summary>
    void CheckAndUpdateMaxData()
    {
        /// TODO : to improve
        nbMaxDatas = level * transform.childCount;
    }

    void UpgradeLevel()
    {
        level++;
        CheckAndUpdateMaxData();
        foreach(Transform section in transform)
        {
            /// TODO : call function to upgarde section
            section.GetComponent<CableSectionController>().Upgrade();
        }
        UpdateWeight();
    }
    float GetWeight()
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
    }

    void CheckSaturation()
    {
        if (IsOperational() == false)
        {
            /// TODO : change color
        }
    }

    void UpdateOperational()
    {
        operational = (nbDatas <= nbMaxDatas);
    }

    void UpdateWeight()
    {
        /// TODO : calcul du poid
        ///  On calcul en fonction de la taille du c�ble et de sa saturation 
        weight = transform.childCount * ((float)nbMaxDatas / (float)nbDatas);
    }

    void AddSection(CableSectionController section)
    {
        section.transform.parent = transform; // add section as a child

        UpdateWeight();
    }

    bool AddData(GameObject data)
    {
        if (IsOperational())
        {
            nbDatas++;
            UpdateOperational();
            UpdateWeight();
            /// TODO : faire parcourir la data le long des sections
            /// ... devrait se faire dans la data

            return true;
        }
        else
        {
            /// TODO : delete data
            // data.GetComponent<CableSectionController>().Delete();
            return false;
        }
    }

    void RemoveData(GameObject data)
    {
        nbDatas--;
        UpdateOperational();
        UpdateWeight();

        /// TODO : Delete data or transmit to objEnd or objBegin

    }

    /// <summary>
    /// Delete cable
    /// </summary>
    void Delete()
    {
        foreach (Transform section in transform)
        {
            /// TODO : call function to remove section
            section.GetComponent<CableSectionController>().Delete();
            
        }
        // delete cable prefab
        Destroy(gameObject);
    }

    void Diviser(GameObject router)
    {
        // create new cable
        GameObject newCable = Instantiate((GameObject)Resources.Load("Prefabs/" + "Cable", typeof(GameObject)), Vector3.zero, Quaternion.identity);
        newCable.GetComponent<CableController>().SetBegin(router);// set begin of new cable
        newCable.GetComponent<CableController>().SetEnd(objEnd);// set end of new cable
        objEnd = router; // set end of this cable

        // parcourir le cable actuel
        bool firstCable = true;
        foreach (Transform section in transform)
        {
            if (firstCable == true && section.transform.position == router.transform.position)
            {
                // suprime la section correspondant au router et changer de cable
                section.GetComponent<CableSectionController>().Delete();
                firstCable = false;
            }
            else if (firstCable == false)
            {
                // add section to new cable (auto change parent)
                newCable.GetComponent<CableController>().AddSection(section.GetComponent<CableSectionController>());
            }
        }
    }
}
