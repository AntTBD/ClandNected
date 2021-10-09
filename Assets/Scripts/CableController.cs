using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
Cable :
 - le câble est un élément cœur du gameplay, il s'agit du seul élément manipulable par le joueur
 - le joueur pourra tirer des câbles depuis des "points de tirage" ( datacenter, maison, routeurs, autres câbles )
 - le câble est un objet regroupant toutes les portions de câbles formant ce dernier ( une portion = une case )
 - un câble possède une capacité maximale de data transportables
   -> un câble peut être amélioré pour augmenter sa capacité de transport
   -> la capacité de transport du câble est calculée en fonction de sa longueur
      -> évite de tricher en faisant se succéder des petits câbles pouvant transporter plus de données qu'un seul long
      -> utiliser un multiplicateur et le nb de portions de câble contenu par le câble
   -> Un câble saturé ne peut accueillir aucune data de plus

Attributs du CableController :
 - ref ObjDepart ( en vrai le sens n'a pas d'importance )                                                                       // OK
 - ref ObjArrivee ( en vrai le sens n'a toujours pas d'importance )                                                             // OK
 - Niveau d'amélioration                                                                                                        // OK
 - Nombre de data max dans le cable                                                                                             // OK
   -> améliorable
 - Nombre de data actuellement dans le cable                                                                                    // ADDED
 - float poid du cable ( le poid nous aide à calculer les chemins les plus courts vers les datacenters )                        // OK
   - Deux options ici :
   -> ( 1 ) On calcul simplement en fonction de la taille de câble (nb de portions de câble)
   -> ( 2 ) On calcul en fonction de la taille du câble et de sa saturation                                                     // OK (could be improve)
      -> permet d'avoir un réseau plus intelligent et équilibré qui essaie d'éviter les bouchons de data
      -> peut être implémenté via une amélioration pour que le joueur le débloque en cours de jeu
 - operationnel                                                                                                                 // OK
   -> dans le cas d'un câble cassé qui foutrait la merde :)
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
 - gérer la section d'un câble en deux câbles lors de la connexion d'un autre à celui-ci (cf-> routeur)                         // OK : to check
 - Un câble saturé devra changer de couleur et tirer vers le rouge, un câble au repos sera bleu ou vert                         // TODO
 - on pourra éventuellement ajouter un système d'usure du câble                                                                 // TODO
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
        ///  On calcul en fonction de la taille du câble et de sa saturation 
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
