using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroneShooting : ShootingScript
{
    private void Update()
    {
        this.Timer += Time.deltaTime;

        if (this.Timer >= this.FireRate * this.EffectsDisplayTime)
        {
            this.DisableEffects();
        }
    }

    public override void Shoot(Vector3 origin, Quaternion rotation)
    {
        if (this.Timer >= this.FireRate)
        {
            // use this.transform instead of origin parameter
            base.Shoot(this.transform.position, rotation);

            StartCoroutine(this.FireBullet());
        }
    }

    //private IEnumerator FirePlasmaBullet()
    //{
    //    GameObject bullet = this.GetBullet(enumBulletType.Plasma);

    //    if (bullet != null)
    //    {
    //        yield return new WaitForSeconds(3f);

    //        bullet.SetActive(false);
    //    }
    //}
}
