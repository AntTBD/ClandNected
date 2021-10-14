using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Utils;

public class CableCreator : MonoBehaviour
{

    private Vector2 mousePos;
    private Grid<GameObject> grid;
    [SerializeField] private GridManager gridManager;
    [SerializeField] private GameObject prefabCables;
    [SerializeField] private GameObject prefabRouter;
    public GameObject pieces;
    private GameObject currentFather;
    private GameObject lastDrawn;
    private CableController _cableController;
    private GameObject depart = null;
    private GameObject arrivee = null;

    void Start()
    {
        mousePos = GetMouseWorldPosition();
        grid = gridManager.GetGrid();
    }

    // Update is called once per frame
    void Update()
    {
        try// test if currentFaher was destroy => in this cas, recreate it
        {
            if (currentFather.gameObject == null) currentFather = new GameObject();
        }
        catch (Exception)
        {
            currentFather = new GameObject();
        }

        mousePos = GetMouseWorldPosition();

        if (depart != null && currentFather != null)
        {
            //Si on est sur point de départ recevable
            if (Input.GetMouseButton(0) && grid.IsInGrid(mousePos))
            {
                DrawPointsPath();
            }
        }
        else if (Input.GetMouseButtonDown(0))
        {
            //On récupère le point de départ du potentiel tuyau
            Vector3 gridPosition = grid.GetXY(mousePos);
            if (grid.IsInGrid(mousePos))
            {
                depart = grid.gridArray[(int)gridPosition.x, (int)gridPosition.y];
                lastDrawn = depart;
            }


            //Si on est sur un point de départ recevable alors on crée un tuyau
            if (depart != null && grid.IsInGrid(mousePos))
            {
                currentFather = Instantiate(prefabCables, Vector3.zero, Quaternion.identity);
                _cableController = currentFather.GetComponent<CableController>();
                _cableController.SetBegin(depart);
            }
        }

        if (Input.GetMouseButtonUp(0))
        {
            //Verification de la position de la souris lorsque l'on relache le bouton
            Vector3 gridPosition = grid.GetXY(mousePos);
            if (grid.IsInGrid(mousePos))
            {
                arrivee = grid.gridArray[(int)gridPosition.x, (int)gridPosition.y];
            }
            else
            {
                arrivee = null;
            }

            if (arrivee != null && canDraw())
            {
                _cableController.SetEnd(arrivee);
                switch (arrivee.tag)
                {
                    case "Maison":
                        {
                            HouseController myTargetHouseController = arrivee.GetComponent<HouseController>();

                            if (!depart.CompareTag("Maison") && myTargetHouseController.GetConnectedCable() == null)
                            {
                                SetUpStartCable();
                                myTargetHouseController.ConnectTo(currentFather);
                            }
                            else
                            {
                                Destroy(currentFather);
                            }
                            break;
                        }
                    case "Router":
                        {
                            SetUpStartCable();
                            arrivee.GetComponent<RouterController>().addPort(_cableController.gameObject);
                            break;
                        }

                    case "DataCenter":
                        {
                            SetUpStartCable();
                            arrivee.GetComponent<DatacenterController>().ConnectNewCable(_cableController);
                            break;
                        }

                    case "CableSection":

                        if (currentFather.transform.childCount == 0)
                        {
                        }
                        //Creation routeur adjacent ?
                        else if (arrivee != currentFather.transform.GetChild(currentFather.transform.childCount - 1).gameObject)
                        {
                            // si house deja connectee on ne fait rien
                            if (_cableController.GetBegin().CompareTag("Maison") && _cableController.GetBegin().GetComponent<HouseController>().GetConnectedCable() != null)
                            {
                                Destroy(currentFather);
                                break;
                            }
                            SetUpStartCable();
                            // test if currentFather was destroyed in SetUpStartCable()
                            try
                            {
                                if (currentFather.gameObject == null) break;
                            }
                            catch (Exception)
                            {
                                break;
                            }
                            GameObject newRouter = Instantiate(prefabRouter, arrivee.transform.position, Quaternion.identity, GameObject.Find("Routers").transform);
                            _cableController.SetEnd(newRouter);
                            if(arrivee.transform.parent.GetComponent<CableController>().Diviser(newRouter, prefabCables))
                            {
                                Debug.LogError("Division failed");
                                Destroy(newRouter);
                                break;
                            }

                            grid.SetValue(arrivee.transform.position, newRouter);
                            arrivee = grid.GetValue(arrivee.transform.position);
                            _cableController.SetEnd(arrivee);
                            arrivee.GetComponent<RouterController>().addPort(_cableController.gameObject);
                        }
                        else
                        {
                            Destroy(currentFather);
                        }
                        break;
                    default:
                        Destroy(currentFather);
                        break;
                }
                DrawCable();
            }
            else
            {
                Destroy(currentFather);
            }

            depart = null;
            currentFather = null;
        }
    }

    public void DrawCable()
    {
        //Dessin
        for (int i = 0; i < currentFather.transform.childCount; i++)
        {
            Vector3 posPrev;
            Vector3 posCurrent;
            Vector3 posNext;

            if (i == 0)
            {
                posPrev = depart.transform.position;
                posCurrent = currentFather.transform.GetChild(i).position;

                if (currentFather.transform.childCount == 1)
                {
                    posNext = arrivee.transform.position;
                }
                else
                {
                    posNext = currentFather.transform.GetChild(i + 1).position;
                }
            }
            else if (i == currentFather.transform.childCount - 1)
            {
                posPrev = currentFather.transform.GetChild(i - 1).position;
                posCurrent = currentFather.transform.GetChild(i).position;
                posNext = arrivee.transform.position;
            }
            else
            {
                posPrev = currentFather.transform.GetChild(i - 1).position;
                posCurrent = currentFather.transform.GetChild(i).position;
                posNext = currentFather.transform.GetChild(i + 1).position;
            }

            ChooseSprite(posPrev, posCurrent, posNext, i);
        }

    }

    public void ChooseSprite(Vector3 prev, Vector3 current, Vector3 next, int index)
    {
        string dir = "";
        CableSectionController sr = currentFather.transform.GetChild(index).gameObject.GetComponent<CableSectionController>();

        prev = Vector3Int.RoundToInt(prev);
        next = Vector3Int.RoundToInt(next);

        if (prev.x == current.x)
        {
            dir += prev.y > next.y ? "S" : prev.y < next.y ? "N" : "";
            dir += prev.x > next.x ? "O" : prev.x < next.x ? "E" : "";
        }
        else
        {
            dir += prev.x > next.x ? "O" : prev.x < next.x ? "E" : "";
            dir += prev.y > next.y ? "S" : prev.y < next.y ? "N" : "";
        }
        switch (dir)
        {
            case "NE":
            case "OS":
                sr.isCorner = true;
                sr.SetActualSprite(true);
                currentFather.transform.GetChild(index).rotation = Quaternion.Euler(new Vector3(0, 0, -90));
                break;

            case "SE":
            case "ON":
                sr.isCorner = true;
                sr.SetActualSprite(true);
                currentFather.transform.GetChild(index).rotation = Quaternion.Euler(new Vector3(0, 0, 0));
                break;

            case "SO":
            case "EN":
                sr.isCorner = true;
                sr.SetActualSprite(true);
                currentFather.transform.GetChild(index).rotation = Quaternion.Euler(new Vector3(0, 0, 90));
                break;

            case "NO":
            case "ES":
                sr.isCorner = true;
                sr.SetActualSprite(true);
                currentFather.transform.GetChild(index).rotation = Quaternion.Euler(new Vector3(0, 0, 180));
                break;

            case "O":
            case "E":
                sr.isCorner = false;
                sr.SetActualSprite(true);
                break;

            case "N":
            case "S":
                sr.isCorner = false;
                sr.SetActualSprite(true);
                currentFather.transform.GetChild(index).rotation = Quaternion.Euler(0, 0, 90);
                break;
            default:
                Debug.LogError(currentFather.transform.GetChild(index).name +": cable section orientation error!");
                break;
        }
    }

    public void DrawPointsPath()
    {
        //On teste si la case est libre        
        if (canDraw())
        {
            PlaceObject(pieces);
        }
    }

    public bool isAdjacent(int mouseX, int mouseY, int lastDrawnX, int lastDrawnY)
    {
        if (Mathf.Abs(mouseX - lastDrawnX) + Mathf.Abs(mouseY - lastDrawnY) < 2)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public GameObject CheckNeighbors(Vector3 pos)
    {
        Vector3 gridPos = grid.GetXY(pos);
        float x = gridPos.x;
        float y = gridPos.y;

        return null;
    }

    public bool canDraw()
    {
        if (currentFather != null)
        {
            int lastDrawnX, lastDrawnY;

            Vector3 gridPosition = grid.GetXY(mousePos);
            int mouseX = (int)gridPosition.x;
            int mouseY = (int)gridPosition.y;

            if (currentFather.transform.childCount != 0)
            {
                Transform lastDrawnTransform = currentFather.transform.GetChild(currentFather.transform.childCount - 1);
                Vector3 lastDrawnPosition = lastDrawnTransform.position;
                Vector3 lastDrawnGridPosition = grid.GetXY(lastDrawnPosition);
                lastDrawnX = (int)lastDrawnGridPosition.x;
                lastDrawnY = (int)lastDrawnGridPosition.y;
            }
            else
            {
                Vector3 lastDrawnGridPosition = grid.GetXY(depart.transform.position);
                lastDrawnX = (int)lastDrawnGridPosition.x;
                lastDrawnY = (int)lastDrawnGridPosition.y;
            }

            return isAdjacent(mouseX, mouseY, lastDrawnX, lastDrawnY);
        }
        return false;
    }

    private void SetUpStartCable()
    {
        switch (depart.tag)
        {
            case "Maison":
                {
                    HouseController myTargetHouseController = depart.GetComponent<HouseController>();

                    if (!arrivee.CompareTag("Maison") && myTargetHouseController.GetConnectedCable() == null)
                    {
                        myTargetHouseController.ConnectTo(currentFather);
                    }
                    else
                    {
                        Destroy(currentFather);
                    }
                    break;
                }
            case "Router":
                {
                    depart.GetComponent<RouterController>().addPort(currentFather);
                    break;
                }

            case "DataCenter":
                {
                    depart.GetComponent<DatacenterController>().ConnectNewCable(_cableController);
                    break;
                }
            case "CableSection":
                {
                    // si house deja connectee on ne fait rien
                    if (_cableController.GetEnd().CompareTag("Maison") && _cableController.GetEnd().GetComponent<HouseController>().GetConnectedCable() != null)
                    {
                        Destroy(currentFather);
                        break;
                    }
                    //Faire section
                    GameObject newRouter = Instantiate(prefabRouter, depart.transform.position, Quaternion.identity, GameObject.Find("Routers").transform);
                    _cableController.SetBegin(newRouter);
                    depart.transform.parent.GetComponent<CableController>().Diviser(newRouter, prefabCables);

                    grid.SetValue(depart.transform.position, newRouter);
                    depart = grid.GetValue(depart.transform.position);
                    _cableController.SetBegin(depart);
                    newRouter.GetComponent<RouterController>().addPort(_cableController.gameObject);

                }
                break;


            default:
                Destroy(currentFather);
                break;
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

    private void PlaceObject(GameObject objectToPlace)
    {
        Vector3 posXY = grid.GetXY(mousePos);
        int x = (int)posXY.x;
        int y = (int)posXY.y;

        if (grid.GetValue(mousePos) == null)
        {
            Place(mousePos, Quaternion.identity, new Vector2(x, y), objectToPlace);
        }
    }

    private void InitialPlace(Vector3 placePos, Quaternion placeRot, Vector2 gridPos, GameObject objectToPlace)
    {
        GameObject placedObject = Instantiate(objectToPlace, grid.GetGridPosition(placePos), placeRot);

        //Adapte la taille du sprite aux cases
        placedObject.transform.localScale = new Vector3(grid.GetCellSize() * 100 / 512, grid.GetCellSize() * 100 / 512, grid.GetCellSize() * 100 / 512);

        grid.SetValue(placePos, placedObject);
    }

    private void Place(Vector3 placePos, Quaternion placeRot, Vector2 gridPos, GameObject objectToPlace)
    {
        GameObject placedObject = Instantiate(objectToPlace, grid.GetGridPosition(mousePos), placeRot);

        //Adapte la taille du sprite aux cases
        placedObject.transform.localScale = new Vector3(grid.GetCellSize() * 100 / 512, grid.GetCellSize() * 100 / 512, grid.GetCellSize() * 100 / 512);

        //placedObject.transform.parent = currentFather.transform;
        currentFather.GetComponent<CableController>().AddSection(placedObject);
        grid.SetValue(placePos, placedObject);
    }

}