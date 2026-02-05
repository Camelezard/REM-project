using UnityEngine;

public class AudioLoudnessDetection : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    public int sampleWindow = 64;
    private AudioClip microphoneClip;
    void Start()
    {
        MicrophoneToAudioClip();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void MicrophoneToAudioClip()
    {
        string lMicrophoneName = Microphone.devices[1];
        microphoneClip = Microphone.Start(null, true, 20, AudioSettings.outputSampleRate);
    }

    public float GetLoudnessFromMicrophone()
    {
        return GetLoudnessFromAudiolip(Microphone.GetPosition(null), microphoneClip);
    }

    public float GetLoudnessFromAudiolip(int pClipPosition, AudioClip pClip)
    {
        int lStartPos = pClipPosition - sampleWindow;

        if (lStartPos < 0) return 0;

        float[] lWaveData = new float[sampleWindow];
        pClip.GetData(lWaveData, lStartPos);

        //Compute loudness
        float lTotalLoudness = 0;

        for (int i = 0; i < sampleWindow; i++)
        {
            lTotalLoudness += Mathf.Abs(lWaveData[i]);
        }

        return lTotalLoudness / sampleWindow;
    }
}
