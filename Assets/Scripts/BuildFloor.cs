using System.Collections;
using UnityEngine;
using System;
using Random = UnityEngine.Random;

public class BuildFloor : MonoBehaviour {

    public int floorNumber = 1;
    //Blue = healy
    //Red = HARD
    //Purple = eh
    public enum FloorColor { BLUE, RED, PURPLE }
    public FloorColor floorColor = FloorColor.BLUE;
    public int lengthOfFloor = 7;
    public int heightOfFloor = 5;
    public int minRooms = 3;
    public int maxRooms = 6;
    public int startPosX = 4;
    public int startPosY = 3;
    //represents a room to be created
    [Serializable]
    public class Room {
        public FloorColor color;
        public bool isExit;
        public bool hasCharger;
        public bool doorNorth = false;
        public bool doorSouth = false;
        public bool doorWest = false;
        public bool doorEast = false;
        public Position pos;
        public Room(bool isExit, int posX, int posY, bool hasCharger, FloorColor color) {
            this.color = color;
            this.isExit = isExit;
            pos = new Position(posX, posY);
            this.hasCharger = hasCharger;
        }
        public Room(bool isExit, Position pos, bool hasCharger, FloorColor color) {
            this.color = color; 
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
    /**
    * builds the layout of rooms in the floor
    */
    public Room[,] buildFloor() {
        Room[,] floor = new Room[lengthOfFloor, heightOfFloor];
        Room start = new Room(false, startPosX, startPosY, false, floorColor);
        floor[startPosX, startPosY] = start;
        Position currPos = start.pos;
        int numRooms = Random.Range(minRooms, maxRooms);
        for (int i = 0; i < numRooms; i++) {
            ArrayList viablePositions = getViablePositions(currPos, floor);
            //if there are no viable positions to be found
            if (viablePositions.Count == 0) {
                break;
            }
            Position lastPos = currPos;
            currPos = (Position)viablePositions[Random.Range(0, viablePositions.Count)];
            Room room = new Room(false, currPos, false, floorColor);
            //sets a door between rooms
            if (currPos.x != lastPos.x) {
                if (currPos.x > lastPos.x) {
                    floor[lastPos.x, lastPos.y].doorEast = true;
                    room.doorWest = true;
                }
                if (currPos.x < lastPos.x) {
                    floor[lastPos.x, lastPos.y].doorWest = true;
                    room.doorEast = true;
                }
            }
            if (currPos.y != lastPos.y) {
                if (currPos.y > lastPos.y) {
                    floor[lastPos.x, lastPos.y].doorNorth = true;
                    room.doorSouth = true;
                }
                if (currPos.y < lastPos.y) {
                    floor[lastPos.x, lastPos.y].doorSouth = true;
                    room.doorSouth = true;
                }
            }
            floor[currPos.x, currPos.y] = room;
        }
        //set a room to have the charger
        if (floorColor == FloorColor.BLUE) {
            floor = addCharger(floor);
        }
        //Defines the last room built as the exit 
        floor[currPos.x, currPos.y].isExit = true;
        //Sets the next floors start position to the exit
        startPosX = currPos.x;
        startPosY = currPos.y;
        return floor;
    }
    /**
     * Adds a charger to the floor randomly
     */
    private Room[,] addCharger(Room[,] floor) {
        while (true) {
            int x = Random.Range(0, lengthOfFloor);
            int y = Random.Range(0, heightOfFloor);
            //if room exists and it's not the starting room
            if (floor[x, y] != null && x != startPosX && y != startPosY) {
                floor[x, y].hasCharger = true;
                return floor;
            }
        }
    }
    /**
     * returns an arraylist of the viable positions for a new room to be
     *  spawned in off of the current position
     * currPos - the current position in the floor
     * floor - the current layout of the floor
     */
    public ArrayList getViablePositions(Position currPos, Room[,] floor) {
        ArrayList viablePositions = new ArrayList();
        if (currPos.x - 1 > -1) {
            if (floor[currPos.x - 1, currPos.y] == null)
                viablePositions.Add(new Position(currPos.x - 1, currPos.y));
        }
        if (currPos.y - 1 > -1) {
            if (floor[currPos.x, currPos.y - 1] == null)
                viablePositions.Add(new Position(currPos.x, currPos.y - 1));
        }
        if (currPos.x + 1 < lengthOfFloor) {
            if (floor[currPos.x + 1, currPos.y] == null)
                viablePositions.Add(new Position(currPos.x + 1, currPos.y));
        }
        if (currPos.y + 1 < heightOfFloor) {
            if (floor[currPos.x, currPos.y + 1] == null)
                viablePositions.Add(new Position(currPos.x, currPos.y + 1));
        }
        return viablePositions;
    }
}
