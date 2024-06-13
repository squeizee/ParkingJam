using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using DG.Tweening;

public class starFxController : MonoBehaviour
{
    public List<GameObject> starFX;
    
    void Start()
    {
        StarAnimation();
    }
    public void StarAnimation()
    {
        foreach (var o in starFX)
        {
            o.SetActive(true);
        }
    }
    
}