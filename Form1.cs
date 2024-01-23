using Lextm.SharpSnmpLib;
using Lextm.SharpSnmpLib.Messaging;
using System.Net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ApplicationSNMP
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private async void button1_Click_1(object sender, EventArgs e)
        {
            string ipAddress = textBoxIPAddress.Text;
            string community = textBoxCommunity.Text;

            // Validation des entrées
            if (string.IsNullOrWhiteSpace(ipAddress) || string.IsNullOrWhiteSpace(community))
            {
                MessageBox.Show("Veuillez fournir une adresse IP et une communauté SNMP validess s'il vous plait .");
                return;
            }

            // OID pour l'information SNMP actuelle,OID dahua via doc internet
            var snmpOid = new ObjectIdentifier(" 1.3.6.1.4.1.1004849.2.10.3");

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
        private IList<Variable> QuerySnmp(string ipAddress, string community, ObjectIdentifier snmpOid)
        {
            var agentIpAddress = IPAddress.Parse(ipAddress);
            var port = 161; // Port SNMP par défaut
            var target = new IPEndPoint(agentIpAddress, port);

            //  de Messenger.Walk pour parcourir l'arborescence SNMP
            var rowCount = Messenger.Walk(VersionCode.V2, target, new OctetString(community), snmpOid, new List<Variable>(), 5000, WalkMode.WithinSubtree);

            if (rowCount > 0)
            {
                MessageBox.Show($"Nombre de lignes dans la table SNMP : {rowCount}");

                //   Messenger.Get  obtenir des variables spécifiques
                var variables = Messenger.Get(VersionCode.V2, target, new OctetString(community), new List<Variable> { new Variable(snmpOid) }, 5000);

                if (variables != null && variables.Any())
                {
                    // Traitez les variables individuelles dans la liste ; message box renvoi résultat box.
                    foreach (var variable in variables)
                    {
                        MessageBox.Show($"La valeur de l'OID {variable.Id} est : {variable.Data}");
                    }

                    return variables;
                }
                else
                {
                    MessageBox.Show("Aucune réponse SNMP reçue.");
                    return null;
                }
            }
            else
            {
                MessageBox.Show("La marche SNMP n'a pas retourné de résultats.");
                return null;
            }
        }
    }
}
