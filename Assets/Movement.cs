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

    void FixedUpdate()
    {
        Vector2 tempPos = position;

        float delta = Time.deltaTime * moveRate;

        if(Input.GetKey(KeyCode.A))
        {
            tempPos += new Vector2( -delta, 0.0f );
        }
        else if(Input.GetKey(KeyCode.D))
        {
            tempPos += new Vector2( delta, 0.0f );
        }

        if ( Input.GetKey( KeyCode.W ) )
        {
            tempPos += new Vector2( 0.0f, delta );
        }
        else if ( Input.GetKey( KeyCode.S ) )
        {
            tempPos += new Vector2( 0.0f, -delta );
        }

        tempPos = new Vector2( Mathf.Min( Mathf.Max( 0.0f, tempPos.x ), 1.0f ), Mathf.Min( Mathf.Max( 0.0f, tempPos.y ), 1.0f ) );

        if ( IsValidPosition( tempPos ) )
        {
            position = tempPos;
            mat.SetFloat( "_LightPosX", position.x );
            mat.SetFloat( "_LightPosY", position.y );
        }
    }

    private bool IsValidPosition(Vector3 position)
    {
        Texture2D tex = (Texture2D)mat.GetTexture( "_Casters" );
        
        if(tex.GetPixel(Mathf.FloorToInt(position.x * tex.width), Mathf.FloorToInt(position.y * tex.height)) != Color.white)
        {
            return false;
        }

        return true;
    }
}
