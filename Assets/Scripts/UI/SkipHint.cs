using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkipHint : MonoBehaviour {
    // Animation events don't let you bind functions to gameObject ¯\_(ツ)_/¯
    public void Deactivate() { gameObject.SetActive(false); }
}
