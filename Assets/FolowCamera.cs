using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FolowCamera : MonoBehaviour
{

    public Transform Target;
    private Vector3 Offset;
    // Start is called before the first frame update
    void Start()
    {
        Offset = this.transform.position - Target.position;
    }

    // Update is called once per frame
    void Update()
    {
        this.transform.position = Target.position + Offset;
    }
}
