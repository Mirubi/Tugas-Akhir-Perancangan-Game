using UnityEngine;
using System.Collections.Generic;


public class RoomManager : MonoBehaviour
{
    public List<GameObject> rooms; // Drag semua Room_1, Room_2, dst ke sini

    public void ActivateRoom(GameObject activeRoom)
    {
        foreach (GameObject room in rooms)
        {
            room.SetActive(room == activeRoom);
        }
    }
    private void Start()
{
    // Di awal, aktifkan hanya Room_1
    ActivateRoom(rooms[0]); // asumsi Room_1 = rooms[0]
}
}
