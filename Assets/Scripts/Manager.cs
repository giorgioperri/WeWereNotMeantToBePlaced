using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPEffects.Components;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Manager : MonoBehaviour
{
    private static Manager _instance;
    public static Manager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindFirstObjectByType<Manager>();
            }
            
            return _instance;
        }
    }

    public Image bg;

    public GameObject leftButton;
    public TextMeshProUGUI leftButtonText;
    
    public GameObject rightButton;
    public TextMeshProUGUI rightButtonText;
    
    public GameObject thirdButton;
    public TextMeshProUGUI thirdButtonText;
    
    public Transform centralTextParent;
    public CanvasGroup textCanvasGroup;
    
    private SceneContentSO nextScene;
    [SerializeField] private SceneContentSO beginningScene;
    [SerializeField] private SceneContentSO refusalScene;

    [SerializeField] private Animator _animator;
    
    [SerializeField] private TMP_FontAsset _bebas;
    [SerializeField] private TMP_FontAsset _ephidona;

    private Button lb;
    private Button rb;
    private Button tb;
    
    private void Awake()
    {
        _instance = this;
        lb = leftButton.GetComponent<Button>();
        rb = rightButton.GetComponent<Button>();
        tb = thirdButton.GetComponent<Button>();
        
        lb.onClick.AddListener(() => LoadScenario(beginningScene));
        rb.onClick.AddListener(() => LoadScenario(refusalScene));
    }

    public void LoadScenario(SceneContentSO sceneContent)
    {
        StartCoroutine(AnimateScenario(sceneContent));
    }

    private IEnumerator AnimateScenario(SceneContentSO sceneContent)
    {
        _animator.Play("Disappear");
        
        //tween out the text canvas group using DOTween
        textCanvasGroup.DOFade(0, 0.5f).SetDelay(.5f);
        
        yield return new WaitForSeconds(2.75f);
        
        bg.sprite = sceneContent.backgroundImage;
        
        leftButton.SetActive(string.IsNullOrEmpty(sceneContent.leftButtonText) == false);
        leftButtonText.text = sceneContent.leftButtonText;
        
        rightButton.SetActive(string.IsNullOrEmpty(sceneContent.rightButtonText) == false);
        rightButtonText.text = sceneContent.rightButtonText;
        
        thirdButton.SetActive(string.IsNullOrEmpty(sceneContent.thirdButtonText) == false);
        thirdButtonText.text = sceneContent.thirdButtonText;
        
        nextScene = sceneContent.nextScene;
        lb.onClick.RemoveAllListeners();
        rb.onClick.RemoveAllListeners();
        tb.onClick.RemoveAllListeners();
        
        lb.onClick.AddListener(() => LoadScenario(nextScene));
        rb.onClick.AddListener(() => LoadScenario(nextScene));
        tb.onClick.AddListener(() => LoadScenario(nextScene));
        
        //Clear the centralTextParent
        foreach (Transform child in centralTextParent)
        {
            Destroy(child.gameObject);
        }
        
        yield return new WaitForSeconds(0.75f);
        
        _animator.Play("Reappear");
        textCanvasGroup.DOFade(1, 0.5f).SetDelay(.5f);
        yield return new WaitForSeconds(1f);
        
        foreach (var line in sceneContent.centralTextOrQuestion)
        {
            //create a new text object in the centralTextParent
            var textObject = new GameObject(line.Key);
            textObject.transform.SetParent(centralTextParent, false);
            
            var textMesh = textObject.AddComponent<TextMeshProUGUI>();
            var TMPWriter = textObject.AddComponent<TMPWriter>();
            TMPWriter.DefaultDelays.SetDelay(0.05f);

            textMesh.text = line.Key;
            textMesh.font = line.Value == TextFont.Bebas ? _bebas : _ephidona;
            textMesh.autoSizeTextContainer = false;
            textMesh.enableAutoSizing = false;
            textMesh.fontSize = line.Value == TextFont.Bebas ? 50 : 100;
            textMesh.alignment = TextAlignmentOptions.Center;
            textMesh.color = Color.white;
            textMesh.textWrappingMode = TextWrappingModes.Normal;
            
            yield return new WaitForSeconds(0.05f * line.Key.Length + 0.5f);
        }
        
        if (!leftButton.activeSelf || !rightButton.activeSelf)
        {
            //Wait for the sceneContent delay and load the next scene
            yield return new WaitForSeconds(sceneContent.delayBeforeNextScene);
            
            if (sceneContent.nextScene == null)
            {
                Debug.LogWarning("Quitting.");
                Application.Quit();
                yield break;
            }
                
            LoadScenario(nextScene);
        }
    }
}
