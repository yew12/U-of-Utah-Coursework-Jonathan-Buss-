using Microsoft.Extensions.Logging;
using System.Text;
using System.Collections.Generic;
using System.Net;
using System.Threading;
using System.Net.Sockets;

namespace Communications
{
    /// <summary>
    /// Class library that handles our chat server and chat client
    /// </summary>
    public class Networking : ILogger
    {

        char _terminationCharacter;
        // for the client
        private TcpClient tcpClient;

        //for the server
        TcpListener network_listener;

        // set or get the name of the client. The default is the tcp client RemoteEndPoint
        public string ID
        {
            get { return $"{tcpClient.Client.RemoteEndPoint}"; } //returns IP address
            set { ID = value; }
        }

        //Delegates called inside networking code upon the given (named) event
        public delegate void ReportMessageArrived(Networking channel, string message);
        public delegate void ReportDisconnect(Networking channel);
        public delegate void ReportConnectionEstablished(Networking channel);

        //Delegate global variables
        ReportConnectionEstablished _onConnect;
        ReportDisconnect _onDisconnect;
        ReportMessageArrived _onMessageArrived;

        ILogger _logger;

        // specialized cancellation token
        private CancellationTokenSource _WaitForCancellation;

        /// <summary>
        /// Constructor 
        /// </summary>
        /// <param name="logger"></param>
        public Networking(ILogger logger,
            ReportConnectionEstablished onConnect, ReportDisconnect onDisconnect,
            ReportMessageArrived onMessage,
            char terminationCharacter)
        {
            //Set the global delegates defined as the delegates passed in 
            _onConnect = onConnect;
            _onDisconnect = onDisconnect;
            _onMessageArrived = onMessage;

            _logger = logger;

            //Initialize TCP Client
            this.tcpClient = new TcpClient();

            _terminationCharacter = terminationCharacter;
        }

        /// <summary>
        /// Create a tcp client object and connect it to the given host/port
        /// Handle the exceptional cases of where the host/port is not available
        /// </summary>
        /// <param name="host"></param>
        /// <param name="port"></param>
        public void Connect(string host, int port)
        {

            /*Creates new tcpClient object with given host and port names
            * (override initialization in constructor)
            *
            * if we get an exception, then we will catch it in our connect to server button method */


            //check to see if already connected
            if (tcpClient.Connected)
                return;

            try
            {
                this.tcpClient = new TcpClient(host, port); // should set remote endpoint if connection is successful
                                                            //check to see if already connected
                if (tcpClient.Connected)
                    //Call the delegate for reporting a connection has been established
                    _onConnect.Invoke(this);

            }
            catch (SocketException ex)
            {
                Console.WriteLine($"Bad {ex}");
                throw ex;
            }


        }

        /// <summary>
        /// Helper method for checking in the Client/Server GUI if there is a connection 
        /// established between server and client 
        /// </summary>
        /// <returns></returns>
        public bool isConnectedCheck()
        {
            return tcpClient.Connected;
        }

        /// <summary>
        /// This method waits for messages 
        /// </summary>
        /// <param name="infinite"></param>
        public async void ClientAwaitMessagesAsync(bool infinite = true)
        {
            try
            {
                StringBuilder dataBacklog = new StringBuilder();
                byte[] buffer = new byte[4096];
                NetworkStream stream = tcpClient.GetStream();

                if (stream == null) return;

                //(If infinite is true) Infinitely await for data to come in over the tcl client. 
                while (true)
                {
                    int total = await stream.ReadAsync(buffer, 0, buffer.Length);

                    string current_data = Encoding.UTF8.GetString(buffer, 0, total);

                    dataBacklog.Append(current_data);

                    Console.WriteLine($"  Received {total} new bytes for a total of {dataBacklog.Length}.");

                    /*If a full message is received (or multiple messages) send then on to the 
                     * "client code" via the onMessage callback*/
                    while (CheckForMessage(dataBacklog, out string? message))
                    {
                        _onMessageArrived(this, message);

                    }
                    if (!infinite) break;
                }
            }
            /*Handle the exceptional case where the tcp client disconnects 
             * while the Networking object is waiting to read data*/
            catch (Exception ex)
            {
                Console.WriteLine($"oops{ex}");
            }
        }

        /// <summary>
        /// this checks for messages that are coming in
        /// </summary>
        /// <param name="data"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        private bool CheckForMessage(StringBuilder data, out string? message)
        {
            string stringData = data.ToString();

            int terminatorIndex = stringData.IndexOf(_terminationCharacter);

            if (terminatorIndex >= 0)
            {
                message = stringData.Substring(0, terminatorIndex);
                data.Remove(0, terminatorIndex + 1);
                return true;
            }

            message = null;
            return false;
        }

        /// <summary>
        /// Used by servers
        /// </summary>
        /// <param name="port"></param>
        /// <param name="infinite"></param>
        public async void WaitForClients(int port, bool infinite)
        {
            try
            {
                // for the server
                network_listener = new TcpListener(IPAddress.Any, port);

                //Start the server up
                network_listener.Start();

                // create cancellation token
                _WaitForCancellation = new();
                while (infinite)
                {

                    //Waits asnychroniously for a connection. If it exists, it will accept connection
                    TcpClient connection = await network_listener.AcceptTcpClientAsync(_WaitForCancellation.Token);

                    // create a new thread 
                    new Thread(() =>
                    {
                        // create a new networking object
                        // create the new client
                        Networking client = new Networking(_logger, _onConnect, _onDisconnect, _onMessageArrived, _terminationCharacter);
                        //set the id
                        client.tcpClient = connection;

                        client.Send($"Command name: {client.ID}");

                        // wait for those messages from the client
                        client.ClientAwaitMessagesAsync(true);

                        //Call the delegate for reporting a connection has been established - calls onConnect in server
                        _onConnect?.Invoke(client); //allows null to be passed in to onConnect
                    }).Start();
                }
            }
            catch (SocketException ex)
            {

                Console.WriteLine($"oops{ex}");
            }
            // catch if the server is disconnected
            catch (OperationCanceledException)
            {
                network_listener.Stop();
            }

        }

        /// <summary>
        /// Cancel the WaitForClients 
        /// </summary>
        public void StopWaitingForClients()
        {
            _WaitForCancellation.Cancel();
        }

        /// <summary>
        /// Close the connection to the remote host
        /// </summary>
        public void Disconnect()
        {
            try
            {
                tcpClient.Close();
                tcpClient = new TcpClient();
                //Call the delegate for reporting a message has arrived
                //_onDisconnect?.Invoke(this); //allows null to be passed in to onConnect
            }
            catch (SocketException ex)
            {
                Console.WriteLine($"oops{ex}");
            }


        }

        /// <summary>
        ///  Continuously:
        ///  1) ask the user for a message to send to the client
        ///  2) encode the message as a UTF8 byte array
        ///  3) send the message to all connected clients (warning: possible race condition from accept thread)
        ///  4) remove any closed clients
        /// </summary>
        /// 
        /// <param name="text"></param>
        public async void Send(string text)
        {
            try
            {
                text = text.Replace(_terminationCharacter, '\r') + _terminationCharacter;
                byte[] message = Encoding.UTF8.GetBytes(text);
                await tcpClient.GetStream().WriteAsync(message);
            }
            catch (SocketException)
            {
                _onDisconnect(this);
                this.tcpClient.Close();
            }

            catch (InvalidOperationException)
            {
                this.tcpClient.Close();
            }
            catch (NullReferenceException ex)
            {
                Console.WriteLine($"{ex}");
            }
        }

        // Beginning of interface methods

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TState"></typeparam>
        /// <param name="state"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public IDisposable BeginScope<TState>(TState state)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="logLevel"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public bool IsEnabled(LogLevel logLevel)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TState"></typeparam>
        /// <param name="logLevel"></param>
        /// <param name="eventId"></param>
        /// <param name="state"></param>
        /// <param name="exception"></param>
        /// <param name="formatter"></param>
        /// <exception cref="NotImplementedException"></exception>
        public void Log<TState>(LogLevel logLevel,
            EventId eventId, TState state, Exception? exception,
            Func<TState, Exception?, string> formatter)
        {
            throw new NotImplementedException();
        }
    }
}

