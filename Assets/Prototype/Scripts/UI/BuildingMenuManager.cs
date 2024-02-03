using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingMenuManager : MonoBehaviour
{

    //Keep adding...
    public GameObject[] Menus;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void BuildingMenu ()
    {
        if (BuildingManager.instance.selectedBuilding != null)
        {
            string names = BuildingManager.instance.selectedBuilding.GetComponent<Building>().buildingName;

            if (names == "Settlement")
            {
                BuildingManager.instance.building_ui = Menus[1];

            }
            else
            {
                BuildingManager.instance.building_ui = Menus[0];
            }
        }

        foreach (GameObject obj in Menus)
        {
            obj.SetActive(false);
        }
    }

}
