using System.Collections;
using UnityEngine;

public class DamageFlash : MonoBehaviour
{
    public Color flashColor = Color.white;
    public float flashTime = 0.08f;

    static readonly int BaseColorId = Shader.PropertyToID("_BaseColor");
    static readonly int ColorId = Shader.PropertyToID("_Color");

    Renderer[] _renderers;
    Material[][] _mats;
    Color[][] _orig;
    Coroutine _co;

    void Awake()
    {
        _renderers = GetComponentsInChildren<Renderer>(true);
        _mats = new Material[_renderers.Length][];
        _orig = new Color[_renderers.Length][];

        for (int i = 0; i < _renderers.Length; i++)
        {
            _mats[i] = _renderers[i].materials; // 会实例化，数量不大没问题
            _orig[i] = new Color[_mats[i].Length];

            for (int j = 0; j < _mats[i].Length; j++)
            {
                var m = _mats[i][j];
                if (m.HasProperty(BaseColorId)) _orig[i][j] = m.GetColor(BaseColorId);
                else if (m.HasProperty(ColorId)) _orig[i][j] = m.GetColor(ColorId);
                else _orig[i][j] = Color.white;
            }
        }
    }

    public void Flash()
    {
        if (_co != null) StopCoroutine(_co);
        _co = StartCoroutine(FlashCo());
    }

    IEnumerator FlashCo()
    {
        SetAll(flashColor);
        yield return new WaitForSeconds(flashTime);
        Restore();
        _co = null;
    }

    void SetAll(Color c)
    {
        for (int i = 0; i < _mats.Length; i++)
        for (int j = 0; j < _mats[i].Length; j++)
        {
            var m = _mats[i][j];
            if (m.HasProperty(BaseColorId)) m.SetColor(BaseColorId, c);
            else if (m.HasProperty(ColorId)) m.SetColor(ColorId, c);
        }
    }

    void Restore()
    {
        for (int i = 0; i < _mats.Length; i++)
        for (int j = 0; j < _mats[i].Length; j++)
        {
            var m = _mats[i][j];
            var c = _orig[i][j];
            if (m.HasProperty(BaseColorId)) m.SetColor(BaseColorId, c);
            else if (m.HasProperty(ColorId)) m.SetColor(ColorId, c);
        }
    }
}
