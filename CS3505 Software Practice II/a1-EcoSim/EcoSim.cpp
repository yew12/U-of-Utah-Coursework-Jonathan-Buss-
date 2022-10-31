#include <iostream>
#include <cmath>
using namespace std;

/**
 * @brief:
 * Author: Gage Buss
 * Assignment 1:
 * Description: This runs a rabbit fox simulation to visually show how the populations change
 * as "time" goes on.
 */

/**
 * @brief Function that updates both the rabbit and fox populations using this function:
 * deltaRabbit = gR(1-R/K) - pRF
 * deltaFoxes = cpRF - mF
 *
 * @param g - rabbitGrowth (g): the rate of increase of rabbits
 * @param p - predationRate (p): fraction of prey eaten per predator
 * @param c - foxPreyConversion (c): fraction of eaten prey turned into new predators
 * @param m - foxMortalityRate (m): a per capita mortality rate for foxes
 * @param K - carryCapacity (K): How many rabbits can be supported by the environment
 * @param numRabbits - user input value
 * @param numFoxes - user input value
 */
void updatePopulations(double g, double p, double c, double m, double K,
                       double &numRabbits, double &numFoxes)
{
    // formulas taken from method brief
    double deltaRabbit = g * numRabbits * (1 - (numRabbits / K)) - p * numRabbits * numFoxes;
    double deltaFox = c * p * numRabbits * numFoxes - m * numFoxes;

    // add the change to current populations
    numRabbits += deltaRabbit;
    numFoxes += deltaFox;
}

/**
 * @brief This function will put the string character into the stream to be displayed.
 *
 * @param numberOfSpaces - number of spaces that will display the populations of the foxes or rabbits
 * @param character - fox or rabbit
 */
void plotCharacter(int numberOfSpaces, char character)
{
    // if number of spaces is below 1, just print the character and return
    if (numberOfSpaces < 1)
    {
        cout << character;
        return;
    }

    string space = " ";
    // creates the number of spaces required
    for (int i = 0; i < numberOfSpaces; i++)
    {
        cout << space;
    }
    // tack on character to end
    cout << character;
}

/**
 * @brief This function should draw a row of a text chart with an "F" and "r" and "*" if
 * the drawing of each would overlap. The characters should be drawn in position
 * floor(num*scale) from the LEFT margin (with the first space being position 0).
 *
 * @param numRabbits - current rabbit population
 * @param numFoxes - current fox population
 * @param scaleFactor
 */
void plotPopulations(double numRabbits, double numFoxes, double scaleFactor)
{
    // we use plotCharacter function as helper
    int rabbitPosition = floor(numRabbits * scaleFactor);
    int foxPosition = floor(numFoxes * scaleFactor);

    if (rabbitPosition == foxPosition)
    {
        // their in same position so we print '*' instead
        plotCharacter(foxPosition, '*');
    }
    else if (rabbitPosition < foxPosition)
    {
        // plot rabbit first,
        plotCharacter(rabbitPosition, 'r');
        // then fox
        plotCharacter(foxPosition - rabbitPosition - 1, 'F');
    }
    else
    {
        // plot fox first,
        plotCharacter(foxPosition, 'F');
        // then rabbit
        plotCharacter(rabbitPosition - foxPosition - 1, 'r');
    }
}

/**
 * @brief Increments the int number at the given memory address.
 *
 * @param incrementVal - gets the value at the given memory address
 */
void incrementCounter(int *incrementVal)
{
    // increments value at the given memory address
    *incrementVal = *incrementVal + 1;
}

/**
 * @brief Spins up the rabbit and fox populations.
 *
 * @param iterations - set value of 100
 * @param initialRabbitVal - user input value
 * @param initialFoxVal - user input value
 */
void runSimulation(int iterations, double initialRabbitVal, double initialFoxVal)
{
    static const double RABBIT_GROWTH = 0.2;       // rabbitGrowth (g): the rate of increase of rabbits
    static const double PREDATION_RATE = 0.0022;   // predationRate (p): fraction of prey eaten per predator
    static const double FOX_PREY_CONVERSION = 0.6; // foxPreyConversion (c): fraction of eaten prey turned into new predators
    static const double FOX_MORTALITY_RATE = 0.2;  // foxMortalityRate (m): a per capita mortality rate for foxes
    static const double CARRY_CAPACITY = 1000.0;   // carryCapacity (K): How many rabbits can be supported by the environment

    // set the parameters needed for the update equations given above
    double rabbitPopulation = initialRabbitVal;
    double foxPopulation = initialFoxVal;

    /* then run simulation for iterations steps or until the predator or prey population
    goes below 1 */
    for (int i = 0; i < iterations; i++)
    {
        if (rabbitPopulation < 0 || foxPopulation < 0)
        {
            break;
        }
        // plot populations, then
        plotPopulations(rabbitPopulation, foxPopulation, 0.1);
        cout << endl;
        // update Populations
        updatePopulations(RABBIT_GROWTH, PREDATION_RATE, FOX_PREY_CONVERSION, FOX_MORTALITY_RATE,
                          CARRY_CAPACITY, rabbitPopulation, foxPopulation);
    }
}

/**
 * @brief Main function that initializes the simulation
 *
 * @return int
 */
int main()
{
    int initialRabbitPopulation = 0;
    int initialFoxPopulation = 0;
    int initialIterations = 500;

    // asks user initial fox and rabbit values
    cout << "Please enter initial rabbit value: " << endl;
    cin >> initialRabbitPopulation;
    if (cin.fail())
    {
        cout << "Invalid rabbit input" << endl;
        exit(1);
    }
    cout << "Now enter initial fox value: " << endl;
    cin >> initialFoxPopulation;
    if (cin.fail())
    {
        cout << "Invalid fox input" << endl;
        exit(1);
    }

    // then calls runSimulation
    runSimulation(initialIterations, initialRabbitPopulation, initialFoxPopulation);
}
