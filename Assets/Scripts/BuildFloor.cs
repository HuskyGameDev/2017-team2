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
    public int minRooms = 0;
    public int maxRooms = 35;
    public int startPosX = 4;
    public int startPosY = 3;
    //represents a room to be created
    [Serializable]
    public class Room {
        public FloorColor color;
        public bool isExit;
        public bool hasCharger;
        public int doorNorth = -1;
        public int doorSouth = -1;
        public int doorWest = -1;
        public int doorEast = -1;
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
    public Room[,] buildFloor(int roomLength) {
        Room[,] floor = new Room[lengthOfFloor, heightOfFloor];
        Room start = new Room(false, startPosX, startPosY, false, floorColor);
        floor[startPosX, startPosY] = start;
        Position currPos = start.pos;
        int numRooms = Random.Range(minRooms, maxRooms);
        for (int i = 1; i < numRooms; i++) {
            ArrayList viablePositions = getViablePositions(currPos, floor);
            Position lastPos = currPos;
            currPos = (Position)viablePositions[Random.Range(0, viablePositions.Count)];
            Room room;
            if (floor[currPos.x, currPos.y] == null) {
                room = new Room(false, currPos, false, floorColor);
                floor[currPos.x, currPos.y] = room;
            } 
            else {
                i--;
            }
            int doorNum = Random.Range(0, roomLength);
            //sets a door between rooms at a position
            if (currPos.x != lastPos.x) {
                if (currPos.x > lastPos.x) {
                    floor[lastPos.x, lastPos.y].doorEast = doorNum;
                    floor[currPos.x, currPos.y].doorWest = doorNum;
                }
                if (currPos.x < lastPos.x) {
                    floor[lastPos.x, lastPos.y].doorWest = doorNum;
                    floor[currPos.x, currPos.y].doorEast = doorNum;
                }
            }
            if (currPos.y != lastPos.y) {
                if (currPos.y > lastPos.y) {
                    floor[lastPos.x, lastPos.y].doorNorth = doorNum;
                    floor[currPos.x, currPos.y].doorSouth = doorNum;
                }
                if (currPos.y < lastPos.y) {
                    floor[lastPos.x, lastPos.y].doorSouth = doorNum;
                    floor[currPos.x, currPos.y].doorSouth = doorNum;
                }
            }

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
             viablePositions.Add(new Position(currPos.x - 1, currPos.y));
        }
        if (currPos.y - 1 > -1) {
             viablePositions.Add(new Position(currPos.x, currPos.y - 1));
        }
        if (currPos.x + 1 < lengthOfFloor) {
             viablePositions.Add(new Position(currPos.x + 1, currPos.y));
        }
        if (currPos.y + 1 < heightOfFloor) {
             viablePositions.Add(new Position(currPos.x, currPos.y + 1));
        }
        return viablePositions;
    }
}
