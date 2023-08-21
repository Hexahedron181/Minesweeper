namespace Minesweeper
{
    public partial class Form1 : Form
    {
        System.Windows.Forms.Button[] btnArray = new
            System.Windows.Forms.Button[225];
        Image Flag = Properties.Resources.Flag;
        Image Bomb = Properties.Resources.Bomb;
        List<int> bombs = new List<int>();
        Random rnd = new Random();
        int bombCount = 1;

    // SURROUNDING BOMB CHECK COORDS ==============================================
        
        int[] surroundingLeftEdge = new int[5] { 1, 15, 16, -14, -15 }; //          1
        int[] surroundingRightEdge = new int[5] { -1, -15, -16, 14, 15 }; //        2
        int[] surroundingTop = new int[5] { -1, 1, 14, 15, 16 }; //                 3
        int[] surroundingBottom = new int[5] { -1, 1, -14, -15, -16 }; //           4
        int[] surroundingN = new int[8] { 1, 16, 15, 14, -1, -16, -15, -14 }; //    5


        int[] topLeft = new int[3] { 1, 15, 16 }; //                                6
        int[] topRight = new int[3] { -1, 14, 15 };//                               7
        int[] bottomLeft = new int[3] { 1, -14, -15 }; //                           8
        int[] bottomRight = new int[3] { -1, -15, -16 }; //                         9      

        int[] corners = new int[4] { 0, 14, 210, 224 }; 
        int[] rightEdge = new int[13] { 29, 44, 59, 74, 89, 104, 119, 134, 149, 164, 179, 194, 209 }; 

        // ============================================================================

        int buttonsPressed = 0;
        public Form1()
        {
            InitializeComponent();
        }

        public void Form1_Load(object sender, EventArgs e)
        {
        // BUTTONS ==========================================

            int xpos = 0;
            int ypos = -35;
            for (int n = 0; n < 225; n++)
            {
                btnArray[n] = new System.Windows.Forms.Button();
                btnArray[n].FlatStyle = FlatStyle.Popup;
                btnArray[n].BackColor = System.Drawing.Color.Gray;
                btnArray[n].Tag = n;
                //btnArray[n].Text = n.ToString();
                //btnArray[n].Font = new Font("Microsoft Sans Serif", 5);
                btnArray[n].Width = 35;
                btnArray[n].Height = 35;
                if (n % 15 == 0)
                {
                    xpos = 0; //The ypos of a button moves down when the buttons reach
                    ypos += 35;            // the end of the screen.
                }
                btnArray[n].Left = xpos;
                btnArray[n].Top = ypos;
                this.Controls.Add(btnArray[n]);
                xpos = xpos + btnArray[n].Width;

                btnArray[n].MouseDown += new System.Windows.Forms.MouseEventHandler(ClickButton);
                // ^^^ Makes the buttons clickable ^^^
            }
            // ==============================================

            int bombNum = 0;
            int num = 0;

            while (bombNum < bombCount)
            {
                num = rnd.Next(0, 225);
                if (bombs.Contains(num) == false) //Checks the space doesn't already contain a bomb.
                {
                    bombs.Add(num); //Imports the bombs around the grid in random places.
                    bombNum++;
                }
            }
        }

        int CheckSurroundings(int[] list, int listLength, int index)
        {
            int num = 0;
            for (int i = 0; i < listLength; i++)
            {
                if (bombs.Contains(index + list[i]))
                {
                    num++;
                }
            }
            return num;
        }



        public int checkLocation(int index, int num, bool bombSearch)
        {
            if (corners.Contains(index))
            {
                switch (index)
                {
                    case 0:
                        num = CheckSurroundings(topLeft, topLeft.Length, index);
                        if (bombSearch == false)
                        {
                            return 6;
                        }
                        break;

                    case 14:
                        num = CheckSurroundings(topRight, topRight.Length, index);
                        if (bombSearch == false)
                        {
                            return 7;
                        }
                        break;

                    case 210:
                        num = CheckSurroundings(bottomLeft, bottomLeft.Length, index);
                        if (bombSearch == false)
                        {
                            return 8;
                        }
                        break;

                    case 224:
                        num = CheckSurroundings(bottomRight, bottomRight.Length, index);
                        if (bombSearch == false)
                        {
                            return 9;
                        }
                        break;
                }
            }

            else if (index % 15 == 0 && index != 0 && index != 210) //Left hand side.
            {
                num = CheckSurroundings(surroundingLeftEdge, surroundingLeftEdge.Length, index);
                if (bombSearch == false)
                {
                    return 1;
                }
            }

            else if (rightEdge.Contains(index) && index != 14 && index != 224) //Right hand side.
            {
                num = CheckSurroundings(surroundingRightEdge, surroundingRightEdge.Length, index);
                if (bombSearch == false)
                {
                    return 2;
                }
            }

            else if (index < 14 && index != 0)
            {
                num = CheckSurroundings(surroundingTop, surroundingTop.Length, index);
                if (bombSearch == false)
                {
                    return 3;
                }
            }

            else if (index > 210 && index != 224)
            {
                num = CheckSurroundings(surroundingBottom, surroundingBottom.Length, index);
                if (bombSearch == false)
                {
                    return 4;
                }
            }

            else
            {
                num = CheckSurroundings(surroundingN, surroundingN.Length, index);
                if (bombSearch == false)
                {
                    return 5;
                }
            }
            return num;
        }

        public void Empty_Squares(List<int> TestedSquares, List<int> emptySquares, int i, int[] list)
        {
            for (int k = 0; k < list.Length; k++)
            {
                if (emptySquares.Contains(i + list[k]) == false && TestedSquares.Contains(i + list[k]) == false)
                {
                    emptySquares.Add(i + list[k]);
                }
            }
        }

        public void noBombs(int index, Button btn)
        {
            List<int> emptySquares = new List<int>();
            emptySquares.Add(index);
            int i = 0;
            List<int> TestedSquares = new List<int>();

            do
            {
                
                int listNum = 0;
                int bombNum = checkLocation(emptySquares[i], 0, true);
                if (bombNum == 0)
                {
                    listNum = checkLocation(emptySquares[i], 0, false);
                    switch (listNum)
                    {
                        case 1:
                            Empty_Squares(TestedSquares, emptySquares, emptySquares[i], surroundingLeftEdge);
                            break;
                        case 2:
                            Empty_Squares(TestedSquares, emptySquares, emptySquares[i], surroundingRightEdge);
                            break;
                        case 3:
                            Empty_Squares(TestedSquares, emptySquares, emptySquares[i], surroundingTop);
                            break;
                        case 4:
                            Empty_Squares(TestedSquares, emptySquares, emptySquares[i], surroundingBottom);
                            break;
                        case 5:
                            Empty_Squares(TestedSquares, emptySquares, emptySquares[i], surroundingN);
                            break;
                        case 6:
                            Empty_Squares(TestedSquares, emptySquares, emptySquares[i], topLeft);
                            break;
                        case 7:
                            Empty_Squares(TestedSquares, emptySquares, emptySquares[i], topRight);
                            break;
                        case 8:
                            Empty_Squares(TestedSquares, emptySquares, emptySquares[i], bottomLeft);
                            break;
                        case 9:
                            Empty_Squares(TestedSquares, emptySquares, emptySquares[i], bottomRight);
                            break;
                    }
                }

                TestedSquares.Add(emptySquares[i]);
                emptySquares.Remove(emptySquares[i]);

            } while(emptySquares.Count > 0);

            foreach (int item in TestedSquares)
            {
                leftClick(item, btnArray[item], true);
            }
        }



        public void leftClick(int index, Button btn, bool noBombClick)
        {

            int bombNum = 0;
            if (btn.BackgroundImage != Flag) //The buttons can't be interacted with when it is a flag.
            {

                if (bombs.Contains(index) == false) //If the button clicked isn't a bomb.
                {

                    bombNum = checkLocation(index, bombNum, true);

                    if (bombNum != 0)
                    {
                        btn.Text = bombNum.ToString();
                    }
                    else if (noBombClick == false)
                    {
                        noBombs(index, btn);
                    }
                    
                }


                btn.FlatStyle = FlatStyle.Flat;
                btn.BackColor = System.Drawing.Color.LightYellow;

                if (bombs.Contains(index) == true)
                {
                    btn.BackgroundImage = Bomb;
                    btn.BackgroundImageLayout = ImageLayout.Stretch;
                    if (MessageBox.Show("You Lost...\nWould you like to try again?", "You Hit A Bomb!", MessageBoxButtons.YesNo) == DialogResult.Yes)
                    {
                        Form1 Form1 = new Form1();
                        Form1.Show();
                    }
                    else
                    {
                        this.Close();
                    }

                }
                if (btn.Enabled == true)
                {
                    buttonsPressed++;
                }
                btn.Enabled = false;
            }
            if (buttonsPressed == 225) //If all buttons pressed and the player has survived.
            {
                MessageBox.Show("YOU WIN!!");
                this.Close();
            }
        }
        public void ClickButton(object sender, System.EventArgs e)
        {

            Button btn = (Button)sender;
            Type t = e.GetType();
            int index = Array.IndexOf(btnArray, btn);
            if (t.Equals(typeof(MouseEventArgs)))
            {
                MouseEventArgs mouse = (MouseEventArgs)e;

                if (mouse.Button == MouseButtons.Left)
                {
                    leftClick(index, btn, false);
                }
                else if (mouse.Button == MouseButtons.Right)
                {
                    if (btn.BackgroundImage != null)
                    {

                        btn.BackgroundImage = null;
                        buttonsPressed--;
                    }
                    else
                    {
                        btn.BackgroundImage = Flag;
                        btn.BackgroundImageLayout = ImageLayout.Stretch;
                        buttonsPressed++;
                        if (buttonsPressed == 225) //If all buttons pressed and the player has survived.
                        {
                            MessageBox.Show("YOU WIN!!");
                            this.Close();
                        }
                    }
                }
            }
        }
    }
}