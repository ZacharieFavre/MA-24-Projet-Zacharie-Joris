/**
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
        public int nbPlayer = 2;

        private int nbRubisNeeded;
        private int nbSaphirNeeded;
        private int nbOnyxNeeded;
        private int nbEmeraudeNeeded;
        private int nbDiamandNeeded;

        private string tempBookedCard;
        private int nbRubisPres;
        private int nbSaphirPres;
        private int nbOnyxPres;
        private int nbEmeraudePres;
        private int nbDiamandPres;
        private int totPresPt;

        private string CardSelected;


        IList<Player> playerList = new List<Player>()
        {
            new Player(){ Name = "Joueur 1", Id = 1, Ressources = new int[]{ 0, 0, 0, 0, 0 }, Coins= new int[]{ 0, 0, 0, 0, 0 }, NbPrestige=0},
            new Player(){ Name = "Joueur 2", Id = 2, Ressources = new int[]{ 0, 0, 0, 0, 0 }, Coins= new int[]{ 0, 0, 0, 0, 0 }, NbPrestige=0},
            new Player(){ Name = "Joueur 3", Id = 3, Ressources = new int[]{ 0, 0, 0, 0, 0 }, Coins= new int[]{ 0, 0, 0, 0, 0 }, NbPrestige=0},
            new Player(){ Name = "Joueur 4", Id = 4, Ressources = new int[]{ 0, 0, 0, 0, 0 }, Coins= new int[]{ 0, 0, 0, 0, 0 }, NbPrestige=0}
        };


        //id of the player that is playing
        private int currentPlayerId = 0;
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
            lblDiamandCoin.Text = "4";
            lblEmeraudeCoin.Text = "4";
            lblOnyxCoin.Text = "4";
            lblRubisCoin.Text = "4";
            lblSaphirCoin.Text = "4";

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
        }

        private void ClickOnCard(object sender, EventArgs e)
        {
            //We get the value on the card and we split it to get all the values we need (number of prestige points and ressource)
            //Enable the button "Validate"

            TextBox txtBox = sender as TextBox;
            if (txtBox.Text != " ")
            {
                nbRubisNeeded = Convert.ToInt32(getBetween(txtBox.Text, "Rubis", "\r\n"));
                nbSaphirNeeded = Convert.ToInt32(getBetween(txtBox.Text, "Saphir", "\r\n"));
                nbOnyxNeeded = Convert.ToInt32(getBetween(txtBox.Text, "Onyx", "\r\n"));
                nbEmeraudeNeeded = Convert.ToInt32(getBetween(txtBox.Text, "Emeraude", "\r\n"));
                nbDiamandNeeded = Convert.ToInt32(getBetween(txtBox.Text, "Diamand", "\r\n"));

                if ((playerList[currentPlayerId].Coins[0] + playerList[currentPlayerId].Ressources[0] >= nbRubisNeeded) && (playerList[currentPlayerId].Coins[1] + playerList[currentPlayerId].Ressources[1] >= nbSaphirNeeded) && (playerList[currentPlayerId].Coins[2] + playerList[currentPlayerId].Ressources[2] >= nbOnyxNeeded) && (playerList[currentPlayerId].Coins[3] + playerList[currentPlayerId].Ressources[3] >= nbEmeraudeNeeded) && (playerList[currentPlayerId].Coins[4] + playerList[currentPlayerId].Ressources[4] >= nbDiamandNeeded))
                {
                    //get the text displayed in the textbox that has been clicked
                    DialogResult dialogResult = MessageBox.Show("Prendre la carte", "Passer", MessageBoxButtons.YesNo);
                    if (dialogResult == DialogResult.Yes)
                    {
                        CardSelected = txtBox.Text;
                        txtPlayerBookedCard.Text = txtBox.Text;
                        cmdValidateChoice.Visible = true;
                        cmdValidateChoice.Enabled = true;
                    }


                }
                else
                {
                    MessageBox.Show("Pas assez de ressources pour acheter cette carte");
                }
            }

        }

        //lire qqch entre deux choses //vient d'internet
        private string getBetween(string strSource, string strStart, string strEnd)
        {
            //supprime des char dans la premiere ligne pour eviter les nom de resource en double
            strSource = strSource.Substring(3, strSource.Length - 3);

            int Start, End;
            if (strSource.Contains(strStart) && strSource.Contains(strEnd))
            {
                Start = strSource.IndexOf(strStart, 0) + strStart.Length;
                End = strSource.IndexOf(strEnd, Start);
                if (strSource.Substring(Start, End - Start) == "")
                {
                    return "0";
                }
                else
                {
                    if (Start - (End - Start) == 0)
                    {
                        return strSource.Substring(Start, End - Start);
                    }
                    else
                    {
                        return "0";
                    }

                }
            }
            else
            {
                return "0";
            }
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
            cmdInsertPlayer.Visible = false;
            cmdDeletePlayer.Visible = false;


            LoadPlayer(currentPlayerId);

        }

        private void NbCoinsFPlayer()
        {
            if (nbPlayer == 2)
            {
                lblDiamandCoin.Text = "4";
                lblEmeraudeCoin.Text = "4";
                lblOnyxCoin.Text = "4";
                lblRubisCoin.Text = "4";
                lblSaphirCoin.Text = "4";
            }
            if (nbPlayer == 3)
            {
                lblDiamandCoin.Text = "5";
                lblEmeraudeCoin.Text = "5";
                lblOnyxCoin.Text = "5";
                lblRubisCoin.Text = "5";
                lblSaphirCoin.Text = "5";
            }
            if (nbPlayer == 4)
            {
                lblDiamandCoin.Text = "7";
                lblEmeraudeCoin.Text = "7";
                lblOnyxCoin.Text = "7";
                lblRubisCoin.Text = "7";
                lblSaphirCoin.Text = "7";
            }
        }
        /// <summary>
        /// load data about the current player
        /// </summary>
        /// <param name="id">identifier of the player</param>
        private void LoadPlayer(int id)
        {

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
            txtPlayerRubisCard.Text = playerList[id].Ressources[0].ToString();
            txtPlayerSaphirCard.Text = playerList[id].Ressources[1].ToString();
            txtPlayerOnyxCard.Text = playerList[id].Ressources[2].ToString();
            txtPlayerEmeraudeCard.Text = playerList[id].Ressources[3].ToString();
            txtPlayerDiamandCard.Text = playerList[id].Ressources[4].ToString();

            lblPlayerRubisCoin.Text = playerList[id].Coins[0].ToString();
            lblPlayerSaphirCoin.Text = playerList[id].Coins[1].ToString();
            lblPlayerOnyxCoin.Text = playerList[id].Coins[2].ToString();
            lblPlayerEmeraudeCoin.Text = playerList[id].Coins[3].ToString();
            lblPlayerDiamandCoin.Text = playerList[id].Coins[4].ToString();
            currentPlayerId = id;

            lblNbPtPrestige.Text = playerList[id].NbPrestige.ToString();

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
                if (lblRubisCoin.Text != "0")
                {
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
                            if ((nbRubis == 1 && nbSaphir == 1) || (nbRubis == 1 && nbOnyx == 1) || (nbRubis == 1 && nbEmeraude == 1) || (nbRubis == 1 && nbDiamand == 1) || ((lblRubisCoin.Text == "3" || lblRubisCoin.Text == "2" || lblRubisCoin.Text == "1") && nbRubis == 1))
                            {
                                MessageBox.Show("Choisissez une autre couleur");
                            }
                            else
                            {
                                nbRubis++;
                                int var = Convert.ToInt32(lblRubisCoin.Text) - 1;
                                lblRubisCoin.Text = var.ToString();
                                lblChoiceRubis.Text = nbRubis + "\r\n";
                                playerList[currentPlayerId].Coins[0] = nbRubis;
                                cmdValidateChoice.Enabled = true;

                            }
                        }
                    }
                }
                else
                {
                    MessageBox.Show("Il n'y en a plus");
                }
                nbtotal = 0;
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
                CardSelected = null;
                txtPlayerBookedCard.Text = null;
                cmdValidateChoice.Visible = true;
                lblChoiceSaphir.Visible = true;
                int nbtotal = nbRubis + nbSaphir + nbOnyx + nbEmeraude + nbDiamand;
                if (lblSaphirCoin.Text != "0")
                {
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
                            if ((nbRubis == 1 && nbSaphir == 1) || (nbSaphir == 1 && nbOnyx == 1) || (nbSaphir == 1 && nbEmeraude == 1) || (nbSaphir == 1 && nbDiamand == 1) || ((lblSaphirCoin.Text == "3" || lblSaphirCoin.Text == "2" || lblSaphirCoin.Text == "1") && nbSaphir == 1))
                            {
                                MessageBox.Show("Choisissez une autre couleur");
                            }
                            else
                            {
                                nbSaphir++;
                                int var = Convert.ToInt32(lblSaphirCoin.Text) - 1;
                                lblSaphirCoin.Text = var.ToString();
                                lblChoiceSaphir.Text = nbSaphir + "\r\n";
                                playerList[currentPlayerId].Coins[1] = nbSaphir;
                                cmdValidateChoice.Enabled = true;
                            }

                        }
                    }
                }
                else
                {
                    MessageBox.Show("Il n'y en a plus");
                }

                nbtotal = 0;
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
                CardSelected = null;
                txtPlayerBookedCard.Text = null;
                cmdValidateChoice.Visible = true;
                lblChoiceOnyx.Visible = true;
                int nbtotal = nbRubis + nbSaphir + nbOnyx + nbEmeraude + nbDiamand;
                if (lblOnyxCoin.Text != "0")
                {
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
                            if ((nbRubis == 1 && nbOnyx == 1) || (nbSaphir == 1 && nbOnyx == 1) || (nbOnyx == 1 && nbEmeraude == 1) || (nbOnyx == 1 && nbDiamand == 1) || ((lblOnyxCoin.Text == "3" || lblOnyxCoin.Text == "2" || lblOnyxCoin.Text == "1") && nbOnyx == 1))
                            {
                                MessageBox.Show("Choisissez une autre couleur");
                            }
                            else
                            {
                                nbOnyx++;
                                int var = Convert.ToInt32(lblOnyxCoin.Text) - 1;
                                lblOnyxCoin.Text = var.ToString();
                                lblChoiceOnyx.Text = nbOnyx + "\r\n";
                                playerList[currentPlayerId].Coins[2] = nbOnyx;
                                cmdValidateChoice.Enabled = true;
                            }
                        }
                    }
                }
                else
                {
                    MessageBox.Show("Il n'y en a plus");
                }
                nbtotal = 0;
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
                CardSelected = null;
                txtPlayerBookedCard.Text = null;
                cmdValidateChoice.Visible = true;
                lblChoiceEmeraude.Visible = true;
                int nbtotal = nbRubis + nbSaphir + nbOnyx + nbEmeraude + nbDiamand;
                if (lblEmeraudeCoin.Text != "0")
                {
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
                            if ((nbRubis == 1 && nbEmeraude == 1) || (nbEmeraude == 1 && nbOnyx == 1) || (nbSaphir == 1 && nbEmeraude == 1) || (nbEmeraude == 1 && nbDiamand == 1) || ((lblEmeraudeCoin.Text == "3" || lblEmeraudeCoin.Text == "2" || lblEmeraudeCoin.Text == "1") && nbEmeraude == 1))
                            {
                                MessageBox.Show("Choisissez une autre couleur");
                            }
                            else
                            {
                                nbEmeraude++;
                                int var = Convert.ToInt32(lblEmeraudeCoin.Text) - 1;
                                lblEmeraudeCoin.Text = var.ToString();
                                lblChoiceEmeraude.Text = nbEmeraude + "\r\n";
                                playerList[currentPlayerId].Coins[3] = nbEmeraude;
                                cmdValidateChoice.Enabled = true;
                            }
                        }
                    }
                }
                else
                {
                    MessageBox.Show("Il n'y en a plus");
                }
                nbtotal = 0;
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
                CardSelected = null;
                txtPlayerBookedCard.Text = null;
                cmdValidateChoice.Visible = true;
                lblChoiceDiamand.Visible = true;
                int nbtotal = nbRubis + nbSaphir + nbOnyx + nbEmeraude + nbDiamand;
                if (lblDiamandCoin.Text != "0")
                {
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
                            if ((nbRubis == 1 && nbDiamand == 1) || (nbDiamand == 1 && nbOnyx == 1) || (nbDiamand == 1 && nbEmeraude == 1) || (nbSaphir == 1 && nbDiamand == 1) || ((lblDiamandCoin.Text == "3" || lblDiamandCoin.Text == "2" || lblDiamandCoin.Text == "1") && nbDiamand == 1))
                            {
                                MessageBox.Show("Choisissez une autre couleur");
                            }
                            else
                            {
                                nbDiamand++;
                                int var = Convert.ToInt32(lblDiamandCoin.Text) - 1;
                                lblDiamandCoin.Text = var.ToString();
                                lblChoiceDiamand.Text = nbDiamand + "\r\n";
                                playerList[currentPlayerId].Coins[4] = nbDiamand;
                                cmdValidateChoice.Enabled = true;
                            }
                        }
                    }
                }
                else
                {
                    MessageBox.Show("Il n'y en a plus");
                }
                nbtotal = 0;
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
            if ((nbDiamand != 0) || (nbOnyx != 0) || (nbRubis != 0) || (nbSaphir != 0) || (nbEmeraude != 0) || (CardSelected != null))
            {
                if ((CardSelected != null) && (nbDiamand + nbOnyx + nbRubis + nbSaphir + nbEmeraude == 0))
                {
                    int nbCardStack1 = listCardOne.Count;
                    int if1 = 0;
                    foreach (Control allCard in flwCardLevel1.Controls)
                    {
                        if (CardSelected == allCard.Text)
                        {
                            if (if1 < nbCardStack1)
                            {
                                allCard.Text = listCardOne.Pop().ToString();
                            }
                            else
                            {
                                allCard.Text = " ";
                            }
                            if1++;
                        }
                    }
                    int nbCardStack2 = listCardTwo.Count;
                    int if2 = 0;
                    foreach (Control allCard in flwCardLevel2.Controls)
                    {
                        if (CardSelected == allCard.Text)
                        {
                            if (if2 < nbCardStack2)
                            {
                                allCard.Text = listCardTwo.Pop().ToString();
                            }
                            else
                            {
                                allCard.Text = " ";
                            }
                            if2++;
                        }
                    }
                    int nbCardStack3 = listCardTree.Count;
                    int if3 = 0;
                    foreach (Control allCard in flwCardLevel3.Controls)
                    {
                        if (CardSelected == allCard.Text)
                        {
                            if (if3 < nbCardStack3)
                            {
                                allCard.Text = listCardTree.Pop().ToString();
                            }
                            else
                            {
                                allCard.Text = " ";
                            }
                            if3++;
                        }
                    }

                    cmdValidateChoice.Enabled = false;
                    cmdNextPlayer.Enabled = true;

                    //enlever les resources lors de l'achat
                    string txtCardSelected = txtPlayerBookedCard.Text;
                    int nbRubisNeededLess = Convert.ToInt32(getBetween(txtCardSelected, "Rubis", "\r\n"));
                    int nbSaphirNeededLess = Convert.ToInt32(getBetween(txtCardSelected, "Saphir", "\r\n"));
                    int nbOnyxNeededLess = Convert.ToInt32(getBetween(txtCardSelected, "Onyx", "\r\n"));
                    int nbEmeraudeNeededLess = Convert.ToInt32(getBetween(txtCardSelected, "Emeraude", "\r\n"));
                    int nbDiamandNeededLess = Convert.ToInt32(getBetween(txtCardSelected, "Diamand", "\r\n"));

                    nbRubisNeededLess -= playerList[currentPlayerId].Ressources[0];
                    playerList[currentPlayerId].Coins[0] = (nbRubisNeededLess - playerList[currentPlayerId].Coins[0]) * -1;

                    nbSaphirNeededLess -= playerList[currentPlayerId].Ressources[1];
                    playerList[currentPlayerId].Coins[1] = (nbSaphirNeededLess - playerList[currentPlayerId].Coins[1]) * -1;

                    nbOnyxNeededLess -= playerList[currentPlayerId].Ressources[2];
                    playerList[currentPlayerId].Coins[2] = (nbOnyxNeededLess - playerList[currentPlayerId].Coins[2]) * -1;

                    nbEmeraudeNeededLess -= playerList[currentPlayerId].Ressources[3];
                    playerList[currentPlayerId].Coins[3] = (nbEmeraudeNeededLess - playerList[currentPlayerId].Coins[3]) * -1;

                    nbDiamandNeededLess -= playerList[currentPlayerId].Ressources[4];
                    playerList[currentPlayerId].Coins[4] = (nbDiamandNeededLess - playerList[currentPlayerId].Coins[4]) * -1;

                    string txtCardSelectedSec = txtPlayerBookedCard.Text.Substring(0, txtPlayerBookedCard.Text.Length - (txtPlayerBookedCard.Text.Length - 4));
                    if (txtCardSelectedSec == "Rubi")
                    {
                        playerList[currentPlayerId].Ressources[0]++;
                    }

                    if (txtCardSelectedSec == "Saph")
                    {
                        playerList[currentPlayerId].Ressources[1]++;
                    }

                    if (txtCardSelectedSec == "Onxy")
                    {
                        playerList[currentPlayerId].Ressources[2]++;
                    }

                    if (txtCardSelectedSec == "Emer")
                    {
                        playerList[currentPlayerId].Ressources[3]++;
                    }

                    if (txtCardSelectedSec == "Diam")
                    {
                        playerList[currentPlayerId].Ressources[4]++;
                    }

                    tempBookedCard = txtPlayerBookedCard.Text;
                    tempBookedCard = tempBookedCard.Substring(0, tempBookedCard.Length - (tempBookedCard.Length - 15));
                    nbRubisPres = Convert.ToInt32(getBetween(tempBookedCard, "is", "\r\n"));
                    nbSaphirPres = Convert.ToInt32(getBetween(tempBookedCard, "hir", "\r\n"));
                    nbOnyxPres = Convert.ToInt32(getBetween(tempBookedCard, "x", "\r\n"));
                    nbEmeraudePres = Convert.ToInt32(getBetween(tempBookedCard, "raude", "\r\n"));
                    nbDiamandPres = Convert.ToInt32(getBetween(tempBookedCard, "mand", "\r\n"));

                    //otenir noble si assez de ressources
                    TextBox txtBox = txtNoble1;
                    if (txtBox.Text != " ")
                    {
                        nbRubisNeeded = Convert.ToInt32(getBetween(txtBox.Text, "Rubis", "\r\n"));
                        nbSaphirNeeded = Convert.ToInt32(getBetween(txtBox.Text, "Saphir", "\r\n"));
                        nbOnyxNeeded = Convert.ToInt32(getBetween(txtBox.Text, "Onyx", "\r\n"));
                        nbEmeraudeNeeded = Convert.ToInt32(getBetween(txtBox.Text, "Emeraude", "\r\n"));
                        nbDiamandNeeded = Convert.ToInt32(getBetween(txtBox.Text, "Diamand", "\r\n"));

                        if ((playerList[currentPlayerId].Ressources[0] >= nbRubisNeeded) && (playerList[currentPlayerId].Ressources[1] >= nbSaphirNeeded) && (playerList[currentPlayerId].Ressources[2] >= nbOnyxNeeded) && (playerList[currentPlayerId].Ressources[3] >= nbEmeraudeNeeded) && (playerList[currentPlayerId].Ressources[4] >= nbDiamandNeeded))
                        {
                            MessageBox.Show("Vous avez obtenu un noble\r\n" + txtNoble1);
                            txtNoble1.Text = listCardFor.Pop().ToString();
                        }
                        
                    }

                    txtBox = txtNoble2;
                    if (txtBox.Text != " ")
                    {
                        nbRubisNeeded = Convert.ToInt32(getBetween(txtBox.Text, "Rubis", "\r\n"));
                        nbSaphirNeeded = Convert.ToInt32(getBetween(txtBox.Text, "Saphir", "\r\n"));
                        nbOnyxNeeded = Convert.ToInt32(getBetween(txtBox.Text, "Onyx", "\r\n"));
                        nbEmeraudeNeeded = Convert.ToInt32(getBetween(txtBox.Text, "Emeraude", "\r\n"));
                        nbDiamandNeeded = Convert.ToInt32(getBetween(txtBox.Text, "Diamand", "\r\n"));

                        if ((playerList[currentPlayerId].Ressources[0] >= nbRubisNeeded) && (playerList[currentPlayerId].Ressources[1] >= nbSaphirNeeded) && (playerList[currentPlayerId].Ressources[2] >= nbOnyxNeeded) && (playerList[currentPlayerId].Ressources[3] >= nbEmeraudeNeeded) && (playerList[currentPlayerId].Ressources[4] >= nbDiamandNeeded))
                        {
                            MessageBox.Show("Vous avez obtenu un noble\r\n" + txtNoble2);
                            txtNoble2.Text = listCardFor.Pop().ToString();
                        }

                    }

                    txtBox = txtNoble3;
                    if (txtBox.Text != " ")
                    {
                        nbRubisNeeded = Convert.ToInt32(getBetween(txtBox.Text, "Rubis", "\r\n"));
                        nbSaphirNeeded = Convert.ToInt32(getBetween(txtBox.Text, "Saphir", "\r\n"));
                        nbOnyxNeeded = Convert.ToInt32(getBetween(txtBox.Text, "Onyx", "\r\n"));
                        nbEmeraudeNeeded = Convert.ToInt32(getBetween(txtBox.Text, "Emeraude", "\r\n"));
                        nbDiamandNeeded = Convert.ToInt32(getBetween(txtBox.Text, "Diamand", "\r\n"));

                        if ((playerList[currentPlayerId].Ressources[0] >= nbRubisNeeded) && (playerList[currentPlayerId].Ressources[1] >= nbSaphirNeeded) && (playerList[currentPlayerId].Ressources[2] >= nbOnyxNeeded) && (playerList[currentPlayerId].Ressources[3] >= nbEmeraudeNeeded) && (playerList[currentPlayerId].Ressources[4] >= nbDiamandNeeded))
                        {
                            MessageBox.Show("Vous avez obtenu un noble\r\n" + txtNoble3);
                            txtNoble3.Text = listCardFor.Pop().ToString();
                        }

                    }

                    txtBox = txtNoble4;
                    if (txtBox.Text != " ")
                    {
                        nbRubisNeeded = Convert.ToInt32(getBetween(txtBox.Text, "Rubis", "\r\n"));
                        nbSaphirNeeded = Convert.ToInt32(getBetween(txtBox.Text, "Saphir", "\r\n"));
                        nbOnyxNeeded = Convert.ToInt32(getBetween(txtBox.Text, "Onyx", "\r\n"));
                        nbEmeraudeNeeded = Convert.ToInt32(getBetween(txtBox.Text, "Emeraude", "\r\n"));
                        nbDiamandNeeded = Convert.ToInt32(getBetween(txtBox.Text, "Diamand", "\r\n"));

                        if ((playerList[currentPlayerId].Ressources[0] >= nbRubisNeeded) && (playerList[currentPlayerId].Ressources[1] >= nbSaphirNeeded) && (playerList[currentPlayerId].Ressources[2] >= nbOnyxNeeded) && (playerList[currentPlayerId].Ressources[3] >= nbEmeraudeNeeded) && (playerList[currentPlayerId].Ressources[4] >= nbDiamandNeeded))
                        {
                            MessageBox.Show("Vous avez obtenu un noble\r\n" + txtNoble4);
                            txtNoble4.Text = listCardFor.Pop().ToString();
                        }

                    }



                    totPresPt += nbRubisPres + nbSaphirPres + nbOnyxPres + nbEmeraudePres + nbDiamandPres;
                    playerList[currentPlayerId].NbPrestige = totPresPt;
                    lblNbPtPrestige.Text = totPresPt.ToString();
                    if (totPresPt >= 15)
                    {
                        MessageBox.Show(playerList[currentPlayerId].Name + " a Gagné!");
                        Application.Exit();
                    }

                }
                else if ((CardSelected != null) && (nbDiamand + nbOnyx + nbRubis + nbSaphir + nbEmeraude == 0))
                {

                    cmdValidateChoice.Enabled = false;
                    cmdNextPlayer.Enabled = true;
                }
                else
                {
                    //si on appuie sur valide et qu'on a une carte et des jetons de selectionné
                    if ((CardSelected != null) && (nbDiamand + nbOnyx + nbRubis + nbSaphir + nbEmeraude != 0))
                    {
                        CardSelected = null;
                        nbDiamand = 0;
                        lblChoiceDiamand.Text = "0";
                        nbOnyx = 0;
                        lblChoiceOnyx.Text = "0";
                        nbRubis = 0;
                        lblChoiceRubis.Text = "0";
                        nbSaphir = 0;
                        lblChoiceSaphir.Text = "0";
                        nbEmeraude = 0;
                        lblChoiceEmeraude.Text = "0";
                        MessageBox.Show("Vous avez une carte et des jetons de séléctionné");
                        cmdValidateChoice.Enabled = true;
                        cmdNextPlayer.Enabled = false;
                    }

                }
                //deux fois
                lblPlayerRubisCoin.Text = playerList[currentPlayerId].Coins[0].ToString();
                lblPlayerSaphirCoin.Text = playerList[currentPlayerId].Coins[1].ToString();
                lblPlayerOnyxCoin.Text = playerList[currentPlayerId].Coins[2].ToString();
                lblPlayerEmeraudeCoin.Text = playerList[currentPlayerId].Coins[3].ToString();
                lblPlayerDiamandCoin.Text = playerList[currentPlayerId].Coins[4].ToString();

                cmdValidateChoice.Enabled = false;
                cmdNextPlayer.Enabled = true;


            }
        }

        /// <summary>
        /// click on the insert button to insert player in the game
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmdInsertPlayer_Click(object sender, EventArgs e)
        {

            if (nbPlayer < 4)
            {
                /// il faudrait faire un sous programe qui renvoie le nom du joueur !
                nbPlayer++;
                if (nbPlayer >= 4)
                {
                    cmdInsertPlayer.Enabled = false;
                }
                NbCoinsFPlayer();
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

            currentPlayerId++;
            if (currentPlayerId >= nbPlayer)
            {
                currentPlayerId = 0;
            }
            nbRubis = 0;
            nbOnyx = 0;
            nbEmeraude = 0;
            nbDiamand = 0;
            nbSaphir = 0;

            cmdValidateChoice.Enabled = false;
            cmdNextPlayer.Visible = false;

            LoadPlayer(currentPlayerId);

        }
        /// <summary>
        /// Click on the delete button to delete a player of the game 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmdDeletePlayer_Click(object sender, EventArgs e)
        {

            if (nbPlayer > 2)
            {
                nbPlayer--;
                if (nbPlayer <= 2)
                {
                    cmdDeletePlayer.Enabled = false;

                }
                NbCoinsFPlayer();
                cmdInsertPlayer.Enabled = true;

                lblNbPlayer.Text = nbPlayer.ToString();
            }
        }

        private void lblChoiceRubis_Click(object sender, EventArgs e)
        {
            if (nbRubis == 1)
            {
                lblChoiceRubis.Visible = false;
                if (nbSaphir == 0 && nbEmeraude == 0 && nbOnyx == 0 && nbDiamand == 0)
                {
                    cmdValidateChoice.Visible = false;
                }
            }
            else
            {

                int var = Convert.ToInt32(lblRubisCoin.Text) + 1;
                lblRubisCoin.Text = var.ToString();
                lblChoiceRubis.Text = nbRubis + "\r\n";
            }
            nbRubis--;
        }

        private void lblChoiceSaphir_Click(object sender, EventArgs e)
        {
            if (nbSaphir == 1)
            {
                lblChoiceSaphir.Visible = false;
                if (nbRubis == 0 && nbEmeraude == 0 && nbOnyx == 0 && nbDiamand == 0)
                {
                    cmdValidateChoice.Visible = false;
                }
            }
            else
            {

                int var = Convert.ToInt32(lblSaphirCoin.Text) + 1;
                lblSaphirCoin.Text = var.ToString();
                lblChoiceSaphir.Text = nbSaphir + "\r\n";
            }
            nbSaphir--;
        }

        private void lblChoiceOnyx_Click(object sender, EventArgs e)
        {
            if (nbOnyx == 1)
            {
                lblChoiceOnyx.Visible = false;
                if (nbRubis == 0 && nbEmeraude == 0 && nbSaphir == 0 && nbDiamand == 0)
                {
                    cmdValidateChoice.Visible = false;
                }
            }
            else
            {

                int var = Convert.ToInt32(lblOnyxCoin.Text) + 1;
                lblOnyxCoin.Text = var.ToString();
                lblChoiceOnyx.Text = nbOnyx + "\r\n";
            }
            nbOnyx--;
        }

        private void lblChoiceEmeraude_Click(object sender, EventArgs e)
        {
            if (nbEmeraude == 1)
            {
                lblChoiceEmeraude.Visible = false;
                if (nbRubis == 0 && nbOnyx == 0 && nbSaphir == 0 && nbDiamand == 0)
                {
                    cmdValidateChoice.Visible = false;
                }
            }
            else
            {

                int var = Convert.ToInt32(lblEmeraudeCoin.Text) + 1;
                lblEmeraudeCoin.Text = var.ToString();
                lblChoiceEmeraude.Text = nbEmeraude + "\r\n";
            }
            nbEmeraude--;
        }

        private void lblChoiceDiamand_Click(object sender, EventArgs e)
        {
            if (nbDiamand == 1)
            {
                lblChoiceDiamand.Visible = false;
                if (nbRubis == 0 && nbOnyx == 0 && nbSaphir == 0 && nbEmeraude == 0)
                {
                    cmdValidateChoice.Visible = false;
                }
            }
            else
            {

                int var = Convert.ToInt32(lblDiamandCoin.Text) + 1;
                lblDiamandCoin.Text = var.ToString();
                lblChoiceDiamand.Text = nbDiamand + "\r\n";
            }
            nbDiamand--;
        }
    }
}
