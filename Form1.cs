using Lextm.SharpSnmpLib;
using Lextm.SharpSnmpLib.Messaging;
using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ApplicationSNMP
{
    public partial class Form1 : Form
    {
        private static readonly Dictionary<string, string> oidMappings = new Dictionary<string, string>
        {
            { "SysNameClass", "1.3.6.1.4.1.1004849.2.1.2.7.0" },
            { "uptime", "1.3.6.1.4.1.1004849.2.1.6.0" },
            { "HardwareRevision", "1.3.6.1.4.1.1004849.2.1.1.2.0" },
            { "DeviceStatus", "1.3.6.1.4.1.1004849.2.1.2.8.0" },
            { "NomMachine", ".1.3.6.1.4.1.1004849.2.1.2.9.0" },
            { "IpVersion", ".1.3.6.1.4.1.1004849.2.2.2.3.0" }
        };

        private static readonly ILog log = LogManager.GetLogger(typeof(Form1));

        public Form1()
        {
            InitializeComponent();
            InitializeOidDropdown();
        }

        private void InitializeOidDropdown()
        {
            BoxOid1.DataSource = new BindingSource(oidMappings, null);
            BoxOid1.DisplayMember = "Key";
            BoxOid1.ValueMember = "Value";
            BoxOid1.SelectedIndex = -1;
        }

        private async void Button1_Click_1(object sender, EventArgs e)
        {
            string ipAddress = TextBoxIPAddress.Text;
            string community = TextBoxCommunity.Text;

            if (!IsValidIpAddress(ipAddress) || !IsValidCommunity(community))
            {
                MessageBox.Show("Veuillez fournir une adresse IP et une communauté SNMP valides.", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (BoxOid1.SelectedIndex == -1)
            {
                MessageBox.Show("Veuillez sélectionner un OID dans la liste.", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            string selectedOid = (string)BoxOid1.SelectedValue;

            var result = await Task.Run(() => QuerySnmp(ipAddress, community, selectedOid));

            if (result != null && result.Any())
            {
                foreach (var variable in result)
                {
                    MessageBox.Show($"La valeur de l'OID {variable.Id} est : {variable.Data}");
                }
            }
            else
            {
                MessageBox.Show("Aucune réponse SNMP reçue.", "Avertissement", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private static bool IsValidIpAddress(string ipAddress)
        {
            return IPAddress.TryParse(ipAddress, out _);
        }

        private static bool IsValidCommunity(string community)
        {
            return !string.IsNullOrEmpty(community) && community.Length <= 20;
        }

        private static IList<Variable> QuerySnmp(string ipAddress, string community, string oid)
        {
            try
            {
                var agentIpAddress = IPAddress.Parse(ipAddress);
                var port = 161; // Port SNMP par défaut
                var target = new IPEndPoint(agentIpAddress, port);
                var snmpOid = new ObjectIdentifier(oid);

                var variables = Messenger.Get(
                    VersionCode.V2,
                    target,
                    new OctetString(community),
                    new List<Variable> { new Variable(snmpOid) },
                    5000
                );

                return variables ?? Enumerable.Empty<Variable>().ToList();
            }
            catch (Lextm.SharpSnmpLib.Messaging.TimeoutException)
            {
                log.Error("La demande SNMP a expiré.");
                MessageBox.Show("La demande SNMP a expiré. Vérifiez les informations saisies et réessayez.", "Erreur de délai", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                log.Error($"Erreur lors de la récupération des informations SNMP : {ex.Message}");
                MessageBox.Show($"Une erreur s'est produite lors de la récupération des informations SNMP : {ex.Message}", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            return null;
        }

        private void eXitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void UpdateFullScreenMenuItemText()
        {
            plaineÉcranToolStripMenuItem.Text = (this.WindowState == FormWindowState.Maximized) ? "Quitter le plein écran" : "Plein écran";
        }

        private void plaineÉcranToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.WindowState = (this.WindowState == FormWindowState.Maximized) ? FormWindowState.Normal : FormWindowState.Maximized;
            UpdateFullScreenMenuItemText();
        }
    }
}
