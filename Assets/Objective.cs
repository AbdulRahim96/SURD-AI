using System.Collections;
using UnityEngine;

public class Objective : MonoBehaviour
{
    public static Objective Instance;
    public GameObject cameraCutscene;
    public int value = 0;
    void Awake()
    {
        Instance = this;
    }

    private void Update()
    {
        if (value >= 3)
        {
            if (Input.GetMouseButtonDown(0))
            {
                value = 0;
                StartCoroutine(EnableCutscene());
            }
        }
    }

    IEnumerator EnableCutscene()
    {
        Time.timeScale = 0.2f;
        cameraCutscene.SetActive(true);
        yield return new WaitForSecondsRealtime(4f);
        cameraCutscene.SetActive(false);
        Time.timeScale = 1f;
    }
}
