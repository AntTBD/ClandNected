using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Utils;

public class GridTester : MonoBehaviour
{
    private Grid<bool> grid;

    [SerializeField] private int height;
    [SerializeField] private int width;

    // Start is called before the first frame update
    private void Start()
    {
        grid = new Grid<bool>(width, height, 1.5f, new Vector3(0, 0, 0), true);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            grid.SetValue(GetMouseWorldPosition(), true);
        }

        if (Input.GetMouseButtonDown(1))
        {
            Debug.Log(grid.GetValue(GetMouseWorldPosition()));
        }
    }

}