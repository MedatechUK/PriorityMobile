using System;
using System.Drawing;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;

namespace J2i.Net.Telnet
{
    /// <summary>
    /// Emulates a terminal with a given number of rows and columns. 
    /// This is a partial implementation of an interpreter for ECMA48
    /// </summary>
    class TerminalEmulator
    {
        List<char[]> _content = new List<char[]>();
        Rectangle TerminalSize = new Rectangle(0,0,80,25);
        int _cursorRow = 0;
        int _cursorColumn = 0;
        int _width = 80;
        int _height = 25;
        bool _enableEscapeSquence = true;
        StringBuilder num1 = new StringBuilder();
        StringBuilder num2 = new StringBuilder();

        /// <summary>
        /// To interpret the escape sequences I model a finite
        /// state machine.  This machine only has four states. 
        /// </summary>
        enum EscapedState
        {
            /// <summary>
            /// State after the first escape character (\x1b) is encountered
            /// </summary>
            EscapeBegun,
            /// <summary>
            /// State after the second escape character is encountered.  
            /// Will read digits until a non digit character is encountered. 
            /// IF that on-digit is ';' then the machine will read the second
            /// number.  Other wise it will execute the escape command
            /// </summary>
            AwaitingFirstNumber,
            /// <summary>
            /// After the first number for a command has been read and the delimiter
            /// encountered an attempt is made to read the second number.  upon encountering
            /// the first non-numeric character an attampt is made to execute the command.
            /// </summary>
            AwaitingSecondNumber,
            /// <summary>
            /// The initial state and also the state
            /// returned to when an escape sequnce has been 
            /// aborted or completed
            /// </summary>
            EscapeCompleted
        };

        EscapedState _currentEscapedState = EscapedState.EscapeCompleted;

        public char[] this[int index]
        {
            get
            {
                if (index < TerminalSize.Height)
                {
                    if (index +1 > _content.Count)    
                    {
                        for (int i = _content.Count; i <= index; ++i)
                        {
                            _content.Add(new char[TerminalSize.Width]);
                            ClearLine(i);
                        }
                    }
                    return _content[index];
                }
                else
                throw new ArgumentOutOfRangeException("index", "The index passed to the terminal buffer must be lower than the number of rows in the buffer");
            }
        }


        /// <summary>
        /// Creates a terminal instance 80 characters wide and 25 characters tall
        /// </summary>
        public TerminalEmulator()
        {

        }

        /// <summary>
        /// Creates a terminal emulator isntance with the 
        /// requested width and height
        /// </summary>
        /// <param name="width">character width of terminal emulator</param>
        /// <param name="height">character height of terminal emulator</param>
        public TerminalEmulator(int width, int height)
        {
            _width = width;
            _height = height;
        }
        /// <summary>
        /// Gets or sets the row in which the cursor is placed
        /// </summary>
        public int CursorRow
        {
            get
            {
                return this._cursorRow;
            }
            set
            {
                while (value >= TerminalSize.Height)
                {
                    --value;
                    LineFeed();
                }
                this._cursorRow = value;
            }
        }

        /// <summary>
        /// Gets or sets the column in which the cursor is placed
        /// </summary>
        public int CursorColumn
        {
            get
            {
                return this._cursorColumn;
            }
            set
            {
                int rowAdvance = value / TerminalSize.Width;
                int colAdvance = value % TerminalSize.Width;
                CursorRow = CursorRow + rowAdvance;
                _cursorColumn = colAdvance;

            }
        }
        /// <summary>
        /// Moves the cursor to the next line. Creates a new row in 
        /// the character buffer if needed
        /// </summary>
        void LineFeed()
        {
            if (this._content.Count < TerminalSize.Height)
            {
                this._content.Add(new char[TerminalSize.Width]);

            }
            else
            {
                char[] c = this[0];
                this._content.RemoveAt(0);
                for (int i = 0; i < c.Length; ++i)
                    c[i] = '\0';
                this._content.Add(c);
            }
        }

        /// <summary>
        /// Moves the cursor to the begining of the line
        /// </summary>
        void CarriageReturn()
        {
            this.CursorColumn = 0;
        }

        /// <summary>
        /// Moves the cursor to the upper left corner of the terminal
        /// </summary>
        void Home()
        {
            CursorRow = CursorColumn = 0;
        }

        /// <summary>
        /// Clears the contents of the terminal emulator
        /// and resets the position of the cursor to the upper left corner
        /// </summary>
        public void Clear()
        {
            Home();
            for (int i = 0; i<_content.Count; ++i)
            {
                ClearLine(i);
            }

        }

        /// <summary>
        /// Empeties the contents of a line and
        /// replaces them with spaces
        /// </summary>
        /// <param name="row">the row to be cleared</param>
        void ClearLine(int row)
        {
            char[] rowChar = this[row];
            for (int i = 0; i < rowChar.Length; ++i)
            {
                rowChar[i] = '\x20';
            }
        }

        /// <summary>
        /// writes a string to the terminal emulator and possibly interprets
        /// escape commands
        /// </summary>
        /// <param name="msg">string to be written</param>
        public void Write(String msg)
        {

            for (int i = 0; i < msg.Length; ++i)
                Write(Convert.ToChar(msg[i]));
        }

        /// <summary>
        /// Writes a character to the terminal emulator possible 
        /// interpreting escape commands
        /// </summary>
        /// <param name="c">Character to be written</param>
        public void Write(char c)
        {
            if (_enableEscapeSquence)
            {
                switch (_currentEscapedState)
                {
                    case EscapedState.AwaitingFirstNumber:
                        if (Char.IsDigit(c))
                            num1.Append(c);
                        else if (c == ';')
                        {
                            _currentEscapedState = EscapedState.AwaitingSecondNumber;
                        }
                        else
                        {
                            ExecuteEscape(c);
                            _currentEscapedState = EscapedState.EscapeCompleted;
                        }
                        break;
                    case EscapedState.AwaitingSecondNumber:
                        if (Char.IsDigit(c))
                            num2.Append(c);
                        else
                        {
                            ExecuteEscape(c);
                            _currentEscapedState = EscapedState.EscapeCompleted;
                        }
                        break;
                    case EscapedState.EscapeBegun:
                        if(c=='[')
                            _currentEscapedState=EscapedState.AwaitingFirstNumber;
                        else
                            _currentEscapedState=EscapedState.EscapeCompleted;
                        break;
                    case EscapedState.EscapeCompleted:
                        if (c == '\x1b')
                        {
                            this._currentEscapedState = EscapedState.EscapeBegun;
                            num1.Length = 0;
                            num2.Length = 0;
                        }
                        else
                            PutChar(c);
                        break;
                    default:
                        PutChar(c);
                        break;
                }

            }
            else
                PutChar(c);
        }

        /// <summary>
        /// Executes an escape sequence
        /// </summary>
        /// <param name="c">character identifying the escape command to execute</param>
        private void ExecuteEscape(char c)
        {
            int cursorDistance =1;
            int row = 1;
            int col = 1;
            int clearType = 0;

            if(num1.Length>0)
                try {clearType =  cursorDistance = row = int.Parse(num1.ToString());}catch{}
            if(num2.Length>0)
                try { col = int.Parse(num2.ToString()); } catch { }

            switch (c)
            {
                case 'H':
                    CursorRow = row-1;
                    CursorColumn = col-1;
                    break;
                case 'A':
                    CursorRow = CursorRow - cursorDistance;
                    break;
                case 'B':
                    CursorRow = CursorRow + cursorDistance;
                    break;
                case 'C':
                    CursorColumn=CursorColumn+cursorDistance;
                    break;
                case 'D':
                    CursorColumn=CursorColumn-cursorDistance;
                    break;
                case 'J':
                    Clear();
                    break;
            }
        }

        /// <summary>
        /// Places a character in the terminal emulator's buffer
        /// </summary>
        /// <param name="c">Character to be written</param>
        private void PutChar(char c)
        {
            if (c == '\r')
                CursorColumn = 0;
            else if (c == '\n')
                CursorRow = CursorRow + 1;
            else
            {
                this[CursorRow][CursorColumn] = c;
                CursorColumn = CursorColumn + 1;
            }
        }
        /// <summary>
        /// returns the contents of the terminal emulator's buffer
        /// as a string
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < this._content.Count; ++i)
            {
                sb.Append(this[i]);
                sb.Append("\r\n");
            }
            sb.Replace('\0', '\x20');
            string output = sb.ToString();
            //Debug.WriteLine(output,"ToString()");
            return output;
        }
    }
}
