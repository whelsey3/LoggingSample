using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace LoggingSample
{
    public partial class Form1 : Form, ILogger
    {
        #region Data
        // the main Logger object
        private Logger mLogger;
        // a logger observer that will write the log entries to a file
        private FileLogger mFileLogger;
        int mTestCounter;
        #endregion

        #region Form methods
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // instantiate the logger
            mLogger = Logger.Instance;

            // instantiate the log observer that will write to disk
            mFileLogger = new FileLogger(@"c:\temp\log.txt" );
            mFileLogger.Init();

            // Add mFileLogger and the current form (both of which are Logger observers because
            // they implement the ILogger interface) to the Logger.
            mLogger.RegisterObserver(this);
            mLogger.RegisterObserver(mFileLogger);
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            // The application is shutting down, so ensure the file logger closes the file it's
            // been writing to.
            mFileLogger.Terminate();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            mTestCounter ++;
            textBox1.Text = mTestCounter.ToString();
            string message = string.Format ("The test counter has just been increased to: {0}", mTestCounter);
            mLogger.AddLogMessage(message);
        }
        #endregion

        #region ILogger Members

        public void ProcessLogMessage(string message)
        {
            // Form1 implements the ProcessLogMessage method as a simple addition of the incoming
            // message to a textbox on the form.
            textBoxLog.AppendText(message + "\r\n");
        }

        #endregion

    }
}