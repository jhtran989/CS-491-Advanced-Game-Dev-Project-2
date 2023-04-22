using UnityEngine;
using static RoomLocations;

namespace Room
{
    public class RoomFireEntry
    {
        private RoomLocationsEnum roomLocationsEnum;
        private GameObject roomFireParent;
        private Vector3 fireLocationPosition;
        
        public RoomLocationsEnum RoomLocationsEnum => roomLocationsEnum;
        public GameObject RoomFireParent => roomFireParent;
        public Vector3 FireLocationPosition => fireLocationPosition;
        

        public RoomFireEntry(RoomLocationsEnum roomLocationsEnum, GameObject roomFireParent, Vector3 fireLocationPosition)
        {
            this.roomLocationsEnum = roomLocationsEnum;
            this.roomFireParent = roomFireParent;
            this.fireLocationPosition = fireLocationPosition;
        }
    }
}