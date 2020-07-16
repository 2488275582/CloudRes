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
using System.Text.RegularExpressions;

namespace FlashAnalyze
{
    public partial class Form1 : Form
    {
        //线程执行读取串口数据事件
        private void RecvData_DoWork(object sender, DoWorkEventArgs e)
        {
            SerialToTextBox Example = (SerialToTextBox)e.Argument;
            Example.Port.DiscardOutBuffer();//先将串口驱动中的缓冲数据丢弃
            string NextBuf = string.Empty;
            byte[] DataBuff = new byte[51200];
            int index = 0;
            while (Example.Port.IsOpen)//如果该串口处于打开/连接状态，则进行数据的接收
            {
                
                int n = Example.Port.BytesToRead;//字节数
                if (n > 0)
                {
                    //Console.WriteLine(n);
                    byte[] buf = new byte[n];//获取到串口缓冲区上接收到的字节数量
                    Example.Port.Read(buf, 0, n);//buffer offset count
                    Console.WriteLine(buf.Length);
                    DealRecvData(buf, ref NextBuf, ref index, Example, ref DataBuff);//根据具体需求从这些sector中分别读取PageData和其他的统计数据
                }
                else
                {
                    Thread.Sleep(300);
                }
            }
        }
        /**
         * Data:从串口缓冲区读上来的字节数
         * NextBuffer:指向一个空的字符串
         * index:索引值，指向当前DataBuffer这个目标字节数组的偏移量
         * Example:串口结构体
         * Databuffer:目标字节数组
         */
         //每一次解析一个数据包
        private void DealRecvData(byte[] Data,ref string NextBuffer,ref int index, SerialToTextBox Example,ref byte[] DataBuffer)
        {
            string StartFlag = "SINGLE_START_S_S";
            if (Data.Length == 0)
            {
                return;
            }
            else if(IsPageRead[Example.Port.PortName])//每个串口的接收数据的标志
            {

                //发送过readpage CMD，解析接收到的数据，收到开始写入数据后将数据写入
                if(ComDataStart[Example.Port.PortName])
                {
                    if (ComWriteNum[Example.Port.PortName] + Data.Length < ReadPageNum)//串口接收的数据没达到pagesize
                    {
                        Buffer.BlockCopy(Data, 0, DataBuffer, ComWriteNum[Example.Port.PortName], Data.Length);//追加
                        ComWriteNum[Example.Port.PortName] += Data.Length;//长度递增
                    }
                    else if (ComWriteNum[Example.Port.PortName] + Data.Length == ReadPageNum)//如果达到pagesize则进行文件的写入
                    {
                        Buffer.BlockCopy(Data, 0, DataBuffer, ComWriteNum[Example.Port.PortName], Data.Length);
                        PageDataWrite(ComDataPath[Example.Port.PortName].FirstOrDefault(), DataBuffer);
                        //串口对应的路径删除
                        ComDataPath[Example.Port.PortName].RemoveAt(0);
                        //串口对应的当前写入长度置0
                        ComWriteNum[Example.Port.PortName] = 0;
                        //串口对应的开始信号关闭
                        ComDataStart[Example.Port.PortName] = false;
                        //串口对应的pageread的信号关闭
                        IsPageRead[Example.Port.PortName] = false;
                        if(CmdList[Example.Port.PortName].Count == 0)//没有要发送的函数指令
                        {
                            IsRecvSingle = false;
                            //IsSendData[Example.Port.PortName] = false;
                        }
                    }
                    else//此次接收完数据后，大于pagesize
                    {
                        int num = ReadPageNum - ComWriteNum[Example.Port.PortName];//剩余量
                        Buffer.BlockCopy(Data, 0, DataBuffer, ComWriteNum[Example.Port.PortName], num);//src,srcOffset,dst,dstOffset,count
                        //写入文件,只写一个page的数据
                        PageDataWrite(ComDataPath[Example.Port.PortName].FirstOrDefault(), DataBuffer);
                        //移除输出路径
                        ComDataPath[Example.Port.PortName].RemoveAt(0);
                        //待写长度置0
                        ComWriteNum[Example.Port.PortName] = 0;
                        //数据接收开始信号关闭
                        ComDataStart[Example.Port.PortName] = false;
                        //发送pageread信号关闭
                        IsPageRead[Example.Port.PortName] = false;

                        byte[] temp = new byte[Data.Length - num];
                        Buffer.BlockCopy(Data, num, temp, 0, Data.Length - num);
                        NextBuffer += Encoding.ASCII.GetString(temp);
                        RecvNextCmd(ref NextBuffer, Example);
                        if (CmdList[Example.Port.PortName].Count == 0)
                        {
                            IsRecvSingle = false;
                            //IsSendData[Example.Port.PortName] = false;
                        }
                    }
                }
                else
                {
                    for (int i = 0; i < Data.Length; i++)
                    {
                        if(index == 16)
                        {
                            index = 0;
                            ComDataStart[Example.Port.PortName] = true;//匹配到完整的StartFlag后，将接受数据写入文件的标志置为True
                            int count = Data.Length - i > ReadPageNum ? ReadPageNum : Data.Length - i;
                            Buffer.BlockCopy(Data, i, DataBuffer, 0, count);//只拷贝一个Page的数据到DataBuffer上
                            ComWriteNum[Example.Port.PortName] += count;
                            if (count == ReadPageNum)
                            {
                                PageDataWrite(ComDataPath[Example.Port.PortName].FirstOrDefault(), DataBuffer);
                                
                                ComDataPath[Example.Port.PortName].RemoveAt(0);

                                ComWriteNum[Example.Port.PortName] = 0;

                                ComDataStart[Example.Port.PortName] = false;//串口读取数据标志置为0

                                IsPageRead[Example.Port.PortName] = false;//串口接收数据标志置为0

                                byte[] tmpByte = new byte[Data.Length - ComWriteNum[Example.Port.PortName] - i];
                                Buffer.BlockCopy(Data, ReadPageNum + i, tmpByte, 0, Data.Length - ReadPageNum - i);
                                NextBuffer += Encoding.ASCII.GetString(tmpByte);
                                if (CmdList[Example.Port.PortName].Count == 0)
                                {
                                    IsRecvSingle = false;
                                    //IsSendData[Example.Port.PortName] = false;
                                } 
                            }
                            break;
                        }
                        if (Data[i] == Convert.ToByte(StartFlag[index]))
                        {
                            index++;
                        }
                        else
                        {
                            index = 0;
                        }
                        NextBuffer += (char)Data[i];
                    }
                    RecvNextCmd(ref NextBuffer, Example);
                }
            }
            else//判断是否接收到下一个信号以及将数据委托主线程打印
            {
                NextBuffer += Encoding.ASCII.GetString(Data);
                RecvNextCmd(ref NextBuffer, Example);
            }
        }
        
        //判断是否收到了下一个cmd的命令
        private void RecvNextCmd(ref string NextBuffer, SerialToTextBox Example)
        {
            if (NextBuffer.Contains("SINGLE_START_S_N"))
            {
                lock (locker)
                {
                    IsSendData[Example.Port.PortName] = true;
                    IsRecvSingle = true;
                }
                LogPrint(NextBuffer, Example);
                NextBuffer = string.Empty;
            }  
        }
        //打印log，以及将log写入文件
        private void LogPrint(string Log, SerialToTextBox Example)
        {
            string temp = Regex.Replace(Log, @"SINGLE_START_S_N", "");
            string result = Regex.Replace(temp, @"SINGLE_START_S_S", "");
            temp = Regex.Replace(result, @"SINGLE_START_S_R", "");
            //委托主线程对UI控件进行操作
            this.BeginInvoke(new System.Threading.ThreadStart(delegate ()
            {
                if(Example.PrintBox.Text.Length > 0xFFFF)
                {
                    Example.PrintBox.Clear();
                }
                Example.PrintBox.Text += temp;
            }));
            try
            {
                string time = "[" + DateTime.Now.ToString() + "." + DateTime.Now.Millisecond.ToString("000") + "] ";
                //使用正则表达式在所有的换行符后加入当前时间戳 
                string WriteText = Regex.Replace(temp, @"\n", "\n" + time);
                File.AppendAllText(SerialToPath[Example.Port.PortName], WriteText);
            }
            catch
            {
                return;
            }
        }
        //获取写入文件的偏移量
        //-1时代表追加，大于等于0时，偏移量的单位为一个page
        private int GetIndexOffert(long size,int offset)
        {
            int index = 0;
            if (offset == -1)
            {
                index = (int)size / ReadPageNum;
            }
            else if (offset > size/ ReadPageNum)
            {
                index = (int)size / ReadPageNum;
            }
            else
            {
                index = offset;
            }
            return index;
        }
        //写一个Page的数据到输出路径下
        private void PageDataWrite(PathAndOffert FileName, byte[] Data)
        {
            int flag = FileName.Offert;//脚本中offset参数
            ReadPageNum /= FileName.PlaneNum;//脚本中的plane数量，最多支持4plane
            for(int i =0; i< FileName.PlaneNum; i++)
            {
                string FilePath = FileName.FilePath[i];
                FileInfo fileInfo = new FileInfo(FilePath);//输出文件信息
                long size = fileInfo.Length;//输出文件初始长度
                int offert = GetIndexOffert(size, flag);

                if (size == 0)//文件当前为空
                {
                    FileStream WriteStream = new FileStream(FilePath, FileMode.Append, FileAccess.Write);
                    BinaryWriter Writer = new BinaryWriter(WriteStream);
                    Writer.Write(Data, i * ReadPageNum, ReadPageNum);
                    Writer.Close();
                    WriteStream.Close();
                }
                else//文件当前长度不为0
                {
                    if (offert * ReadPageNum >= size)//偏移量大于文件长度，转为追加
                    {
                        FileStream WriteStream = new FileStream(FilePath, FileMode.Append, FileAccess.Write);
                        BinaryWriter Writer = new BinaryWriter(WriteStream);
                        Writer.Write(Data, i * ReadPageNum, ReadPageNum);
                        Writer.Close();
                        WriteStream.Close();
                    }
                    else//在文件中进行插入
                    {
                        string NewFile = fileInfo.DirectoryName + @"/MyTempNewPageYang.bin";
                        FileStream ReadStream = new FileStream(FilePath, FileMode.Open, FileAccess.Read);
                        BinaryReader Reader = new BinaryReader(ReadStream);
                        FileStream WriteStream = new FileStream(NewFile, FileMode.Create, FileAccess.Write);//创建一个临时文件接受数据
                        BinaryWriter Writer = new BinaryWriter(WriteStream);
                        byte[] tmpData = new byte[ReadPageNum];
                        for (int j = 0; j < offert; j++)//先将offset前的原始文件拷贝到临时文件
                        {
                            Reader.Read(tmpData, 0, ReadPageNum);
                            Writer.Write(tmpData, 0, ReadPageNum);
                        }
                        Writer.Write(Data, i * ReadPageNum, ReadPageNum);//再将Data中的一个page的数据写到临时文件
                        for (int j = offert; j < size / ReadPageNum; j++)//再将原始文件Offset后面的数据拷贝到临时文件中去
                        {
                            Reader.Read(tmpData, 0, ReadPageNum);
                            Writer.Write(tmpData, 0, ReadPageNum);
                        }
                        Reader.Close();
                        ReadStream.Close();
                        Writer.Close();
                        WriteStream.Close();
                        File.Delete(FilePath);
                        File.Move(NewFile, FilePath);//再将临时文件和原始文件进行替换
                    }
                }
            }
        }
        //发送数据辅助线程执行代码
        private void SendData_DoWork(object sender, DoWorkEventArgs e)
        {
            string str = "flash start 1 " + FlashFlag + '\n';
            //首先发送start验证是否上电
            foreach (SerialPort port in PortList.Values)
            {
                CmdList[port.PortName].Insert(0, str);
            }
            Thread.Sleep(200);
            int count = 0;
            while (true)
            {
                try
                {
                    foreach (string name in PortList.Keys)//对于每一个串口
                    {
                        if (CmdList[name].Count == 0)//如果该串口要发送的指令数量为0
                        {
                            this.BeginInvoke(new System.Threading.ThreadStart(delegate ()
                            {
                                RunBtn.Enabled = true;
                                flashToolStripMenuItem.Enabled = true;
                                SendToolStripButton.Enabled = true;
                            }));
                            count++;
                        }
                        if (count == CmdList.Count)
                        {
                            return;
                        }
                    }
                    if (IsRecvSingle)
                    {
                        lock (locker)
                        {
                            IsRecvSingle = false;
                        }
                        //轮询所有串口判断是否需要发送数据，
                        string tmpCmd = string.Empty;
                        Thread.Sleep(100);
                        foreach (SerialPort port in PortList.Values)
                        {
                            if (IsSendData[port.PortName])
                            {
                                tmpCmd = CmdList[port.PortName].FirstOrDefault();
                                port.Write(tmpCmd);//打印当前执行的函数要下发的命令
                                lock (locker)
                                {
                                    IsSendData[port.PortName] = false;//处理一个要发送数据的指令的时候，将发送信号置为false
                                }
                                IsPageRead[port.PortName] = false;//
                                if (tmpCmd.Substring(6, 4).CompareTo("page") == 0)//flash pageprogram
                                {
                                    Thread.Sleep(100);
                                    WritePageNum = WriteDataList[port.PortName].FirstOrDefault().ProgramPageSize;//获取当前串口要编程的字节数
                                    byte[] tmpBuf = new byte[WritePageNum];
                                    FileStream stream = null;
                                    BinaryReader reader = null;
                                    try
                                    {
                                        stream = new FileStream(
                                        WriteDataList[port.PortName].FirstOrDefault().FilePath[0], FileMode.Open);
                                        int Seek = GetFileOffert(WriteDataList[port.PortName].FirstOrDefault().FilePath[0],
                                            WriteDataList[port.PortName].FirstOrDefault().Offert);
                                        if(Seek != 0)
                                        {
                                            stream.Seek(Seek, SeekOrigin.Begin);//指定stream的偏移量
                                        }
                                        FileInfo info = new FileInfo(WriteDataList[port.PortName].FirstOrDefault().FilePath[0]);//获取源文件的info
                                        reader = new BinaryReader(stream);
                                        if (info.Length < WritePageNum)
                                        {
                                            reader.Read(tmpBuf, 0, (int)info.Length);
                                        }
                                        else
                                        {
                                            reader.Read(tmpBuf, 0, (int)WritePageNum);
                                        }                                       
                                        reader.Close();
                                        stream.Close();

                                    }
                                    catch(Exception ex)
                                    {
                                    }
                                    
                                    for (int i = 0; i < WritePageNum / 1024; i++)
                                    {
                                        port.Write(tmpBuf, i * 1024, 1024);//每次通过串口发送1K数据
                                        Thread.Sleep(200);
                                    }
                                    WriteDataList[port.PortName].RemoveAt(0);
                                }
                                else if (tmpCmd.Substring(6, 4).CompareTo("read") == 0)//发送的"flash readpage"
                                {
                                    lock (locker)
                                    {
                                        ReadPageNum = PageLength[0];//当前要读的字节数
                                        IsPageRead[port.PortName] = true;//读页信号置为true，详见另一个接收数据线程
                                        PageLength.RemoveAt(0);//当前要读的字节数
                                    }
                                }
                            }
                            CmdList[port.PortName].RemoveAt(0);
                        }
                    }
                }
                
                catch
                {
                    continue;
                }
                
            }    
        }

        //串口的日志文件和一些串口先关属性的字典
        private void CreateFileHandle(string port)
        {
            string time = DateTime.Now.ToString("yyyy-MM-dd") + "-" +
                DateTime.Now.Hour.ToString() + "-" + DateTime.Now.Minute.ToString() + "-" + DateTime.Now.Second.ToString();
            string path = logFilePath +  time + port + ".txt";

            SerialToPath.Add(port, path);//保存串口对应的文件到字典
            ComDataStart.Add(port, false);//串口是否接受数据的标志字典
            ComWriteNum.Add(port, 0);//串口写入长度字典
            IsPageRead.Add(port, false);//串口是否需要接受数据字典
            if(!IsSendData.ContainsKey(port))
            {
                IsSendData.Add(port, true);//串口当前是否需要发送数据 字典
            }
        }
        //加载脚本文件
        private void LoadTreeNodes()
        {
            FiletreeView.Nodes.Clear();
            TreeNode root = new TreeNode();
            root.Text = @"Script";
            root.Tag = @"./Script";
            root.ImageIndex = 0;
            root.SelectedImageIndex = root.ImageIndex;
            FiletreeView.Nodes.Add(root);
            InsertTreeNode(root);
            this.FiletreeView.ExpandAll();
        }
        //插入节点
        private void InsertTreeNode(TreeNode fNode)
        {
            string path = fNode.Tag.ToString();
            //父目录
            DirectoryInfo fDir = new DirectoryInfo(path);
            FileSystemInfo[] finfos = fDir.GetFileSystemInfos();
            foreach (FileSystemInfo f in finfos)
            {
                string type = f.GetType().ToString();
                TreeNode node = new TreeNode();
                string[] tempStr = f.Name.Split('.');
                node.Text = tempStr[0];
                node.Tag = f.FullName;

                fNode.Nodes.Add(node);
                if ("System.IO.DirectoryInfo" == type) //是文件夹时才递归调用自己
                {
                    InsertTreeNode(node);
                }
                else
                {
                    node.ImageIndex = 1;
                    node.SelectedImageIndex = node.ImageIndex;
                }
            }
        }
        //打开脚本
        private void OpenScript()
        {
           if (SaveFile())
            {
                SelectScript = ClickNode.Tag.ToString();
                ScriptTextBox.Clear();
                string temp = DataProcess.ReadScriptFile(ClickNode.Tag.ToString());
                ScriptTextBox.Text = temp;
                OldText = ScriptTextBox.Text;
                ScriptNameLabel.Text = ClickNode.Text;
            }
            
            ScriptWatcher.Path = ClickNode.Parent.Tag.ToString();//设置监听的目录
            ScriptWatcher.Filter = "*.txt";//设置监听的文件
            ScriptWatcher.NotifyFilter = NotifyFilters.LastWrite;//监听的动作类型，多种类型用或|连接
            ScriptWatcher.Changed += new FileSystemEventHandler(OnChanged);//设置文件更改回调函数
            ScriptWatcher.EnableRaisingEvents = true;
        }
        //确认是否保存脚本文件
        private bool SaveFile()
        {
            if (SelectScript != string.Empty && ScriptTextChange())
            {
                DialogResult dr = MessageBox.Show("是否保存更改脚本文件？", "Script", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
                if (dr == DialogResult.Yes)
                {
                    DataProcess.ModifyScriptText(SelectScript, ScriptTextBox.Text);
                }
                else if(dr == DialogResult.Cancel)
                {
                    return false;
                }
            }
            return true;
        }
        //判断脚本文件内容是否改变
        private bool ScriptTextChange()
        {
            bool ScriptTextChange = false;
            string Present = ScriptTextBox.Text;
            //选中的脚本不为空 点击的节点不为空（树节点的父节点或者叶子结点）
            if (SelectScript != string.Empty && ClickNode!= null && ClickNode.Nodes.Count == 0)
            {   
                //旧文本为空，当前文本内容为空，则Flag为False
                if (OldText == string.Empty && Present == string.Empty)
                {
                    return false;
                }
                //旧文本不为空，当前文本为空，Flag为True
                else if (OldText != string.Empty && Present == string.Empty)
                {
                    ScriptTextChange = true;
                }
                //旧文本为空，当前文本不为空，Flag为True
                else if (OldText == string.Empty && Present != string.Empty)
                {
                    ScriptTextChange = true;
                }
                //旧文本不等于当前文本，Flag为False
                else if (OldText.CompareTo(Present) != 0)
                {
                    ScriptTextChange = true;
                }
                //其他情况，默认Flag为False，即没有改变
            }
            
            return ScriptTextChange;
        }
        //处理脚本
        private bool ScriptIdentify()
        {
            Lua lua = new Lua();
            string FunName = string.Empty;
            string Connects = ScriptTextBox.Text;
            //编译
            if(Connects == string.Empty)
            {
                return false;
            }
            Form1 obj = new Form1();
            lua.RegisterFunction("ReadID", obj, obj.GetType().GetMethod("ReadID"));
            lua.RegisterFunction("EraseBlock", obj, obj.GetType().GetMethod("EraseBlock"));
            lua.RegisterFunction("BlockProgram", obj, obj.GetType().GetMethod("BlockProgram"));
            lua.RegisterFunction("GetFeature", obj, obj.GetType().GetMethod("GetFeature"));
            lua.RegisterFunction("PageRead", obj, obj.GetType().GetMethod("PageRead"));
            lua.RegisterFunction("PageProgram", obj, obj.GetType().GetMethod("PageProgram"));
            lua.RegisterFunction("SetFeature", obj, obj.GetType().GetMethod("SetFeature"));
            lua.RegisterFunction("Printf", obj, obj.GetType().GetMethod("Print"));
            lua.RegisterFunction("Init", obj, obj.GetType().GetMethod("InitToDebug"));
            lua.RegisterFunction("TimesPageRead", obj, obj.GetType().GetMethod("TimesPageRead"));
            //执行
            try
            {
                lua.DoString(Connects);
            }
            catch(Exception ex)
            {
                string error;
                if(ex.InnerException != null)
                {
                    error = ex.Source + '\n' + ex.InnerException.Message;
                }
                else
                {
                    error = ex.Source + '\n' + ex.Message;
                }
                MessageBox.Show(error, "语法错误！！",MessageBoxButtons.OK);
                foreach(string name in CmdList.Keys)
                {
                    CmdList[name].Clear();
                }
                return false;
            }
            return true;
        }
        //功能执行函数
        public void ReadID(int Chan,int Chip)
        {
            string temp = "flash idread " + Chan.ToString() + " " + Chip.ToString() + '\n';
            foreach (string name in CmdList.Keys)
            {
                CmdList[name].Add(temp);
            }
        }
        public void EraseBlock(int Chan, int Chip, int Mode, string Block,int DelayTime,int Reset_flag)
        {
            if (Reset_flag != 0 && Reset_flag != 1)
            {
                throw new Exception("Reset_flag只能为0或者1");
            }
            string temp = "flash blockerase " + Chan.ToString() + " " + Chip.ToString() + " "
                + Mode.ToString() + " " + Block +" "+DelayTime.ToString() + " " + Reset_flag.ToString()+'\n';
            foreach (string name in CmdList.Keys)
            {
                CmdList[name].Add(temp);
            }
        }
        public void BlockProgram(int Chan, int Chip, int Mode, string Block, int SeedStart,int Manual,int CMD1Code,int CMD1Flag,int SendData)
        {
            /*
            if(ManualFlag == -1)
            {
                ManualFlag = Manual;
            }
            else
            {
                if(ManualFlag != Manual)
                {
                    throw new Exception("当前脚本中只能包含一种SDMA或Manual方式！！！");
                }
            }
            */
            if (Manual == 0 && SendData == 0)
            {
                throw new Exception("暂只支持Manual方式不发数据！");
            }
            Int32 args = ((Chan & 0xFF) << 24) | ((Chip & 0xFF) << 16) | ((Mode & 0xFF) << 8) | (Manual & 0xFF);
            Int32 args1 = ((CMD1Code & 0xFF) << 24) | ((CMD1Flag & 0xFF) << 16) | ((SendData & 0xFF) << 8);
            string temp = "flash blockprogram " + " " + Block + " " + SeedStart.ToString() + " " + args.ToString("X") + " "+args1.ToString("X")+'\n';
            foreach (string name in CmdList.Keys)
            {
                CmdList[name].Add(temp);
            }
        }
        public void GetFeature(int Chan, int Chip, int FeatureAddress)
        {
            string temp = "flash getfeature " + Chan.ToString() + " " + Chip.ToString() + " " + FeatureAddress.ToString() + '\n';
            foreach (string name in CmdList.Keys)
            {
                CmdList[name].Add(temp);
            }
        }
        /**
         * 参数个数：13
         */
        public void PageRead(int Chan, int Chip, int Mode, string Block, int PageAddr, int PageNum, string OutPath, int Offset, int Manual, int SaveFlag,int From,int Column_Flag,int CMD1,int RB_Flag)
        {
            /*
            if (ManualFlag == -1)
            {
                ManualFlag = Manual;
            }
            else
            {
                if (ManualFlag != Manual)
                {
                    throw new Exception("当前脚本中只能包含一种SDMA或Manual方式！！！");
                }
            }
            /*
            if(PageNum == 0)
            {
                throw new Exception("读取的page数量不能为0！");
            }*/
            //FromCache = 0,不发00-30h指令，直接读取寄存器上的数据，
            //FromArray = 1,先从Array读到cache
            if(RB_Flag!=1 && RB_Flag != 0)
            {
                throw new Exception("RBFlag只支持0或者1！");
            }
            if(From !=1 && From !=0)
            {
                throw new Exception("From参数只能为0或者1！");
            }
            if (Column_Flag != 0 && Column_Flag != 1) 
            {
                throw new Exception("Column_Flag参数只能为0或者1！");
            }
            //if(CMD1 != 0x30 && CMD1 != 0x31)
            //{
            //    throw new Exception("Readpage的CMD1只支持30h或31h");
            //}
            if (SaveFlag == 1 && PageNum > 1)
            {
                throw new Exception("数据传输仅支持1个page读！");
            }
            if (FlashFlag == 0)//B05A
            {
                if (Mode == 0 && ((PageAddr + PageNum) > 768))//TLC
                {
                    throw new Exception("page数量超出范围！");
                }
                else if (Mode == 1 && (PageAddr + PageNum > (768 / 3)))//SLC
                {
                    throw new Exception("page数量超出范围！");

                }
            }
            else if(FlashFlag == 1)//GCG
            {
                if (Mode == 0 && (PageAddr + PageNum > 792))//MLC
                {
                    throw new Exception("page数量超出范围！");
                }
                else if (Mode == 1 && ((PageAddr + PageNum) > (792 / 2)))//SLC
                {
                    throw new Exception("page数量超出范围！");
                }
            }

            if ((PageAddr + PageNum) > 792)
            {
                throw new Exception("page数量超出范围！");
            }
            //进行参数的拼接:31-24为channel 23-16为chip 15-8为SLC(1)|MLC(0) 7-0为Manual(1)|DMA(0)
            Int32 args = ((Chan & 0xFF) << 24) | ((Chip & 0xFF) << 16) | ((Mode & 0xFF) << 8) | (Manual & 0xFF);
            //args1: RB_flag|column_flag | CMD1
            Int32 args1 = ((RB_Flag & 0xFF) << 16) | ((Column_Flag & 0xFF) << 8) | (CMD1 & 0xFF);
            //Console.WriteLine(Column_Flag+" "+CMD1);
            string temp = "flash readpage " + Block + " " + PageAddr.ToString() + " " + PageNum.ToString() + " " + args.ToString("X") + " " + SaveFlag .ToString()+" "+ From.ToString()+" "+ args1.ToString("X")+'\n';
            foreach (string name in CmdList.Keys)
            {
                CmdList[name].Add(temp);
            }
            int num = 0;
            if (Manual == 1)
            {
                num = FlashManualSize;
            }
            else if (Manual == 0)
            {
                num = FlashDmaSize;
            }
            else
            {
                throw new Exception("SDMA或Manual方式错误！！！");
            }
            //如果Block中存在","逗号就说明是多plane，此时需要禁用读Block和读到SD卡中
            if(((Regex.Matches(Block, ",").Count ) > 0))
            {
                if(PageNum > 1)
                {
                    throw new Exception("多plane读方式只支持读取一个page");
                }
                else if(SaveFlag == 0)
                {
                    throw new Exception("多plane读方式不支持写入SD卡！！！");
                }
            }
            //读的大小为两个Block
            num *= (Regex.Matches(Block, ",").Count + 1);
            PageLength.Add(num);
            ReadPageNum = PageLength[0];
            try
            {
                if(SaveFlag == 1)
                {
                    FileExsits(OutPath, Offset, Block,RB_Flag);//保存到本地时需要对脚本中的输出路径进行验证
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public void TimesPageRead(int Chan, int Chip, int Mode, string Block, int PageAddr, int PageNum,int Manual,int From, int CMD1,int Times)
        {
            if (From != 1 && From != 0)
            {
                throw new Exception("From参数只能为0或者1！");
            }
            //if (Column_Flag != 0 && Column_Flag != 1)
            //{
            //    throw new Exception("Column_Flag参数只能为0或者1！");
            //}
            if (FlashFlag == 0)//B05A
            {
                if (Mode == 0 && ((PageAddr + PageNum) > 768))//TLC
                {
                    throw new Exception("page数量超出范围！");
                }
                else if (Mode == 1 && (PageAddr + PageNum > (768 / 3)))//SLC
                {
                    throw new Exception("page数量超出范围！");

                }
            }
            else if (FlashFlag == 1)//GCG
            {
                if (Mode == 0 && (PageAddr + PageNum > 792))//MLC
                {
                    throw new Exception("page数量超出范围！");
                }
                else if (Mode == 1 && ((PageAddr + PageNum) > (792 / 2)))//SLC
                {
                    throw new Exception("page数量超出范围！");
                }
            }

            if ((PageAddr + PageNum) > 792)
            {
                throw new Exception("page数量超出范围！");
            }
            //进行参数的拼接:31-24为channel 23-16为chip 15-8为SLC(1)|MLC(0) 7-0为Manual(1)|DMA(0)
            Int32 args = ((Chan & 0xFF) << 24) | ((Chip & 0xFF) << 16) | ((Mode & 0xFF) << 8) | (Manual & 0xFF);
            //args1:Times | CMD1
            Int32 args1 = ((Times & 0xFF) << 8) | (CMD1 & 0xFF);
            //Console.WriteLine(Column_Flag+" "+CMD1);
            string temp = "flash timesreadpage " + Block + " " + PageAddr.ToString() + " " + PageNum.ToString() + " " + args.ToString("X") + " " + From.ToString() + " " + args1.ToString("X") + '\n';
            foreach (string name in CmdList.Keys)
            {
                CmdList[name].Add(temp);
            }/*
            int num = 0;
            if (Manual == 1)
            {
                num = FlashManualSize;
            }
            else if (Manual == 0)
            {
                num = FlashDmaSize;
            }
            else*/if(Manual!=0&&Manual!=1)
            {
                throw new Exception("SDMA或Manual方式错误！！！");
            }
            //如果Block中存在","逗号就说明是多plane，此时需要禁用读Block和读到SD卡中
            if (((Regex.Matches(Block, ",").Count) > 0))
            {
                if (PageNum > 1)
                {
                    throw new Exception("多plane读方式只支持读取一个page");
                }
            }
            //读的大小为两个Block
            //num *= (Regex.Matches(Block, ",").Count + 1);
            //PageLength.Add(num);
            //ReadPageNum = PageLength[0];
        }
        /**
         * 参数个数：13个
         * 
         */
        public void PageProgram(int Chan, int Chip, int Mode, string Block, int Page, string ProgramDataPath, int offert, int Manual,int DelayTime,int Reset_flag,int CMD_Code,int CMD1_Flag,int SendData)
        {
            /*
            if (ManualFlag == -1)
            {
                ManualFlag = Manual;
            }
            else
            {
                if (ManualFlag != Manual)
                {
                    throw new Exception("当前脚本中只能包含一种SDMA或Manual方式！！！");
                }
            }
            */
            if(SendData !=0 && SendData != 1)
            {
                throw new Exception("SendData 只能为0或者1！");
            }
            if(SendData ==0 && Manual == 0)
            {
                throw new Exception("只支持Manual方式编程时不发数据！");
            }
            if(!(CMD_Code>=0 && CMD_Code <= 3))
            {
                throw new Exception("CMD_Code 只能为0，1，2！");
            }
            if(CMD1_Flag != 0 && CMD1_Flag != 1)
            {
                throw new Exception("CMD1_Flag 只能为0或者1！");
            }
            if(Reset_flag !=1 && Reset_flag != 0)
            {
                throw new Exception("Reset_flag 只能为0或者1！");
            }
            Int32 args = ((Chan & 0xFF) << 24) | ((Chip & 0xFF) << 16) | ((Mode & 0xFF) << 8) | (Manual & 0xFF);
            Int32 args1 = ((CMD_Code & 0xFF) << 24) | ((CMD1_Flag & 0xFF) << 16) | ((SendData & 0xFF) << 8);
            string temp = "flash pageprogram" + " " + Block + " " + Page.ToString() + " " + args.ToString("X") + " " + DelayTime.ToString() +" "+Reset_flag.ToString()+" "+args1.ToString("X")+'\n';
            //Console.WriteLine(temp);
            foreach (string name in CmdList.Keys)
            {
                CmdList[name].Add(temp);
            }
            int num = 0;
            if (Manual == 1)
            {
                num = FlashManualSize;
            }
            else if (Manual == 0)
            {
                num = FlashDmaSize;
            }
            
            if(File.Exists(ProgramDataPath))
            {
               PathAndOffert tmpPath = new PathAndOffert(ProgramDataPath, "", "", "", offert, 0,num);
               foreach(string port in PortList.Keys)
                {
                    if(!WriteDataList.ContainsKey(port))
                    {
                        List<PathAndOffert> path = new List<PathAndOffert>();
                        WriteDataList.Add(port, path);
                    }
                    WriteDataList[port].Add(tmpPath);
                }
            }
            else
            {
                throw new Exception("文件路径不存在，请检查！");
            }
        }
        public void SetFeature(int Chan, int Chip, int FeatureAddress, int P1, int P2, int P3, int P4)
        {
            Int32 args = ((P4 & 0xFF) << 24) | ((P3 & 0xFF) << 16) | ((P2 & 0xFF) << 8) | (P1 & 0xFF);
            string temp = "flash setfeature " + Chan.ToString() + " " + Chip.ToString() + " " + FeatureAddress.ToString()
                + " " + args.ToString("X") + '\n';
            foreach (string name in CmdList.Keys)
            {
                CmdList[name].Add(temp);
            }
        }
        public void Print(string number)
        {
            string temp = "flash myprint " + number + '\n';
            foreach (string name in CmdList.Keys)
            {
                CmdList[name].Add(temp);
            }
        }
        public void InitToDebug()
        {
            string temp = "flash init 1\n";
            foreach (string name in CmdList.Keys)
            {
                CmdList[name].Add(temp);
            }
        }
        //判断文件是否存在，以及坐标偏移
        private void FileExsits(string path,int offert,string Block,int RBFlag)
        {
            string tmpStr = string.Empty;
            string FilePath = string.Empty;

            PathAndOffert temp = new PathAndOffert(null, null, null, null, offert, 1,0);
            string[] FileName = path.Split(',');//FileName[0]必定是文件夹的名字
            string[] Blocks = Block.Split(',');
            if (!Directory.Exists(FileName[0]))//判断文件夹是否存在，没有则新建
            {
                Directory.CreateDirectory(FileName[0]);
            }
            if(Blocks.Count() != FileName.Count() -1 )//OutPath中的逗号数目比Block中的多1个
            {
                throw new Exception("ReadPage 中Block和plane数应该和数据路径对应！！！");
            }
            if(FileName.Count() == 2)//单plane
            {
                tmpStr = FileName[0] + @"/" + FileName[1];
                temp.FilePath[0] = tmpStr;
                temp.PlaneNum = 1;
            }
            else if(FileName.Count() == 3)//两个Plane
            {
                tmpStr = FileName[0] + @"/" + FileName[1];
                temp.FilePath[0] = tmpStr;
                tmpStr = FileName[0] + @"/" + FileName[2];
                temp.FilePath[1] = tmpStr;
                temp.PlaneNum = 2;
            }
            else if(FileName.Count() == 5)//4个Plane
            {
                tmpStr = FileName[0] + @"/" + FileName[1];
                temp.FilePath[0] = tmpStr;
                tmpStr = FileName[0] + @"/" + FileName[2];
                temp.FilePath[1] = tmpStr;
                tmpStr = FileName[0] + @"/" + FileName[3];
                temp.FilePath[2] = tmpStr;
                tmpStr = FileName[0] + @"/" + FileName[4];
                temp.FilePath[3] = tmpStr;
                temp.PlaneNum = 4;
            }
            else
            {
                throw new Exception("ReadPage 路径格式错误！！！");
            }
            if (RBFlag==1)//如果统计R/B#拉低时间，则新建一个csv文件
            {
                try
                {
                    FileStream stream = new FileStream(FileName[0] + @"/" + @"R/B_Falling_Duration.csv", FileMode.Create, FileAccess.Write);
                    stream.Close();
                }
                catch
                {
                    throw new Exception("R/B统计表格创建失败！");
                }
            }
            if(PortList.Keys.Count == 1)
            {
                string name = PortList.FirstOrDefault().Key;
                if (!ComDataPath.Keys.Contains(name))
                {
                    List<PathAndOffert> tmpList = new List<PathAndOffert>();
                    ComDataPath.Add(name, tmpList);
                }
                for (int i = 0; i < temp.PlaneNum; i++)
                {
                    try
                    {
                        //temp.FilePath[i] += ".bin";
                        if (!File.Exists(temp.FilePath[i]))
                        {
                            //File.Create(temp.FilePath[i]).Dispose();
                            FileStream stream = new FileStream(temp.FilePath[i], FileMode.Create, FileAccess.Write);
                            stream.Close();
                        }
                    }
                    catch
                    {
                        throw new Exception("文件路径不正确！！！");
                    }
                    
                }
                ComDataPath[name].Add(temp);
            }
            else
            {
                foreach (string name in PortList.Keys)
                {
                    PathAndOffert Privious = temp;
                    temp = Privious;
                    if (!ComDataPath.Keys.Contains(name))
                    {
                        List<PathAndOffert> tmpList = new List<PathAndOffert>();
                        ComDataPath.Add(name, tmpList);
                    }
                    for (int i = 0; i < Privious.PlaneNum; i++)
                    {
                        temp.FilePath[i] += name;
                        if (!File.Exists(temp.FilePath[i]))
                        {
                            //File.Create(temp.FilePath[i]).Dispose();
                            FileStream stream = new FileStream(temp.FilePath[i], FileMode.Create, FileAccess.Write);
                            stream.Close();
                        }
                    }
                    ComDataPath[name].Add(temp);
                }
            }
        }
        //获取读取文件位置偏移
        private int GetFileOffert(string path, int flag)
        {
            int index = 0;
            FileInfo info = new FileInfo(path);
            long size = info.Length;
            if (flag == -1 || flag == 0)
            {
                index = 0;
            }
            else if (flag >= size / WritePageNum)
            {
                index = (int)size / WritePageNum -1;
            }
            else
            {
                index = flag;
            }
            return index * WritePageNum;
        }
        //绘制左侧行号
        private void showLineNo()
        {
            Console.WriteLine("repaint");
            //获得当前坐标信息
            Point p = this.ScriptTextBox.Location;//获取当前textbox的位置
            int crntFirstIndex = this.ScriptTextBox.GetCharIndexFromPosition(p);//获取当前textbox的第一行
            int crntFirstLine = this.ScriptTextBox.GetLineFromCharIndex(crntFirstIndex);//获取当前textbox的第一行
            Point crntFirstPos = this.ScriptTextBox.GetPositionFromCharIndex(crntFirstIndex);
            p.Y += this.ScriptTextBox.Height;
            int crntLastIndex = this.ScriptTextBox.GetCharIndexFromPosition(p);
            int crntLastLine = this.ScriptTextBox.GetLineFromCharIndex(crntLastIndex);
            Point crntLastPos = this.ScriptTextBox.GetPositionFromCharIndex(crntLastIndex);

            //准备画图
            Graphics g = this.RowNumber.CreateGraphics();
            Font font = new Font(this.ScriptTextBox.Font, this.ScriptTextBox.Font.Style);
            SolidBrush brush = new SolidBrush(Color.Green);

            //画图开始
            //刷新画布
            //Rectangle rect = this.RowNumber.ClientRectangle;
            brush.Color = this.RowNumber.BackColor;
            g.FillRectangle(brush, 0, 0, this.RowNumber.ClientRectangle.Width, this.RowNumber.ClientRectangle.Height);      
            brush.Color = Color.White;//重置画笔颜色

            //绘制行号

            int lineSpace = 0;
            if (crntFirstLine != crntLastLine)
            {
                lineSpace = (crntLastPos.Y - crntFirstPos.Y) / (crntLastLine - crntFirstLine);
            }
            else
            {
                lineSpace = Convert.ToInt32(this.ScriptTextBox.Font.Size);
            }
            int brushX = this.RowNumber.ClientRectangle.Width - Convert.ToInt32(font.Size * 3);//最多支持3位数的行号，从大的行号到小的行号进行绘制
            int brushY = crntLastPos.Y + Convert.ToInt32(font.Size * 0.21f);//惊人的算法啊！！
            for (int i = crntLastLine; i >= 0; i--)
            {
                g.DrawString((i + 1).ToString(), font, brush, brushX, brushY);
                brushY -= lineSpace;
            }
            g.Dispose();
            font.Dispose();
            brush.Dispose();
        }
    }
}