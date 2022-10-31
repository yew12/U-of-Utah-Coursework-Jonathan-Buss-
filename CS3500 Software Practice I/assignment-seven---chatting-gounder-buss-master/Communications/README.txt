```
Authors:    Jonathan Gage Buss & Sanjay Gounder
Partner:    Sanjay Gounder, Jonathan Gage Buss
Date:       4-April-2022
Course:     CS 3500, University of Utah, School of Computing
GitHub ID:  yew12 & sanjaygounder
Repo:       https://github.com/Utah-School-of-Computing-de-St-Germain/assignment-seven---chatting-gounder-buss.git
Commit #:   bbb42c8d7ce98c94ab03dfdca61d5cbb93e1d9d1
Project:    Networking
Copyright:  CS 3500, Jonathan Gage Buss and Sanjay Gounder - This work may not be copied for use in Academic Coursework.
```

# Comments to Evaluators:

Coordinating the connection between the ChatClient GUI and the ChatServer GUI via the Networking.cs class
was very confusing and took us lots of time to understand the functionalities of Send() and 
ClientAwaitMessagesAsync(). Once we figured out how these two methods help us connect between server and
client the assignment felt more managable. We created one non-API specific method which needed to be 
accessed outside the Networking class called isConnectedCheck. We figured this was a useful method
for essentially checking if the client had a connection established and would then return true or false.
We worked on making the Networking class as 'generic' as possible to be able to utilized in a variety of
different functionalities in the future.

# Assignment Specific Topics

One issue we ran across was working with multiple threads and when/how to create a new thread. Understanding
how an asynchronous method works took a little time too.

# Consulted Peers: 

Consulted several TA's and other members in the CADE lab that are in the same class about how they 
approached the assignment. 

# References:
	No references for this class.