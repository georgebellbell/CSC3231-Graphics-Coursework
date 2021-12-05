Shader "Custom/OceanShader"
{
    Properties
    {
        _Color("Color", Color) = (1,1,1,1)
        _MainTex("Texture", 2D) = "white" {}
        
        // variables for wave movement
        _Wave1("Wave 1", Vector) = (1,0,0.5,10)
        _Wave2("Wave 2", Vector) = (0,1,0.25,20)
        _Wave3("Wave 3", Vector) = (1,1,0.15,10)

        // variables for water foam
        _EdgeColor("Edge Color", Color) = (1,1,1,1)
        _DepthFactor("Depth Factor", float) = 1
    }
        SubShader
        {
            Tags { "RenderType" = "Transparent" "Queue" = "Transparent"}
            LOD 200

            //Blend One One
            //ZTest Always
            Pass
            {
                Blend SrcAlpha OneMinusSrcAlpha
                CGPROGRAM
                #pragma vertex      vert
                #pragma fragment    frag
                #pragma alpha
                #pragma addshadow

                #include "UnityCG.cginc"

                struct appdata
                {
                    float4 vertex   : POSITION;
                    float2 uv       : TEXCOORD0;
                };

                struct v2f
                {
                    float2 uv       : TEXCOORD0;
                    float4 screenDepth : TEXCOORD1;
                    float4 vertex   : SV_POSITION;
                };

                float4 _Color;
                float4 _Wave1, _Wave2, _Wave3;

                sampler2D _CameraDepthTexture;
                float _DepthFactor;
                float4 _EdgeColor;

                
                // Uses trochoidal (or Gerstner) waves to calculate the position, tangent and binormal of a given vertex
                float3 GerstnerWave(float4 wave, float3 vertexPos, inout float3 tangent, inout float3 binormal) {
                    float steepness = wave.z;
                    float wavelength = wave.w;

                    float waveNumber = 2 * UNITY_PI / wavelength;
                    float phaseSpeed = sqrt(9.8 / waveNumber);
                    float2 direction = normalize(wave.xy);
                    float f = waveNumber * (dot(direction, vertexPos.xz) - phaseSpeed * _Time.y);
                    float a = steepness / waveNumber;

                    float s = steepness * sin(f);
                    float c = steepness * cos(f);

                    tangent +=  float3(-direction.x * direction.x * s, direction.x * c, -direction.x * direction.y * s);
                    binormal += float3(-direction.x * direction.y * s, direction.y * c, -direction.y * direction.y * s);
                    float3 newPosition = float3(direction.x * (a * cos(f)), a * sin(f), direction.y * (a * cos(f)));

                    return newPosition;
                }
          
                v2f vert(appdata v)
                {
                    v2f output;

                    float4 localSpace = v.vertex;

                    float3 gridPoint = localSpace.xyz;

                    float3 tangent = float3(1, 0, 0);
                    float3 binormal = float3(0, 0, 1);
                    
                    // calculate the vertex position using three seperate instances of GerstnerWave
                    gridPoint += GerstnerWave(_Wave1, gridPoint, tangent, binormal);
                    gridPoint += GerstnerWave(_Wave2, gridPoint, tangent, binormal);
                    gridPoint += GerstnerWave(_Wave3, gridPoint, tangent, binormal);

                    float3 normal = normalize(cross(binormal, tangent));

                    localSpace.xyz = gridPoint;

                    output.vertex = UnityObjectToClipPos(localSpace);
                    output.screenDepth = ComputeScreenPos(output.vertex);
                    output.uv = normal;

                    return output;
                }


                float4 frag(v2f input) : COLOR
                {
                    // generates foam line next to land and other objects that collide by using a screen position and camera depth
                    float4 depthSample = SAMPLE_DEPTH_TEXTURE_PROJ(_CameraDepthTexture, input.screenDepth);
                    float depth = LinearEyeDepth(depthSample).r;

                    float foamLine = 1 - saturate(_DepthFactor * (depth - input.screenDepth.w));

                    float4 col = _Color + foamLine * _EdgeColor;

                    return col;
                }
                ENDCG
            }
        }
}
