Shader "Custom/CTestShader"
{
    Properties
	{
       _Gloss("Gloss", Range(1,50)) = 10
	   _SpeColor("SpeColor", Color) = (1,1,1,1)
    }

	SubShader
	{
	   Pass
	   {
		   Tags { "LightMode" = "ForwardBase" }

		   CGPROGRAM

		   #include "Lighting.cginc"

  	       #pragma vertex vertFunc
	       #pragma fragment fragFunc

	       fixed4 _SpeColor;
	       float _Gloss;

	       struct a2v
	       {
	          float4 objectVertex : POSITION;
		      float3 objectNormal : NORMAL;
	       };

	       struct v2f
	       {
	          float4 clipsVertex : SV_POSITION;
		      float3 worldNormal : TEXCOORD0;
			  float4 worldVertex : TEXCOORD1;
	       };

	       v2f vertFunc(a2v vData)
	       {
		      v2f fData;
		      fData.clipsVertex = UnityObjectToClipPos(vData.objectVertex);
		      fData.worldNormal = normalize(mul((float3x3)unity_ObjectToWorld, vData.objectNormal));
			  fData.worldVertex = mul(unity_ObjectToWorld, vData.objectVertex);
		      return fData;
	       }

	       fixed4 fragFunc(v2f fData):SV_Target
	       {
		      fixed3 ambient = UNITY_LIGHTMODEL_AMBIENT.rgb;

	   	      fixed3 lightDir = normalize(_WorldSpaceLightPos0.xyz);
		      //fixed3 diffuseColor = _LightColor0.rgb * (dot(lightDir, fData.worldNormal)*0.5 + 0.5);
		      fixed3 diffuseColor = _LightColor0.rgb * max(0, dot(lightDir, fData.worldNormal));
		     
			  fixed3 worldViewDir = normalize(_WorldSpaceCameraPos.xyz - fData.worldVertex);
			  //fixed3 worldViewDir = UnityWorldSpaceViewDir(fData.worldVertex);
			  fixed3 worldReflectDir = normalize(reflect(-lightDir, fData.worldNormal));
			  fixed3 specularColor = _LightColor0.rgb * _SpeColor.rgb * pow(max(0, dot(worldViewDir, worldReflectDir)), _Gloss);

		      fixed3 color = ambient + diffuseColor + specularColor;
		      return fixed4(color,1);
	       }
	       ENDCG
	   }
	}

	Fallback "Diffuse"
}
