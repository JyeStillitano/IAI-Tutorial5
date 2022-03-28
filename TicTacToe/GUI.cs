using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicTacToe
{
    /**
 * Displays the Tic Tac Toe game board, and allows the user to interact with it.
 * 
 * @author Steven Morris
 * @version 0.1
 */

    public class GUI
    {
        private JFrame frame;
        private JPanel gamePanel;

        private JTextArea messages;
        private ArrayList<DrawButton> buttons;

        private int winsAI;
        private int winsPlayer;
        private int draws;
        private JLabel aiLabel;
        private JLabel playerLabel;
        private JLabel drawsLabel;

        //0 for Person, 1 for AI. AI will always be crosses, but crosses can go first
        private int playerTurn;

        //Tells the game to wait for a move to be made
        private boolean turnOver;

        public GUI(int playerTurn)
        {
            frame = new JFrame("Tic Tac Toe");

            this.playerTurn = playerTurn;
            winsAI = winsPlayer = draws = 0;
            turnOver = false;
            buttons = new ArrayList<DrawButton>();

            setPanels();

            frame.setDefaultCloseOperation(JFrame.DISPOSE_ON_CLOSE);
            frame.setSize(500, 600);
            frame.setVisible(true);
        }

        /**
         * Allows the AI to mark the square it has chosen
         *
         * @param compMove - The move the computer wants to make
         */
        public void makeComputerMove(State compMove)
        {
            buttons.get(compMove.getMoveMade()).clickedOn(1);
            playerTurn = playerTurn == 1 ? 0 : 1;
        }

        /**
         * Allows the player to make a move, but prevents the AI from doing so
         */
        public void makePlayerMove()
        {
            while (!getTurnOver()) { }

            setTurnOver(false);
        }

        /**
         * Presents the game over screen. Allow player to restart or exit
         * 
         * @param message - How the game ended
         */
        public boolean gameOver(String message)
        {
            int ans = JOptionPane.showConfirmDialog(frame, message + " Would you like to play again?"
                    , "Game Over", JOptionPane.YES_NO_OPTION);

            switch (message)
            {
                case "Draw!":
                    draws++;
                    drawsLabel.setText(draws + "");
                    break;
                case "Player won!":
                    winsPlayer++;
                    playerLabel.setText(winsPlayer + "");
                    break;
                case "Computer won!":
                    winsAI++;
                    aiLabel.setText(winsAI + "");
                    break;
            }

            return ans == 0 ? true : false;
        }

        /**
         * A method that determines if the turn is over or not. It is synchronized as more than one thread tries to access at a time
         *
         * @return - The value of turnOver
         */
        public synchronized boolean getTurnOver()
        {
            return turnOver;
        }

        /**
         * Sets the value of turnOver through a synchornized accessor.
         * 
         * @param val - The new value of turnOver
         */
        public synchronized void setTurnOver(boolean val)
        {
            turnOver = val;
        }

        /**
         * Finds out what buttons have been clicked, and by what player
         * 
         * return - An array of the button states
         */
        public int[] getButtonStates()
        {
            int[] states = new int[9];
            int i = 0;

            for (DrawButton b : buttons)
            {
                states[i] = b.getState();
                i++;
            }

            return states;
        }

        /**
         * Sets up the components that will be displayed
         */
        private void setPanels()
        {
            JPanel mainPanel = new JPanel(new BorderLayout());
            gamePanel = new JPanel(new GridLayout(3, 3));

            //Set up the panel that will display number of games won by each player, and drawn
            JPanel winsPanel = new JPanel(new GridLayout(1, 3));

            JPanel playerPanel = new JPanel();
            playerPanel.add(new JLabel("Player: "));
            playerLabel = new JLabel(winsPlayer + "");
            playerPanel.add(playerLabel);

            JPanel aiPanel = new JPanel();
            aiPanel.add(new JLabel("Computer: "));
            aiLabel = new JLabel(winsAI + "");
            aiPanel.add(aiLabel);

            JPanel drawsPanel = new JPanel();
            drawsPanel.add(new JLabel("Draws: "));
            drawsLabel = new JLabel(draws + "");
            drawsPanel.add(drawsLabel);

            winsPanel.add(playerPanel);
            winsPanel.add(aiPanel);
            winsPanel.add(drawsPanel);

            setGamesPanel();

            //Set up the area where the messages will show
            messages = new JTextArea(5, 30);
            JScrollPane scrollPane = new JScrollPane(messages);
            scrollPane.setHorizontalScrollBarPolicy(ScrollPaneConstants.HORIZONTAL_SCROLLBAR_NEVER);
            messages.setEditable(false);

            mainPanel.add(winsPanel, BorderLayout.NORTH);
            mainPanel.add(gamePanel, BorderLayout.CENTER);
            mainPanel.add(scrollPane, BorderLayout.SOUTH);

            frame.add(mainPanel);
        }

        /**
         * Sets up the buttons ready for display
         */
        private void setGamesPanel()
        {
            //Set up the game panel
            for (int i = 0; i < 9; i++)
            {
                DrawButton button = new DrawButton();

                button.addActionListener(new ActionListener()
                {
                public void actionPerformed(ActionEvent e)
                {
                    ((DrawButton)e.getSource()).clickedOn(0);

                    playerTurn = playerTurn == 1 ? 0 : 1;
                    setTurnOver(true);
                }
            });

            buttons.add(button);

            gamePanel.add(button);
        }
    }

    /**
     * Will set all the buttons to default states so that the game can be played again
     */
    public void resetGameBoard()
    {
        gamePanel.removeAll();
        buttons.clear();

        setGamesPanel();
        gamePanel.repaint();
        gamePanel.revalidate();
    }

    /**
     * Adds a String onto the end of the current text in the messages box.
     *  
     * @return - The String to append
     */
    public void addMessage(String message)
    {
        messages.append(message);
    }

    /**
     * The DrawButton class is an extension of JButton that allows for more control of how the button is rendered
     * once it has been clicked.
     */
    class DrawButton extends JButton
    {
        private boolean clicked;
    private int playerTurn;

    public DrawButton()
    {
        clicked = false;
        setBackground(Color.WHITE);
        playerTurn = 2;

        addMouseListener(new MouseAdapter()
        {
                public void mouseEntered(MouseEvent e)
        {
            setBackground(Color.WHITE);
        }
    });
        }

/**
 * Returns what user (if any) clicked the button
 * 
 * @return - 0: naught, 1: cross, 2: not yet clicked
 */
public int getState()
{
    return playerTurn;
}

/**
 * Forces the button into a disabled state and flags it ready for the paint event.
 * 
 * @param playerTurn - Whether it was naughts or crosses that clicked the button
 */
public void clickedOn(int playerTurn)
{
    clicked = true;
    setEnabled(false);
    this.playerTurn = playerTurn;
    this.repaint();
    this.revalidate();
}

/**
 * Uses default draw behavior until the button has been flagged. After this, a naught or
 * cross will be drawn depending on who clicked the button.Sets up the components that will be displayed
 */
public void paintComponent(Graphics g)
{
    if (!clicked)
    {
        super.paintComponent(g);
    }
    else
    {
        super.paintComponent(g);

        int width = getWidth();
        int height = getHeight();

        //Change stroke size
        Graphics2D g2d = (Graphics2D)g;
        g2d.setStroke(new BasicStroke(5));
        g2d.setRenderingHint(RenderingHints.KEY_ANTIALIASING, RenderingHints.VALUE_ANTIALIAS_ON);

        //Determine whether we are drawing a naught or a cross
        if (playerTurn == 0)
            g2d.drawOval((int)(getHorizontalAlignment() + (width * 0.1)), (int)(getVerticalAlignment() + (height * 0.1)), (int)(width / 1.25), (int)(height / 1.25));
        else
        {
            g2d.drawLine((int)(0 + (width * 0.1)), (int)(0 + (height * 0.1)), (int)(width - (width * 0.1)), (int)(height - (height * 0.1)));
            g2d.drawLine((int)(width - (width * 0.1)), (int)(0 + (height * 0.1)), (int)(0 + (width * 0.1)), (int)(height - (height * 0.1)));
        }
    }
}
    }
}

}
