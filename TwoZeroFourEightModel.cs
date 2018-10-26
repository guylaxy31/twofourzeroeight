using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace twozerofoureight
{
    class TwoZeroFourEightModel : Model
    {
        protected int boardSize; // default is 4
        protected int[,] board;
        protected Random rand;
        protected int[] range;

        public TwoZeroFourEightModel() : this(4)
        {
            // default board size is 4 
        }

        public TwoZeroFourEightModel(int size)
        {
            boardSize = size;
            board = new int[boardSize, boardSize];
            range = Enumerable.Range(0, boardSize).ToArray();
            foreach (int i in range)
            {
                foreach (int j in range)
                {
                    board[i, j] = 0;
                }
            }
            rand = new Random();
            // initialize board
            HandleChanges();
        }

        public int[,] GetBoard()
        {
            return board;
        }

        public bool CheckBlockFull()            // check each num n board is full? if full return true!!
        {
            int count = 0;
            foreach (int num in board)          // for loop
            {
                if (num > 0)
                {
                    count++;
                }
            }
            if (count == 16) return true;
            else return false;
        }

        public bool GameOver() 
        {
            int count = 0;                                                    //      y0  y1  y2  y3
                for (int x = 0; x < 3; x++)             // limit x = 0 1 2        x0 
                {                                                             //  x1
                    for (int y = 0; y < 4; y++)                               //  x2
                    {                                                         //  x3                    <--- x3 will not check because already check from x2 loops
                        if (y != 3) 
                        {
                            if (board[x, y] != board[x, y + 1] && board[x, y] != board[x + 1, y])       //if current isn't right of board check right and bottom
                            {
                                count++;
                            }
                        }
                        else
                        {
                            if (board[x, y] != board[x + 1, y])         // if current is right of board check bottom only
                            {
                                count++;
                            }
                        }
                    }
                }
                if (count >= 12)                                        //if all num in board was count  check full or !full
                {                                                       // if true function gameover  will return true and display show
                    if (CheckBlockFull())
                    {
                        return true;
                    }
                    return false;
                }
                else
                {
                    return false;
                }
        }


        public int GetScore()
        {
            int sc=0;                   // everytime when use this method clear score before calculate
            foreach(int numinboard in board)
            {
                sc += numinboard;
            }
            return sc; 
        }

        private void AddRandomSlot()
        {
            while (true)
            {
                int x = rand.Next(boardSize);
                int y = rand.Next(boardSize);
                if (board[x, y] == 0)
                {
                    board[x, y] = 2;
                    return;
                }
            }
        }

        // Perform shift and merge to the left of the given array.
        protected bool ShiftAndMerge(int[] buffer)
        {
            bool changed = false; // whether the array has changed
            int pos = 0; // next available slot index
            int lastMergedSlot = -1; // last slot that resulted from merging
            foreach (int k in range)
            {
                if (buffer[k] != 0) // nonempty slot
                {
                    // check if we can merge with the previous slot
                    if (pos > 0 && pos - 1 > lastMergedSlot && buffer[pos - 1] == buffer[k])
                    {
                        // merge
                        buffer[pos - 1] *= 2;
                        buffer[k] = 0;
                        lastMergedSlot = pos - 1;
                        changed = true;
                    }
                    else
                    {
                        // shift to the next available slot
                        buffer[pos] = buffer[k];
                        if (pos != k)
                        {
                            buffer[k] = 0;
                            changed = true;
                        }
                        // move the next available slot
                        pos++;
                    }
                }
            }
            return changed;
        }

        protected void HandleChanges(bool changed = true)
        {
            // if the board has changed, add a new number
            // and notify all views
            if (changed)
            {
                AddRandomSlot();
                NotifyAll();
            }
        }

        public void PerformDown()
        {
            bool changed = false; // whether the board has changed
            foreach (int i in range)
            {
                int[] buffer = new int[boardSize];
                // extract the current column from bottom to top
                foreach (int j in range)
                {
                    buffer[boardSize - j - 1] = board[j, i];
                }
                // process the extracted array
                // also track changes
                changed = ShiftAndMerge(buffer) || changed;
                // copy back
                foreach (int j in range)
                {
                    board[j, i] = buffer[boardSize - j - 1];
                }
            }
            HandleChanges(changed);
        }

        public void PerformUp()
        {
            bool changed = false; // whether the board has changed
            foreach (int i in range)
            {
                int[] buffer = new int[boardSize];
                // extract the current column from top to bottom
                foreach (int j in range)
                {
                    buffer[j] = board[j, i];
                }
                // process the extracted array
                // also track changes
                changed = ShiftAndMerge(buffer) || changed;
                // copy back
                foreach (int j in range)
                {
                    board[j, i] = buffer[j];
                }
            }
            HandleChanges(changed);
        }

        public void PerformRight()
        {
            bool changed = false; // whether the board has changed
            foreach (int i in range)
            {
                int[] buffer = new int[boardSize];
                // extract the current column from right to left
                foreach (int j in range)
                {
                    buffer[boardSize - j - 1] = board[i, j];
                }
                // process the extracted array
                // also track changes
                changed = ShiftAndMerge(buffer) || changed;
                // copy back
                foreach (int j in range)
                {
                    board[i, j] = buffer[boardSize - j - 1];
                }
            }
            HandleChanges(changed);
        }

        public void PerformLeft()
        {
            bool changed = false; // whether the board has changed
            foreach (int i in range)
            {
                int[] buffer = new int[boardSize];
                // extract the current column from left to right
                foreach (int j in range)
                {
                    buffer[j] = board[i, j];
                }
                // process the extracted array
                // also track changes
                changed = ShiftAndMerge(buffer) || changed;
                // copy back
                foreach (int j in range)
                {
                    board[i, j] = buffer[j];
                }
            }
            HandleChanges(changed);
        }
    }
}
