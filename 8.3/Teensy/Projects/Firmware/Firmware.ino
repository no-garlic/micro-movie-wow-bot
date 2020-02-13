
#include <Metro.h>

const int ledPin = 13;

Metro blinkMetro = Metro(250);

void setup()
{
    pinMode(ledPin, OUTPUT);
    digitalWrite(ledPin, HIGH);
}

void loop()
{
    if (blinkMetro.check() == 1)
    {
        updateBlink();
    }
}

void updateBlink()
{
    static int i = 0;

    switch (i)
    {
    case 0:
        digitalWrite(ledPin, HIGH);
        break;

    case 4:
        digitalWrite(ledPin, LOW); 
        break;
    }

    i = (i + 1) % 5;
}


