using System;
using System.Collections.Generic;
using Project.Tools.DictionaryHelp;
using TMPro;
using TMPro.EditorUtilities;
using UnityEngine;

public enum TextFont 
{
    Bebas = 0,
    Ephidona = 1
}

[Serializable] 
[ CreateAssetMenu(fileName = "SceneContent", menuName = "ScriptableObjects/SceneContent", order = 1 )]
public class SceneContentSO : ScriptableObject
{
    public string leftButtonText;
    public string rightButtonText;
    public string thirdButtonText;

    public SerializableDictionary<string, TextFont> centralTextOrQuestion;
    
    public Sprite backgroundImage;
    
    public float delayBeforeNextScene = 2.75f;
    public SceneContentSO nextScene;
    
    public bool doesAnswerMatter = false;
    public bool isFinalScene = false;
}

