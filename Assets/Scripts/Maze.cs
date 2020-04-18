using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Maze : MonoBehaviour
{
    [System.Serializable]
    public class Cell
    {
        public bool visited;
        public GameObject north; //1
        public GameObject south; //4
        public GameObject east; //2
        public GameObject west; //3
    }

    public GameObject wall;
    public GameObject floor;
    public GameObject room;
    public float wallLength = 1.0f;
    public int xSize = 5;
    public int ySize = 5;
    private Vector3 initialPos;
    private GameObject wallHolder;
    public Cell[] cells;
    private int currentCell;
    private int totalCells;
    private int visitedCells = 0;
    private bool startedBuilding = false;
    private int currentNeighbor;
    private List<int> lastCells;
    private int backingUp = 0;
    private int wallToBreak = 0;
    private bool isEntry = false;
    private bool isExit = false;
    Transform entryPoint;
    Transform exitPoint;


    // Start is called before the first frame update
    void Start()
    {
        createWalls();
        createCells();
        Invoke("generateCopies", .01f);
    }

    void createWalls()
    {

        wallHolder = new GameObject();
        wallHolder.name = "Maze";
     

        initialPos = new Vector3((-xSize / 2) + wallLength / 2, 0f, (-ySize / 2) + wallLength / 2);
        Vector3 myPos = initialPos;
        GameObject tempWall;
        GameObject tempFloor;
        

        //for x-axis
        for (int i = 0; i < ySize; i++)
        {
            for(int j = 0; j <= xSize; j++)
            {
                myPos = new Vector3(initialPos.x + (j * wallLength) - wallLength / 2, 0f, initialPos.z + (i * wallLength) - wallLength / 2);
                tempWall = Instantiate(wall, myPos, Quaternion.identity) as GameObject;
                tempWall.transform.parent = wallHolder.transform;
            }
        }

        //for y-axis (entry & exit should be created here)
        for (int i = 0; i <= ySize; i++)
        {
            for (int j = 0; j < xSize; j++)
            {
                myPos = new Vector3(initialPos.x + (j * wallLength), 0f, initialPos.z + (i * wallLength) - wallLength);
                tempWall = Instantiate(wall, myPos, Quaternion.Euler(0f, 90f,0f)) as GameObject;
                tempWall.transform.parent = wallHolder.transform;

                //creates an entry at very top beginning
                if (i == xSize && isEntry == false)
                {
                    isEntry = true;
                    Destroy(tempWall);
                }


                //creates an exit at the far end of the bottom
                else if (j == xSize - 1 && isExit == false)
                {
                    isExit = true;
                    Destroy(tempWall);
                }

            }
        }
        for(int i = 0; i <= ySize - 1; i++)
        {
            for(int j = 0; j < xSize; j++)
            {
                myPos = new Vector3(initialPos.x + (j * wallLength), -0.5f, initialPos.z + (i * wallLength) - wallLength / 2);
                tempFloor = Instantiate(floor, myPos, Quaternion.identity) as GameObject;
                tempFloor.transform.parent = wallHolder.transform;
            }
        }
        
    }

    void createCells()
    {
        lastCells = new List<int>();
        lastCells.Clear();
        totalCells = xSize * ySize;
        GameObject[] allWalls;
        int children = wallHolder.transform.childCount;
        allWalls = new GameObject[children];
        cells = new Cell[xSize * ySize];
        int eastWestProcess = 0;
        int childPorcess = 0;
        int termCount = 0;

        //get all children
        for(int i = 0; i < children; i++)
        {
            allWalls[i] = wallHolder.transform.GetChild(i).gameObject;
        }
        //assigns walls to the cells
        for (int cellprocess = 0; cellprocess < cells.Length; cellprocess++)
        {
            if (termCount == xSize)
            {
                eastWestProcess++;
                termCount = 0;
            }
            cells[cellprocess] = new Cell();
            cells[cellprocess].east = allWalls[eastWestProcess];
            cells[cellprocess].south = allWalls[childPorcess+(xSize + 1) * ySize];
            eastWestProcess++;
            
            termCount++;
            childPorcess++;
            cells[cellprocess].west = allWalls[eastWestProcess];
            cells[cellprocess].north = allWalls[(childPorcess + (xSize + 1) * ySize) + xSize - 1];

        }
        createMaze();
    }

    void createMaze()
    {
        while(visitedCells < totalCells)
        {
            if(startedBuilding)
            {
                giveMeNeighbor();
                if(cells[currentNeighbor].visited == false && cells[currentCell].visited == true)
                {
                    breakWall();
                    cells[currentNeighbor].visited = true;
                    visitedCells++;
                    lastCells.Add(currentCell);
                    currentCell = currentNeighbor;
                    if(lastCells.Count > 0)
                    {
                        backingUp = lastCells.Count - 1;
                    }
                }
            }
            else
            {
                currentCell = Random.Range(0, totalCells);
                cells[currentCell].visited = true;
                visitedCells++;
                startedBuilding = true;
            }
        }
        Debug.Log("Finished");
    }
    
    void breakWall()
    {
        switch(wallToBreak)
        {
            case 1:
                Destroy(cells[currentCell].north);
                break;
            case 2:
                Destroy(cells[currentCell].east);
                break;
            case 3:
                Destroy(cells[currentCell].west);
                break;
            case 4:
                Destroy(cells[currentCell].south);
                break;

        }
    }

    void giveMeNeighbor()
    {
        int length = 0;
        int[] neighbors = new int[4];
        int check = 0;
        int[] connectingWall = new int[4];

        
        check = ((currentCell + 1) / xSize);
        check -= 1;
        check *= xSize;
        check += xSize;

        //west
        if(currentCell + 1 < totalCells && (currentCell + 1) != check)
        {
            if(cells[currentCell + 1].visited == false)
            {
                neighbors[length] = currentCell + 1;
                connectingWall[length] = 3;
                length++;
            }
        }
        //east
        if (currentCell - 1 >= 0 && currentCell  != check)
        {
            if (cells[currentCell - 1].visited == false)
            {
                neighbors[length] = currentCell - 1;
                connectingWall[length] = 2;
                length++;
            }
        }
        //north
        if (currentCell + xSize < totalCells)
        {
            if (cells[currentCell + xSize].visited == false)
            {
                neighbors[length] = currentCell + xSize;
                connectingWall[length] = 1;
                length++;
            }
        }
        //south
        if (currentCell - xSize >= 0)
        {
            if (cells[currentCell - xSize].visited == false)
            {
                neighbors[length] = currentCell - xSize;
                connectingWall[length] = 4;
                length++;
            }
        }
        if(length != 0)
        {
            int theChosenOne = Random.Range(0, length);
            currentNeighbor = neighbors[theChosenOne];
            wallToBreak = connectingWall[theChosenOne];
        }
        else
        {
            if(backingUp > 0)
            {
                currentCell = lastCells[backingUp];
                backingUp--;
            }
        }
    }

    void generateCopies()
    {
        //makes copies of the maze for players 2-4
        Instantiate(wallHolder, new Vector3(39.01f,0f, 13.06f), Quaternion.Euler(0f,90f,0f));
        Instantiate(wallHolder, new Vector3(52.19f,0,-25.89f), Quaternion.Euler(0f,180f,0f));
        Instantiate(wallHolder, new Vector3(13.06f,0,-38.98f), Quaternion.Euler(0f,-90f,0f));

        //create the middle room
        Instantiate(room, new Vector3(-0.04f, 0f, 0.1f), Quaternion.identity);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
