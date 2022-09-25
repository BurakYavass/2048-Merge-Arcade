using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class FollowerList : MonoBehaviour
{
    public List<GameObject> follower;
    [SerializeField] private GameObject creatBall;
    private bool clearList;

    private void Start()
    {
        GameEventHandler.current.OnPlayerMergeArea += RemoveBall;
    }

    private void SetNewBall(GameObject go)
    {
        follower.Add(go);
    }

    private void OnDisable()
    {
        GameEventHandler.current.OnPlayerMergeArea -= RemoveBall;
    }

    private void RemoveBall(bool obj)
    {
        clearList = obj;
    }

    private void Update()
    {
        if (clearList)
        {
            if (follower.Count > 1)
            {
                var ballIndex = follower.FindLast(x => x.name == "ballwithTexture(Clone)");
                follower.Remove(ballIndex);
            }
        }
    }

    public void SaveBall(GameObject obje)
    {
        follower.Add(obje);
    }

    public GameObject ReturnLast()
    {
        var last = follower.LastOrDefault();
        return last;
    }
}
