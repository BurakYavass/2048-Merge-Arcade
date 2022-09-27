using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerBallCounter : MonoBehaviour
{
    [SerializeField] private BallController _ballController;

    public int stackValue;

    public void BallCountCheck()
    {
        for (int i = 0; i < _ballController.balls.Count; i++)
        {
            stackValue += _ballController.balls[i].GetComponent<Ball>().GetValue();
        }
    }
    public float GetValue()
    {
        return stackValue;
    }
}
