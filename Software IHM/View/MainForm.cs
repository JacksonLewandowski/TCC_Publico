using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.IO.Ports;
using System.Linq;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace IHM_ESP
{
    public partial class MainForm : Form
    {
        ESPCom espCom;
        MainMenu mainMenu;

        ChartSettings speedConfig   = new ChartSettings() { multiplier = 1, x_max = 100, x_min = 0, y_max = 2200, y_min = 0 };
        ChartSettings voltageConfig = new ChartSettings() { multiplier = 1, x_max = 100, x_min = 0, y_max = 250, y_min = 0 };
        ChartSettings currentConfig = new ChartSettings() { multiplier = 1, x_max = 100, x_min = 0, y_max = 10, y_min = 0 };

        ControllerSettings controllerSettings = new ControllerSettings();

        int roll_x = 100;
        public MainForm()
        {
            InitializeComponent();
            InitializeCharts();
            InitializeMenu();
            InitializeTextBox();

            comboBox1.Click += ComboBox1_Click;

            btn_set_pwm.Enabled = false;
            btn_set_speed.Enabled = false;
            this.Text = "Interface com o controlador";
        }

        private void ComboBox1_Click(object sender, EventArgs e)
        {
            update_com_list();
        }

        private void InitializeMenu()
        {
            mainMenu = new MainMenu();
            MenuItem File = mainMenu.MenuItems.Add("&Arquivo");
            MenuItem exportCsv = new MenuItem("&Exportar .csv");
            exportCsv.Click += ExportCsv_Click;
            File.MenuItems.Add(exportCsv);

            MenuItem settings = mainMenu.MenuItems.Add("&Ajustes");
            settings.Click += Settings_Click;
            this.Menu = mainMenu;
        }

        private void Settings_Click(object sender, EventArgs e)
        {
            if (espCom == null)
                MessageBox.Show("É preciso conectar com o controlador para utilizar essa funcionalidade");
            else
            {
                byte[] registersData = espCom.GetAllHoldingRegisters();
                controllerSettings.FillFromBytes(registersData);
                View.FormSettings form = new View.FormSettings(controllerSettings);
                form.ShowDialog();
                if(form.SettingsChanged)
                {
                    DialogResult result = MessageBox.Show("Confirmar alterações?", "Confirmar", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if(result == DialogResult.Yes)
                    {
                        bool success = espCom.SetAllHoldingRegisters(controllerSettings.ConvertToBytes());
                        if(!success)
                            MessageBox.Show("Erro ao gravar configurações", "Erro" , MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        public class Record
        {
            public double tempo { get; set; }
            public double velocidade { get; set; }
            public double tensao { get; set; }
            public double corrente { get; set; }
        }

        private void ExportCsv_Click(object sender, EventArgs e)        
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();

            saveFileDialog.Filter = "csv files (*.csv)|*.csv";
            saveFileDialog.RestoreDirectory = true;

            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                // Intervalo entre amostras
                double interval = 1;// timer_requestData.Interval / 1000.0;

                List<Record> records = new List<Record>();

                foreach (var data in chart_speed.Series["Velocidade"].Points)
                    records.Add(new Record() 
                    { 
                        tempo = data.XValue * interval, 
                        velocidade = data.YValues[0] 
                    });

                int i = 0;
                foreach(var data in chart_voltage.Series["Tensão"].Points)
                {
                    records[i].tensao = data.YValues[0];
                    i++;
                }

                i = 0;
                foreach(var data in chart_current.Series["Corrente"].Points)
                {
                    records[i].corrente = data.YValues[0];
                    i++;
                }

                string filename = saveFileDialog.FileName;
                using (var writer = new StreamWriter(filename))
                using (var csv = new CsvHelper.CsvWriter(writer, CultureInfo.InvariantCulture))
                    csv.WriteRecords(records);              
                
            }
        }

        private void InitializeTextBox()
        {
            textBox_speed.Mask = "9999";
            textBox_speed.PromptChar = ' ';
            textBox_speed.Text = trackBar_speed.Minimum.ToString();

            textBox_pwm.Mask = "999";
            textBox_pwm.PromptChar = ' ';
            textBox_pwm.Text = trackBar_pwm.Minimum.ToString();

            textBox_roll_x.Mask = "999";
            textBox_roll_x.PromptChar = ' ';
            textBox_roll_x.Text = roll_x.ToString();
        }

        private void InitializeCharts()
        {
            //Ajustes do gráfico de velocidade
            chart_speed.Titles.Add("Velocidade");
            chart_speed.ChartAreas[0].AxisX.Minimum = speedConfig.x_min;
            chart_speed.ChartAreas[0].AxisX.Maximum = speedConfig.x_max;
            chart_speed.ChartAreas[0].AxisY.Minimum = speedConfig.y_min;
            chart_speed.ChartAreas[0].AxisY.Maximum = speedConfig.y_max;
            chart_speed.ChartAreas[0].AxisY.Title = "RPM";
            chart_speed.Series.Clear();
            chart_speed.Series.Add("Velocidade");
            chart_speed.Series["Velocidade"].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;

            //Ajustes do gráfico de tensão de armadura
            chart_voltage.Titles.Add("Tensão de armadura");
            chart_voltage.ChartAreas[0].AxisX.Minimum = voltageConfig.x_min;
            chart_voltage.ChartAreas[0].AxisX.Maximum = voltageConfig.x_max;
            chart_voltage.ChartAreas[0].AxisY.Minimum = voltageConfig.y_min;
            chart_voltage.ChartAreas[0].AxisY.Maximum = voltageConfig.y_max;
            chart_voltage.Series.Clear();
            chart_voltage.Series.Add("Tensão");
            chart_voltage.Series["Tensão"].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.FastLine;

            //Ajustes do gráfico de corrente de armadura
            chart_current.Titles.Add("Corrente de armadura");
            chart_current.ChartAreas[0].AxisX.Minimum = voltageConfig.x_min;
            chart_current.ChartAreas[0].AxisX.Maximum = voltageConfig.x_max;
            chart_current.ChartAreas[0].AxisY.Minimum = voltageConfig.y_min;
            chart_current.ChartAreas[0].AxisY.Maximum = voltageConfig.y_max;
            chart_current.Series.Clear();
            chart_current.Series.Add("Corrente");
            chart_current.Series["Corrente"].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Spline;
        }

        private void trackBar_speed_Scroll(object sender, EventArgs e)
        {
            textBox_speed.Text = trackBar_speed.Value.ToString();
        }

        private void trackBar_pwm_Scroll(object sender, EventArgs e)
        {
            textBox_pwm.Text = trackBar_pwm.Value.ToString();
        }

        private void textBox_speed_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                int value = Convert.ToInt32(textBox_speed.Text);
                value = Math.Min(value, trackBar_speed.Maximum);
                value = Math.Max(value, trackBar_speed.Minimum);

                btn_set_speed_Click(this, new EventArgs());
                trackBar_speed.Value = value;
                textBox_speed.Text = value.ToString();
            }
        }

        private void textBox_pwm_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                int value = Convert.ToInt32(textBox_pwm.Text);
                value = Math.Min(value, trackBar_pwm.Maximum);
                value = Math.Max(value, trackBar_pwm.Minimum);

                btn_set_pwm_Click(this, new EventArgs());
                trackBar_pwm.Value = value;
                textBox_pwm.Text = value.ToString();
            }
        }

        private void btn_set_speed_Click(object sender, EventArgs e)
        {
            int value = Convert.ToInt16(textBox_speed.Text);
            espCom.SetRPM(value);
        }

        private void btn_set_pwm_Click(object sender, EventArgs e)
        {
            // Velocidade == 0 => ajuste manual 
            // velocidade != 0 => ajuste automático 
            espCom.SetRPM(0);

            int value = Convert.ToInt16(textBox_pwm.Text);
            espCom.SetPWM(value);
        }        

        private void update_com_list()
        {   
            comboBox1.Items.Clear();
            foreach (var port in SerialPort.GetPortNames())
                comboBox1.Items.Add(port);
        }

        private Random random = new Random();
        private void timer_update_Tick(object sender, EventArgs e)
        {
            fill_series(chart_speed.Series["Velocidade"], queue_rpm);
            fill_series(chart_voltage.Series["Tensão"], queue_voltage);
            fill_series(chart_current.Series["Corrente"], queue_current);

            chart_roll();
            try
            {
                lb_instant_rpm.Text = chart_speed.Series["Velocidade"].Points.Last().YValues[0].ToString();
                lb_instant_voltage.Text = chart_voltage.Series["Tensão"].Points.Last().YValues[0].ToString();
                lb_instant_current.Text = chart_current.Series["Corrente"].Points.Last().YValues[0].ToString();
            }
            catch(Exception ex)
            {
                
            }
            

        }

        /// <summary>
        /// Insere os valores de "queue" em "series".
        /// </summary>
        /// <param name="series">Série que será preenchida com os valores provenientes da fila.</param>
        /// <param name="queue">Fila com os valores utilizados para preencher a série.</param>
        private void fill_series(Series series, ConcurrentQueue<double> queue)
        {
            double result = 0;
            while (queue.TryDequeue(out result))
                series.Points.AddXY(series.Points.Count, result);

        }

        private void chart_roll()
        {
            if (cbox_roll_x.Checked)
            {
                if (chart_speed.Series["Velocidade"].Points.Count + 20 > chart_speed.ChartAreas[0].AxisX.Maximum)
                {
                    chart_speed.ChartAreas[0].AxisX.Minimum = chart_speed.Series["Velocidade"].Points.Count - (roll_x - 10);
                    chart_speed.ChartAreas[0].AxisX.Maximum = chart_speed.Series["Velocidade"].Points.Count + 10;

                    chart_voltage.ChartAreas[0].AxisX.Minimum = chart_voltage.Series["Tensão"].Points.Count - (roll_x - 10);
                    chart_voltage.ChartAreas[0].AxisX.Maximum = chart_voltage.Series["Tensão"].Points.Count + 10;

                    chart_current.ChartAreas[0].AxisX.Minimum = chart_current.Series["Corrente"].Points.Count - (roll_x - 10);
                    chart_current.ChartAreas[0].AxisX.Maximum = chart_current.Series["Corrente"].Points.Count + 10;
                }
            }
        }

        Point prevPosition = new Point();
        ToolTip tooltip = new ToolTip();
        private void chart_speed_MouseMove(object sender, MouseEventArgs e)
        {
            var pos = e.Location;
            if (prevPosition == pos)
                return;
            tooltip.RemoveAll();
            prevPosition = pos;
            var results = chart_speed.HitTest(pos.X, pos.Y, false,
                                            ChartElementType.DataPoint);
            foreach (var result in results)
            {
                if (result.ChartElementType == ChartElementType.DataPoint)
                {
                    var prop = result.Object as DataPoint;
                    if (prop != null)
                    {
                        var pointXPixel = result.ChartArea.AxisX.ValueToPixelPosition(prop.XValue);
                        var pointYPixel = result.ChartArea.AxisY.ValueToPixelPosition(prop.YValues[0]);
                        tooltip.Show("X=" + prop.XValue + ", Y=" + prop.YValues[0], this.chart_speed,
                                            pos.X, pos.Y - 15);
                        
                    }
                }
            }
        }        

        private void btn_connect_Click(object sender, EventArgs e)
        {
            if (espCom == null)
            {
                esp_connect();
            }else
            {
                esp_disconnect();
                espCom = null;
            }

            update_com_list();
        }

        private void esp_disconnect()
        {
            if (espCom != null)
                espCom.Disconnect();

            btn_set_speed.Enabled = false;
            btn_set_pwm.Enabled = false;
            btn_start.Enabled = false;

            btn_connect.Text = "Conectar";
            comboBox1.ResetText();
        }

        private void esp_connect()
        {
            string comPort;
            try
            {
                comPort = comboBox1.SelectedItem.ToString();
            }
            catch(Exception ex)
            {
                Debug.WriteLine(ex);
                MessageBox.Show("Selecione uma porta válida.");
                return;
            }
                
            espCom = new ESPCom(comPort, 115200);

            btn_set_speed.Enabled = true;
            btn_set_pwm.Enabled = true;
            btn_start.Enabled = true;

            btn_connect.Text = "Desconectar";
        }

        System.Threading.Thread thread_request_data = null;

        private void btn_start_Click(object sender, EventArgs e)
        {
            if(btn_start.Text == "Iniciar")
            {
                btn_start.Text = "Parar";
                espCom.Start();
                doRequest = true;
                thread_request_data = new System.Threading.Thread(thread_request_data_fn);
                thread_request_data.Start();
            }
            else
            {
                btn_start.Text = "Iniciar";
                espCom.Stop();
                doRequest = false;
            }
            System.Threading.Thread.Sleep(1000);
        }

        #region Chart_clicks
        private void chart_speed_Click(object sender, EventArgs e)
        {

        }

        private void chart_voltage_Click(object sender, EventArgs e)
        {

        }

        private void chart_current_Click(object sender, EventArgs e)
        {

        }

        private void chart_speed_DoubleClick(object sender, EventArgs e)
        {
            Form form = new View.FormChartSettings(speedConfig);
            form.ShowDialog();
            chart_update(chart_speed, speedConfig);
        }

        private void chart_voltage_DoubleClick(object sender, EventArgs e)
        {
            Form form = new View.FormChartSettings(voltageConfig);
            form.ShowDialog();
            chart_update(chart_voltage, voltageConfig);
        }

        private void chart_current_DoubleClick(object sender, EventArgs e)
        {
            Form form = new View.FormChartSettings(currentConfig);
            form.ShowDialog();
            chart_update(chart_current, currentConfig);
        }
        #endregion
        private void chart_update(System.Windows.Forms.DataVisualization.Charting.Chart chart, ChartSettings config)
        {   
            chart.ChartAreas[0].AxisY.Minimum = config.y_min;
            chart.ChartAreas[0].AxisY.Maximum = config.y_max;
        }

        private void textBox_roll_x_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.KeyCode == Keys.Enter)
            {
                btn_set_roll_Click(sender, e);
            }
        }

        private void btn_set_roll_Click(object sender, EventArgs e)
        {
            int temp = 0;
            try
            {
                temp = Convert.ToInt32(textBox_roll_x.Text);
            } catch(Exception ex)
            {                
                MessageBox.Show(ex.Message);
                return;
            }
            if (temp > 0)
                roll_x = temp;
            else
                MessageBox.Show("Valor deve ser maior que zero.");
            }

        private bool doRequest = false;
        private bool messageError = false;
        List<double> list_rpm = new List<double>();
        List<double> list_voltage = new List<double>();
        List<double> list_current = new List<double>();

        ConcurrentQueue<double> queue_rpm = new ConcurrentQueue<double>();
        ConcurrentQueue<double> queue_voltage = new ConcurrentQueue<double>();
        ConcurrentQueue<double> queue_current = new ConcurrentQueue<double>();
        private void thread_request_data_fn()
        {
            Stopwatch stopwatch = Stopwatch.StartNew();
            while (doRequest)
            {   
                byte[] data = espCom.RequestData();
                
                double voltage = 0;
                double current = 0;
                double rpm = 0;

                if (data != null)
                {   
                    voltage = voltageConfig.multiplier * ((data[0] << 8) + data[1]);
                    current = currentConfig.multiplier * ((data[2] << 8) + data[3]);
                    rpm     = speedConfig.multiplier   * ((data[4] << 8) + data[5]);

                    queue_voltage.Enqueue(voltage);
                    queue_current.Enqueue(current);
                    queue_rpm.Enqueue(rpm);

                    messageError = false;
                }                

                queue_voltage.Enqueue(voltage);
                queue_current.Enqueue(current);
                queue_rpm.Enqueue(rpm);

                while (stopwatch.ElapsedMilliseconds < 10) ;
                stopwatch.Restart();
            }
            Debug.Write("thread_request_data_fn finished");
        }

        private void simulaGrafico()
        {
            UInt16[] valores = { 100, 2, 500 };

            List<byte> lista = new List<byte>();
            for (int i = 0; i < valores.Length; i++)
            {
                lista.AddRange(BitConverter.GetBytes(valores[i]));
            }

            byte[] data = lista.ToArray();

            double voltage = voltageConfig.multiplier * ((data[1] << 8) + data[0]);
            double current = currentConfig.multiplier * ((data[3] << 8) + data[2]);
            double rpm = speedConfig.multiplier * ((data[5] << 8) + data[4]);

            queue_voltage.Enqueue(voltage);
            queue_current.Enqueue(current);
            queue_rpm.Enqueue(rpm);
        }

        private void btn_clear_Click(object sender, EventArgs e)
        {
            chart_speed.Series["Velocidade"].Points.Clear();
            chart_voltage.Series["Tensão"].Points.Clear();
            chart_current.Series["Corrente"].Points.Clear();
        }
    }
}
