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
        foreach (var ball in _ballController.balls)
        {
            stackValue += ball.GetComponent<Ball>().GetValue();
        }
    }
    public float GetValue()
    {
        return stackValue;
    }
}
