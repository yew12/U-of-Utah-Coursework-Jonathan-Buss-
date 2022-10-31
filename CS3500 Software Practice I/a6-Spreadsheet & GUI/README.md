```
Author:     Jonathan Gage Buss & Sanjay Gounder
Partner:    Sanjay Gounder
Date:       3-March-2022
Course:     CS 3505, University of Utah, School of Computing
GitHub ID:  yew12 & sanjaygounder
Repo:       https://github.com/Utah-School-of-Computing-de-St-Germain/assignment-six---spreadsheet-and-gui-buss-gounder
Commit #:   5e9b90e 
Solution:   SpreadsheetGUI
Copyright:  CS 3500, Sanjay Gounder and Jonathan Gage Buss - This work may not be copied for use in Academic Coursework.
```

# Overview of the Spreadsheet functionality

The Spreadsheet program is currently capable of taking in a string expression and parsing it to return
the integer answer to that number. It now contains a Dependency Graph library that will be later utilized
when a user has an input similar to "A1 + A3", and we first need to find the outputs of those two cells before
we can compute that expression. After our third assignment we now have a generic formula class which is a re-factored
Evaluator class. Finishing up the fourth assignment, we now have a spreadsheet class that essentially sets 
the contents of a given cell. With the fifth assignment finished, we have completely finished the Modal part of our project. 
We have a working back-end with just the GUI to install now. 

Future extensions are ... GUI. Hopefully a fully working spreadsheet too.

# Time Expenditures:

    1. Assignment One:   Predicted Hours:        10        Actual Hours:       12
    2. Assignment Two:   Predicted Hours:        10        Actual Hours:       11.5 
    3. Assignment Three: Predicted Hours:        10        Actual Hours:       14 
    4. Assignment Four:  Predicted Hours:        8         Actual Hours:       11 
    5. Assignment Five:  Predicted Hours:        12        Actual Hours:       10.5 
    6. Assignment Six:   Predicted Hours:        10        Actual Hours:       14 
    7. Assignment Seven: Predicted Hours:                  Actual Hours:     


# Examples of Good Software Practice (GSP)

## Well named, commented, short methods that do a specific job and return a specific result:
Thus far I have done a good job at explaining my methods with comments and even explaining a line of code when it is 
obvious what it is doing. I think this helps me and hopefully the readers of my code on what it is doing when you are reading
it line by line. 

## DRY (Dont Repeat Yourself):
I have tried my best to utilize code that is already written for myself. If I have a group of code that could be thrown into
a helper method, I have tried to see that and do that so when I have to go back to fix any bugs, it has made my life much easier
since I can narrow down the parts of my code that are breaking. One thing I need to do is look for opportunities to add extensions. 

## Separation of concerns:
As each assignment goes by, I have been able to make as many helper methods that are needed and I think it really started 
to show in Assignment 3, in my helper method. I have several helper methods that each do their own thing. This has made my code
much easier to read and understand. 

## Understanding the requirments:
In each of the assignments, I have tried my best to understand what each assignment is asking which allows me to think of 
tests to have prior to writing the code. This also began to show in Assignment 3 where I had a higher number of tests compared 
to Assignment 2 by around 30 more tests. 

## Code re-use:
In the last assignment (A4), I was able to do a good job in re-using code from previous assignments to accomplish all of the 
requirements. While reading the documentation, the author (unsure as of who wrote it) alluded to using code from assignments 
prior to the fourth one and it would allow us to only write a minimal amount of new code. The biggest problem was figuring out 
how to accomplish this, which I thought I did pretty well. I ended up only writing a small amount of new code and utilizing 
a lot of my old code saving me a lot of time debugging. 

# Time Estimates Review

I think that I am getting better at estimating the time in how long it takes me (or a partner) to finish the given assignment.
Some weeks I do feel like I am off on my time estimates and I think that shows in my time trackers above. As long as I am relatively 
close to the time estimate, then I think it I am making progress. 
