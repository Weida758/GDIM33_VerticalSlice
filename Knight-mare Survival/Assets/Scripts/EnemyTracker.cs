using UnityEngine;

public class EnemyTracker : MonoBehaviour
{
    public static int ActiveCount;

    void OnEnable() { ActiveCount++; }
    void OnDisable() { ActiveCount--; }
}