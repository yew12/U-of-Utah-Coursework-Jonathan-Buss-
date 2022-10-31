```
Authors:    Jonathan Gage Buss & Sanjay Gounder
Partner:    Sanjay Gounder, Jonathan Gage Buss
Date:       4-April-2022
Course:     CS 3500, University of Utah, School of Computing
GitHub ID:  yew12 & sanjaygounder
Repo:       https://github.com/Utah-School-of-Computing-de-St-Germain/assignment-seven---chatting-gounder-buss.git
Commit #:   ed0b09f9a9f17db8a647ea44ea0c9bc4aa25e856
Project:    ChatServer
Copyright:  CS 3500, Jonathan Gage Buss and Sanjay Gounder - This work may not be copied for use in Academic Coursework.
```

# Comments to Evaluators:

Coordinating the connection between the ChatClient GUI and the ChatServer GUI via the Networking.cs class
was very confusing and took us lots of time to understand the functionalities of Send() and 
ClientAwaitMessagesAsync(). Once we figured out how these two methods help us connect between server and
client the assignment felt more managable. We added one method to the GUI to get the IP address
for whatever computer you are running the server from. 

# Assignment Specific Topics

This GUI allows a 'server' to establish connection between multiple 'clients', or 'users', and send messages
from one 'client' to other 'clients.' Graders will notice in the onMessage callback we have
multiple checks for specific strings, such as 'Command Participants','Command Closing', and 'Command Name'. 
With these checks we would do certain actions. 

For example, if a client's form is closed, this action means the client is disconnecting. So, the client 
sends a message to the server which we labeled to be "Command Closing" followed by the client's name. 
The server, in the onMessage callback, will receive this message. To check it is this specific message, 
we check if there are enough characters to first get the substring of what would be 'command closing.' We 
then see if the substring of that message equals "Command Closing," and then we take the necessary actions 
for removing the client from the list of clients connected and displaying a message that the client has 
disconnected. We have documented our code to explain these actions taken and why.

# Consulted Peers: 

Consulted several TA's and other members in the CADE lab that are in the same class about how they 
approached the assignment. 

# References:
	1. Stack Overflow 
		"Get Local IP Address"
		- https://stackoverflow.com/questions/6803073/get-local-ip-address
	
	2. Stack Overflow
		"Difference between delegates and callbacks"
		- https://stackoverflow.com/questions/290819/are-delegates-and-callbacks-the-same-or-similar#:~:text=The%20calling%20class%20sets%20the,to%20the%20calling%20classes%20callback.

	2. Coding.Vision
		"C# Simple TCP Server"
		- https://codingvision.net/c-simple-tcp-server
	
	3. Youtube
		"How to fix blurry Windows Forms Windows in high-dpi settings | C# Winforms"
		- https://www.youtube.com/watch?v=-pmER189dWQ