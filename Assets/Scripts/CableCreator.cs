using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Utils;

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
        grid = new Grid<GameObject>(width, height, 1.5f, new Vector3(0, 0, 0), true);
    }

    // Update is called once per frame
    void Update()
    {
        mousePos = GetMouseWorldPosition();
        if (Input.GetMouseButton(0) && grid.IsInGrid(mousePos))
        {
            if (grid.GetValue(mousePos) == null)
            {
                grid.SetValue(mousePos, Instantiate(finalObject, grid.GetGridPosition(mousePos), Quaternion.identity));
            }
        }
    }
}