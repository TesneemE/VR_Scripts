using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UIAudio : MonoBehaviour, IPointerClickHandler
{
    public string clickAudioName;

    public void OnPointerClick(PointerEventData eventData)
    {
        if(clickAudioName != "")
        {
            AudioManager.instance.Play(clickAudioName);
        }
    }

}
