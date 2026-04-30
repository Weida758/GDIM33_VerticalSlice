using UnityEngine;

[CreateAssetMenu(fileName = "EnemyStat", menuName = "Scriptable Objects/EnemyStat")]
public class EnemyStat : ScriptableObject
{
    public new string name;
    public float maxHealth;
    public float damage;
    public float speed;
    public float knockbackDuration = 0.15f;
    public float knockbackForce = 8f;
    
}
