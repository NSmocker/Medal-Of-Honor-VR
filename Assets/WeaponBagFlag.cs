using UnityEngine;

public class WeaponBagFlag : MonoBehaviour
{
    public bool isInBag = false;
    public float notGrabbedTime = 0.5f; 
    // Start is called once before the first execution of Update after the MonoBehaviour is created
   
    public void FlagGrabbed( )
    {
        
        Invoke(nameof(SetNotInBag), notGrabbedTime);
    }
    public void FlagBagged( )
    {
        
        isInBag = true;
    }

    void SetNotInBag()
    {
        isInBag = false;
    }
    
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
