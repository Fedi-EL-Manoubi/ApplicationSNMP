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

        // D�claration du logger en tant que membre de classe
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
            var port = 161; // Port SNMP par d�faut
            var target = new IPEndPoint(agentIpAddress, port);

            try
            {
                // Utilisation de Messenger.Get pour obtenir des variables sp�cifiques
                var variables = Messenger.Get(VersionCode.V2, target, new OctetString(community), new List<Variable> { new(snmpOid) }, 5000);

                if (variables != null && variables.Any())
                {
                    // Retourner les informations r�cup�r�es, peut planter si l'information get (oid) n'est pas valide a regenerer la solution.
                    return variables;
                }
                else
                {
                    //Log si aucune r�ponse SNMP re�ue ou peut-�tre d� � un OID incompr�hensible de l'appareil (nvr;camera...)
                    log.Warn("Aucune r�ponse SNMP re�ue.");
                    return null;
                }
            }
            catch (Lextm.SharpSnmpLib.Messaging.TimeoutException)
            {
                // Log si la demande a expir� ou l'OID n'est pas valide
                log.Error("La demande SNMP a expir�.");

                // Affichez une MessageBox indiquant que le d�lai a expir� de la session.
                MessageBox.Show("La demande SNMP a expir�. V�rifiez les informations saisies et r�essayez.", "Erreur de d�lai", MessageBoxButtons.OK, MessageBoxIcon.Error);

                return null;
            }
            catch (Exception ex) // R�cup�re l'info du d�lai Time qui est d�pass�e est affiche le box suivante:
            {
                // Log des erreurs
                log.Error($"Erreur lors de la r�cup�ration des informations SNMP : {ex.Message}");

                // Affichez une MessageBox pour d'autres erreurs (ici la box remonte lors d'une erreur IP ou communaut� fausse ou introuvable).
                MessageBox.Show($"Une erreur s'est produite lors de la r�cup�ration des informations SNMP : {ex.Message}", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);

                return null;
            }
            finally
            {
                // Log de fin de la m�thode envoie vers fichiers logs.
                log.Info($"QuerySnmp ended for IP: {ipAddress}, Community: {community}, OID: {snmpOid}");
            }
        }

        private async void Button1_Click_1(object sender, EventArgs e)
        {
            string ipAddress = TextBoxIPAddress.Text;
            string community = TextBoxCommunity.Text;

            // Validation des entr�es
            if (string.IsNullOrWhiteSpace(ipAddress) || string.IsNullOrWhiteSpace(community))
            {
                MessageBox.Show("Veuillez fournir une adresse IP et une communaut� SNMP valides s'il vous plait .");
                return;
            }

            // R�cup�rer l'OID s�lectionn� dans la ComboBox
            string? selectedOid = BoxOid1.SelectedItem?.ToString();

            // V�rifier si un OID a �t� s�lectionn� sinon 
            if (string.IsNullOrEmpty(selectedOid))
            {
                MessageBox.Show("Veuillez s�lectionner un OID dans la liste.");
                return;
            }

            // Cr�er l'objet ObjectIdentifier � partir de l'OID s�lectionn�
            var snmpOid = new ObjectIdentifier(selectedOid);

            try
            {
                // Utilisation d'un thread asynchrone pour �viter de bloquer l'interface utilisateur
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
                    MessageBox.Show("Aucune r�ponse SNMP re�ue.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erreur lors de la r�cup�ration des informations SNMP :( : {ex.Message}");
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
                    MessageBox.Show("Nom d'OID inconnu. Veuillez s�lectionner un autre nom.");
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
                plaine�cranToolStripMenuItem.Text = "Quitter le plein �cran";
            }
            else
            {
                plaine�cranToolStripMenuItem.Text = "Plein �cran";
            }
        }

        private void plaine�cranToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (this.WindowState == FormWindowState.Maximized)
            {
                this.WindowState = FormWindowState.Normal; // Quitte le mode plein �cran
            }
            else
            {
                this.WindowState = FormWindowState.Maximized; // Passe en mode plein �cran
            }
            UpdateFullScreenMenuItemText(); // Met � jour le texte de l'�l�ment de menu
        }
        // D�claration et initialisation de oidMappings avec quelques exemples

    }
}

// voir ce qu'il ce passe lors de la recup�rationd des info ps: ce qui ne va pas c'est la r�cuperation de information par exemple jenvoie une requete uptime le nvr recois bien l reponds en envoyent la question mes le programme dit au

// Modifier la methode de r�cup�ration d'info snmp lors de la recup info du nvr vers le programme lors de l'affichage.s