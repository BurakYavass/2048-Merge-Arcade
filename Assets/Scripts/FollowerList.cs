using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class FollowerList : MonoBehaviour
{
    public List<GameObject> follower;
    [SerializeField] private GameObject creatBall;
    public bool clearList = false;

    private void Start()
    {
        //GameEventHandler.current.OnPlayerMergeArea += RemoveBall;
        //GameEventHandler.current.OnPlayerUpgradeArea += RemoveBall;
        //GameEventHandler.current.OnBallWallArea += RemoveBall;
    }

    private void SetNewBall(GameObject go)
    {
        follower.Add(go);
    }

    private void OnDisable()
    {
        GameEventHandler.current.OnPlayerMergeArea -= RemoveBall;
        GameEventHandler.current.OnPlayerUpgradeArea -= RemoveBall;
        GameEventHandler.current.OnBallWallArea -= RemoveBall;
    }

    private void RemoveBall(bool work)
    {
        clearList = work;
    }

    private void Update()
    {
        if (clearList)
        {
            if (follower.Count > 1)
            {
                var ballIndex = follower.Find(x => x.name == "ballwithTexture(Clone)");
                follower.Remove(ballIndex);
            }
            else
            {
                clearList = false;
            }
        }

        follower.RemoveAll((obje => obje == null));
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
