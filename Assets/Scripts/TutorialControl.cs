using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialControl : SingletonBehaviour<TutorialControl>
{
    [SerializeField] private LineRenderer tutorialRenderer;

    [SerializeField] private Transform player;

    [SerializeField] private List<Transform> targetList = new List<Transform>();

    [SerializeField] private GameObject tutorialArrow;
    
    [SerializeField] private float slideSpeed;

    private Transform _target;
    
    private TutorialStage _stage;
    public TutorialStage Stage
    {
        get => _stage;
        private set
        {
            _stage = value;
            PlayerPrefs.SetInt("Tutorial" ,(int)_stage);
            if ((int)_stage < targetList.Count)
            {
                _target = targetList[(int) _stage];
                tutorialRenderer.gameObject.SetActive(true);
            }
            else
            {
                tutorialRenderer.gameObject.SetActive(false);
            }
        }
    }
    
    void Start()
    {
        Stage = (TutorialStage)PlayerPrefs.GetInt("Tutorial", 0);
    }

    // Update is called once per frame
    void Update()
    {
        if (!_target)
            return;
        
        var playerPos = player.position;
        var targetPos = _target.position;
        playerPos.y = 0.6f;
        targetPos.y = 0.6f;

        tutorialArrow.SetActive(true);
        tutorialArrow.transform.position = targetPos;
        
        tutorialRenderer.SetPosition(0,playerPos);
        tutorialRenderer.SetPosition(1, targetPos);
        
        var textureOffset = tutorialRenderer.material.GetTextureOffset("_MainTex");           
        textureOffset.x -= slideSpeed * Time.deltaTime;            
        tutorialRenderer.material.SetTextureOffset("_MainTex", textureOffset);

        if (_stage == TutorialStage.Completed)
        {
            tutorialArrow.SetActive(false);
        }

    }

    public void CompleteStage(TutorialStage tutorialStage)
    {
        if (Stage == tutorialStage)
        {
            Stage++;
        }
    }
    
}

public enum TutorialStage
{
    Box,
    Box2,
    Enemy,
    Merge,
    UpgradeArea,
    Upgrade,
    RestoreHeal,
    Teleport,
    Completed,
}