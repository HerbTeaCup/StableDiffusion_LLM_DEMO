using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class APIManager : MonoBehaviour, IManager
{

    private void Awake()
    {
        ManagerRegister.AddManager<APIManager>(this);
    }
}
