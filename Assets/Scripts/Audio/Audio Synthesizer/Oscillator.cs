using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class Oscillator
{
    public double frequency = 440.0;
    public float gain;

    public WaveType waveType;
    
    public bool loop = true;


    private double increment;                       //Le montant que l'onde va se déplacer chaque frame, déterminé par la fréquence
    private double phase;                           //Notre position actuelle sur l'onde
    private double samplingFrequency = 48000.0;     //La fréquence d'échantillonage par défaut d'Unity


    float[] frequencies; //Les différentes fréquences jouées par les touches
    int curFreq;        //la fréquence en cours







    public Oscillator(bool _loop, WaveType _waveType)
    {
        loop = _loop;
        waveType = _waveType;


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

    public void GenerateSound(float volume, ManualMusicNote note)
    {
        gain = volume;
        curFreq = note.frequencyIndex;
        frequency = frequencies[curFreq];
    }


    public void GenerateSound(AutoMusicNote note)
    {
        gain = note.volume;
        curFreq = note.frequencyIndex;
        frequency = frequencies[curFreq];
    }

    public void Stop()
    {
        gain = 0f;
        curFreq = 0;
    }






    public void OnAudioFilterRead(float[] data, int channels)
    {
        increment = frequency * 2.0 * Mathf.PI / samplingFrequency;

        for (int i = 0; i < data.Length; i += channels)
        {
            phase += increment;

            SwitchWaveType(ref data[i]);


            //Pour s'assurer que le son sorte des deux oreilles
            if (channels == 2)
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
