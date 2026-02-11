using UnityEngine;
using System;

public class SpawnSystem : MonoBehaviour
{
    public float mob1SpawnTime = 10f;
    public float mob2SpawnTime = 10f;
    public float mob3SpawnTime = 10f;

    public float mob1Timer;
    public float mob2Timer;
    public float mob3Timer;

    public bool mob1Ready;
    public bool mob2Ready;
    public bool mob3Ready;

    public event Action OnMob1Ready;
    public event Action OnMob2Ready;
    public event Action OnMob3Ready;

    void Update()
    {
        Tick(ref mob1Timer, mob1SpawnTime, ref mob1Ready, OnMob1Ready);
        Tick(ref mob2Timer, mob2SpawnTime, ref mob2Ready, OnMob2Ready);
        Tick(ref mob3Timer, mob3SpawnTime, ref mob3Ready, OnMob3Ready);
    }

    void Tick(ref float timer, float maxTime, ref bool ready, Action readyEvent)
    {
        if (ready) return;

        timer += Time.deltaTime;

        if (timer >= maxTime)
        {
            ready = true;
            readyEvent?.Invoke();
        }
    }

    public void ResetMob1()
    {
        mob1Timer = 0f;
        mob1Ready = false;
    }

    public void ResetMob2()
    {
        mob2Timer = 0f;
        mob2Ready = false;
    }

    public void ResetMob3()
    {
        mob3Timer = 0f;
        mob3Ready = false;
    }
}
