using System;
using Lextm.SharpSnmpLib;
using Lextm.SharpSnmpLib.Messaging;
using log4net;
using System.Net;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace ApplicationSNMP
{
    public partial class Form1 : Form
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(Form1));

        public Form1()
        {
            InitializeComponent();
            InitializeOidMappings();
            InitializeLogger();
        }

        private void InitializeOidMappings()
        {
            oidMappings = new List<OidMapping>
            {
                new OidMapping("SysNameClass", "1.3.6.1.4.1.1004849.2.1.2.7.0"),
                new OidMapping("uptime", "1.3.6.1.4.1.1004849.2.1.6.0"),
                new OidMapping("HardwareRevision", "1.3.6.1.4.1.1004849.2.1.1.2.0"),
                new OidMapping("DeviceStatus", "1.3.6.1.4.1.1004849.2.1.2.8.0"),
                new OidMapping("NomMachine", ".1.3.6.1.4.1.1004849.2.1.2.9.0"),
                new OidMapping("IpVersion", ".1.3.6.1.4.1.1004849.2.2.2.3.0"),
            };

            BoxOid1.DataSource = oidMappings;
            BoxOid1.DisplayMember = "Name";
            BoxOid1.ValueMember = "Oid";
            BoxOid1.SelectedIndex = -1;
        }

        private void InitializeLogger()
        {
            log4net.Config.XmlConfigurator.Configure(new System.IO.FileInfo("log4net.config"));
        }

        private async void Button1_Click_1(object sender, EventArgs e)
        {
            string ipAddress = TextBoxIPAddress.Text;
            string community = TextBoxCommunity.Text;

            if (!IsValidIpAddress(ipAddress) || string.IsNullOrEmpty(community))
            {
                MessageBox.Show("Veuillez fournir une adresse IP et une communauté SNMP valides.", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            string? selectedName = BoxOid1.SelectedItem?.ToString();

            if (string.IsNullOrEmpty(selectedName))
            {
                MessageBox.Show("Veuillez sélectionner un OID dans la liste.", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            var mapping = oidMappings.FirstOrDefault(m => m.Name == selectedName);

            if (mapping == null)
            {
                MessageBox.Show("Aucun OID correspondant trouvé pour le nom sélectionné.", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            var result = await Task.Run(() => QuerySnmp(ipAddress, community, mapping.Oid));

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

        private bool IsValidIpAddress(string ipAddress)
        {
            return IPAddress.TryParse(ipAddress, out _);
        }

        private static IList<Variable>? QuerySnmp(string ipAddress, string community, string oid)
        {
            var agentIpAddress = IPAddress.Parse(ipAddress);
            var port = 161;
            var target = new IPEndPoint(agentIpAddress, port);

            try
            {
                var variables = Messenger.Get(
                    VersionCode.V2,
                    target,
                    new OctetString(community),
                    new List<Variable> { new Variable(new ObjectIdentifier(oid)) },
                    5000
                );

                if (variables != null && variables.Any())
                {
                    return variables;
                }
                else
                {
                    log.Warn("Aucune réponse SNMP reçue.");
                    return null;
                }
            }
            catch (Lextm.SharpSnmpLib.Messaging.TimeoutException)
            {
                log.Error("La demande SNMP a expiré.");
                MessageBox.Show("La demande SNMP a expiré. Vérifiez les informations saisies et réessayez.", "Erreur de délai", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return null;
            }
            catch (Exception ex)
            {
                log.Error($"Erreur lors de la récupération des informations SNMP : {ex.Message}");
                MessageBox.Show($"Une erreur s'est produite lors de la récupération des informations SNMP : {ex.Message}", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return null;
            }
            finally
            {
                log.Info($"QuerySnmp ended for IP: {ipAddress}, Community: {community}, OID: {oid}");
            }
        }

        private void eXitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void UpdateFullScreenMenuItemText()
        {
            if (this.WindowState == FormWindowState.Maximized)
            {
                plaineÉcranToolStripMenuItem.Text = "Quitter le plein écran";
            }
            else
            {
                plaineÉcranToolStripMenuItem.Text = "Plein écran";
            }
        }

        private void plaineÉcranToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.WindowState = (this.WindowState == FormWindowState.Maximized) ? FormWindowState.Normal : FormWindowState.Maximized;
            UpdateFullScreenMenuItemText();
        }

        public class OidMapping
        {
            public string Name { get; set; }
            public string Oid { get; set; }

            public OidMapping(string name, string oid)
            {
                Name = name;
                Oid = oid;
                
            }
        }

        private List<OidMapping> oidMappings;
    }
}
