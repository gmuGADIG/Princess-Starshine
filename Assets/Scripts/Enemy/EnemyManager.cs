using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager
{
    public static List<GameObject> enemies = new List<GameObject>(GameObject.FindGameObjectsWithTag("Enemy"));//TODO: Enemies need to add and remove themselves from this list
}