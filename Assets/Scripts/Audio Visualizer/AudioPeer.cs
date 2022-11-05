using System;
using UnityEngine;


[RequireComponent(typeof(AudioSource))]
public class AudioPeer : MonoBehaviour
{
    public static int samplingRatio;
    public static int nbBands;

    public static float[] samplesLeft;
    public static float[] samplesRight;
    public static float[] freqBands;
    public static float[] bandBuffers;

    //Renvoient une valeur entre 0 et 1
    public static float[] audioBand;
    public static float[] audioBandBuffer;

    public static float amplitude, amplitudeBuffer;
    float amplitudeHighest;

    float[] bufferDecrease;
    float[] freqBandsHighest;

    [SerializeField] AudioClip soundTest;
    [SerializeField] Channnel channel;
    [SerializeField] int _nbBands = 8;
    [SerializeField] int _samplingRatio = 512;
    [SerializeField] float frequencyBandAverageScale = 10f;
    [SerializeField] float bufferDeceaseStart = .005f;
    [SerializeField] float bufferDeceaseAcceleration = 1.2f;

    [Tooltip("Assigne une valeur de base à frequencyHighest pour que l'audio démarre correctement son échantillonnement.")]
    [SerializeField] float audioProfile = 5f;

    AudioSource source;

    private void OnValidate()
    {
        samplingRatio = _samplingRatio;
        nbBands = _nbBands;
    }


    private void Reset()
    {
        OnValidate();
    }

    // Start is called before the first frame update
    void Awake()
    {
        source = GetComponent<AudioSource>();
        if (soundTest) 
        {
            source.clip = soundTest;
            source.Play();
        } 
        samplesLeft = new float[samplingRatio];
        samplesRight = new float[samplingRatio];

        //Chacune doit avoir sa propre ligne pour qu'elles ne s'écrasent pas les unes les autres
        freqBands = new float[nbBands];
        freqBandsHighest = new float[nbBands];
        bandBuffers = new float[nbBands];
        bufferDecrease = new float[nbBands];
        audioBand = new float[nbBands];
        audioBandBuffer = new float[nbBands];

        AudioProfile(audioProfile);
    }


    // Update is called once per frame
    void Update()
    {
        GetSpectrumAudioSource();
        MakeFrequencyBands();
        BandBuffer();
        CreateAudioBands();
        GetAmplitude();
    }







    private void AudioProfile(float _audioProfile)
    {
        for (int i = 0; i < nbBands; i++)
        {
            freqBandsHighest[i] = _audioProfile;
        }
    }

    private void GetAmplitude()
    {
        float curAmplitude = 0f, curAmplitudeBuffer = 0f;

        for (int i = 0; i < nbBands; i++)
        {
            curAmplitude += audioBand[i];
            curAmplitudeBuffer += audioBandBuffer[i];
        }

        if(curAmplitude > amplitudeHighest)
        {
            amplitudeHighest = curAmplitude;
        }
        amplitude = curAmplitude / amplitudeHighest;
        amplitudeBuffer = curAmplitudeBuffer / amplitudeHighest;
    }


    //Pour avoir la valeur de la bande (si le spectre audio tombe dans son intervalle ou non)
    private void CreateAudioBands()
    {
        for (int i = 0; i < nbBands; i++)
        {
            if(freqBands[i] > freqBandsHighest[i])
            {
                freqBandsHighest[i] = freqBands[i];
            }
            audioBand[i] = freqBands[i] / freqBandsHighest[i];
            audioBandBuffer[i] = bandBuffers[i] / freqBandsHighest[i];
        }
    }


    //Pour afficher le spectre audio sur la fréquenc d'échantillonage (ici 512)
    void GetSpectrumAudioSource()
    {
        source.GetSpectrumData(samplesLeft, 0, FFTWindow.Blackman);
        source.GetSpectrumData(samplesRight, 1, FFTWindow.Blackman);
    }


    //Renvoie la moyenne du spectre audio pour ne l'afficher que sur une bande réduite (ici 8 bandes)
    void MakeFrequencyBands()
    {
        int count = 0;
        int sampleCount = 1;
        int power = 0;


        for (int i = 0; i < nbBands; i++)
        {
            float average = 0;
            if(i == 16 || i == 32 || i == 40 || i == 48 || i == 56)
            {
                power++;
                sampleCount = (int)Mathf.Pow(2, power);
                if (power == 3) sampleCount -= 2;
            }

            //int sampleCount = (int)Mathf.Pow(2, i) * 2;

            //if (i == 7) sampleCount += 2;


            for (int j = 0; j < sampleCount; j++)
            {
                switch (channel)
                {
                    case Channnel.Stereo:
                        average += samplesLeft[count] + samplesRight[count] * (count + 1);
                        break;
                    case Channnel.Left:
                        average += samplesLeft[count] * (count + 1);
                        break;
                    case Channnel.Right:
                        average += samplesRight[count] * (count + 1);
                        break;
                }
                count++;
            }

            average /= count;
            freqBands[i] = average * frequencyBandAverageScale;
        }
    }


    //Pour adoucir la retombée des bandes de fréquence
    void BandBuffer()
    {
        for (int i = 0; i < nbBands; i++)
        {
            if (freqBands[i] > bandBuffers[i])
            {
                bandBuffers[i] = freqBands[i];
                bufferDecrease[i] = bufferDeceaseStart;
            }
            if (freqBands[i] < bandBuffers[i])
            {
                bandBuffers[i] -= bufferDecrease[i];
                bufferDecrease[i] *= bufferDeceaseAcceleration;

            }
        }
    }
}
