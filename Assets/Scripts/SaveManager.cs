using System.Collections;
using System.Collections.Generic;
using UnityEditor.Build.Content;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SaveManager : MonoBehaviour
{
    [SerializeField] private bool _Test;
    [SerializeField] private GameObject level1Chests;
    [SerializeField] private List<GameObject> _AllObje = new List<GameObject>();
    
    private void Awake()
    {
        foreach (Transform t in level1Chests.transform)
        {
            _AllObje.Add(t.gameObject);
        }
        
        if (_Test)
        {
            for (int i = 0; i < _AllObje.Count; i++)
            {
                if (PlayerPrefs.GetInt("Level"+ SceneManager.GetActiveScene().buildIndex + "_" + i) == 1)
                {
                    _AllObje[i].SetActive(true);
                }
            }
        }
        else
        {
            if (PlayerPrefs.GetInt("GetData"+SceneManager.GetActiveScene().buildIndex ) == 1)
            {
                for (int i = 0; i < _AllObje.Count; i++)
                {
                    if (PlayerPrefs.GetInt("Level" + SceneManager.GetActiveScene().buildIndex + "_" + i)==1)
                    {
                        _AllObje[i].SetActive(true);
                    }
                    else
                    {
                        _AllObje[i].SetActive(false);
                    }
                }
            }
        }
    }
    void OnApplicationFocus(bool hasFocus)
    {
        if (!hasFocus)
        {
            for (int i = 0; i < _AllObje.Count; i++)
            { 
                if (_AllObje[i].activeSelf)
                {
                    PlayerPrefs.SetInt("Level" + SceneManager.GetActiveScene().buildIndex + "_" + i, 1);
                    PlayerPrefs.Save();
                }
                else
                {
                    PlayerPrefs.SetInt("Level" + SceneManager.GetActiveScene().buildIndex + "_" + i, 0);
                    PlayerPrefs.Save();
                }
              
            }
            if (PlayerPrefs.GetInt("GetData" + SceneManager.GetActiveScene().buildIndex) == 0)
            {
                PlayerPrefs.SetInt("GetData" + SceneManager.GetActiveScene().buildIndex, 1);
                PlayerPrefs.Save();
            }
        }
    }
}
