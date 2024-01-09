using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossHealthBarUI : MonoBehaviour
{
    [SerializeField] Image healthBar;
    [SerializeField] Image secondHealthBar;
    [SerializeField] float secondHealthBarDelay = 1f;
    [SerializeField] float secondHealthBarCorrectionSpeed = 3f;

    static BossHealthBarUI instance;

    float secondHealthBarTarget = 1f;

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

    private void Update()
    {
        instance.secondHealthBar.fillAmount = Mathf.Lerp(instance.secondHealthBar.fillAmount, secondHealthBarTarget, secondHealthBarCorrectionSpeed * Time.deltaTime);
    }

    public static void SetHealth(float healthPercent)
    {
        if (instance == null || !instance.isActiveAndEnabled)
        {
            return;
        }
        instance.healthBar.fillAmount = healthPercent;
        instance.StartCoroutine(SetSecondHealthBar(healthPercent));
    }

    public static IEnumerator SetSecondHealthBar(float healthPercent)
    {
        yield return new WaitForSeconds(instance.secondHealthBarDelay);
        instance.secondHealthBarTarget = healthPercent;
    }
}
