using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bonus : MonoBehaviour
{
    Rigidbody rg;
    // Start is called before the first frame update
    void Start()
    {
        rg = this.GetComponent<Rigidbody>();
        rg.AddForce(Vector3.up * 200+Vector3.left*150);
        StartCoroutine(CoroutineFreezeObject(2f));
    }

    private IEnumerator CoroutineFreezeObject(float x)
    {
        yield return new WaitForSeconds(x);
        rg.isKinematic = true;
    }

    // Update is called once per frame
    void Update()
    {
    }
}
