using PlasticGui.WorkspaceWindow.Items;
using UnityEngine;

/// <summary>
/// Affiche l'onde sonore dans la scène sous la forme
/// d'une rangée de cubes
/// </summary>
public class Waveform : MonoBehaviour
{
    #region Variables Unity

    [field: SerializeField]
    public GameObject CubePrefab { get; set; }

    /// <summary>
    /// Table de la taille de l'onde (AudioInput.Waveform)
    /// </summary>
    private GameObject[] CubesInScene { get; set; } = new GameObject[1024];

    #endregion

    #region Fonctions Unity

    // Start is called before the first frame update
    void Start()
    {
        float x = -512f, y = -100f, z = 0f;
        float xIncrement = CubePrefab.transform.localScale.x;

        for (int i = 0; i < CubesInScene.Length; i++)
        {
            GameObject cubeGo = Instantiate(CubePrefab, transform);
            CubesInScene[i] = cubeGo;

            cubeGo.transform.position = new(x, y, z);
            x += xIncrement;
        }
    }

    // Update is called once per frame
    void Update()
    {
        float[] wf = AudioInput.s_waveForm;

        for (int i = 0; i < CubesInScene.Length; i++)
        {
            Transform cubeT = CubesInScene[i].transform;
            Vector3 v = cubeT.localPosition;
            v.y = wf[i] * 100f;
            cubeT.localPosition = v;
        }
    }

    #endregion
}
