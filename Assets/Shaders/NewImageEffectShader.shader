// Upgrade NOTE: replaced '_World2Object' with 'unity_WorldToObject'

// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "TestShader/MyShader"
{
	Properties
	{
		//_MainTex ("Texture", 2D) = "white" {}

		/*_Color("MainColor",Color) = (1,1,1,1)
	    _Vector("Vector",Vector) = (1,1,1,1)
		_Int("Int",Int) = 222
		_Float("Float",Float) = 3.4
		_Range("Range",Range(0,1)) = 0.5
		//2D纹理
		_2D("Texture",2D) = "red"{}
		//3D纹理
		_3D("Texture",3D) = "white"{}
		//立方体纹理 天空盒
		_Cube("Cube",Cube) = "black"{}*/

		_SelfColor("selfColor", Color) = (1,1,1,1) // 添加一个颜色来改变自身的颜色
		_Gloss("Gloss", Float) = 10
	}

	SubShader
	{
		Pass
		{
		    Tags { "LightMode" = "ForwardBase" }  // 表示当前pass块是为哪一个 rendering path 设计的 有不同类型 如：Vertex  ForwardAdd 等 
			                                     // camera 支持不同的 renderingPath ...
			CGPROGRAM

			#include "Lighting.cginc" // 引用Unity内置文件

			// 使用属性时重新定义 名字一样 自动赋属性块中的值 结尾加分号
			// float 可以用  half  fixed 来代替 区别在于不同的范围和精度

			// float 32位
			// half 16位  [-6w , 6w]
			// fixed 11位 [-2, 2]

			// 为了避免浪费显存 一般颜色用 fixed 存储   位置用 float 存储  

			/*float4 _Color;
			float4 _Vector;
			float _Int;
			float _Float;
			float _Range;

			sampler2D _2D;
			sampler3D _3D;
			samplerCUBE _Cube;*/

			fixed4 _SelfColor;
			float _Gloss;

			// 定义函数 顶点函数  【 完成顶点坐标从 模型空间 -> 剪裁空间 的转换 】
			#pragma vertex vert
			// 片元函数  【 返回模型对应屏幕上的每个像素的颜色值 】
			#pragma fragment frag

			// 通过语义来指定参数和函数的意义 
			// POSITION 指定该输入变量为 模型空间下顶点坐标 这样系统就知道 vpos 传进来是什么值
			// SV_POSITION 指定该函数的输出值为剪裁空间下的顶点坐标
			// 顶点着色器的输出 就是 片元着色器的输入 两者的语义是一致的


			// application to vertex
			struct a2v
			{
				float4 vertex : POSITION;  // 顶点坐标
				float3 normal : NORMAL;    // 法线
				float4 texcoord : TEXCOORD0; // 纹理坐标
			};

			// vertex to fragment
			struct v2f
			{
			    float4 vertex : SV_POSITION;
				float3 normal : NORMAL;
				float3 color : COLOR0;
			};  


			v2f vert(a2v vdata) 
			{
			   v2f fdata;
			   fdata.vertex = UnityObjectToClipPos(vdata.vertex);  // 最新方法 替代用mul()直接矩阵相乘 
			   //fdata.normal = normalize(mul(vdata.normal, (float3x3)unity_WorldToObject));
			   //注意 法线（模型空间） 和 光源（世界空间） 要保持同一个空间
			   fixed3 ligthDir = normalize(_WorldSpaceLightPos0.xyz);  // 光源单位向量
			   fixed3 normalDir = normalize(mul(vdata.normal, (float3x3)unity_WorldToObject)); // 法线向量 这里将法线从模型空间转为世界空间

			   //兰伯特光照
			   //fixed3 diffuseColor = _LightColor0.rgb * max(0, dot(ligthDir, normalDir)) * _SelfColor.rgb;

			   //半兰伯特光照
			   fixed3 diffuseColor = _LightColor0.rgb * dot(ligthDir, normalDir)*0.5 + 0.5;

			   fixed3 ambient = UNITY_LIGHTMODEL_AMBIENT.rgb;  // 环境光 UNITY_LIGHTMODEL_AMBIENT

			   // 高光反射
			   fixed3 reflectDir = normalize(reflect(-ligthDir, normalDir));//这里得取负数才是入射光方向 否则效果则会刚好相反亮点会出现在阴暗面
			   fixed3 viewDir = normalize(_WorldSpaceCameraPos.xyz - mul(unity_ObjectToWorld, vdata.vertex)); // 注意同步坐标空间
			   fixed3 specluar = _LightColor0.rgb * _SelfColor.rgb * pow(max(0, dot(reflectDir, viewDir)), _Gloss);

			   fdata.color = diffuseColor + ambient + specluar;

			   return fdata;
			}

			fixed4 frag(v2f fdata):SV_Target
			{
			    //fixed3 ligthDir = normalize(_WorldSpaceLightPos0.xyz);  // 光源单位向量

			    //fixed3 diffuseColor = _LightColor0.rgb * max(0, dot(ligthDir, fdata.normal));
			    
			    //return fixed4(diffuseColor,1);
			   return fixed4(fdata.color,1);
			}

			ENDCG
		}
	}

	Fallback "Diffuse"
}
