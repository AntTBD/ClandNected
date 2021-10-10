using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
Portion de cable :
 - la portion de cable est contenue sur un case de notre grille, elle est toujours fille d'un cable
 - elle doit �tre construite conform�ment aux portions qui l'entourent
   -> lors de sa construction le sprite affich� doit correspondre � la direction de la portion de cable

Attributs : 
 - r�f ObjDepart ( encore une fois �a n'a pas d'importance )
 - r�f ObjArrivee ( IDEM )
   -> ces r�f�rences servent � l'affichage de notre portion lors de sa cr�ation
 - Peut �tre autre chose ?
 - List de sprites pour les diff�rents niveau
 - 

Methods :
 - Calcul du sprite en fonction de ObjDepart et objArrivee

Note :
 - Attention � ne pas construire sur une case d�j� occup�e !
 - Attention � la connexion entre deux c�bles cf -> Cable !
 - Honnetement je suis pas trop s�r de comment �a va se passer avec sa mais bon courage :D 
*/

public class CableSectionController : MonoBehaviour
{

    private int level;
    private Sprite actualSprite;
    [SerializeField] public List<Sprite> sprites;
    [SerializeField] private Color colorSatured, colorNonSatured;

    private bool cableSatured;

    // Start is called before the first frame update
    void Start()
    {/*
        level = 0;
        name = "Section "+Random.Range(0, 1000).ToString();
        
        if (sprites == null)
        {
            Debug.LogError("[CableSectionController] Sprites manquant !!!");
        }
        SetActualSprite();
        */
    }

    public void SetActualSprite()
    {
        if (sprites != null && sprites[level] != null)
        {
            actualSprite = sprites[level];
            Debug.Log("Level : " + level);
            transform.GetComponent<SpriteRenderer>().sprite = actualSprite;
        }
    }

    public void Upgrade()
    {
        level++;
        SetActualSprite();



    }

    /// <summary>
    /// Change bordures color
    /// </summary>
    /// <param name="satured"></param>
    public void SetSatured(bool satured)
    {
        Debug.Log("SetSatured");
        cableSatured = satured;
        if (cableSatured)
        {
            // add color indicator (transparent red)
            transform.GetComponent<SpriteRenderer>().color = colorNonSatured;
        }
        else
        {
            // remove color indicator
            transform.GetComponent<SpriteRenderer>().color = colorSatured;
        }
    }

    public void Delete()
    {

        Destroy(gameObject);
    }
}
