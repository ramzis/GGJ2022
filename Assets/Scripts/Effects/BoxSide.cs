using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxSide : MonoBehaviour
{
    [SerializeField]
    private  Animator  anim;
    // Start is called before the first frame update
    void Start()
    {
        anim= gameObject.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void stopAnim( )
    {
        anim.enabled = false;
    }
}
