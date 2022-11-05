using UnityEngine;

public class AmplitudeCube : MonoBehaviour
{
    [SerializeField] int band;
    [SerializeField] float startScale = 1f, scaleMultiplier = 30f;

    [SerializeField] Color startCol = Color.yellow, endCol = Color.cyan;

    [Tooltip("Si à true, les bandes afficheront la fréquence avec une retombée adoucie si elles ne sont plus appelées.")]
    [SerializeField] bool useBuffers = false;

    Transform t;
    Material mat;

    // Start is called before the first frame update
    void Start()
    {
        t = transform;
        mat = GetComponent<MeshRenderer>().material;

        band = t.GetSiblingIndex();
    }

    // Update is called once per frame
    void Update()
    {
        float value = useBuffers ? AudioPeer.amplitudeBuffer : AudioPeer.amplitude;
        float scale = (value * scaleMultiplier) + startScale;
        Color col = Color.Lerp(startCol, endCol, value);

        t.localScale = Vector3.one * scale;
        mat.SetColor("_Color", col);
        mat.SetColor("_EmissionColor", col);
    }
}
