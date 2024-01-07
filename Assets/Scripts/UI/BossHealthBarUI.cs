using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossHealthBarUI : MonoBehaviour
{
    [SerializeField] Image healthBar;

    static BossHealthBarUI instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this);
        }
    }

    public static void SetHealth(float healthPercent)
    {
        if (instance == null)
        {
            return;
        }
        instance.healthBar.fillAmount = healthPercent;
    }
}
