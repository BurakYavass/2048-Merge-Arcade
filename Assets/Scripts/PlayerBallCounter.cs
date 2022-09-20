using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

public class PlayerBallCounter : MonoBehaviour
{
    [SerializeField] private BallController _ballController;

    public float stackValue;

    public void BallCountCheck()
    {
        for (int i = 1; i < _ballController._Balls.Count; i++)
        {
            stackValue += _ballController._Balls[i].GetComponent<Ball>().GetValue();
        }
    }

    public float GetValue()
    {
        return stackValue;
    }
}
