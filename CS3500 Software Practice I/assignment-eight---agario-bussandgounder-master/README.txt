```
Author:     Jonathan Gage Buss & Sanjay Gounder
Partner:    Sanjay Gounder
Date:       6-April-2022
Course:     CS 3500, University of Utah, School of Computing
GitHub ID:  yew12 & sanjaygounder
Repo:       https://github.com/Utah-School-of-Computing-de-St-Germain/assignment-seven---chatting-gounder-buss.git
Commit #:   588f6f9796c56589e4fb0d2e1cb230d3c494227b
Solution:   Agario
Copyright:  CS 3500, Sanjay Gounder and Jonathan Gage Buss - This work may not be copied for use in Academic Coursework.
```
 
# Overview of the our Chat Program
In this solution, my partner and I have created a client program that allows the user of this code the ability to 
connect to a server and play a game with similiar functionality to Agar.io. All this is done through a networking API that essentially
builds that connection to allow communication between the client and the server.

The server was provided to us by the CS3500 department as was a Protocols API dealing with commands to be sent
and received from the server and client which will lead to actions in the game such as zooming in the screen,
splitting a player, and moving the player around. 

We also have included the models for the game such as players, food, and the world itself. The players and 
food all have properties for JSON Serialization/Deserialization. The world has backing structures of dictionaries
containing all the players and food to paint as well as the players and food which are dead and eaten to not
draw.

# Time Expenditures:

    1. Assignment One:   Predicted Hours:        10        Actual Hours:       12
    2. Assignment Two:   Predicted Hours:        10        Actual Hours:       11.5 
    3. Assignment Three: Predicted Hours:        10        Actual Hours:       14 
    4. Assignment Four:  Predicted Hours:        8         Actual Hours:       11 
    5. Assignment Five:  Predicted Hours:        12        Actual Hours:       10.5 
    6. Assignment Six:   Predicted Hours:        10        Actual Hours:       14 
    7. Assignment Seven: Predicted Hours:        12        Actual Hours:       27
    8. Assignment Eight: Predicted Hours:        15        Actual Hours:       35

