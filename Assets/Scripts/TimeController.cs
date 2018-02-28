using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;

public class TimeController : MonoBehaviour {

    [SerializeField]
    Slider slider;

    [SerializeField]
    [Range(0.0f, 1.0f)]
    float scale = 1.0f;

    private void Start()
    {
        slider.value = scale;
    }

    void Update () {
        scale = slider.value;

        Time.timeScale = scale;

        if (Input.GetKeyDown(KeyCode.R))        
            UnityEngine.SceneManagement.SceneManager.LoadScene(0);
	}
}
