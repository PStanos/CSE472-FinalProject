using UnityEngine;
using System.Collections;

public class Movement : MonoBehaviour
{
    private Vector2 position = new Vector2( 0.5f, 0.5f );
    public float moveRate = 0.01f;
    Material mat;

    public bool isGravityOn = false;
    private bool onGround = false;
    private bool jumping = false;
    private float timeJumping = 0.0f;

    void Start()
    {
        mat = GetComponent<SpriteRenderer>().material;
    }

    void FixedUpdate()
    {
        Vector2 tempPos = position;

        float delta = Time.deltaTime * moveRate;

        if ( Input.GetKey( KeyCode.A ) )
        {
            tempPos += new Vector2( -delta, 0.0f );
        }
        else if ( Input.GetKey( KeyCode.D ) )
        {
            tempPos += new Vector2( delta, 0.0f );
        }

        if ( !isGravityOn )
        {
            if ( Input.GetKey( KeyCode.W ) )
            {
                tempPos += new Vector2( 0.0f, delta );
            }
            else if ( Input.GetKey( KeyCode.S ) )
            {
                tempPos += new Vector2( 0.0f, -delta );
            }
        }
        else
        {
            if ( ( Input.GetKey( KeyCode.W ) && onGround ) )
            {
                jumping = true;
                onGround = false;
                tempPos += new Vector2( 0.0f, delta );
            }
            else if ( jumping )
            {
                tempPos += new Vector2( 0.0f, delta );
            }
            else
            {
                tempPos += new Vector2( 0.0f, -delta );
            }
        }

        tempPos = new Vector2( Mathf.Min( Mathf.Max( 0.0f, tempPos.x ), 1.0f ), Mathf.Min( Mathf.Max( 0.0f, tempPos.y ), 1.0f ) );

        if ( IsValidXPosition( tempPos ) )
        {
            position.x = tempPos.x;
            mat.SetFloat( "_LightPosX", position.x );
        }

        if ( IsValidYPosition( tempPos ) )
        {
            position.y = tempPos.y;
            mat.SetFloat( "_LightPosY", position.y );

            if(tempPos.y == 0.0f)
            {
                jumping = false;
                timeJumping = 0.0f;
                onGround = true;
            }
        }
        else if ( tempPos.y < position.y )
        {
            jumping = false;
            timeJumping = 0.0f;
            onGround = true;
        }
    }

    void Update()
    {
        if ( jumping )
        {
            timeJumping += Time.deltaTime;

            if ( timeJumping >= 0.4f )
            {
                jumping = false;
                timeJumping = 0.0f;
            }
        }
    }

    private bool IsValidXPosition( Vector3 tempPos )
    {
        Texture2D tex = (Texture2D)mat.GetTexture( "_Casters" );

        if ( tex.GetPixel( Mathf.FloorToInt( tempPos.x * tex.width ), Mathf.FloorToInt( position.y * tex.height ) ) != Color.white )
        {
            return false;
        }

        return true;
    }

    private bool IsValidYPosition( Vector3 tempPos )
    {
        Texture2D tex = (Texture2D)mat.GetTexture( "_Casters" );

        if ( tex.GetPixel( Mathf.FloorToInt( position.x * tex.width ), Mathf.FloorToInt( tempPos.y * tex.height ) ) != Color.white )
        {
            return false;
        }

        return true;
    }
}
