using UnityEngine;
using System.Collections;

public class Constants : Singleton<Constants> 
{
    public Vector3 ROCK_VEL_THRESHOLD = Vector3.one / 50;
    public Vector3 ROCK_ANGVEL_THRESHOLD = Vector3.one / 50;

    public float ROCK_BALANCE_CHECK_DURATION = 2;

    public int ROCK_COUNT;
    public int CAIRN_COUNT;
}
