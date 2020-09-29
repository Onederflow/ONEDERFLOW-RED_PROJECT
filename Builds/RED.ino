#include <BLEDevice.h>
#include <BLEServer.h>
#include <BLEUtils.h>
#include <BLE2902.h>

#include <iostream>
#include <string>
#include "LedControlMS.h"
#include "math.h"

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


class MyServerCallbacks: public BLEServerCallbacks {
    void onConnect(BLEServer* pServer) {
      deviceConnected = true;
    };
    void onDisconnect(BLEServer* pServer) {
      deviceConnected = false;
    }
};



struct matrix
{
  bool data[24][32];  
};
struct oneline
{
  bool data[8];
};

void setup() {
  Serial.begin(115200);
  BLEDevice::init("Red matrix device");  // Create the BLE Device
  BLEServer *pServer = BLEDevice::createServer();  // Create the BLE Server
  pServer->setCallbacks(new MyServerCallbacks());
  BLEService *pService = pServer->createService(SERVICE_UUID);  // Create the BLE Service
  // Create a BLE Characteristic
  pCharacteristic = pService->createCharacteristic(CHARACTERISTIC_UUID,BLECharacteristic::PROPERTY_READ   |BLECharacteristic::PROPERTY_WRITE  |BLECharacteristic::PROPERTY_NOTIFY |BLECharacteristic::PROPERTY_INDICATE);
  pCharacteristic->addDescriptor(new BLE2902());
  
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
  if (deviceConnected) {
    charact_data = pCharacteristic->getValue();
     if(charact_data[0] == 'm') 
     {
      string_to_matrix();  
      write_my_data();
     };
  }
  delay(2000);
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

oneline bytetoint(byte data)
{
  oneline res;
  for(int i = 0; i < 8; i++)
  {
     res.data[i] = data/pow(2,(8-i));
     data = data / 2;  
  };
  return res;
}

void string_to_matrix()
{
  byte data[96];
  Serial.print("start to byte"); Serial.printf("\n");
  for(int i = 0; i < 96; i++)
  {
     const char* temp = (const char*)charact_data[i+1];   
      Serial.print(i); Serial.print(" ");Serial.print(temp); Serial.print("\n");
      data[i] = atoi("54");
      Serial.print(data[i]); Serial.print("\n");

  };
    Serial.print("end to byte"); Serial.printf("\n");
  int k = 0;
  for(int i = 0; i < 24; i++)
    for(int j = 0; j < 4; j++)
    {
        Serial.print("get one"); Serial.printf("\n");
    oneline one = bytetoint(k);
      Serial.print("one ok"); Serial.printf("\n");
    for(int m = 0 ; m < 8; m++)
      mymatrix.data[i][j*8+m] = one.data[m];
      Serial.print("k++"); Serial.printf("\n");
    k++;
    
    Serial.print(mymatrix.data[i][j]); Serial.printf("_");
    Serial.printf("\n");
      
    charact_data = "n";
    pCharacteristic->setValue("ok");
    Serial.printf("\n");
    }
}

