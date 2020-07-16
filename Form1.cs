using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using LogicalHandleBll;
using System.IO.Ports;
using System.Threading;
using System.IO;
using NLua;

namespace FlashAnalyze
{
    //串口，输出框结构体，方便线程传值
    struct SerialToTextBox
    {
        public SerialPort Port;
        public RichTextBox PrintBox;
        public SerialToTextBox(SerialPort PortValue,RichTextBox PrintCon)
        {
            Port = PortValue;
            PrintBox = PrintCon;
        }
    }
    //串口参数配置
    struct SerialArgs
    {
        public static int BrauRate = 460800;
        public static StopBits StopBit = StopBits.One;
        public static Parity CheckBit = Parity.None;
        public static int DataBit = 8;
    }
    //PageRead时的文件路径和偏移量
    public struct PathAndOffert
    {
        public string[] FilePath ;//文件路径
        public int Offert;//偏移的pagesize
        public int PlaneNum;//plane数量
        public int ProgramPageSize;//编程的page数量
        public PathAndOffert(string path1,string path2,string path3,string path4,int offert,int num,int size)
        {
            FilePath = new string[4];
            FilePath[0] = path1;
            FilePath[1] = path2;
            FilePath[2] = path3;
            FilePath[3] = path4;
            Offert = offert;
            PlaneNum = num;
            ProgramPageSize = size;
        }
    }
    //主控，目前只是一个主控，后续添加主控时需要注意menuItem需要设置只能选择一个
    //不同的主控对应发送的数据不同
    enum MasterContrl
    {
        Storart
    }

    public partial class Form1 : Form
    {
        private string logFilePath = @"./log/";
        //锁
        public static object locker = new object();
        //已连接的串口号对应串口句柄
        public static Dictionary<string, SerialPort> PortList = new Dictionary<string, SerialPort>();
        //已连接串口号对应写入文件的路径
        public static Dictionary<string, string> SerialToPath = new Dictionary<string, string>();
        //所有脚本节点
        public List<string> ScriptList;
        //当前点击的节点
        private TreeNode ClickNode;
        //选择的脚本
        private string SelectScript;
        //修改文件名时记住修改之前文件名
        private string ChangeFileName;
        //记录打开时候的内容
        private string OldText;
        //判断是否接收到信号，总得发送数据开关
        public static bool IsRecvSingle = false;
        //判断当前是否需要发送数据
        public static Dictionary<string,bool> IsSendData = new Dictionary<string, bool>();
        //需要发送的CMD list
        public static Dictionary<string, List<string>> CmdList = new Dictionary<string, List<string>>();
        //当前次需要读取数据大小
        private static int ReadPageNum ;
        //当前需要操作的页的大小,可能是多plane操作
        //private static int PageSize;
        //写入数据大小
        private static int WritePageNum ;
        //当前选择的flash的page大小
        private static int FlashDmaSize;
        private static int FlashManualSize;
        //当前脚本中的manual方式
       // private static int ManualFlag = -1;
        //读数据方式
        private static int FlashFlag = 0;

        //FW
        private byte[] FWfile = new byte[28 * 1024];

        //需要发送的数据    
        public static Dictionary<string, List<PathAndOffert>> WriteDataList = new Dictionary<string, List<PathAndOffert>>();
        //写入数据的路径
        public static Dictionary<string,List< PathAndOffert>> ComDataPath = new Dictionary<string, List<PathAndOffert>>();
        //是否需要接收数据
        public static Dictionary<string,bool> IsPageRead = new Dictionary<string, bool>();
        //记录当前PageRead时的长度
        public static List<int> PageLength = new List<int>();
        //是否接收到数据开始信号
        public static Dictionary<string,bool> ComDataStart = new Dictionary<string, bool>();
        // 当前每个串口对应的写入长度
        public static Dictionary<string ,int> ComWriteNum = new Dictionary<string, int>();
        //当前选择的主控
        //private static MasterContrl MainContrl = MasterContrl.Storart;
        //判断是否是双击
        private bool isFirstClick;
        private bool isDoubleClick;
        private int milliseconds ;
        //文件系统监控对象
        private static FileSystemWatcher ScriptWatcher = new FileSystemWatcher();
        //保存文件时编辑器中光标所处的位置
        private static int selectionStart;
        private Rectangle doubleClickRectangle;

        public Form1()
        {
            InitializeComponent();
            this.ScriptTextBox.LanguageOption = RichTextBoxLanguageOptions.UIFonts;
            ScriptList = new List<string>();
            SelectScript = string.Empty;
            isFirstClick = true;
            isDoubleClick = false;
            milliseconds = 0;
            doubleClickRectangle = new Rectangle();
            CheckForIllegalCrossThreadCalls = false;
        }
        //窗体加载
        private void Form1_Load(object sender, EventArgs e)
        {
            List<string> ComList = new List<string>();
            //窗口初始化首先获取串口连接信息，用于渲染下拉菜单
            ComList = DataProcess.GetSerialPortInfo();
            try
            {
                foreach (string PerCom in ComList)
                {
                    ToolStripMenuItem toolStripItem = new ToolStripMenuItem();//新创建一个下拉菜单项
                    toolStripItem.Name = PerCom;//
                    toolStripItem.CheckOnClick = true;
                    toolStripItem.Text = PerCom;//text
                    SerialSclectComBox.DropDownItems.Add(toolStripItem);//添加到下拉列表中
                }
            }
            catch
            {
                return;
            }
            //插入树节点
            LoadTreeNodes();
            flashToolStripMenuItem.SelectedIndex = 0;
        }
        //调整窗口大小
        private void Form1_Resize(object sender, EventArgs e)
        {
            FilePanel.Height += (this.Height - FilePanel.Height - 125);
            FiletreeView.Height += (this.Height - FiletreeView.Height - 150);
            ScriptPanel.Width += (this.Width - ScriptPanel.Width - 265);
            ScriptPanel.Height += (this.Height - ScriptPanel.Height - 125);
            ScriptTextBox.Width += (this.Width - ScriptTextBox.Width - 320);
            ScriptTextBox.Height += (this.Height - ScriptTextBox.Height - 150);
            RowNumber.Height += (this.Height - RowNumber.Height - 150);
        }
        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            foreach (string port in PortList.Keys)
            {
                PortList[port].Close();
            }
            PortList.Clear();
            SaveFile();
        }
        
        //连接串口
        private void ConnectSerialBtn_Click(object sender, EventArgs e)
        {
            string SerialStr = "当前连接串口：";
            foreach (ToolStripMenuItem Item in SerialSclectComBox.DropDownItems)
            {
                if(Item.Checked && (!PortList.ContainsKey(Item.Name)))
                {
                    SerialPort PortConnect = new SerialPort(Item.Name);
                    try
                    {
                        PortConnect.BaudRate = SerialArgs.BrauRate;//波特率
                        PortConnect.StopBits = SerialArgs.StopBit;//终止bit
                        PortConnect.Parity = SerialArgs.CheckBit;//
                        PortConnect.DataBits = SerialArgs.DataBit;
                        PortConnect.NewLine = "\n";
                        PortConnect.ReadTimeout = 2000;
                        PortConnect.WriteTimeout = 500;
                        PortConnect.WriteBufferSize = 4096;//4K
                        PortConnect.ReadBufferSize = 16384;//
                        PortConnect.Open();
                        //将串口添加到连接的串口列表中
                        PortList.Add(Item.Name, PortConnect);
                        List<string> tmp = new List<string>();
                        CmdList.Add(Item.Name, tmp);
                       
                    }
                    catch
                    {
                        MessageBox.Show(Item.Name + "connect error !!!","ERROR");
                        continue;
                    }
                }
            }
            foreach(string name in PortList.Keys)
            {
                SerialStr += (name + "  ");
            }
            //显示状态栏已连接串口
            toolStatusCom.Text = SerialStr;
        }
        //断开连接
        private void InterruptBtn_Click(object sender, EventArgs e)
        {
            SelectCom StopSerial = new SelectCom();
            StopSerial.ShowDialog();
            string temp = "当前已连接串口：";
            //修改串口下拉列表选择项
            foreach(ToolStripMenuItem Item in SerialSclectComBox.DropDownItems)
            {
                if(PortList.ContainsKey(Item.Text))
                {
                    Item.Checked = true;
                }
                else
                {
                    Item.Checked = false;
                }
            }
            //修改界面下方状态条显示的当前连接串口
            foreach(string port in PortList.Keys)
            {
                temp += (port + "  ");
            }
            toolStatusCom.Text = temp;      
        }
        //打开下拉框
        private void SerialSclectComBox_DropDownOpened(object sender, EventArgs e)
        {
            //每次点击串口选择下拉框时获取当前电脑上的com设备，并判断是否在下拉框中，如果没有，则添加
            List<string> ComList = new List<string>();
            ComList = DataProcess.GetSerialPortInfo();
            try
            {
                foreach (string PerCom in ComList)
                {
                    if (!SerialSclectComBox.DropDownItems.ContainsKey(PerCom))
                    {
                        ToolStripMenuItem toolStripItem = new ToolStripMenuItem();
                        toolStripItem.Name = PerCom;
                        toolStripItem.CheckOnClick = true;
                        toolStripItem.Text = PerCom;
                        SerialSclectComBox.DropDownItems.Add(toolStripItem);
                    }
                }
            }
            catch
            {
                return;
            }
        }
        //运行脚本
        //运行脚本需要判断当前连接了几个串口，然后针对于每个串口需要新建一个窗口，每个串口对应一接收数据的线程。
        //所有的串口发送数据只用一个线程，先解析脚本，将所有需要发送的数据添加到list中，然后循环发送所有的CMD。
        private void RunBtn_Click(object sender, EventArgs e)
        {
            if(PortList.Count == 0 )
            {
                MessageBox.Show("please open serial port first !!!", "WARNING", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            RunBtn.Enabled = false;
            SendToolStripButton.Enabled = false;
            //ManualFlag = -1;

            WriteDataList.Clear();
            PageLength.Clear();
            ComDataPath.Clear();

            //处理脚本
            if (!ScriptIdentify())
            {
                RunBtn.Enabled = true;
                SendToolStripButton.Enabled = true;
                return;
            }
            int i = 5,j = 5;
            //遍历每个串口
            foreach(SerialPort port in PortList.Values)
            {
                if(!SerialToPath.ContainsKey(port.PortName))
                {
                    //创建输出打印框
                    PrintLog PortLogBox = new PrintLog();
                    PortLogBox.Top = i;
                    PortLogBox.Left = j;
                    PortLogBox.Text = port.PortName;
                    PortLogBox.Show();
                    //创建写入本地文件句柄
                    CreateFileHandle(port.PortName);
                    //接收数据线程
                    BackgroundWorker backgroundWorker = new BackgroundWorker();
                    backgroundWorker.DoWork += new System.ComponentModel.DoWorkEventHandler(this.RecvData_DoWork);
                    SerialToTextBox Argument = new SerialToTextBox(port, PortLogBox.PrintLogBox);
                    backgroundWorker.RunWorkerAsync(Argument);
                    i += 20;
                    j += 20;
                }
                this.Activate();
            }

            flashToolStripMenuItem.Enabled = false;
            //发送数据线程
            BackgroundWorker SendWoker = new BackgroundWorker();
            SendWoker.DoWork += new System.ComponentModel.DoWorkEventHandler(this.SendData_DoWork);
            IsRecvSingle = true;
            SendWoker.RunWorkerAsync();
        }
        //暂停
        private void StopBtn_Click(object sender, EventArgs e)
        {
            foreach (string name in CmdList.Keys)
            {
                CmdList[name].Clear();
            }
            RunBtn.Enabled = true;
            SendToolStripButton.Enabled = true;
            flashToolStripMenuItem.Enabled = true;
            foreach (string port in PortList.Keys)
            {
                IsSendData[port] = true;
            }
        }
        //打开脚本
        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenScript();
        }
        //打开其它路径下的脚本
        private void OpenOtherFileBtn_Click(object sender, EventArgs e)
        {
            if (SaveFile())
            {
                DialogResult dr = OpenFileDialogFW.ShowDialog();
                if (dr != DialogResult.Cancel)
                {
                    string filename = OpenFileDialogFW.FileName;
                    string temp = File.ReadAllText(filename);
                    ScriptTextBox.Text = temp;
                    OldText = ScriptTextBox.Text;
                    SelectScript = filename;
                }
            }
        }
        //删除脚本
        private void DelToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if(ClickNode.Tag.ToString() == SelectScript)
            {
                ScriptTextBox.Clear();
                SelectScript = "";
                toolStatusScriptLabel.Text = string.Empty;
            }
            File.Delete(ClickNode.Tag.ToString());
            LoadTreeNodes();
        }
        //重命名
        private void ReNameToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ChangeFileName = ClickNode.Text;
            ClickNode.BeginEdit();
        }
        //编辑完节点名称之后
        private void FiletreeView_AfterLabelEdit(object sender, NodeLabelEditEventArgs e)
        {
            if(e.Label == null)
            {
                return;
            }
            string dest = string.Empty;
            if(Directory.Exists(ClickNode.Tag.ToString()))
            {
                dest = ClickNode.Parent.Tag.ToString() + @"/" + e.Label;
                string Transit = dest + "-temp";
                if(Transit.CompareTo(ClickNode.Tag.ToString())!= 0)
                {
                    Directory.Move(ClickNode.Tag.ToString(), Transit);
                }
                else
                {
                    Transit = dest + "-tmp";
                }
                Directory.Move(Transit, dest);
            }
            else
            {
                dest = ClickNode.Parent.Tag.ToString() + @"/" + e.Label + ".txt";
                File.Move(ClickNode.Tag.ToString(), dest);
            }
            ClickNode.Tag = dest;
        }
        //增加
        private void AddToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string Name = ClickNode.Tag.ToString() + @"/" + DateTime.Now.ToString("yyyy-MM-dd") + "-" +
               DateTime.Now.Hour.ToString() + "-" + DateTime.Now.Minute.ToString() + "-" + DateTime.Now.Second.ToString();
            File.Create(Name + ".txt").Close();
            LoadTreeNodes();
        }
        //新建文件夹
        private void FoldeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string Name = ClickNode.Tag.ToString() + @"/" + DateTime.Now.ToString("yyyy-MM-dd") + "-" +
              DateTime.Now.Hour.ToString() + "-" + DateTime.Now.Minute.ToString() + "-" + DateTime.Now.Second.ToString();
            try
            {
                Directory.CreateDirectory(Name);
            }
            catch
            {
                MessageBox.Show("Create File Error !!!", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            LoadTreeNodes(); 
           
        }
        //删除文件夹
        private void DelDirToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string path = ClickNode.Tag.ToString();
            if (Directory.Exists(path))
            {
                DirectoryInfo fDir = new DirectoryInfo(path);
                FileSystemInfo[] finfos = fDir.GetFileSystemInfos();
                foreach (FileSystemInfo f in finfos)
                {
                    f.Delete();
                }
                Directory.Delete(path);
            }
            LoadTreeNodes();
        }
        //刷新列表
        private void RushToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LoadTreeNodes();
        }
        //在mousedown和timer区分单击和双击事件中判断是单击还是双击
        private void FiletreeView_MouseDown(object sender, MouseEventArgs e)
        {

            Point ClickPoint = new Point(e.X, e.Y);
            ClickNode = FiletreeView.GetNodeAt(ClickPoint);
            if (e.Button == MouseButtons.Right)//鼠标右键单击
            {
                if (ClickNode != null)
                {
                    ClickNode.ContextMenuStrip = contextMenuStrip1;
                    if(!Directory.Exists(ClickNode.Tag.ToString()))
                    {
                        AddToolStripMenuItem.Enabled = false;
                        FoldeToolStripMenuItem.Enabled = false;
                        DelDirToolStripMenuItem.Enabled= false;
                        OpenToolStripMenuItem.Enabled = true;
                        DelToolStripMenuItem.Enabled = true;
                    }
                    else
                    {
                        AddToolStripMenuItem.Enabled = true;
                        FoldeToolStripMenuItem.Enabled = true;
                        DelDirToolStripMenuItem.Enabled = true;
                        OpenToolStripMenuItem.Enabled = false;
                        DelToolStripMenuItem.Enabled = false;
                    }
                }
            }
            else//鼠标左键单击
            {
                if (isFirstClick)//如果有第一次单击
                {
                    isFirstClick = false;
                    doubleClickRectangle = new Rectangle(
                        e.X - (SystemInformation.DoubleClickSize.Width / 2),
                        e.Y - (SystemInformation.DoubleClickSize.Height / 2),
                        SystemInformation.DoubleClickSize.Width,
                        SystemInformation.DoubleClickSize.Height);
                    Invalidate();

                    // Start the double click timer.
                    timer1.Start();
                }
                else
                {
                    if (doubleClickRectangle.Contains(e.Location) &&
                        milliseconds < SystemInformation.DoubleClickTime)
                    {
                        isDoubleClick = true;
                    }
                }
            }
        }
        
        private void OnChanged(object source, FileSystemEventArgs e)
        {
            Console.WriteLine("Watch Text Changed");
            //判断当前form是否处于窗口最前位置，即是否获得焦点，如果没有获取焦点，则进行文件的重载
            if (!this.Focused)
            {
                //重新加载这个文件
                ScriptTextBox.Clear();
                string temp = DataProcess.ReadScriptFile(SelectScript);
                ScriptTextBox.Text = temp;
                OldText = ScriptTextBox.Text;
                ScriptTextBox.SelectionStart = selectionStart;
                //Console.WriteLine("1"+SelectScript);
                //Console.WriteLine("2"+ClickNode.Tag.ToString());
            }
            
        }
        private void timer1_Tick(object sender, EventArgs e)
        {
            milliseconds += 100;
            if (milliseconds >= SystemInformation.DoubleClickTime)//如果当前的计时大于系统能判定的双击间隔时间，则进行进行双击的处理
            {
                timer1.Stop();//停止该计时器
                if (isDoubleClick)//如果该标志举起，则说明此次单击判为双击
                {
                    if (ClickNode == null || ClickNode.Nodes.Count > 0)
                    {
                        isFirstClick = true;
                        isDoubleClick = false;
                        milliseconds = 0;
                        return;
                    }
                    OpenScript();//打开脚本
                }
                isFirstClick = true;//重新将单击标志支起
                isDoubleClick = false;//重新将双击标志清除
                milliseconds = 0;//同时从当前单击的时间节点重新开始计时
            }
        }
        //保存文件（菜单、快捷键）
        private void SaveToolStripMenuItem_Click(object sender, EventArgs e)
        {
           if (SelectScript != string.Empty && ScriptTextChange())
            {
                OldText = ScriptTextBox.Text;//保存旧文本的内容
                //获取旧文本中光标的位置，以便在重新加载文件后将光标聚焦到指定位置
               // selectionStart = ScriptTextBox.SelectionStart;
                DataProcess.ModifyScriptText(SelectScript, OldText);
            }
        }
        //串口配置
        private void SetUp_Click(object sender, EventArgs e)
        {
            ChangeArgs args = new ChangeArgs();
            args.ShowDialog();
        }
        //绘制行号调用
        //文本变化时的重绘
        private void ScriptTextBox_TextChanged(object sender, EventArgs e)
        {
            Console.WriteLine("OnTextChanged");
            
            showLineNo();
        }
        //垂直滚动条位置改变时的重绘
        private void ScriptTextBox_VScroll(object sender, EventArgs e)
        {
            Console.WriteLine("OnVScroll");
            showLineNo();
        }
        //Form1绘制窗口时的重绘
        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            Console.WriteLine("OnPaint");
            showLineNo();
        }
        //about
        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string about = "ICMAX flash analysis V1.01";
            MessageBox.Show(about,"about",MessageBoxButtons.OK,MessageBoxIcon.Information);
        }

        private void ScriptTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F5 && e.Control)
            {
                RunBtn_Click(null, null);
            }
            else if (e.Control && e.KeyCode == Keys.V)
            {
                e.SuppressKeyPress = true;
                ScriptTextBox.Paste(DataFormats.GetFormat(DataFormats.Text));
            }
        }
        private void ScriptTextBox_SelectionChanged(object sender, EventArgs e)
        {
            int selectionStart = ScriptTextBox.SelectionStart;
            int index = ScriptTextBox.GetFirstCharIndexOfCurrentLine();
            int row = ScriptTextBox.GetLineFromCharIndex(selectionStart)+1;
            int column = selectionStart - index + 1;
            Console.WriteLine(row.ToString()+","+column.ToString());
        }
        //选择flash
        private void flashToolStripMenuItem_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(flashToolStripMenuItem.SelectedIndex == 0)
            {
                FlashManualSize = 38 * 512;
                FlashDmaSize = 32 * 512;
                FlashFlag = 0;
            }
            else if(flashToolStripMenuItem.SelectedIndex == 1)
            {
                FlashManualSize = 36 * 512;
                FlashDmaSize = 32 * 512;
                FlashFlag = 1;
            }
        }
        //选择固件
        private void SelectFWfileBtn_Click(object sender, EventArgs e)
        {
           
            if (DialogResult.OK == SelectFWdialog.ShowDialog())
            {
                if (SelectFWdialog.FileNames.Count() != 2)
                {
                    MessageBox.Show("选择文件数量不对！");
                    return;
                }
                FileInfo info1 = new FileInfo(SelectFWdialog.FileNames[0]);
                FileInfo info2 = new FileInfo(SelectFWdialog.FileNames[1]);
                if (info1.Name.Substring(0,2).CompareTo("DC") == 0 && info2.Name.Substring(0,2).CompareTo("SB")== 0)
                {
                    FileStream stream1 = new FileStream(SelectFWdialog.FileNames[0], FileMode.Open, FileAccess.Read);
                    FileStream stream2 = new FileStream(SelectFWdialog.FileNames[1], FileMode.Open, FileAccess.Read);
                    BinaryReader Reader1 = new BinaryReader(stream1);
                    BinaryReader Reader2 = new BinaryReader(stream2);
                    Reader1.Read(FWfile, 0, 20 * 1024);
                    Reader2.Read(FWfile, 20 * 1024, 8 * 1024);
                    Reader1.Close();
                    Reader2.Close();
                    stream1.Close();
                    stream2.Close();
                }
                else if (info2.Name.CompareTo("DCP02Image_DCP02_UpdateCP_B16A.bin") == 0 && info1.Name.CompareTo("SB_MT29F128G08EBCDB_016I02P16_20190816_ALL.bin") == 0)
                {
                    FileStream stream1 = new FileStream(SelectFWdialog.FileNames[1], FileMode.Open, FileAccess.Read);
                    FileStream stream2 = new FileStream(SelectFWdialog.FileNames[0], FileMode.Open, FileAccess.Read);
                    BinaryReader Reader1 = new BinaryReader(stream1);
                    BinaryReader Reader2 = new BinaryReader(stream2);
                    Reader1.Read(FWfile, 0, 20 * 1024);
                    Reader2.Read(FWfile, 20 * 1024, 8 * 1024);
                    Reader1.Close();
                    Reader2.Close();
                    stream1.Close();
                    stream2.Close();
                }
                else
                {
                    MessageBox.Show("文件选择不正确，请重新选择！", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                SendToolStripButton.Enabled = true;
            }
        }
        //发送FW
        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            StopBtn_Click(null, null);
            BackgroundWorker backgroundWorker = new BackgroundWorker();
            backgroundWorker.DoWork += new System.ComponentModel.DoWorkEventHandler(this.SendFWData_DoWork);
            backgroundWorker.RunWorkerAsync();
        }
        private void SendFWData_DoWork(object sender, DoWorkEventArgs e)
        {
            this.BeginInvoke(new System.Threading.ThreadStart(delegate ()
            {
                RunBtn.Enabled = false;//禁用运行按钮
                SendToolStripButton.Enabled = false;//禁用发送按钮

            }));
            foreach (SerialPort port in PortList.Values)
            {
                port.WriteLine("flash recvfw 1");
                Thread.Sleep(100);
                for (int i = 0; i < 28 / 2; i++)
                {
                    port.Write(FWfile, i * 2048, 2048);
                    Thread.Sleep(500);
                }
                this.BeginInvoke(new System.Threading.ThreadStart(delegate ()
                {
                    RunBtn.Enabled = true;
                    SendToolStripButton.Enabled = true;

                }));
            }
        }
    }
}