using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InputFieldScript : MonoBehaviour
{
    private InputField inputField;
    public DOTweenAnimation animation;
    public KeyCode openKeyboardKey;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        inputField = GetComponent<InputField>();
    }

    // Update is called once per frame
    void Update()
    {
        if (EventSystem.current.currentSelectedGameObject == inputField.gameObject)
        {
           // PlayerMovement.enableControls = false;
        }
        if(Input.GetKeyDown(openKeyboardKey))
        {
            inputField.Select();
            animation.DORestart();
            PlayerMovement.enableControls = false;
        }
        if (Input.GetKeyDown(KeyCode.Escape))
            OnSubmit();
    }

    public void OnSubmit()
    {
        animation.DOPlayBackwards();
        PlayerMovement.enableControls = true;
    }
}
