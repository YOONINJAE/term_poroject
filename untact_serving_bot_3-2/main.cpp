#include "mbed.h"
#include "VL53L0X.h"
#include "DFPlayerMini.h" //slave
///////////Stepmotor Initial setting value///////////////////
#define axis_radius 121.5 // 로봇의 중심-바퀴 거리
#define pulley_radius 32 //바퀴의 반지름
#define MOTOR_STEP_ANGLE 1.8 //스텝각
#define MICRO_STEP_DIV 2 // 마이크로스텝 분할수
#define STEP_CYCLE 10//스텝주기, 단위 : ms (스텝모터의 속도 설정)클수록 느림

#define DESIRED_STEPS_PER_SEC (1000/STEP_CYCLE) // 초당 스텝수
#define MOTOR_STEPS_PER_REV 360/MOTOR_STEP_ANGLE // 모터 1회전 스텝수
#define DESIRED_MICRO_STEPS_PER_SEC (2000/STEP_CYCLE) // 초당 마이크로 스텝수(50-->10) 100-->300 정도로 설정해보자
#define SAMPLE_TIME (1000000L/DESIRED_MICRO_STEPS_PER_SEC) // 샘플타임

BusOut _bus(D10,D9,D7,D6); // StepMoter1: D10,D9  StepMoter2 : D7,D6   pulse-direction
Timer tmr; // 타이머 객체 생성

////////////////////function//////////////////////////////
void Lidar_Initial_set();//Lidar setting
void step();
void Straight();
void Straight_back();
void Move(double distance);
void Rotate(int16_t Angle);
void Rotation();
void NodeToNode(char nodeNum[], char command[], uint8_t numOfNode, uint8_t numOfCommand);
int Calculation(double distance);

//변수선언==========================================================================================================================
bool flag_St; //Straight flag
bool flag_Ag; //Rotate flag
enum {CW=0,CCW=1} _dir;
int32_t InputStep;
int32_t _steps = 0;
int32_t micro_step;
int32_t getSteps(){return _steps;};
//==================================================================================================================================//

Serial pc(USBTX,USBRX, 9600);//115200으로 변경

//MP3 객체 생성
DFPlayerMini dfplayer(PA_11,PA_12); //slave

VL53L0X sensor1(I2C_SDA, I2C_SCL,D1);

DigitalOut led(LED1);
DigitalOut x1(D0, 1);

//limit switch 객체 생성
InterruptIn lim_sw1(A1,PullUp);
InterruptIn lim_sw2(A2,PullUp);
InterruptIn lim_sw3(A3,PullUp);
InterruptIn lim_sw4(A4,PullUp);
DigitalIn line_tracer1(A0,PullUp);
DigitalIn line_tracer2(A5,PullUp);
DigitalIn line_tracer3(D3,PullUp);
DigitalIn line_tracer4(D5,PullUp);
DigitalIn dist_tracker(D11,PullUp);

void checkSwitch(int swCount);

//BlueTooth 객체 생성
Serial BTModule(D8,D2);

bool rxButton = false;
char arrayButton = 0;
char nodeNum[50];
char command[4];
void InitBTModule();
int numOfNode_tmp = 0;
int numOfCommand_tmp = 0;

void onSerialRx(){
    if (BTModule.readable())
        {  
            rxButton = true;
            if (BTModule.getc() == 's') { //start read node array
                while(1){
                    char c = BTModule.getc();
                    if (c == 'n') {
                        arrayButton = 0;
                    }
                    else if (c == 't') {
                        arrayButton = 1;
                    }
                    else if (c == 'e'){
                        break;
                    }
                    
                    if( arrayButton == 0 && c != 'n'){
                        nodeNum[numOfNode_tmp] = c;
                        numOfNode_tmp++;
                    }
                    else if( arrayButton == 1 && c != 't'){
                        command[numOfCommand_tmp] = (int)c;
                        numOfCommand_tmp++;
                    }
                }
            } 
        }
}
int main()
{
    InitBTModule();
    Lidar_Initial_set();
    uint8_t numOfNode = 0;
    uint8_t numOfCommand = 0;
    while (!BTModule.writeable()) { } //wait until the BTModule is ready
    BTModule.attach(&onSerialRx);

    dfplayer.mp3_set_volume(30); //slave-
    dfplayer.mp3_play_physical (7);
    
    while(1) {
        pc.printf("%d\n", sensor1.readRange());
        if (rxButton){  
            dfplayer.mp3_play_physical (6);
            wait(3);
            rxButton = false;
            pc.printf("Node Array\n");
            for( int i = 0; i < numOfNode_tmp ; i++){
                pc.putc(nodeNum[i]); 
            }
            pc.printf("\n");
            pc.printf("Table Array\n");
            for( int j = 0; j < numOfCommand_tmp ; j++){
                pc.putc(command[j]); 
            }
            pc.printf("\n");
            numOfNode = numOfNode_tmp;
            numOfCommand = numOfCommand_tmp;
            numOfNode_tmp = 0;
            numOfCommand_tmp = 0;

            dfplayer.mp3_play_physical (4);
            wait(3);
        }
        if(!rxButton){
            if(numOfNode > 0){
            
                NodeToNode(nodeNum, command, numOfNode, numOfCommand);
                dfplayer.mp3_play_physical (5);
                numOfNode = 0;
            }

            //배열을 이용하여 커맨드를 생성하면 됨!
            //dfplayer.mp3_play_physical (5);
        }
    }
}

void NodeToNode(char nodeNum[], char command[], uint8_t numOfNode, uint8_t numOfCommand){
    int i = 0;
    int swCount = 1;
    for ( int j = 0; j< numOfNode ; j++){

        if ((nodeNum[j+1] - nodeNum[j]) == 10){
            int c = 0;
            while((nodeNum[j+1] - nodeNum[j]) == 10){
                pc.printf("Forward\n");
                c++;
                j++;
            }
            Move(300 * c);
            wait_us(200000);
            j = j-1;           
        }
        
        else if ((nodeNum[j+1] - nodeNum[j]) == -10){
            int c = 0;
            while((nodeNum[j+1] - nodeNum[j]) == -10){
                pc.printf("Back\n");
                c++;
                j++;
            }
            Move(-300 * c);
            wait_us(200000);
            j = j-1;  
        }
        else if ((nodeNum[j+1] - nodeNum[j]) == -1){
            pc.printf("left\n");
            Rotate(-90);
            wait_us(200000);
            Move(461);
            wait_us(200000);
            Rotate(90);
            wait_us(200000);
        }
        else if ((nodeNum[j+1] - nodeNum[j]) == 1){
            pc.printf("Right\n"); 
            Rotate(90);
            wait_us(200000);
            Move(461);
            wait_us(200000);
            Rotate(-90);
            wait_us(200000);
        }
        else if ((nodeNum[j+1] - nodeNum[j]) == 2){
            pc.printf("Right x 2\n"); 
            Rotate(90);
            wait_us(200000);
            Move(923);
            wait_us(200000);
            Rotate(-90); 
            wait_us(200000);
        }
        else if ((nodeNum[j+1] - nodeNum[j]) == -2){
            pc.printf("Left x 2\n"); 
            Rotate(-90);
            wait_us(200000);
            Move(923);
            wait_us(200000);
            Rotate(90); 
            wait_us(200000);
        }
        else if ((nodeNum[j+1] - nodeNum[j]) == 0){
            if(command[i] == 2){
                pc.printf("R L Table\n");
                Rotate(-90);
                wait_us(200000);
                Move(400);
                wait_us(200000);
                checkSwitch(swCount);
                swCount++;
                Move(-800);
                checkSwitch(swCount);
                Move(400);
                wait_us(200000);
                Rotate(90);
                wait_us(200000);
                swCount++;
            }
            else if(command[i] == 1){
                pc.printf("L Table\n");
                Rotate(-90);
                wait_us(200000);
                Move(400);
                wait_us(200000);
                checkSwitch(swCount);
                Move(-400);
                wait_us(200000);
                Rotate(90);
                wait_us(200000);
                swCount++;
                
            }
            else if(command[i] == 0){  
                pc.printf("R Table\n");
                Rotate(90);
                wait_us(200000);
                Move(400);
                wait_us(200000);
                checkSwitch(swCount);
                Move(-400);
                wait_us(200000);
                Rotate(-90);
                wait_us(200000);
                swCount++;
            }
            i++;  
        } 
    }  
}

void checkSwitch(int swCount){
    dfplayer.mp3_play_physical (2);
    if( swCount == 1){
        while( lim_sw1.read() == 0 ){}
    }
    else if( swCount == 2){
        while( lim_sw2.read() == 0 ){}
    }
    else if( swCount == 3){
        while( lim_sw3.read()== 0 ){}
    }
    else if( swCount == 4){
        while( lim_sw4.read()== 0 ){}
    }
    dfplayer.mp3_play_physical (3);
    wait(2);
}

void InitBTModule(){
     while(!BTModule.writeable()) {} //wait until writeable
    pc.printf("\r\nStarting Slave Program\r\n");
    BTModule.printf("AT+RENEW\r\n");
    pc.printf("Device Reset\r\n");
    wait(2);
    BTModule.printf("AT+ROLE0\r\n"); //set chip to slave mode
    pc.printf("Device Set to Slave\r\n");
    
    while (BTModule.readable()) //flush BTModule buffer (clear characters that have built up)
        pc.putc(BTModule.getc());       
    wait(2);
    while(BTModule.getc() != '&') {} //wait for initialize character from master
    BTModule.putc('&'); //send acknowledgement back to the master
}


void Lidar_Initial_set(){

    wait_us(1e4);
    x1 = 0;
    wait_us(1e4);
    
    x1 = 1;
    
    sensor1.setAddress(0x30);
    sensor1.init();
    sensor1.setTimeout(500);  // for polling
    sensor1.setApplication(VL53L0X::LONG_FAST);
    sensor1.startContinuous();
    wait_us(2000000);
    

}

int Calculation(double distance)
{
    double radian=0, degree = 0;
    radian = distance / pulley_radius; //theta값을 구한다
    degree = radian * (180/ 3.141592); //회전할 각도를 구한다
    micro_step = int(degree / (MOTOR_STEP_ANGLE / MICRO_STEP_DIV)); //돌아가야할 마이크로스텝수

    return micro_step;
}
///////////////////////////////straight, backward moving function ////////////////////////////////
void Move(double distance)
{
    sensor1.startContinuous();
    uint16_t cnt = 3000;
    uint8_t dx = 10;
    InputStep = Calculation(distance);
    flag_St = true; //회전 시작 flag
    uint16_t ob_dist = 150; //장애물 탐지 거리

    if(InputStep>0) _dir = CW;
    else if(InputStep<0) _dir = CCW;
    tmr.start();
    bool mp3Count = 1;
    bool pre_mp3Count = 0;

    while(flag_St) {
        if(tmr.read_us()>SAMPLE_TIME) {
            tmr.reset();
            if(abs(InputStep)>500){
                if(abs(getSteps())<250){wait_us(cnt*3);cnt-=dx;}
                if(abs(getSteps())>= abs(InputStep)-250){wait_us(cnt*3);cnt+=dx;}}
            else{
                if(abs(getSteps())<abs(InputStep/2)){wait_us(cnt*3);cnt-=dx/2;}
                if(abs(getSteps())>= abs(InputStep/2)){wait_us(cnt*3);cnt+=dx/2;}
                }
           
            mp3Count = !mp3Count; 
            //if(sensor3.readRange()<ob_dist)  // 장애물 탐지시 1초간 정지
            if(InputStep>0)Straight(); //1step 이동
            else if(InputStep<0)Straight_back();

            if(abs(_steps) >= abs(InputStep)) {  
                if(dist_tracker.read()==1) _steps+= _dir? 1: -1;
                else{
                flag_St = false;
                _steps = 0;
                cnt=3000;}
            }
        }
    }
}
void Straight()
{
    if((line_tracer1.read()==1&&line_tracer2.read()==1) ||(line_tracer1.read()==0&&line_tracer2.read()==0)){
        _bus= _dir ? 0b1100:0b0110; 
        wait_us(1); //minimum pulse width
        _bus= _dir ? 0b1001:0b0011;
        }
    else if(line_tracer1.read()==1){
        _bus= _dir ? 0b1000:0b0010;
        wait_us(1); //minimum pulse width
        _bus= _dir ? 0b1001:0b0011;
        }
    else if(line_tracer2.read()==1) {
        _bus= _dir ? 0b1100:0b0110;
        wait_us(1); //minimum pulse width
        _bus= _dir ? 0b1000:0b0010;
        }

    _steps+= _dir? -1: 1; // updating current steps 
}
void Straight_back()
{
    if((line_tracer3.read()==1&&line_tracer4.read()==1) ||(line_tracer3.read()==0&&line_tracer4.read()==0)){
        _bus= _dir ? 0b1100:0b0110; 
        wait_us(1); //minimum pulse width
        _bus= _dir ? 0b1001:0b0011;
        }
    else if(line_tracer4.read()==1){
        _bus= _dir ? 0b1000:0b0010;
        wait_us(1); //minimum pulse width
        _bus= _dir ? 0b1001:0b0110;
        }
    else if(line_tracer3.read()==1) {
        _bus= _dir ? 0b1100:0b0010;
        wait_us(1); //minimum pulse width
        _bus= _dir ? 0b1000:0b0011;
        }

    _steps+= _dir? -1: 1; // updating current steps 
}
///////////////////////Rdotation function /////////////////////
void Rotate(int16_t angle){
    ///////가감속 변수///////
    uint16_t cnt = 3000;
    uint8_t dx = 10;
    if(angle>0)InputStep = Calculation(axis_radius*angle*3.141592/180);
    else if(angle<0)InputStep = Calculation(axis_radius*angle*3.141592/180)-2;
    flag_Ag = true; //회전 시작 flag

    if(InputStep>0) _dir = CW;
    else if(InputStep<0) _dir = CCW;
    tmr.start();
    while(flag_Ag) {
        if(tmr.read_us()>SAMPLE_TIME) {
            tmr.reset();
            if(abs(getSteps())<abs(InputStep/2)){wait_us(cnt*3);cnt-=dx/2;}
            if(abs(getSteps())>= abs(InputStep/2)){wait_us(cnt*3);cnt+=dx/2;}
            Rotation(); //1step 이동
            if(abs(_steps) >= abs(InputStep)) {  //특정 스텝수만큼 이동하면 stop
                flag_Ag = false;
                _steps = 0;
                cnt=3000;
            }
        }
    }
}
void Rotation()
{
    _bus= _dir ? 0b0101:0b0000; 
    wait_us(1); //minimum pulse width
    _bus= _dir ? 0b1010:0b1111;
    _steps+= _dir? -1: 1; // updating current steps
}
