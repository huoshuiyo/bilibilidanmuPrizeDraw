using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityChan;

public class PrizeDrawAnim : MonoBehaviour
{
    public GameObject model;
    public Animator animator;
    public string poseName;
    public void ShowModel() 
    {
        model.SetActive(true);
        //model.transform.localEulerAngles = new Vector3(model.transform.rotation.x, model.transform.rotation.y-180f, model.transform.rotation.z);
        animator.Play(poseName);

        Invoke("OpenWind", 1);
    }
    public void OpenWind() 
    {
        model.GetComponent<RandomWind>().isWindActive = true;
    }
}
