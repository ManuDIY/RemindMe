﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;
using System.IO;
using Business_Logic_Layer;

namespace RemindMe
{
    public partial class UCDebugMode : UserControl
    {
        List<string> localCacheList = new List<string>();
        public UCDebugMode()
        {
            InitializeComponent();
        }
      

        private void btnOpenErrorPrompt_Click(object sender, EventArgs e)
        {
            BLIO.Log("btnOpenErrorPrompt_Click");
            ExceptionPopup pop = new ExceptionPopup(new ReminderException("Test",null), "This is a test error in debug mode");
            pop.Show();
        }

        private void btnAppdataFolder_Click(object sender, EventArgs e)
        {
            BLIO.Log("btnAppdataFolder_Click");
            Process.Start(Path.GetDirectoryName(IOVariables.errorLog));
        }

        private void tmrDetails_Tick(object sender, EventArgs e)
        {                        
            if (this.Visible)
            {
                lblMemoryUsage.Text = GetMemory() / 1000 + " Mb";// Process.GetCurrentProcess().VirtualMemorySize64 / 1024 / 1024 + "Mb";               
            }
        }

        private void UCDebugMode_Load(object sender, EventArgs e)
        {            
            tmrDetails.Start();
            tmrLog.Start();
            localCacheList.AddRange(BLIO.systemLog);            
        }

        private long GetMemory()
        {            
            PerformanceCounter pc = new PerformanceCounter();
            pc.CategoryName = "Process";
            pc.CounterName = "Working Set - Private";
            pc.InstanceName = Process.GetCurrentProcess().ProcessName;
            return Convert.ToInt32(pc.NextValue()) / (int)(1024);            
        }

        private void bunifuFlatButton2_Click(object sender, EventArgs e)
        {
            string text = RemindMePrompt.ShowText("Enter a message");

            if(!string.IsNullOrWhiteSpace(text))
                RemindMeMessageFormManager.MakeMessagePopup(text, 4);
            else
                RemindMeMessageFormManager.MakeMessagePopup("This is a test.", 4);
        }

        private void tmrLog_Tick(object sender, EventArgs e)
        {
            if (this.Visible)
            {                                
                if(localCacheList.Count != BLIO.systemLog.Count)
                {
                    tbSystemLog.Clear();                    
                    tbSystemLog.AppendText(string.Join(Environment.NewLine, BLIO.systemLog));

                    localCacheList.Clear();
                    localCacheList.AddRange(BLIO.systemLog);
                }
            }
        }

        private void UCDebugMode_VisibleChanged(object sender, EventArgs e)
        {
            BLIO.Log("Showing debug mode");
        }

        private void btnCheckUpdate_Click(object sender, EventArgs e)
        {
            BLIO.Log("btnCheckUpdate_Click");
            Form1.Instance.CheckForUpdates();
        }

        private void btnRequery_Click(object sender, EventArgs e)
        {
            //Copy the contents of the textbox to the system clipboard                  
            Clipboard.SetText(tbSystemLog.Text);
            BLIO.Log("Copied system log to clipboard");
        }
    }
}
