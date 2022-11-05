using UnityEngine;

public class AudioBandCube : MonoBehaviour
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
        mat = t.GetChild(0).GetComponent<MeshRenderer>().material;

        band = t.GetSiblingIndex();
    }

    // Update is called once per frame
    void Update()
    {
        float value = useBuffers ? AudioPeer.audioBandBuffer[band] : AudioPeer.audioBand[band];
        t.localScale = new Vector3(t.localScale.x, (value * scaleMultiplier) + startScale, t.localScale.z);
        Color col = Color.Lerp(startCol, endCol, value);
        mat.SetColor("_Color", col);
        mat.SetColor("_EmissionColor", col);
    }
}
