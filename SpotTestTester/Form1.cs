using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using System.Runtime.InteropServices;
using System.Diagnostics;
using System.IO;
using System.IO.Ports;
using Edward;
using System.Reflection;
using System.Resources;


namespace SpotTestTester
{
    public partial class Form1 :Form 
    {
        public Form1()
        {
            InitializeComponent();
            //
            //（句柄，毫秒时间，函数）
            AnimateWindow(this.Handle, 100, AW_SLIDE + AW_VER_NEGATIVE);//开始窗体动画
            x = this.Location.X;//窗体左上角x坐标
            y = this.Location.Y;//窗体左上角y坐标
            WIDTH = this.Width;//得到当前窗体的宽度
            HEIGHT = this.Height;//得到当前窗体的高度
            //得到屏幕的分辨率
            SH = Screen.PrimaryScreen.Bounds.Height;
            SW = Screen.PrimaryScreen.Bounds.Width;
            //
            //this.spPLC.DataReceived += new System.IO.Ports.SerialDataReceivedEventHandler(this.spPLC_DataReceived);
            //this.spSN.DataReceived += new System.IO.Ports.SerialDataReceivedEventHandler(this.spSN_DataReceived);
        }


        #region Define Param

        string exeTitle = "WCD Spot Test Auto Spilt Barcode Tester,Ver:" + Application.ProductVersion + ",Author:edward_song@yeah.net";

        //
        public const Int32 AW_HOR_POSITIVE = 0x00000001;
        public const Int32 AW_HOR_NEGATIVE = 0x00000002;
        public const Int32 AW_VER_POSITIVE = 0x00000004;
        public const Int32 AW_VER_NEGATIVE = 0x00000008;
        public const Int32 AW_CENTER = 0x00000010;
        public const Int32 AW_HIDE = 0x00010000;
        public const Int32 AW_ACTIVATE = 0x00020000;
        public const Int32 AW_SLIDE = 0x00040000;
        public const Int32 AW_BLEND = 0x00080000;
        static int x;
        static int y;
        static int WIDTH;
        static int HEIGHT;
        int SH;
        int SW;
        //
        //
        public PCBWeb.WebService ws = new PCBWeb.WebService();//定義webservice

        bool Debug_Mode = false;
        string PLC_Port = string.Empty;
        string SN_Port = string.Empty;
        bool Web_Use = true;
        string Web_Site = "http://172.0.1.172/Tester.WebService/WebService.asmx";
        string Stage = "TD";

        public string appFolder = Application.StartupPath + @"\SpotTest";
        public string iniFilePath = Application.StartupPath + @"\SpotTest\SysConfig.ini";
        public string logFolder = Application.StartupPath + @"\SpotTest\BarcodeLog";

        bool bStart = false;

        //SerialPort spPLC = new System.IO.Ports.SerialPort();
        //SerialPort spSN = new System.IO.Ports.SerialPort();

        ////
        public string STAGE_OK = @"@00FA000000000010282000100000100117E*" + @Edward.Other.Chr(13);
        public string STAGE_NG = @"@00FA000000000010282000100000100227E*" + @Edward.Other.Chr(13);

        public const string WRITE_OK = @"@00FA00400000000102000040*";
        public const string WRITE_NG = @"00FA1345*";
        string plcreadtemp = string.Empty;


        System.Media.SoundPlayer sp;              
        Assembly assembly = Assembly.GetExecutingAssembly();
        

        #endregion

        #region API define

        //重写API函数，用来执行窗体动画显示操作
        [DllImportAttribute("user32.dll")]
        private static extern bool AnimateWindow(IntPtr hwnd, int dwTime, int dwFlags);

        #endregion

        #region Dynamic detect serial port

        // usb消息定义
        public const int WM_DEVICE_CHANGE = 0x219;
        public const int DBT_DEVICEARRIVAL = 0x8000;
        public const int DBT_DEVICE_REMOVE_COMPLETE = 0x8004;
        public const UInt32 DBT_DEVTYP_PORT = 0x00000003;

        [StructLayout(LayoutKind.Sequential)]
        struct DEV_BROADCAST_HDR
        {
            public UInt32 dbch_size;
            public UInt32 dbch_devicetype;
            public UInt32 dbch_reserved;
        }

        [StructLayout(LayoutKind.Sequential)]
        protected struct DEV_BROADCAST_PORT_Fixed
        {
            public uint dbcp_size;
            public uint dbcp_devicetype;
            public uint dbcp_reserved;
            // Variable?length field dbcp_name is declared here in the C header file.
        }

        /// <summary>
        /// 检测USB串口的拔插
        /// </summary>
        /// <param name="m"></param>
        protected override void WndProc(ref Message m)
        {
            if (m.Msg == WM_DEVICE_CHANGE)        // 捕获USB设备的拔出消息WM_DEVICECHANGE
            {

                string portName = Marshal.PtrToStringUni((IntPtr)(m.LParam.ToInt32() + Marshal.SizeOf(typeof(DEV_BROADCAST_PORT_Fixed))));
                switch (m.WParam.ToInt32())
                {

                    case DBT_DEVICE_REMOVE_COMPLETE:    // USB拔出 
                        DEV_BROADCAST_HDR dbhdr0 = (DEV_BROADCAST_HDR)Marshal.PtrToStructure(m.LParam, typeof(DEV_BROADCAST_HDR));
                        if (dbhdr0.dbch_devicetype == DBT_DEVTYP_PORT)
                        {
                            try
                            {
                                // comboPortName.Items.Remove(portName);

                                if (btnStart.Enabled)
                                {
                                    getSerialPort(comboPLCPortName);
                                    getSerialPort(comboSNPortName);
                                }
                                else
                                {
                                    sp = new System.Media.SoundPlayer(global::SpotTestTester.Properties.Resources.ng);
                                    sp.Play();
                                    updateMessage(lstHistoryLog, "Port '" + portName + "' leaved.");
                                    updateMessage(lstHistoryLog, "偵測到串口丟失，請重新設置后點擊開始，若無法啟動，點擊Restart再點擊Start。");
                                    closePort(spPLC);
                                    closePort(spSN);
                                    pressStopButton();
                                }

                              
                            }
                            catch (Exception ex)
                            {
                                sp = new System.Media.SoundPlayer(global::SpotTestTester.Properties.Resources.ng);
                                sp.Play();
                                updateMessage(lstHistoryLog, "Port '" + portName + "' leaved.");
                                updateMessage(lstHistoryLog, ex.Message);
                                

                            }
                            Console.WriteLine("Port '" + portName + "' leaved.");
                        }

                        break;
                    case DBT_DEVICEARRIVAL:             // USB插入获取对应串口名称
                        DEV_BROADCAST_HDR dbhdr = (DEV_BROADCAST_HDR)Marshal.PtrToStructure(m.LParam, typeof(DEV_BROADCAST_HDR));
                        if (dbhdr.dbch_devicetype == DBT_DEVTYP_PORT)
                        {
                            getSerialPort(comboPLCPortName);
                            getSerialPort(comboSNPortName);
                            Console.WriteLine("Port '" + portName + "' arrived.");
                        }
                        break;
                }
            }
            base.WndProc(ref m);
        }
        #endregion
        private void Form1_Load(object sender, EventArgs e)
        {
            this.Text = exeTitle;
            frmLoadCheckFolder();
            //
            IniFile.iniFilePathValue = iniFilePath;
            if (!File.Exists (iniFilePath ))
                createIniFile(iniFilePath);
            //
            getSerialPort(comboPLCPortName);
            getSerialPort(comboSNPortName);
            //
            loadSysConfigData(iniFilePath);

            Console.WriteLine("SN_Port:" + SN_Port);
            Console.WriteLine("PLC_Port:" + PLC_Port);
            //
            loadConfigDataToUI();



           


            bStart = true;
            //if (!string.IsNullOrEmpty(comboPLCPortName.Text.Trim()))
            //{
            //    PLC_Port = comboPLCPortName.Text.Trim().ToUpper();
            //}
            //if (!string.IsNullOrEmpty(comboSNPortName.Text.Trim()))
            //{
            //    SN_Port = comboSNPortName.Text.Trim().ToUpper();
            //}

            //download file

          
        }


        #region 结束窗体时特效
        /// <summary>
        /// 结束窗体时特效
        /// </summary>
        private void window()
        {

            while (this.Width > 124)
            {
                if (this.Height >= 40)
                {
                    this.Location = new System.Drawing.Point(x, y += 15);//设置窗体位置
                    this.Size = new Size(this.Width, this.Height -= 25);//设置窗体大小
                    this.Refresh();//重绘窗体
                }

                else
                {
                    this.Location = new System.Drawing.Point(x += 15, y);

                    if (this.Width > 123)
                    {

                        this.Size = new Size(this.Width -= 25, this.Height);
                        this.Refresh();
                        this.Opacity -= 0.04;//设置窗体透明度递减
                    }

                }
                Thread.Sleep(10);  // 线程休眠10毫秒
            }

        }
        #endregion

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
           // window();
        }

        private void Form1_Move(object sender, EventArgs e)
        {
            //x = this.Location.X;
            //y = this.Location.Y;
        }

        /// <summary>
        /// 窗體加載時檢測文件夾和文件
        /// </summary>
        private void frmLoadCheckFolder()
        {

            if (!Directory.Exists(appFolder))
            {
                try
                {
                    Directory.CreateDirectory(appFolder);
                }
                catch (Exception e)
                {
                    throw e;

                }
            }

            if (!Directory.Exists(logFolder))
            {
                try
                {
                    Directory.CreateDirectory(logFolder);
                }
                catch (Exception e)
                {
                    throw e;
                }
            }



        }
        /// <summary>
        /// 更新消息至listbox
        /// </summary>
        /// <param name="listbox">Skin list</param>
        /// <param name="message">message contents</param>
        private void updateMessage(ListBox  listbox, string message)
        {


            string item = DateTime.Now.ToString("yyyyMMddHHmmss") + "->" + @message;          
     
            this.Invoke((EventHandler)(delegate
            {
                if (listbox.Items.Count > 100)
                    listbox.Items.RemoveAt(0);
                listbox.Items.Add(item);
                if (listbox.Items.Count > 1)
                {
                    listbox.TopIndex = listbox.Items.Count - 1;
                    listbox.SetSelected(listbox.Items.Count - 1, true);
                }
            }
));
        }

        /// <summary>
        /// 檢測WebService的可連通性,可連通返回true，不可連通，返回false
        /// </summary>
        /// <param name="website">WebService的地址</param>
        /// <returns>可連通返回true，不可連通返回false</returns>
        private bool checkWebService(string website)
        {
            Stopwatch sw = new Stopwatch();
            TimeSpan ts = new TimeSpan();
            sw.Start();
            updateMessage(lstHistoryLog, "Check Web Service");
            saveLog("Check Web Service");
            ws.Url = Web_Site;
            try
            {
                ws.Discover();
            }
            catch (Exception e)
            {
                sw.Stop();
                ts = sw.Elapsed;
                updateMessage(lstHistoryLog, "Check Web Service NG,Used time(ms):" + ts.Milliseconds);
                saveLog("Check Web Service NG,Used time(ms):" + ts.Milliseconds + "\r\n" + "Message:".PadLeft(24) + e.Message);
                MessageBox.Show("Can't Connect to Web Service,\r\nMessage:" + e.Message);
                return false;
            }
            sw.Stop();
            ts = sw.Elapsed;
            updateMessage(lstHistoryLog, "Check Web Service OK,Used time(ms):" + ts.Milliseconds);
            saveLog("Check Web Service OK,Used time(ms):" + ts.Milliseconds);
            return true;
        }

        /// <summary>
        /// 保存log到log文件中去
        /// </summary>
        /// <param name="logcontents">需要保存的log內容</param>
        private void saveLog(string logcontents)
        {
            string logPath = logFolder + @"\" + DateTime.Now.ToString("yyyyMMdd") + @"_all.log";
            //FileStream fs = new FileStream();
            if (!File.Exists(logPath))
            {
                FileStream fs = File.Create(logPath);
                //  fs = File.Create(logPath);
                fs.Close();
            }
            try
            {
                StreamWriter sw = File.AppendText(logPath);
                sw.Write(DateTime.Now.ToString("yyyyMMddHHmmss") + "->" + logcontents + "\r\n");
                sw.Close();
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// 保存log到log文件中去
        /// </summary>
        /// <param name="usn">條碼</param>
        /// <param name="stage">條碼對應的站別</param>
        private void saveLog(string usn, string stage)
        {
            string logPath = logFolder + @"\" + DateTime.Now.ToString("yyyyMMdd") + @"_" + stage + @".log";
            //FileStream fs = new FileStream();
            if (!File.Exists(logPath))
            {
                FileStream fs = File.Create(logPath);
                //  fs = File.Create(logPath);
                fs.Close();
            }
            try
            {
                StreamWriter sw = File.AppendText(logPath);
                sw.Write(DateTime.Now.ToString("yyyyMMddHHmmss") + "->" + usn + "->" + stage + "\r\n");
                sw.Close();
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// 保存log到log文件中去
        /// </summary>
        /// <param name="usn">條碼</param>
        /// <param name="stage">條碼對應的站別</param>
        /// <param name="result">sfcs查詢的結果，true or false</param>
        /// <param name="plcwrite">向plc寄存器寫入的值</param>
        private void saveLog(string usn, string stage, string result, int plcwrite)
        {
            string logPath = logFolder + @"\" + DateTime.Now.ToString("yyyyMMdd") + @"_ScanList.log";
            //FileStream fs = new FileStream();
            if (!File.Exists(logPath))
            {
                FileStream fs = File.Create(logPath);
                //  fs = File.Create(logPath);
                fs.Close();
            }
            try
            {
                StreamWriter sw = File.AppendText(logPath);
                sw.Write(DateTime.Now.ToString("yyyyMMddHHmmss") + "->" + usn.PadRight(30) + stage.PadRight(10) + plcwrite.ToString().PadRight(10) + result + "\r\n");
                sw.Close();
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// 創建ini配置檔并加載初始化值
        /// </summary>
        /// <param name="inifilepath">配置檔地址</param>
        /// <returns></returns>
        private void createIniFile(string inifilepath)
        {
            FileStream fs = File.Create(iniFilePath);
            fs.Close();
            IniFile.IniWriteValue("SysConfig", "SysVersion", ProductVersion, inifilepath);
            IniFile.IniWriteValue("SysConfig", "Debug_Mode", "0", inifilepath);
            IniFile.IniWriteValue("Robot_Set", "PLC_Port", "", inifilepath);
            IniFile.IniWriteValue("Robot_Set", "SN_Port", "", inifilepath);
            IniFile.IniWriteValue("SFCS_Set", "Web_Use", "1", inifilepath);
            IniFile.IniWriteValue("SFCS_Set", "Web_Site", "http://172.0.1.172/Tester.WebService/WebService.asmx", inifilepath);
            IniFile.IniWriteValue("SFCS_Set", "Stage", "TD", inifilepath);
        }

        /// <summary>
        /// 從ini配置檔中讀取數據
        /// </summary>
        /// <param name="inifilepath">配置檔地址</param>
        private void loadSysConfigData(string inifilepath)
        {
            IniFile.IniFilePath = inifilepath;
            if (IniFile.IniReadValue("SysConfig", "Debug_Mode").Trim() == "0")
                Debug_Mode = false;
            if (IniFile.IniReadValue("SysConfig", "Debug_Mode").Trim() == "1")
                Debug_Mode = true;
            if (IniFile.IniReadValue("SFCS_Set", "Web_Use").Trim() == "1")
                Web_Use = true;
            if (IniFile.IniReadValue("SFCS_Set", "Web_Use").Trim() == "0")
                Web_Use = false;

            PLC_Port = IniFile.IniReadValue ("Robot_Set","PLC_Port").Trim().ToUpper ();
            SN_Port = IniFile.IniReadValue ("Robot_Set","SN_Port").Trim().ToUpper ();

            Web_Site = IniFile.IniReadValue("SFCS_Set", "Web_Site").Trim();
            Stage = IniFile.IniReadValue("SFCS_Set", "Stage").Trim().ToUpper();
        }

        /// <summary>
        /// load config data to ui
        /// </summary>
        private void loadConfigDataToUI()
        {
            txtWebSite.Text = Web_Site;
            txtStage.Text = Stage;
            comboSNPortName.Text = SN_Port;
            comboPLCPortName.Text = PLC_Port;


            if (Debug_Mode)
                txtBarcode.ReadOnly = true;
            else
                txtBarcode.ReadOnly = false;
            
        }


        /// <summary>
        /// get serial port
        /// </summary>
        /// <param name="comboportname">combox</param>
        private void getSerialPort(ComboBox comboportname)
        {
            comboportname.Items.Clear();
            foreach (string st in System.IO.Ports.SerialPort.GetPortNames())
            {
                comboportname.Items.Add(st);
            }

            if (comboportname.Items.Count > 0)
            {
                comboportname.Sorted = true;
                comboportname.SelectedIndex = 0;

            }
            //else
            //    comboportname.Text = string.Empty;
        }

        /// <summary>
        /// open port,OK,return true;NG,return false
        /// </summary>
        /// <param name="serialport">serail port</param>
        /// <returns>OK,return true;NG,return false</returns>
        private bool  openPort(System.IO.Ports.SerialPort serialport)
        {
    
            if (serialport.IsOpen)
            {
                updateMessage(lstHistoryLog,  serialport.PortName + " is already open.");
                return true;
            }
            else
            {
                try
                {

                    serialport.Open();
                    updateMessage(lstHistoryLog, "Open " + serialport.PortName + " is success.");
                    return true;

                }
                catch (Exception ex)
                {
                    updateMessage(lstHistoryLog, "Open " + serialport.PortName + " is fail,detail:" + ex.Message);
                    return false;
                }    
           }
   
        }

        /// <summary>
        /// open port,OK,return true;NG,return false
        /// </summary>
        /// <param name="serialport">serail port</param>
        /// <returns>OK,return true;NG,return false</returns>
        private bool openPort(System.IO.Ports.SerialPort serialport,string portname)
        {
            serialport.PortName = portname.Trim().ToUpper();

            if (serialport.IsOpen)
            {
                updateMessage(lstHistoryLog, serialport.PortName + " is already open.");
                return true;
            }
            else
            {
                try
                {

                    serialport.Open();
                    updateMessage(lstHistoryLog, "Open " + serialport.PortName + " is success.");
                    return true;

                }
                catch (Exception ex)
                {
                    updateMessage(lstHistoryLog, "Open " + serialport.PortName + " is fail,detail:" + ex.Message);
                    return false;
                }
            }

        }

        /// <summary>
        /// close port,OK return true;NG return false
        /// </summary>
        /// <param name="serialport">serial port</param>
        /// <returns>OK return true;NG return false</returns>
        private bool closePort(SerialPort serialport)
        {
            if (serialport.IsOpen)
            {
                try
                {
                    serialport.Close();
                    updateMessage(lstHistoryLog, serialport.PortName + " is closed.");
                    return true;
                }
                catch (Exception ex)
                {
                    updateMessage(lstHistoryLog, "Close " + serialport.PortName + " is fail,detail:" + ex.Message);
                    return false;
                }
            }
            else
            {
                updateMessage(lstHistoryLog, serialport.PortName + " is already closed.");
                return true;
            }

        }



        private void btnStart_Click(object sender, EventArgs e)
        {

            // check port
            if (string.IsNullOrEmpty(comboPLCPortName.Text.Trim()))
            {
                // MessageBox.Show("Serial PortName can't be empty.");
                updateMessage(lstHistoryLog, "Serial PortName can't be empty.");

                comboPLCPortName.Focus();
                return;
            }

            // check web site
            if (string.IsNullOrEmpty(txtWebSite.Text.Trim()))
            {
                updateMessage(lstHistoryLog, "Web Service's site can't be empty.");
                txtWebSite.Focus();
                return;
            }
            //
            if (comboPLCPortName.Text.Trim().ToUpper() == comboSNPortName.Text.Trim().ToUpper())
            {
                updateMessage(lstHistoryLog, "PLC portname and SN portname can't be same.");
                comboPLCPortName.Focus();
                return;
            }

            //
            if (Web_Use)
            {
                ws.Url = Web_Site;
                try
                {
                    ws.Discover();
                    updateMessage(lstHistoryLog, "Connect web service sucess.");
                }
                catch (Exception ex)
                {
                    updateMessage(lstHistoryLog, ex.Message);
                    return;
                }
            }
            //
            if (!openPort(spPLC, PLC_Port))
            {
                closePort(spPLC);
                return;
            }
            //
            if (!openPort(spSN, SN_Port))
            {
                closePort(spPLC);
                closePort(spSN);
                return;
            }

            //
            pressStartButton();




        }

        private void pressStartButton()
        {
            if (!Debug_Mode)
                txtBarcode.ReadOnly = true;
            txtWebSite.ReadOnly = true;
            comboPLCPortName.Enabled = false;
            comboSNPortName.Enabled = false;
            btnStop.Enabled = true;
            btnStart.Enabled = false;
            btnTestWebSite.Enabled = false;
            btnRestart.Enabled = false;


        }

        private void pressStopButton()
        {

            txtBarcode.ReadOnly = false;
            txtWebSite.ReadOnly = false;
            comboPLCPortName.Enabled = true;
            comboSNPortName.Enabled = true;
            btnStop.Enabled = false;
            btnStart.Enabled = true;
            btnTestWebSite.Enabled = true;
            btnRestart.Enabled = true;
        }

        private void btnTestWebSite_Click(object sender, EventArgs e)
        {
            // check web site
            if (string.IsNullOrEmpty(txtWebSite.Text.Trim()))
            {
                updateMessage(lstHistoryLog, "Web Service's site can't be empty.");
                txtWebSite.Focus();
                return;
            }
            //

            ws.Url = Web_Site;
            try
            {
            ws.Discover();
               updateMessage(lstHistoryLog, "Connect web service sucess.");
            }
            catch (Exception ex)
            {
              updateMessage(lstHistoryLog, ex.Message);
              return;
            }
        }

        private void comboPLCPortName_SelectedIndexChanged(object sender, EventArgs e)
        {

            if (bStart)
            {
                if (comboPLCPortName.SelectedIndex != -1)
                {
                    PLC_Port = comboPLCPortName.Text.Trim().ToUpper();
                    IniFile.IniWriteValue("Robot_Set", "PLC_Port", PLC_Port);
                    spPLC.PortName = PLC_Port;
                  //  spPLC = new SerialPort(PLC_Port, 9600, Parity.None, 8, StopBits.One);
                }
            }
            //else
            //{
            //    if (comboPLCPortName.SelectedIndex != -1)
            //    {
            //       // if (string.IsNullOrEmpty (PLC_Port ))
            //      //  {
            //            //PLC_Port = comboPLCPortName.Text.Trim().ToUpper();
            //            //IniFile.IniWriteValue("Robot_Set", "PLC_Port", PLC_Port);
            //            //spPLC.PortName = PLC_Port;
            //        //}

            //      //  spPLC = new SerialPort(PLC_Port, 9600, Parity.None, 8, StopBits.One);
            //    }
            //}

        }

        private void txtWebSite_TextChanged(object sender, EventArgs e)
        {
            if (bStart)
            {
                Web_Site = txtWebSite.Text.Trim();
                IniFile.IniWriteValue("SFCS_Set", "Web_Site", Web_Site);
            }
        }

        private void btnStop_Click(object sender, EventArgs e)
        {
            if (!closePort(spPLC))
                return;
            if (!closePort(spSN))
                return;
            pressStopButton();
        }

        /// <summary>
        /// 檢查USN站別是否在當前站別,在為true，不在為false
        /// </summary>
        /// <param name="usn">條碼</param>
        /// <param name="stage">站別</param>
        /// <returns>在當前站別為true，不在當前站別為false</returns>
        private bool checkStage(string usn, string stage)
        {

            //  checkWebService(web_Site);

            Stopwatch sw = new Stopwatch();
            TimeSpan ts = new TimeSpan();
            sw.Start();
            usn = usn.Trim().ToUpper();
            updateMessage(lstHistoryLog, "SFCS:" + usn + ",Stage:" + stage);
            saveLog("SFCS:" + usn  + ",Stage:" + stage);
            string result = ws.CheckRoute(usn, stage);
            sw.Stop();
            ts = sw.Elapsed;
            if (result.ToUpper() == "OK")
            {
                updateMessage(lstHistoryLog, result + ",Used time(ms):" + ts.Milliseconds);
                saveLog(usn, stage);

                return true;
            }
            else
            {
                updateMessage(lstHistoryLog, result + ",Used time(ms):" + ts.Milliseconds);
                saveLog(result + ",Used time(ms):" + ts.Milliseconds);
                saveLog(usn, getStage(result));
                return false;
            }

        }

        /// <summary>
        /// 從SFCS返回的結果裡面抓取站別信息
        /// </summary>
        /// <param name="sfcsresult">sfcs 返回的結果</param>
        /// <returns></returns>
        private string getStage(string sfcsresult)
        {
            string stage = string.Empty;

            // If message = "" Then GetStage = ""
            //Dim s() As String = message.Split(" ")
            //For i As Integer = 0 To s.Length - 1
            //    If s(i).ToLower.Trim = "to" Then
            //        GetStage = s(i + 1)
            //        Exit For
            //    End If
            //Next

            if (string.IsNullOrEmpty(sfcsresult))
                stage = string.Empty;
            string[] s = sfcsresult.Split(' ');
            for (int i = 0; i <= s.Length - 1; i++)
            {
                if (s[i].ToLower().Trim() == "to")
                {
                    stage = s[i + 1];
                    break;
                }
                if (s[i].ToLower().Trim() == "storein!")
                {
                    stage = "SA";
                    break;
                }
            }
            return stage;
        }


        /// <summary>
        /// send string data to serialport
        /// </summary>
        /// <param name="serialport">serialport</param>
        /// <param name="data">string data </param>
        private void SendData(SerialPort serialport, string data)
        {
            try
            {
                serialport.Write(data);
               // updateMessage(lstHistoryLog, "PC->PLC:" + data );
            }
            catch (Exception ex)
            {
                updateMessage(lstHistoryLog, "Wrtie " + serialport.PortName + " = " + data + " fail,detail:" + ex.Message);
            }
        }

        private void comboSNPortName_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (bStart)
            {
                if (comboSNPortName.SelectedIndex != -1)
                {
                    SN_Port = comboSNPortName.Text.Trim().ToUpper();
                    IniFile.IniWriteValue("Robot_Set", "SN_Port", SN_Port);
                   // spSN  = new SerialPort(SN_Port, 9600, Parity.None, 8, StopBits.One);
                    spSN.PortName = SN_Port;
                }
            }
            //else
            //{
            //    if (comboSNPortName.SelectedIndex != -1)
            //    {
            //        //SN_Port = comboSNPortName.Text.Trim().ToUpper();
            //        //IniFile.IniWriteValue("Robot_Set", "SN_Port", SN_Port);
            //        // spSN.PortName = SN_Port;
            //       // spSN  = new SerialPort(SN_Port, 9600, Parity.None, 8, StopBits.One);
            //    }
            //}
        }

        private void spSN_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            if (spSN.BytesToRead == 0)
                return;
            string sn = spSN.ReadTo(Edward.Other.Chr(13));
            //string sn = spSN.ReadExisting();
            this.Invoke((EventHandler)(delegate
            {
                txtBarcode.Text = sn.ToString().ToUpper();
                updateMessage(lstHistoryLog, "SN:" + sn);
                //Stream soundStream;
                sp = new System.Media.SoundPlayer(global::SpotTestTester.Properties.Resources.ok);
                sp.Play();
                txtTotal.Text = (Convert.ToInt32(txtTotal.Text.Trim()) + 1).ToString();
            }
            ));

            if (checkStage(sn, Stage.ToUpper().Trim()))
            {
                SendData(spPLC, STAGE_OK);
                this.Invoke((EventHandler)(delegate
                {
                    updateMessage(lstHistoryLog, "Write PLC:" + STAGE_OK);
                    txtOK.Text = (Convert.ToInt32(txtOK.Text.Trim()) + 1).ToString();
                }
                ));
            }
            else
            {
                SendData(spPLC, STAGE_NG);
                this.Invoke((EventHandler)(delegate
                {
                    updateMessage(lstHistoryLog, "Write PLC:" + STAGE_NG);
                }
));
            }


        }

        private void spPLC_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            if (spPLC.BytesToRead == 0)
                return;
           // string temp = spPLC.ReadTo("*");
            string temp = spPLC.ReadExisting().Trim();
            plcreadtemp = plcreadtemp + temp;
//            this.Invoke((EventHandler)(delegate
//            {

//                updateMessage(lstHistoryLog, temp);
//            }
//));
            
            if (plcreadtemp .Contains ( WRITE_OK))
            {
                this.Invoke((EventHandler)(delegate
                {
                    updateMessage(lstHistoryLog, "Write PLC OK," + WRITE_OK );
                    plcreadtemp = string.Empty;
                }));

                return;
            }
            if (plcreadtemp.Contains ( WRITE_NG))
            {
                this.Invoke((EventHandler)(delegate
                {
                    updateMessage(lstHistoryLog, "Write PLC OK," + WRITE_NG );
                    plcreadtemp = string.Empty;
                }));
                return;
            }
        }

        private void txtBarcode_TextChanged(object sender, EventArgs e)
        {

        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
          //  Environment.Exit(0);
        }

        private void Form1_DoubleClick(object sender, EventArgs e)
        {
            loadSysConfigData(iniFilePath);

            Console.WriteLine("SN_Port:" + SN_Port);
            Console.WriteLine("PLC_Port:" + PLC_Port);
            //
            loadConfigDataToUI();
        }

        #region downloadresoucefile

        /// <summary>
        /// 將資源文件中的文件下載到本地
        /// </summary>
        /// <param name="filename">本地文件路徑</param>
        /// <param name="file">資源文件中的文件</param>
        /// <param name="hiddenFlag">true，本地文件隱藏；false，本地文件不隱藏</param>
        /// <returns>下載成功，true；下載失敗，false</returns>
        private bool DownloadResouceFile(string filename, byte[] file, bool hiddenFlag)
        {
            if (System.IO.File.Exists(filename))
                return true;
            //byte[] file = global::EdwardToolBox.Properties.Resources.
            try
            {
                System.IO.FileStream fsObj = new System.IO.FileStream(filename, System.IO.FileMode.Create);
                fsObj.Write(file, 0, file.Length);
                fsObj.Close();
                if (hiddenFlag)
                {
                    System.IO.FileInfo fi = new System.IO.FileInfo(filename);
                    fi.Attributes = System.IO.FileAttributes.Hidden;
                }
                return true;
            }
            catch (Exception ex)
            {
                //MessageBox.Show("Create " + filename.Substring(filename.LastIndexOf(@"\") + 1, filename.Length) + " file," + ex.Message);
                updateMessage(lstHistoryLog, ex.Message);
                return false;
            }
        }

        #endregion

        private void btnRestart_Click(object sender, EventArgs e)
        {
            Application.Restart();
        }



//        private void spPLC_DataReceived(object sender, SerialDataReceivedEventArgs e)
//        {
//            if (spPLC.BytesToRead == 0)
//                return;
//            string temp = spPLC.ReadTo("*");
//            if (temp == WRITE_OK)
//            {
//                this.Invoke((EventHandler)(delegate
//                {
//                    updateMessage(lstHistoryLog, "Write PLC OK," + WRITE_OK + "*");
//                }));

//                return;
//            }
//            if (temp == WRITE_NG)
//            {
//                this.Invoke((EventHandler)(delegate
//                {
//                    updateMessage(lstHistoryLog, "Write PLC OK," + WRITE_NG + "*");
//                }));
//                return;
//            }
//        }

//        private void spSN_DataReceived(object sender, SerialDataReceivedEventArgs e)
//        {
//            if (spSN.BytesToRead == 0)
//                return;
//            string sn = spSN.ReadTo(Edward.Other.Chr(13));
//            this.Invoke((EventHandler)(delegate
//            {
//                txtBarcode.Text = sn.ToString().ToUpper();
//                updateMessage(lstHistoryLog, "SN:" + sn);
//            }
//            ));

//            if (checkStage(sn, Stage.ToUpper().Trim()))
//            {
//                SendData(spPLC, STAGE_OK);
//                this.Invoke((EventHandler)(delegate
//                {
//                    updateMessage(lstHistoryLog, "Write PLC:" + STAGE_OK);
//                }
//                ));
//            }
//            else
//            {
//                SendData(spPLC, STAGE_NG);
//                this.Invoke((EventHandler)(delegate
//                {
//                    updateMessage(lstHistoryLog, "Write PLC:" + STAGE_NG);
//                }
//));
//            }





//        }
    }
}
