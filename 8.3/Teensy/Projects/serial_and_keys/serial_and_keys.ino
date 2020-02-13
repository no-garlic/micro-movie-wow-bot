
const int ledPin = 13;

void setup()
{
    pinMode(ledPin, OUTPUT);
    Serial.begin(9600);
}

struct SerialData
{
    byte message;
    byte key;
    byte modifiers;
    byte handshake;
    int  duration;
};

void loop()
{
    if (Serial.available() >= 8) 
    {
        SerialData serialData;
        char *buffer = (char *) &(serialData);

        Serial.readBytes(buffer, 8);

        if (serialData.message == 1)
        {
          Keyboard.set_key1(serialData.key);
          Keyboard.set_modifier(serialData.modifiers);
          Keyboard.send_now();
  
          delay(serialData.duration);
  
          Keyboard.set_key1(0);
          Keyboard.set_modifier(0);
          Keyboard.send_now();
  
          Serial.write(serialData.handshake);
        }
    }

    digitalWrite(ledPin, HIGH);
    delay(50);
    digitalWrite(ledPin, LOW);
    delay(50);
    
}
