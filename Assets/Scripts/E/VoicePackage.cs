using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VoicePackage : MonoBehaviour
{
    public Animator voicePackageAnimator;

    bool isShowVoicePackage = false;

    float timeToWait = 0;
    public void ControlVoicePackage() 
    {
        if (timeToWait>0)
        {
            return;
        }
        if (!isShowVoicePackage)
        {
            voicePackageAnimator.Play("ShowVoicePack");
            isShowVoicePackage = true;
            timeToWait = 1.2f;
            return;
        }
        voicePackageAnimator.Play("CloseVoicePack");
        isShowVoicePackage = false;
        timeToWait = 1.2f;
    }
    private void Update()
    {
        timeToWait -= Time.deltaTime;
    }
}
