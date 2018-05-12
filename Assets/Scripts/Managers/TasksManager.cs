using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TasksManager : MonoBehaviour
{
    /// <summary>
    /// used to emit events when the player is near an important object
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag.Equals(Resources.Tags.Player))
        {
            EventManager.Emit(Resources.Events.PlayerEnteredImportantArea);

            switch (this.tag)
            {
                // the player is in the main control room
                case Resources.Tags.MainComputer:
                    EventManager.Emit(Resources.Events.MainComputerFound);
                    break;
                // the player is in the storage room
                case Resources.Tags.StorageRoom:
                    EventManager.Emit(Resources.Events.StorageRoomFound);
                    break;
                // the player has found the spare parts for his ship
                case Resources.Tags.DarkMatterModule:
                    EventManager.Emit(Resources.Events.DarkMatterModuleFound);
                    break;
                // the player is near the ship
                case Resources.Tags.Ship:
                    EventManager.Emit(Resources.Events.SpaceshipFound);
                    break;
                default: break;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag.Equals(Resources.Tags.Player))
        {
            EventManager.Emit(Resources.Events.PlayerExitedImportantArea);
        }
    }
}
