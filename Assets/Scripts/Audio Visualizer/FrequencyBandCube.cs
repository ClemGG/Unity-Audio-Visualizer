using UnityEngine;


//Instancié dans AudioCubeSpawner
public class FrequencyBandCube : MonoBehaviour
{
    [SerializeField] int band;
    [SerializeField] float startScale = 1f, scaleMultiplier = 30f;

    [Tooltip("Si à true, les bandes afficheront la fréquence avec une retombée adoucie si elles ne sont plus appelées.")]
    [SerializeField] bool useBuffers = false;

    Transform t;

    // Start is called before the first frame update
    void Start()
    {
        t = transform;
        band = t.GetSiblingIndex();
    }

    // Update is called once per frame
    void Update()
    {
        t.localScale = new Vector3(t.localScale.x, ((useBuffers ? AudioPeer.bandBuffers[band] : AudioPeer.freqBands[band]) * scaleMultiplier) + startScale, t.localScale.z);
    }
}
