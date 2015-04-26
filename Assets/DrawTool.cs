using UnityEngine;
using System.Collections;

public class DrawTool : MonoBehaviour
{
    Material mat;

    void Start()
    {
        mat = GetComponent<SpriteRenderer>().material;
    }

    void Update()
    {
        if ( Input.GetMouseButton( 0 ) )
        {
            Vector2 hitLoc = GetUVCoordinate();

            if ( hitLoc.x >= 0 && hitLoc.y >= 0 )
            {
                //Debug.Log( hitLoc );
                Texture2D tex = (Texture2D)mat.GetTexture( "_Casters" );

                tex.SetPixel( (int)( tex.width * hitLoc.x ), (int)( tex.height * hitLoc.y ), Color.black );
                tex.Apply();
                mat.SetTexture( "_Casters", tex );
                GetComponent<SpriteRenderer>().material = mat;
            }
        }
    }

    private Vector2 GetUVCoordinate()
    {
        Ray cast = Camera.main.ScreenPointToRay( Input.mousePosition );

        RaycastHit hit;
        if ( Physics.Raycast( cast, out hit, 20.0f ) )
        {
            //Debug.Log( mat.GetTexture( "_Casters" ).width );
            Vector2 normalizedHitPos = new Vector2( hit.point.x - hit.transform.position.x, hit.point.y - hit.transform.position.y ) + new Vector2( 0.5f, 0.5f );
            Debug.Log( normalizedHitPos );
            return new Vector2( normalizedHitPos.x, normalizedHitPos.y );
        }
        else
        {
            return new Vector2( -1.0f, -1.0f );
        }
    }
}
