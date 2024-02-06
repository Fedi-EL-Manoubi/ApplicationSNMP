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
        // Déclaration du logger en tant que membre de classe
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
                    // Log si aucune réponse SNMP reçue ou peut-être dû à un OID incompréhensible de l'appareil (nvr;camera...)
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
                // Log de fin de la méthode 
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

            // OID pour l'information SNMP actuelle,OID dahua via doc internet
            var snmpOid = new ObjectIdentifier(".1.3.6.1.4.1.1004849.2.1.2.6.0");


            try
            {
                // Utilisation d'un thread asynchrone pour éviter de bloquer l'interface utilisateur
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
                    MessageBox.Show("Aucune réponse SNMP reçue.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erreur lors de la récupération des informations SNMP :( : {ex.Message}");
            }
        }


    }
}
// voir ce qu'il ce passe lors de la recupérationd des info ps: ce qui ne va pas c'est la récuperation de information par exemple jenvoie une requete uptime le nvr recois bien l reponds en envoyent la question mes le programme dit au

// Modifier la methode de récupération d'info snmp lors de la recup info du nvr vers le programme lors de l'affichage.s