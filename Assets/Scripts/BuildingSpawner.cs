using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

public class BuildingSpawner : MonoBehaviour
{
    [SerializeField] private Transform dataCenters;
    [SerializeField] private Transform houses;
    [SerializeField] private GridManager gridManager;
    [SerializeField] private int minDistanceBetweenDataCenters;

    [SerializeField] private GameObject houseGO;
    [SerializeField] private GameObject datacenterGO;
    [SerializeField] private GameObject moneyManager;
    [SerializeField] private int secondBeforeSpawnHouse = 5;
    [SerializeField] private int dataCenterPrice = 100;
    private Grid<GameObject> _grid;

    private int maxX;
    private int maxY;

    void Start()
    {
        _grid = gridManager.GetGrid();
        maxX = Mathf.FloorToInt(_grid.GetWidth() / 2.0f);
        maxY = Mathf.FloorToInt(_grid.GetHeight() / 2.0f);

        houseGO.transform.localScale = new Vector3(_grid.GetCellSize() * 100 / 512, _grid.GetCellSize() * 100 / 512, _grid.GetCellSize() * 100 / 512);
        datacenterGO.transform.localScale = new Vector3(_grid.GetCellSize() * 100 / 512, _grid.GetCellSize() * 100 / 512, _grid.GetCellSize() * 100 / 512);

        this.SpawnDatacenter(true);
        StartCoroutine("coroutineSpawnHouse");
    }

    void Update()
    {
        if (Input.GetKeyUp(KeyCode.Space))
        {
            //SpawnDatacenter();
            SpawnHouse();
        }
    }

    // Coroutine de spawn des houses

    IEnumerator coroutineSpawnHouse()
    {
        //TODO : Change that by while game is running
        while (true)
        {
            this.SpawnHouse();
            yield return new WaitForSeconds(secondBeforeSpawnHouse);
        }
    }

    public void SpawnDatacenter(bool isPaid = false)
    {
        if (!isPaid)
            isPaid = moneyManager.GetComponent<MoneyManager>().removeMoney(dataCenterPrice);
        if (isPaid)
        {
            for (int i = 0; i < 1000; i++)
            {
                Vector3 pos = new Vector3(
                    Random.Range(-maxX, maxX + 1),
                    Random.Range(-maxY, maxY + 1),
                    0);
                //Debug.Log ("Random pos : " + pos);
                if (_grid.GetValue(pos) == null && !IsTooCloseFromDatacenters(pos))
                {
                    _grid.SetValue(pos, Instantiate(datacenterGO, _grid.GetGridPosition(pos), Quaternion.identity, dataCenters));
                    break;
                }
            }
        }
    }

    private bool IsTooCloseFromDatacenters(Vector3 pos)
    {
        foreach (Transform dataCenter in dataCenters)
        {
            if (Vector3.Distance(pos, dataCenter.position) < minDistanceBetweenDataCenters)
            {
                return true;
            }
        }
        return false;
    }

    public void SpawnHouse () {
        for (int i = 0; i < 1000; i++) {
            var datacenter = dataCenters.GetChild (Random.Range (0, dataCenters.childCount)).position;
            Vector3 pos = new Vector3 (0, 0, 0);
            pos.x = RandomFromDistribution.RandomRangeNormalDistribution (datacenter.x, -maxX, maxX, RandomFromDistribution.ConfidenceLevel_e._60);
            pos.y = RandomFromDistribution.RandomRangeNormalDistribution (datacenter.y, -maxY, maxY, RandomFromDistribution.ConfidenceLevel_e._60);
            //Debug.Log ("House pos : " + pos);
            if (_grid.IsInGrid(pos) && _grid.GetValue(pos) == null)
            {
                _grid.SetValue(pos, Instantiate(houseGO, _grid.GetGridPosition(pos), Quaternion.identity, houses));
                break;
            }

        }
    }
}