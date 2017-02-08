using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO.Ports;
using System.Threading;
namespace Serial_receive
{
    public partial class Form1 : Form
    {
        // All members variables should be placed here
        // more readable 
        string t;
        SerialPort sp;

        public Form1()
        {
            InitializeComponent();
            Control.CheckForIllegalCrossThreadCalls = false;

            // You can already seatch ports when the constructor of the FORM1 is calling 
            // And let the user search ports again with a click
            // That's why i put it in a function

            SearchPorts();
        }
       //search button  
        private void button1_Click(object sender, EventArgs e)
        {
            comboBox1.Items.Clear();
            SearchPorts();
        }
        void SearchPorts()
        {
            string[] ports = SerialPort.GetPortNames();
            foreach (string port in ports)
            {
                comboBox1.Items.Add(port);
            }
        }
  
        private void button2_Click(object sender, EventArgs e)
        {
            // See here how do i catch the exception if it will be thrown so the user will be a message box
            OpenCloseSerial();
        }      
        void OpenCloseSerial()
        {
            try
            {
                if (sp == null || sp.IsOpen == false)
                {
                    t = comboBox1.Text.ToString();
                    sErial(t);
                    button2.Text = "Close Serial port";
                }
                else
                {
                    sp.Close();
                    button2.Text = "Connect and wait for inputs";
                    
                }
            }
            catch (Exception err)
            {
                MessageBox.Show(err.Message);
            }           
        }
      
        void sErial(string Port_name)
        {
            try
            {
                sp = new SerialPort(Port_name, 115200, Parity.None, 8, StopBits.One);
                sp.DataReceived += new SerialDataReceivedEventHandler(DataReceivedHandler);
                sp.Open();
            }
            catch (Exception err)
            {
                throw (new SystemException(err.Message));
            }
        }
//
        private  void DataReceivedHandler(object sender,SerialDataReceivedEventArgs e)
        {

            // This line is not need , the sp is  a global to the class!!
            //SerialPort sp = (SerialPort)sender;
            if (e.EventType == SerialData.Chars)
            {
                if (sp.IsOpen)
                {
                    string w = sp.ReadExisting();
                    if (w != String.Empty)
                    {
                        Invoke(new Action(() => richTextBox1.AppendText(w)));
                    }
                }
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (sp == null || sp.IsOpen == false)
            {
                OpenCloseSerial();
            }
        }
    }
}
