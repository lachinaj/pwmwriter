#include <Servo.h>

// PWM Pins: 3,5,6,9,10,11
#define PIN_A 3
#define PIN_B 5
#define PIN_C 6
#define PIN_D 9
#define PIN_E 10
#define PIN_F 11

char datas[50];
int pos = 0;

Servo servo_A;
Servo servo_B;
Servo servo_C;
Servo servo_D;
Servo servo_E;
Servo servo_F;

void setup() 
{
  Serial.begin(115200);
  
  servo_A.attach(PIN_A);
  servo_B.attach(PIN_B);
  servo_C.attach(PIN_C);
  servo_D.attach(PIN_D);
  servo_E.attach(PIN_E);
  servo_F.attach(PIN_F);
}

void loop() 
{
  int rb = Serial.read();
  if(rb != -1)
  {
    if(rb == '\n')
    {
      datas[pos]=0;
      char* val = strchr(datas, ':');
      *val = 0; ++val;
      int value = atoi(val);
      Move(datas[0], value);
      pos = 0;
    }
    else
    {
      datas[pos] = rb;
      pos++;
    }
  }
}

void Move(int Pin, int Pos)
{
  switch(Pin)
  {
    case 'A': servo_A.write(Pos); break;
    case 'B': servo_B.write(Pos); break;
    case 'C': servo_C.write(Pos); break;
    case 'D': servo_D.write(Pos); break;
    case 'E': servo_E.write(Pos); break;
    case 'F': servo_F.write(Pos); break;
  }
  Serial.print("Pin=");
  Serial.print(Pin);
  Serial.print(" ; Pos=");
  Serial.println(Pos);
}
