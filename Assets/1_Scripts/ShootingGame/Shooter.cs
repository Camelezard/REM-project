using UnityEngine;

public class Shooter : Dodger
{
    public GameObject bullet;
    public Transform cannon;
    public float delayBetweenShots = 1f;
    private float _ElapsedTime = 0f;
    protected override void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
        Shoot();
    }

    private void Shoot()
    {
        _ElapsedTime += Time.deltaTime;
        if (_ElapsedTime > delayBetweenShots)
        {
            Instantiate(bullet, cannon.position, Quaternion.identity);
            _ElapsedTime = 0f;
        }
    }
}
