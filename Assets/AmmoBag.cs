using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
public class AmmoBag : MonoBehaviour
{
    public List<Transform> slots;
    public UnityEvent onAmmoBagged;


    public void OnTriggerEnter(Collider other)
    {
        var ammoBagFlag = other.GetComponent<AmmoBagFlag>();
        if(ammoBagFlag != null)
        {
            if(ammoBagFlag.isInBag)
            {
                return;
            }
            foreach (Transform slot in slots)
            {
                if (slot.childCount == 0)
                {
                    other.transform.SetParent(slot);
                    other.transform.localPosition = Vector3.zero;
                    other.transform.localRotation = Quaternion.identity;
                    other.attachedRigidbody.isKinematic = true;
                    break;
                }
            }
            ammoBagFlag.FlagBagged();
            onAmmoBagged?.Invoke();
        }

    }

    void OnTriggerExit(Collider other)
    {
        var ammoBagFlag = other.GetComponent<AmmoBagFlag>();
        if (ammoBagFlag != null)
        {
            ammoBagFlag.FlagGrabbed();
        }
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
