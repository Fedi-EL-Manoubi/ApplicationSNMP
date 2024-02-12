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


        private Rectangle orignalFormSize;

        // Déclaration du logger en tant que membre de classe
        private static readonly ILog log = LogManager.GetLogger(typeof(Form1));

        public Form1()
        {
            InitializeComponent();

            // Initialisation de log4net
            log4net.Config.XmlConfigurator.Configure(new System.IO.FileInfo("log4net.config"));

            BoxOid1.DataSource = oidMappings;
            BoxOid1.DisplayMember = "Name"; // Afficher les noms des OID dans la ComboBox
            BoxOid1.ValueMember = "Oid"; // Utiliser les OID comme valeurs
            BoxOid1.SelectedIndex = -1;

            //BoxOid1.SelectedIndexChanged += BoxOid1_SelectedIndexChanged;
        }
        private static IList<Variable>? QuerySnmp(string ipAddress, string community, ObjectIdentifier snmpOid)
        {
            var agentIpAddress = IPAddress.Parse(ipAddress);
            var port = 161; // Port SNMP par défaut
            var target = new IPEndPoint(agentIpAddress, port);

            try
            {
                // Utilisation de Messenger.Get pour obtenir des variables spécifiques
                var variables = Messenger.Get(VersionCode.V2, target, new OctetString(community), new List<Variable> { new(snmpOid) }, 5000);

                if (variables != null && variables.Any())
                {
                    // Retourner les informations récupérées, peut planter si l'information get (oid) n'est pas valide a regenerer la solution.
                    return variables;
                }
                else
                {
                    //Log si aucune réponse SNMP reçue ou peut-être dû à un OID incompréhensible de l'appareil (nvr;camera...)
                    log.Warn("Aucune réponse SNMP reçue.");
                    return null;
                }
            }
            catch (Lextm.SharpSnmpLib.Messaging.TimeoutException)
            {
                // Log si la demande a expiré ou l'OID n'est pas valide
                log.Error("La demande SNMP a expiré.");

                // Affichez une MessageBox indiquant que le délai a expiré de la session.
                MessageBox.Show("La demande SNMP a expiré. Vérifiez les informations saisies et réessayez.", "Erreur de délai", MessageBoxButtons.OK, MessageBoxIcon.Error);

                return null;
            }
            catch (Exception ex) // Récupère l'info du délai Time qui est dépassée est affiche le box suivante:
            {
                // Log des erreurs
                log.Error($"Erreur lors de la récupération des informations SNMP : {ex.Message}");

                // Affichez une MessageBox pour d'autres erreurs (ici la box remonte lors d'une erreur IP ou communauté fausse ou introuvable).
                MessageBox.Show($"Une erreur s'est produite lors de la récupération des informations SNMP : {ex.Message}", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);

                return null;
            }
            finally
            {
                // Log de fin de la méthode envoie vers fichiers logs.
                log.Info($"QuerySnmp ended for IP: {ipAddress}, Community: {community}, OID: {snmpOid}");
            }
        }

        private async void Button1_Click_1(object sender, EventArgs e)
        {
            string ipAddress = TextBoxIPAddress.Text;
            string community = TextBoxCommunity.Text;

            // Validation des entrées
            if (string.IsNullOrWhiteSpace(ipAddress) || string.IsNullOrWhiteSpace(community))
            {
                MessageBox.Show("Veuillez fournir une adresse IP et une communauté SNMP valides s'il vous plait .");
                return;
            }

            // Récupérer l'OID sélectionné dans la ComboBox
            string? selectedOid = BoxOid1.SelectedItem?.ToString();

            // Vérifier si un OID a été sélectionné sinon 
            if (string.IsNullOrEmpty(selectedOid))
            {
                MessageBox.Show("Veuillez sélectionner un OID dans la liste.");
                return;
            }

            // Créer l'objet ObjectIdentifier à partir de l'OID sélectionné
            var snmpOid = new ObjectIdentifier(selectedOid);

            try
            {
                // Utilisation d'un thread asynchrone pour éviter de bloquer l'interface utilisateur
                var result = await Task.Run(() => QuerySnmp(ipAddress, community, snmpOid));

                if (result != null && result.Any())
                {
                    // Traitez les variables individuelles dans la liste des ,
                    foreach (var variable in result)
                    {
                        MessageBox.Show($"La valeur de l'OID {variable.Id} est : {variable.Data}");
                    }
                }
                else
                {
                    MessageBox.Show("Aucune réponse SNMP reçue.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erreur lors de la récupération des informations SNMP :( : {ex.Message}");
            }
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

        // Dans votre classe Form1   ComboBoxOid
        private List<OidMapping> oidMappings = new List<OidMapping>
{
    new OidMapping("SysNameClass", "1.3.6.1.4.1.1004849.2.1.2.7.0"),
    new OidMapping("UpTime", "1.3.6.1.4.1.1004849.2.1.6.0"),
    new OidMapping("HardwareRevision", "1.3.6.1.4.1.1004849.2.1.1.2.0"),
};


        private void BoxOid1_SelectedIndexChanged(object sender, EventArgs e)
        {
            string? selectedName = BoxOid1.SelectedItem?.ToString();

            if (!string.IsNullOrEmpty(selectedName))
            {
                var mapping = oidMappings.FirstOrDefault(m => m.Name == selectedName);
                if (mapping != null)
                {
                    string correspondingOid = mapping.Oid;
                    // Utilisez cet OID comme requis
                }
                else
                {
                    MessageBox.Show("Nom d'OID inconnu. Veuillez sélectionner un autre nom.");
                }
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
            if (this.WindowState == FormWindowState.Maximized)
            {
                this.WindowState = FormWindowState.Normal; // Quitte le mode plein écran
            }
            else
            {
                this.WindowState = FormWindowState.Maximized; // Passe en mode plein écran
            }
            UpdateFullScreenMenuItemText(); // Met à jour le texte de l'élément de menu
        }
        // Déclaration et initialisation de oidMappings avec quelques exemples

    }
}

// voir ce qu'il ce passe lors de la recupérationd des info ps: ce qui ne va pas c'est la récuperation de information par exemple jenvoie une requete uptime le nvr recois bien l reponds en envoyent la question mes le programme dit au

// Modifier la methode de récupération d'info snmp lors de la recup info du nvr vers le programme lors de l'affichage.s