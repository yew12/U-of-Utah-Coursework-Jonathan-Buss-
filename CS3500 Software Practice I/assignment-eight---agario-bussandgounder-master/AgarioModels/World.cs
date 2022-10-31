using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Drawing;
using System.Numerics;

namespace AgarioModels
{
    public class World
    {
        // dictionary, long: key, player: player
        public Dictionary<long, Player>? playerDict { get; set; }
        //list of dead players
        public Dictionary<long, Player>? deadPlayers { get; set; }

        //list of food
        public Dictionary<long, Food>? food { get; set; }

        //list of eaten food
        public Dictionary<long, Food>? eatenFood { get; set; }

        // the world Width and Height
        // (please use read only 'constants') defaulting to 5000 by 5000
        public readonly int width = 5_000;
        public readonly int height = 5_000;

        /// <summary>
        /// width property
        /// </summary>
        public int Width { get { return width; } }
        /// <summary>
        /// height property
        /// </summary>
        public int Height { get { return height; } }

        // logger object
        private readonly ILogger<World> _logger;



        //has top speed
        //low speed variables
        //size of world

        /// <summary>
        /// Constructor for our world class
        /// </summary>
        /// <param name="logger"></param>
        /// <param name=""></param>
        public World(ILogger<World> logger)
        {
            _logger = logger;

            //create the player dictionary
            playerDict = new Dictionary<long, Player>();

            //create the list for food
            food = new Dictionary<long, Food>();

            //create the dead players dictionary
            deadPlayers = new Dictionary<long, Player>();

            //create the list for eaten food
            eatenFood = new Dictionary<long, Food>();
        }

        /// <summary>
        /// Adds player list that is sent from the server
        /// 
        /// We lock our player dictionary until it is filled 
        /// on first startup of game. We do this in tandem with our
        /// food dictionary
        /// </summary>
        public void AddPlayers(List<Player> playerList)
        {
            foreach (Player player in playerList)
            {
                if (!playerDict.ContainsKey(player.ID))
                {
                    playerDict?.Add(player.ID, player);
                }
                else
                {
                    playerDict[player.ID] = player;
                }
            }

        }

        /// <summary>
        /// Adds food list that is sent from the server
        /// 
        /// We lock our food dictionary until it is filled 
        /// on first startup of game. We do this in tandem with our
        /// player dictionary
        /// </summary>
        public void AddFood(List<Food> foodList)
        {
            foreach (Food f in foodList)
            {
                food?.Add(f.ID, f);
            }

        }

        /// <summary>
        /// Removes the food that is been eaten from dictionary
        /// </summary>
        public void RemoveFood(List<int> foodList)
        {
            foreach (int foodID in foodList)
            {
                eatenFood?.Add(foodID, food[foodID]);
            } 
        }

        /// <summary>
        /// Removes the dead players from our playerDictionary
        /// </summary>
        public void RemovePlayer(List<int> playersList)
        {
            foreach (int playerID in playersList)
            {
                if (!deadPlayers.ContainsKey(playerID))
                {
                    deadPlayers?.Add(playerID, playerDict[playerID]);
                }
                else
                {
                    deadPlayers[playerID] = playerDict[playerID];
                }
            }
        }

        /// <summary>
        /// Random ARGB color generator
        /// - https://stackoverflow.com/questions/13034131/net-random-rgb-color
        /// </summary>
        /// <returns></returns>
        private int GetRandomColor()
        {
            Random rand = new Random();
            return Color.FromArgb(rand.Next(256), rand.Next(256), rand.Next(256)).ToArgb();
        }




    }
}