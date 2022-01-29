using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxHolder : MonoBehaviour
{
    [SerializeField]
    private List<Animator> anim;
    // Start is called before the first frame update
     public void AllBoxControl( float time)
    {
        for(int i = 0; i < anim.Count; i++)
        {
            anim[i].SetFloat("Speed", 1f / time);
          
        }
    }
    void Update()
    {
       
    }
    public void naikinti()
    {
        DestroyImmediate(gameObject);
    } 
}
