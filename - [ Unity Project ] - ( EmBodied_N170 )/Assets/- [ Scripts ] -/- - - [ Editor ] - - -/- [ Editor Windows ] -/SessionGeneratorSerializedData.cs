using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class SessionGeneratorSerializedData : ScriptableObject
{
    [HideInInspector] public string fileDirectory;
    [HideInInspector] public string filename;


    [SerializeField]
    [HideInInspector]
    public GameObject heldItem;
    
    [SerializeField]
    [HideInInspector]
    public PrefabList 
        groupA = new PrefabList(),
        groupAStatues = new PrefabList(),
        groupB = new PrefabList(),
        groupBStatues = new PrefabList();

    [HideInInspector] public int blocks;
    [HideInInspector] public int trials;

    [HideInInspector] public float approachDistance;
    [HideInInspector] public Enums.Handedness participantHandedness;
    [HideInInspector] public Enums.Room initialTargetRoom;

    [HideInInspector] public string questionOne;
    [HideInInspector] public Texture2D questionOneLowTexture, questionOneHighTexture;
    
    [HideInInspector] public string questionTwo;
    [HideInInspector] public Texture2D questionTwoLowTexture, questionTwoHighTexture;
}

[Serializable]
public class PrefabList
{
    public bool listVisibility;
    
    public int listCount;
    
    [SerializeField]
    public List<GameObject> prefabList = new List<GameObject>();
}