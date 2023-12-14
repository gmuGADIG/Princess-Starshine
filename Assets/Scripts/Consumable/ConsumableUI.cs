using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ConsumableUI : MonoBehaviour
{
    public GameObject UIParent;

    IEnumerator LateStart() {
        yield return 0; 

        UIParent.SetActive(false);
        var image = UIParent.GetComponentInChildren<Image>();
        Player.instance.PickedUpConsumable += (consumable) => {
            if (consumable == null) {
                UIParent.SetActive(false);
                return;
            }

            image.sprite = consumable.GetComponentInChildren<SpriteRenderer>().sprite;
            UIParent.SetActive(true);
        };
    }

    void Start() {
        StartCoroutine(LateStart());
    }
}
