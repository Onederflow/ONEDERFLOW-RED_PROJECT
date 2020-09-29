using UnityEngine;
using UnityEngine.UI;

public class ButtonClick : MonoBehaviour
{
    public Bluetooth ble;
	public Text TextName;
	public Text TextAddress;
    public GameObject go;
	public void Click ()
	{
        ble.Name_n = TextName;
        ble.Address_n = TextAddress;
        ble.go = go;
        ble.OnConnect();
    }
}
