using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


public class FindObjsToFade : MonoBehaviour
{

    private Transform player;
    public RaycastHit[] hits = new RaycastHit[6];
    
    public int numOfHits;

    private CapsuleCollider col;

    // Start is called before the first frame update
    void Awake()
    {
        player = GameObject.Find("Player").transform;

        col = player.GetComponent<CapsuleCollider>();


    }

    // Update is called once per frame
    void Update()
    {

        var direction = new Vector3 {[col.direction] = 1};
        var offset = (col.height) / 2 - col.radius;

        Vector3 point1 = transform.TransformPoint(col.center - direction * (offset+0.1f));
        Vector3 point2 = transform.TransformPoint(col.center + direction * (offset+0.1f));

        Vector3 directionCast = (player.position - transform.position).normalized;
        

        //Every object that should be transparentable needs to belong to one of these layers, can add layers as needed here
        int layers = LayerMask.GetMask("Default", "Ground", "StableGround");

        
        numOfHits = Physics.CapsuleCastNonAlloc(
            point1, point2, col.radius-0.1f, directionCast, hits, 
            (player.position - transform.position).magnitude,
            layers, QueryTriggerInteraction.Ignore);

        if (numOfHits == 0) {Array.Clear(hits,0,hits.Length);}

        Debug.DrawLine(player.position, transform.position, Color.green);

        for (int i = 0; i < numOfHits; i++){
            ObjectFader fader = hits[i].transform.GetComponent<ObjectFader>();
            if (fader != null)
            {
                
                if (!fader.IsFade()){
                    fader.Fade();
                }
            }
            
        }

        
    
    }

}
