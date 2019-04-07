using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obsticle : MonoBehaviour
{
    public GameObject particleSystemGO;
    ParticleSystem particleSystem;

    private void OnDestroy()
    {
        Destroy(Instantiate(particleSystemGO, this.transform.position, Quaternion.identity),2);
       
    

        
    }
    // Start is called before the first frame update
    void Start()
    {
        particleSystemGO.GetComponent<ParticleSystem>().Pause();
   
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
