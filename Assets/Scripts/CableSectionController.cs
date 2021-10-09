using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
Portion de cable :
 - la portion de cable est contenue sur un case de notre grille, elle est toujours fille d'un cable
 - elle doit être construite conformément aux portions qui l'entourent
   -> lors de sa construction le sprite affiché doit correspondre à la direction de la portion de cable

Attributs : 
 - réf ObjDepart ( encore une fois ça n'a pas d'importance )
 - réf ObjArrivee ( IDEM )
   -> ces références servent à l'affichage de notre portion lors de sa création
 - Peut être autre chose ?
 - List de sprites pour les différents niveau
 - 

Methods :
 - Calcul du sprite en fonction de ObjDepart et objArrivee

Note :
 - Attention à ne pas construire sur une case déjà occupée !
 - Attention à la connexion entre deux câbles cf -> Cable !
 - Honnetement je suis pas trop sûr de comment ça va se passer avec sa mais bon courage :D 
*/

public class CableSectionController : MonoBehaviour
{

    private int level;
    private Sprite actualSprite;
    [SerializeField] public List<Sprite> sprites;
    private Color colorSatured, colorNonSatured;

    private bool cableSatured;

    // Start is called before the first frame update
    void Start()
    {
        level = 0;

        if(sprites == null)
        {
            Debug.LogError("[CableSectionController] Sprites manquant !!!");
        }
        SetActualSprite();

        colorSatured = new Color(255, 0, 0, 255 / 2f);
        colorNonSatured = new Color(255, 255, 255, 255);
    }

    public void SetActualSprite()
    {
        actualSprite = sprites[level];
        transform.GetComponent<SpriteRenderer>().sprite = actualSprite;
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
        cableSatured = satured;
        if (cableSatured)
        {
            // add color indicator (transparent red)
            transform.GetComponent<SpriteRenderer>().color = colorSatured;
        }
        else
        {
            // remove color indicator
            transform.GetComponent<SpriteRenderer>().color = colorNonSatured;
        }
    }

    public void Delete()
    {

        Destroy(gameObject);
    }
}
