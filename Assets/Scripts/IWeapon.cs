using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IWeapon
{
    public void Shoot();
    public void Reload();
    public void StartRecoil();
    public void StopRecoil();
}
