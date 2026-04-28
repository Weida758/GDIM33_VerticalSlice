using UnityEngine;

public class VFXAuraEmitter : MonoBehaviour
{
    public GameObject vfxPrefab;
    public Transform followTarget;
    public Vector2 offset;
    public float interval = 1f;
    public bool parentToTarget = true;

    private float timer;

    void Start()
    {
        if (followTarget == null) followTarget = transform;
    }

    void Update()
    {
        timer -= Time.deltaTime;
        if (timer <= 0f)
        {
            Spawn();
            timer = interval;
        }
    }

    void Spawn()
    {
        Vector2 pos = (Vector2)followTarget.position + offset;
        Transform parent = parentToTarget ? followTarget : null;
        VFXPool.Instance.Get(vfxPrefab, pos, Quaternion.identity, parent);
    }
}
