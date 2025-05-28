using DG.Tweening;
using TMPro;
using UnityEngine;

public class Objective_Panel : MonoBehaviour
{
    public CanvasGroup panel; 

    public void SetText(string msg)
    {
        TextMeshProUGUI text = panel.GetComponentInChildren<TextMeshProUGUI>();
        panel.DOFade(1, 1);

        text.text = "";
        text.text = msg;

        panel.DOFade(0, 1).SetDelay(5f).OnComplete(() =>
        {
            text.text = "";
        });
    }
    
}
