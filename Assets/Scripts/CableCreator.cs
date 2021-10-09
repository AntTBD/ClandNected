using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CableCreator : MonoBehaviour
{

    //public Grid grid;

    public Texture2D tex;
    private Sprite mySprite;
    private SpriteRenderer sr;

    public GameObject cube;

    void Awake()
    {
        sr = gameObject.AddComponent<SpriteRenderer>() as SpriteRenderer;
        sr.color = new Color(0.9f, 0.9f, 0.9f, 1.0f);

        transform.position = new Vector3(1.5f, 1.5f, 0.0f);
    }

    void Start()
    {
        mySprite = Sprite.Create(tex, new Rect(0.0f, 0.0f, tex.width, tex.height), new Vector2(0.5f, 0.5f), 100.0f);
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButton(0))
        {
            sr.sprite = mySprite;
            cube.SetActive(true);
            Debug.Log("pressed");
        }
        else
        {
            cube.SetActive(false);
            Destroy(mySprite);
        }
    }
}
