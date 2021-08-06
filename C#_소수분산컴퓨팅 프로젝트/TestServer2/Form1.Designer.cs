namespace TestServer2
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
            this.lbljudge = new System.Windows.Forms.Label();
            this.calculating = new System.Windows.Forms.Label();
            this.lblTime = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.lblNPrime = new System.Windows.Forms.Label();
            this.txtNMax = new System.Windows.Forms.TextBox();
            this.lblCalculate2 = new System.Windows.Forms.Label();
            this.btnClose = new System.Windows.Forms.Button();
            this.btnListen = new System.Windows.Forms.Button();
            this.txtMyIP = new System.Windows.Forms.TextBox();
            this.timmConnStatus = new System.Windows.Forms.Timer(this.components);
            this.label8 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // lbljudge
            // 
            this.lbljudge.AutoSize = true;
            this.lbljudge.Location = new System.Drawing.Point(84, 102);
            this.lbljudge.Name = "lbljudge";
            this.lbljudge.Size = new System.Drawing.Size(28, 15);
            this.lbljudge.TabIndex = 124;
            this.lbljudge.Text = "NO";
            // 
            // calculating
            // 
            this.calculating.AutoSize = true;
            this.calculating.Location = new System.Drawing.Point(182, 133);
            this.calculating.Name = "calculating";
            this.calculating.Size = new System.Drawing.Size(15, 15);
            this.calculating.TabIndex = 122;
            this.calculating.Text = "0";
            // 
            // lblTime
            // 
            this.lblTime.AutoSize = true;
            this.lblTime.Location = new System.Drawing.Point(94, 179);
            this.lblTime.Name = "lblTime";
            this.lblTime.Size = new System.Drawing.Size(15, 15);
            this.lblTime.TabIndex = 121;
            this.lblTime.Text = "0";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(7, 179);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(87, 15);
            this.label2.TabIndex = 120;
            this.label2.Text = "소요된 시간";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(7, 156);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(87, 15);
            this.label1.TabIndex = 119;
            this.label1.Text = "소수의 개수";
            // 
            // lblNPrime
            // 
            this.lblNPrime.AutoSize = true;
            this.lblNPrime.Location = new System.Drawing.Point(94, 156);
            this.lblNPrime.Name = "lblNPrime";
            this.lblNPrime.Size = new System.Drawing.Size(15, 15);
            this.lblNPrime.TabIndex = 118;
            this.lblNPrime.Text = "0";
            // 
            // txtNMax
            // 
            this.txtNMax.Location = new System.Drawing.Point(158, 63);
            this.txtNMax.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtNMax.Name = "txtNMax";
            this.txtNMax.Size = new System.Drawing.Size(137, 25);
            this.txtNMax.TabIndex = 117;
            this.txtNMax.Text = "Input";
            // 
            // lblCalculate2
            // 
            this.lblCalculate2.AutoSize = true;
            this.lblCalculate2.BackColor = System.Drawing.SystemColors.Control;
            this.lblCalculate2.Location = new System.Drawing.Point(7, 43);
            this.lblCalculate2.Name = "lblCalculate2";
            this.lblCalculate2.Size = new System.Drawing.Size(71, 15);
            this.lblCalculate2.TabIndex = 116;
            this.lblCalculate2.Text = "lblServer2";
            // 
            // btnClose
            // 
            this.btnClose.Location = new System.Drawing.Point(174, 156);
            this.btnClose.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(120, 40);
            this.btnClose.TabIndex = 115;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // btnListen
            // 
            this.btnListen.Location = new System.Drawing.Point(158, 8);
            this.btnListen.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnListen.Name = "btnListen";
            this.btnListen.Size = new System.Drawing.Size(130, 50);
            this.btnListen.TabIndex = 114;
            this.btnListen.Text = "Listen";
            this.btnListen.UseVisualStyleBackColor = true;
            this.btnListen.Click += new System.EventHandler(this.btnListen_Click);
            // 
            // txtMyIP
            // 
            this.txtMyIP.Location = new System.Drawing.Point(10, 9);
            this.txtMyIP.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtMyIP.Name = "txtMyIP";
            this.txtMyIP.Size = new System.Drawing.Size(135, 25);
            this.txtMyIP.TabIndex = 112;
            this.txtMyIP.Text = "127.0.0.1";
            // 
            // timmConnStatus
            // 
            this.timmConnStatus.Enabled = true;
            this.timmConnStatus.Tick += new System.EventHandler(this.timmConnStatus_Tick);
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("굴림", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label8.Location = new System.Drawing.Point(6, 68);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(129, 20);
            this.label8.TabIndex = 128;
            this.label8.Text = "Pirme Input :";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.BackColor = System.Drawing.SystemColors.Control;
            this.label4.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label4.Location = new System.Drawing.Point(7, 102);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(71, 15);
            this.label4.TabIndex = 129;
            this.label4.Text = "Cal-ing?";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.BackColor = System.Drawing.SystemColors.Control;
            this.label3.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label3.Location = new System.Drawing.Point(7, 133);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(144, 15);
            this.label3.TabIndex = 130;
            this.label3.Text = "Calculating Num : ";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(306, 199);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.lbljudge);
            this.Controls.Add(this.calculating);
            this.Controls.Add(this.lblTime);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.lblNPrime);
            this.Controls.Add(this.txtNMax);
            this.Controls.Add(this.lblCalculate2);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.btnListen);
            this.Controls.Add(this.txtMyIP);
            this.Name = "Form1";
            this.Text = "Server2";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Label lbljudge;
        private System.Windows.Forms.Label calculating;
        private System.Windows.Forms.Label lblTime;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label lblNPrime;
        private System.Windows.Forms.TextBox txtNMax;
        private System.Windows.Forms.Label lblCalculate2;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Button btnListen;
        private System.Windows.Forms.TextBox txtMyIP;
        private System.Windows.Forms.Timer timmConnStatus;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
    }
}

