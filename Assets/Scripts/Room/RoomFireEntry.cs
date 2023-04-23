using UnityEngine;
using static RoomLocations;

namespace Room
{
    public class RoomFireEntry
    {
        private RoomLocationsEnum _roomLocationsEnum;
        private GameObject _roomFireParent;
        private Vector3 _fireLocationPosition;
        
        // FIXME: better if the room controller was used in the first place...
        private RoomController _roomController;

        public RoomLocationsEnum RoomLocationsEnum => _roomLocationsEnum;
        public GameObject RoomFireParent => _roomFireParent;
        public Vector3 FireLocationPosition => _fireLocationPosition;
        public RoomController RoomController => _roomController;
        

        // public RoomFireEntry(RoomLocationsEnum roomLocationsEnum, GameObject roomFireParent, Vector3 fireLocationPosition)
        // {
        //     this.roomLocationsEnum = roomLocationsEnum;
        //     this.roomFireParent = roomFireParent;
        //     this.fireLocationPosition = fireLocationPosition;
        // }

        public RoomFireEntry(RoomLocationsEnum roomLocationsEnum, GameObject roomFireParent, 
            Vector3 fireLocationPosition, RoomController roomController)
        {
            _roomLocationsEnum = roomLocationsEnum;
            _roomFireParent = roomFireParent;
            _fireLocationPosition = fireLocationPosition;
            _roomController = roomController;
        }
    }
}