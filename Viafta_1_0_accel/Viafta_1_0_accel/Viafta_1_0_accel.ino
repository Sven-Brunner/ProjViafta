// Include the necessary libraries for the ADXL335_U acceleration sensor and the RFID-RC522 NFC module
#include <SPI.h>
#include <ADXL335.h>
#include <MFRC522.h>
#include <math.h>

// Set the pins for the ADXL335 acceleration sensor
#define ADXL335_x_pin A0
#define ADXL335_y_pin A1
#define ADXL335_z_pin A2
// Set the pins for the RFID-RC522 NFC module and the LED
#define SS_pin 10
#define RST_pin 9
#define LED_pin 13
// Create an instance of the ADXL335 acceleration sensor
ADXL335 adxl = ADXL335();
// Create an instance of the RFID-RC522 NFC module
MFRC522 mfrc522(SS_pin, RST_pin);

MFRC522::MIFARE_Key key;  
int blockNum = 2; 
byte blockData [16] = {"Hello-World12345"};
byte bufferLen = 18;
byte readBlockData[18]; 
MFRC522::StatusCode status; 
// Create variables to store the gyroscope data from the ADXL335 sensor
unsigned int x;
unsigned int y;
unsigned int z;
double x_g_value, y_g_value, z_g_value;
// Define the time interval, distance and velocity
double timeInterval = 0.01; // 10ms
double distance = 0.0;
double maxVelocity = 0.0;
//Initialation Sensors and serial Interface
void setup() {
  // Initialize serial communication for debugging purposes
  Serial.begin(9600);
  // Initialize the ADXL335_U acceleration sensor
  adxl.begin();
  // Initialize the RFID-RC522 NFC module
  SPI.begin();
  mfrc522.PCD_Init();
  // Set the LED pin as an output
  pinMode(LED_pin, OUTPUT);
}
//Start mainfunction
void loop() {
  // Read the gyroscope data from the ADXL335_U acceleration sensor
  x = analogRead(ADXL335_x_pin);
  y = analogRead(ADXL335_y_pin);
  z = analogRead(ADXL335_z_pin);
  // Print the gyroscope data to the serial monitor for debugging purposes
 /* Serial.print("X: ");
  Serial.print(x);
  Serial.print("  Y: ");
  Serial.print(y);
  Serial.print("  Z: ");
  Serial.println(z);*/
  //Calculation of the acceleration in g units on every axis
  x_g_value = ((((double)(x * 5) / 1024) - 1.65) / 0.330); // Acceleration in x-direction in g units 
  y_g_value = ((((double)(y * 5) / 1024) - 1.65) / 0.330); // Acceleration in y-direction in g units 
  z_g_value = ((((double)(z * 5) / 1024) - 1.80) / 0.330); // Acceleration in z-direction in g units 
  // Calculate the magnitude of acceleration vector
  double accMag = sqrt(pow(x_g_value, 2) + pow(y_g_value, 2) + pow( z_g_value, 2));
  // Calculate the velocity in the general direction
  double velGeneral = accMag * timeInterval;
  distance += velGeneral * timeInterval;
  //Maximum speed reached
  if(velGeneral > maxVelocity){
    maxVelocity = velGeneral;
  }
  
  /* Prepare the ksy for authentication */
  /* All keys are set to FFFFFFFFFFFFh at chip delivery from the factory */
  for (byte i = 0; i < 6; i++)
  {
    key.keyByte[i] = 0xFF;
  }
  /* Look for new cards */
  /* Reset the loop if no new card is present on RC522 Reader */
  if ( ! mfrc522.PICC_IsNewCardPresent())
  {
    return;
  }
  
  /* Select one of the cards */
  if ( ! mfrc522.PICC_ReadCardSerial()) 
  {
    return;
  }
  Serial.print("\n");
  Serial.println("**Card Detected**");
  /* Print UID of the Card */
  Serial.print(F("Card UID:"));
  for (byte i = 0; i < mfrc522.uid.size; i++)
  {
    Serial.print(mfrc522.uid.uidByte[i] < 0x10 ? " 0" : " ");
    Serial.print(mfrc522.uid.uidByte[i], HEX);
  }
  Serial.print("\n");
  /* Print type of card (for example, MIFARE 1K) */
  Serial.print(F("PICC type: "));
  MFRC522::PICC_Type piccType = mfrc522.PICC_GetType(mfrc522.uid.sak);
  Serial.println(mfrc522.PICC_GetTypeName(piccType));
         
   /* Call 'WriteDataToBlock' function, which will write data to the block */
   Serial.print("\n");
   Serial.println("Writing to Data Block...");

   byte data[] = {"v",  maxVelocity,"d" ,distance};
   WriteDataToBlock(blockNum, blockData);
   
   /* Read data from the same block */
   Serial.print("\n");
   Serial.println("Reading from Data Block...");
   ReadDataFromBlock(blockNum, readBlockData);
   /* If you want to print the full memory dump, uncomment the next line */
   //mfrc522.PICC_DumpToSerial(&(mfrc522.uid));
   
   /* Print the data read from block */
   Serial.print("\n");
   Serial.print("Data in Block:");
   Serial.print(blockNum);
   Serial.print(" --> ");
   for (int j=0 ; j<16 ; j++)
   {
     Serial.write(readBlockData[j]);
   }
   Serial.print("\n");
  
  delay(10);  // Delay for half a second before reading the gyroscope data a
}
void WriteDataToBlock(int blockNum, byte blockData[]) 
{
  /* Authenticating the desired data block for write access using Key A */
  status = mfrc522.PCD_Authenticate(MFRC522::PICC_CMD_MF_AUTH_KEY_A, blockNum, &key, &(mfrc522.uid));
  if (status != MFRC522::STATUS_OK)
  {
    Serial.print("Authentication failed for Write: ");
    Serial.println(mfrc522.GetStatusCodeName(status));
    return;
  }
  else
  {
    Serial.println("Authentication success");
  }

  
  /* Write data to the block */
  status = mfrc522.MIFARE_Write(blockNum, blockData, 16);
  if (status != MFRC522::STATUS_OK)
  {
    Serial.print("Writing to Block failed: ");
    Serial.println(mfrc522.GetStatusCodeName(status));
    return;
  }
  else
  {
    Serial.println("Data was written into Block successfully");
  }
  
}

void ReadDataFromBlock(int blockNum, byte readBlockData[]) 
{
  /* Authenticating the desired data block for Read access using Key A */
  byte status = mfrc522.PCD_Authenticate(MFRC522::PICC_CMD_MF_AUTH_KEY_A, blockNum, &key, &(mfrc522.uid));

  if (status != MFRC522::STATUS_OK)
  {
     Serial.print("Authentication failed for Read: ");
     Serial.println(mfrc522.GetStatusCodeName(status));
     return;
  }
  else
  {
    Serial.println("Authentication success");
  }

  /* Reading data from the Block */
  status = mfrc522.MIFARE_Read(blockNum, readBlockData, &bufferLen);
  if (status != MFRC522::STATUS_OK)
  {
    Serial.print("Reading failed: ");
    Serial.println(mfrc522.GetStatusCodeName(status));
    return;
  }
  else
  {
    Serial.println("Block was read successfully");  
  }
  
}
