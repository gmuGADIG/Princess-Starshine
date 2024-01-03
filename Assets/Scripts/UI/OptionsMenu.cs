using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class OptionsMenu : MonoBehaviour
{
    public UnityEvent OnClose;

    public void Close() {
        gameObject.SetActive(false);
        OnClose.Invoke();
    }

    void Update() {
        if (Input.GetKeyDown(KeyCode.Escape)) {
            Close();
        }
    }
}
