using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EasyUI.PickerWheelUI;
public class GameManager : MonoBehaviour
{
    public PickerWheel pickerWheelPrefab;
    public Player player;

    // Start is called before the first frame update
    void Start()
    {
        //GeneratePickerWheel();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public PickerWheel GeneratePickerWheel()
    {
        PickerWheel newWheel = Instantiate(pickerWheelPrefab) as PickerWheel;
        newWheel.transform.SetParent(GameObject.FindGameObjectWithTag("Canvas").transform);
        newWheel.GetComponent<RectTransform>().anchoredPosition = new Vector3(0, 2, 0);
        return newWheel;
    }
}
