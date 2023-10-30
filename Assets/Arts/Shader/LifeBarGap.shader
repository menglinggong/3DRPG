Shader "MyShader/MyLifeBarGap"
{
    Properties
    {
        [HideInInspector]
        _Width("Width", float) = 10
        _Color("Color", Color) = (1,1,1,1)
        _BlackWidth("BlackWidth", float) = 1
        _BloodVolume("BloodVolume", float) = 5
        _BlackColor("BlackColor", Color) = (0,0,0,1)
        _life("life", Range(0,1)) = 1
        _LossColor("LossColor", Color) = (1,1,1,1)
    }
    SubShader
    {
        Cull Off
        ZWrite Off
        ZTest Off
        Blend SrcAlpha OneMinusSrcAlpha
 
 
        Tags
        {
            "Queue" = "Transparent"
            "RenderType" = "Transparent"
        }
 
        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            
            #include "UnityCG.cginc"
 
            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };
 
            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };
 
            float _Width;
            fixed4 _Color;
            float _BlackWidth;
            float _BloodVolume;
            fixed4 _BlackColor;
            fixed _life;
            fixed4 _LossColor;
 
            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }
 
            fixed4 frag (v2f i) : SV_Target
            {
                half virtualHealthBarWidth = i.uv.x * _BloodVolume;
                
                //如果当前位置是1000血的倍数，且在间隔的两倍宽度位置，设置颜色显示为黑色
                if (virtualHealthBarWidth % _Width > _Width - _BlackWidth * 2 && (virtualHealthBarWidth + _Width) / _Width % _Width < 1)
                {
                    return _BlackColor;
                }
                //如果当前位置在间隔宽度内
                else if (virtualHealthBarWidth % _Width > _Width - _BlackWidth)
                {
                    //设置一半显示为黑色
                    if (i.uv.y > 0.4)
                    {
                        return _BlackColor;
                    }
                    else
                    {
                        //如果未超出剩余血量，显示为血条颜色，否则显示为损失的颜色
                        if(i.uv.x < _life)
                            return _Color;
                        else
                            return _LossColor;
                    }
                }
                //剩余情况为不在间隔范围内
                else
                {
                    //如果未超出剩余血量，显示为血条颜色，否则显示为损失的颜色
                    if (i.uv.x < _life)
                        return _Color;
                    else
                        return _LossColor;
                }
            }
            ENDCG
        }
    }
}