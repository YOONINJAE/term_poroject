using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net;
using System.Net.Sockets;
using System.Xml.Serialization;

namespace Final_Test
{
    public partial class Form1 : Form
    {
        private TClient clientCalculate1; //서버 1에 계산 명령 보내기
        private TClient clientRcv1;//서버 1로부터 소수 값 받기
        private TClient clientCalculate2; //서버 2에 계산 명령 보내기
        private TClient clientRcv2;//서버 2로부터 소수 값 받기


        int svr1_i = 0;
        string rbuffSvr1 = "";
        private int Svr1_SaveNum;
        int[,] Index1 = new int[800, 2];//PicArea의 너비 픽셀수 배열 선언

        int svr2_i = 0;
        string rbuffSvr2 = "";
        private int Svr2_SaveNum;
        int[,] Index2 = new int[800, 2];//PicArea의 너비 픽셀수 배열 선언

        int xmin = 0, ymin = 2;
        int xmax = 30000000, ymax = 2000000;

        public Form1()
        {
            InitializeComponent();
            Control.CheckForIllegalCrossThreadCalls = false;

        }
        private void Form1_Load(object sender, EventArgs e)
        {
            txtServerIP.Text = TSocket.HostAddresses()[1].ToString();//XP는 [0]
        }
        //xpixel ,ypixel 화 함수
        private float xpixel(double xw)
        {
            return (float)(PicArea.ClientSize.Width * (xw - xmin) / (xmax - xmin));
        }
        private float ypixel(double yw)
        {
            return (float)(PicArea.ClientSize.Height * (1 - (yw - ymin) / (ymax - ymin)));
        }

        private void FindPrime_Click(object sender, EventArgs e)
        {
            Application.DoEvents();

            //서버 통신
            if (clientCalculate1 == null) return;
            if (clientCalculate2 == null) return;

            //input 과 step() server_1,2로 송신
            int input = Convert.ToInt32(InPut.Text);
            int step = xmax/ PicArea.Width;
            txtstep.Text = Convert.ToString(step);
            
            string st1 = TSocket.sSTX() + Convert.ToString(input)
                                 + "," + Convert.ToString(step) + TSocket.sETX();
            clientCalculate1.ClientSend(st1);
            clientCalculate2.ClientSend(st1);

            //x/lnx 그래프 표시
            Graphics grp = PicArea.CreateGraphics();
            lblxlnx.Text = "x/lnx";
            for (int k = 0; k <= PicArea.Width; k++)
            {
                double pic_step = xmax / PicArea.Width;
                grp.DrawEllipse(new Pen(Color.Red), xpixel(k * pic_step),
                                                    ypixel(k * pic_step / Math.Log(k * pic_step)), 1, 1);
            }

            //x 범위값 Text 표시
            lblsvr1_range.Text =  "2 ~ " + InPut.Text;
            lblsvr2_range.Text =  InPut.Text + " ~ 30000000";
        }
        

        private void timmConnStatus_Tick(object sender, EventArgs e)
        {
            //Server_1
            if (clientCalculate1 == null) { lblSvr1.Text = "Cal1 : " + "NULL"; }
            else
            {
                csConnStatus conn = clientCalculate1.ClientStatus();
                lblSvr1.Text = "Svr1 : " + conn.ToString();
                //Svr1,2 IP표시??
                IPAdd1.Text = txtServerIP.Text;

            }
            //Server_2
            if (clientCalculate2 == null) { lblSvr2.Text = "Cal2 : " + "NULL"; }
            else
            {
                csConnStatus conn = clientCalculate2.ClientStatus();
                lblSvr2.Text = "Svr2 : " + conn.ToString();
                IPAdd2.Text = txtServerIP.Text;
            }
        }

        private void btnConnect_Click(object sender, EventArgs e)
        {
            string serverIP = txtServerIP.Text;
            string clientIP = TSocket.HostAddresses()[1].ToString(); //XP는 [0]

            if (clientCalculate1 == null) clientCalculate1 = new TClient();
            clientCalculate1.ClientBeginConnect(serverIP, 5001, clientIP); 

            if (clientRcv1 == null) clientRcv1 = new TClient(Svr1_PimeNumberDataArrived);
            clientRcv1.ClientBeginConnect(serverIP, 5002, clientIP); 

            if (clientCalculate2 == null) clientCalculate2 = new TClient();
            clientCalculate2.ClientBeginConnect(serverIP, 5003, clientIP); 

            if (clientRcv2 == null) clientRcv2 = new TClient(Svr2_PimeNumberDataArrived);
            clientRcv2.ClientBeginConnect(serverIP, 5004, clientIP);
        }

        //Svr1_PrimeNumberDataArrived - server1의 x값 , pi(x), 시간 수신
        private void Svr1_PimeNumberDataArrived()
        {
            lblSvr1_Cal.Text = Convert.ToString("Calculating...");
            
            //Server1의 x값, pi(x)값 수신
            while (true)
            {
                rbuffSvr1 = clientRcv1.GetRcvMsg();
                int idx1 = rbuffSvr1.IndexOf(TSocket.sSTX());
                if (idx1 < 0) break;
                int idx2 = rbuffSvr1.IndexOf(TSocket.sETX(), idx1);

                if (idx1 >= 0 && idx2 > idx1)
                {
                    string range = rbuffSvr1.Substring(idx1 + 1, idx2 - idx1 - 1);
                    char[] sep = new char[] { ',' };
                    string[] xy = range.Split(sep);

                    //"EXIT"를 입력받으면 Server1의 계산시간 수신
                    if (xy[1] == "EXIT")
                    {
                        lblSvr1_caltime.Text = xy[0];
                        lblSvr1_Cal.Text = Convert.ToString("Complete!");
                        lblpi.Text = "pi(x)";
                        for (int k = 0; k < PicArea.Width; k++)
                        {
                            Graphics grp = PicArea.CreateGraphics();
                            grp.DrawEllipse(new Pen(Color.Blue), xpixel(Index1[k, 0]),
                                                                ypixel(Index1[k, 1]), 1, 1);
                        }
                    }
                    //배열에 저장
                    Index1[svr1_i, 0] = Convert.ToInt32(xy[0]);
                    Index1[svr1_i, 1] = Convert.ToInt32(xy[1]);

                    //Sever1의 마지막 x의 소수개수
                    Svr1_SaveNum = Index1[svr1_i, 1];

                    svr1_i++;

                    rbuffSvr1 = rbuffSvr1.Substring(idx2 + 1);
                }
                else
                    break;
            }
        }

        //Svr2_PrimeNumberDataArrived - server1의 x값 , pi(x), 시간 수신
        private void Svr2_PimeNumberDataArrived()
        {
            lblSvr2_Cal.Text = Convert.ToString("Calculating...");

            //Server2의 x값 pi(x)값 수신
            while (true)
            {
                rbuffSvr2 = clientRcv2.GetRcvMsg();
                int idx1 = rbuffSvr2.IndexOf(TSocket.sSTX());
                if (idx1 < 0) break;
                int idx2 = rbuffSvr2.IndexOf(TSocket.sETX(), idx1);

                if (idx1 >= 0 && idx2 > idx1)
                {
                    string range = rbuffSvr2.Substring(idx1 + 1, idx2 - idx1 - 1);
                    char[] sep = new char[] { ',' };
                    string[] xy = range.Split(sep);

                    //"EXIT"를 입력받으면 Server2의 계산시간 수신 및 그래프 그리기
                    if (xy[1] == "EXIT")
                    {
                        lblSvr2_caltime.Text = xy[0];
                        lblSvr2_Cal.Text = Convert.ToString("Complete!");
                        TotalNum.Text = Convert.ToString(Svr1_SaveNum + Svr2_SaveNum);
                        for (int k = 0; k < PicArea.Width; k++)
                        {
                            Graphics grp = PicArea.CreateGraphics();
                            grp.DrawEllipse(new Pen(Color.Blue), xpixel(Index2[k, 0]),
                                                                 ypixel(Index2[k, 1] + Svr1_SaveNum), 1, 1);
                        }
                        //배열 출력
                        for (int j = 0; j < PicArea.Width/2; j++) Console.WriteLine("{0}:{1}", Index1[j,0], Index1[j,1]);
                        for (int j = 0; j < PicArea.Width/2; j++) Console.WriteLine("{0}:{1}", Index2[j,0], Index2[j,1]+Svr1_SaveNum);
                    }

                    //배열에 저장
                    Index2[svr2_i, 0] = Convert.ToInt32(xy[0]);
                    Index2[svr2_i, 1] = Convert.ToInt32(xy[1]);

                    //Sever2의 마지막 x의 소수개수
                    Svr2_SaveNum = Index2[svr2_i, 1];

                    svr2_i++;

                    rbuffSvr2 = rbuffSvr2.Substring(idx2 + 1);
                }
                else
                    break;
            }
            
        }
        
        private void btnClose_Click(object sender, EventArgs e)
        {
            if (clientCalculate1 != null) clientCalculate1.ClientClose();
            if (clientCalculate2 != null) clientCalculate2.ClientClose();
        }
    }
}
