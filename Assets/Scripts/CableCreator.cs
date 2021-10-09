using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CableCreator : MonoBehaviour
{
    private Vector2 mousePos;

    public GameObject finalObject;

    private Grid<GameObject> grid;

    [SerializeField] private int height;
    [SerializeField] private int width;
    [SerializeField] private LayerMask allTilesLayer;

    void Start()
    {
        grid = new Grid<GameObject> (width, height, 3f, new Vector3 (-15,-10,0));
    }

    // Update is called once per frame
    void Update()
    {
        mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        transform.position = new Vector2(Mathf.Round(mousePos.x), Mathf.Round(mousePos.y));
        if(Input.GetMouseButton(0))
        {
            RaycastHit2D rayHit = Physics2D.Raycast(transform.position, Vector2.zero, Mathf.Infinity, allTilesLayer);
            
            if (rayHit.collider == null)
            {
                if (mousePos.x >= grid.GetOriginPosition().x && mousePos.y >= grid.GetOriginPosition().y && mousePos.x < grid.GetWidth() && mousePos.y < grid.GetHeight()) 
                {
                    Instantiate(finalObject, grid.GetGridPosition(mousePos), Quaternion.identity);
                } 
                else 
                {
                    Debug.LogError ("INDEX OUT OF GRID ARRAY, U RETARDED (set value)");
                }
            }
        }
    }
}