using System;
using Lextm.SharpSnmpLib;
using Lextm.SharpSnmpLib.Messaging;
using log4net;
using System.Net;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.Runtime.CompilerServices;
using System.Reflection.Metadata.Ecma335;

namespace ApplicationSNMP
{
    public partial class Form1 : Form
    {
        // D�claration du logger en tant que membre de classe
        private static readonly ILog log = LogManager.GetLogger(typeof(Form1));

        public Form1()
        {
            InitializeComponent();

            // Initialisation de log4net
            log4net.Config.XmlConfigurator.Configure(new System.IO.FileInfo("log4net.config"));
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
                    // Log si aucune r�ponse SNMP re�ue ou peut-�tre d� � un OID incompr�hensible de l'appareil (nvr;camera...)
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
                // Log de fin de la m�thode 
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

            // OID pour l'information SNMP actuelle,OID dahua via doc internet
            var snmpOid = new ObjectIdentifier(".1.3.6.1.4.1.1004849.2.1.2.6.0");


            try
            {
                // Utilisation d'un thread asynchrone pour �viter de bloquer l'interface utilisateur
                var result = await Task.Run(() => QuerySnmp(ipAddress, community, snmpOid));

                if (result != null && result.Any())
                {
                    // Traitez les variables individuelles dans la liste
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


    }
}
// voir ce qu'il ce passe lors de la recup�rationd des info ps: ce qui ne va pas c'est la r�cuperation de information par exemple jenvoie une requete uptime le nvr recois bien l reponds en envoyent la question mes le programme dit au

// Modifier la methode de r�cup�ration d'info snmp lors de la recup info du nvr vers le programme lors de l'affichage.s