using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyUserInPool : MonoBehaviour
{
    public void DestroyMe() 
    {
        Destroy(this.gameObject);
    }
}
