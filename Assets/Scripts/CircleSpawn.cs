using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CircleSpawn : MonoBehaviour
{
    [SerializeField] private GameObject circleSpawn;
    
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(CircleSpawnThread());
    }
    
    IEnumerator CircleSpawnThread()
    {
        float maxScale = 2.5f;
        float stepScale = 0.05f;
        while (circleSpawn.transform.localScale.x < maxScale)
        {
            circleSpawn.transform.localScale = Vector3.MoveTowards(circleSpawn.transform.localScale, new Vector2(maxScale, maxScale), stepScale);
            yield return null;
        }
        circleSpawn.transform.localScale = new Vector2(0.5f,0.5f);
        yield return new WaitForSeconds(0.25f);
        while (circleSpawn.transform.localScale.x < maxScale)
        {
            circleSpawn.transform.localScale = Vector3.MoveTowards(circleSpawn.transform.localScale, new Vector2(maxScale, maxScale), stepScale);
            yield return null;
        }
        Destroy(circleSpawn);
    }
}