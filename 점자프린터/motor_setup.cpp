#include "mbed.h"
#include "motor.h"
#define EM 0x19

uint8_t YLine = 1; // Line 갯수 count
void Complete();
void Preset();
void XReset();
void YReset();

extern class Serial pc;
extern volatile uint8_t buffer[25];
extern volatile uint8_t LEN;
extern volatile uint8_t Data[25];
extern volatile bool CheckFlag;
extern volatile bool AbortFlag;
extern volatile bool PrintFlag;
extern void Echo();
extern void Flush_array(volatile uint8_t *);


DRV8825 Stepper1(D4,D3);
DRV8825 Stepper2(D7,D6);
DigitalOut led(D11);
InterruptIn XLimit(D5); // Normally closed Limit switch


void Act() // Actuating
{
    wait_us(10000);
    led=1;
    wait_us(80000);
    led=0;
    wait_us(100000);
}

void Print()
{

    uint8_t cnt = 0 ;  // 점 갯수 count
    int sign = 1;
    int k = 0;
    int j=0;

    for(j=0; j<14; j++) {
        sign = 1-2*((YLine+1)%2); // even : 역방향 odd : 정방향
        switch((YLine%6)) { // if reverse dir. , read Data 4bytes reversely
            case 2:
                k = -j+11;
                break;
            case 4:
                k = -j+3;
                break;
            case 0:
                k = -j+19;
                break;
            default: // right Dir., Read Data 순서대로
                k = j;
        }
        for(int i=0; i<8; i++) {
            if(AbortFlag) { // when Abort protocol was Received, Stop printing
                XReset();
                YReset();
                Preset();
                PrintFlag=false;
                Flush_array(Data);
                return;
            }
            if(sign==1) {
                bool dot = (((*(Data+k)<<i)/128)%2==1); // scan Byte from LEFT to right, (ODD)
                if(dot) Act();
            } else {
                bool dot = ((*(Data+k)>>i)%2==1); // scan Byte from Right to Left, (Even)
                if(dot) Act();
            }
            bool NextLine = ((j==3)||(j==7)||(j==11))&&(i==7);
            if(NextLine) {
                Stepper2.Move(1.2); YLine++;
            } else {
                Stepper1.Move((sign*2.5));
                if((cnt%2)==1) Stepper1.Move((sign*1.5));
            }
            cnt++;
        }

    }
    Stepper2.Move(1.2);
    Complete();
}

void Complete()
{
    pc.putc(EM);
    Flush_array(Data);
    PrintFlag = false;
    wait_us(100);

}
void Preset()
{
    XReset();
    Stepper1.Move(1);
}

void XReset()
{
    Stepper1.Move(1);
    while(XLimit.read()) {
        Stepper1.Move(-0.1);
        wait_us(3500);
    }
    while(!XLimit.read()) {
        Stepper1.Move(0.1);
        wait_us(30000);
    }
}

void YReset()
{
    Stepper2.Move(100);
}
