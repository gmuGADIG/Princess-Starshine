using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkipTutorial : MonoBehaviour
{
    //current script is WIP
    public static bool InTutorial = true;
    public void Skip() {
        InTutorial = false;
    }

}
