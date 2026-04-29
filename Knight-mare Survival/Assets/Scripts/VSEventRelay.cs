using UnityEngine;
using Unity.VisualScripting;

public class VSEventRelay : MonoBehaviour
{
    public void Fire(string eventName)
    {
        CustomEvent.Trigger(gameObject, eventName);
    }
}