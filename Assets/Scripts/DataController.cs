using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class DataController : MonoBehaviour
{

    [SerializeField] private GameObject objDepart;

    [SerializeField]
    private GameObject objArrive;

    private GameObject dataCenter;
    private int indexChild = 0;
    private bool direction;
    [SerializeField]
    private float speed = 2f;
    private void Start()
    {
        var trs = transform;
        name = Random.Range(0, 1000).ToString() + "'s data";
        //Initial objDepart is it's parent aka HouseObject
        objDepart = trs.parent.gameObject;
        trs.position = objDepart.transform.position;
        objArrive = objDepart.GetComponent<HouseController>().GetConnectedCable();
        dataCenter = SelectRandomDataCenter();
        GetComponent<SpriteRenderer>().sortingOrder = 4;
        InitializeIndex();
    }
    private void FixedUpdate()
    {
        if (objArrive == null)
            return;
        var step = speed * Time.deltaTime;

        transform.position = Vector3.MoveTowards(transform.position, objArrive.transform.position, step);
        if (transform.position != objArrive.transform.position) return;
        if (direction) indexChild++;
        else indexChild--;
        if (indexChild < 0 || indexChild >= objArrive.transform.parent.childCount)
        {
            var cableController = objArrive.transform.parent.GetComponent<CableController>();
            var endCable = direction ? cableController.GetEnd() : cableController.GetBegin();
            transform.position = Vector3.MoveTowards(transform.position, endCable.transform.position, Single.PositiveInfinity);

            if (endCable.CompareTag("Router"))
            {
                cableController.RemoveData(gameObject);
                objDepart = endCable;
                objArrive = endCable.GetComponent<RouterController>().GetShortestPath(dataCenter);
                if (objArrive == null)
                    Delete(false);
                indexChild = InitializeIndex();

            }
            else if (endCable.CompareTag("DataCenter"))
            {
                endCable.GetComponent<DatacenterController>().AddNewDataToWaitingList(this);
                cableController.RemoveData(gameObject);
                objArrive = null;
            }

        }
        else if (objArrive != null && (objArrive.CompareTag("Router") || objArrive.CompareTag("DataCenter")))
        {
            if (objArrive.CompareTag("Router"))
            {
                objDepart = objArrive;
                objArrive = objArrive.GetComponent<RouterController>().GetShortestPath(dataCenter);
                if (objArrive == null)
                    Delete(false);
                indexChild = InitializeIndex();

            }
            else if (objArrive.CompareTag("DataCenter"))
            {
                objArrive.GetComponent<DatacenterController>().AddNewDataToWaitingList(this);
                objArrive = null;
            }
        }
        else
        {
            objArrive = objArrive.transform.parent.GetChild(indexChild).gameObject;
        }

    }

    private GameObject SelectRandomDataCenter()
    {
        var dataCenters = GameObject.Find("DataCenters").transform;
        var indexDcSelected = Random.Range(0, dataCenters.childCount);
        return dataCenters.GetChild(indexDcSelected).gameObject;
    }

    private int InitializeIndex()
    {
        if (objArrive == null)
            return 0;
        var parentObj = objArrive.transform.parent;
        parentObj.GetComponent<CableController>().AddData(gameObject);
        direction = objArrive.Equals(parentObj.GetChild(0).gameObject);
        return direction ? 0 : parentObj.childCount;
    }

    public void Delete(bool isSatisfate)
    {

        gameObject.GetComponentInParent<HouseController>().SetIsSatified(isSatisfate);
        Destroy(this.gameObject);
    }

    public void OnDestroy()
    {
        Debug.Log(name + "destroyed");
    }
}