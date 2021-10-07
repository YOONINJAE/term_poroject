///////////설정값//////////////////////
#define axis_radius 124
#define pulley_radius 32 //풀리의 반지름
#define MOTOR_STEP_ANGLE 1.8 //스텝각
#define MICRO_STEP_DIV 8 // 마이크로스텝 분할수
#define STEP_CYCLE 10//스텝주기, 단위 : ms (스텝모터의 속도 설정)
///////////설정값//////////////////////

#define DESIRED_STEPS_PER_SEC (1000/STEP_CYCLE) //1000ms / 20ms(1step주기) 초당 스텝수
#define MOTOR_STEPS_PER_REV 360/MOTOR_STEP_ANGLE // 모터 1회전 스텝수
#define MICRO_STEP_PER_REV (MOTOR_STEPS_PER_REV*MICRO_STEP_DIV)//1회전당 총 마이크로 스텝수 1600
#define DESIRED_MICRO_STEPS_PER_SEC (MICRO_STEP_DIV*DESIRED_STEPS_PER_SEC) // 1600 , 초당 마이크로 스텝수
#define SAMPLE_TIME (1000000L/DESIRED_MICRO_STEPS_PER_SEC) //625 , 샘플타임

class DRV8825
{
    BusOut _bus;
    Timer tmr;

private:
    void step();
    void Straight();
    void Rotation();
    int Calculation(double distance);

    int32_t _steps;
    enum {CW=0,CCW=1} _dir;
    int InputStep;
    bool flag_St;
    bool flag_Ag;
    double radian, degree;
    int micro_step;

public:
    int32_t steps; //main에서 사용할 step수
    DRV8825(PinName pin1, PinName pin2, PinName pin3, PinName pin4);
    void Move(double distance);
    void Rotate(int16_t Angle);
    int32_t getSteps(){ return _steps;};
};

////////////////////////////////////Private////////////////////////////////////
void DRV8825::Rotation()
{
    _bus= _dir ? 0b0101:0b0000; 
    wait_us(1); //minimum pulse width
    _bus= _dir ? 0b1010:0b1111;
    _steps+= _dir? -1: 1; // updating current steps
}
void DRV8825::Straight()
{
    _bus= _dir ? 0b1100:0b0110; //_dir이 1면 1100반환 , _dir이 거짓이면 0110반환
    wait_us(1); //minimum pulse width
    _bus= _dir ? 0b1001:0b0011;
    _steps+= _dir? -1: 1; // updating current steps
    
}

int DRV8825::Calculation(double distance)
{
    radian=0, degree = 0;
    radian = distance / pulley_radius; //theta값을 구한다
    degree = radian * (180/ 3.141592); //회전할 각도를 구한다
    micro_step = int(degree / (MOTOR_STEP_ANGLE / MICRO_STEP_DIV)); //돌아가야할 마이크로스텝수

    return micro_step;
}

////////////////////////////////////Public////////////////////////////////////
DRV8825::DRV8825(PinName pin1, PinName pin2, PinName pin3,PinName pin4): _bus(pin1,pin2,pin3,pin4)
{
    _dir =CW;
    _steps=0;

}
void DRV8825::Rotate(int16_t angle){
    InputStep = Calculation(axis_radius*angle*3.141592/180);
    flag_Ag = true; //회전 시작 flag

    if(InputStep>0) _dir = CW;
    else if(InputStep<0) _dir = CCW;
    tmr.start();
    while(flag_Ag) {
        if(tmr.read_us()>SAMPLE_TIME) {
            tmr.reset();
            Rotation(); //1step 이동
            if(_steps*_steps >= InputStep*InputStep) {  //특정 스텝수만큼 이동하면 stop
                flag_Ag = false;
                _steps = 0;
            }
        }
    }
    
}
void DRV8825::Move(double distance)
{
    InputStep = Calculation(distance);
    flag_St = true; //회전 시작 flag

    if(InputStep>0) _dir = CW;
    else if(InputStep<0) _dir = CCW;
    tmr.start();
    while(flag_St) {
        if(tmr.read_us()>SAMPLE_TIME) {
            tmr.reset();
            steps = getSteps();// step 추출
            Straight(); //1step 이동

            if(_steps*_steps >= InputStep*InputStep) {  //특정 스텝수만큼 이동하면 stop
                flag_St = false;
                _steps = 0;
            }
        }
    }
}
