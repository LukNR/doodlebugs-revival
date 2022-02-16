using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Dvouplosnicek : MonoBehaviour
{
    public Rigidbody2D rb;

    public Vector2 Forsage_V2;
    public float TorqueForce_F;

    public bool isReversed = false;

    public AnimationCurve DragCurve;

    public bool EngineOn;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    void Start()
    {
        EngineOn = true;
    }

    void Update() {

        if (Input.GetKey(KeyCode.LeftArrow)) {
            rb.AddTorque( EngineOn ? TorqueForce_F : TorqueForce_F * 5f );
        }
        if (Input.GetKey(KeyCode.RightArrow)) {
            rb.AddTorque( EngineOn ? - TorqueForce_F : -TorqueForce_F * 5f );
        }

    }

    private float speed;

    // something with physics
    void FixedUpdate()
    {

        speed = rb.velocity.magnitude;
        Vector3 vtn = rb.transform.right.normalized;
        Vector2 force = new Vector2( Forsage_V2.x *(  vtn.x - vtn.y ) , Forsage_V2.y * ( 1f + ( vtn.y * vtn.x ) ) );

        rb.drag = 0.01f * speed;
        //rb.angularDrag = 

        if( speed > 10 )
        {
            rb.AddTorque( rb.velocity.magnitude *  0.1f );
        }

        if( speed < 3 && vtn.y > 0.7 ) EngineOn = false;

        if( speed > 5 && vtn.y < 0.3 ) EngineOn = true;


        if( EngineOn ) rb.AddForce( force );

    }

    public void OnGUI()
    {
        GUILayout.Label( "Engine " + EngineOn );
        GUILayout.Label( "Speed " + speed.ToString() );
        GUILayout.Label( "Height " + (int)this.transform.position.y );
        GUILayout.Label( "DIR " + rb.transform.right.ToString() );
    }

    public void Flip() {
        rb.transform.localRotation *= Quaternion.Euler(0, 180, 0);
        isReversed = !isReversed;

        // back from edge with normalized values
        if (isReversed) {
            transform.localRotation = Quaternion.Euler(new Vector3(0, 180, 0));
        } else {
            transform.localRotation = Quaternion.Euler(new Vector3(0, 0, 0));
        }
        //speed = 5;
    }

}
