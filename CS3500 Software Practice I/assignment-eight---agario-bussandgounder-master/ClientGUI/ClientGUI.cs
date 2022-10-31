using AgarioModels;
using Communications;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using System.Diagnostics;
using System.Net.Sockets;
using System.Numerics;
using System.Text.Json;

namespace ClientGUI
{
    public partial class ClientGUI : Form
    {

        //world object
        World world;

        Networking clientNetwork;

        //factor for how much we want to zoom in the screen
        private readonly int SCALEFACTOR = 15;

        //1/2 of the side length of the box
        private readonly int OFFSET = 250;

        //the box's (grey rectangle) side length
        private readonly int SIDELENGTHOFBOX = 500;

        //logger for the client
        private readonly ILogger<ClientGUI> _logger;

        //logger for the world
        private readonly ILogger<World> worldLogger;

        //tracking the name of the user
        private string clientName;

        //playerID to be tracked
        private long playerID;

        //boolean for if the playerID has been retrieved (meaning player is loaded)
        private bool playerLoaded = false;

        //boolean for if the game has been fully started (connection occurred)
        private bool gameStarted = false;
        
        //boolean for if the playerID has been retrieved and all the players have been added to the world
        private bool playersLoaded = false;

        //vector for tracking mouse coordinates to be sent to server for moving the client (player)
        private Vector2 mouseCoordinate;

        /// <summary>
        /// clients gui constructor
        /// </summary>
        public ClientGUI(ILogger<ClientGUI> logger)
        {
            _logger = logger;


            world = new World(worldLogger);
            InitializeComponent();

            instructionsLabel.Text = "Type your name in and\npress 'enter' or " +
                "'connect' to start playing";
            instructionsLabel.Font = new Font("Type your name in andpress 'enter'\nor the " +
                "'connect' to start playing", 15, FontStyle.Bold);
            clientNetwork = new Networking(_logger, onConnect, onDisconnect, onMessage, '\n');

            this.connectServerButton.Visible = false;

            this.coordinatesLabel.Visible = false;

            // fill the server textbox with the right server host
            this.serverTextBox.Text = "localhost";

            //Reduce or prevent flickers from ocurring
            DoubleBuffered = true;

            //Hiding the message which will appear when player dies
            playerDeadLabel.Visible = false;

            // once we connect, then we draw the scene
            this.Paint += drawScene;
            var timer = new System.Windows.Forms.Timer();
            timer.Interval = 1000 / 30;  // 1000 milliseconds in a second divided by 30 frames per second
            timer.Tick += (a, b) => this.Invalidate();
            timer.Start();
        }

        /// <summary>
        /// Utilizes arrow keys to navigate around our spreadsheet grid
        /// - http://csharp.net-informations.com/gui/key-press-cs.htm
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="keyData"></param>
        /// <returns></returns>
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            // if we have an enter key, we want to connect to the server
            if (keyData == Keys.Enter)
            {
                //Start the connection
                connectClientToServer();
            }

            // if we have an space key, we want to split
            if (keyData == Keys.Space)
            {
                if(gameStarted && playerLoaded)
                    // call split only if game is started and players are loaded
                    Split();
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }

            /// <summary>
            /// callback listener for when our client connects to the server
            /// </summary>
            /// <param name="connection"></param>
            private void onConnect(Networking connection)
        {
            connection.Send(String.Format(Protocols.CMD_Start_Game, clientName));
        }

        /// <summary>
        /// Callback method for disconnection ocurring.
        /// </summary>
        /// <param name="connection"></param>
        private void onDisconnect(Networking connection)
        {
            // disconnect player
            clientNetwork.Disconnect();
            _logger.LogDebug($"{clientName} has closed the Agario.io, disconnecting them from the server");

        }

        /// <summary>
        /// listener for when the server sends the client messages
        /// </summary>
        /// <param name="connection"></param>
        /// <param name="message"></param>
        private void onMessage(Networking connection, String message)
        {
            lock (world)
            {
                // CMD_Player_Object - When a player asks to join a game, the server will create a player object
                if (message.StartsWith(Protocols.CMD_Player_Object))
                {
                    string substring = message.Substring(Protocols.CMD_Player_Object.Length);
                    long.TryParse(substring, out long ID);
                    // don't create own - need to know ID so we know which player to follow around 
                    this.playerID = ID;
                    playerLoaded = true;
                    _logger.LogDebug($"{clientName}:{playerID} has connected");
                    //update mass of player
                }
                // CMD_Food - a list of food 
                if (message.StartsWith(Protocols.CMD_Food))
                {
                    string substring = message.Substring(Protocols.CMD_Food.Length);
                    List<Food>? deserializedFood = JsonSerializer.Deserialize<List<Food>>(substring);
                    // call our helper method in world to update the food list
                    world.AddFood(deserializedFood);
                }
            // CMD_Update_Players - the location, mass, etc., of each player in the game.
            if (message.StartsWith(Protocols.CMD_Update_Players))
                {
                    string substring = message.Substring(Protocols.CMD_Update_Players.Length);
                    List<Player>? deserializedPlayers = JsonSerializer.Deserialize<List<Player>>(substring);
                    // call our helper method in world to update the player list
                    //send info to DB
                    world.AddPlayers(deserializedPlayers);
                    if (playerLoaded)
                        playersLoaded = true;
                }
                // CMD_Eaten_Food - Sent when food is eaten. 
                if (message.StartsWith(Protocols.CMD_Eaten_Food))
                {
                    string substring = message.Substring(Protocols.CMD_Eaten_Food.Length);
                    List<int>? eatenFood = JsonSerializer.Deserialize<List<int>>(substring);
                    // call our helper method in world to update the food list
                    world.RemoveFood(eatenFood);
                }
                // CMD_Dead_Players - a list of dead player's ID
                if (message.StartsWith(Protocols.CMD_Dead_Players))
                {
                    string substring = message.Substring(Protocols.CMD_Dead_Players.Length);
                    List<int>? deserializedDeadPlayers = JsonSerializer.Deserialize<List<int>>(substring);
                    // call our helper method in world to update the food list
                    world.RemovePlayer(deserializedDeadPlayers);

                    if (deserializedDeadPlayers.Contains((int)playerID))
                    {
                        Invoke(() =>
                        {
                            playerDeadLabel.Visible = true;
                            this.errorMessageLabel.Text = $"{clientName} has died! " +
                                $"Close the client and restart to play again.";

                            playerDeadLabel.Font = new Font($"{clientName} has died! " +
                                $"Close the client and restart to play again.",
                                20, FontStyle.Bold);
                            playerDeadLabel.BackColor = Color.Yellow;
                        });
                        _logger.LogInformation($"{clientName} has died");
                       
                    }
                }
                // CMD_HeartBeat - sent at completion of "game loop"
                if (message.StartsWith(Protocols.CMD_HeartBeat))
                {
                    if (gameStarted && playersLoaded)
                    {
                        float mouseX = mouseCoordinate.X;
                        float mouseY = mouseCoordinate.Y;
                        moveMouse(mouseX, mouseY);
                    }
                }

            }
            
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void nameTextBox_TextChanged(object sender, EventArgs e)
        {
            this.connectServerButton.Visible = true;
        }

        private void serverTextBox_TextChanged(object sender, EventArgs e)
        {
        }

        private void label1_Click(object sender, EventArgs e)
        {
        }

        /// <summary>
        /// connection button that requests us to connect to the server
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void connectServerButton_Click(object sender, EventArgs e)
        {
            connectClientToServer();
        }

        private void connectClientToServer()
        {
            new Thread(() =>
            {
                try
                {
                    // set our client name variable
                    this.clientName = this.nameTextBox.Text;

                    clientNetwork.Connect("localhost", 11000);

                    // we have successfully connected, so we update boolean and draw rectangle
                    gameStarted = true;

                    //Logging statements for food and players loaded
                    _logger.LogDebug("Food is loaded in");
                    _logger.LogDebug("Players are loaded in");

                    Invoke(() =>
                    {
                        this.errorMessageLabel.Text = "Connected!";
                        errorMessageLabel.Font = new Font("Connected", 20, FontStyle.Bold);
                        errorMessageLabel.BackColor = Color.LightCyan;
                        this.BackColor = Color.LightSalmon;

                        //hiding all elements for connecting to the server
                        this.playerNameLabel.Visible = false;
                        this.serverLabel.Visible = false;
                        this.serverTextBox.Visible = false;
                        this.nameTextBox.Visible = false;
                        this.connectServerButton.Visible = false;
                        this.instructionsLabel.Visible = false;
                    });
                    clientNetwork.ClientAwaitMessagesAsync(true);
                }
                catch (Exception)
                {
                    Invoke(() =>
                    {
                        // writes that the user could not connect to the server
                        this.errorMessageLabel.Text = "Could not connect to server!";
                        errorMessageLabel.Font = new Font("Could not connect to server", 20, FontStyle.Bold);
                    });
                    _logger.LogCritical("Issue connecting to server");
                }
            }).Start();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void drawScene(object? sender, PaintEventArgs e)
        {

            // have a is ready - want to know position first
            if (!gameStarted)
                return;
            if (!playersLoaded)
                return;

            lock (world)
            {
                //Rectangle
                drawRectangle(e);

                // load in the players
                paintPlayers(e);

                // load in the food values
                paintFood(e);
            }
        }

        private void drawRectangle(PaintEventArgs e)
        {
            //Rectangle
            SolidBrush brush = new(Color.Gray);

            Pen pen = new(new SolidBrush(Color.Black));

            Rectangle rect = new Rectangle(5, 5, SIDELENGTHOFBOX, SIDELENGTHOFBOX);

            e.Graphics.DrawRectangle(pen, rect);
            e.Graphics.FillRectangle(brush, rect);
        }

        private void paintPlayers(PaintEventArgs e)
        {
            foreach (Player player in world.playerDict.Values)
            {
                if (world.deadPlayers.ContainsKey(player.ID))
                    continue;
                Color c = Color.FromArgb(player.ARGBColor);
                SolidBrush brush2 = new(c);
                ConvertCoordinates(player, out float newX, out float newY, out float newRadius);
                //set the mouse coordinates to wherever the player is at 

                if (newX > SIDELENGTHOFBOX || newX < 0)
                    continue;
                if (newY > SIDELENGTHOFBOX || newY < 0)
                    continue;
                e.Graphics.FillEllipse(brush2, new Rectangle((int)newX, (int)newY, (int)newRadius,
                    (int)newRadius));
                // draws the player
                e.Graphics.DrawString(player.Name, new Font("Times New Roman", 20, FontStyle.Regular), new SolidBrush(Color.Black), newX, newY + 20);
            }
        }

        private void paintFood(PaintEventArgs e)
        {
            foreach (Food food in world.food.Values)
            {
                if (world.eatenFood.ContainsKey(food.ID))
                    continue;
                Color c = Color.FromArgb(food.ARGBColor);
                SolidBrush brush2 = new(c);
                //ensuring the food is only printed inside the box
                ConvertCoordinates(food, out float newX, out float newY, out float newRadius);
                if (newX > SIDELENGTHOFBOX || newX < 0)
                    continue;
                if (newY > SIDELENGTHOFBOX || newY < 0)
                    continue;
                //rectangle with the location and size of the food object passed in to convert coordinate
                e.Graphics.FillEllipse(brush2, newX, newY, newRadius, newRadius);
            }
        }
        /// <summary>
        /// Converts the coordinates back into "world's" 5000x5000
        /// </summary>
        /// <param name="oldCoordinate"></param>
        /// <returns></returns>
        private void ConvertMouseCoordinates(float oldX, float oldY, out float newX, out float newY)
        {
            newX = OFFSET + this.Left - oldX;
            newY = OFFSET + this.Top - oldY;
            ConvertCoordinates(world.playerDict[playerID], out float playerX, out float playerY, out float r);
            newX = playerX - newX;
            newY = playerY - newY;

            float diffX = oldX - SIDELENGTHOFBOX;
            float diffY = oldY - SIDELENGTHOFBOX;

            diffX = diffX / SIDELENGTHOFBOX * -1*world.width;
            diffY = diffY / SIDELENGTHOFBOX * -1*world.width;

            newX += diffX;
            newY += diffY;

        }
        /// <summary>
        /// Converts the coordinates 
        /// </summary>
        /// <param name="oldCoordinate"></param>
        /// <returns></returns>
        private void ConvertCoordinates(GameObject gameObject, out float newX, out float newY, out float newRadius)
        {
            
            Player gamePlayer = world.playerDict[playerID];

            //Get the difference of where the GameObject is in reference to the player playing the game
            newX = gamePlayer.X - gameObject.X; 
            newY = gamePlayer.Y - gameObject.Y;
            
            //Convert the x, y, and masses to the screen by dividing by the 5000 width and multiplying
            //by the box's side length
            newX = newX / world.width * SIDELENGTHOFBOX; 
            newY = newY / world.width * SIDELENGTHOFBOX;
            float scaledMass = gameObject.Mass * SIDELENGTHOFBOX / world.width;
            newRadius = (float)Math.Sqrt(scaledMass / Math.PI);
            newRadius *= SCALEFACTOR;
            

            //Multiply by the scale factor which we define above
            newX *= SCALEFACTOR;
            newY *= SCALEFACTOR;

            //Add the offset which will be 1/2 of the box's side length
            newX += OFFSET;
            newY += OFFSET;

        }

        private void fpsActualLabel_Click(object sender, EventArgs e)
        {
        }

        private void moveMouse(float x, float y)
        {
            ConvertMouseCoordinates(x, y, out float newX, out float newY);
            Debug.WriteLine($"X:{newX},Y:{newY}");
            clientNetwork.Send(String.Format(Protocols.CMD_Move, newX, newY));
        }

        /// <summary>
        /// Sets mouse coordinates as global variables, only updates Vector mouseCoordinate in this method
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ClientGUI_MouseMove(object sender, MouseEventArgs e)
        {
            Invoke(() =>
            {
                CoordinateText.Text = $"[{MousePosition.X}],[{MousePosition.Y}]";
                CoordinateText.Font = new Font($"[{MousePosition.X}],[{MousePosition.Y}]", 20, FontStyle.Bold);
            });

            mouseCoordinate.X = e.X;
            mouseCoordinate.Y = e.Y;
        }

        private void FormClosingX(object sender, FormClosingEventArgs e)
        {
            try
            {
                world.deadPlayers.Add(playerID, world.playerDict[playerID]);

            } 
            catch(Exception ex)
            {
                _logger.LogError($"There was an error when attempting to close the client: {ex}");
            }
        }

        /// <summary>
        /// Method for splitting a player
        /// </summary>
        private void Split()
        {
            float x = mouseCoordinate.X;
            float y = mouseCoordinate.Y;
            //get the world coordinates which will send to the server for where the split to occur
            ConvertMouseCoordinates(x, y, out float newX, out float newY);
            clientNetwork.Send(String.Format(Protocols.CMD_Split, newX, newY));
        }
    }
}