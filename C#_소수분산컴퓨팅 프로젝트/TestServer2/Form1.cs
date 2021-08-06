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

namespace TestServer2
{
    public partial class Form1 : Form
    {
        private TServer serverCalculate2; //계산 명령 수신용
        private TServer serverSendResult2;// 결과 송신용

        int Primerange;
        int step;
        int nprime;
        int xmax = 30000000;
        public Form1()
        {
            InitializeComponent();
            Control.CheckForIllegalCrossThreadCalls = false;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            txtMyIP.Text = TSocket.HostAddresses()[1].ToString(); //XP는 [0]
        }

        private void timmConnStatus_Tick(object sender, EventArgs e)
        {
            if (serverCalculate2 == null) { lblCalculate2.Text = "Svr1 : " + "NULL"; }
            else
            {
                csConnStatus conn = serverCalculate2.ServerStatus();
                lblCalculate2.Text = "Svr2 : " + conn.ToString();
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            if (serverCalculate2 != null) serverCalculate2.ServerClose();
            if (serverSendResult2 != null) serverSendResult2.ServerClose();
        }
        private void btnListen_Click(object sender, EventArgs e)
        {
            string myIP = txtMyIP.Text;

            if (serverCalculate2 == null) serverCalculate2 = new TServer(GetRange);
            serverCalculate2.ServerStartListen(myIP, 5003);
            if (serverSendResult2 == null) serverSendResult2 = new TServer();
            serverSendResult2.ServerStartListen(myIP, 5004);
        }
        //클라이언트의 Prime Input 수신
        private void GetRange()
        {
            while (true)
            {
                //범위 버퍼에 메시지 저장
                string rbuffrange = serverCalculate2.GetRcvMsg();
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
                    //Console.WriteLine(Primerange);
                    //Console.WriteLine(step);

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
            int NumStart = Primerange +2; //18000002
            int SaveNum = 0;

            for (int i = NumStart; i <= xmax; i += step)
            {
                PrimeCSharp.FindNumberOfPrimeNumber(NumStart, i, out nprime);
                NumStart = i;
                SaveNum += nprime; //직전까지의 소수값을 SaveNum에 저장
                calculating.Text = Convert.ToString(i); //계산위치
                lblNPrime.Text = Convert.ToString(SaveNum);//실시간 소수값
                Console.WriteLine("{0}:{1}", i, SaveNum);
                // x값과 pi(x)값 송신
                string result = TSocket.sSTX() + Convert.ToString(i) +
                         "," + Convert.ToString(SaveNum) + TSocket.sETX();
                serverSendResult2.ServerSend(result);
            }
            double dtime = Util.TimeInSeconds(stime);

            lbljudge.Text = "NO";

            //시간과 종료를 알리는 EXIT 값을 보낸다.
            lblTime.Text = string.Format("{0:0.00}", dtime) + " sec";
            string Cal2_time = TSocket.sSTX() + lblTime.Text +
                         "," + "EXIT" + TSocket.sETX();
            serverSendResult2.ServerSend(Cal2_time);
        }
    }
}
