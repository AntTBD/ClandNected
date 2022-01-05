using System;
using System.Collections;
using TMPro;
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

    private float maxX;
    private float maxY;

    [SerializeField] private TextMeshProUGUI datacenterNumberValue;
    [SerializeField] private TextMeshProUGUI houseNumberValue;

    void Start()
    {
        _grid = gridManager.GetGrid();
        maxX = _grid.GetWidth() / 2.0f;
        maxY = _grid.GetHeight() / 2.0f;

        houseGO.transform.localScale = new Vector3(_grid.GetCellSize() * 1.3f, _grid.GetCellSize() * 1.3f, 1);
        datacenterGO.transform.localScale = new Vector3(_grid.GetCellSize() * 1.3f, _grid.GetCellSize() * 1.3f, 1);

        if (!datacenterNumberValue)
            datacenterNumberValue = GameObject.Find("datacenterNumberValue").GetComponent<TextMeshProUGUI>();
        if (!houseNumberValue)
            houseNumberValue = GameObject.Find("houseNumberValue").GetComponent<TextMeshProUGUI>();

        this.SpawnDatacenter(true);
        StartCoroutine(CoroutineSpawnHouse());
    }

#if UNITY_EDITOR
    void Update()
    {
        if (Input.GetKeyUp(KeyCode.Space))
        {
            //SpawnDatacenter();
            SpawnHouse();
            SpawnHouse();
            SpawnHouse();
            SpawnHouse();
            SpawnHouse();
            SpawnHouse();
            SpawnHouse();
            SpawnHouse();
            SpawnHouse();
            SpawnHouse();
            SpawnHouse();
            SpawnHouse();
            SpawnHouse();
            SpawnHouse();
            SpawnHouse();
            SpawnHouse();
            SpawnHouse();
            SpawnHouse();
            SpawnHouse();
            SpawnHouse();
            SpawnHouse();
            SpawnHouse();
            SpawnHouse();
            SpawnHouse();
            SpawnHouse();
            SpawnHouse();
            SpawnHouse();
            SpawnHouse();
            SpawnHouse();
            SpawnHouse();
            SpawnHouse();
            SpawnHouse();
            SpawnHouse();
            SpawnHouse();
            SpawnHouse();
            SpawnHouse();
            SpawnHouse();
            SpawnHouse();
            SpawnHouse();
            SpawnHouse();
            SpawnHouse();
            SpawnHouse();
        }
    }
#endif
    // Coroutine de spawn des houses

    IEnumerator CoroutineSpawnHouse()
    {
        //TODO : Change that by while game is running
        while (!_grid.IsFull())
        {
            this.SpawnHouse();
            yield return new WaitForSeconds(secondBeforeSpawnHouse);
        }
    }

    public void SpawnDatacenter(bool isPaid = false)
    {
        if(_grid.IsFull()) return;
        
        if (!isPaid)
            isPaid = moneyManager.GetComponent<MoneyManager>().removeMoney(dataCenterPrice);
        
        if (isPaid)
        {
            for (int i = 0; i < 1000; i++)
            {
                Vector3 pos = new Vector3(
                    Random.Range(-maxX, maxX),
                    Random.Range(-maxY, maxY),
                    0);
                if (_grid.GetValue(pos) == null && !IsTooCloseFromDatacenters(pos))
                {
                    _grid.SetValue(pos, Instantiate(datacenterGO, _grid.GetGridPosition(pos), Quaternion.identity, dataCenters));
                    datacenterNumberValue.text = dataCenters.childCount.ToString();
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

    public void SpawnHouse()
    {
        if(_grid.IsFull()) return;
        
        for (int i = 0; i < 1000; i++)
        {
            var datacenter = dataCenters.GetChild(Random.Range(0, dataCenters.childCount)).position;
            Vector3 pos = new Vector3(0, 0, 0);
            pos.x = RandomFromDistribution.RandomRangeNormalDistribution(datacenter.x, -maxX, maxX, RandomFromDistribution.ConfidenceLevel_e._60);
            pos.y = RandomFromDistribution.RandomRangeNormalDistribution(datacenter.y, -maxY, maxY, RandomFromDistribution.ConfidenceLevel_e._60);

            if (_grid.IsInGrid(pos) && _grid.GetValue(pos) == null)
            {
                _grid.SetValue(pos, Instantiate(houseGO, _grid.GetGridPosition(pos), Quaternion.identity, houses));
                houseNumberValue.text = houses.childCount.ToString();
                break;
            }

        }
    }

    private void OnDestroy()
    {
        StopCoroutine(CoroutineSpawnHouse());
    }
}