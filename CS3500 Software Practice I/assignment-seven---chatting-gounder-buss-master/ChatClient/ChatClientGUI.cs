using Communications;
using Microsoft.Extensions.Logging;
using System.Net.Sockets;

namespace ChatClient
{
    /// <summary>
    /// Chat client GUI class that uses the networking API to communicate with
    /// the Chat server GUI
    /// </summary>
    public partial class ChatClientGUI : Form
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer clientComponents = null;

        private readonly ILogger<ChatClientGUI> _logger;

        // make network global variable to be called in the constructor 
        Networking clientNetwork;

        // global variable for our client
        string clientName;

        /// <summary>
        /// Chat client constructor
        /// </summary>
        /// <param name="logger"></param>
        public ChatClientGUI(ILogger<ChatClientGUI> logger)
        {
            _logger = logger;
            InitializeComponent();

            // disable our connected to server label
            connectedToServerLabel.Visible = false;

            // initially disable our send message button
            sendMessageButton.Visible = false;

            // create our networking objects
            clientNetwork = new Networking(_logger, onConnect, null, onMessage, '\n');

            _logger.LogDebug("Chat client is started for the first time");
        }

        /// <summary>
        /// callback method for our onConnect delegate
        /// </summary>
        /// <param name="channel"></param>
        public void onConnect(Networking channel)
        {

            // logs the users name 
            //_logger.LogInformation($"{channel.ID} has connected");
            Invoke(() =>
            {
                // writes the user that connected name
                this.MessageDisplayTextBox.Items.Add("Connected to server!");
            });


            _logger.LogInformation($"{channel.ID} has connected to server");

            // set our global variable
            clientName = this.NameTextbox.Text;

            _logger.LogInformation($"{channel.ID} has set their Command name to {clientName}");

            // send the server the connection name
            channel.Send("Command Name " + clientName);
        }

        /// <summary>
        /// The callback method for onMessage
        /// </summary>
        /// <param name="channel"></param>
        /// <param name="message"></param>
        public void onMessage(Networking channel, string message)
        {
            // if the message length is greater than 20, odds are it is a list of participants
            if (message.Length > 20)
            {
                // gets the command name substring
                string checkCommandPar = message.Substring(0, 20);

                if (checkCommandPar == "Command Participants")
                {
                    // cut the command participants 
                    string participants = message.Remove(0, 21);

                    string[] participantsSplit = participants.Split(',');

                    Invoke(() =>
                    {
                        // clear the text box incase there are duplicated
                        this.ParticipantsTextBox.Items.Clear();
                        foreach (string partipantToAdd in participantsSplit)
                        {
                            this.ParticipantsTextBox.Items.Add(partipantToAdd);
                        }
                    });

                    _logger.LogInformation($"{channel.ID}: {clientName} has requested to retrieve participants from the server");

                    return;
                }

            }

            if (message == "Server Down")
            {
                //disconnects client from server
                clientNetwork.Disconnect();

                _logger.LogDebug($"Host has shutdown the server, and {channel.ID} ({clientName}) is now disconnected");

                return;
            }

            if (message.Length > 12)
            {
                // gets the command name checkCommandName
                string checkCommandName = message.Substring(0, 12);

                //if we receive command name, return to avoid printing Command name: [IPAddress]
                //this should only be dealt with by the server
                if (checkCommandName == "Command name")
                {
                    return;
                }
            }
            Invoke(() =>
            {
                // update our message text box
                this.MessageDisplayTextBox.Items.Add(message);
            });

        }

        /// <summary>
        /// Button for connecting to server
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ConnectServerButton_Click(object sender, EventArgs e)
        {
            new Thread(() =>
            {
                try
                {
                    // whatever is typed into the server name and our set port of 11000
                    clientNetwork.Connect(this.ServerNameTextbox.Text, 11000);

                    _logger.LogDebug($"{clientName} has attempted to connect to {this.ServerNameTextbox.Text}");

                    Invoke(() =>
                    {
                        // disable our button
                        ConnectServerButton.Visible = false;
                        // set the label to visible and change size
                        connectedToServerLabel.Visible = true;
                        // create our font to be large
                        connectedToServerLabel.Font = new Font("Connect to server", 20, FontStyle.Bold);
                        // enable our send message button
                        sendMessageButton.Visible = true;
                    });

                }
                catch (SocketException)
                {
                    Invoke(() =>
                     {
                         // writes that the user could not connect to the server
                         this.MessageDisplayTextBox.Items.Add("Could not connect to server");
                     });

                    _logger.LogDebug($"{clientName} could not connect to server - {this.ServerNameTextbox.Text}");
                }

            }
                ).Start();
        }

        /// <summary>
        /// Server name textbox
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ServerNameTextbox_TextChanged(object sender, EventArgs e)
        { }

        /// <summary>
        /// User name text box
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void NameTextbox_TextChanged(object sender, EventArgs e)
        { }

        /// <summary>
        /// Text box where you type a message in
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TypeMessageTexbox_TextChanged(object sender, EventArgs e)
        { }

        /// <summary>
        /// connect to server label
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void connectedToServerLabel_Click(object sender, EventArgs e)
        { }

        /// <summary>
        /// method for our send message button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void sendMessageButton_Click(object sender, EventArgs e)
        {
            //if the client is not connected to server following the server being shutdown
            if (!clientNetwork.isConnectedCheck())
            {
                Invoke(() =>
                {
                    this.MessageDisplayTextBox.Items.Add("Not connected to server");
                    // enable our button
                    ConnectServerButton.Visible = true;
                    // set the label to not invisible and change size
                    connectedToServerLabel.Visible = false;
                    // disable our send message button
                    sendMessageButton.Visible = false;
                });

                _logger.LogDebug($"{clientName} tried to send a message but was not connected to server");
            }


            try
            {
                string messageToSend = TypeMessageTexbox.Text;

                // send the message that was typed
                clientNetwork.Send(messageToSend);

                // allows client to wait for messages 
                clientNetwork.ClientAwaitMessagesAsync(true);

                _logger.LogInformation($"{clientName} has sent: '{messageToSend}'");
            }
            catch (NullReferenceException)
            {
                this.MessageDisplayTextBox.Items.Add("Not connected to server");
                _logger.LogDebug($"{clientName} tried to send a message but was not connected to server");
            }

            catch (ObjectDisposedException)
            {
                this.MessageDisplayTextBox.Items.Add("Not connected to server");
                _logger.LogDebug($"{clientName} tried to send a message but was not connected to server");
            }
        }

        /// <summary>
        /// Button that pulls in all of our clients and displays them in the box
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RetrieveParticipantsButton_Click(object sender, EventArgs e)
        {
            if (!clientNetwork.isConnectedCheck())
            {
                Invoke(() =>
                {
                    this.MessageDisplayTextBox.Items.Add("Not connected to server");
                    // enable our button
                    ConnectServerButton.Visible = true;
                    // set the label to not invisible and change size
                    connectedToServerLabel.Visible = false;
                    // disable our send message button
                    sendMessageButton.Visible = false;

                    //Get participants list to have no one showing since no connection exists
                    this.ParticipantsTextBox.Items.Clear();
                    this.ParticipantsTextBox.Refresh();
                });

                _logger.LogDebug($"{clientName} tried to retrieve participants but was not connected to server");
            }

            Invoke(() =>
            {
                //Get participants list to have no one showing since no connection exists
                this.ParticipantsTextBox.Items.Clear();
                this.ParticipantsTextBox.Refresh();
            });

            clientNetwork.Send("Command Participants");
            clientNetwork.ClientAwaitMessagesAsync(true);

            _logger.LogInformation($"{clientName} has requested to retrieve the participants from the server");
        }

        /// <summary>
        /// Method that deals with the red x button in the chat client gui
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FormClosingX(object sender, FormClosingEventArgs e)
        {
            // notify the server that the person has closed their gui
            clientNetwork.Send("Command Closing " + clientName);
            clientNetwork.ClientAwaitMessagesAsync(true);
            clientNetwork.Disconnect();

            _logger.LogDebug($"{clientName} has closed the chat client, disconnecting them from the server");

        }
    }
}


