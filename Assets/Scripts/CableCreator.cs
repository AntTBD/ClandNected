using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Utils;

public class CableCreator : MonoBehaviour
{
    private Vector2 mousePos;

    public CableType[] cableTypes;

    private Grid<GameObject> grid;
    public Grid<GameObject>.Direction dir;
    public GameObject tempObject;

    private GameObject depart = null;

    [SerializeField] private int height;
    [SerializeField] private int width;

    void Start()
    {
        mousePos = GetMouseWorldPosition();
        grid = new Grid<GameObject>(width, height, 1.5f, new Vector3(0, 0, 0), true);
        Place(mousePos,Quaternion.identity,new Vector2(3,3),tempObject);
    }

    // Update is called once per frame
    void Update()
    {
        mousePos = GetMouseWorldPosition();

        if(depart != null)
        {
            if (Input.GetMouseButton(0) && grid.IsInGrid(mousePos) && depart.tag == "Maison")
            {
                PlaceObject();
            }
        }
        else if (Input.GetMouseButtonDown(0))
        {
            Vector3 gridPosition = grid.GetXY(mousePos);
            depart = grid.gridArray[(int)gridPosition.x,(int)gridPosition.y];
        }

        if (Input.GetMouseButtonUp(0))
        {
            depart = null;
        }
    }

    /*private void DestroyObject(Vector2 gridPos)
    {
        int x = (int)gridPos.x;
        int y = (int)gridPos.y;

        if (grid.GetValue(x, y) != null)
        {
            Destroy(grid.gridArray[x, y]);
            grid.SetValue(x, y, null);

            for (int i = 1; i < 5; i++)
            {
                Vector2 checkPos;

                switch (i)
                {
                    default:
                    case 1:
                        // down
                        checkPos = new Vector2(x, y - 1);
                        break;
                    case 2:
                        // right
                        checkPos = new Vector2(x + 1, y);
                        break;
                    case 3:
                        // up
                        checkPos = new Vector2(x, y + 1);
                        break;
                    case 4:
                        // left
                        checkPos = new Vector2(x - 1, y);
                        break;
                }

                int checkX = (int)checkPos.x;
                int checkY = (int)checkPos.y;

                if (grid.GetValue(checkX, checkY) != null)
                {
                    if (grid.GetValue(checkX, checkY).name.Contains("Cable"))
                    {
                        Vector3 thisPlacePos = grid.GetWorldPosition(checkX, checkY) + new Vector3(grid.GetCellSize(), 0, grid.GetCellSize()) * 0.5f;
                        GameObject thisObjectToPlace = GetCableType(checkX, checkY);

                        Place(thisPlacePos, Quaternion.Euler(0, grid.GetRotationAngle(dir), 0), checkPos, thisObjectToPlace);
                    }
                }
            }
        }
    }*/

    private void PlaceObject()
    {
        Vector3 posXY = grid.GetXY(mousePos);
        int x = (int)posXY.x;
        int y = (int)posXY.y;

        if (grid.GetValue(mousePos) == null)
        {
            GameObject objectToPlace =tempObject;
            Place(mousePos,Quaternion.identity,new Vector2(x,y),objectToPlace);

            /*for (int i = 1; i < 5; i++)
            {
                Vector2 checkPos;

                switch (i)
                {
                    default:
                    case 1:
                        // down
                        checkPos = new Vector2(x, y - 1);
                        break;
                    case 2:
                        // right
                        checkPos = new Vector2(x + 1, y);
                        break;
                    case 3:
                        // up
                        checkPos = new Vector2(x, y + 1);
                        break;
                    case 4:
                        // left
                        checkPos = new Vector2(x - 1, y);
                        break;
                }

                int checkX = (int)checkPos.x;
                int checkY = (int)checkPos.y;

                if (grid.GetValue(checkX, checkY) != null)
                {
                    if (grid.GetValue(checkX, checkY).name.Contains("Road"))
                    {
                        Vector3 thisPlacePos = grid.GetWorldPosition(checkX, checkY) + new Vector3(grid.GetCellSize(), 0, grid.GetCellSize()) * 0.5f;
                        GameObject thisObjectToPlace = GetCableType(checkX, checkY);
                        Place(thisPlacePos, Quaternion.Euler(0, grid.GetRotationAngle(dir), 0), checkPos, thisObjectToPlace);
                    }
                }
            }*/
        }
    }

    private void Place(Vector3 placePos, Quaternion placeRot, Vector2 gridPos, GameObject objectToPlace)
    {
        GameObject placedObject = Instantiate(objectToPlace, grid.GetGridPosition(mousePos), placeRot);

        //Adapte la taille du sprite aux cases
        placedObject.transform.localScale = new Vector3(grid.GetCellSize()*100/512,grid.GetCellSize()*100/512,grid.GetCellSize()*100/512);

        grid.SetValue(mousePos, placedObject);
    }

    [System.Serializable]
    public class CableType
    {
        public GameObject cablePrefab;
        public bool down;
        public bool right;
        public bool up;
        public bool left;
    }
}