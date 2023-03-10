Shader "Custom/ShadowOnly"
{
    Properties {
        //_MainTex ("Base (RGB)", 2D) = "white" {}
    }
    SubShader
    {
        // Rien n'est affiché, mais des ombres sont tout de même projetées
        Pass {
        Name "ShadowCaster"
        Tags { "LightMode" = "ShadowCaster" }
     
        Fog {Mode Off}
        ZWrite On ZTest LEqual Cull Off
        Offset 1, 1
 
        CGPROGRAM
        #pragma vertex vert
        #pragma fragment frag
        #pragma multi_compile_shadowcaster
        #include "UnityCG.cginc"
        
        struct v2f {
            V2F_SHADOW_CASTER;
        };
 
        v2f vert( appdata_base v )
        {
            v2f o;
            TRANSFER_SHADOW_CASTER(o)
            return o;
        }
 
        float4 frag( v2f i ) : SV_Target
        {
            SHADOW_CASTER_FRAGMENT(i)
        }
        ENDCG
 
        }
    }
}