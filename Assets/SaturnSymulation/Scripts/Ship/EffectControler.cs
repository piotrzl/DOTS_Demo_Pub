using System.Collections;
using System.Collections.Generic;
//using System.Numerics;
using Unity.VisualScripting;
using UnityEngine;

public class EffectControler : MonoBehaviour
{

    [SerializeField] List<TrailRenderer> thrusterTrailRenderers = new List<TrailRenderer>();
    [SerializeField] List<ParticleSystem> thrusterParticle = new List<ParticleSystem>();
    [SerializeField] float particleThrusterPower = 0.2f;
    
    [SerializeField] Vector3 trialLocalDistanceWithPower = new Vector3(0,0,-2f);

    List<Vector3> trialBasePosition = new List<Vector3>();
    private void OnEnable()
    {
        for (int i = 0; i < thrusterTrailRenderers.Count; ++i)
        {
            trialBasePosition.Add(thrusterTrailRenderers[i].transform.localPosition);
        }
    }

    void Start() 
    {
        EnableTrialThruster(false);
        EnableParticleThruster(false);
    }


    public void EnableTrialThruster(bool enable) 
    {
        for(int i =0; i < thrusterTrailRenderers.Count; ++i) 
        {
            thrusterTrailRenderers[i].emitting = enable;
        }
    }
    
    public void SetThrusterPower(int power) // power = 1 to 10 
    {
        power = Mathf.Clamp(power, 1, 10);

        for (int i = 0; i < thrusterParticle.Count; ++i)
        {
            thrusterParticle[i].startLifetime = power * particleThrusterPower;

        }

        for (int i = 0; i < thrusterTrailRenderers.Count; ++i)
        {
            thrusterTrailRenderers[i].transform.localPosition = trialBasePosition[i] + trialLocalDistanceWithPower * power;
        }
    }

    public void EnableParticleThruster(bool enable) 
    {
        for (int i = 0; i < thrusterParticle.Count; ++i)
        {
            if(enable)
                thrusterParticle[i].Play(true);
            else
                thrusterParticle[i].Stop(true);
        }
    }


}
