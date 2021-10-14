using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class DataController : MonoBehaviour
{

    [SerializeField] private GameObject objDepart;
    [SerializeField] private GameObject objArrive;


    [SerializeField] private GameObject dataCenter;
    [SerializeField] private int indexChild = 0;
    [SerializeField] private bool direction;
    [SerializeField]
    private float speed = 2f;

    private void Start()
    {
        var trs = transform;
        name = "Data " + Random.Range(0, 10000).ToString();
        //Initial objDepart is it's parent aka HouseObject
        objDepart = trs.parent.gameObject;
        trs.position = objDepart.transform.position;
        objArrive = objDepart.GetComponent<HouseController>().GetConnectedCable();//recuperation du premier cable
        dataCenter = SelectRandomDataCenter();
        GetComponent<SpriteRenderer>().sortingOrder = 4;
        GetComponent<SpriteRenderer>().color = dataCenter.GetComponent<DatacenterController>().datasColor;
        InitializeIndex();
    }
    private void FixedUpdate()
    {
        int etat = arrivedAtTheEndOfTheCable();
        var step = speed * Time.deltaTime;


        switch (etat)
        {
            case 0:
                {
                    //Debug.Log(name + "Etat 0");
                    /*CableController cable = objArrive.GetComponent<CableController>();
                    if (cable.transform.childCount - 1 > indexChild)
                    {
                        cable.AddData(gameObject);
                        direction = cable.GetBegin().Equals(objDepart);// debut du cable == obj de départ
                        indexChild = direction ? 0 : cable.transform.childCount - 1;
                    }*/
                    int indexChildTemp;
                    if (direction) indexChildTemp = indexChild;
                    else indexChildTemp = (objArrive.transform.childCount - 1) - indexChild;// sens inverse
                    if (indexChildTemp >= 0)
                    {
                        //Debug.Log(name + " Etat 0 | index : " + indexChildTemp + " = size:" + (objArrive.transform.childCount - 1) + " - id:" + indexChild);
                        // movement dans le cable
                        transform.position = Vector3.MoveTowards(transform.position, objArrive.transform.GetChild(indexChildTemp).position, step);
                        if (transform.position != objArrive.transform.GetChild(indexChildTemp).position)
                        {
                            return;
                        }
                        else indexChild++;
                    }
                    break;
                }
            case 1:
            case 2:
                {
                    //Debug.Log(name + "Etat 1&2");
                    CableController cable = objArrive.GetComponent<CableController>();
                    // movement jusqu'au point de connexion du cable
                    if (direction)
                    {
                        transform.position = Vector3.MoveTowards(transform.position, cable.GetEnd().transform.position, step);
                        if (transform.position == cable.GetEnd().transform.position)
                        {
                            cable.RemoveData(gameObject);
                            objArrive = cable.GetEnd();
                        }
                    }
                    else
                    {
                        transform.position = Vector3.MoveTowards(transform.position, cable.GetBegin().transform.position, step);
                        if (transform.position == cable.GetBegin().transform.position)
                        {
                            cable.RemoveData(gameObject);
                            objArrive = cable.GetBegin();
                        }
                    }
                    break;
                }
            case 3:
                {
                    //Debug.Log(name + "Etat 3");
                    if (objArrive.CompareTag("Router"))
                    {
                        objDepart = objArrive;
                        objArrive = objArrive.GetComponent<RouterController>().GetShortestPath(dataCenter);
                        if (objArrive == null)// on error, route not find
                            Delete(false);
                        indexChild = InitializeIndex();

                    }
                    else if (objArrive.CompareTag("DataCenter"))
                    {
                        objArrive.GetComponent<DatacenterController>().AddNewDataToWaitingList(this);
                        objArrive = null;
                    }
                    break;
                }
            default:
                //Debug.Log(name + " Etat error");
                break;
        }
    }

    /// <summary>
    /// 3 = arrived at router or datacenter<br/>
    /// 2 = arrive at the end of the cable<br/>
    /// 1 = in cable with 0 sections<br/>
    /// 0 = in cable<br/>
    /// -1 = error
    /// </summary>
    /// <returns></returns>
    private int arrivedAtTheEndOfTheCable()
    {
        if (objArrive != null && (objArrive.CompareTag("Router") || objArrive.CompareTag("DataCenter")))// si le cable n'a pas de section
        {
            return 3;
        }
        else if (objArrive != null && objArrive.CompareTag("Cable")) {// si dans le cable
            if (objArrive.transform.childCount > 0)// si cable comporte 1 ou plusieurs sections
            {
                if (direction && indexChild > (objArrive.transform.childCount - 1)) // si de objDepart vers objArrive && si on a dépassé la derniere section
                {
                    return 2;
                }
                else if (!direction && (objArrive.transform.childCount - 1) - indexChild < 0) // si de objArrive vers objDepart && si on a dépassé la premiere section
                {
                    return 2;
                }
                else // si on est au milieu du cable
                {
                    return 0;
                }
            }
            else// si longueur cable =0
            {
                return 1;
            }
        }
        else // error
        {
            return -1;
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
            return -1;
        var cable = objArrive.transform;
        cable.GetComponent<CableController>().AddData(gameObject);
        direction = objArrive.GetComponent<CableController>().GetBegin().Equals(objDepart);// debut du cable == obj de départ
        return 0; // first element (la gestion du sens se fait apres)
    }

    public void Delete(bool isSatisfate)
    {

        gameObject.GetComponentInParent<HouseController>().SetIsSatified(isSatisfate);
        Destroy(gameObject);
    }

    public void OnDestroy()
    {
        Debug.Log(name + " destroyed");
    }

    public GameObject GetDatacenterOfDestination()
    {
        return dataCenter;
    }
}