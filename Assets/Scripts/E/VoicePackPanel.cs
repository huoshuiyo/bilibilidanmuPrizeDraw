using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;

using UnityEngine;
using UnityEngine.UI;

public class VoicePackPanel : MonoBehaviour
{
    private CanvasGroup voicePackPanelCanvasGroup;
    public Transform voicePackItemParent;
    public GameObject voicePackItemPrefabs;

    private List<AudioClip> audioClips;

    public string voicePath = "";

    private void Start()
    {
        voicePackPanelCanvasGroup = GetComponent<CanvasGroup>();
        //CloseThisPanel();
        CreateVoicePackItem();
    }

    public void CreateVoicePackItem()
    {
        audioClips = Resources.LoadAll<AudioClip>(voicePath).ToList<AudioClip>();
        foreach (AudioClip clip in audioClips) 
        {
            GameObject voiceGameObject = voicePackItemPrefabs;
            voiceGameObject.GetComponent<VoicePackItem>().Init(clip.name, clip);
            Instantiate(voiceGameObject, voicePackItemParent);
        }
    }

    public void OpenThisPanel()
    {
        voicePackPanelCanvasGroup.alpha = 1;
        voicePackPanelCanvasGroup.blocksRaycasts = true;
        voicePackPanelCanvasGroup.interactable = true;
    }

    public void CloseThisPanel()
    {
        voicePackPanelCanvasGroup.alpha = 0;
        voicePackPanelCanvasGroup.blocksRaycasts = false;
        voicePackPanelCanvasGroup.interactable = false;
    }
}
