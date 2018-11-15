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
        private int totCoinChoice;
        public int nbPlayer = 2;

        
        private int nbRubisNeeded;
        private int nbSaphirNeeded;
        private int nbOnyxNeeded;
        private int nbEmeraudeNeeded;
        private int nbDiamandNeeded;

        //utilisé pour claculer le nombre de points de prestige
        private string tempBookedCard;
        private int nbRubisPres;
        private int nbSaphirPres;
        private int nbOnyxPres;
        private int nbEmeraudePres;
        private int nbDiamandPres;
        private int totPresPt;

        private string CardSelected;

        //crée une liste de joueurs Avec un nom donné un id donné un certain nombre de ressources de coins et de point de prestige 
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
        /// initialize the game
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
            //donne le nombre de jetons de base
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

            //prépare la fenêtre de jeu
            this.Width = 680;
            this.Height = 540;

            enableClicLabel = false;

            cmdDeletePlayer.Enabled = false;
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

        /// <summary>
        /// verifie si l'on peut acheter la carte sur la quelle on a clique
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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

        /// <summary>
        /// read text between 2 parametres
        /// </summary>
        /// <param name="strSource">le texte source</param>
        /// <param name="strStart">le permier mot</param>
        /// /// <param name="strEnd">le second mot</param>
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
                    if (strSource.Substring(Start, End - Start)=="\t\t")
                    {
                        return "0";
                    }
                    else
                    {
                        return strSource.Substring(Start, End - Start);
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
            //prépare la fenêtre de jeu
            this.Width = 680;
            this.Height = 780;
            cmdInsertPlayer.Visible = false;
            cmdDeletePlayer.Visible = false;

            //charge le premier joueur
            LoadPlayer(currentPlayerId);

        }

        /// <summary>
        /// donne le nombre de jetons suivant le nombre de joueurs choisis
        /// </summary>
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

            //initialise les variables
            string name = playerList[id].Name;
            CardSelected = null;
            txtPlayerBookedCard.Text = null;

            //no coins or card selected yet, labels are empty
            lblChoiceDiamand.Text = "";
            lblChoiceOnyx.Text = "";
            lblChoiceRubis.Text = "";
            lblChoiceSaphir.Text = "";
            lblChoiceEmeraude.Text = "";

            //charge le nomre de ressources du joueur
            lblChoiceCard.Text = "";
            txtPlayerRubisCard.Text = playerList[id].Ressources[0].ToString();
            txtPlayerSaphirCard.Text = playerList[id].Ressources[1].ToString();
            txtPlayerOnyxCard.Text = playerList[id].Ressources[2].ToString();
            txtPlayerEmeraudeCard.Text = playerList[id].Ressources[3].ToString();
            txtPlayerDiamandCard.Text = playerList[id].Ressources[4].ToString();

            //charge le nomre de jetons du joueur
            lblPlayerRubisCoin.Text = playerList[id].Coins[0].ToString();
            lblPlayerSaphirCoin.Text = playerList[id].Coins[1].ToString();
            lblPlayerOnyxCoin.Text = playerList[id].Coins[2].ToString();
            lblPlayerEmeraudeCoin.Text = playerList[id].Coins[3].ToString();
            lblPlayerDiamandCoin.Text = playerList[id].Coins[4].ToString();

            //set l'id du joueur dans une autre variable
            currentPlayerId = id;

            //charge le nomre de point de prestige du joueur
            lblNbPtPrestige.Text = playerList[id].NbPrestige.ToString();
            //affiche le joueur actuel
            lblPlayer.Text = "Jeu de " + name;
            //désactive le boutton jouer
            cmdPlay.Enabled = false;
        }

        /// <summary>
        /// click on the red coin (rubis) to tell the player has selected this coin
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void lblRubisCoin_Click(object sender, EventArgs e)
        {
            nbRubis = veriftakecoin(lblChoiceRubis, lblRubisCoin, nbRubis);
        }

        /// <summary>
        /// click on the blue coin (saphir) to tell the player has selected this coin
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void lblSaphirCoin_Click(object sender, EventArgs e)
        {
            nbSaphir = veriftakecoin(lblChoiceSaphir, lblSaphirCoin, nbSaphir);
        }

        /// <summary>
        /// click on the black coin (onyx) to tell the player has selected this coin
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void lblOnyxCoin_Click(object sender, EventArgs e)
        {
            nbOnyx = veriftakecoin(lblChoiceOnyx, lblOnyxCoin, nbOnyx);
        }

        /// <summary>
        /// click on the green coin (emeraude) to tell the player has selected this coin
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void lblEmeraudeCoin_Click(object sender, EventArgs e)
        {
            nbEmeraude = veriftakecoin(lblChoiceEmeraude, lblEmeraudeCoin, nbEmeraude);
        }

        /// <summary>
        /// click on the white coin (diamand) to tell the player has selected this coin
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void lblDiamandCoin_Click(object sender, EventArgs e)
        {
            nbDiamand = veriftakecoin(lblChoiceDiamand, lblDiamandCoin,nbDiamand);
        }

        /// <summary>
        /// on verifie ce que l'on peut faire quand l'on appuye sur un jeton
        /// </summary>
        /// <param name="lblChoice">pour pouvoir modifier le label du nombre de jeton séléctionne du type de jeton choisis</param>
        /// <param name="lblCoin">pour pouvoir modifier le nombre de jeton restant du type de jeton choisis</param>
        /// /// <param name="nbJetonChoisis">c est le nombre de jeton deja prris par le joueur du type de jeton choisis</param>
        private int veriftakecoin(Label lblChoice, Label lblCoin, int nbJetonChoisis)
        {
            if (enableClicLabel)
            {
                // enlève la carte choisie si il y en a une
                CardSelected = null;
                // l'efface de la text box
                txtPlayerBookedCard.Text = null;
                //le bouton valider apparait
                cmdValidateChoice.Visible = true;
                //le label du jeton cible apparait
                lblChoice.Visible = true;
                //calcule le nombre total de jeton choisis
                int nbtotal = nbRubis + nbSaphir + nbOnyx + nbEmeraude + nbDiamand;
                if (lblCoin.Text != "0")
                {
                    //si il a pris 3 jetons il ne peut pas en prendre plus  
                    if (nbtotal >= 3)
                    {
                        MessageBox.Show("Vous ne pouvez pas prendre plus de jetons");
                    }
                    else
                    {
                        //si il a déjà pris 2 jetons d'eune couleur il ne peut plus prendre de jetons
                        if (nbRubis == 2 || nbSaphir == 2 || nbOnyx == 2 || nbEmeraude == 2 || nbDiamand == 2)
                        {
                            MessageBox.Show("Vous pouvez prendre maximum 2 pièces de la même couleur");
                        }
                        else
                        {
                            // si il a déjà pris 1 jetons de ce type + un autre il ne peut pas reprendre de ce type
                            if ((nbRubis == 1 && nbJetonChoisis == 1) || (nbJetonChoisis == 1 && nbOnyx == 1) || (nbJetonChoisis == 1 && nbEmeraude == 1) || (nbSaphir == 1 && nbJetonChoisis == 1) || ((lblCoin.Text == "3" || lblCoin.Text == "2" || lblCoin.Text == "1") && nbJetonChoisis == 1))
                            {
                                MessageBox.Show("Choisissez une autre couleur");
                            }
                            else
                            {
                                nbJetonChoisis++;
                                int var = Convert.ToInt32(lblCoin.Text) - 1;
                                lblCoin.Text = var.ToString();
                                lblChoice.Text = nbJetonChoisis + "\r\n";

                                //on peut appuyer sur le bonton pour passer à la suite
                                cmdValidateChoice.Enabled = true;
                            }
                        }
                    }
                }
                // si il n'y a plus de coin choisis il ne peut plus en prendre
                else
                {
                    MessageBox.Show("Il n'y en a plus");
                }
                //il remet le nomre total de jetons à 0
                nbtotal = 0;
            }
            return nbJetonChoisis;
        }

        /// <summary>
        /// click on the validate button to approve the selection of coins or card
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmdValidateChoice_Click(object sender, EventArgs e)
        {
            //affiche le boutton pour passer au joueur suivant
            cmdNextPlayer.Visible = true;
            //TO DO Check if card or coins are selected, impossible to do both at the same time
            if ((nbDiamand != 0) || (nbOnyx != 0) || (nbRubis != 0) || (nbSaphir != 0) || (nbEmeraude != 0) || (CardSelected != null))
            {
                //calcule le nombre de jetons total choisi
                totCoinChoice = nbDiamand + nbOnyx + nbRubis + nbSaphir + nbEmeraude;
                if ((CardSelected != null)&&(totCoinChoice == 0))
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

                    //enlever les resources lors de l'achat
                    string txtCardSelected = txtPlayerBookedCard.Text;
                    int nbRubisNeededLess = Convert.ToInt32(getBetween(txtCardSelected, "Rubis", "\r\n"));
                    int nbSaphirNeededLess = Convert.ToInt32(getBetween(txtCardSelected, "Saphir", "\r\n"));
                    int nbOnyxNeededLess = Convert.ToInt32(getBetween(txtCardSelected, "Onyx", "\r\n"));
                    int nbEmeraudeNeededLess = Convert.ToInt32(getBetween(txtCardSelected, "Emeraude", "\r\n"));
                    int nbDiamandNeededLess = Convert.ToInt32(getBetween(txtCardSelected, "Diamand", "\r\n"));


                    nbRubisNeededLess -= playerList[currentPlayerId].Ressources[0];
                    if (nbRubisNeededLess>=0)
                    {
                        playerList[currentPlayerId].Coins[0] = (nbRubisNeededLess - playerList[currentPlayerId].Coins[0]) * -1;
                        int varRubisCalc = Convert.ToInt32(lblRubisCoin.Text) + nbRubisNeededLess;
                        lblRubisCoin.Text = varRubisCalc.ToString();
                    }
                    
                    nbSaphirNeededLess -= playerList[currentPlayerId].Ressources[1];
                    if (nbSaphirNeededLess >= 0)
                    {
                        playerList[currentPlayerId].Coins[1] = (nbSaphirNeededLess - playerList[currentPlayerId].Coins[1]) * -1;
                        int varSaphirCalc = Convert.ToInt32(lblSaphirCoin.Text) + nbSaphirNeededLess;
                        lblSaphirCoin.Text = varSaphirCalc.ToString();
                    }
                    
                    nbOnyxNeededLess -= playerList[currentPlayerId].Ressources[2];
                    if (nbOnyxNeededLess >= 0)
                    {
                        playerList[currentPlayerId].Coins[2] = (nbOnyxNeededLess - playerList[currentPlayerId].Coins[2]) * -1;
                        int varOnyxCalc = Convert.ToInt32(lblOnyxCoin.Text) + nbOnyxNeededLess;
                        lblOnyxCoin.Text = varOnyxCalc.ToString();
                    }
                    
                    nbEmeraudeNeededLess -= playerList[currentPlayerId].Ressources[3];
                    if (nbEmeraudeNeededLess >= 0)
                    {
                        playerList[currentPlayerId].Coins[3] = (nbEmeraudeNeededLess - playerList[currentPlayerId].Coins[3]) * -1;
                        int varEmeraudeCalc = Convert.ToInt32(lblEmeraudeCoin.Text) + nbEmeraudeNeededLess;
                        lblEmeraudeCoin.Text = varEmeraudeCalc.ToString();
                    }
                    
                    nbDiamandNeededLess -= playerList[currentPlayerId].Ressources[4];
                    if (nbDiamandNeededLess >= 0)
                    {
                        playerList[currentPlayerId].Coins[4] = (nbDiamandNeededLess - playerList[currentPlayerId].Coins[4]) * -1;
                        int varDiamandCalc = Convert.ToInt32(lblDiamandCoin.Text) + nbDiamandNeededLess;
                        lblDiamandCoin.Text = varDiamandCalc.ToString();
                    }
                    
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

                    totPresPt += nbRubisPres + nbSaphirPres + nbOnyxPres + nbEmeraudePres + nbDiamandPres;

                    //otenir noble si assez de ressources
                    int nbRubisNeededNob = 0;
                    int nbSaphirNeededNob = 0;
                    int nbOnyxNeededNob = 0;
                    int nbEmeraudeNeededNob = 0;
                    int nbDiamandNeededNob = 0;
                    TextBox txtBox = txtNoble1;
                    if (txtBox.Text != " ")
                    {
                        nbRubisNeededNob = Convert.ToInt32(getBetween(txtBox.Text, "Rubis", "\r\n"));
                        nbSaphirNeededNob = Convert.ToInt32(getBetween(txtBox.Text, "Saphir", "\r\n"));
                        nbOnyxNeededNob = Convert.ToInt32(getBetween(txtBox.Text, "Onyx", "\r\n"));
                        nbEmeraudeNeededNob = Convert.ToInt32(getBetween(txtBox.Text, "Emeraude", "\r\n"));
                        nbDiamandNeededNob = Convert.ToInt32(getBetween(txtBox.Text, "Diamand", "\r\n"));

                        if ((playerList[currentPlayerId].Ressources[0] >= nbRubisNeededNob) && (playerList[currentPlayerId].Ressources[1] >= nbSaphirNeededNob) && (playerList[currentPlayerId].Ressources[2] >= nbOnyxNeededNob) && (playerList[currentPlayerId].Ressources[3] >= nbEmeraudeNeededNob) && (playerList[currentPlayerId].Ressources[4] >= nbDiamandNeededNob))
                        {
                            MessageBox.Show("Vous avez obtenu des points de perstige grâce à un noble");
                            txtNoble1.Text = listCardFor.Pop().ToString();
                        }
                        
                    }

                    txtBox = txtNoble2;
                    if (txtBox.Text != " ")
                    {
                        nbRubisNeededNob = Convert.ToInt32(getBetween(txtBox.Text, "Rubis", "\r\n"));
                        nbSaphirNeededNob = Convert.ToInt32(getBetween(txtBox.Text, "Saphir", "\r\n"));
                        nbOnyxNeededNob = Convert.ToInt32(getBetween(txtBox.Text, "Onyx", "\r\n"));
                        nbEmeraudeNeededNob = Convert.ToInt32(getBetween(txtBox.Text, "Emeraude", "\r\n"));
                        nbDiamandNeededNob = Convert.ToInt32(getBetween(txtBox.Text, "Diamand", "\r\n"));

                        if ((playerList[currentPlayerId].Ressources[0] >= nbRubisNeededNob) && (playerList[currentPlayerId].Ressources[1] >= nbSaphirNeededNob) && (playerList[currentPlayerId].Ressources[2] >= nbOnyxNeededNob) && (playerList[currentPlayerId].Ressources[3] >= nbEmeraudeNeededNob) && (playerList[currentPlayerId].Ressources[4] >= nbDiamandNeededNob))
                        {
                            MessageBox.Show("Vous avez obtenu des points de perstige grâce à un noble");
                            txtNoble2.Text = listCardFor.Pop().ToString();
                        }

                    }

                    txtBox = txtNoble3;
                    if (txtBox.Text != " ")
                    {
                        nbRubisNeededNob = Convert.ToInt32(getBetween(txtBox.Text, "Rubis", "\r\n"));
                        nbSaphirNeededNob = Convert.ToInt32(getBetween(txtBox.Text, "Saphir", "\r\n"));
                        nbOnyxNeededNob = Convert.ToInt32(getBetween(txtBox.Text, "Onyx", "\r\n"));
                        nbEmeraudeNeededNob = Convert.ToInt32(getBetween(txtBox.Text, "Emeraude", "\r\n"));
                        nbDiamandNeededNob = Convert.ToInt32(getBetween(txtBox.Text, "Diamand", "\r\n"));

                        if ((playerList[currentPlayerId].Ressources[0] >= nbRubisNeededNob) && (playerList[currentPlayerId].Ressources[1] >= nbSaphirNeededNob) && (playerList[currentPlayerId].Ressources[2] >= nbOnyxNeededNob) && (playerList[currentPlayerId].Ressources[3] >= nbEmeraudeNeededNob) && (playerList[currentPlayerId].Ressources[4] >= nbDiamandNeededNob))
                        {
                            MessageBox.Show("Vous avez obtenu des points de perstige grâce à un noble");
                            txtNoble3.Text = listCardFor.Pop().ToString();
                        }

                    }

                    txtBox = txtNoble4;
                    if (txtBox.Text != " ")
                    {
                        nbRubisNeededNob = Convert.ToInt32(getBetween(txtBox.Text, "Rubis", "\r\n"));
                        nbSaphirNeededNob = Convert.ToInt32(getBetween(txtBox.Text, "Saphir", "\r\n"));
                        nbOnyxNeededNob = Convert.ToInt32(getBetween(txtBox.Text, "Onyx", "\r\n"));
                        nbEmeraudeNeededNob = Convert.ToInt32(getBetween(txtBox.Text, "Emeraude", "\r\n"));
                        nbDiamandNeededNob = Convert.ToInt32(getBetween(txtBox.Text, "Diamand", "\r\n"));

                        if ((playerList[currentPlayerId].Ressources[0] >= nbRubisNeededNob) && (playerList[currentPlayerId].Ressources[1] >= nbSaphirNeededNob) && (playerList[currentPlayerId].Ressources[2] >= nbOnyxNeededNob) && (playerList[currentPlayerId].Ressources[3] >= nbEmeraudeNeededNob) && (playerList[currentPlayerId].Ressources[4] >= nbDiamandNeededNob))
                        {
                            
                            MessageBox.Show("Vous avez obtenu des points de perstige grâce à un noble");
                            txtNoble4.Text = listCardFor.Pop().ToString();
                        }

                    }
                    
                    playerList[currentPlayerId].NbPrestige = totPresPt;
                    lblNbPtPrestige.Text = totPresPt.ToString();
                    if (totPresPt >= 15)
                    {
                        MessageBox.Show(playerList[currentPlayerId].Name + " a Gagné!");
                        Application.Exit();
                    }
                    cmdValidateChoice.Enabled = false;
                    cmdNextPlayer.Enabled = true;

                }
                //si on appuie sur valide et qu'on a une carte et des jetons de selectionné
                else if ((CardSelected != null) && (totCoinChoice != 0))
                {
                    CardSelected = null;
                    txtPlayerBookedCard.Text = "";
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

                if ((CardSelected == null) && (totCoinChoice != 0))
                {
                    cmdValidateChoice.Enabled = false;
                    cmdNextPlayer.Enabled = true;
                }

                    //deux fois
                    playerList[currentPlayerId].Coins[0] += nbRubis;
                playerList[currentPlayerId].Coins[1] += nbSaphir;
                playerList[currentPlayerId].Coins[2] += nbOnyx;
                playerList[currentPlayerId].Coins[3] += nbEmeraude;
                playerList[currentPlayerId].Coins[4] += nbDiamand;

                lblPlayerRubisCoin.Text = playerList[currentPlayerId].Coins[0].ToString();
                lblPlayerSaphirCoin.Text = playerList[currentPlayerId].Coins[1].ToString();
                lblPlayerOnyxCoin.Text = playerList[currentPlayerId].Coins[2].ToString();
                lblPlayerEmeraudeCoin.Text = playerList[currentPlayerId].Coins[3].ToString();
                lblPlayerDiamandCoin.Text = playerList[currentPlayerId].Coins[4].ToString();

                txtPlayerRubisCard.Text= playerList[currentPlayerId].Ressources[0].ToString();
                txtPlayerSaphirCard.Text= playerList[currentPlayerId].Ressources[1].ToString();
                txtPlayerOnyxCard.Text= playerList[currentPlayerId].Ressources[2].ToString();
                txtPlayerEmeraudeCard.Text= playerList[currentPlayerId].Ressources[3].ToString();
                txtPlayerDiamandCard.Text= playerList[currentPlayerId].Ressources[4].ToString();

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
                // Ajoute un joueur
                nbPlayer++;

                if (nbPlayer >= 4)
                {
                    cmdInsertPlayer.Enabled = false;
                }
                //méthode pour choisir le nombre de jeton par rapport au nombre de joueur
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
            //il incrémente l'id du joueur pour passer au suivant
            currentPlayerId++;
            //si on arrive au dérnier joueur il revient au premier 
            if (currentPlayerId >= nbPlayer)
            {
                currentPlayerId = 0;
            }
            //il remet toutes les variables à 0 
            nbRubis=0;
            nbOnyx=0;
            nbEmeraude=0;
            nbDiamand=0;
            nbSaphir=0;

            cmdValidateChoice.Enabled = false;
            cmdNextPlayer.Visible = false;
            
            //il charge le joueur suivant
            LoadPlayer(currentPlayerId);

        }
        /// <summary>
        /// Click on the delete button to delete a player of the game 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmdDeletePlayer_Click(object sender, EventArgs e)
        {
            //si le nombre de joueur est plus grand que 2 on peut en enlever
            if (nbPlayer >2)
            {
                nbPlayer--;
                if (nbPlayer <= 2)
                {
                    cmdDeletePlayer.Enabled = false;

                }
                //méthode pour choisir le nombre de jeton par rapport au nombre de joueur
                NbCoinsFPlayer();
                cmdInsertPlayer.Enabled = true;

                lblNbPlayer.Text = nbPlayer.ToString();
            }
        }

        private void lblChoiceRubis_Click(object sender, EventArgs e)
        {
            if (nbRubis <= 1)
            {
                nbRubis = 1;
                lblChoiceRubis.Visible = false;
                if (nbSaphir == 0 && nbEmeraude == 0 && nbOnyx == 0 && nbDiamand == 0)
                {
                    cmdValidateChoice.Visible = false;
                }
            }
            else
            {
                lblChoiceRubis.Text = nbRubis + "\r\n";
            }
            int var = Convert.ToInt32(lblRubisCoin.Text) + 1;
            lblRubisCoin.Text = var.ToString();
            nbRubis--;
        }

        private void lblChoiceSaphir_Click(object sender, EventArgs e)
        {
            if (nbSaphir <= 1)
            {
                nbSaphir = 1;
                lblChoiceSaphir.Visible = false;
                if (nbRubis == 0 && nbEmeraude == 0 && nbOnyx == 0 && nbDiamand == 0)
                {
                    cmdValidateChoice.Visible = false;
                }
            }
            else
            {
                lblChoiceSaphir.Text = nbSaphir + "\r\n";
            }
            nbSaphir--;
            int var = Convert.ToInt32(lblSaphirCoin.Text) + 1;
            lblSaphirCoin.Text = var.ToString();
        }

        private void lblChoiceOnyx_Click(object sender, EventArgs e)
        {
            if (nbOnyx <= 1)
            {
                nbOnyx = 1;
                lblChoiceOnyx.Visible = false;
                if (nbRubis == 0 && nbEmeraude == 0 && nbSaphir == 0 && nbDiamand == 0)
                {
                    cmdValidateChoice.Visible = false;
                }
            }
            else
            {
                lblChoiceOnyx.Text = nbOnyx + "\r\n";
            }
            nbOnyx--;
            int var = Convert.ToInt32(lblOnyxCoin.Text) + 1;
            lblOnyxCoin.Text = var.ToString();
        }

        private void lblChoiceEmeraude_Click(object sender, EventArgs e)
        {
            if (nbEmeraude <= 1)
            {
                nbEmeraude = 1;
                lblChoiceEmeraude.Visible = false;
                if (nbRubis == 0 && nbOnyx == 0 && nbSaphir == 0 && nbDiamand == 0)
                {
                    cmdValidateChoice.Visible = false;
                }
            }
            else
            {
                lblChoiceEmeraude.Text = nbEmeraude + "\r\n";
            }
            nbEmeraude--;
            int var = Convert.ToInt32(lblEmeraudeCoin.Text) + 1;
            lblEmeraudeCoin.Text = var.ToString();
        }

        private void lblChoiceDiamand_Click(object sender, EventArgs e)
        {
            if (nbDiamand <= 1)
            {
                nbDiamand = 1;
                lblChoiceDiamand.Visible = false;
                if (nbRubis == 0 && nbOnyx == 0 && nbSaphir == 0 && nbEmeraude == 0)
                {
                    cmdValidateChoice.Visible = false;
                }
            }
            else
            {
                lblChoiceDiamand.Text = nbDiamand + "\r\n";
            }
            nbDiamand--;
            int var = Convert.ToInt32(lblDiamandCoin.Text) + 1;
            lblDiamandCoin.Text = var.ToString();
        }
    }
}
