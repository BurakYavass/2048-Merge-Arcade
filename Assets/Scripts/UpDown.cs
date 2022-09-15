using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpDown : MonoBehaviour
{
    [SerializeField]
    float _Speed;



    GameObject _Player;

    float _StartY;
    bool _Down;
    Vector3 _StartPos;
    private void Start()
    {
        _Player = GameObject.FindGameObjectWithTag("Player");
        _StartY = transform.position.y;
        
    }
    void Update()
    {

        if (!_Player.GetComponent<PlayerControl>().GetStop())
        {
            if (_Down)
            {
                transform.position = Vector3.Lerp(transform.position, transform.position - Vector3.up, _Speed * Time.deltaTime);
            }
            else
            {
                transform.position = Vector3.Lerp(transform.position, transform.position + Vector3.up, _Speed * Time.deltaTime);

            }
        }
        else
        {
            transform.position = Vector3.Lerp(transform.position, new Vector3(transform.position.x, _StartY, transform.position.z)  , _Speed * Time.deltaTime);
        }
       
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("BallDown"))
        {
            _Down = false;
        }

        if (other.CompareTag("BallUp"))
        {
            _Down = true;
        }
    }

}
