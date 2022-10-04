using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class FollowerList : MonoBehaviour
{
    public List<GameObject> follower;
    public bool clearList = false;

    // public void RemoveBall(bool work)
    // {
    //     clearList = work;
    //     
    // }

    private void Update()
    {
        follower.RemoveAll((obje => obje == null));
        if (clearList)
        {
            if (follower.Count > 0)
            {
                var ballIndex = follower.Find(x => x.name == "ballwithTexture(Clone)");
                follower.Remove(ballIndex);
            }
            else
            {
                clearList = false;
            }
        }

        
    }

    public void SaveBall(GameObject obje)
    {
        follower.Add(obje);
    }

    public GameObject ReturnLast()
    {
        return follower.Last();
    }
}
