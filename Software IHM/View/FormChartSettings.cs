using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace IHM_ESP.View
{
    public partial class FormChartSettings : Form
    {
        ChartSettings config;
        public FormChartSettings(ChartSettings chartConfig)
        {
            InitializeComponent();
            config = chartConfig;
            InitializeTextBox();
        }
        private void InitializeTextBox()
        {
            tb_ymax.Mask = "9999";
            tb_ymax.PromptChar = ' '; 
            tb_ymax.Text = config.y_max.ToString();

            tb_ymin.Mask = "9999";
            tb_ymin.PromptChar = ' ';
            tb_ymin.Text = config.y_min.ToString();

            tb_mult.Mask = "9.9999";
            tb_mult.PromptChar = ' ';
            tb_mult.Text = config.multiplier.ToString("0.0000");
        }

        private void btn_ok_Click(object sender, EventArgs e)
        {
            config.y_max = Convert.ToDouble(tb_ymax.Text);
            config.y_min = Convert.ToDouble(tb_ymin.Text);
            config.multiplier = Convert.ToDouble(tb_mult.Text);

            this.Close();
        }
    }
}
