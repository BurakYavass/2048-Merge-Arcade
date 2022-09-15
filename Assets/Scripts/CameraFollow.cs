using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField]
    GameObject _Camera;
    [SerializeField]
    GameObject _MergePos;
    [SerializeField]
    GameObject _PortalPos;
    [SerializeField]
    GameObject _OrginalPos;
   [SerializeField]
    Vector3 _Distance;
      GameObject _Player;
    bool _IsOriginal;
    bool _IsMerge;
    bool _IsPortal;
     void Start()
    {
        _Player = GameObject.FindGameObjectWithTag("Player");
        _Distance.z = transform.position.z - _Player.transform.position.z;
        _Distance.y = transform.position.y - _Player.transform.position.y;
        _Distance.x = transform.position.x - _Player.transform.position.x;
    }   
    void Update()
    {
        if (_Player)
        {
            transform.position =new Vector3(_Player.transform.position.x + _Distance.x, transform.position.y, _Player.transform.position.z + _Distance.z) ;
           
        }

        if (_IsMerge)
        {
            _Camera.transform.position = Vector3.Lerp(_Camera.transform.position, _MergePos.transform.position, .05f);
            _Camera.transform.rotation = Quaternion.Lerp(_Camera.transform.rotation, _MergePos.transform.rotation, .05f);

        }
        else if (_IsPortal)
        {
            _Camera.transform.position = Vector3.Lerp(_Camera.transform.position, _PortalPos.transform.position, .05f);
            _Camera.transform.rotation = Quaternion.Lerp(_Camera.transform.rotation, _PortalPos.transform.rotation, .05f);

        }
        else
        {
            _Camera.transform.position = Vector3.Lerp(_Camera.transform.position, _OrginalPos.transform.position, .05f);
            _Camera.transform.rotation = Quaternion.Lerp(_Camera.transform.rotation, _OrginalPos.transform.rotation, .05f);

        }

    }
    public void SetMerge()
    {
        _IsMerge = true;
        _IsOriginal = false;
        _IsPortal = false;
    }  
    public void SetOriginal()
    {
        _IsMerge = false;
        _IsOriginal = true;
        _IsPortal = false;
    } 
    public void SetPortal()
    {
        _IsMerge = false;
        _IsOriginal = false;
        _IsPortal = true;
    }
}
