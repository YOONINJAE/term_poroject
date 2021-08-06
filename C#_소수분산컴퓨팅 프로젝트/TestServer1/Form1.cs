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

namespace TestServer1
{
    public partial class Form1 : Form
    {
        private TServer serverCalculate1; //계산 명령 수신용
        private TServer serverSendResult1;// 결과 송신용

        int Primerange;
        int step;
        int nprime;
        public Form1()
        {
            InitializeComponent();
            Control.CheckForIllegalCrossThreadCalls = false;
        }

        //내 IP 주소 불러오기
        private void Form1_Load_1(object sender, EventArgs e)
        {
            txtMyIP.Text = TSocket.HostAddresses()[1].ToString();

        }

        private void timmConnStatus_Tick_1(object sender, EventArgs e)
        {
            if (serverCalculate1 == null) { lblCalculate1.Text = "Svr1 : " + "NULL"; }
            else
            {
                csConnStatus conn = serverCalculate1.ServerStatus();
                lblCalculate1.Text = "Svr1 : "+ conn.ToString();
            }
        }


        private void btnClose_Click_1(object sender, EventArgs e)
        {
            if (serverCalculate1 != null) serverCalculate1.ServerClose();
            if (serverSendResult1 != null) serverSendResult1.ServerClose();
        }
        
        private void btnListen_Click_1(object sender, EventArgs e)
        {
            string myIP = txtMyIP.Text;

            if (serverCalculate1 == null) serverCalculate1 = new TServer(GetRange);
            serverCalculate1.ServerStartListen(myIP, 5001); 
            if (serverSendResult1 == null) serverSendResult1 = new TServer();
            serverSendResult1.ServerStartListen(myIP, 5002);  

        }
        //클라이언트의 Prime Input 수신
        private void GetRange()
        {
            while (true)
            {
                //범위 버퍼에 메시지 저장
                string rbuffrange = serverCalculate1.GetRcvMsg();
                int idx1 = rbuffrange.IndexOf(TSocket.sSTX());
                if (idx1 < 0) break;
                int idx2 = rbuffrange.IndexOf(TSocket.sETX(), idx1);

                if (idx1 >= 0 && idx2 > idx1)
                {
                    string range = rbuffrange.Substring(idx1 + 1, idx2 - idx1 - 1);
                    char[] sep = new char[] { ',' };
                    string[] xy = range.Split(sep);
                    Primerange = Convert.ToInt32(xy[0]);
                    step = Convert.ToInt32(xy[1]);
                    
                    rbuffrange = rbuffrange.Substring(idx2 + 1);
                    txtNMax.Text = Convert.ToString(Primerange);
                }
                else
                    break;
            }
            //계산 실행
            Calculate(Primerange, step);
        }

        //계산 함수
        private void Calculate(int Primerange, int step)
        {
            ////소수, 시간, 계산중인지 표시
            lblNPrime.Text = "...";
            lblTime.Text = "...";
            lbljudge.Text = "Calculating...";

            Application.DoEvents();
            DateTime stime = DateTime.Now;
            
            //계산시작
            int NumStart = 2;
            int SaveNum = 0;

            for (int i = 2; i <= Primerange + 2; i += step)
            {
                PrimeCSharp.FindNumberOfPrimeNumber(NumStart, i, out nprime);
                NumStart = i;
                if (i == 2+step) SaveNum = 0;
                SaveNum += nprime; //직전까지의 소수값을 SaveNum에 저장

                calculating.Text = Convert.ToString(i); //실시간 x값
                lblNPrime.Text = Convert.ToString(SaveNum);//실시간 pi(x)값
                Console.WriteLine("{0}:{1}",i,SaveNum);//x, pi(x)출력

                // x값과 pi(x)값 송신
                string result = TSocket.sSTX() + Convert.ToString(i) +
                         "," + Convert.ToString(SaveNum) + TSocket.sETX();
                serverSendResult1.ServerSend(result);
            }
            double dtime = Util.TimeInSeconds(stime);
            
            lbljudge.Text = "NO";

            //시간과 종료를 알리는 EXIT 값을 보낸다.
            lblTime.Text = string.Format("{0:0.00}", dtime)+" sec";
            string Cal1_time = TSocket.sSTX() + lblTime.Text +
                         "," + "EXIT" + TSocket.sETX();
            serverSendResult1.ServerSend(Cal1_time);
        }

    }
}
