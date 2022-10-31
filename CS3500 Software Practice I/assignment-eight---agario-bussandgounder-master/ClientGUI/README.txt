```
Authors:    Jonathan Gage Buss & Sanjay Gounder
Partner:    Sanjay Gounder, Jonathan Gage Buss
Date:       16-April-2022
Course:     CS 3500, University of Utah, School of Computing
GitHub ID:  yew12 & sanjaygounder
Repo:       https://github.com/Utah-School-of-Computing-de-St-Germain/assignment-seven---chatting-gounder-buss.git
Commit #:   588f6f9796c56589e4fb0d2e1cb230d3c494227b
Project:    ClientGUI
Copyright:  CS 3500, Jonathan Gage Buss and Sanjay Gounder - This work may not be copied for use in Academic Coursework.
```

# Comments to Evaluators:
This assignment gave us lots of trouble. We got the zoom working great and the players and food painted
on properly, but had some slight issues with displaying data due to the multi-threading. The move works, but it can move jaggedly. 
We believe there is an issue with our convert coordinates for a mouse when we translate back
into the world's 5000x5000 coordinate system. The coordinates will seem off a little sometimes. The coordinate 
system is defined somehow as 0,0 in the bottom right corner, and the top left is 5000,5000

When a player grows, I noticed the origin for the circle being drawn is the bottom left corner of the circle.
We noticed this feature because the label is printed with no offset for x, but with a +20 for the y-coordinate. 
So, as the circle grows it will grow in a right and upward trajectory instead of growing in from the center of
where it was at. I tried to remedy this issue by having the circle spawn at an offset of the radius in both
the x and y direction, but I don't think this was correct logic. So, we simply left it as it was.
It also gets harder to move as fast as you get bigger.

Finally, data was giving us trouble displaying. Data such as heartbeats, number of food left, fps, and even
the world coordinates, was being difficult due to the multi-threading. So, we removed it and simply show
mouse screen coordinates. 

We know this code is not the best, but it did teach us lots about the math and intricacies which may go into
game design.

# Assignment Specific Topics

See above for our issues with the code.

# Consulted Peers: 

Consulted several TA's and other members in the CADE lab that are in the same class about how they 
approached the assignment. 

# References:
	
	1. Youtube
		"How to fix blurry Windows Forms Windows in high-dpi settings | C# Winforms"
		- https://www.youtube.com/watch?v=-pmER189dWQ


