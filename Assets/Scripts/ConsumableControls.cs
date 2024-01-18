using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Rendering;

public class ConsumableControls : MonoBehaviour {
    private bool hasPickup = false;
    private string pickupType = "None";

    // Update is called once per frame
    void Update() {
        if (Input.GetKeyDown("x") || Input.GetKeyDown("space")) {
            if (hasPickup == true) {
                Debug.Log(pickupType + " Consumable Used");
                hasPickup = false;
            }
            else {
                Debug.Log("No consumable is held");
            }
        }
    }
    private void OnCollisionEnter2D(UnityEngine.Collision2D other) {
        if ((other.gameObject.tag == "Red" || other.gameObject.tag == "Blue") && hasPickup == false) {
            pickupType = other.gameObject.tag;
            Debug.Log(pickupType + " consumable has been picked up!");
            hasPickup = true;
            Destroy(other.gameObject);
        }
    }
}
