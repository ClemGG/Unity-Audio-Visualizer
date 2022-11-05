using System;
using System.Collections;
using UnityEngine;

public class AudioCubeSpectrum : MonoBehaviour
{
    [SerializeField] GameObject spectrumPrefab;
    [SerializeField] bool spawnSpectrum = true;
    [SerializeField] float maxScale = 10000f;
    [SerializeField] float spectrumSpacing = 100f;
    [SerializeField] float waitDelay = .001f;
    [SerializeField] Color startCol = Color.yellow, endCol = Color.cyan;

    GameObject[] spawnedSpectrumPrefabs;

    int samplingRatio;

    Transform t;

    private void OnValidate()
    {
        t = transform;

        if (Application.isPlaying)
        {
            for (int i = 0; i < samplingRatio; i++)
            {
                if (spawnedSpectrumPrefabs[i]) PlaceSpectrumPrefab(spawnedSpectrumPrefabs[i], i);
                
            }
        }
    }


    // Start is called before the first frame update
    void Start()
    {
        t = transform;
        if(spawnSpectrum && spectrumPrefab) StartCoroutine(SpawnSpectrumPrefabs());

    }


    private void Update()
    {
        for (int i = 0; i < samplingRatio; i++)
        {
            if(spawnedSpectrumPrefabs[i] != null)
            {
                spawnedSpectrumPrefabs[i].transform.localScale = new Vector3(1f, (AudioPeer.samplesLeft[i] + AudioPeer.samplesRight[i]) * maxScale, 1f);
            }
        }
    }



    private IEnumerator SpawnSpectrumPrefabs()
    {
        samplingRatio = AudioPeer.samplingRatio;
        spawnedSpectrumPrefabs = new GameObject[samplingRatio];

        WaitForSeconds wait = new WaitForSeconds(waitDelay);
        for (int i = 0; i < samplingRatio; i++)
        {
            GameObject instanceSpectrumPrefab = Instantiate(spectrumPrefab, transform);
            instanceSpectrumPrefab.name = $"SpectrumCube n°{i + 1}";
            PlaceSpectrumPrefab(instanceSpectrumPrefab, i, true);
            spawnedSpectrumPrefabs[i] = instanceSpectrumPrefab;

            if (!Mathf.Approximately(waitDelay, 0f)) yield return wait;
        }
    }


    void PlaceSpectrumPrefab(GameObject prefab, int i, bool start = false)
    {
        float angle = 360f / samplingRatio * i * Mathf.Deg2Rad;
        Transform prefabT = prefab.transform;
        prefabT.position = (Vector3.forward * Mathf.Sin(angle) + Vector3.right * Mathf.Cos(angle)) * spectrumSpacing;
        prefabT.rotation = Quaternion.LookRotation((t.position - prefabT.position), Vector3.up);

        if (start)
        {

            Material prefabMat = prefabT.GetChild(0).GetComponent<MeshRenderer>().materials[0];
            float ratio = (float)i / (float)samplingRatio / 1f;

            Color newCol = Color.Lerp(startCol, endCol, ratio);
            prefabMat.SetColor("_Color", newCol);
            prefabMat.SetColor("_EmissionColor", newCol);
        }
    }

}
