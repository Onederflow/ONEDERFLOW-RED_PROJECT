#include <BLEDevice.h>
#include <BLEServer.h>
#include <BLEUtils.h>
#include <BLE2902.h>

#include <iostream>
#include <string>

#include "LedControlMS.h"

#define NBR_MTX 12

LedControl lc = LedControl(27, 14, 12, NBR_MTX);

BLECharacteristic *pCharacteristic;
bool deviceConnected = false;

uint8_t value = 0;

char type_now = 'n'; 
char type_save = 'n';

std::string charact_data = "";

#define SERVICE_UUID        "4fafc201-1fb5-459e-8fcc-c5c9c331914b"
#define CHARACTERISTIC_UUID "beb5483e-36e1-4688-b7f5-ea07361b26a8"

struct matrix
{
  bool data[24][32];  
};
struct byteline
{
  bool data[8];  
};


void string_to_matrix2()
{
  byteline value;
  int y;
  int panel;  
  for(int i = 0; i < 96; i++)
  {                                                                                                                                                                                                                             
    value = bytetoline((int)charact_data[i]);
    panel = i / 8;
    y = i % 8;
    for(int x = 0; x < 8; x++)
       lc.setLed(panel, x, 7 - y, value.data[x]);
  };
}

class MyCallbackHandler: public BLECharacteristicCallbacks {
    void onWrite(BLECharacteristic* pCharacteristic) {
      charact_data = pCharacteristic->getValue();
      string_to_matrix2();
      Serial.printf("\n\nWriting...\n\n");
    }
};

class MyServerCallbacks: public BLEServerCallbacks {
    void onConnect(BLEServer* pServer) {
      deviceConnected = true;
    };
    void onDisconnect(BLEServer* pServer) {
      deviceConnected = false;
    };
};





void setup() {
  Serial.begin(115200);
  BLEDevice::init("RedPad");  // Create the BLE Device
  BLEServer *pServer = BLEDevice::createServer();  // Create the BLE Server
  pServer->setCallbacks(new MyServerCallbacks());
  
  BLEService *pService = pServer->createService(SERVICE_UUID);  // Create the BLE Service
  // Create a BLE Characteristic
  pCharacteristic = pService->createCharacteristic(CHARACTERISTIC_UUID,BLECharacteristic::PROPERTY_READ   |BLECharacteristic::PROPERTY_WRITE  |BLECharacteristic::PROPERTY_NOTIFY |BLECharacteristic::PROPERTY_INDICATE);
  pCharacteristic->addDescriptor(new BLE2902());
  pCharacteristic->setCallbacks(new MyCallbackHandler());
  
  // Start the service
  pService->start();

  // Start advertising
  pServer->getAdvertising()->start();
  Serial.println("Waiting a client connection to notify...");
  pCharacteristic->setValue("-1");


  for (int i = 0; i < NBR_MTX; i++) {
    lc.shutdown(i, false);
    /* Set the brightness to a medium values */
    lc.setIntensity(i, 8);
    /* and clear the display */
    lc.clearDisplay(i);
  }
  lc.setLed(0, 0, 0, true);
  lc.setLed(0, 7, 0, true);
  lc.setLed(0, 0, 7, true);
  lc.setLed(0, 7, 7, true);

  lc.setLed(1, 0, 0, true);
  lc.setLed(1, 7, 0, true);
  lc.setLed(1, 0, 7, true);
  lc.setLed(1, 7, 7, true);

  lc.setLed(2, 0, 0, true);
  lc.setLed(2, 7, 0, true);
  lc.setLed(2, 0, 7, true);
  lc.setLed(2, 7, 7, true);

  lc.setLed(3, 0, 0, true);
  lc.setLed(3, 7, 0, true);
  lc.setLed(3, 0, 7, true);
  lc.setLed(3, 7, 7, true);
}

matrix mymatrix; 

void loop() {
  delay(1500);
}

void write_my_data()
{
  lc.clearDisplay(0);
  lc.clearDisplay(1);
  lc.clearDisplay(2);
  lc.clearDisplay(3);
 
    for (int i = 0; i < 24; i++)
      for (int j = 0; j < 32 ; j++)
       matrix_setLed(i,j);
}

void matrix_setLed(int i, int j)
{
  int square_y = j / 8;
  int square_x = i / 8;
  int new_x = 7 - (i - square_x * 8);
  int new_y = 7 - (j - square_y * 8);
  
   lc.setLed(square_x * 4 + square_y, new_x, new_y, mymatrix.data[i][j]);
}

byteline bytetoline(int data)
{
  byteline res;
  int temp = data;
    for(int i = 7; i >= 0; i--)
    {
      if(temp/(int)pow(2,i) == 1)
      {
        res.data[i] = true;
        temp = temp - pow(2,i);
      }
      else
        res.data[i] = false;
    };
  return res;
}



