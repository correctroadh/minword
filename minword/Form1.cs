﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Printing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Tab;

namespace minword
{
    public partial class Form1 : Form
    {
        private static int FormCount = 0;
        private static List<Word> words = new List<Word>();
        public static int selectindex = -1; // 当前选中的是哪个word;  

        //定义此常量是为了统计MDI窗体数目，
        MainMenu mnuMain = new MainMenu();
        MenuItem FileMenu;
        MenuItem ExitMenu;
        MenuItem WindowMenu;

        private PageSetupDialog pageSetupDialog = new PageSetupDialog();
        private PrintDialog printDialog = new PrintDialog();
        private PrintDocument printDocument = new PrintDocument();
        private PrintPreviewDialog printPreviewDialog = new PrintPreviewDialog();


        public Form1()
        {
            InitializeComponent();
            this.IsMdiContainer = true;
            this.Text = "MDI演示程序";
            FileMenu = new MenuItem();
            FileMenu.Text = "文件";
            FileMenu.MenuItems.Add("新建", new EventHandler(New_Click));
            FileMenu.MenuItems.Add("打开", new EventHandler(OpenFile));
            FileMenu.MenuItems.Add("保存", new EventHandler(saveFile));
            FileMenu.MenuItems.Add("另存为", new EventHandler(saveFileToAnother));
            FileMenu.MenuItems.Add("全部保存", new EventHandler(OpenFile));
            FileMenu.MenuItems.Add("关闭", new EventHandler(closeCurrent));
            FileMenu.MenuItems.Add("全部关闭", new EventHandler(closeAll));
            FileMenu.MenuItems.Add(new MenuItem("-"));
            FileMenu.MenuItems.Add("页面设置", new EventHandler(PageSetupToolStripMenuItem_Click));
            FileMenu.MenuItems.Add("打印预览", new EventHandler(PrintPreviewToolStripMenuItem_Click));
            FileMenu.MenuItems.Add("打印", new EventHandler(printCtrlPToolStripMenuItem_Click));
            FileMenu.MenuItems.Add(new MenuItem("-"));
            
            ExitMenu = new MenuItem();
            ExitMenu.Text = "退出(&X)";
            ExitMenu.Click += new EventHandler(Exit_Click);
            FileMenu.MenuItems.Add(ExitMenu);


            
            WindowMenu = new MenuItem();
            WindowMenu.Text = "窗口(&W)";
            WindowMenu.MenuItems.Add("堆叠(&C)", new EventHandler(Cascade_Click));
            WindowMenu.MenuItems.Add("平铺(&V)", new EventHandler(TileH_Click));
            WindowMenu.MdiList = true;
            //这一句比较重要，有了这一句就可以实现在新建一个MDI窗体后会在此主菜单项下显示存在的MDI窗体菜单项


            mnuMain.MenuItems.Add(FileMenu);
            mnuMain.MenuItems.Add(WindowMenu);
            this.Menu = mnuMain;

        }

        private void saveFile(object sender, EventArgs e)
        {
            if(selectindex != -1)
            {
                words[selectindex].saveFile();

            }
        }
        private void saveFileToAnother(object sender, EventArgs e)
        {
            if (selectindex != -1)
                words[selectindex].saveFileToAnother();
        }

        private void closeCurrent(object sender, EventArgs e)
        {
            if (selectindex != -1)
                words[selectindex].CloseWindows();
        }
        private void closeAll(object sender, EventArgs e)
        {
            words.ForEach((Word word) =>
            {
                word.CloseWindows();
            });
        }

        private void PageSetupToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.pageSetupDialog.ShowDialog();
        }

        private void printCtrlPToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.printDialog.ShowDialog();
        }

        private void PrintPreviewToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.printPreviewDialog.ShowDialog();
        }


        private void OpenFile(object sender, EventArgs e)
        {
            Console.WriteLine(openFileDialog1.ShowDialog());
        }
        private void Cascade_Click(object sender, EventArgs e)
        {
            this.LayoutMdi(MdiLayout.Cascade);
        }

        


        private void TileH_Click(object sender, EventArgs e)
        {
            this.LayoutMdi(MdiLayout.TileHorizontal);
        }

        private void New_Click(object sender, EventArgs e)
        {
            Word frmTemp = new Word(FormCount);
            //新建一个窗体
            frmTemp.MdiParent = this;
            //定义此窗体的父窗体，从而此窗体成为一个MDI窗体
            frmTemp.Text = "新建文档" + (FormCount+1).ToString();
            //设定MDI窗体的标题
            FormCount++;
            frmTemp.Show();
            words.Add(frmTemp);
            //把此MDI窗体显示出来
        }

        private void Exit_Click(object sender, EventArgs e)
        {
            this.Dispose();
            Application.Exit();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            this.pageSetupDialog.Document = printDocument;
            this.pageSetupDialog.AllowMargins = true;
            this.pageSetupDialog.AllowOrientation = true;
            this.pageSetupDialog.AllowPaper = true;
            this.pageSetupDialog.AllowPrinter = true;
            this.printPreviewDialog.ClientSize = new System.Drawing.Size(400, 300);
            this.printPreviewDialog.SetAutoScrollMargin(0, 0);
            this.printPreviewDialog.ClientSize = new Size(400, 300);
            this.printPreviewDialog.Document = printDocument;
            this.printDialog.Document = printDocument;
            this.printDialog.AllowSomePages = true;
            this.printDialog.AllowSelection = true;
            this.printDialog.AllowPrintToFile = true;

        }

        private void openFileDialog1_FileOk(object sender, CancelEventArgs e)
        {
            Console.WriteLine(openFileDialog1.FileName);
            Word frmTemp = new Word(FormCount,openFileDialog1.FileName);
            frmTemp.MdiParent = this;
            frmTemp.Text = openFileDialog1.FileName;
            frmTemp.Show();
            FormCount++;
            words.Add(frmTemp);
        }
    }
}
