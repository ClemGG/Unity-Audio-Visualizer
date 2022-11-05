using UnityEngine;

public class OscillatorMono : MonoBehaviour
{
    [Tooltip("La fréquence en Hertz que l'oscillateur va produire.")]
    [SerializeField] double frequency = 440.0;

    [Tooltip("Le volume de l'oscillateur ajouté au gain.")]
    [SerializeField] public float volume = .1f;

    enum WaveType { Sine, Square, Triangle};
    [SerializeField] WaveType waveType;

    [SerializeField] bool auto = true;
    [SerializeField] bool loop = true;

    public ManualMusicNote[] notes = new ManualMusicNote[8];
    [SerializeField] AutoMusicNote[] autoNotes = new AutoMusicNote[8];
    float nextNoteTimer;
    int curNote = 0;

    private double increment;                       //Le montant que l'onde va se déplacer chaque frame, déterminé par la fréquence
    private double phase;                           //Notre position actuelle sur l'onde
    private double samplingFrequency = 48000.0;     //La fréquence d'échantillonage par défaut d'Unity

    [ReadOnly, SerializeField] float gain;
    [ReadOnly, SerializeField] float[] frequencies; //Les différentes fr&quences jouées par les touches
    [ReadOnly, SerializeField] int curFreq;        //la fréquence en cours
    


    private void Start()
    {
        frequencies = new float[9];
        frequencies[0] = 0;
        frequencies[1] = 440;
        frequencies[2] = 494;
        frequencies[3] = 554;
        frequencies[4] = 587;
        frequencies[5] = 659;
        frequencies[6] = 740;
        frequencies[7] = 831;
        frequencies[8] = 880;
    }


    private void Update()
    {
        //if (Input.GetKeyDown(KeyCode.Space))
        //{
        //    gain = volume;
        //    frequency = frequencies[curFreq];
        //    curFreq += 1;
        //    curFreq = curFreq % frequencies.Length;   //Ramène thisFreq à 0 si on atteint la fin de la liste
        //}
        //if (Input.GetKeyUp(KeyCode.Space))
        //{
        //    gain = 0;
        //}


        if (!auto)
        {
            for (int i = 0; i < notes.Length; i++)
            {
                if (Input.GetKeyDown(notes[i].keycode))
                {
                    gain = volume;
                    curFreq = notes[i].frequencyIndex;
                    frequency = frequencies[curFreq];
                }
            }
        }
        else
        {
            if (curNote < autoNotes.Length)
            {
                if (nextNoteTimer < autoNotes[curNote].delayBeforeNextNote)
                {
                    nextNoteTimer += Time.deltaTime;
                    gain = autoNotes[curNote].volume;
                    curFreq = autoNotes[curNote].frequencyIndex;
                    frequency = frequencies[curFreq];
                }
                else
                {
                    nextNoteTimer = 0f;
                    curNote++;
                }
            }
            else if (loop)
            {
                curNote = 0;
            }
            else
            {
                gain = 0f;

            }
        }
    }

    private void OnAudioFilterRead(float[] data, int channels)
    {
        increment = frequency * 2.0 * Mathf.PI / samplingFrequency;

        for (int i = 0; i < data.Length; i += channels)
        {
            phase += increment;

            SwitchWaveType(ref data[i]);


            //Pour s'assurer que le son sorte des deux oreilles
            if(channels == 2)
            {
                data[i + 1] = data[i];
            }


            ////Pour jouer la phase en boucle (apparemment pas nécessaire)
            //if(phase > Mathf.PI * 2)
            //{
            //    phase -= Mathf.PI * 2;
            //}
        }
    }

    private void SwitchWaveType(ref float data)
    {
        switch (waveType)
        {
            //Sine wave
            case WaveType.Sine:
                data = (float)(gain * Mathf.Sin((float)phase));
                break;

            //Square wave
            case WaveType.Square:
                if (gain * Mathf.Sin((float)phase) >= 0 * gain)
                {
                    data = (float)gain * .6f;
                }
                else
                {
                    data = (float)-gain * .6f;
                }
                break;

            //Triangle wave
            case WaveType.Triangle:
                data = (float)(gain * (double)Mathf.PingPong((float)phase, 1f));
                break;

        }
    }
}

public enum WaveType { Sine, Square, Triangle };
public enum Channnel { Stereo, Left, Right };

[System.Serializable]
public class ManualMusicNote
{
    [HideInInspector] public bool pressed;
    public string keycode;
    public int frequencyIndex;
}


[System.Serializable]
public class AutoMusicNote
{
    public int frequencyIndex;
    public float delayBeforeNextNote;
    public float volume = .1f;
}