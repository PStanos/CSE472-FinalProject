Shader "Custom/ShadowCast" {
	Properties{
		_LightPosX ("Light Position X", Range (0,1)) = 0.5
		_LightPosY ("Light Position Y", Range (0,1)) = 0.5
		_LightSize ("Light Size", Range (0,1)) = .5
		_LightDistance ("Light Distance", Range(0,.1)) = 0.01
		_LightColor ("Light Color", Color ) = (1,1,1,1)
		_Casters ("Shadow Casters", 2D ) = "white" {}
	}
    SubShader {
        Pass {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma target 3.0

            #include "UnityCG.cginc"


            struct vertexInput {
                float4 vertex : POSITION;
                float4 texcoord0 : TEXCOORD0;
            };
            
            float _LightPosX, _LightPosY, _LightDistance, _LightSize;
            fixed4 _LightColor;
            sampler2D _Casters;
           

            struct fragmentInput{
                float4 position : SV_POSITION;
                float4 texcoord0 : TEXCOORD0;
            };

            fragmentInput vert(vertexInput i){
                fragmentInput o;
                o.position = mul (UNITY_MATRIX_MVP, i.vertex);
                o.texcoord0 = i.texcoord0;
                return o;
            }
            
            fixed4 frag(fragmentInput i) : SV_Target 
            {
            	fixed4 col = tex2D(_Casters, float2(i.texcoord0.x,i.texcoord0.y)); // Get the current pixel color
            	
            	float2 vec = float2(i.texcoord0.x-_LightPosX, i.texcoord0.y-_LightPosY); // The vector position of the current pixel with origin at the light position
   
            	float2 lightPos = float2(_LightPosX,_LightPosY); // The vector2 defining the light's position
            	
            	fixed4 ambientColor = fixed4(0.01,0.01,0.01,0); // The minimum light value
            	
				_LightDistance -= .025;
				
            	if((col.r < 1.0)||(col.g < 1.0)||(col.b < 1.0)) // If this pixel is within a shadowcasting object it will always be black
            	{
            		return fixed4(0,0,0,0);
            	}
            	else
            	{
            		
            		for(int s = 0; s<10 ; s++ ) // Iterate set amount of times for each pixel, more iterations = more accurate shadows
            		{
						// Checks if each sample pixel contains a shadowcaster, if it does it lies between the light source and this pixel, so cast a shadow.

            			fixed4 sampCol = tex2D(_Casters, lightPos+(vec*(s/(10.0))) );
            			
            			if((sampCol.r < 1.0)||(sampCol.g < 1.0)||(sampCol.b < 1.0))
            			{
            				return ambientColor; 

            			}

            		}
            	}

            	float intensity = 1.0/( pow(i.texcoord0.x-_LightPosX,2) + pow(i.texcoord0.y-_LightPosY,2) +_LightDistance);
            	
            	intensity = (_LightSize/20.0)*intensity;

            	return ambientColor+fixed4(_LightColor.r*intensity,_LightColor.g*intensity,_LightColor.b*intensity,1);

            }

            
            ENDCG
        }
    }
}
