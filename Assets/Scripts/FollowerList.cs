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
        // GameEventHandler.current.OnPlayerMergeArea += RemoveBall;
        // if (PlayerPrefs.GetInt("BallSaveCount") > 1)
        // {
        //     for (int i = 0; i < PlayerPrefs.GetInt("BallSaveCount"); i++)
        //     {
        //         GameObject go = Instantiate(creatBall, follower[i].transform.position, Quaternion.identity,transform);
        //         var ball = go.GetComponent<Ball>();
        //         ball.SetValue(PlayerPrefs.GetInt("BallSave" + (i + 1)));
        //         SetNewBall(go);
        //         go.tag = "StackBall";
        //         //ball.ballRb.isKinematic = false;
        //         //ball.agent.enabled = true;
        //         ball.SetGoTarget(ReturnLast().transform);
        //     }
        // }
        // PlayerPrefs.DeleteKey("BallSaveCount");
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
        var last = follower.Last();
        return last;
    }

    // void OnApplicationFocus(bool hasFocus)
    // {
    //     if (!hasFocus)
    //     {
    //         if (follower.Count>1)
    //         {
    //             for (int i = 1; i < follower.Count; i++)
    //             {
    //                 PlayerPrefs.SetInt("BallSave" + i, (int)(follower[i].GetComponent<Ball>().GetValue()));
    //                 PlayerPrefs.SetInt("BallSaveCount", follower.Count);
    //                 PlayerPrefs.Save();
    //             }
    //         }
    //     }
    // }
}
