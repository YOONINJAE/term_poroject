void setup() {
Serial3.begin(57600);
Serial.begin(57600);
Serial3.println("co1=1");
Serial3.println("co2=1");
}
volatile double v1,v2;       
void extract_velocity_Left();
void loop() {
  // put your main code here, to run repeatedly:
    String recv_str3 = "";
//Serial3.println("v1");
//    do {
//      recv_str3 += (char)Serial3.read();
//    } while (Serial3.available());
//    Serial.print(recv_str3);
//Serial3.println("v2");
//    do {
//      recv_str3 += (char)Serial3.read();
//    } while (Serial3.available());
//    Serial.print(recv_str3);

Serial3.println("mvc=100,100");
extract_velocity_Left();
delay(1000);


Serial3.println("mvc=-100,-100");
extract_velocity_Left();
delay(1000);
//Serial.println()
}
void extract_velocity_Left(){
    String recv_str3 = "";
    String str_v1, str_v2;
    bool positive;
    Serial3.println("v1");
    delay(10); ////데이터 송신후 수신 delay
    do {
      recv_str3 += (char)Serial3.read();
    } while (Serial3.available());
    Serial.println(recv_str3);
    //'=', '.' 인덱스 넘버 찾기
    int find_equal = recv_str3.indexOf('=', 0); //from 0
    int find_dot = recv_str3.indexOf('.', 0);
    int find_positive = recv_str3.indexOf('-', 0);
    //속도 값 추출
    for (int i = find_equal + 1; i < recv_str3.length(); i++) {str_v1 += recv_str3[i];}
    if (find_positive < 0) positive = true;
    else if (find_positive > 0) positive = false;
    double v1_1 = str_v1.toInt();
    double v1_2 = str_v1.substring(find_dot - 2, find_dot).toInt() / 100.;
    v2 = positive ? v1_1 + v1_2 : v1_1 - v1_2;
    //velocity_Left = v2*radius*2*3.141592/60;
  }
