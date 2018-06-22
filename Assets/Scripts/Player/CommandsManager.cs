using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommandsManager : MonoBehaviour
{
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            EventManager.Emit(Resources.Events.Patrol);
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            EventManager.Emit(Resources.Events.Scan);
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            EventManager.Emit(Resources.Events.Attack);
        }
    }
}
