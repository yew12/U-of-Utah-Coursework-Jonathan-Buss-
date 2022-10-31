using Communications;
using Microsoft.Extensions.Logging;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace ChatServer
{
    /// <summary>
    /// Chat server GUI class that uses the networking API to communicate
    /// with the Chat client GUI 
    /// </summary>
    public partial class ChatServerGUI : Form
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer serverComponents = null;

        private readonly ILogger<ChatServerGUI> _logger;

        // list for all the clients connected
        private List<Networking> clients = new();

        // dictionary to keep track of all the user's respective names and IP addresses
        Dictionary<string, string> clientNameDict;

        // make network global variable to be called in the constructor - listens on one port
        // can receive connection requests
        Networking serverNetwork;

        /// <summary>
        /// Chat server constructor
        /// </summary>
        /// <param name="logger"></param>
        public ChatServerGUI(ILogger<ChatServerGUI> logger)
        {
            _logger = logger;
            InitializeComponent();

            clientNameDict = new Dictionary<string, string>();

            //Diplay the MachineName you are running the server from as the server name
            ServerNameTextbox.Text = Environment.MachineName.ToString();

            //Display the IPAdress from the machine you are running the server from as the server IP
            ServerIPTextbox.Text = GetLocalIPAddress();

            serverNetwork = new Networking(_logger, onConnect, onDisconnect, onMessage, '\n');

            // turns the server on for the port 
            serverNetwork.WaitForClients(11000, true);

            _logger.LogDebug($"Server is started for the first time");
        }

        /// <summary>
        /// callback method for our onConnect delegate
        /// 
        /// update gui and manage clients
        /// </summary>
        /// <param name="channel"></param>
        public void onConnect(Networking channel)
        {
            /*channel is the serverNetworking object, it is listening and coming in this callback
             and saying let us make a connection
             */
            //IP address should be set by networking object

            // lock the list of clients, then add that users channel
            lock (this.clients)
            {
                this.clients.Add(channel);
            }
        }

        /// <summary>
        /// callback method for our onMessage delegeate
        /// 
        ///  
        /// </summary>
        /// <param name="channel"></param>
        /// <param name="message"></param>
        public void onMessage(Networking channel, string message)
        {
            lock (this.clients)
            {
                if (message.Length > 12)
                {
                    // gets the command name checkCommandName
                    string checkCommandName = message.Substring(0, 12);

                    if (checkCommandName == "Command Name")
                    {
                        // remove the command name 
                        string name = message.Remove(0, 13);
                        // add the clients IP address and their command name
                        clientNameDict.Add(channel.ID, name);
                        Invoke(() =>
                        {
                            // update GUI
                            this.ParticipantsTextbox.Items.Add($"{name}: {channel.ID} ");
                            this.MessageTextBox.Items.Add($"{channel.ID}: {message}");
                        });

                        // log the persons ip and name
                        _logger.LogInformation($"{channel.ID}: {name} has connected.");
                        return;
                    }
                }
                // if the client is asking for the list of participants
                if (message == "Command Participants")
                {
                    StringBuilder participantList = new StringBuilder("Command Participants,");
                    // loop through the clients and send back to the 
                    foreach (string participants in clientNameDict.Values)
                    {
                        if (participants.Equals(clientNameDict.Values.Last()))
                            participantList.Append(participants);
                        else
                            // add to the list plus a comma
                            participantList.Append(participants + ",");
                    }
                    // send the participants back to the channel that requested it
                    channel.Send(participantList.ToString());

                    return;
                }

                if (message.Length > 15)
                {
                    // gets the command name checkCommandName
                    string checkCommandName = message.Substring(0, 15);

                    if (checkCommandName == "Command Closing")
                    {
                        // remove the command name 
                        string name = message.Remove(0, 16);
                        // remove the clients IP address and their command name
                        clientNameDict.Remove(channel.ID);
                        // remove from the channel 
                        clients.Remove(channel);
                        Invoke(() =>
                        {
                            // update GUI
                            this.ParticipantsTextbox.Items.Remove($"{name}: {channel.ID} ");
                            this.MessageTextBox.Items.Add($"Disconnection: {name} - *{channel.ID} - Disconnected");
                            this.ParticipantsTextbox.Refresh();
                        });

                        _logger.LogInformation($"{channel.ID}: {name} has disconnected");
                        return;
                    }
                }

                Invoke(() =>
                {
                    // get the clients name based off their ip address
                    clientNameDict.TryGetValue(channel.ID, out string name);
                    // update our gui with that clients message
                    this.MessageTextBox.Items.Add(name + ": " + message);
                });

                _logger.LogInformation($"{channel.ID} sent: {message}");

                // go through and update the guis of each and every client that is connected
                foreach (var client in clients)
                {
                    // get the clients name based off their ip address
                    clientNameDict.TryGetValue(channel.ID, out string clientName);
                    client.Send(clientName + "-" + message);
                }
            }
        }

        /// <summary>
        /// call back method for using our onDisconnect delegate
        /// </summary>
        /// <param name="channel"></param>
        public void onDisconnect(Networking channel)
        {
            serverNetwork.Disconnect();
        }

        /// <summary>
        /// Participants Label
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ParticipantsLabel_Click(object sender, EventArgs e)
        {

        }

        /// <summary>
        /// Server IP Label
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void label2_Click(object sender, EventArgs e)
        { }

        /// <summary>
        /// ServerNameLabel 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ServerNameLabel_Click(object sender, EventArgs e)
        { }


        /// <summary>
        /// Participants text box
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ParticipantsTextbox_TextChanged(object sender, EventArgs e)
        { }

        /// <summary>
        /// Button for shutting down the server
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ShutdownServerButton_Click(object sender, EventArgs e)
        {
            // if you press 'Shutdown Server'
            if (ShutdownServerButton.Text == "Shutdown Server")
            {

                //iterate through each of the connected TCP Clients and disconnect clients from server
                foreach (var client in clients)
                {
                    this.MessageTextBox.Items.Add($"{client.ID} has disconnected");
                    client.Send("Server Down");
                    _logger.LogInformation($"{client.ID} has disconnected");
                }
                // clear the clients list
                clients.Clear();

                // clear our dictionary 
                clientNameDict.Clear();

                //disconnect the server itself (stop the listener)
                serverNetwork.Disconnect();

                // stop waiting for clients
                serverNetwork.StopWaitingForClients();

                _logger.LogDebug("Server is shutdown through server shutdown button.");

                //change name of button to 'Start Server'
                ShutdownServerButton.Text = "Start Server";

                this.MessageTextBox.Items.Add("Server Disconnected!");

                //Remove the clients connected to server
                this.ParticipantsTextbox.Items.Clear();
                this.ParticipantsTextbox.Refresh();
            }
            // if you press 'Start Server'
            else if (ShutdownServerButton.Text == "Start Server")
            {
                // turns the server on for the port 
                serverNetwork.WaitForClients(11000, true);

                _logger.LogDebug("Server is started back up through start server button.");

                //change name of button to 'Shutdown Server'
                ShutdownServerButton.Text = "Shutdown Server";

                this.MessageTextBox.Items.Add("Server Online!");
            }
        }

        /// <summary>
        /// Text box for our server name
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ServerNameTextbox_TextChanged(object sender, EventArgs e)
        { }

        /// <summary>
        /// Text box for our server IP textbox 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ServerIPTextbox_TextChanged(object sender, EventArgs e)
        { }

        /// <summary>
        /// Helper method for getting the IP Adress of the computer you are operating on
        /// </summary>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        private static string GetLocalIPAddress()
        {
            // Reference - https://stackoverflow.com/questions/6803073/get-local-ip-address
            var host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (var ip in host.AddressList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                {
                    return ip.ToString();
                }
            }
            throw new Exception("No network adapters with an IP address in the system!");
        }

        /// <summary>
        /// Not going to use
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ChatServerGUI_Load(object sender, EventArgs e)
        { }
    }
}