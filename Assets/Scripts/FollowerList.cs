using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class FollowerList : MonoBehaviour
{
    public List<Transform> follower;
    public Transform ReturnLast()
    {
        var last = follower.LastOrDefault();
        return last;
    }
}
