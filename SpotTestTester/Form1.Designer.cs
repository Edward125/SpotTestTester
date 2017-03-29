namespace SpotTestTester
{
    partial class Form1
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.lstHistoryLog = new System.Windows.Forms.ListBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.btnRestart = new System.Windows.Forms.Button();
            this.comboSNPortName = new System.Windows.Forms.ComboBox();
            this.label5 = new System.Windows.Forms.Label();
            this.txtOK = new System.Windows.Forms.TextBox();
            this.txtTotal = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.txtStage = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.comboPLCPortName = new System.Windows.Forms.ComboBox();
            this.btnTestWebSite = new System.Windows.Forms.Button();
            this.txtWebSite = new System.Windows.Forms.TextBox();
            this.txtBarcode = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.lblWeb_Site = new System.Windows.Forms.Label();
            this.btnStop = new System.Windows.Forms.Button();
            this.btnStart = new System.Windows.Forms.Button();
            this.lblBarcode = new System.Windows.Forms.Label();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.spPLC = new System.IO.Ports.SerialPort(this.components);
            this.spSN = new System.IO.Ports.SerialPort(this.components);
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // lstHistoryLog
            // 
            this.lstHistoryLog.FormattingEnabled = true;
            this.lstHistoryLog.ItemHeight = 17;
            this.lstHistoryLog.Location = new System.Drawing.Point(8, 19);
            this.lstHistoryLog.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.lstHistoryLog.Name = "lstHistoryLog";
            this.lstHistoryLog.Size = new System.Drawing.Size(609, 327);
            this.lstHistoryLog.TabIndex = 0;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.lstHistoryLog);
            this.groupBox1.Location = new System.Drawing.Point(12, 188);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.groupBox1.Size = new System.Drawing.Size(630, 356);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.btnRestart);
            this.groupBox2.Controls.Add(this.comboSNPortName);
            this.groupBox2.Controls.Add(this.label5);
            this.groupBox2.Controls.Add(this.txtOK);
            this.groupBox2.Controls.Add(this.txtTotal);
            this.groupBox2.Controls.Add(this.label4);
            this.groupBox2.Controls.Add(this.label2);
            this.groupBox2.Controls.Add(this.txtStage);
            this.groupBox2.Controls.Add(this.label1);
            this.groupBox2.Controls.Add(this.comboPLCPortName);
            this.groupBox2.Controls.Add(this.btnTestWebSite);
            this.groupBox2.Controls.Add(this.txtWebSite);
            this.groupBox2.Controls.Add(this.txtBarcode);
            this.groupBox2.Controls.Add(this.label3);
            this.groupBox2.Controls.Add(this.lblWeb_Site);
            this.groupBox2.Controls.Add(this.btnStop);
            this.groupBox2.Controls.Add(this.btnStart);
            this.groupBox2.Controls.Add(this.lblBarcode);
            this.groupBox2.Location = new System.Drawing.Point(12, 77);
            this.groupBox2.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Padding = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.groupBox2.Size = new System.Drawing.Size(630, 112);
            this.groupBox2.TabIndex = 2;
            this.groupBox2.TabStop = false;
            // 
            // btnRestart
            // 
            this.btnRestart.Location = new System.Drawing.Point(518, 78);
            this.btnRestart.Name = "btnRestart";
            this.btnRestart.Size = new System.Drawing.Size(99, 32);
            this.btnRestart.TabIndex = 17;
            this.btnRestart.Text = "Restart";
            this.btnRestart.UseVisualStyleBackColor = true;
            this.btnRestart.Click += new System.EventHandler(this.btnRestart_Click);
            // 
            // comboSNPortName
            // 
            this.comboSNPortName.FormattingEnabled = true;
            this.comboSNPortName.Location = new System.Drawing.Point(207, 78);
            this.comboSNPortName.Name = "comboSNPortName";
            this.comboSNPortName.Size = new System.Drawing.Size(65, 25);
            this.comboSNPortName.TabIndex = 16;
            this.comboSNPortName.SelectedIndexChanged += new System.EventHandler(this.comboSNPortName_SelectedIndexChanged);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(149, 83);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(57, 17);
            this.label5.TabIndex = 15;
            this.label5.Text = "SN_Port:";
            // 
            // txtOK
            // 
            this.txtOK.Location = new System.Drawing.Point(477, 80);
            this.txtOK.Name = "txtOK";
            this.txtOK.ReadOnly = true;
            this.txtOK.Size = new System.Drawing.Size(38, 23);
            this.txtOK.TabIndex = 14;
            this.txtOK.Text = "0";
            this.txtOK.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // txtTotal
            // 
            this.txtTotal.Location = new System.Drawing.Point(404, 80);
            this.txtTotal.Name = "txtTotal";
            this.txtTotal.ReadOnly = true;
            this.txtTotal.Size = new System.Drawing.Size(38, 23);
            this.txtTotal.TabIndex = 13;
            this.txtTotal.Text = "0";
            this.txtTotal.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(445, 82);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(29, 17);
            this.label4.TabIndex = 12;
            this.label4.Text = "OK:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(364, 83);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(40, 17);
            this.label2.TabIndex = 11;
            this.label2.Text = "Total:";
            // 
            // txtStage
            // 
            this.txtStage.Location = new System.Drawing.Point(321, 80);
            this.txtStage.Name = "txtStage";
            this.txtStage.ReadOnly = true;
            this.txtStage.Size = new System.Drawing.Size(40, 23);
            this.txtStage.TabIndex = 10;
            this.txtStage.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(277, 83);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(44, 17);
            this.label1.TabIndex = 9;
            this.label1.Text = "Stage:";
            // 
            // comboPLCPortName
            // 
            this.comboPLCPortName.FormattingEnabled = true;
            this.comboPLCPortName.Location = new System.Drawing.Point(75, 78);
            this.comboPLCPortName.Name = "comboPLCPortName";
            this.comboPLCPortName.Size = new System.Drawing.Size(65, 25);
            this.comboPLCPortName.TabIndex = 8;
            this.comboPLCPortName.SelectedIndexChanged += new System.EventHandler(this.comboPLCPortName_SelectedIndexChanged);
            // 
            // btnTestWebSite
            // 
            this.btnTestWebSite.Location = new System.Drawing.Point(425, 46);
            this.btnTestWebSite.Name = "btnTestWebSite";
            this.btnTestWebSite.Size = new System.Drawing.Size(86, 31);
            this.btnTestWebSite.TabIndex = 7;
            this.btnTestWebSite.Text = "TestWeb";
            this.btnTestWebSite.UseVisualStyleBackColor = true;
            this.btnTestWebSite.Click += new System.EventHandler(this.btnTestWebSite_Click);
            // 
            // txtWebSite
            // 
            this.txtWebSite.Location = new System.Drawing.Point(76, 49);
            this.txtWebSite.Name = "txtWebSite";
            this.txtWebSite.Size = new System.Drawing.Size(339, 23);
            this.txtWebSite.TabIndex = 6;
            this.txtWebSite.TextChanged += new System.EventHandler(this.txtWebSite_TextChanged);
            // 
            // txtBarcode
            // 
            this.txtBarcode.Font = new System.Drawing.Font("微软雅黑", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txtBarcode.Location = new System.Drawing.Point(75, 12);
            this.txtBarcode.Name = "txtBarcode";
            this.txtBarcode.Size = new System.Drawing.Size(436, 33);
            this.txtBarcode.TabIndex = 5;
            this.txtBarcode.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.txtBarcode.TextChanged += new System.EventHandler(this.txtBarcode_TextChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(11, 81);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(61, 17);
            this.label3.TabIndex = 4;
            this.label3.Text = "PLC_Port:";
            // 
            // lblWeb_Site
            // 
            this.lblWeb_Site.AutoSize = true;
            this.lblWeb_Site.Location = new System.Drawing.Point(11, 51);
            this.lblWeb_Site.Name = "lblWeb_Site";
            this.lblWeb_Site.Size = new System.Drawing.Size(59, 17);
            this.lblWeb_Site.TabIndex = 3;
            this.lblWeb_Site.Text = "WebSite:";
            // 
            // btnStop
            // 
            this.btnStop.Enabled = false;
            this.btnStop.Location = new System.Drawing.Point(518, 44);
            this.btnStop.Name = "btnStop";
            this.btnStop.Size = new System.Drawing.Size(99, 32);
            this.btnStop.TabIndex = 2;
            this.btnStop.Text = "Stop";
            this.btnStop.UseVisualStyleBackColor = true;
            this.btnStop.Click += new System.EventHandler(this.btnStop_Click);
            // 
            // btnStart
            // 
            this.btnStart.Location = new System.Drawing.Point(518, 12);
            this.btnStart.Name = "btnStart";
            this.btnStart.Size = new System.Drawing.Size(99, 32);
            this.btnStart.TabIndex = 1;
            this.btnStart.Text = "Start";
            this.btnStart.UseVisualStyleBackColor = true;
            this.btnStart.Click += new System.EventHandler(this.btnStart_Click);
            // 
            // lblBarcode
            // 
            this.lblBarcode.AutoSize = true;
            this.lblBarcode.Location = new System.Drawing.Point(13, 22);
            this.lblBarcode.Name = "lblBarcode";
            this.lblBarcode.Size = new System.Drawing.Size(60, 17);
            this.lblBarcode.TabIndex = 0;
            this.lblBarcode.Text = "Barcode:";
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = global::SpotTestTester.Properties.Resources.title;
            this.pictureBox1.Location = new System.Drawing.Point(87, 12);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(507, 56);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.pictureBox1.TabIndex = 3;
            this.pictureBox1.TabStop = false;
            // 
            // spPLC
            // 
            this.spPLC.DataReceived += new System.IO.Ports.SerialDataReceivedEventHandler(this.spPLC_DataReceived);
            // 
            // spSN
            // 
            this.spSN.DataReceived += new System.IO.Ports.SerialDataReceivedEventHandler(this.spSN_DataReceived);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoValidate = System.Windows.Forms.AutoValidate.Disable;
            this.BackColor = System.Drawing.Color.White;
            this.BackgroundImage = global::SpotTestTester.Properties.Resources.background;
            this.ClientSize = new System.Drawing.Size(658, 561);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "Form1";
            this.Text = "Form1";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.Form1_FormClosed);
            this.Load += new System.EventHandler(this.Form1_Load);
            this.DoubleClick += new System.EventHandler(this.Form1_DoubleClick);
            this.Move += new System.EventHandler(this.Form1_Move);
            this.groupBox1.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListBox lstHistoryLog;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label lblBarcode;
        private System.Windows.Forms.TextBox txtWebSite;
        private System.Windows.Forms.TextBox txtBarcode;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label lblWeb_Site;
        private System.Windows.Forms.Button btnStop;
        private System.Windows.Forms.Button btnStart;
        private System.Windows.Forms.Button btnTestWebSite;
        private System.Windows.Forms.ComboBox comboPLCPortName;
        private System.Windows.Forms.TextBox txtStage;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtOK;
        private System.Windows.Forms.TextBox txtTotal;
        private System.Windows.Forms.ComboBox comboSNPortName;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.IO.Ports.SerialPort spPLC;
        private System.IO.Ports.SerialPort spSN;
        private System.Windows.Forms.Button btnRestart;
    }
}

