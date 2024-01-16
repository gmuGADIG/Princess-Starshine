using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ConsumableUI : MonoBehaviour
{
    public GameObject UIParent;

    void Start() {
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

        Player.instance.Thaw();
    }
}
