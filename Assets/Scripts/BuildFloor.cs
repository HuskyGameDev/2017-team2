using System.Collections;
using UnityEngine;

public class BuildFloor : MonoBehaviour {
   
    public int floorNumber = 1;
    //Blue = healy
    //Red = HARD
    //Purple = eh
    public enum FloorColor { BLUE, RED, PURPLE }
    public FloorColor floorColor  = FloorColor.BLUE;
    public int lengthOfFloor = 5;
    public int heightOfFloor = 4;
    public int minRooms = 3;
    public int maxRooms = 6;
    public int startPosX = 0;
    public int startPosY = 0;
    
    //represents a room to be created
    public class Room {
        public bool isExit;
        public bool hasCharger;
        public bool doorNorth = false;
        public bool doorSouth = false;
        public bool doorWest = false;
        public bool doorEast = false;
        public Position pos;
        public Room(bool isExit, int posX, int posY, bool hasCharger) {
            this.isExit = isExit;
            pos = new Position(posX, posY);
            this.hasCharger = hasCharger;
        }
        public Room(bool isExit, Position pos, bool hasCharger) {
            this.isExit = isExit;
            this.pos = pos;
            this.hasCharger = hasCharger;
        }
    }
    
    public class Position {
        public int x;
        public int y;

        public Position(int posX, int posY) {
            x = posX;
            y = posY;
        }
    }

    public void buildRoom(Room room) {

    }

	// Use this for initialization
	void Start () {
        Room[,] floorMap = buildMap();
        for (int i = 0; i < lengthOfFloor; i++)
            for (int j = 0; j < heightOfFloor; j++) {
                if (floorMap[i, j] != null)
                    buildRoom(floorMap[i, j]);
            }
	}
	
    public Room[,] buildMap() {
        Room[,] floor = new Room[lengthOfFloor, heightOfFloor];
        System.Random random = new System.Random(Time.time.ToString().GetHashCode());
        Room start = new Room(false, startPosX, startPosY, false);
        floor[startPosX, startPosY] = start;
        Position currPos = start.pos;
        int numRooms = random.Next() % (maxRooms - minRooms);
        for (int i = 0; i < numRooms; i++) {
            ArrayList viablePositions = getViablePositions(currPos, floor);
            //if there are no viable positions to be found
            if (viablePositions.Count == 0) {
                floor[currPos.x, currPos.y].isExit = true;
                break;
            }
            currPos = (Position) viablePositions[random.Next() % viablePositions.Count];
            Room room = new Room(false, currPos, false);
            //Find doors
            if (currPos.x - 1 > -1) {
                if (floor[currPos.x - 1, currPos.y] != null) {
                    room.doorWest = true;
                    floor[currPos.x - 1, currPos.y].doorEast = true;
                }
            }
            if (currPos.y - 1 > -1) {
                if (floor[currPos.x, currPos.y - 1] != null) {
                    room.doorNorth = true;
                    floor[currPos.x - 1, currPos.y].doorSouth = true;
                }
            }
            if (currPos.x + 1 < lengthOfFloor) {
                if (floor[currPos.x + 1, currPos.y] != null) {
                    room.doorEast = true;
                    floor[currPos.x + 1, currPos.y].doorWest = true;
                }
            }
            if (currPos.y + 1 < heightOfFloor) {
                if (floor[currPos.x, currPos.y + 1] != null) {
                    room.doorSouth = true;
                    floor[currPos.x, currPos.y + 1].doorNorth = true;
                }
            }
            floor[currPos.x, currPos.y] = room;
        }
        //set a room to have the charger
        if (floorColor == FloorColor.BLUE) {
            while (true) {
                int x = random.Next() % lengthOfFloor;
                int y = random.Next() % heightOfFloor;
                if (floor[x, y] != null) {
                    floor[x, y].hasCharger = true;
                    break;
                }
            }
        }
        return floor;
    }

    public ArrayList getViablePositions(Position currPos, Room[,] floor) {
        ArrayList viablePositions = new ArrayList();
        if (currPos.x - 1 > -1) {
            if (floor[currPos.x - 1, currPos.y] != null)
                viablePositions.Add(new Position(currPos.x - 1, currPos.y));
        }
        if (currPos.y - 1 > -1) {
            if (floor[currPos.x, currPos.y - 1] != null)
                viablePositions.Add(new Position(currPos.x, currPos.y - 1));
        }
        if (currPos.x + 1 < lengthOfFloor) {
            if (floor[currPos.x + 1, currPos.y] != null)
                viablePositions.Add(new Position(currPos.x + 1, currPos.y));
        }
        if (currPos.y + 1 < heightOfFloor) {
            if (floor[currPos.x, currPos.y + 1] != null)
                viablePositions.Add(new Position(currPos.x, currPos.y + 1));
        }
        return viablePositions;
    }
	// Update is called once per frame
	void Update () {
		
	}
}
