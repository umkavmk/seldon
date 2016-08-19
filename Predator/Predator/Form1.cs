using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace Predator
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            chart1.Series.Clear();
            chart2.Series.Clear();
            chart3.Series.Clear();
        }
        System.Threading.Thread newThread;
        double T = 0;
        double L = 1;
        bool flag = true;
        bool thread = false;
        bool stop = true;
        int speed;

        double[,] V1;
        double[,] V2;
        double[,] V3;

        double h;
        double tau;
        double B = 1, A = 0;
        int N;
        int M;
        double ymax = 0;
        double[] x;
        int time = 0;

        double lymda1 = 0, lymda2 = 0, lymda3 = 0;
        double w1 = 0, w2 = 0, gamma1 = 0, gamma2 = 0, alpha1 = 0, alpha2 = 0, beta1 = 0, beta2 = 0;

        public double f1(double x)
        {
            L = B - A;
            double res = Convert.ToDouble(a00.Text.ToString());
            res += Convert.ToDouble(a01.Text.ToString()) * Math.Cos(Math.PI * 1 * x / L);
            res += Convert.ToDouble(a02.Text.ToString()) * Math.Cos(Math.PI * 2 * x / L);
            res += Convert.ToDouble(a03.Text.ToString()) * Math.Cos(Math.PI * 3 * x / L);
            res += Convert.ToDouble(a04.Text.ToString()) * Math.Cos(Math.PI * 4 * x / L);
            res += Convert.ToDouble(a05.Text.ToString()) * Math.Cos(Math.PI * 5 * x / L);
            res += Convert.ToDouble(a06.Text.ToString()) * Math.Cos(Math.PI * 6 * x / L);
            res += Convert.ToDouble(a07.Text.ToString()) * Math.Cos(Math.PI * 7 * x / L);
            res += Convert.ToDouble(a08.Text.ToString()) * Math.Cos(Math.PI * 8 * x / L);
            res += Convert.ToDouble(a09.Text.ToString()) * Math.Cos(Math.PI * 9 * x / L);
            if (res >= 0)
                return res;
            else
                return -1;
        }

        public double f2(double x)
        {
            L = B - A;
            double res = Convert.ToDouble(a10.Text.ToString());
            res += Convert.ToDouble(a11.Text.ToString()) * Math.Cos(Math.PI * 1 * x / L);
            res += Convert.ToDouble(a12.Text.ToString()) * Math.Cos(Math.PI * 2 * x / L);
            res += Convert.ToDouble(a13.Text.ToString()) * Math.Cos(Math.PI * 3 * x / L);
            res += Convert.ToDouble(a14.Text.ToString()) * Math.Cos(Math.PI * 4 * x / L);
            res += Convert.ToDouble(a15.Text.ToString()) * Math.Cos(Math.PI * 5 * x / L);
            res += Convert.ToDouble(a16.Text.ToString()) * Math.Cos(Math.PI * 6 * x / L);
            res += Convert.ToDouble(a17.Text.ToString()) * Math.Cos(Math.PI * 7 * x / L);
            res += Convert.ToDouble(a18.Text.ToString()) * Math.Cos(Math.PI * 8 * x / L);
            res += Convert.ToDouble(a19.Text.ToString()) * Math.Cos(Math.PI * 9 * x / L);
            if (res >= 0)
                return res;
            else
                return -1;
        }

        public double f3(double x)
        {
            L = B - A;
            double res = Convert.ToDouble(a20.Text.ToString());
            res += Convert.ToDouble(a21.Text.ToString()) * Math.Cos(Math.PI * 1 * x / L);
            res += Convert.ToDouble(a22.Text.ToString()) * Math.Cos(Math.PI * 2 * x / L);
            res += Convert.ToDouble(a23.Text.ToString()) * Math.Cos(Math.PI * 3 * x / L);
            res += Convert.ToDouble(a24.Text.ToString()) * Math.Cos(Math.PI * 4 * x / L);
            res += Convert.ToDouble(a25.Text.ToString()) * Math.Cos(Math.PI * 5 * x / L);
            res += Convert.ToDouble(a26.Text.ToString()) * Math.Cos(Math.PI * 6 * x / L);
            res += Convert.ToDouble(a27.Text.ToString()) * Math.Cos(Math.PI * 7 * x / L);
            res += Convert.ToDouble(a28.Text.ToString()) * Math.Cos(Math.PI * 8 * x / L);
            res += Convert.ToDouble(a29.Text.ToString()) * Math.Cos(Math.PI * 9 * x / L);
            if (res >= 0)
                return res;
            else
                return -1;
        }

        public void step(ref double[,] V1, ref double[,] V2, ref double[,] V3, double[] x, double h, int N,
            double tau, int cur, int next, double lymda1, double lymda2, double lymda3, double w1, double w2,
            double gamma1, double gamma2, double alpha1, double alpha2, double beta1, double beta2)
        {
            for (int i = 1; i < N; i++)
            {
                V1[next, i] = tau * lymda1 * (1 / (h * h)) * (V1[cur, i - 1] - 2 * V1[cur, i] + V1[cur, i + 1]) + tau * V1[cur, i] * (alpha1 - V1[cur, i] - w1 * V2[cur, i] - beta1 * V3[cur, i]) + V1[cur, i];

                V2[next, i] = tau * lymda2 * (1 / (h * h)) * (V2[cur, i - 1] - 2 * V2[cur, i] + V2[cur, i + 1]) + tau * V2[cur, i] * (alpha2 - V2[cur, i] - w2 * V1[cur, i] - beta2 * V3[cur, i]) + V2[cur, i];

                V3[next, i] = tau * lymda3 * (1 / (h * h)) * (V3[cur, i - 1] - 2 * V3[cur, i] + V3[cur, i + 1]) + tau * V3[cur, i] * (-1 + gamma1 * V1[cur, i] + gamma2 * V2[cur, i]) + V3[cur, i];
            }

            V1[next, 0] = V1[next, 1];
            V2[next, 0] = V2[next, 1];
            V3[next, 0] = V3[next, 1];

            V1[next, N] = V1[next, N - 1];
            V2[next, N] = V2[next, N - 1];
            V3[next, N] = V3[next, N - 1];
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void a01_TextChanged(object sender, EventArgs e)
        {

        }

        private void plotV1(int cur)
        {
            chart1.Series["Жертва1"].Points.Clear();
            for (int j = 0; j < N + 1; j++)
            {
                chart1.Series["Жертва1"].Points.AddXY(x[j], V1[cur, j]);
            }
        }

        private void plotV2(int cur)
        {
            chart2.Series["Жертва2"].Points.Clear();
            for (int j = 0; j < N + 1; j++)
            {
                chart2.Series["Жертва2"].Points.AddXY(x[j], V2[cur, j]);
            }
        }

        private void plotV3(int cur)
        {
            chart3.Series["Хищник"].Points.Clear();
            for (int j = 0; j < N + 1; j++)
            {
                chart3.Series["Хищник"].Points.AddXY(x[j], V3[cur, j]);
            }
        }

        private void plotVt1(int cur, double t)
        {
            string name = "T=" + t.ToString();
            chart1.Series.Add(name);
            chart1.Series[name].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
            for (int j = 0; j < N + 1; j++)
            {
                chart1.Series[name].Points.AddXY(x[j], V1[cur, j]);
            }
        }

        private void plotVt2(int cur, double t)
        {
            string name = "T=" + t.ToString();
            chart2.Series.Add(name);
            chart2.Series[name].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
            for (int j = 0; j < N + 1; j++)
            {
                chart2.Series[name].Points.AddXY(x[j], V2[cur, j]);
            }
        }

        private void plotVt3(int cur, double t)
        {
            string name = "T=" + t.ToString();
            chart3.Series.Add(name);
            chart3.Series[name].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
            for (int j = 0; j < N + 1; j++)
            {
                chart3.Series[name].Points.AddXY(x[j], V3[cur, j]);
            }
        }

        private void run()
        {

            int next, cur;
            while (flag)
            {
                trackBar1.Invoke(new Action(() => speed = trackBar1.Value));
                next = (time + 1) % 2;
                cur = time % 2;

                progressBar1.Invoke(new Action(() => progressBar1.Value = (int)((double)(time % ((int)(T / tau))) / (T / tau) * 100)));
                chart1.Invoke(new Action(() => plotV1(cur)));
                chart2.Invoke(new Action(() => plotV2(cur)));
                chart3.Invoke(new Action(() => plotV3(cur)));

                step(ref V1, ref V2, ref V3, x, h, N, tau, cur, next, lymda1, lymda2, lymda3, w1, w2, gamma1, gamma2, alpha1, alpha2, beta1, beta2);
                time++;
                Thread.Sleep(speed);

                Time.Invoke(new Action(() => Time.Text = "t = " + (tau * time).ToString()));
                if ((time % ((int)(T / tau))) == 0 && (time > 0))
                {
                    chart1.Invoke(new Action(() => plotVt1(cur, time * tau)));
                    chart2.Invoke(new Action(() => plotVt2(cur, time * tau)));
                    chart3.Invoke(new Action(() => plotVt3(cur, time * tau)));
                }

            }

        }


        private void button1_Click(object sender, EventArgs e)
        {
            chart1.ChartAreas[0].AxisX.Maximum = Convert.ToDouble(textBox4.Text.ToString());
            chart1.ChartAreas[0].AxisX.Minimum = Convert.ToDouble(textBox3.Text.ToString());
            chart1.ChartAreas[0].AxisY.Maximum = Convert.ToDouble(textBox6.Text.ToString());
            chart1.ChartAreas[0].AxisY.Minimum = Convert.ToDouble(textBox5.Text.ToString());
            chart2.ChartAreas[0].AxisX.Maximum = Convert.ToDouble(textBox4.Text.ToString());
            chart2.ChartAreas[0].AxisX.Minimum = Convert.ToDouble(textBox3.Text.ToString());
            chart2.ChartAreas[0].AxisY.Maximum = Convert.ToDouble(textBox6.Text.ToString());
            chart2.ChartAreas[0].AxisY.Minimum = Convert.ToDouble(textBox5.Text.ToString());
            chart3.ChartAreas[0].AxisX.Maximum = Convert.ToDouble(textBox4.Text.ToString());
            chart3.ChartAreas[0].AxisX.Minimum = Convert.ToDouble(textBox3.Text.ToString());
            chart3.ChartAreas[0].AxisY.Maximum = Convert.ToDouble(textBox6.Text.ToString());
            chart1.ChartAreas[0].AxisY.Minimum = Convert.ToDouble(textBox5.Text.ToString());

            ymax = Convert.ToDouble(textBox6.Text.ToString());

            speed = trackBar1.Value;
            flag = true;
            tau = Convert.ToDouble(textBox1.Text.ToString());
            N = (int)numericUpDown1.Value;
            T = Convert.ToDouble(textBox2.Text.ToString());
            alpha1 = Convert.ToDouble(textBox7.Text.ToString());
            alpha2 = Convert.ToDouble(textBox8.Text.ToString());
            beta1 = Convert.ToDouble(textBox9.Text.ToString());
            beta2 = Convert.ToDouble(textBox10.Text.ToString());
            w1 = Convert.ToDouble(textBox12.Text.ToString());
            w2 = Convert.ToDouble(textBox11.Text.ToString());
            gamma1 = Convert.ToDouble(textBox14.Text.ToString());
            gamma2 = Convert.ToDouble(textBox13.Text.ToString());
            lymda1 = Convert.ToDouble(textBox15.Text.ToString());
            lymda2 = Convert.ToDouble(textBox16.Text.ToString());
            lymda3 = Convert.ToDouble(textBox17.Text.ToString());
            L = Convert.ToDouble(textBox18.Text.ToString());

            B = L;
            h = (B - A) / N;
            M = (int)(T / tau);
            int n;
            n = N + 1;
            V1 = new double[2, n];
            V2 = new double[2, n];
            V3 = new double[2, n];



            x = new double[n];
            for (int i = 0; i < n; i++)
            {
                x[i] = A + i * h;
            }

            for (int i = 0; i < n; i++)
            {
                V1[0, i] = f1(x[i]);
                V2[0, i] = f2(x[i]);
                V3[0, i] = f3(x[i]);
            }

            chart1.Series.Clear();
            chart2.Series.Clear();
            chart3.Series.Clear();
            chart1.Series.Add("Жертва1");
            chart2.Series.Add("Жертва2");
            chart3.Series.Add("Хищник");
            chart1.Series["Жертва1"].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
            chart2.Series["Жертва2"].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
            chart3.Series["Хищник"].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;

            stop = true;
            button4.Text = "Стоп";

            time = 0;
            newThread = new System.Threading.Thread(this.run);
            thread = true;
            newThread.Start();

        }

        private void button2_Click(object sender, EventArgs e)
        {
            chart1.ChartAreas[0].AxisX.Maximum = Convert.ToDouble(textBox4.Text.ToString());
            chart1.ChartAreas[0].AxisX.Minimum = Convert.ToDouble(textBox3.Text.ToString());
            chart1.ChartAreas[0].AxisY.Maximum = Convert.ToDouble(textBox6.Text.ToString());
            chart1.ChartAreas[0].AxisY.Minimum = Convert.ToDouble(textBox5.Text.ToString());
            chart2.ChartAreas[0].AxisX.Maximum = Convert.ToDouble(textBox4.Text.ToString());
            chart2.ChartAreas[0].AxisX.Minimum = Convert.ToDouble(textBox3.Text.ToString());
            chart2.ChartAreas[0].AxisY.Maximum = Convert.ToDouble(textBox6.Text.ToString());
            chart2.ChartAreas[0].AxisY.Minimum = Convert.ToDouble(textBox5.Text.ToString());
            chart3.ChartAreas[0].AxisX.Maximum = Convert.ToDouble(textBox4.Text.ToString());
            chart3.ChartAreas[0].AxisX.Minimum = Convert.ToDouble(textBox3.Text.ToString());
            chart3.ChartAreas[0].AxisY.Maximum = Convert.ToDouble(textBox6.Text.ToString());
            chart1.ChartAreas[0].AxisY.Minimum = Convert.ToDouble(textBox5.Text.ToString());
            L = Convert.ToDouble(textBox18.Text.ToString());

            B = L;

            flag = false;
            stop = true;
            button4.Text = "Стоп";

            if (thread)
            {
                newThread.Abort();
                thread = false;
            }
            chart1.Series.Clear();
            chart2.Series.Clear();
            chart3.Series.Clear();
            chart1.Series.Add("Начальные условия");
            chart2.Series.Add("Начальные условия");
            chart3.Series.Add("Начальные условия");
            chart1.Series["Начальные условия"].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
            chart2.Series["Начальные условия"].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
            chart3.Series["Начальные условия"].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
            h = (B - A) / (double)numericUpDown1.Value;
            for (int i = 0; i < (int)numericUpDown1.Value + 1; i++)
            {
                chart1.Series["Начальные условия"].Points.AddXY(A + i * h, f1(A + i * h));
                chart2.Series["Начальные условия"].Points.AddXY(A + i * h, f1(A + i * h));
                chart3.Series["Начальные условия"].Points.AddXY(A + i * h, f1(A + i * h));
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            flag = false;
            stop = true;
            button4.Text = "Стоп";

            if (thread)
            {
                newThread.Abort();
                thread = false;
            }
            Thread.Sleep(1000);
            chart1.Series.Clear();
            chart2.Series.Clear();
            chart3.Series.Clear();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (stop)
            {
                if (flag)
                {
                    flag = false;
                    if (thread)
                    {
                        newThread.Abort();
                        thread = false;
                    }
                    stop = false;
                    button4.Text = "Продолжить";
                }
            }
            else
            {
                chart1.ChartAreas[0].AxisX.Maximum = Convert.ToDouble(textBox4.Text.ToString());
                chart1.ChartAreas[0].AxisX.Minimum = Convert.ToDouble(textBox3.Text.ToString());
                chart1.ChartAreas[0].AxisY.Maximum = Convert.ToDouble(textBox6.Text.ToString());
                chart1.ChartAreas[0].AxisY.Minimum = Convert.ToDouble(textBox5.Text.ToString());
                chart2.ChartAreas[0].AxisX.Maximum = Convert.ToDouble(textBox4.Text.ToString());
                chart2.ChartAreas[0].AxisX.Minimum = Convert.ToDouble(textBox3.Text.ToString());
                chart2.ChartAreas[0].AxisY.Maximum = Convert.ToDouble(textBox6.Text.ToString());
                chart2.ChartAreas[0].AxisY.Minimum = Convert.ToDouble(textBox5.Text.ToString());
                chart3.ChartAreas[0].AxisX.Maximum = Convert.ToDouble(textBox4.Text.ToString());
                chart3.ChartAreas[0].AxisX.Minimum = Convert.ToDouble(textBox3.Text.ToString());
                chart3.ChartAreas[0].AxisY.Maximum = Convert.ToDouble(textBox6.Text.ToString());
                chart1.ChartAreas[0].AxisY.Minimum = Convert.ToDouble(textBox5.Text.ToString());

                chart1.Series.Clear();
                chart2.Series.Clear();
                chart3.Series.Clear();
                chart1.Series.Add("Жертва1");
                chart2.Series.Add("Жертва2");
                chart3.Series.Add("Хищник");
                chart1.Series["Жертва1"].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
                chart2.Series["Жертва2"].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
                chart3.Series["Хищник"].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;

                stop = true;
                button4.Text = "Стоп";

                newThread = new System.Threading.Thread(this.run);
                thread = true;
                flag = true;
                newThread.Start();
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            N = (int)numericUpDown1.Value;
            L = Convert.ToDouble(textBox18.Text.ToString());
            B = L;
            h = (B - A) / (N + 1);
            tau = h * h / 10 / (Math.Max(Math.Max(Convert.ToDouble(textBox15.Text.ToString()),Convert.ToDouble(textBox16.Text.ToString())),
                Math.Max(Convert.ToDouble(textBox17.Text.ToString()), 1)));
            textBox1.Text = tau.ToString();
        }

        void messagesave()
        { 
            MessageBox.Show("Введите имя файла");
        }

        private void button6_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveFileDialog1 = new SaveFileDialog();

            saveFileDialog1.Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*";
            saveFileDialog1.FilterIndex = 1;
            saveFileDialog1.RestoreDirectory = true;
            saveFileDialog1.FileName = "Experement1.txt";

            Thread mess = new Thread(messagesave);
            mess.Start();
            Thread.Sleep(1000);

            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                mess.Abort();
                string filename = saveFileDialog1.FileName;
                if (true)
                {
                    // Create a file to write to.
                    using (StreamWriter sw = File.CreateText(filename))
                    {
                        
                        sw.WriteLine(textBox18.Text.ToString());
                        sw.WriteLine(textBox7.Text.ToString());
                        sw.WriteLine(textBox8.Text.ToString());
                        sw.WriteLine(textBox9.Text.ToString());
                        sw.WriteLine(textBox10.Text.ToString());
                        sw.WriteLine(textBox11.Text.ToString());
                        sw.WriteLine(textBox12.Text.ToString());
                        sw.WriteLine(textBox13.Text.ToString());
                        sw.WriteLine(textBox14.Text.ToString());
                        sw.WriteLine(textBox15.Text.ToString());
                        sw.WriteLine(textBox16.Text.ToString());
                        sw.WriteLine(textBox17.Text.ToString());
                        sw.WriteLine(a00.Text.ToString());
                        sw.WriteLine(a01.Text.ToString());
                        sw.WriteLine(a02.Text.ToString());
                        sw.WriteLine(a03.Text.ToString());
                        sw.WriteLine(a04.Text.ToString());
                        sw.WriteLine(a05.Text.ToString());
                        sw.WriteLine(a06.Text.ToString());
                        sw.WriteLine(a07.Text.ToString());
                        sw.WriteLine(a08.Text.ToString());
                        sw.WriteLine(a09.Text.ToString());

                        sw.WriteLine(a10.Text.ToString());
                        sw.WriteLine(a11.Text.ToString());
                        sw.WriteLine(a12.Text.ToString());
                        sw.WriteLine(a13.Text.ToString());
                        sw.WriteLine(a14.Text.ToString());
                        sw.WriteLine(a15.Text.ToString());
                        sw.WriteLine(a16.Text.ToString());
                        sw.WriteLine(a17.Text.ToString());
                        sw.WriteLine(a18.Text.ToString());
                        sw.WriteLine(a19.Text.ToString());

                        sw.WriteLine(a20.Text.ToString());
                        sw.WriteLine(a21.Text.ToString());
                        sw.WriteLine(a22.Text.ToString());
                        sw.WriteLine(a23.Text.ToString());
                        sw.WriteLine(a24.Text.ToString());
                        sw.WriteLine(a25.Text.ToString());
                        sw.WriteLine(a26.Text.ToString());
                        sw.WriteLine(a27.Text.ToString());
                        sw.WriteLine(a28.Text.ToString());
                        sw.WriteLine(a29.Text.ToString());
                    }
                }
            }
        }

        void messageload()
        {
            MessageBox.Show("Выберите подходящий файл");
        }

        private void button7_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog();

            openFileDialog1.Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*";
            openFileDialog1.FilterIndex = 2;
            openFileDialog1.RestoreDirectory = true;


            Thread mess = new Thread(messageload);
            mess.Start();
            Thread.Sleep(1000);


            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                mess.Abort();
                string path = openFileDialog1.FileName;
                using (StreamReader sr = File.OpenText(path))
                {
                    textBox18.Text = sr.ReadLine();
                    textBox7.Text = sr.ReadLine();
                    textBox8.Text = sr.ReadLine();
                    textBox9.Text = sr.ReadLine();
                    textBox10.Text = sr.ReadLine();
                    textBox11.Text = sr.ReadLine();
                    textBox12.Text = sr.ReadLine();
                    textBox13.Text = sr.ReadLine();
                    textBox14.Text = sr.ReadLine();
                    textBox15.Text = sr.ReadLine();
                    textBox16.Text = sr.ReadLine();
                    textBox17.Text = sr.ReadLine();
                    a00.Text= sr.ReadLine();
                    a01.Text = sr.ReadLine();
                    a02.Text = sr.ReadLine();
                    a03.Text = sr.ReadLine();
                    a04.Text = sr.ReadLine();
                    a05.Text = sr.ReadLine();
                    a06.Text = sr.ReadLine();
                    a07.Text = sr.ReadLine();
                    a08.Text = sr.ReadLine();
                    a09.Text = sr.ReadLine();

                    a10.Text = sr.ReadLine();
                    a11.Text = sr.ReadLine();
                    a12.Text = sr.ReadLine();
                    a13.Text = sr.ReadLine();
                    a14.Text = sr.ReadLine();
                    a15.Text = sr.ReadLine();
                    a16.Text = sr.ReadLine();
                    a17.Text = sr.ReadLine();
                    a18.Text = sr.ReadLine();
                    a19.Text = sr.ReadLine();

                    a20.Text = sr.ReadLine();
                    a21.Text = sr.ReadLine();
                    a22.Text = sr.ReadLine();
                    a23.Text = sr.ReadLine();
                    a24.Text = sr.ReadLine();
                    a25.Text = sr.ReadLine();
                    a26.Text = sr.ReadLine();
                    a27.Text = sr.ReadLine();
                    a28.Text = sr.ReadLine();
                    a29.Text = sr.ReadLine();
                }
            }
        }
    }
}
