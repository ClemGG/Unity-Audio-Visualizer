using UnityEngine;

/// <summary>
/// Récupère le son du micro
/// </summary>
public class AudioInput : MonoBehaviour
{
    #region Variables statiques

    /// <summary>
    /// Le son représenté en une table de nombres (Mono)
    /// </summary>
    [field: ReadOnly, SerializeField]
    public static float[] s_waveForm { get; set; } = new float[1024];

    /// <summary>
    /// Le spectre de magnitude
    /// </summary>
    [field: ReadOnly, SerializeField]
    public static float[] s_magnitudeSpectrum { get; set; } = new float[512];

    #endregion

    #region Variables d'instance

    private AudioSource _audioSource;

    #endregion

    #region Fonctions Unity

    // Start is called before the first frame update
    void Start()
    {
        _audioSource = GetComponent<AudioSource>();
        _audioSource.loop = true;

        //S'assure qu'on ait au moins 1 entrée audio (microphone) pour continuer
        if (Microphone.devices.Length > 0)
        {
            string selectedDevice = Microphone.devices[0];

            //Le microphone devient l'AudioClip pour l'AudioSource.
            //lengthSec est la latence.
            _audioSource.clip = Microphone.Start(selectedDevice, true, 1, AudioSettings.outputSampleRate);

            //Réduit la latence du micro
            while (Microphone.GetPosition(selectedDevice) > 0) { }
        }

        _audioSource.Play();
    }

    // Update is called once per frame
    void Update()
    {
        //Récupère les données du son joué
        _audioSource.GetOutputData(s_waveForm, 0);
        _audioSource.GetSpectrumData(s_magnitudeSpectrum, 0, FFTWindow.Hanning);
    }

    #endregion
}
