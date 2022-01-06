using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DotCount : MonoBehaviour
{
    void Start()
    {
        foreach(Transform child in transform)
        {
            ScoreManager.Instance.SetDotList(child.gameObject);
        }
    }

}
