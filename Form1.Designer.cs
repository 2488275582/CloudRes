namespace FlashAnalyze
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
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.MenuStrip = new System.Windows.Forms.MenuStrip();
            this.文件ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.OpenOtherFileBtn = new System.Windows.Forms.ToolStripMenuItem();
            this.SaveToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.SetUp = new System.Windows.Forms.ToolStripMenuItem();
            this.主控ToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.stoartToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.帮助ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.aboutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.flashToolStripMenuItem = new System.Windows.Forms.ToolStripComboBox();
            this.ToolStrip = new System.Windows.Forms.ToolStrip();
            this.toolStripSeparator5 = new System.Windows.Forms.ToolStripSeparator();
            this.RunBtn = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
            this.StopBtn = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripButton3 = new System.Windows.Forms.ToolStripLabel();
            this.SerialSclectComBox = new System.Windows.Forms.ToolStripDropDownButton();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.ConnectSerialBtn = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.InterruptBtn = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator6 = new System.Windows.Forms.ToolStripSeparator();
            this.SelectFWfileBtn = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator7 = new System.Windows.Forms.ToolStripSeparator();
            this.SendToolStripButton = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator8 = new System.Windows.Forms.ToolStripSeparator();
            this.FilePanel = new System.Windows.Forms.Panel();
            this.FiletreeView = new System.Windows.Forms.TreeView();
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.label1 = new System.Windows.Forms.Label();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.OpenToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.AddToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.DelToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ReNameToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.FoldeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.DelDirToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.RushToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ScriptPanel = new System.Windows.Forms.Panel();
            this.RowNumber = new System.Windows.Forms.Panel();
            this.ScriptTextBox = new System.Windows.Forms.RichTextBox();
            this.ScriptNameLabel = new System.Windows.Forms.Label();
            this.OpenFileDialogFW = new System.Windows.Forms.OpenFileDialog();
            this.StatusStrip = new System.Windows.Forms.StatusStrip();
            this.ScriptName = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStatusCom = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStatusScriptLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.timer2 = new System.Windows.Forms.Timer(this.components);
            this.SelectFWdialog = new System.Windows.Forms.OpenFileDialog();
            this.MenuStrip.SuspendLayout();
            this.ToolStrip.SuspendLayout();
            this.FilePanel.SuspendLayout();
            this.contextMenuStrip1.SuspendLayout();
            this.ScriptPanel.SuspendLayout();
            this.StatusStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // MenuStrip
            // 
            this.MenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.文件ToolStripMenuItem,
            this.SetUp,
            this.主控ToolStripMenuItem1,
            this.帮助ToolStripMenuItem,
            this.flashToolStripMenuItem});
            this.MenuStrip.Location = new System.Drawing.Point(0, 0);
            this.MenuStrip.Name = "MenuStrip";
            this.MenuStrip.Size = new System.Drawing.Size(1439, 29);
            this.MenuStrip.TabIndex = 0;
            this.MenuStrip.Text = "menuStrip1";
            // 
            // 文件ToolStripMenuItem
            // 
            this.文件ToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.OpenOtherFileBtn,
            this.SaveToolStripMenuItem});
            this.文件ToolStripMenuItem.Name = "文件ToolStripMenuItem";
            this.文件ToolStripMenuItem.Size = new System.Drawing.Size(44, 25);
            this.文件ToolStripMenuItem.Text = "文件";
            // 
            // OpenOtherFileBtn
            // 
            this.OpenOtherFileBtn.Name = "OpenOtherFileBtn";
            this.OpenOtherFileBtn.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.O)));
            this.OpenOtherFileBtn.Size = new System.Drawing.Size(171, 22);
            this.OpenOtherFileBtn.Text = "打开文件";
            this.OpenOtherFileBtn.Click += new System.EventHandler(this.OpenOtherFileBtn_Click);
            // 
            // SaveToolStripMenuItem
            // 
            this.SaveToolStripMenuItem.Name = "SaveToolStripMenuItem";
            this.SaveToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.S)));
            this.SaveToolStripMenuItem.Size = new System.Drawing.Size(171, 22);
            this.SaveToolStripMenuItem.Text = "保存文件";
            this.SaveToolStripMenuItem.Click += new System.EventHandler(this.SaveToolStripMenuItem_Click);
            // 
            // SetUp
            // 
            this.SetUp.Name = "SetUp";
            this.SetUp.ShowShortcutKeys = false;
            this.SetUp.Size = new System.Drawing.Size(68, 25);
            this.SetUp.Text = "串口配置";
            this.SetUp.Click += new System.EventHandler(this.SetUp_Click);
            // 
            // 主控ToolStripMenuItem1
            // 
            this.主控ToolStripMenuItem1.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.stoartToolStripMenuItem});
            this.主控ToolStripMenuItem1.Name = "主控ToolStripMenuItem1";
            this.主控ToolStripMenuItem1.Size = new System.Drawing.Size(44, 25);
            this.主控ToolStripMenuItem1.Text = "主控";
            // 
            // stoartToolStripMenuItem
            // 
            this.stoartToolStripMenuItem.Checked = true;
            this.stoartToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
            this.stoartToolStripMenuItem.Name = "stoartToolStripMenuItem";
            this.stoartToolStripMenuItem.Size = new System.Drawing.Size(111, 22);
            this.stoartToolStripMenuItem.Text = "Stoart";
            // 
            // 帮助ToolStripMenuItem
            // 
            this.帮助ToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.aboutToolStripMenuItem});
            this.帮助ToolStripMenuItem.Name = "帮助ToolStripMenuItem";
            this.帮助ToolStripMenuItem.Size = new System.Drawing.Size(44, 25);
            this.帮助ToolStripMenuItem.Text = "帮助";
            // 
            // aboutToolStripMenuItem
            // 
            this.aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
            this.aboutToolStripMenuItem.Size = new System.Drawing.Size(110, 22);
            this.aboutToolStripMenuItem.Text = "about";
            this.aboutToolStripMenuItem.Click += new System.EventHandler(this.aboutToolStripMenuItem_Click);
            // 
            // flashToolStripMenuItem
            // 
            this.flashToolStripMenuItem.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.flashToolStripMenuItem.Items.AddRange(new object[] {
            "B05",
            "GCG"});
            this.flashToolStripMenuItem.Name = "flashToolStripMenuItem";
            this.flashToolStripMenuItem.Size = new System.Drawing.Size(75, 25);
            this.flashToolStripMenuItem.SelectedIndexChanged += new System.EventHandler(this.flashToolStripMenuItem_SelectedIndexChanged);
            // 
            // ToolStrip
            // 
            this.ToolStrip.ImageScalingSize = new System.Drawing.Size(32, 32);
            this.ToolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripSeparator5,
            this.RunBtn,
            this.toolStripSeparator4,
            this.StopBtn,
            this.toolStripSeparator1,
            this.toolStripButton3,
            this.SerialSclectComBox,
            this.toolStripSeparator3,
            this.ConnectSerialBtn,
            this.toolStripSeparator2,
            this.InterruptBtn,
            this.toolStripSeparator6,
            this.SelectFWfileBtn,
            this.toolStripSeparator7,
            this.SendToolStripButton,
            this.toolStripSeparator8});
            this.ToolStrip.Location = new System.Drawing.Point(0, 29);
            this.ToolStrip.Name = "ToolStrip";
            this.ToolStrip.Size = new System.Drawing.Size(1439, 39);
            this.ToolStrip.TabIndex = 1;
            this.ToolStrip.Text = "toolStrip1";
            // 
            // toolStripSeparator5
            // 
            this.toolStripSeparator5.Name = "toolStripSeparator5";
            this.toolStripSeparator5.Size = new System.Drawing.Size(6, 39);
            // 
            // RunBtn
            // 
            this.RunBtn.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.RunBtn.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.RunBtn.Enabled = false;
            this.RunBtn.Image = global::FlashAnalyze.Properties.Resources.arrow_triangle_right;
            this.RunBtn.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.RunBtn.Name = "RunBtn";
            this.RunBtn.Size = new System.Drawing.Size(36, 36);
            this.RunBtn.Text = "开始";
            this.RunBtn.Click += new System.EventHandler(this.RunBtn_Click);
            // 
            // toolStripSeparator4
            // 
            this.toolStripSeparator4.Name = "toolStripSeparator4";
            this.toolStripSeparator4.Size = new System.Drawing.Size(6, 39);
            // 
            // StopBtn
            // 
            this.StopBtn.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.StopBtn.Image = global::FlashAnalyze.Properties.Resources.icon_pause;
            this.StopBtn.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.StopBtn.Name = "StopBtn";
            this.StopBtn.Size = new System.Drawing.Size(36, 36);
            this.StopBtn.Text = "暂停";
            this.StopBtn.Click += new System.EventHandler(this.StopBtn_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 39);
            // 
            // toolStripButton3
            // 
            this.toolStripButton3.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButton3.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton3.Name = "toolStripButton3";
            this.toolStripButton3.Size = new System.Drawing.Size(0, 36);
            this.toolStripButton3.Text = "toolStripButton3";
            // 
            // SerialSclectComBox
            // 
            this.SerialSclectComBox.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.SerialSclectComBox.Image = ((System.Drawing.Image)(resources.GetObject("SerialSclectComBox.Image")));
            this.SerialSclectComBox.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.SerialSclectComBox.Name = "SerialSclectComBox";
            this.SerialSclectComBox.Size = new System.Drawing.Size(137, 36);
            this.SerialSclectComBox.Text = "        串口选择         ";
            this.SerialSclectComBox.DropDownOpened += new System.EventHandler(this.SerialSclectComBox_DropDownOpened);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(6, 39);
            // 
            // ConnectSerialBtn
            // 
            this.ConnectSerialBtn.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.ConnectSerialBtn.Image = ((System.Drawing.Image)(resources.GetObject("ConnectSerialBtn.Image")));
            this.ConnectSerialBtn.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.ConnectSerialBtn.Name = "ConnectSerialBtn";
            this.ConnectSerialBtn.Size = new System.Drawing.Size(108, 36);
            this.ConnectSerialBtn.Text = "      连接串口      ";
            this.ConnectSerialBtn.Click += new System.EventHandler(this.ConnectSerialBtn_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(6, 39);
            // 
            // InterruptBtn
            // 
            this.InterruptBtn.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.InterruptBtn.Image = ((System.Drawing.Image)(resources.GetObject("InterruptBtn.Image")));
            this.InterruptBtn.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.InterruptBtn.Name = "InterruptBtn";
            this.InterruptBtn.Size = new System.Drawing.Size(108, 36);
            this.InterruptBtn.Text = "      断开连接      ";
            this.InterruptBtn.Click += new System.EventHandler(this.InterruptBtn_Click);
            // 
            // toolStripSeparator6
            // 
            this.toolStripSeparator6.Name = "toolStripSeparator6";
            this.toolStripSeparator6.Size = new System.Drawing.Size(6, 39);
            // 
            // SelectFWfileBtn
            // 
            this.SelectFWfileBtn.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.SelectFWfileBtn.Image = ((System.Drawing.Image)(resources.GetObject("SelectFWfileBtn.Image")));
            this.SelectFWfileBtn.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.SelectFWfileBtn.Name = "SelectFWfileBtn";
            this.SelectFWfileBtn.Size = new System.Drawing.Size(108, 36);
            this.SelectFWfileBtn.Text = "   选择固件文件   ";
            this.SelectFWfileBtn.Click += new System.EventHandler(this.SelectFWfileBtn_Click);
            // 
            // toolStripSeparator7
            // 
            this.toolStripSeparator7.Name = "toolStripSeparator7";
            this.toolStripSeparator7.Size = new System.Drawing.Size(6, 39);
            // 
            // SendToolStripButton
            // 
            this.SendToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.SendToolStripButton.Enabled = false;
            this.SendToolStripButton.Image = ((System.Drawing.Image)(resources.GetObject("SendToolStripButton.Image")));
            this.SendToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.SendToolStripButton.Name = "SendToolStripButton";
            this.SendToolStripButton.Size = new System.Drawing.Size(110, 36);
            this.SendToolStripButton.Text = "      发送FW        ";
            this.SendToolStripButton.Click += new System.EventHandler(this.toolStripButton1_Click);
            // 
            // toolStripSeparator8
            // 
            this.toolStripSeparator8.Name = "toolStripSeparator8";
            this.toolStripSeparator8.Size = new System.Drawing.Size(6, 39);
            // 
            // FilePanel
            // 
            this.FilePanel.BackColor = System.Drawing.SystemColors.Window;
            this.FilePanel.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.FilePanel.Controls.Add(this.FiletreeView);
            this.FilePanel.Controls.Add(this.label1);
            this.FilePanel.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.FilePanel.Location = new System.Drawing.Point(3, 67);
            this.FilePanel.Name = "FilePanel";
            this.FilePanel.Size = new System.Drawing.Size(247, 847);
            this.FilePanel.TabIndex = 3;
            // 
            // FiletreeView
            // 
            this.FiletreeView.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.FiletreeView.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.FiletreeView.ImageIndex = 0;
            this.FiletreeView.ImageList = this.imageList1;
            this.FiletreeView.LabelEdit = true;
            this.FiletreeView.Location = new System.Drawing.Point(0, 22);
            this.FiletreeView.Name = "FiletreeView";
            this.FiletreeView.SelectedImageIndex = 0;
            this.FiletreeView.Size = new System.Drawing.Size(240, 823);
            this.FiletreeView.TabIndex = 1;
            this.FiletreeView.AfterLabelEdit += new System.Windows.Forms.NodeLabelEditEventHandler(this.FiletreeView_AfterLabelEdit);
            this.FiletreeView.MouseDown += new System.Windows.Forms.MouseEventHandler(this.FiletreeView_MouseDown);
            // 
            // imageList1
            // 
            this.imageList1.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList1.ImageStream")));
            this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList1.Images.SetKeyName(0, "file3.ico");
            this.imageList1.Images.SetKeyName(1, "txt.ico");
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(0, 3);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(72, 16);
            this.label1.TabIndex = 0;
            this.label1.Text = "文件视图";
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.OpenToolStripMenuItem,
            this.AddToolStripMenuItem,
            this.DelToolStripMenuItem,
            this.ReNameToolStripMenuItem,
            this.FoldeToolStripMenuItem,
            this.DelDirToolStripMenuItem,
            this.RushToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(137, 158);
            // 
            // OpenToolStripMenuItem
            // 
            this.OpenToolStripMenuItem.Name = "OpenToolStripMenuItem";
            this.OpenToolStripMenuItem.Size = new System.Drawing.Size(136, 22);
            this.OpenToolStripMenuItem.Text = "打开脚本";
            this.OpenToolStripMenuItem.Click += new System.EventHandler(this.openToolStripMenuItem_Click);
            // 
            // AddToolStripMenuItem
            // 
            this.AddToolStripMenuItem.Name = "AddToolStripMenuItem";
            this.AddToolStripMenuItem.Size = new System.Drawing.Size(136, 22);
            this.AddToolStripMenuItem.Text = "新建脚本";
            this.AddToolStripMenuItem.Click += new System.EventHandler(this.AddToolStripMenuItem_Click);
            // 
            // DelToolStripMenuItem
            // 
            this.DelToolStripMenuItem.Name = "DelToolStripMenuItem";
            this.DelToolStripMenuItem.Size = new System.Drawing.Size(136, 22);
            this.DelToolStripMenuItem.Text = "删除脚本";
            this.DelToolStripMenuItem.Click += new System.EventHandler(this.DelToolStripMenuItem_Click);
            // 
            // ReNameToolStripMenuItem
            // 
            this.ReNameToolStripMenuItem.Name = "ReNameToolStripMenuItem";
            this.ReNameToolStripMenuItem.Size = new System.Drawing.Size(136, 22);
            this.ReNameToolStripMenuItem.Text = "重命名";
            this.ReNameToolStripMenuItem.Click += new System.EventHandler(this.ReNameToolStripMenuItem_Click);
            // 
            // FoldeToolStripMenuItem
            // 
            this.FoldeToolStripMenuItem.Name = "FoldeToolStripMenuItem";
            this.FoldeToolStripMenuItem.Size = new System.Drawing.Size(136, 22);
            this.FoldeToolStripMenuItem.Text = "新建文件夹";
            this.FoldeToolStripMenuItem.Click += new System.EventHandler(this.FoldeToolStripMenuItem_Click);
            // 
            // DelDirToolStripMenuItem
            // 
            this.DelDirToolStripMenuItem.Name = "DelDirToolStripMenuItem";
            this.DelDirToolStripMenuItem.Size = new System.Drawing.Size(136, 22);
            this.DelDirToolStripMenuItem.Text = "删除文件夹";
            this.DelDirToolStripMenuItem.Click += new System.EventHandler(this.DelDirToolStripMenuItem_Click);
            // 
            // RushToolStripMenuItem
            // 
            this.RushToolStripMenuItem.Name = "RushToolStripMenuItem";
            this.RushToolStripMenuItem.Size = new System.Drawing.Size(136, 22);
            this.RushToolStripMenuItem.Text = "刷新";
            this.RushToolStripMenuItem.Click += new System.EventHandler(this.RushToolStripMenuItem_Click);
            // 
            // ScriptPanel
            // 
            this.ScriptPanel.BackColor = System.Drawing.SystemColors.Window;
            this.ScriptPanel.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.ScriptPanel.Controls.Add(this.RowNumber);
            this.ScriptPanel.Controls.Add(this.ScriptTextBox);
            this.ScriptPanel.Controls.Add(this.ScriptNameLabel);
            this.ScriptPanel.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.ScriptPanel.Location = new System.Drawing.Point(251, 67);
            this.ScriptPanel.Name = "ScriptPanel";
            this.ScriptPanel.Size = new System.Drawing.Size(1188, 847);
            this.ScriptPanel.TabIndex = 5;
            // 
            // RowNumber
            // 
            this.RowNumber.BackColor = System.Drawing.SystemColors.InactiveCaptionText;
            this.RowNumber.Font = new System.Drawing.Font("宋体", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.RowNumber.Location = new System.Drawing.Point(-1, 22);
            this.RowNumber.Name = "RowNumber";
            this.RowNumber.Size = new System.Drawing.Size(53, 823);
            this.RowNumber.TabIndex = 2;
            // 
            // ScriptTextBox
            // 
            this.ScriptTextBox.AcceptsTab = true;
            this.ScriptTextBox.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.ScriptTextBox.BulletIndent = 5;
            this.ScriptTextBox.Font = new System.Drawing.Font("宋体", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.ScriptTextBox.Location = new System.Drawing.Point(49, 22);
            this.ScriptTextBox.Name = "ScriptTextBox";
            this.ScriptTextBox.Size = new System.Drawing.Size(1137, 823);
            this.ScriptTextBox.TabIndex = 1;
            this.ScriptTextBox.TabStop = false;
            this.ScriptTextBox.Text = "";
            this.ScriptTextBox.WordWrap = false;
            this.ScriptTextBox.VScroll += new System.EventHandler(this.ScriptTextBox_VScroll);
            this.ScriptTextBox.TextChanged += new System.EventHandler(this.ScriptTextBox_TextChanged);
            this.ScriptTextBox.KeyDown += new System.Windows.Forms.KeyEventHandler(this.ScriptTextBox_KeyDown);
            // 
            // ScriptNameLabel
            // 
            this.ScriptNameLabel.AutoSize = true;
            this.ScriptNameLabel.Location = new System.Drawing.Point(3, 0);
            this.ScriptNameLabel.Name = "ScriptNameLabel";
            this.ScriptNameLabel.Size = new System.Drawing.Size(0, 16);
            this.ScriptNameLabel.TabIndex = 0;
            // 
            // OpenFileDialogFW
            // 
            this.OpenFileDialogFW.FileName = "openFileDialog1";
            // 
            // StatusStrip
            // 
            this.StatusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ScriptName,
            this.toolStatusCom,
            this.toolStatusScriptLabel});
            this.StatusStrip.Location = new System.Drawing.Point(0, 917);
            this.StatusStrip.Name = "StatusStrip";
            this.StatusStrip.Size = new System.Drawing.Size(1439, 22);
            this.StatusStrip.TabIndex = 6;
            this.StatusStrip.Text = "statusStrip1";
            // 
            // ScriptName
            // 
            this.ScriptName.Name = "ScriptName";
            this.ScriptName.Size = new System.Drawing.Size(0, 17);
            // 
            // toolStatusCom
            // 
            this.toolStatusCom.Name = "toolStatusCom";
            this.toolStatusCom.Size = new System.Drawing.Size(0, 17);
            // 
            // toolStatusScriptLabel
            // 
            this.toolStatusScriptLabel.Name = "toolStatusScriptLabel";
            this.toolStatusScriptLabel.Size = new System.Drawing.Size(0, 17);
            // 
            // timer1
            // 
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // SelectFWdialog
            // 
            this.SelectFWdialog.FileName = "openFileDialog1";
            this.SelectFWdialog.Multiselect = true;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1439, 939);
            this.Controls.Add(this.StatusStrip);
            this.Controls.Add(this.ScriptPanel);
            this.Controls.Add(this.FilePanel);
            this.Controls.Add(this.ToolStrip);
            this.Controls.Add(this.MenuStrip);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.MenuStrip;
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "FLASH ANALYZE";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.Load += new System.EventHandler(this.Form1_Load);
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.Form1_Paint);
            this.Resize += new System.EventHandler(this.Form1_Resize);
            this.MenuStrip.ResumeLayout(false);
            this.MenuStrip.PerformLayout();
            this.ToolStrip.ResumeLayout(false);
            this.ToolStrip.PerformLayout();
            this.FilePanel.ResumeLayout(false);
            this.FilePanel.PerformLayout();
            this.contextMenuStrip1.ResumeLayout(false);
            this.ScriptPanel.ResumeLayout(false);
            this.ScriptPanel.PerformLayout();
            this.StatusStrip.ResumeLayout(false);
            this.StatusStrip.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip MenuStrip;
        private System.Windows.Forms.ToolStripMenuItem 文件ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem SaveToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem SetUp;
        private System.Windows.Forms.ToolStripMenuItem 帮助ToolStripMenuItem;
        private System.Windows.Forms.ToolStrip ToolStrip;
        private System.Windows.Forms.ToolStripButton RunBtn;
        private System.Windows.Forms.ToolStripButton StopBtn;
        private System.Windows.Forms.Panel FilePanel;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Panel ScriptPanel;
        private System.Windows.Forms.Label ScriptNameLabel;
        private System.Windows.Forms.OpenFileDialog OpenFileDialogFW;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator5;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator4;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripLabel toolStripButton3;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator6;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator7;
        private System.Windows.Forms.StatusStrip StatusStrip;
        private System.Windows.Forms.ToolStripStatusLabel ScriptName;
        private System.Windows.Forms.ToolStripButton InterruptBtn;
        private System.Windows.Forms.ToolStripButton ConnectSerialBtn;
        private System.Windows.Forms.ToolStripButton SelectFWfileBtn;
        private System.Windows.Forms.TreeView FiletreeView;
        private System.Windows.Forms.ToolStripDropDownButton SerialSclectComBox;
        private System.Windows.Forms.ToolStripStatusLabel toolStatusCom;
        private System.Windows.Forms.ToolStripMenuItem OpenOtherFileBtn;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem OpenToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem DelToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem ReNameToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem aboutToolStripMenuItem;
        private System.Windows.Forms.ToolStripStatusLabel toolStatusScriptLabel;
        private System.Windows.Forms.RichTextBox ScriptTextBox;
        private System.Windows.Forms.Panel RowNumber;
        private System.Windows.Forms.ToolStripMenuItem 主控ToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem stoartToolStripMenuItem;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.Timer timer2;
        private System.Windows.Forms.ToolStripMenuItem AddToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem FoldeToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem RushToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem DelDirToolStripMenuItem;
        private System.Windows.Forms.ImageList imageList1;
        private System.Windows.Forms.ToolStripButton SendToolStripButton;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator8;
        private System.Windows.Forms.OpenFileDialog SelectFWdialog;
        private System.Windows.Forms.ToolStripComboBox flashToolStripMenuItem;
        //private System.Windows.Forms.MessageBox reloadScriptMessage;
    }
}

