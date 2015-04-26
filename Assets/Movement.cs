using UnityEngine;
using System.Collections;

public class Movement : MonoBehaviour
{
    private Vector2 position = new Vector2( 0.5f, 0.5f );
    public float moveRate = 0.01f;
    Material mat;

    void Start()
    {
        mat = GetComponent<SpriteRenderer>().material;
    }

    void Update()
    {
        float delta = Time.deltaTime * moveRate;

        if(Input.GetKey(KeyCode.A))
        {
            position += new Vector2( -delta, 0.0f );
        }
        else if(Input.GetKey(KeyCode.D))
        {
            position += new Vector2( delta, 0.0f );
        }

        if ( Input.GetKey( KeyCode.W ) )
        {
            position += new Vector2( 0.0f, delta );
        }
        else if ( Input.GetKey( KeyCode.S ) )
        {
            position += new Vector2( 0.0f, -delta );
        }

        position = new Vector2( Mathf.Min( Mathf.Max( 0.0f, position.x ), 1.0f ), Mathf.Min( Mathf.Max( 0.0f, position.y ), 1.0f ) );
        mat.SetFloat( "_LightPosX", position.x );
        mat.SetFloat( "_LightPosY", position.y );
    }
}
