using UnityEngine;

public class Spectrum : MonoBehaviour
{
    #region Variables Unity

    [field: SerializeField]
    public GameObject CubePrefab { get; set; }

    /// <summary>
    /// Table de la taille de l'onde (AudioInput.Waveform)
    /// </summary>
    private GameObject[] CubesInScene { get; set; } = new GameObject[512];

    #endregion

    #region Fonctions Unity

    // Start is called before the first frame update
    void Start()
    {
        float x = -512f, y = 100f, z = 0f;
        float xIncrement = CubePrefab.transform.localScale.x * 2f;

        for (int i = 0; i < CubesInScene.Length; i++)
        {
            GameObject cubeGo = Instantiate(CubePrefab, transform);
            CubesInScene[i] = cubeGo;

            cubeGo.transform.position = new(x, y, z);
            x += xIncrement;
            cubeGo.transform.localScale = new(2f, 0f, 1f);
        }
    }

    // Update is called once per frame
    void Update()
    {
        float[] spectrum = AudioInput.s_magnitudeSpectrum;

        for (int i = 0; i < CubesInScene.Length; i++)
        {
            Transform cubeT = CubesInScene[i].transform;
            Vector3 v = cubeT.localScale;
            v.y = Mathf.Sqrt(spectrum[i]) * 600f;
            cubeT.localScale = v;
        }
    }

    #endregion
}
