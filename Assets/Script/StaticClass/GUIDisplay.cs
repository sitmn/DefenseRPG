using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class GUIDisplay : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public static void DisplayFadeIn(GameObject targetObj){
        if(targetObj.activeSelf == false){
            targetObj.SetActive(true);
            CanvasGroup canvasGroup = targetObj.GetComponent<CanvasGroup>();
            canvasGroup.alpha = 0;
            canvasGroup.DOFade(1, 0.5f);
        }
    }

    public static void DisplayFadeOut(GameObject targetObj){
        if(targetObj.activeSelf == true){
            CanvasGroup canvasGroup = targetObj.GetComponent<CanvasGroup>();
            canvasGroup.alpha = 1;
            canvasGroup.DOFade(0, 0.5f);
            targetObj.SetActive(false);
        }
    }
}
