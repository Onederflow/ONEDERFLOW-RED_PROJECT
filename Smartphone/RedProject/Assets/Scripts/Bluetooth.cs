using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections.Generic;
using System.Text;

public class Bluetooth : MonoBehaviour
{
	public Transform PanelScrollContents;
	public GameObject ButtonPrefab;
	public Text TextScanButton;

	private bool _scanning = false;

    public Text Name_n;
    public Text Address_n;

    private bool connecting_n = false;
    private string connectedID_n = null;
    private string serviceUUID_n = null;
    private string CharacteristicUUID_n = null;

    bool _connected = false;

    public GameObject go;

    public void Initialize ()
	{
		BluetoothLEHardwareInterface.Initialize (true, false, () => {
			
		}, (error) => {
		});
	}
	public void OnBack ()
	{
		RemovePeripherals ();

		if (_scanning)
			OnScan ();
		    BluetoothLEHardwareInterface.DeInitialize (() => {
		});
	}

	public void OnScan ()
	{
		if (_scanning)
		{
			BluetoothLEHardwareInterface.StopScan ();
			TextScanButton.text = "Start Scan";
			_scanning = false;
		}
		else
		{
			RemovePeripherals ();
			BluetoothLEHardwareInterface.ScanForPeripheralsWithServices (null, (address, name) => {
				AddPeripheral (name, address);

			}, (address, name, rssi, advertisingInfo) => {
			});

			TextScanButton.text = "Stop Scan";
			_scanning = true;
		}
	}

	void RemovePeripherals ()
	{
		for (int i = 0; i < PanelScrollContents.childCount; ++i)
		{
			GameObject gameObject = PanelScrollContents.GetChild (i).gameObject;
			Destroy (gameObject);
		}
	}

    void AddPeripheral(string name, string address)
    {
        GameObject peripheralObject = (GameObject)Instantiate(ButtonPrefab);
        ButtonClick script = peripheralObject.GetComponent<ButtonClick>();
        script.TextName.text = name;
        script.TextAddress.text = address;
        script.ble = this;
        peripheralObject.transform.SetParent(PanelScrollContents);
        peripheralObject.transform.localScale = new Vector3(1f, 1f, 1f);
    }



    bool Connected
    {
        get { return _connected; }
        set
        {
            _connected = value;

            if (_connected)
            {
                go.GetComponent<Text>().text = "Disconnect";
                connecting_n = false;
            }
            else
            {
                go.GetComponent<Text>().text = "Connect";
                ledON = false;
            }
        }
    }

    public void Initialize(ButtonClick button_now)
    {
        Connected = false;
        Name_n.text = button_now.TextName.text;
        Address_n.text = button_now.TextAddress.text;
    }

    void disconnect(Action<string> action)
    {
        BluetoothLEHardwareInterface.DisconnectPeripheral(Address_n.text, action);
    }

    public void OnConnect()
    {
        if (!connecting_n)
        {
            if (Connected)
            {
                disconnect((Address) =>
                {
                    Connected = false;
                });
            }
            else
            {
                BluetoothLEHardwareInterface.ConnectToPeripheral(Address_n.text, (address) =>
                {
                },
                (address, serviceUUID) =>
                {
                },
                (address, serviceUUID, characteristicUUID) =>
                {
                   // info.text += "";
                    //info.text += " " + address + " " + serviceUUID + " " + characteristicUUID;

                    // discovered characteristic
                    if (IsEqual(serviceUUID, serviceUUID))
                    {
                        connectedID_n = address;
                        serviceUUID_n = serviceUUID;
                        CharacteristicUUID_n = characteristicUUID;

                        Connected = true;

                        if (IsEqual(characteristicUUID, CharacteristicUUID_n))
                        {
                            BluetoothLEHardwareInterface.SubscribeCharacteristicWithDeviceAddress(connectedID_n, serviceUUID, CharacteristicUUID_n, (deviceAddress, notification) =>
                            {

                            }, (deviceAddress2, characteristic, data) =>
                            {

                                if (deviceAddress2.CompareTo(connectedID_n) == 0)
                                {
                                    if (IsEqual(characteristicUUID, CharacteristicUUID_n))
                                    {
         
                                    }
                                }

                            });
                        }
                        else if (IsEqual(characteristicUUID, CharacteristicUUID_n))
                        {
                        }
                    }
                }, (address) =>
                {

                    // this will get called when the device disconnects
                    // be aware that this will also get called when the disconnect
                    // is called above. both methods get call for the same action
                    // this is for backwards compatibility
                    Connected = false;
                });

                connecting_n = true;
            }
        }
    }

    private bool ledON = false;
    public void OnLED()
    {/*
		ledON = !ledON;
		if (ledON)
		{
			SendByte ((byte)0x01);
			LEDHighlight.SetActive (true);
		}
		else
		{
			SendByte ((byte)0x00);
			LEDHighlight.SetActive (false);
		}-*/


        string temp = "m";
        System.Random rnd = new System.Random();
        for (int i = 0; i < 24; i++)
            for (int j = 0; j < 32; j++)
                temp = temp + rnd.Next(0, 2).ToString() + " ";
        byte[] data = Encoding.ASCII.GetBytes(temp);
        //info.text = "Start";

        //connectedID_n = Address_n.text;
        //_serviceUUID = uuid1.text;
        //_readCharacteristicUUID = uuid2.text;
       // _writeCharacteristicUUID = uuid2.text;



        //info.text = connectedID + " / " + serviceUUID + " / " + CharacteristicUUID + " / " + data.ToString();
        SendBytes(data);
        System.Threading.Thread.Sleep(500);
        BluetoothLEHardwareInterface.ReadCharacteristic(connectedID_n, serviceUUID_n, CharacteristicUUID_n, (str, d) =>
        {
            data = d;
           // info.text = info.text + "               " + str + Encoding.ASCII.GetString(d);
        });
      //  if (info.text == "Start")
           // info.text = "End";
      //  info.text = temp;
    }


    string FullUUID(string uuid)
    {
        return "0000" + uuid + "-0000-1000-8000-00805f9b34fb";
    }

    bool IsEqual(string uuid1, string uuid2)
    {
        if (uuid1.Length == 4)
            uuid1 = FullUUID(uuid1);
        if (uuid2.Length == 4)
            uuid2 = FullUUID(uuid2);

        return (uuid1.ToUpper().CompareTo(uuid2.ToUpper()) == 0);
    }

    public void SendByte(byte value)
    {
        byte[] data = new byte[] { value };
        BluetoothLEHardwareInterface.WriteCharacteristic(connectedID_n, serviceUUID_n, CharacteristicUUID_n, data, data.Length, true, (characteristicUUID) =>
        {
            BluetoothLEHardwareInterface.Log("Write Succeeded");
        });
    }

    public void SendBytes(byte[] data)
    {
        BluetoothLEHardwareInterface.WriteCharacteristic(connectedID_n, serviceUUID_n, CharacteristicUUID_n, data, data.Length, true, (characteristicUUID) =>
        {

         //   info.text = "ok " + info.text;
        });
    }

    protected string BytesToString(byte[] bytes)
    {
        string result = "";

        foreach (var b in bytes)
            result += b.ToString("X2");

        return result;
    }
}
