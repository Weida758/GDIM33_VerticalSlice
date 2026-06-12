using UnityEngine;

[System.Serializable]
public struct WeaponLevel
{
    public float damage;
    public float cooldown;
    public float area;
    public float speed;
    public int count;
    public float duration;
    public int pierce;
    [TextArea] public string upgradeDescription;
}

[CreateAssetMenu(fileName = "WeaponData", menuName = "Scriptable Objects/Weapon Data")]
public class WeaponDataSO : ScriptableObject
{
    public string weaponName;
    [TextArea] public string description;
    public Sprite icon;
    public GameObject weaponPrefab;
    public WeaponLevel[] levels;

    public int MaxLevel
    {
        get
        {
            if (levels == null) return 0;
            return levels.Length;
        }
    }
}
