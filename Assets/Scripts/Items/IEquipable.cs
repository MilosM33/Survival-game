using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IEquipable
{
    Vector3 posOffset { get;}
    Vector3 rotOffset { get; }
    void Equip();
     void Unequip();

}
