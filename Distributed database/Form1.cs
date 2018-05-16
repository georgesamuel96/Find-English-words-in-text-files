using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;
using System.Collections;
using System.IO;


namespace Distributed_database
{
    public partial class Form1 : Form
    {
        string[] files;
        string query;
        List<string> doc = new List<string>();
        Dictionary<string, List<string>> dic = new Dictionary<string, List<string>>();

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog fbd = new FolderBrowserDialog();

            if (fbd.ShowDialog() == DialogResult.OK)
            {

                files = Directory.GetFiles(fbd.SelectedPath, "*.txt");

                for (int i = 0; i < files.Length; i++)
                {
                    List<string> lst = new List<string>();
                    string temp = File.ReadAllText(files[i]);
                    splitString(temp, lst);
                    dic.Add(Path.GetFileName(files[i]), lst);
                }

            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            textBox1.Text = "";
            listBox1.Items.Clear();
        }

        private void find_Click(object sender, EventArgs e)
        {
            query = textBox1.Text;
            string str1 = "", str2 = "", op = "", st;
            int z = 0, x = 0, y = 0, flag = 0, ft = 1;

            for (int j = 0; j < dic.Count; j++)
            {
                str1 = ""; str2 = ""; op = "";
                st = Path.GetFileName(files[j]);
                ft = 1;
                z = 0; x = 0; y = 0; flag = 0;
                for (int i = 0; i < query.Length; i++)
                {
                    if (query[i] == ' ')
                    {

                        if (z == 2)
                        {
                            x = 0; y = 0;
                            for (int k = 0; k < dic[st].Count; k++)
                            {
                                if (dic[st][k] == str1)
                                    x = 1;
                                if (dic[st][k] == str2)
                                    y = 1;
                            }
                            flag = checkFlag(ref ft, x, y, flag, op);
                            z = 0;
                            str2 = op = "";

                        }
                        z++;
                    }
                    else
                    {
                        if (z == 0)
                            str1 += query[i];
                        else if (z == 1)
                            op += query[i];
                        else
                            str2 += query[i];
                    }
                }
                y = 0;
                for (int k = 0; k < dic[st].Count; k++)
                {
                    if (dic[st][k] == str1)
                        x = 1;
                    if (dic[st][k] == str2)
                        y = 1;
                }
                flag = checkFlag(ref ft, x, y, flag, op);
                z = 0;
                str2 = op = "";
                if (flag == 1)
                {
                    listBox1.Items.Add(st);
                }
            }
        }

        public void splitString(string str, List<string> doc)
        {
            string st = "";
            for (int i = 0; i < str.Length; i++)
            {
                if (str[i] == ' ' || str[i] == '\r' || str[i] == '\n')
                {
                    if (st.Length > 0)
                        doc.Add(st);
                    st = "";
                }
                else
                    st = st + str[i];
            }
            if (st.Length > 0)
                doc.Add(st);
        }

        public bool findWord(string str, List<string> doc)
        {
            for (int i = 0; i < doc.Count; i++)
            {
                if (doc[i] == str)
                    return true;
            }
            return false;
        }

        public int checkFlag(ref int ft, int x, int y, int flag, string op)
        {
            if (ft == 1)
            {
                ft = 0;
                flag = flag | x;
                if (op[0] == '&')
                    flag = flag & y;
                else if (op[0] == '|')
                    flag = flag | y;
            }
            else
            {
                if (op[0] == '&')
                    flag = flag & y;
                else if (op[0] == '|')
                    flag = flag | y;
            }
            return flag;
        }
    }
}
