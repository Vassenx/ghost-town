using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Tilemaps;

public class gridSelection : MonoBehaviour
{
    public Tilemap bigTile;
    public Tilemap smolTile;

    public ghostBuilder build;
    public GameObject indicator;
    public GameObject selectObj;

    private Vector2 selectedPosition;
    private Vector2 mousePosition;
    private int mapSize;

    private GameObject buildObj;
    private tileType buildType;

    private indicatorColor indicColor;
    public iconSelector icon;

    bool buildGhost;
    int ghostHold = 0;
    int ruinsHold = 0;

    private List<GameObject> selectors = new List<GameObject>();

    //bool selectShow;

    void Start()
    {
        setBuild();
        mapSize = build.mapSize;
        indicColor = indicator.GetComponent<indicatorColor>();
        buildGhost = false;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            if (!buildGhost)
            {
                buildGhost = true;
            }
            else
            {
                buildGhost = false;
            }

            setBuild();
        }

        if (Input.GetKeyDown(KeyCode.Q))
        {
            if (buildGhost)
            {
                ghostHold--;
                if (ghostHold < 0)
                {
                    ghostHold = build.ghostToBuild.Count - 1;
                }
            }
            else
            {
                ruinsHold--;
                if (ruinsHold < 0)
                {
                    ruinsHold = build.ruinsToBuild.Count - 1;
                }
            }

            setBuild();
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            if (buildGhost)
            {
                ghostHold++;
                if (ghostHold >= build.ghostToBuild.Count)
                {
                    ghostHold = 0;
                }
            }
            else
            {
                ruinsHold++;
                if (ruinsHold >= build.ruinsToBuild.Count)
                {
                    ruinsHold = 0;
                }
            }

            setBuild();
        }



        mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePosition = new Vector2(mousePosition.x, mousePosition.y);

        if (checkCellBounds(bigTile, mousePosition, mapSize))
        {
            if (!indicator.activeInHierarchy)
            {
                indicator.SetActive(true);
                Cursor.visible = false;
            }

            indicator.transform.position = findCellCenter(bigTile);
            recolorSelector(bigTile.WorldToCell(mousePosition));

            if (Input.GetMouseButtonDown(0))
            {
                selectedPosition = findCellCenter(bigTile);

                //setBuild();

                //Debug.Log("Cell Clicked at : " + bigTile.WorldToCell(selectedPosition));
                //Debug.Log("Position Clicked at : " + selectedPosition);

                if (buildGhost)
                {
                    build.spawnGhost(buildObj.name, selectedPosition, bigTile.WorldToCell(selectedPosition), buildType.type);
                }
                else
                {
                    build.spawnRuin(buildObj.name, selectedPosition, bigTile.WorldToCell(selectedPosition), buildType.type);
                }

                setBuild();
            }
        }
        else
        {
            if (indicator.activeInHierarchy)
            {
                indicator.SetActive(false);
                Cursor.visible = true;
            }
        }
    }

    private void setBuild()
    {
        selectClear();

        if (buildGhost)
        {
            buildObj = build.ghostToBuild[ghostHold];
            buildType = buildObj.GetComponent<tileType>();
            icon.change(ghostHold, buildGhost);

            selectShow(build.findAllOfType(buildType.type));
        }
        else
        {
            buildObj = build.ruinsToBuild[ruinsHold];
            buildType = buildObj.GetComponent<tileType>();
            icon.change(ruinsHold, buildGhost);
        }
    }

    private void selectShow(List<Vector2> locationList)
    {
        foreach(Vector2 location in locationList)
        {
            GameObject newIndicator = Instantiate<GameObject>(selectObj, transform);
            newIndicator.transform.position = location;
            selectors.Add(newIndicator);
        }
    }

    private void selectClear()
    {
        foreach(GameObject indic in selectors)
        {
            GameObject.Destroy(indic);
        }
        selectors.Clear();
    }

    private void recolorSelector(Vector3Int tileIndex)
    {
        if (buildGhost)
        {
            if (build.checkRuins(tileIndex) != null && build.checkGhost(tileIndex) == null)
            {
                indicColor.change(0);
            }
            else
            {
                indicColor.change(1);
            }
        }
        else
        {
            if (build.checkRuins(tileIndex) == null)
            {
                indicColor.change(2);
            }
            else
            {
                indicColor.change(1);
            }
        }
    }

    private Vector3 findCellCenter(Tilemap searchTile)
    {
        //Debug.Log("Clicked" + inputClick);
        //Debug.Log("Clicked Cell" + searchTile.WorldToCell(inputClick));
        //Debug.Log("Clicked Cell Centered" + searchTile.CellToWorld(searchTile.WorldToCell(inputClick)));

        Vector3 returnPosition = searchTile.CellToWorld(searchTile.WorldToCell(mousePosition));
        returnPosition = new Vector3(returnPosition.x, returnPosition.y + (searchTile.cellSize.y * 0.5f), 0);

        return returnPosition;
    }

    private bool checkCellBounds(Tilemap searchTile, Vector2 checkPosition, int sizeLim)
    {
        Vector3Int checkCell = searchTile.WorldToCell(checkPosition);

        if (checkCell.x >= 0 && checkCell.x < sizeLim && checkCell.y >= 0 && checkCell.y < sizeLim)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
