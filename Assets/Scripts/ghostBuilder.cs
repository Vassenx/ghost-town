using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ghostBuilder : MonoBehaviour
{
    public List<GameObject> ghostToBuild;
    public List<GameObject> ruinsToBuild;

    public List<List<GameObject>> ruinsGrid = new List<List<GameObject>>();
    public List<List<GameObject>> ghostGrid = new List<List<GameObject>>();

    private List<GameObject> ruinsWalls = new List<GameObject>();
    private List<GameObject> ruinsFloor = new List<GameObject>();
    private List<GameObject> ruinsFence = new List<GameObject>();

    private List<List<GameObject>> ruinsObjs = new List<List<GameObject>>();
    private List<tileType> ghostPos = new List<tileType>();

    public int mapSize = 9;

    public void Start()
    {
        for (int i = 0; i < mapSize; i++)
        {
            ruinsGrid.Add(new List<GameObject>());
            ghostGrid.Add(new List<GameObject>());
            for (int j = 0; j < mapSize; j++)
            {
                ruinsGrid[i].Add(null);
                ghostGrid[i].Add(null);
            }
        }

        ruinsObjs.Add(ruinsWalls);
        ruinsObjs.Add(ruinsFloor);
        ruinsObjs.Add(ruinsFence);
    }

    public void spawnGhost(string ghostName, Vector2 spawnPosition, Vector3Int spawnIndex, string spawnType)
    {
        if (spawnIndex.x < mapSize && spawnIndex.y < mapSize)
        {
            if (checkRuins(spawnIndex) != null)
            {
                if (checkRuins(spawnIndex).GetComponent<tileType>().type == spawnType)
                {
                    if (checkGhost(spawnIndex) == null)
                    {
                        GameObject newGhost = Instantiate<GameObject>(ghostToBuild[ghostSearch(ghostName)], transform);
                        newGhost.transform.position = spawnPosition;
                        newGhost.GetComponent<FadeController>().showGhost();
                        newGhost.GetComponent<tileType>().index = spawnIndex;

                        ghostGrid[spawnIndex.x][spawnIndex.y] = newGhost;
                        ghostPos.Add(newGhost.GetComponent<tileType>());

                        Debug.Log("Built a " + ghostName + " at " + spawnIndex);
                    }
                }
            }
        }
    }

    public void spawnRuin(string ruinsName, Vector2 spawnPosition, Vector3Int spawnIndex, string spawnType)
    {
        if (spawnIndex.x < mapSize && spawnIndex.y < mapSize)
        {
            if (checkRuins(spawnIndex) != null)
            {
                GameObject hold = ruinsGrid[spawnIndex.x][spawnIndex.y];
                ruinsObjs[typeToInt(spawnType)].Remove(hold);
                ruinsGrid[spawnIndex.x][spawnIndex.y] = null;
                GameObject.Destroy(hold);
            }

            GameObject newRuins = Instantiate<GameObject>(ruinsToBuild[ruinsSearch(ruinsName)], transform);
            newRuins.transform.position = spawnPosition;
            newRuins.GetComponent<tileType>().index = spawnIndex;
            ruinsGrid[spawnIndex.x][spawnIndex.y] = newRuins;

            Debug.Log("Built a " + ruinsName + " at " + spawnIndex);

            ruinsObjs[typeToInt(spawnType)].Add(newRuins);
        }
    }

    public GameObject checkGhost(Vector3Int checkIndex)
    {
        if (ghostGrid[checkIndex.x][checkIndex.y] != null)
        {
            //Debug.Log("Found Ghost :" + ghosts[checkIndex.x][checkIndex.y].name);
            return ghostGrid[checkIndex.x][checkIndex.y];
        }
        else
        {
            return null;
        }
    }

    public GameObject checkRuins(Vector3Int checkIndex)
    {
        if (ruinsGrid[checkIndex.x][checkIndex.y] != null)
        {
            //Debug.Log("Found Ruins :" + ruins[checkIndex.x][checkIndex.y].name);
            return ruinsGrid[checkIndex.x][checkIndex.y];
        }
        else
        {
            return null;
        }
    }

    int ghostSearch(string searchFor)
    {
        for (int i = 0; i < ghostToBuild.Count; i++)
        {
            if (ghostToBuild[i].name == searchFor)
            {
                return i;
            }
        }
        return 0;
    }

    int ruinsSearch(string searchFor)
    {
        for (int i = 0; i < ruinsToBuild.Count; i++)
        {
            if (ruinsToBuild[i].name == searchFor)
            {
                return i;
            }
        }
        return 0;
    }

    int typeToInt(string type)
    {
        if (type == "Walls")
        {
            return 0;
        }
        else if (type == "Floor")
        {
            return 1;
        }
        else if (type == "Fence")
        {
            return 2;
        }
        else
        {
            return 3;
        }
    }

    public List<Vector2> findAllOfType(string type)
    {
        List<Vector2> returnList = new List<Vector2>();

        foreach(GameObject savedObject in ruinsObjs[typeToInt(type)])
        {
            tileType checkTile = savedObject.GetComponent<tileType>();
            returnList.Add(savedObject.transform.position);

            foreach (tileType ghostPosition in ghostPos)
            {
                if (checkTile.index == ghostPosition.index)
                {
                    returnList.Remove(savedObject.transform.position);
                }
            }
        }

        return returnList;
    }
}
