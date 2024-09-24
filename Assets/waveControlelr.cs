using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;


public class waveControlelr : MonoBehaviour
{
    MeshRenderer rend;
    public Material material;
    public AudioSource audioSource;
    private int sampleSize = 256;
    private float[] audioSamples;

    public float threshold = 0.1f;

    // Start is called before the first frame update
    void Start()
    {
        //audioSource = GetComponent<AudioSource>();
        rend = GetComponent<MeshRenderer>();
        audioSamples = new float[sampleSize];

        
    }

    // Update is called once per frame
    void Update()
    {
        audioSource.GetOutputData(audioSamples, 0);
        float sum = 0f;
        for (int i = 0; i < sampleSize; i++)
        {
            sum += audioSamples[i] * audioSamples[i];
        }
        float inputAmp = Mathf.Sqrt(sum / sampleSize);
        float waves_number = 20;
        //f (inputAmp <= threshold) waves_number = 0;
        float waveAmp = inputAmp * 0.1f;
        float speed = 0.2f;
        float noiseAmp = 5;
        float noiseScale = 9;//frequency
        material.SetFloat("_waveAmp",waveAmp);
        material.SetFloat("_speed", speed);
        material.SetFloat("_noiseAmp", noiseAmp);
        material.SetFloat("_noiseScale", noiseScale);
        material.SetFloat("_waves_number", waves_number);
        //Debug.Log(material.GetFloat("_waves_number"));
        
    }
}
