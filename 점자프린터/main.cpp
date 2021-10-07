#include "mbed.h"

extern class Serial pc;
extern class InterruptIn YLimit;
extern volatile uint8_t buffer[25];
extern volatile uint8_t Data[25];
extern volatile uint8_t CMD;
extern volatile uint8_t LEN ;
extern volatile uint8_t ReceivedCS;
extern volatile int BIndex;
extern volatile bool CheckFlag;
extern volatile bool SaveFlag;
extern volatile bool EchoFlag;
extern volatile bool PrintFlag;
extern void Echo(); // do echo after serial
extern void Print(); // 1Line Letters (3 lines of braille)
extern void Preset(); // move to the origin 
extern void XReset(); // move to X axis' origin
extern void YReset(); // move to Y axis' origin
extern void save_data();
extern void RX_ISR();
int main()
{   pc.baud(115200);
    pc.attach(&RX_ISR);
    DigitalOut VCC(A0);

    VCC= 1;
    Preset();
    //XReset();
    //YReset();
    while(true) {
        if(SaveFlag) save_data();
        if(EchoFlag) Echo();
        wait_us(100);
        if(PrintFlag) Print();

    }
}
