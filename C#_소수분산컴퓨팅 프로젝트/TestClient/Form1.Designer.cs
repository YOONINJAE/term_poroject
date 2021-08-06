namespace Final_Test
{
    partial class Form1
    {
        /// <summary>
        /// 필수 디자이너 변수입니다.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 사용 중인 모든 리소스를 정리합니다.
        /// </summary>
        /// <param name="disposing">관리되는 리소스를 삭제해야 하면 true이고, 그렇지 않으면 false입니다.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form 디자이너에서 생성한 코드

        /// <summary>
        /// 디자이너 지원에 필요한 메서드입니다. 
        /// 이 메서드의 내용을 코드 편집기로 수정하지 마세요.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.PicArea = new System.Windows.Forms.PictureBox();
            this.FindPrime = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.txtServerIP = new System.Windows.Forms.TextBox();
            this.btnClose = new System.Windows.Forms.Button();
            this.btnConnect = new System.Windows.Forms.Button();
            this.timmConnStatus = new System.Windows.Forms.Timer(this.components);
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.InPut = new System.Windows.Forms.TextBox();
            this.IPAdd2 = new System.Windows.Forms.TextBox();
            this.IPAdd1 = new System.Windows.Forms.TextBox();
            this.lblSvr1 = new System.Windows.Forms.Label();
            this.lblSvr2 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.lblsvr1_range = new System.Windows.Forms.Label();
            this.lblSvr1_caltime = new System.Windows.Forms.Label();
            this.lblSvr2_caltime = new System.Windows.Forms.Label();
            this.lblSvr1_Cal = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.lblsvr2_range = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.lblSvr2_Cal = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.txtstep = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.label13 = new System.Windows.Forms.Label();
            this.lblpi = new System.Windows.Forms.Label();
            this.lblxlnx = new System.Windows.Forms.Label();
            this.label15 = new System.Windows.Forms.Label();
            this.TotalNum = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.PicArea)).BeginInit();
            this.SuspendLayout();
            // 
            // PicArea
            // 
            this.PicArea.BackColor = System.Drawing.Color.White;
            this.PicArea.Location = new System.Drawing.Point(345, 20);
            this.PicArea.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.PicArea.Name = "PicArea";
            this.PicArea.Size = new System.Drawing.Size(914, 500);
            this.PicArea.TabIndex = 0;
            this.PicArea.TabStop = false;
            // 
            // FindPrime
            // 
            this.FindPrime.Location = new System.Drawing.Point(11, 224);
            this.FindPrime.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.FindPrime.Name = "FindPrime";
            this.FindPrime.Size = new System.Drawing.Size(300, 41);
            this.FindPrime.TabIndex = 1;
            this.FindPrime.Text = "Calculate";
            this.FindPrime.UseVisualStyleBackColor = true;
            this.FindPrime.Click += new System.EventHandler(this.FindPrime_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.BackColor = System.Drawing.Color.White;
            this.label3.Location = new System.Drawing.Point(351, 24);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(73, 15);
            this.label3.TabIndex = 16;
            this.label3.Text = "2,000,000";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.BackColor = System.Drawing.Color.White;
            this.label4.Location = new System.Drawing.Point(1162, 492);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(81, 15);
            this.label4.TabIndex = 17;
            this.label4.Text = "30,000,000";
            // 
            // txtServerIP
            // 
            this.txtServerIP.Location = new System.Drawing.Point(11, 66);
            this.txtServerIP.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtServerIP.Name = "txtServerIP";
            this.txtServerIP.Size = new System.Drawing.Size(137, 25);
            this.txtServerIP.TabIndex = 111;
            this.txtServerIP.Text = "127.0.0.1";
            // 
            // btnClose
            // 
            this.btnClose.Location = new System.Drawing.Point(11, 469);
            this.btnClose.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(300, 51);
            this.btnClose.TabIndex = 106;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // btnConnect
            // 
            this.btnConnect.Location = new System.Drawing.Point(11, 13);
            this.btnConnect.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnConnect.Name = "btnConnect";
            this.btnConnect.Size = new System.Drawing.Size(300, 45);
            this.btnConnect.TabIndex = 105;
            this.btnConnect.Text = "Server Connection";
            this.btnConnect.UseVisualStyleBackColor = true;
            this.btnConnect.Click += new System.EventHandler(this.btnConnect_Click);
            // 
            // timmConnStatus
            // 
            this.timmConnStatus.Enabled = true;
            this.timmConnStatus.Tick += new System.EventHandler(this.timmConnStatus_Tick);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(324, 492);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(15, 15);
            this.label5.TabIndex = 115;
            this.label5.Text = "2";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(351, 522);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(15, 15);
            this.label6.TabIndex = 116;
            this.label6.Text = "0";
            // 
            // InPut
            // 
            this.InPut.Location = new System.Drawing.Point(191, 164);
            this.InPut.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.InPut.Name = "InPut";
            this.InPut.Size = new System.Drawing.Size(120, 25);
            this.InPut.TabIndex = 117;
            this.InPut.Text = "18000000";
            // 
            // IPAdd2
            // 
            this.IPAdd2.Location = new System.Drawing.Point(11, 132);
            this.IPAdd2.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.IPAdd2.Name = "IPAdd2";
            this.IPAdd2.Size = new System.Drawing.Size(137, 25);
            this.IPAdd2.TabIndex = 121;
            this.IPAdd2.Text = "Server2 IP Input";
            // 
            // IPAdd1
            // 
            this.IPAdd1.Location = new System.Drawing.Point(11, 99);
            this.IPAdd1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.IPAdd1.Name = "IPAdd1";
            this.IPAdd1.Size = new System.Drawing.Size(137, 25);
            this.IPAdd1.TabIndex = 122;
            this.IPAdd1.Text = "Server1 IP Input";
            // 
            // lblSvr1
            // 
            this.lblSvr1.AutoSize = true;
            this.lblSvr1.Location = new System.Drawing.Point(169, 105);
            this.lblSvr1.Name = "lblSvr1";
            this.lblSvr1.Size = new System.Drawing.Size(71, 15);
            this.lblSvr1.TabIndex = 123;
            this.lblSvr1.Text = "lblServer1";
            // 
            // lblSvr2
            // 
            this.lblSvr2.AutoSize = true;
            this.lblSvr2.Location = new System.Drawing.Point(169, 138);
            this.lblSvr2.Name = "lblSvr2";
            this.lblSvr2.Size = new System.Drawing.Size(71, 15);
            this.lblSvr2.TabIndex = 125;
            this.lblSvr2.Text = "lblServer2";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("굴림", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label8.Location = new System.Drawing.Point(11, 166);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(174, 20);
            this.label8.TabIndex = 126;
            this.label8.Text = "Find Pirme Input :";
            // 
            // lblsvr1_range
            // 
            this.lblsvr1_range.AutoSize = true;
            this.lblsvr1_range.Location = new System.Drawing.Point(11, 296);
            this.lblsvr1_range.Name = "lblsvr1_range";
            this.lblsvr1_range.Size = new System.Drawing.Size(30, 15);
            this.lblsvr1_range.TabIndex = 127;
            this.lblsvr1_range.Text = "Null";
            // 
            // lblSvr1_caltime
            // 
            this.lblSvr1_caltime.AutoSize = true;
            this.lblSvr1_caltime.Location = new System.Drawing.Point(13, 403);
            this.lblSvr1_caltime.Name = "lblSvr1_caltime";
            this.lblSvr1_caltime.Size = new System.Drawing.Size(15, 15);
            this.lblSvr1_caltime.TabIndex = 136;
            this.lblSvr1_caltime.Text = "0";
            // 
            // lblSvr2_caltime
            // 
            this.lblSvr2_caltime.AutoSize = true;
            this.lblSvr2_caltime.Location = new System.Drawing.Point(159, 403);
            this.lblSvr2_caltime.Name = "lblSvr2_caltime";
            this.lblSvr2_caltime.Size = new System.Drawing.Size(15, 15);
            this.lblSvr2_caltime.TabIndex = 135;
            this.lblSvr2_caltime.Text = "0";
            // 
            // lblSvr1_Cal
            // 
            this.lblSvr1_Cal.AutoSize = true;
            this.lblSvr1_Cal.Location = new System.Drawing.Point(11, 351);
            this.lblSvr1_Cal.Name = "lblSvr1_Cal";
            this.lblSvr1_Cal.Size = new System.Drawing.Size(25, 15);
            this.lblSvr1_Cal.TabIndex = 138;
            this.lblSvr1_Cal.Text = "No";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Font = new System.Drawing.Font("굴림", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label10.Location = new System.Drawing.Point(9, 271);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(95, 17);
            this.label10.TabIndex = 143;
            this.label10.Text = "Svr1 range";
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Font = new System.Drawing.Font("굴림", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label12.Location = new System.Drawing.Point(155, 271);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(95, 17);
            this.label12.TabIndex = 144;
            this.label12.Text = "Svr2 range";
            // 
            // lblsvr2_range
            // 
            this.lblsvr2_range.AutoSize = true;
            this.lblsvr2_range.Location = new System.Drawing.Point(159, 296);
            this.lblsvr2_range.Name = "lblsvr2_range";
            this.lblsvr2_range.Size = new System.Drawing.Size(30, 15);
            this.lblsvr2_range.TabIndex = 145;
            this.lblsvr2_range.Text = "Null";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("굴림", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label2.Location = new System.Drawing.Point(8, 377);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(109, 17);
            this.label2.TabIndex = 146;
            this.label2.Text = "Svr1 Caltime";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Font = new System.Drawing.Font("굴림", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label11.Location = new System.Drawing.Point(155, 377);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(109, 17);
            this.label11.TabIndex = 147;
            this.label11.Text = "Svr2 Caltime";
            // 
            // lblSvr2_Cal
            // 
            this.lblSvr2_Cal.AutoSize = true;
            this.lblSvr2_Cal.Location = new System.Drawing.Point(155, 351);
            this.lblSvr2_Cal.Name = "lblSvr2_Cal";
            this.lblSvr2_Cal.Size = new System.Drawing.Size(25, 15);
            this.lblSvr2_Cal.TabIndex = 148;
            this.lblSvr2_Cal.Text = "No";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("굴림", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label7.Location = new System.Drawing.Point(115, 194);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(70, 20);
            this.label7.TabIndex = 149;
            this.label7.Text = "Step :";
            // 
            // txtstep
            // 
            this.txtstep.Location = new System.Drawing.Point(191, 193);
            this.txtstep.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtstep.Name = "txtstep";
            this.txtstep.Size = new System.Drawing.Size(120, 25);
            this.txtstep.TabIndex = 150;
            this.txtstep.Text = "Null";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Font = new System.Drawing.Font("굴림", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label9.Location = new System.Drawing.Point(9, 324);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(117, 17);
            this.label9.TabIndex = 151;
            this.label9.Text = "Svr1 Cal-ing?";
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Font = new System.Drawing.Font("굴림", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label13.Location = new System.Drawing.Point(155, 324);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(117, 17);
            this.label13.TabIndex = 152;
            this.label13.Text = "Svr2 Cal-ing?";
            // 
            // lblpi
            // 
            this.lblpi.AutoSize = true;
            this.lblpi.BackColor = System.Drawing.Color.White;
            this.lblpi.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblpi.Location = new System.Drawing.Point(807, 203);
            this.lblpi.Name = "lblpi";
            this.lblpi.Size = new System.Drawing.Size(25, 15);
            this.lblpi.TabIndex = 153;
            this.lblpi.Text = "...";
            // 
            // lblxlnx
            // 
            this.lblxlnx.AutoSize = true;
            this.lblxlnx.BackColor = System.Drawing.Color.White;
            this.lblxlnx.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblxlnx.Location = new System.Drawing.Point(884, 284);
            this.lblxlnx.Name = "lblxlnx";
            this.lblxlnx.Size = new System.Drawing.Size(25, 15);
            this.lblxlnx.TabIndex = 154;
            this.lblxlnx.Text = "...";
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Font = new System.Drawing.Font("굴림", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label15.Location = new System.Drawing.Point(8, 433);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(180, 17);
            this.label15.TabIndex = 155;
            this.label15.Text = "Total PrimeNumber : ";
            // 
            // TotalNum
            // 
            this.TotalNum.AutoSize = true;
            this.TotalNum.Location = new System.Drawing.Point(188, 435);
            this.TotalNum.Name = "TotalNum";
            this.TotalNum.Size = new System.Drawing.Size(15, 15);
            this.TotalNum.TabIndex = 156;
            this.TotalNum.Text = "0";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1268, 540);
            this.Controls.Add(this.TotalNum);
            this.Controls.Add(this.label15);
            this.Controls.Add(this.lblxlnx);
            this.Controls.Add(this.lblpi);
            this.Controls.Add(this.label13);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.txtstep);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.lblSvr2_Cal);
            this.Controls.Add(this.label11);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.lblsvr2_range);
            this.Controls.Add(this.label12);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.lblSvr1_Cal);
            this.Controls.Add(this.lblSvr1_caltime);
            this.Controls.Add(this.lblSvr2_caltime);
            this.Controls.Add(this.lblsvr1_range);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.lblSvr2);
            this.Controls.Add(this.lblSvr1);
            this.Controls.Add(this.IPAdd1);
            this.Controls.Add(this.IPAdd2);
            this.Controls.Add(this.InPut);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.txtServerIP);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.btnConnect);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.FindPrime);
            this.Controls.Add(this.PicArea);
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.Name = "Form1";
            this.Text = "Client";
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.PicArea)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox PicArea;
        private System.Windows.Forms.Button FindPrime;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txtServerIP;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Button btnConnect;
        private System.Windows.Forms.Timer timmConnStatus;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox InPut;
        private System.Windows.Forms.TextBox IPAdd2;
        private System.Windows.Forms.TextBox IPAdd1;
        private System.Windows.Forms.Label lblSvr1;
        private System.Windows.Forms.Label lblSvr2;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label lblsvr1_range;
        private System.Windows.Forms.Label lblSvr1_caltime;
        private System.Windows.Forms.Label lblSvr2_caltime;
        private System.Windows.Forms.Label lblSvr1_Cal;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Label lblsvr2_range;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label lblSvr2_Cal;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox txtstep;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.Label lblpi;
        private System.Windows.Forms.Label lblxlnx;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.Label TotalNum;
    }
}

