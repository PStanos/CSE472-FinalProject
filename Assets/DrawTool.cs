using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DrawTool : MonoBehaviour
{
    Material mat;
    Color[] colors;
	Color[] colors2;
    public int penSize = 10;
    List<Vector2> points;
    

    void Start()
    {
        points = new List<Vector2>();
        mat = GetComponent<SpriteRenderer>().material;
        colors = new Color[penSize*penSize];
		colors2 = new Color[penSize*penSize];

        for (int i = 0; i < penSize*penSize; i++) { colors[i] = Color.black; }
		for (int i = 0; i < penSize*penSize; i++) { colors2[i] = Color.white; }

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
                tex.SetPixels( (int)( tex.width * hitLoc.x ), (int)( tex.height * hitLoc.y ), penSize,penSize, colors,0 );
                tex.Apply();
                mat.SetTexture( "_Casters", tex );
                GetComponent<SpriteRenderer>().material = mat;
            }
        }

		//Erase
		if ( Input.GetMouseButton( 1 ) )
		{
			Vector2 hitLoc = GetUVCoordinate();
			
			if ( hitLoc.x >= 0 && hitLoc.y >= 0 )
			{
				//Debug.Log( hitLoc );
				Texture2D tex = (Texture2D)mat.GetTexture( "_Casters" );
				tex.SetPixels( (int)( tex.width * hitLoc.x ), (int)( tex.height * hitLoc.y ), penSize,penSize, colors2,0 );
				tex.Apply();
				mat.SetTexture( "_Casters", tex );
				GetComponent<SpriteRenderer>().material = mat;
			}
		}

        //method to draw straight line

        if (Input.GetKey(KeyCode.LeftControl) && Input.GetMouseButtonDown(0))
        { 
            if (points.Count < 2)
            {
                Vector2 hitLoc = GetUVCoordinate();
                if (hitLoc.x >= 0 && hitLoc.y >= 0)
                {
                    points.Add(hitLoc);
                }
            }
        }

        if (points.Count == 2)
        {
            Texture2D tex = (Texture2D)mat.GetTexture("_Casters");
            int x0 = (int)(tex.width * points[0].x);
            int y0 = (int)(tex.width * points[0].y);
            int x1 = (int)(tex.width * points[1].x);
            int y1 = (int)(tex.width * points[1].y);

            DrawLine(tex, x0, y0, x1, y1);

            points.Clear();
        }

        //finish line draw

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


    void DrawLine(Texture2D tex, int x0, int y0, int x1, int y1)
    {
        int dy = (int)(y1 - y0);
        int dx = (int)(x1 - x0);
        int stepx, stepy;

        if (dy < 0) { dy = -dy; stepy = -1; }
        else { stepy = 1; }
        if (dx < 0) { dx = -dx; stepx = -1; }
        else { stepx = 1; }
        dy <<= 1;
        dx <<= 1;

        float fraction = 0;

        tex.SetPixels(x0, y0, penSize, penSize, colors) ;
        if (dx > dy)
        {
            fraction = dy - (dx >> 1);
            while (Mathf.Abs(x0 - x1) > 1)
            {
                if (fraction >= 0)
                {
                    y0 += stepy;
                    fraction -= dx;
                }
                x0 += stepx;
                fraction += dy;
                tex.SetPixels(x0, y0, penSize, penSize, colors);
            }
        }
        else
        {
            fraction = dx - (dy >> 1);
            while (Mathf.Abs(y0 - y1) > 1)
            {
                if (fraction >= 0)
                {
                    x0 += stepx;
                    fraction -= dy;
                }
                y0 += stepy;
                fraction += dx;
                tex.SetPixels(x0, y0, penSize, penSize, colors);
            }
        }
    }


}
