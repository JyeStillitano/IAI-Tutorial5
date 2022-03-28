using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicTacToe
{
    public class TicTacToeLogic
    {
        //Determines what person goes first
        private Random randomNum;
        private int firstPlayer;

        //A reference to the GUI
        //private TicTacToeGUI gui;

        //The number of moves that have been made on the GUI by both the player and the AI (this does not include depth of search by the AI)
        private int numMoves;

        public TicTacToeLogic()
        {
            randomNum = new Random();

            //Determine what player will go first. 0 for human, 1 for AI
            firstPlayer = initialPlayer();
            gui = new TicTacToeGUI(firstPlayer);

            gameLoop();
        }

        /**
         * This function will keep running for as long as the user wishes to conitune playing the game. It makes sure to reinitialise the
         * game board and logic each run through.
         */
        private void gameLoop()
        {
            bool cont = false;

            //Continue looping while the user wishes to play again
            do
            {
                numMoves = 0;
                int gameOver = 2;

                //Keep running through the same game while there have not been 9 moves, and while the game has not been one by either player
                while (gameOver == 2 && numMoves < 9)
                {
                    //Determine whether it is the player or the AI that goes first, and allow the appropraite move
                    if (firstPlayer == 0)
                    {
                        gui.makePlayerMove();
                        numMoves++;

                        //No player can have won the game in under 5 moves, so don't bother checking
                        if (numMoves >= 4)
                        {
                            gameOver = checkWin(gui.getButtonStates());

                            //If the number of moves has reached 9, or gameOver is not 2 (which represents neither playerr has won yet), break from the loop to display winner
                            if (numMoves == 9 || gameOver != 2)
                                break;
                        }

                        //Allow the AI to figure out what move it would like to make
                        determineMoveSet();
                    }
                    else
                    {
                        //The comments in the section above apply to here as well, my functional decomposition was not as good as it could have been. If you have the time,
                        //you should try to rewrite the program so that it follows better convention.
                        determineMoveSet();
                        numMoves++;

                        //No player can have won the game in under 5 moves, so don't bother checking
                        if (numMoves >= 4)
                        {
                            gameOver = checkWin(gui.getButtonStates());

                            if (numMoves == 9 || gameOver != 2)
                                break;
                        }

                        gui.makePlayerMove();
                    }

                    numMoves++;

                    //No player can have won the game in under 5 moves, so don't bother checking
                    if (numMoves >= 4)
                    {
                        gameOver = checkWin(gui.getButtonStates());

                        if (gameOver != 2)
                            break;
                    }
                }

                String message = "";

                //Determine how the game ended and show the appropriate message
                if (gameOver == 2)
                    message = "Draw!";
                else
                    message = gameOver == 0 ? "Player won!" : "Computer won!";

                //Prompt the player to determine if they would like to play again
                cont = gui.gameOver(message);

                //Reinitialise the game board if so
                if (cont)
                {
                    gui.resetGameBoard();
                    firstPlayer = initialPlayer();
                }
            }
            while (cont);
        }

        /**
         * Randomly determines who will be the first player to act on the game board.
         * 
         * @return - The player that will go first: 0 for player, 1 for AI
         */
        public int initialPlayer()
        {
            return randomNum.Next(2);
        }

        /**
         * Allow the computer to determine the BEST possible move. This will be run each time the computer gets a turn.
         */
        public void determineMoveSet()
        {
            //This gets the state of the gameboard from the GUI. This will be rused as the initial node in the search.
            int[] gameBoard = gui.getButtonStates();


        }

        /**
         * Keeps passing cost up parents when parents have run out of children to explore. This is a recursive function in the case of 
         * parents who have no more children to visit.
         * 
         * @param state - The node that requires parent checking
         */
        public void parentChecking(State state)
        {

        }

        /**
         * Checks to see if the current state is a leaf node and if so, passes the node to the parent function to determine if the 
         * node should be made the favorite child. If not, check all of the possible moves given the state of the board and add each new node to the frontier.
         * 
         * @param state - The configuration of the game to check
         */
        private void examineState(State state)
        {

        }


        /**
         * Check to see if either player has gotten 3 in a row. Assumes game board is represented from top left, to bottom right
         * as in reading English, from 0 - 8
         * 
         * return = 0: player wins, 1: AI wins, 2: no winner yet
         */
        public int checkWin(int[] state)
        {
            //First row
            if (state[0] == state[1])
                if (state[1] == state[2])
                    return state[0];

            //Second row
            if (state[3] == state[4])
                if (state[4] == state[5])
                    return state[3];

            //Third row
            if (state[6] == state[7])
                if (state[7] == state[8])
                    return state[6];

            //First column
            if (state[0] == state[3])
                if (state[3] == state[6])
                    return state[0];

            //Second column
            if (state[1] == state[4])
                if (state[4] == state[7])
                    return state[1];

            //Third column
            if (state[2] == state[5])
                if (state[5] == state[8])
                    return state[2];

            //Diagonal top left
            if (state[0] == state[4])
                if (state[4] == state[8])
                    return state[0];

            //Diagonal top right
            if (state[2] == state[4])
                if (state[4] == state[6])
                    return state[2];

            //No winner has been found, as none of the win checks were successful        
            return 2;
        }
    }

}
