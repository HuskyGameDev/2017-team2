using System.Collections;
using UnityEngine;
using System;
using Random = UnityEngine.Random;

public class BuildFloor : MonoBehaviour {

    public int floorNumber = 1;
    // Specific floor colors for the first 20 floors, STORY mode
    private FloorColor[] twenty = new FloorColor[] { FloorColor.BLUE, FloorColor.BLUE, FloorColor.BLUE, FloorColor.PURPLE, FloorColor.BLUE,
                                                     FloorColor.PURPLE, FloorColor.PURPLE, FloorColor.PURPLE, FloorColor.BLUE, FloorColor.RED,
                                                     FloorColor.BLUE, FloorColor.PURPLE, FloorColor.RED, FloorColor.BLUE, FloorColor.PURPLE,
                                                     FloorColor.RED, FloorColor.PURPLE, FloorColor.BLUE, FloorColor.RED, FloorColor.RED };
    /**
     * Color dictating enemy color, material color, song?, difficulty,
     *  whether or not there's a healing station
     * Blue = moderate + healing machine
     * Purple = harder
     * Red = HARD
     */
    public enum FloorColor { BLUE, RED, PURPLE, GREY }
    public enum Direction { NORTH, SOUTH, EAST, WEST }
    public FloorColor floorColor;
    public int lengthOfFloor = 7;
    public int heightOfFloor = 5;
    public int minRooms = 1;
    public int maxRooms = 35;
    public int startPosX = 4; //equal to start position of NEXT floor after build
    public int startPosY = 3; //as above
    public int numEnemies;
    /**
     * Represents a room to be created
     * doorX - position of door, -1 if none
     * pos - postion of room in floor
     * isEntrance - true if starting room for U
     */
    [Serializable]
    public class Room {
        public FloorColor color;
        public bool isEntrance = false;
        public bool isExit = false;
        public bool hasCharger;
        public bool hasKey;
        public Direction finalDoor;
        public int doorNorth = -1;
        public int doorSouth = -1;
        public int doorWest = -1;
        public int doorEast = -1;
        public Position pos;
        public Room(int posX, int posY, bool hasCharger, FloorColor color) {
            this.color = color;
            pos = new Position(posX, posY);
            this.hasCharger = hasCharger;
        }
        public Room(Position pos, bool hasCharger, FloorColor color) {
            this.color = color; 
            this.pos = pos;
            this.hasCharger = hasCharger;
        }
    }
    /**
     * Represents a position in the floor
     * x and y - coordinates
     */
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
        floorColor = floorNumber < 21 && floorNumber > 0  && !DataBetweenScenes.isEndless ? twenty[floorNumber - 1] : randomColor();
        Room[,] floor = new Room[lengthOfFloor, heightOfFloor];
        Room start = new Room(startPosX, startPosY, false, floorColor);
        start.isEntrance = true;
        floor[startPosX, startPosY] = start;
        Position currPos = start.pos;
        int numRooms = Random.Range(minRooms, maxRooms);
        for (int i = 1; i < numRooms; i++) {
            ArrayList viablePositions = getViablePositions(currPos, floor);
            //hold last position to build doors
            Position lastPos = currPos;
            currPos = (Position)viablePositions[Random.Range(0, viablePositions.Count)];
            //if new room to create
            if (floor[currPos.x, currPos.y] == null) {
                Room room = new Room(currPos, false, floorColor);
                floor[currPos.x, currPos.y] = room;
            } 
            //if room already existed
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
                    floor[currPos.x, currPos.y].doorNorth = doorNum;
                }
            }

        }
        //set a room to have the charger
        if (floorColor == FloorColor.BLUE) {
            floor = addCharger(floor);
        }

        //set a room to have the key
        if (!DataBetweenScenes.isEndless) {
            floor = addKey(floor);
        }
        //Defines the last room built as the exit
        floor[currPos.x, currPos.y].isExit = true;
        //Sets the next floors start position to the exit
        startPosX = currPos.x;
        startPosY = currPos.y;
        return floor;
    }
    /**
    * builds the layout of rooms in the final floor
    */
    public Room[,] buildFinalFloor(int roomLength) {
        floorColor = FloorColor.GREY;
        Room[,] floor = new Room[lengthOfFloor, heightOfFloor];
        Room start = new Room(startPosX, startPosY, false, floorColor);
        start.isEntrance = true;
        floor[startPosX, startPosY] = start;
        Position currPos = start.pos;
        int numRooms = Random.Range(minRooms, maxRooms);
        for (int i = 1; i < numRooms; i++) {
            ArrayList viablePositions = getViablePositions(currPos, floor);
            //hold last position to build doors
            Position lastPos = currPos;
            currPos = (Position)viablePositions[Random.Range(0, viablePositions.Count)];
            //if new room to create
            if (floor[currPos.x, currPos.y] == null) {
                Room room = new Room(currPos, false, floorColor);
                floor[currPos.x, currPos.y] = room;
            }
            //if room already existed
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
                    floor[currPos.x, currPos.y].doorNorth = doorNum;
                }
            }

        }
        //Defines the last room built as the exit 
        floor[currPos.x, currPos.y].isExit = true;
        if (currPos.x == 0 || floor[currPos.x - 1, currPos.y] == null)
            floor[currPos.x, currPos.y].finalDoor = Direction.WEST;
        // else if (floor[currPos.x + 1, currPos.y] == null
        // floor[currPos.x, currPos.y].finalDoor = Direction.EAST;
        else if (currPos.y == heightOfFloor - 1 || floor[currPos.x, currPos.y + 1] == null)
            floor[currPos.x, currPos.y].finalDoor = Direction.NORTH;
        // else if (floor[currPos.x, currPos.y - 1] == null)
        // floor[currPos.x, currPos.y].finalDoor = Direction.SOUTH;
        else { //if final room was surrounded by other rooms, remake floor 
            return buildFinalFloor(roomLength);
        }
        //Sets the next floors start position to the exit
        startPosX = currPos.x;
        startPosY = currPos.y;
        return floor;
    }
    /**
     * Returns a random floor color
     * currently returns : 3/6 blue
     *                     2/6 purple
     *                     1/6 red
     * could be tweaked to modify difficulty if desired
     */
    private FloorColor randomColor() {
        FloorColor result;
        int i = Random.Range(0, 5);
        if (i == 0)
            result = FloorColor.RED;
        else if (i <= 2)
            result = FloorColor.PURPLE;
        else
            result = FloorColor.BLUE;
        return result;
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
     * Adds a key to the floor randomly
     */
    private Room[,] addKey(Room[,] floor) {
        while (true) {
            int x = Random.Range(0, lengthOfFloor);
            int y = Random.Range(0, heightOfFloor);
            //if room exists and it's not the starting room
            if (floor[x, y] != null && x != startPosX && y != startPosY) {
                floor[x, y].hasKey = true;
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
