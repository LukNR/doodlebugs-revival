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

    public GameObject Explosion_GO;

    private Vector3 position_V3;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    void Start()
    {
        EngineOn = true;
        position_V3 = transform.position;
        MinLevel = transform.position.y;
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
    private Vector2 force;
    private bool resp;
    private float MaxLevel;
    private float MinLevel;
    private float SpeedRecord;

    // something with physics
    void FixedUpdate()
    {

        speed = rb.velocity.magnitude;
        Vector3 vtn = rb.transform.right.normalized;

        if( MaxLevel < this.transform.position.y ) MaxLevel = this.transform.position.y;
        if( MinLevel > this.transform.position.y && rb.velocity.y > 0 && transform.position.y > 0 ) MinLevel = this.transform.position.y;
        if( SpeedRecord < speed ) SpeedRecord = speed;

        /*     VERSION 0.1
        if( rb.velocity.x > 0 )
            force = new Vector2( Forsage_V2.x *(  vtn.x - vtn.y ) , Forsage_V2.y * ( 1f + ( vtn.y * vtn.x ) ) );
        else
            force = new Vector2( Forsage_V2.y * vtn.x , Forsage_V2.x * vtn.y );
        */

        if( rb.velocity.x > 0.2f )
            force = new Vector2( Forsage_V2.x * ( vtn.x - vtn.y ) , Forsage_V2.y * ( 1f + ( vtn.y * vtn.x ) ) * ( Mathf.Sign( vtn.y ) * rb.velocity.x / 8f ) );
        else
            force = new Vector2( Forsage_V2.y * vtn.x , Forsage_V2.x * vtn.y );

        if( EngineOn)
        {
            rb.drag = 0.01f * speed;
            rb.gravityScale = 0.2f * ( 1f + this.transform.position.y / 100f );
        }
        else
        {
            rb.drag = 0.01f;
            rb.gravityScale = 0.4f * ( 1f + this.transform.position.y / 100f );
        }

        if( speed > 10 )
        {
            rb.AddTorque( rb.velocity.magnitude *  0.1f );
        }

        if( speed < 5 && vtn.y > 0.7 ) EngineOn = false;

        // Rychlost je vetsi nez 10 a rotace neni uplne kolma k zemi
        if( speed > 10 && Mathf.Abs( vtn.y ) < 0.8f ) EngineOn = true;

        // Pada dolu, rychlost vetsi nez 10 a mam alespon nejaky naklon 
        if( rb.velocity.y < 0 && Mathf.Abs( vtn.x ) > 0.23f && speed > 10 )
        {
            force += new Vector2( speed * 0.3f, vtn.x * speed * 0.1f );
        }


        if( EngineOn )
            rb.AddForce( force );


        if( this.transform.position.y < 0 && !resp )
            StartCoroutine( Respawn() );

        if( transform.position.x > 500 ) transform.position = new Vector3( 0 , transform.position.y , transform.position.z );

    }

    public IEnumerator Respawn()
    {
        resp = true;

        GameObject ex = Instantiate( Explosion_GO , this.transform );
        ex.SetActive( true );
        ex.transform.parent = null;

        if( TryGetComponent( out SpriteRenderer sr ) )
            sr.enabled = false;

        yield return new WaitForSeconds( 1f );

        Destroy( ex );

        if( sr ) sr.enabled = true;
        this.transform.position = position_V3;
        this.transform.rotation = Quaternion.identity;
        rb.velocity = Vector2.zero;
        EngineOn = true;

       resp = false;
        yield return null;

    }

    public void OnGUI()
    {
        GUILayout.Label( "Engine " + EngineOn );
        GUILayout.Label( "Speed " + speed.ToString() );
        GUILayout.Label( "Height " + (int)this.transform.position.y );

        GUILayout.Label( "Recorman MaxLevel " + (int)( MaxLevel  ) );
        GUILayout.Label( "Recorman LowLevel " +  MinLevel  );
        GUILayout.Label( "Recorman of Speed " + (int)SpeedRecord );

        GUILayout.Label( "DIR " + rb.transform.right.ToString() );
        //GUILayout.Label( "Force " + force.ToString() );


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
