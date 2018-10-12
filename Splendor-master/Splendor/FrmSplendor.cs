﻿/**
 * \file      frmAddVideoGames.cs
 * \author    F. Andolfatto
 * \version   1.0
 * \date      August 22. 2018
 * \brief     Form to play.
 *
 * \details   This form enables to choose coins or cards to get ressources (precious stones) and prestige points 
 * to add and to play with other players
 */


using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Splendor
{
    /// <summary>
    /// manages the form that enables to play with the Splendor
    /// </summary>
    public partial class frmSplendor : Form
    {
        //used to store the number of coins selected for the current round of game
        private int nbRubis;
        private int nbOnyx;
        private int nbEmeraude;
        private int nbDiamand;
        private int nbSaphir;
        public int nbPlayer = 1;
        public int id = 0;
        private string CardSelected;


        IList<Player> playerList = new List<Player>()
        {
            new Player(){ Name = "Joueur 1", Id = 1, Ressources = new int[]{ 0, 0, 0, 0, 0 }, Coins= new int[]{ 0, 0, 0, 0, 0 }},
            new Player(){ Name = "Joueur 2", Id = 2, Ressources = new int[]{ 0, 0, 0, 0, 0 }, Coins= new int[]{ 0, 0, 0, 0, 0 }},
            new Player(){ Name = "Joueur 3", Id = 3, Ressources = new int[]{ 0, 0, 0, 0, 0 }, Coins= new int[]{ 0, 0, 0, 0, 0 }},
            new Player(){ Name = "Joueur 4", Id = 4, Ressources = new int[]{ 0, 0, 0, 0, 0 }, Coins= new int[]{ 0, 0, 0, 0, 0 }}
        };


        //id of the player that is playing
        private int currentPlayerId;
        //boolean to enable us to know if the user can click on a coin or a card
        private bool enableClicLabel;
        //connection to the database
        private static ConnectionDB conn = new ConnectionDB();
        //load cards from the database
        Stack<Card> listCardOne = conn.GetListCardAccordingToLevel(1);
        Stack<Card> listCardTwo = conn.GetListCardAccordingToLevel(2);
        Stack<Card> listCardTree = conn.GetListCardAccordingToLevel(3);
        Stack<Card> listCardFor = conn.GetListCardAccordingToLevel(4);

        /// <summary>
        /// constructor
        /// </summary>
        public frmSplendor()
        {
            InitializeComponent();
        }

        /// <summary>
        /// loads the form and initialize data in it
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void frmSplendor_Load(object sender, EventArgs e)
        {
            lblGoldCoin.Text = "5";

            lblDiamandCoin.Text = "7";
            lblEmeraudeCoin.Text = "7" ;
            lblOnyxCoin.Text = "7";
            lblRubisCoin.Text = "7";
            lblSaphirCoin.Text = "7";



            //Afficher les cartes de level 1
            int nbDataInStack1 = listCardOne.Count;
            int i1 = 0;
            foreach (Control ctrl in flwCardLevel1.Controls)
            {
                //check when you are at the end of the stack
                if (i1 < nbDataInStack1)
                {
                    ctrl.Text = listCardOne.Pop().ToString();
                    i1++;
                }

            }

            //afficher les cartes de level 2
            int nbDataInStack2 = listCardTwo.Count;
            int i2 = 0;
            foreach (Control ctrl in flwCardLevel2.Controls)
            {
                //check when you are at the end of the stack
                if (i2 < nbDataInStack2)
                {
                    ctrl.Text = listCardTwo.Pop().ToString();
                    i2++;
                }

            }

            //afficher les cartes de level 3
            int nbDataInStack3 = listCardTree.Count;
            int i3 = 0;
            foreach (Control ctrl in flwCardLevel3.Controls)
            {
                //check when you are at the end of the stack
                if (i3 < nbDataInStack3)
                {
                    ctrl.Text = listCardTree.Pop().ToString();
                    i3++;
                }

            }

            //afficher les cartes de level 4
            int nbDataInStack4 = listCardFor.Count;
            int i4 = 0;
            foreach (Control ctrl in flwCardNoble.Controls)
            {
                //check when you are at the end of the stack
                if (i4 < nbDataInStack4)
                {
                    ctrl.Text = listCardFor.Pop().ToString();
                    i4++;
                }

            }


            this.Width = 680;
            this.Height = 540;

            enableClicLabel = false;

            lblChoiceDiamand.Visible = false;
            lblChoiceOnyx.Visible = false;
            lblChoiceRubis.Visible = false;
            lblChoiceSaphir.Visible = false;
            lblChoiceEmeraude.Visible = false;
            cmdValidateChoice.Visible = false;
            cmdNextPlayer.Visible = false;

            //we wire the click on all cards to the same event
            //display the iformation of the card for the level 1
            txtLevel11.Click += ClickOnCard;
            txtLevel12.Click += ClickOnCard;
            txtLevel13.Click += ClickOnCard;
            txtLevel14.Click += ClickOnCard;

            //display the iformation of the card for the level 2
            txtLevel21.Click += ClickOnCard;
            txtLevel22.Click += ClickOnCard;
            txtLevel23.Click += ClickOnCard;
            txtLevel24.Click += ClickOnCard;

            //display the iformation of the card for the level 3
            txtLevel31.Click += ClickOnCard;
            txtLevel32.Click += ClickOnCard;
            txtLevel33.Click += ClickOnCard;
            txtLevel34.Click += ClickOnCard;

            //display the iformation of the card for the level 4
            txtNoble1.Click += ClickOnCard;
            txtNoble2.Click += ClickOnCard;
            txtNoble3.Click += ClickOnCard;
            txtNoble4.Click += ClickOnCard;
        }

        private void ClickOnCard(object sender, EventArgs e)
        {
            //We get the value on the card and we split it to get all the values we need (number of prestige points and ressource)
            //Enable the button "Validate"


            TextBox txtBox = sender as TextBox;
            //get the text displayed in the textbox that has been clicked
            MessageBox.Show(txtBox.Text);
            CardSelected = txtBox.Text;
            txtPlayerBookedCard.Text = txtBox.Text;
            cmdValidateChoice.Visible = true;
        }

        /// <summary>
        /// click on the play button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmdPlay_Click(object sender, EventArgs e)
        {
            this.Width = 680;
            this.Height = 780;

            int id = 0;
           
            LoadPlayer(idPlayer);

        }


        /// <summary>
        /// load data about the current player
        /// </summary>
        /// <param name="id">identifier of the player</param>
        private void LoadPlayer(int id) { 

            enableClicLabel = true;

            string name = playerList[id].Name;
            CardSelected = null;
            txtPlayerBookedCard.Text = null;
            //no coins or card selected yet, labels are empty
            lblChoiceDiamand.Text = "";
            lblChoiceOnyx.Text = "";
            lblChoiceRubis.Text = "";
            lblChoiceSaphir.Text = "";
            lblChoiceEmeraude.Text = "";

            lblChoiceCard.Text = "";

            Player player = new Player();
            playerList[id].Name = name;
            playerList[id].Id = id;
            playerList[id].Ressources = new int[] { 2, 0, 1, 1, 1 };
            playerList[id].Coins = new int[] { nbRubis,nbSaphir,nbOnyx,nbEmeraude,nbDiamand };

            lblPlayerRubisCoin.Text = playerList[id].Coins[0].ToString();
            lblPlayerSaphirCoin.Text = playerList[id].Coins[1].ToString();
            lblPlayerOnyxCoin.Text = playerList[id].Coins[2].ToString();
            lblPlayerEmeraudeCoin.Text = playerList[id].Coins[3].ToString();
            lblPlayerDiamandCoin.Text = playerList[id].Coins[4].ToString();
            currentPlayerId = id;

            lblPlayer.Text = "Jeu de " + name;

            cmdPlay.Enabled = false;
        }

        /// <summary>
        /// click on the red coin (rubis) to tell the player has selected this coin
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void lblRubisCoin_Click(object sender, EventArgs e)
        {
            if (enableClicLabel)
            {
                CardSelected = null;
                txtPlayerBookedCard.Text = null;
                cmdValidateChoice.Visible = true;
                lblChoiceRubis.Visible = true;
                int nbtotal = nbRubis + nbSaphir + nbOnyx + nbEmeraude + nbDiamand;
                if (nbtotal >= 3)
                {
                    MessageBox.Show("Vous ne pouvez pas prendre plus de jetons");
                }
                else
                {
                    if (nbRubis == 2 || nbSaphir == 2 || nbOnyx == 2 || nbEmeraude == 2 || nbDiamand == 2)
                    {
                        MessageBox.Show("Vous pouvez prendre maximum 2 pièces de la même couleur");
                    }
                    else
                    {
                        if ((nbRubis == 1 && nbSaphir == 1) || (nbRubis == 1 && nbOnyx == 1) || (nbRubis == 1 && nbEmeraude == 1) || (nbRubis == 1 && nbDiamand == 1))
                        {
                            MessageBox.Show("Choisissez une autre couleur");
                        }
                        else
                        {
                            if (lblRubisCoin.Text == "2")
                            {
                                MessageBox.Show("Vous ne pouvez pas prendre de Rubis");
                            }
                            else
                            {
                                nbRubis++;
                                int var = Convert.ToInt32(lblRubisCoin.Text) - 1;
                                lblRubisCoin.Text = var.ToString();
                                lblChoiceRubis.Text = nbRubis + "\r\n";
                                playerList[id].Coins[0] = nbRubis;

                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// click on the blue coin (saphir) to tell the player has selected this coin
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void lblSaphirCoin_Click(object sender, EventArgs e)
        {
            if (enableClicLabel)
            {
                cmdValidateChoice.Visible = true;
                lblChoiceSaphir.Visible = true;
                int nbtotal = nbRubis + nbSaphir + nbOnyx + nbEmeraude + nbDiamand;
                if (nbtotal >= 3)
                {
                    MessageBox.Show("Vous ne pouvez pas prendre plus de jetons");
                }
                else
                {
                    if (nbRubis == 2 || nbSaphir == 2 || nbOnyx == 2 || nbEmeraude == 2 || nbDiamand == 2)
                    {
                        MessageBox.Show("Vous pouvez prendre maximum 2 pièces de la même couleur");
                    }
                    else
                    {
                        if ((nbRubis == 1 && nbSaphir == 1) || (nbSaphir == 1 && nbOnyx == 1) || (nbSaphir == 1 && nbEmeraude == 1) || (nbSaphir == 1 && nbDiamand == 1))
                        {
                            MessageBox.Show("Choisissez une autre couleur");
                        }
                        else
                        {
                            if (lblSaphirCoin.Text == "2")
                            {
                                MessageBox.Show("Vous ne pouvez pas prendre plus de cette couleur");
                            }
                            else
                            {
                                nbSaphir++;
                                int var = Convert.ToInt32(lblSaphirCoin.Text) - 1;
                                lblSaphirCoin.Text = var.ToString();
                                lblChoiceSaphir.Text = nbSaphir + "\r\n";
                                playerList[id].Coins[1] = nbSaphir;
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// click on the black coin (onyx) to tell the player has selected this coin
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void lblOnyxCoin_Click(object sender, EventArgs e)
        {
            if (enableClicLabel)
            {
                cmdValidateChoice.Visible = true;
                lblChoiceOnyx.Visible = true;
                int nbtotal = nbRubis + nbSaphir + nbOnyx + nbEmeraude + nbDiamand;
                if (nbtotal >= 3)
                {
                    MessageBox.Show("Vous ne pouvez pas prendre plus de jetons");
                }
                else
                {
                    if (lblOnyxCoin.Text == "2")
                    {
                        MessageBox.Show("Vous ne pouvez pas prendre plus de cette couleur");
                    }
                    else { 
                        if (nbRubis == 2 || nbSaphir == 2 || nbOnyx == 2 || nbEmeraude == 2 || nbDiamand == 2)
                        {
                            MessageBox.Show("Vous pouvez prendre maximum 2 pièces de la même couleur");
                        }
                        else
                        {
                            if ((nbRubis == 1 && nbOnyx == 1) || (nbSaphir == 1 && nbOnyx == 1) || (nbOnyx == 1 && nbEmeraude == 1) || (nbOnyx == 1 && nbDiamand == 1))
                            {
                                MessageBox.Show("Choisissez une autre couleur");
                            }                            
                            else
                            {
                                nbOnyx++;
                                int var = Convert.ToInt32(lblOnyxCoin.Text) - 1;
                                lblOnyxCoin.Text = var.ToString();
                                lblChoiceOnyx.Text = nbOnyx + "\r\n";
                                playerList[id].Coins[2] = nbOnyx;
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// click on the green coin (emeraude) to tell the player has selected this coin
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void lblEmeraudeCoin_Click(object sender, EventArgs e)
        {
            if (enableClicLabel)
            {
                cmdValidateChoice.Visible = true;
                lblChoiceEmeraude.Visible = true;
                int nbtotal = nbRubis + nbSaphir + nbOnyx + nbEmeraude + nbDiamand;
                if (nbtotal >= 3)
                {
                    MessageBox.Show("Vous ne pouvez pas prendre plus de jetons");
                }
                else
                {
                    if (nbRubis == 2 || nbSaphir == 2 || nbOnyx == 2 || nbEmeraude == 2 || nbDiamand == 2)
                    {
                        MessageBox.Show("Vous pouvez prendre maximum 2 pièces de la même couleur");
                    }
                    else
                    {
                        if ((nbRubis == 1 && nbEmeraude == 1) || (nbEmeraude == 1 && nbOnyx == 1) || (nbSaphir == 1 && nbEmeraude == 1) || (nbEmeraude == 1 && nbDiamand == 1))
                        {
                            MessageBox.Show("Choisissez une autre couleur");
                        }
                        else
                        {
                            if (lblEmeraudeCoin.Text == "2")
                            {
                                MessageBox.Show("Vous ne pouvez pas prendre de Rubis");
                            }
                            else
                            {
                                nbEmeraude++;
                                int var = Convert.ToInt32(lblEmeraudeCoin.Text) - 1;
                                lblEmeraudeCoin.Text = var.ToString();
                                lblChoiceEmeraude.Text = nbEmeraude + "\r\n";
                                playerList[id].Coins[3] = nbEmeraude;
                            }
                        }
                    }
                }
            }

        }

        /// <summary>
        /// click on the white coin (diamand) to tell the player has selected this coin
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void lblDiamandCoin_Click(object sender, EventArgs e)
        {
            if (enableClicLabel)
            {
                cmdValidateChoice.Visible = true;
                lblChoiceDiamand.Visible = true;
                int nbtotal = nbRubis + nbSaphir + nbOnyx + nbEmeraude + nbDiamand;
                if (nbtotal >= 3)
                {
                    MessageBox.Show("Vous ne pouvez pas prendre plus de jetons");
                }
                else
                {
                    if (nbRubis == 2 || nbSaphir == 2 || nbOnyx == 2 || nbEmeraude == 2 || nbDiamand == 2)
                    {
                        MessageBox.Show("Vous pouvez prendre maximum 2 pièces de la même couleur");
                    }
                    else
                    {
                        if ((nbRubis == 1 && nbDiamand == 1) || (nbDiamand == 1 && nbOnyx == 1) || (nbDiamand == 1 && nbEmeraude == 1) || (nbSaphir == 1 && nbDiamand == 1))
                        {
                            MessageBox.Show("Choisissez une autre couleur");
                        }
                        else
                        {
                            if (lblEmeraudeCoin.Text == "2")
                            {
                                MessageBox.Show("Vous ne pouvez pas prendre de Rubis");
                            }
                            else
                            {
                                nbDiamand++;
                                int var = Convert.ToInt32(lblDiamandCoin.Text) - 1;
                                lblDiamandCoin.Text = var.ToString();
                                lblChoiceDiamand.Text = nbDiamand + "\r\n";
                                playerList[id].Coins[4] = nbDiamand;
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// click on the validate button to approve the selection of coins or card
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmdValidateChoice_Click(object sender, EventArgs e)
        {
            cmdNextPlayer.Visible = true;
            //TO DO Check if card or coins are selected, impossible to do both at the same time
            if ((nbDiamand != 0)||(nbOnyx != 0)||(nbRubis != 0)||(nbSaphir != 0)||(nbEmeraude != 0)||(CardSelected != null))
            {
                cmdNextPlayer.Enabled = true;
            }
            
            cmdNextPlayer.Enabled = true;
            Player player = new Player();
            LoadPlayer(player.Id);
        }

        /// <summary>
        /// click on the insert button to insert player in the game
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmdInsertPlayer_Click(object sender, EventArgs e)
        {
            if (nbPlayer >= 4)
            {
                cmdInsertPlayer.Enabled = false;
            }
            else if(nbPlayer<4)
            {
                /// il faudrait faire un sous programe qui renvoie le nom du joueur !
                nbPlayer++;
                cmdDeletePlayer.Enabled = true;

                
                lblNbPlayer.Text = nbPlayer.ToString();
            }
        }
   
        /// <summary>
        /// click on the next player to tell him it is his turn
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmdNextPlayer_Click(object sender, EventArgs e)
        {
            //TO DO in release 1.0 : 3 is hard coded (number of players for the game), it shouldn't. 
            //TO DO Get the id of the player : in release 0.1 there are only 3 players
            //Reload the data of the player

            //a enlever
            int nbPlayer = 3;
            //We are not allowed to click on the next button
            idPlayer++;
            if (idPlayer < nbPlayer)
            {
                idPlayer = 0;
            }
            if (CardSelected != null)
            {
                int nbCardStack1 = listCardOne.Count;
                int if1 = 0;
                foreach (Control allCard in flwCardLevel1.Controls)
                {
                    if (if1 < nbCardStack1)
                    {
                        if (CardSelected == allCard.Text)
                        {
                            allCard.Text = listCardOne.Pop().ToString();
                        }
                        if1++;
                    } 
                }
                int nbCardStack2 = listCardTwo.Count;
                int if2 = 0;
                foreach (Control allCard in flwCardLevel2.Controls)
                {
                    if (if2 < nbCardStack2)
                    {
                        if (CardSelected == allCard.Text)
                        {
                            allCard.Text = listCardTwo.Pop().ToString();
                        }
                        if2++;
                    }
                }
                int nbCardStack3 = listCardTree.Count;
                int if3 = 0;
                foreach (Control allCard in flwCardLevel3.Controls)
                {
                    if (if3 < nbCardStack3)
                    {
                        if (CardSelected == allCard.Text)
                        {
                            allCard.Text = listCardTree.Pop().ToString();
                        }
                        if3++;
                    }
                }
                int nbCardStack4 = listCardFor.Count;
                int if4 = 0;
                foreach (Control allCard in flwCardNoble.Controls)
                {
                    if (if4 < nbCardStack4)
                    {
                        if (CardSelected == allCard.Text)
                        {
                            allCard.Text = listCardFor.Pop().ToString();
                        }
                        if4++;
                    }
                }
            }
            

            cmdNextPlayer.Visible = false;
            
            LoadPlayer(idPlayer);

        }

        private void cmdDeletePlayer_Click(object sender, EventArgs e)
        {
            if (nbPlayer <= 1)
            {
                cmdDeletePlayer.Enabled = false;

            }
            else if (nbPlayer >1)
            {
                nbPlayer--;
                cmdInsertPlayer.Enabled = true;
                
                lblNbPlayer.Text = nbPlayer.ToString();
            }
        }

        private void lblChoiceRubis_Click(object sender, EventArgs e)
        {
            if (nbRubis==0)
            {
                lblChoiceRubis.Visible = false;
                if (nbSaphir == 0 && nbEmeraude == 0 && nbOnyx == 0 && nbDiamand == 0)
                {
                    cmdValidateChoice.Visible = false;
                }
            }
            else
            {
                nbRubis--;
                int var = Convert.ToInt32(lblRubisCoin.Text) + 1;
                lblRubisCoin.Text = var.ToString();
                lblChoiceRubis.Text = nbRubis + "\r\n";
            }
        }

        private void lblChoiceSaphir_Click(object sender, EventArgs e)
        {
            if (nbSaphir == 0)
            {
                lblChoiceSaphir.Visible = false;
                if (nbRubis == 0 && nbEmeraude == 0 && nbOnyx == 0 && nbDiamand == 0)
                {
                    cmdValidateChoice.Visible = false;
                }
            }
            else
            {
                nbSaphir--;
                int var = Convert.ToInt32(lblSaphirCoin.Text) + 1;
                lblSaphirCoin.Text = var.ToString();
                lblChoiceSaphir.Text = nbSaphir + "\r\n";
            }
        }

        private void lblChoiceOnyx_Click(object sender, EventArgs e)
        {
            if (nbOnyx == 0)
            {
                lblChoiceOnyx.Visible = false;
                if (nbRubis == 0 && nbEmeraude == 0 && nbSaphir == 0 && nbDiamand == 0)
                {
                    cmdValidateChoice.Visible = false;
                }
            }
            else
            {
                nbOnyx--;
                int var = Convert.ToInt32(lblOnyxCoin.Text) + 1;
                lblOnyxCoin.Text = var.ToString();
                lblChoiceOnyx.Text = nbOnyx + "\r\n";
            }
        }

        private void lblChoiceEmeraude_Click(object sender, EventArgs e)
        {
            if (nbEmeraude == 0)
            {
                lblChoiceEmeraude.Visible = false;
                if (nbRubis == 0 && nbOnyx == 0 && nbSaphir == 0 && nbDiamand == 0)
                {
                    cmdValidateChoice.Visible = false;
                }
            }
            else
            {
                nbEmeraude--;
                int var = Convert.ToInt32(lblEmeraudeCoin.Text) + 1;
                lblEmeraudeCoin.Text = var.ToString();
                lblChoiceEmeraude.Text = nbEmeraude + "\r\n";
            }
        }

        private void lblChoiceDiamand_Click(object sender, EventArgs e)
        {
            if (nbDiamand == 0)
            {
                lblChoiceDiamand.Visible = false;
                if (nbRubis == 0 && nbOnyx == 0 && nbSaphir == 0 && nbEmeraude == 0)
                {
                    cmdValidateChoice.Visible = false;
                }
            }
            else
            {
                nbDiamand--;
                int var = Convert.ToInt32(lblDiamandCoin.Text) + 1;
                lblDiamandCoin.Text = var.ToString();
                lblChoiceDiamand.Text = nbDiamand + "\r\n";
            }
        }
    }
}
