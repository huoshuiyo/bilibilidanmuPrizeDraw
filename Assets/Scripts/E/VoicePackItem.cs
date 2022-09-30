using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VoicePackItem : MonoBehaviour
{
    public Text voiceText;
    public AudioSource voiceSource;

    public void Init(string voiceName , AudioClip audioClip)
    {
        voiceText.text = voiceName;
        voiceSource.clip = audioClip;
    }
}
