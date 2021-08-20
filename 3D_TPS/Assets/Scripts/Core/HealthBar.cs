using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [Header("誰的血量")]
    [SerializeField] private Health health;
    [Header("他的Health")]
    [SerializeField] private GameObject rootCanvas;
    [Header("前景")]
    [SerializeField] private Image foreground;
    [Range(0, 1)]
    [SerializeField] private float changeHealthRatio = 0.05f;

    void Update()
    {
        //如果血量的百分比約等於1 or 0
        if(Mathf.Approximately(health.GetHealthRatio(),0) || Mathf.Approximately(health.GetHealthRatio(),1))
        {
            rootCanvas.SetActive(false);
            return;
        }

        rootCanvas.SetActive(true);
        rootCanvas.transform.LookAt(Camera.main.transform.position);
        foreground.fillAmount = Mathf.Lerp(foreground.fillAmount, health.GetHealthRatio(), changeHealthRatio);
    }
}
