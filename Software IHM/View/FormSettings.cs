using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace IHM_ESP.View
{
    public partial class FormSettings : Form
    {   
        ControllerSettings controllerSettings;
        public bool SettingsChanged { get; private set; }
        public FormSettings(ControllerSettings settings)
        {
            InitializeComponent();
            controllerSettings = settings;
            this.Text = "Ajustes";
        }

        private void FormSettings_Load(object sender, EventArgs e)
        {
            listView_settings.Items.Clear();
            PropertyInfo[] properties = typeof(ControllerSettings).GetProperties();

            foreach(var prop in properties)
            {
                foreach(Devices.ESP32RegisterAttribute attr in prop.GetCustomAttributes(typeof(Devices.ESP32RegisterAttribute)))
                {
                    ListViewItem listViewItem = new ListViewItem(attr.Description);
                    listViewItem.SubItems.Add(prop.GetValue(controllerSettings).ToString());
                    listViewItem.Tag = prop;
                    listView_settings.Items.Add(listViewItem);
                }
            }
        }

        private void listView_settings_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(listView_settings.SelectedItems.Count > 0)
            {
                PropertyInfo property = listView_settings.SelectedItems[0].Tag as PropertyInfo;
                tb_value.Text = property.GetValue(controllerSettings).ToString();

                Devices.ESP32RegisterAttribute attr = property.GetCustomAttribute<Devices.ESP32RegisterAttribute>();
                lb_min_value.Text = "Valor mínimo" + Environment.NewLine + attr.MinValue.ToString();
                lb_max_value.Text = "Valor máximo" + Environment.NewLine + attr.MaxValue.ToString();
            }
        }

        private void tb_value_Validating(object sender, CancelEventArgs e)
        {
            
        }

        private void btn_set_value_Click(object sender, EventArgs e)
        {
            Regex regex = new Regex(@"^[0-9]+$");
            if (regex.IsMatch(tb_value.Text))
            {
                if (listView_settings.SelectedItems.Count > 0)
                {
                    PropertyInfo property = listView_settings.SelectedItems[0].Tag as PropertyInfo;
                    Devices.ESP32RegisterAttribute attr = property.GetCustomAttribute<Devices.ESP32RegisterAttribute>();
                    UInt16 value = 0;
                    try
                    {
                        value = Convert.ToUInt16(tb_value.Text);
                    }catch(Exception ex)
                    {
                        MessageBox.Show("Verifique o número digitado.", "Erro ao converter valor", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                    

                    if (value <= attr.MaxValue && value >= attr.MinValue)
                    {
                        property.SetValue(controllerSettings, value);
                        listView_settings.SelectedItems[0].SubItems[1].Text = tb_value.Text;
                        SettingsChanged = true;
                    }
                    else
                        MessageBox.Show("Valor fora do intervalo válido.", "Erro ao atribuir valor", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
                MessageBox.Show("Insira apenas números inteiros dentro do intervalo adequado.", "Caracteres inválidos", MessageBoxButtons.OK, MessageBoxIcon.Warning);

        }
    }
}
