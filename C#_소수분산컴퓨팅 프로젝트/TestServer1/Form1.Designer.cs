namespace TestServer1
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
            this.txtMyIP = new System.Windows.Forms.TextBox();
            this.lblCalculate1 = new System.Windows.Forms.Label();
            this.btnClose = new System.Windows.Forms.Button();
            this.btnListen = new System.Windows.Forms.Button();
            this.timmConnStatus = new System.Windows.Forms.Timer(this.components);
            this.txtNMax = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.lblNPrime = new System.Windows.Forms.Label();
            this.calculating = new System.Windows.Forms.Label();
            this.lbl2 = new System.Windows.Forms.Label();
            this.lbljudge = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.lblTime = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // txtMyIP
            // 
            this.txtMyIP.Location = new System.Drawing.Point(12, 10);
            this.txtMyIP.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtMyIP.Name = "txtMyIP";
            this.txtMyIP.Size = new System.Drawing.Size(135, 25);
            this.txtMyIP.TabIndex = 90;
            this.txtMyIP.Text = "127.0.0.1";
            // 
            // lblCalculate1
            // 
            this.lblCalculate1.AutoSize = true;
            this.lblCalculate1.BackColor = System.Drawing.SystemColors.Control;
            this.lblCalculate1.Location = new System.Drawing.Point(13, 44);
            this.lblCalculate1.Name = "lblCalculate1";
            this.lblCalculate1.Size = new System.Drawing.Size(71, 15);
            this.lblCalculate1.TabIndex = 96;
            this.lblCalculate1.Text = "lblServer1";
            // 
            // btnClose
            // 
            this.btnClose.Location = new System.Drawing.Point(173, 146);
            this.btnClose.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(122, 42);
            this.btnClose.TabIndex = 94;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click_1);
            // 
            // btnListen
            // 
            this.btnListen.Location = new System.Drawing.Point(164, 10);
            this.btnListen.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnListen.Name = "btnListen";
            this.btnListen.Size = new System.Drawing.Size(131, 49);
            this.btnListen.TabIndex = 93;
            this.btnListen.Text = "Listen";
            this.btnListen.UseVisualStyleBackColor = true;
            this.btnListen.Click += new System.EventHandler(this.btnListen_Click_1);
            // 
            // timmConnStatus
            // 
            this.timmConnStatus.Enabled = true;
            this.timmConnStatus.Tick += new System.EventHandler(this.timmConnStatus_Tick_1);
            // 
            // txtNMax
            // 
            this.txtNMax.Location = new System.Drawing.Point(156, 67);
            this.txtNMax.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtNMax.Name = "txtNMax";
            this.txtNMax.Size = new System.Drawing.Size(137, 25);
            this.txtNMax.TabIndex = 99;
            this.txtNMax.Text = "Input";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 150);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(87, 15);
            this.label1.TabIndex = 101;
            this.label1.Text = "소수의 개수";
            // 
            // lblNPrime
            // 
            this.lblNPrime.AutoSize = true;
            this.lblNPrime.Location = new System.Drawing.Point(100, 151);
            this.lblNPrime.Name = "lblNPrime";
            this.lblNPrime.Size = new System.Drawing.Size(15, 15);
            this.lblNPrime.TabIndex = 100;
            this.lblNPrime.Text = "0";
            // 
            // calculating
            // 
            this.calculating.AutoSize = true;
            this.calculating.Location = new System.Drawing.Point(188, 125);
            this.calculating.Name = "calculating";
            this.calculating.Size = new System.Drawing.Size(15, 15);
            this.calculating.TabIndex = 108;
            this.calculating.Text = "0";
            // 
            // lbl2
            // 
            this.lbl2.AutoSize = true;
            this.lbl2.BackColor = System.Drawing.SystemColors.Control;
            this.lbl2.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lbl2.Location = new System.Drawing.Point(11, 125);
            this.lbl2.Name = "lbl2";
            this.lbl2.Size = new System.Drawing.Size(144, 15);
            this.lbl2.TabIndex = 109;
            this.lbl2.Text = "Calculating Num : ";
            // 
            // lbljudge
            // 
            this.lbljudge.AutoSize = true;
            this.lbljudge.Location = new System.Drawing.Point(88, 100);
            this.lbljudge.Name = "lbljudge";
            this.lbljudge.Size = new System.Drawing.Size(28, 15);
            this.lbljudge.TabIndex = 110;
            this.lbljudge.Text = "NO";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.BackColor = System.Drawing.SystemColors.Control;
            this.label3.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label3.Location = new System.Drawing.Point(11, 100);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(71, 15);
            this.label3.TabIndex = 111;
            this.label3.Text = "Cal-ing?";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("굴림", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label8.Location = new System.Drawing.Point(11, 68);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(129, 20);
            this.label8.TabIndex = 127;
            this.label8.Text = "Pirme Input :";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(13, 171);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(87, 15);
            this.label2.TabIndex = 102;
            this.label2.Text = "소요된 시간";
            // 
            // lblTime
            // 
            this.lblTime.AutoSize = true;
            this.lblTime.Location = new System.Drawing.Point(100, 171);
            this.lblTime.Name = "lblTime";
            this.lblTime.Size = new System.Drawing.Size(15, 15);
            this.lblTime.TabIndex = 103;
            this.lblTime.Text = "0";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(304, 196);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.lbljudge);
            this.Controls.Add(this.lbl2);
            this.Controls.Add(this.calculating);
            this.Controls.Add(this.lblTime);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.lblNPrime);
            this.Controls.Add(this.txtNMax);
            this.Controls.Add(this.lblCalculate1);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.btnListen);
            this.Controls.Add(this.txtMyIP);
            this.Name = "Form1";
            this.Text = "Server1";
            this.Load += new System.EventHandler(this.Form1_Load_1);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.TextBox txtMyIP;
        private System.Windows.Forms.Label lblCalculate1;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Button btnListen;
        private System.Windows.Forms.Timer timmConnStatus;
        private System.Windows.Forms.TextBox txtNMax;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label lblNPrime;
        private System.Windows.Forms.Label calculating;
        private System.Windows.Forms.Label lbl2;
        private System.Windows.Forms.Label lbljudge;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label lblTime;
    }
}

