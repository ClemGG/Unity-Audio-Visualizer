using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class OscillatorPlayer : MonoBehaviour
{
    Oscillator oscillator;


    [SerializeField] bool auto = true, loop = true;
    [SerializeField] WaveType waveType;
    [SerializeField] float volume = .1f;

    [Space(10)]

    [SerializeField] ManualMusicNote[] manualNotes;
    [SerializeField] AutoMusicNote[] autoNotes;



    float nextNoteTimer;
    int curNote = 0;


    private void OnValidate()
    {
        if (Application.isPlaying) 
        {
            oscillator = new Oscillator(loop, waveType);
        }
    }

    private void Start()
    {
        OnValidate();
    }

    private void Update()
    {
        if (auto)
        {
            if (curNote < autoNotes.Length)
            {
                if (nextNoteTimer < autoNotes[curNote].delayBeforeNextNote)
                {
                    nextNoteTimer += Time.deltaTime;
                    oscillator.GenerateSound(autoNotes[curNote]);
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
                oscillator.Stop();
            }
        }



        else
        {
            for (int i = 0; i < manualNotes.Length; i++)
            {
                ManualMusicNote note = manualNotes[i];

                if (Input.GetKeyDown(note.keycode))
                {
                    note.pressed = true;
                    oscillator.GenerateSound(volume, manualNotes[i]);
                }
                else if(Input.GetKeyUp(note.keycode))
                {
                    note.pressed = false;
                }

            }

            bool stop = true;
            for (int i = 0; i < manualNotes.Length; i++)
            {
                ManualMusicNote note = manualNotes[i];

                if (note.pressed)
                    stop = false;
            }
            if (stop) oscillator.Stop();
        }


    }


    private void OnAudioFilterRead(float[] data, int channels)
    {
        oscillator.OnAudioFilterRead(data, channels);
    }
}
