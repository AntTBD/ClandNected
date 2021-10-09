using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class DataController : MonoBehaviour
{
    private GameObject objDepart;

    private GameObject objArrive;

    private GameObject dataCenter;
    private float speed = 15f;
    private void Start()
    {
        var trs = transform;
        //Initial objDepart is it's parent aka HouseObject
        objDepart = trs.parent.gameObject;
        trs.position = objDepart.transform.position;
        dataCenter = SelectRandomDataCenter();
        Debug.Log("Iniatlization finished");
        GetComponent<SpriteRenderer>().sortingOrder = 1;
    }
    public void FixedUpdate()
    {
        float step = speed * Time.deltaTime;
        transform.position=Vector3.MoveTowards(transform.position, dataCenter.transform.position, step);
        if (transform.position.Equals())
        {
            dataCenter = SelectRandomDataCenter();
        }
    }

    private GameObject SelectRandomDataCenter()
    {
        var dataCenters = GameObject.Find("DataCenters").transform;
        var indexDcSelected = Random.Range(0, dataCenters.childCount);
        return dataCenters.GetChild(indexDcSelected).gameObject;
    }
}
