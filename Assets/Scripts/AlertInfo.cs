using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AlertInfo : MonoBehaviour
{
    public Text infoText;

    public void SetAlertInfo(string textInfo)
    {
        infoText.text = textInfo;
        gameObject.SetActive(true);
        Invoke("HideAlert", 4f);
    }

    void HideAlert()
    {
        gameObject.SetActive(false);
    }

}
