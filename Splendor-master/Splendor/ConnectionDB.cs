using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;

namespace Splendor
{
    /// <summary>
    /// contains methods and attributes to connect and deal with the database
    /// TO DO : le modèle de données n'est pas super, à revoir!!!!
    /// </summary>
    class ConnectionDB
    {
        //connection to the database
        private SQLiteConnection m_dbConnection; 

        /// <summary>
        /// constructor : creates the connection to the database SQLite
        /// </summary>
        public ConnectionDB()
        {

            SQLiteConnection.CreateFile("Splendor.sqlite");
            
            m_dbConnection = new SQLiteConnection("Data Source=Splendor.sqlite;Version=3;");
            m_dbConnection.Open();

            //create and insert players
            CreateInsertPlayer();
            //Create and insert cards
            //TO DO
            CreateInsertCardCost();
            //Create and insert ressources
            //TO DO
            CreateInsertRessources();
        }
        
        /// <summary>
        /// get the list of cards according to the level
        /// </summary>
        /// <returns>cards stack</returns>
        public Stack<Card> GetListCardAccordingToLevel(int level)
        {
            /*
            Card card11 = new Card();
            card11.Level = 1;
            card11.PrestigePt = 1;
            card11.Cout = new int[] { 1, 0, 2, 0, 2 };
            card11.Ress = Ressources.Rubis;
            */
            //Get all the data from card table selecting them according to the data
            string sql = "select * from card where level = " + level;
            SQLiteCommand command = new SQLiteCommand(sql, m_dbConnection);
            SQLiteDataReader reader = command.ExecuteReader();
            //Create an object "Stack of Card"
            Stack<Card> listCard = new Stack<Card>();
            //do while to go to every record of the card table
            while (reader.Read())
            {
                Card card = new Card();

                //Get the ressourceid and the number of prestige points
                switch (reader["fkRessource"])
                {
                    case 1:
                        card.Ress = Ressources.Rubis;
                        break;

                    case 2:
                        card.Ress = Ressources.Emeraude;
                        break;

                    case 3:
                        card.Ress = Ressources.Onyx;
                        break;

                    case 4:
                        card.Ress = Ressources.Saphir;
                        break;

                    case 5:
                        card.Ress = Ressources.Diamand;
                        break;

                    default:

                        break;
                }

                card.PrestigePt = (int)reader["nbPtPrestige"];
                card.Level = (int)reader["level"];
                //Create a card object

                //select the cost of the card : look at the cost table (and other)
                string sql2 = "select * from cost";
                SQLiteCommand command2 = new SQLiteCommand(sql2, m_dbConnection);
                SQLiteDataReader readercost = command.ExecuteReader();

                //initialiser la card.cout
                card.Cout = new int[] { 0, 0, 0, 0, 0 };
                //do while to go to every record of the card table
                while (readercost.Read())
                {
                    if (readercost["fkCard"] == reader["idcard"])
                    {

                        //get the nbRessource of the cost
                        card.Cout[(int)readercost["fkRessource"]-1] = (int)readercost["nbRessource"];
                    }
                }

                //push card into the stack
                listCard.Push(card);
            }
            return listCard;
        }


        /// <summary>
        /// create the "player" table and insert data
        /// </summary>
        private void CreateInsertPlayer()
        {
            string sql = "CREATE TABLE player (id INT PRIMARY KEY, pseudo VARCHAR(20))";
            SQLiteCommand command = new SQLiteCommand(sql, m_dbConnection);
            command.ExecuteNonQuery();

            sql = "insert into player (id, pseudo) values (0, 'Fred')";
            command = new SQLiteCommand(sql, m_dbConnection);
            command.ExecuteNonQuery();
            sql = "insert into player (id, pseudo) values (1, 'Harry')";
            command = new SQLiteCommand(sql, m_dbConnection);
            command.ExecuteNonQuery();
            sql = "insert into player (id, pseudo) values (2, 'Sam')";
            command = new SQLiteCommand(sql, m_dbConnection);
            command.ExecuteNonQuery();
        }

        
        /// <summary>
        /// get the name of the player according to his id
        /// </summary>
        /// <param name="id">id of the player</param>
        /// <returns></returns>
        public string GetPlayerName(int id)
        {
            string sql = "select pseudo from player where id = " + id;
            SQLiteCommand command = new SQLiteCommand(sql, m_dbConnection);
            SQLiteDataReader reader = command.ExecuteReader();
            string name = "";
            while (reader.Read())
            {
                name = reader["pseudo"].ToString();
            }
            return name;
        }

        /// <summary>
        /// create the table "ressources" and insert data
        /// </summary>
        private void CreateInsertRessources()
        {
            string sql = "CREATE TABLE ressources (idRessource INT PRIMARY KEY)";
            SQLiteCommand command = new SQLiteCommand(sql, m_dbConnection);
            command.ExecuteNonQuery();

            string[] sqlRess =
            {
                "insert into ressources(idRessource) values (1)",
                "insert into ressources(idRessource) values (2)",
                "insert into ressources(idRessource) values (3)",
                "insert into ressources(idRessource) values (4)",
                "insert into ressources(idRessource) values (5)"
            };
        

            foreach (string valeurs in sqlRess)
            {
                SQLiteCommand command1 = new SQLiteCommand(valeurs, m_dbConnection);
                command1.ExecuteNonQuery();
            }
        }

        /// <summary>
        ///  create tables "card", "cost" and insert data
        /// </summary>
        private void CreateInsertCardCost()
        {
            string sql1 = "CREATE TABLE card (idcard INT PRIMARY KEY, fkRessource INT , level INT, nbPtPrestige INT, fkPlayer INT, FOREIGN KEY (fkRessource) REFERENCES ressources (idRessource), FOREIGN KEY (fkPlayer) REFERENCES player (id))";
            SQLiteCommand command1 = new SQLiteCommand(sql1, m_dbConnection);
            command1.ExecuteNonQuery();


            string[] sqlCard = {
                "insert into card(idcard, fkRessource, level, nbPtPrestige) values(2, 0, 4, 3)",
                "insert into card(idcard, fkRessource, level, nbPtPrestige) values(3, 0, 4, 3)",
                "insert into card(idcard, fkRessource, level, nbPtPrestige) values(4, 0, 4, 3)",
                "insert into card(idcard, fkRessource, level, nbPtPrestige) values(5, 0, 4, 3)",
                "insert into card(idcard, fkRessource, level, nbPtPrestige) values(6, 0, 4, 3)",
                "insert into card(idcard, fkRessource, level, nbPtPrestige) values(7, 0, 4, 3)",
                "insert into card(idcard, fkRessource, level, nbPtPrestige) values(8, 0, 4, 3)",
                "insert into card(idcard, fkRessource, level, nbPtPrestige) values(9, 0, 4, 3)",
                "insert into card(idcard, fkRessource, level, nbPtPrestige) values(10, 0, 4, 3)",
                "insert into card(idcard, fkRessource, level, nbPtPrestige) values(11, 0, 4, 3)",
                "insert into card(idcard, fkRessource, level, nbPtPrestige) values(12, 4, 3, 5)",
                "insert into card(idcard, fkRessource, level, nbPtPrestige) values(13, 3, 3, 5)",
                "insert into card(idcard, fkRessource, level, nbPtPrestige) values(14, 2, 3, 3)",
                "insert into card(idcard, fkRessource, level, nbPtPrestige) values(15, 5, 3, 3)",
                "insert into card(idcard, fkRessource, level, nbPtPrestige) values(16, 1, 3, 4)",
                "insert into card(idcard, fkRessource, level, nbPtPrestige) values(17, 2, 3, 4)",
                "insert into card(idcard, fkRessource, level, nbPtPrestige) values(18, 5, 3, 4)",
                "insert into card(idcard, fkRessource, level, nbPtPrestige) values(19, 5, 3, 5)",
                "insert into card(idcard, fkRessource, level, nbPtPrestige) values(20, 1, 3, 4)",
                "insert into card(idcard, fkRessource, level, nbPtPrestige) values(21, 4, 3, 4)",
                "insert into card(idcard, fkRessource, level, nbPtPrestige) values(22, 2, 3, 5)",
                "insert into card(idcard, fkRessource, level, nbPtPrestige) values(23, 3, 3, 4)",
                "insert into card(idcard, fkRessource, level, nbPtPrestige) values(24, 1, 3, 3)",
                "insert into card(idcard, fkRessource, level, nbPtPrestige) values(25, 4, 3, 3)",
                "insert into card(idcard, fkRessource, level, nbPtPrestige) values(26, 2, 3, 4)",
                "insert into card(idcard, fkRessource, level, nbPtPrestige) values(27, 3, 3, 4)",
                "insert into card(idcard, fkRessource, level, nbPtPrestige) values(28, 4, 3, 4)",
                "insert into card(idcard, fkRessource, level, nbPtPrestige) values(29, 1, 3, 5)",
                "insert into card(idcard, fkRessource, level, nbPtPrestige) values(30, 5, 3, 4)",
                "insert into card(idcard, fkRessource, level, nbPtPrestige) values(31, 3, 3, 3)",
                "insert into card(idcard, fkRessource, level, nbPtPrestige) values(32, 5, 2, 2)",
                "insert into card(idcard, fkRessource, level, nbPtPrestige) values(33, 1, 2, 1)",
                "insert into card(idcard, fkRessource, level, nbPtPrestige) values(34, 5, 2, 1)",
                "insert into card(idcard, fkRessource, level, nbPtPrestige) values(35, 5, 2, 2)",
                "insert into card(idcard, fkRessource, level, nbPtPrestige) values(36, 5, 2, 1)",
                "insert into card(idcard, fkRessource, level, nbPtPrestige) values(37, 2, 2, 2)",
                "insert into card(idcard, fkRessource, level, nbPtPrestige) values(38, 4, 2, 2)",
                "insert into card(idcard, fkRessource, level, nbPtPrestige) values(39, 4, 2, 1)",
                "insert into card(idcard, fkRessource, level, nbPtPrestige) values(40, 2, 2, 2)",
                "insert into card(idcard, fkRessource, level, nbPtPrestige) values(41, 2, 2, 2)",
                "insert into card(idcard, fkRessource, level, nbPtPrestige) values(42, 3, 2, 2)",
                "insert into card(idcard, fkRessource, level, nbPtPrestige) values(43, 1, 2, 2)",
                "insert into card(idcard, fkRessource, level, nbPtPrestige) values(44, 5, 2, 3)",
                "insert into card(idcard, fkRessource, level, nbPtPrestige) values(45, 4, 2, 3)",
                "insert into card(idcard, fkRessource, level, nbPtPrestige) values(46, 2, 2, 1)",
                "insert into card(idcard, fkRessource, level, nbPtPrestige) values(47, 3, 2, 1)",
                "insert into card(idcard, fkRessource, level, nbPtPrestige) values(48, 1, 2, 3)",
                "insert into card(idcard, fkRessource, level, nbPtPrestige) values(49, 4, 2, 1)",
                "insert into card(idcard, fkRessource, level, nbPtPrestige) values(50, 3, 2, 2)",
                "insert into card(idcard, fkRessource, level, nbPtPrestige) values(51, 2, 2, 1)",
                "insert into card(idcard, fkRessource, level, nbPtPrestige) values(52, 4, 2, 2)",
                "insert into card(idcard, fkRessource, level, nbPtPrestige) values(53, 1, 2, 1)",
                "insert into card(idcard, fkRessource, level, nbPtPrestige) values(54, 1, 2, 2)",
                "insert into card(idcard, fkRessource, level, nbPtPrestige) values(55, 3, 2, 1)",
                "insert into card(idcard, fkRessource, level, nbPtPrestige) values(56, 4, 2, 2)",
                "insert into card(idcard, fkRessource, level, nbPtPrestige) values(57, 3, 2, 2)",
                "insert into card(idcard, fkRessource, level, nbPtPrestige) values(58, 1, 2, 2)",
                "insert into card(idcard, fkRessource, level, nbPtPrestige) values(59, 5, 2, 2)",
                "insert into card(idcard, fkRessource, level, nbPtPrestige) values(60, 2, 2, 3)",
                "insert into card(idcard, fkRessource, level, nbPtPrestige) values(61, 3, 2, 3)",
                "insert into card(idcard, fkRessource, level, nbPtPrestige) values(62, 3, 1, 0)",
                "insert into card(idcard, fkRessource, level, nbPtPrestige) values(63, 2, 1, 0)",
                "insert into card(idcard, fkRessource, level, nbPtPrestige) values(64, 1, 1, 0)",
                "insert into card(idcard, fkRessource, level, nbPtPrestige) values(65, 5, 1, 0)",
                "insert into card(idcard, fkRessource, level, nbPtPrestige) values(66, 4, 1, 0)",
                "insert into card(idcard, fkRessource, level, nbPtPrestige) values(67, 5, 1, 0)",
                "insert into card(idcard, fkRessource, level, nbPtPrestige) values(68, 5, 1, 0)",
                "insert into card(idcard, fkRessource, level, nbPtPrestige) values(69, 5, 1, 0)",
                "insert into card(idcard, fkRessource, level, nbPtPrestige) values(70, 5, 1, 0)",
                "insert into card(idcard, fkRessource, level, nbPtPrestige) values(71, 5, 1, 0)",
                "insert into card(idcard, fkRessource, level, nbPtPrestige) values(72, 5, 1, 0)",
                "insert into card(idcard, fkRessource, level, nbPtPrestige) values(73, 5, 1, 1)",
                "insert into card(idcard, fkRessource, level, nbPtPrestige) values(74, 1, 1, 0)",
                "insert into card(idcard, fkRessource, level, nbPtPrestige) values(75, 1, 1, 0)",
                "insert into card(idcard, fkRessource, level, nbPtPrestige) values(76, 1, 1, 0)",
                "insert into card(idcard, fkRessource, level, nbPtPrestige) values(77, 1, 1, 0)",
                "insert into card(idcard, fkRessource, level, nbPtPrestige) values(78, 1, 1, 0)",
                "insert into card(idcard, fkRessource, level, nbPtPrestige) values(79, 1, 1, 0)",
                "insert into card(idcard, fkRessource, level, nbPtPrestige) values(80, 1, 1, 1)",
                "insert into card(idcard, fkRessource, level, nbPtPrestige) values(81, 3, 1, 0)",
                "insert into card(idcard, fkRessource, level, nbPtPrestige) values(82, 3, 1, 0)",
                "insert into card(idcard, fkRessource, level, nbPtPrestige) values(83, 3, 1, 0)",
                "insert into card(idcard, fkRessource, level, nbPtPrestige) values(84, 3, 1, 0)",
                "insert into card(idcard, fkRessource, level, nbPtPrestige) values(85, 3, 1, 0)",
                "insert into card(idcard, fkRessource, level, nbPtPrestige) values(86, 3, 1, 0)",
                "insert into card(idcard, fkRessource, level, nbPtPrestige) values(87, 3, 1, 1)",
                "insert into card(idcard, fkRessource, level, nbPtPrestige) values(88, 4, 1, 0)",
                "insert into card(idcard, fkRessource, level, nbPtPrestige) values(89, 4, 1, 0)",
                "insert into card(idcard, fkRessource, level, nbPtPrestige) values(90, 4, 1, 0)",
                "insert into card(idcard, fkRessource, level, nbPtPrestige) values(91, 4, 1, 0)",
                "insert into card(idcard, fkRessource, level, nbPtPrestige) values(92, 4, 1, 0)",
                "insert into card(idcard, fkRessource, level, nbPtPrestige) values(93, 4, 1, 0)",
                "insert into card(idcard, fkRessource, level, nbPtPrestige) values(94, 4, 1, 1)",
                "insert into card(idcard, fkRessource, level, nbPtPrestige) values(95, 2, 1, 0)",
                "insert into card(idcard, fkRessource, level, nbPtPrestige) values(96, 2, 1, 0)",
                "insert into card(idcard, fkRessource, level, nbPtPrestige) values(97, 2, 1, 0)",
                "insert into card(idcard, fkRessource, level, nbPtPrestige) values(98, 2, 1, 0)",
                "insert into card(idcard, fkRessource, level, nbPtPrestige) values(99, 2, 1, 0)",
                "insert into card(idcard, fkRessource, level, nbPtPrestige) values(100, 2, 1, 0)",
                "insert into card(idcard, fkRessource, level, nbPtPrestige) values(101, 2, 1, 1)"
             };

            foreach (string valeurs in sqlCard)
            {
                SQLiteCommand command2 = new SQLiteCommand(valeurs, m_dbConnection);
                command2.ExecuteNonQuery();
            }


            string sql2 = "CREATE TABLE cost (idCost INT PRIMARY KEY, fkCard INT, fkRessource INT, nbRessource INT, FOREIGN KEY (fkCard) REFERENCES card (idcard), FOREIGN KEY (fkRessource) REFERENCES ressources (idRessource))";
            SQLiteCommand command3 = new SQLiteCommand(sql2, m_dbConnection);
            command3.ExecuteNonQuery();

            string[] sqlCost =
            {
                "insert into cost(idCost, fkCard, fkRessource, nbRessource) values (3, 3,1,4)",
                "insert into cost(idCost, fkCard, fkRessource, nbRessource) values (6, 6,1,4)",
                "insert into cost(idCost, fkCard, fkRessource, nbRessource) values (7, 7,1,3)",
                "insert into cost(idCost, fkCard, fkRessource, nbRessource) values (9, 9,1,3)",
                "insert into cost(idCost, fkCard, fkRessource, nbRessource) values (11, 11,1,3)",
                "insert into cost(idCost, fkCard, fkRessource, nbRessource) values (13, 13,1,7)",
                "insert into cost(idCost, fkCard, fkRessource, nbRessource) values (14, 14,1,3)",
                "insert into cost(idCost, fkCard, fkRessource, nbRessource) values (15, 15,1,5)",
                "insert into cost(idCost, fkCard, fkRessource, nbRessource) values (16, 16,1,3)",
                "insert into cost(idCost, fkCard, fkRessource, nbRessource) values (23, 23,1,7)",
                "insert into cost(idCost, fkCard, fkRessource, nbRessource) values (25, 25,1,3)",
                "insert into cost(idCost, fkCard, fkRessource, nbRessource) values (27, 27,1,6)",
                "insert into cost(idCost, fkCard, fkRessource, nbRessource) values (29, 29,1,3)",
                "insert into cost(idCost, fkCard, fkRessource, nbRessource) values (30, 30,1,3)",
                "insert into cost(idCost, fkCard, fkRessource, nbRessource) values (31, 31,1,3)",
                "insert into cost(idCost, fkCard, fkRessource, nbRessource) values (32, 32,1,5)",
                "insert into cost(idCost, fkCard, fkRessource, nbRessource) values (33, 33,1,2)",
                "insert into cost(idCost, fkCard, fkRessource, nbRessource) values (34, 34,1,2)",
                "insert into cost(idCost, fkCard, fkRessource, nbRessource) values (35, 35,1,4)",
                "insert into cost(idCost, fkCard, fkRessource, nbRessource) values (36, 36,1,3)",
                "insert into cost(idCost, fkCard, fkRessource, nbRessource) values (38, 38,1,1)",
                "insert into cost(idCost, fkCard, fkRessource, nbRessource) values (39, 39,1,3)",
                "insert into cost(idCost, fkCard, fkRessource, nbRessource) values (42, 42,1,3)",
                "insert into cost(idCost, fkCard, fkRessource, nbRessource) values (48, 48,1,6)",
                "insert into cost(idCost, fkCard, fkRessource, nbRessource) values (51, 51,1,3)",
                "insert into cost(idCost, fkCard, fkRessource, nbRessource) values (53, 53,1,2)",
                "insert into cost(idCost, fkCard, fkRessource, nbRessource) values (57, 57,1,2)",
                "insert into cost(idCost, fkCard, fkRessource, nbRessource) values (59, 59,1,5)",
                "insert into cost(idCost, fkCard, fkRessource, nbRessource) values (62, 62,1,3)",
                "insert into cost(idCost, fkCard, fkRessource, nbRessource) values (63, 63,1,3)",
                "insert into cost(idCost, fkCard, fkRessource, nbRessource) values (64, 64,1,1)",
                "insert into cost(idCost, fkCard, fkRessource, nbRessource) values (66, 66,1,2)",
                "insert into cost(idCost, fkCard, fkRessource, nbRessource) values (67, 67,1,2)",
                "insert into cost(idCost, fkCard, fkRessource, nbRessource) values (70, 70,1,1)",
                "insert into cost(idCost, fkCard, fkRessource, nbRessource) values (72, 72,1,1)",
                "insert into cost(idCost, fkCard, fkRessource, nbRessource) values (76, 76,1,2)",
                "insert into cost(idCost, fkCard, fkRessource, nbRessource) values (81, 81,1,1)",
                "insert into cost(idCost, fkCard, fkRessource, nbRessource) values (84, 84,1,1)",
                "insert into cost(idCost, fkCard, fkRessource, nbRessource) values (85, 85,1,1)",
                "insert into cost(idCost, fkCard, fkRessource, nbRessource) values (86, 86,1,1)",
                "insert into cost(idCost, fkCard, fkRessource, nbRessource) values (88, 88,1,1)",
                "insert into cost(idCost, fkCard, fkRessource, nbRessource) values (91, 91,1,2)",
                "insert into cost(idCost, fkCard, fkRessource, nbRessource) values (93, 93,1,1)",
                "insert into cost(idCost, fkCard, fkRessource, nbRessource) values (94, 94,1,4)",
                "insert into cost(idCost, fkCard, fkRessource, nbRessource) values (96, 96,1,2)",
                "insert into cost(idCost, fkCard, fkRessource, nbRessource) values (97, 97,1,1)",
                "insert into cost(idCost, fkCard, fkRessource, nbRessource) values (98, 98,1,2)",
                "insert into cost(idCost, fkCard, fkRessource, nbRessource) values (100, 100,1,1)",
                "insert into cost(idCost, fkCard, fkRessource, nbRessource) values (112, 2,2,4)",
                "insert into cost(idCost, fkCard, fkRessource, nbRessource) values (113, 3,2,4)",
                "insert into cost(idCost, fkCard, fkRessource, nbRessource) values (118, 8,2,3)",
                "insert into cost(idCost, fkCard, fkRessource, nbRessource) values (119, 9,2,3)",
                "insert into cost(idCost, fkCard, fkRessource, nbRessource) values (121, 11,2,3)",
                "insert into cost(idCost, fkCard, fkRessource, nbRessource) values (125, 15,2,3)",
                "insert into cost(idCost, fkCard, fkRessource, nbRessource) values (126, 16,2,6)",
                "insert into cost(idCost, fkCard, fkRessource, nbRessource) values (127, 17,2,3)",
                "insert into cost(idCost, fkCard, fkRessource, nbRessource) values (130, 20,2,7)",
                "insert into cost(idCost, fkCard, fkRessource, nbRessource) values (132, 22,2,3)",
                "insert into cost(idCost, fkCard, fkRessource, nbRessource) values (134, 24,2,3)",
                "insert into cost(idCost, fkCard, fkRessource, nbRessource) values (135, 25,2,3)",
                "insert into cost(idCost, fkCard, fkRessource, nbRessource) values (137, 27,2,3)",
                "insert into cost(idCost, fkCard, fkRessource, nbRessource) values (139, 29,2,7)",
                "insert into cost(idCost, fkCard, fkRessource, nbRessource) values (141, 31,2,5)",
                "insert into cost(idCost, fkCard, fkRessource, nbRessource) values (144, 34,2,3)",
                "insert into cost(idCost, fkCard, fkRessource, nbRessource) values (145, 35,2,1)",
                "insert into cost(idCost, fkCard, fkRessource, nbRessource) values (147, 37,2,3)",
                "insert into cost(idCost, fkCard, fkRessource, nbRessource) values (149, 39,2,2)",
                "insert into cost(idCost, fkCard, fkRessource, nbRessource) values (151, 41,2,5)",
                "insert into cost(idCost, fkCard, fkRessource, nbRessource) values (152, 42,2,5)",
                "insert into cost(idCost, fkCard, fkRessource, nbRessource) values (157, 47,2,3)",
                "insert into cost(idCost, fkCard, fkRessource, nbRessource) values (159, 49,2,3)",
                "insert into cost(idCost, fkCard, fkRessource, nbRessource) values (161, 51,2,2)",
                "insert into cost(idCost, fkCard, fkRessource, nbRessource) values (165, 55,2,2)",
                "insert into cost(idCost, fkCard, fkRessource, nbRessource) values (167, 57,2,4)",
                "insert into cost(idCost, fkCard, fkRessource, nbRessource) values (168, 58,2,2)",
                "insert into cost(idCost, fkCard, fkRessource, nbRessource) values (170, 60,2,6)",
                "insert into cost(idCost, fkCard, fkRessource, nbRessource) values (172, 62,2,1)",
                "insert into cost(idCost, fkCard, fkRessource, nbRessource) values (176, 66,2,1)",
                "insert into cost(idCost, fkCard, fkRessource, nbRessource) values (180, 70,2,2)",
                "insert into cost(idCost, fkCard, fkRessource, nbRessource) values (181, 71,2,2)",
                "insert into cost(idCost, fkCard, fkRessource, nbRessource) values (182, 72,2,1)",
                "insert into cost(idCost, fkCard, fkRessource, nbRessource) values (183, 73,2,4)",
                "insert into cost(idCost, fkCard, fkRessource, nbRessource) values (184, 74,2,1)",
                "insert into cost(idCost, fkCard, fkRessource, nbRessource) values (187, 77,2,1)",
                "insert into cost(idCost, fkCard, fkRessource, nbRessource) values (188, 78,2,1)",
                "insert into cost(idCost, fkCard, fkRessource, nbRessource) values (189, 79,2,1)",
                "insert into cost(idCost, fkCard, fkRessource, nbRessource) values (192, 82,2,2)",
                "insert into cost(idCost, fkCard, fkRessource, nbRessource) values (193, 83,2,3)",
                "insert into cost(idCost, fkCard, fkRessource, nbRessource) values (194, 84,2,2)",
                "insert into cost(idCost, fkCard, fkRessource, nbRessource) values (195, 85,2,1)",
                "insert into cost(idCost, fkCard, fkRessource, nbRessource) values (196, 86,2,1)",
                "insert into cost(idCost, fkCard, fkRessource, nbRessource) values (198, 88,2,1)",
                "insert into cost(idCost, fkCard, fkRessource, nbRessource) values (201, 91,2,2)",
                "insert into cost(idCost, fkCard, fkRessource, nbRessource) values (202, 92,2,2)",
                "insert into cost(idCost, fkCard, fkRessource, nbRessource) values (203, 93,2,3)",
                "insert into cost(idCost, fkCard, fkRessource, nbRessource) values (205, 95,2,1)",
                "insert into cost(idCost, fkCard, fkRessource, nbRessource) values (224, 4,3,3)",
                "insert into cost(idCost, fkCard, fkRessource, nbRessource) values (225, 5,3,4)",
                "insert into cost(idCost, fkCard, fkRessource, nbRessource) values (226, 6,3,4)",
                "insert into cost(idCost, fkCard, fkRessource, nbRessource) values (227, 7,3,3)",
                "insert into cost(idCost, fkCard, fkRessource, nbRessource) values (231, 11,3,3)",
                "insert into cost(idCost, fkCard, fkRessource, nbRessource) values (233, 13,3,3)",
                "insert into cost(idCost, fkCard, fkRessource, nbRessource) values (234, 14,3,3)",
                "insert into cost(idCost, fkCard, fkRessource, nbRessource) values (235, 15,3,3)",
                "insert into cost(idCost, fkCard, fkRessource, nbRessource) values (238, 18,3,7)",
                "insert into cost(idCost, fkCard, fkRessource, nbRessource) values (239, 19,3,7)",
                "insert into cost(idCost, fkCard, fkRessource, nbRessource) values (241, 21,3,3)",
                "insert into cost(idCost, fkCard, fkRessource, nbRessource) values (244, 24,3,3)",
                "insert into cost(idCost, fkCard, fkRessource, nbRessource) values (245, 25,3,5)",
                "insert into cost(idCost, fkCard, fkRessource, nbRessource) values (247, 27,3,3)",
                "insert into cost(idCost, fkCard, fkRessource, nbRessource) values (250, 30,3,6)",
                "insert into cost(idCost, fkCard, fkRessource, nbRessource) values (253, 33,3,3)",
                "insert into cost(idCost, fkCard, fkRessource, nbRessource) values (254, 34,3,2)",
                "insert into cost(idCost, fkCard, fkRessource, nbRessource) values (255, 35,3,2)",
                "insert into cost(idCost, fkCard, fkRessource, nbRessource) values (258, 38,3,4)",
                "insert into cost(idCost, fkCard, fkRessource, nbRessource) values (260, 40,3,1)",
                "insert into cost(idCost, fkCard, fkRessource, nbRessource) values (263, 43,3,5)",
                "insert into cost(idCost, fkCard, fkRessource, nbRessource) values (266, 46,3,2)",
                "insert into cost(idCost, fkCard, fkRessource, nbRessource) values (267, 47,3,2)",
                "insert into cost(idCost, fkCard, fkRessource, nbRessource) values (269, 49,3,3)",
                "insert into cost(idCost, fkCard, fkRessource, nbRessource) values (273, 53,3,3)",
                "insert into cost(idCost, fkCard, fkRessource, nbRessource) values (274, 54,3,5)",
                "insert into cost(idCost, fkCard, fkRessource, nbRessource) values (279, 59,3,3)",
                "insert into cost(idCost, fkCard, fkRessource, nbRessource) values (281, 61,3,6)",
                "insert into cost(idCost, fkCard, fkRessource, nbRessource) values (282, 62,3,1)",
                "insert into cost(idCost, fkCard, fkRessource, nbRessource) values (284, 64,3,3)",
                "insert into cost(idCost, fkCard, fkRessource, nbRessource) values (285, 65,3,1)",
                "insert into cost(idCost, fkCard, fkRessource, nbRessource) values (286, 66,3,1)",
                "insert into cost(idCost, fkCard, fkRessource, nbRessource) values (287, 67,3,1)",
                "insert into cost(idCost, fkCard, fkRessource, nbRessource) values (288, 68,3,2)",
                "insert into cost(idCost, fkCard, fkRessource, nbRessource) values (290, 70,3,1)",
                "insert into cost(idCost, fkCard, fkRessource, nbRessource) values (291, 71,3,1)",
                "insert into cost(idCost, fkCard, fkRessource, nbRessource) values (292, 72,3,1)",
                "insert into cost(idCost, fkCard, fkRessource, nbRessource) values (294, 74,3,1)",
                "insert into cost(idCost, fkCard, fkRessource, nbRessource) values (298, 78,3,2)",
                "insert into cost(idCost, fkCard, fkRessource, nbRessource) values (299, 79,3,1)",
                "insert into cost(idCost, fkCard, fkRessource, nbRessource) values (308, 88,3,1)",
                "insert into cost(idCost, fkCard, fkRessource, nbRessource) values (309, 89,3,2)",
                "insert into cost(idCost, fkCard, fkRessource, nbRessource) values (310, 90,3,3)",
                "insert into cost(idCost, fkCard, fkRessource, nbRessource) values (312, 92,3,2)",
                "insert into cost(idCost, fkCard, fkRessource, nbRessource) values (316, 96,3,2)",
                "insert into cost(idCost, fkCard, fkRessource, nbRessource) values (317, 97,3,1)",
                "insert into cost(idCost, fkCard, fkRessource, nbRessource) values (320, 100,3,2)",
                "insert into cost(idCost, fkCard, fkRessource, nbRessource) values (332, 2,4,4)",
                "insert into cost(idCost, fkCard, fkRessource, nbRessource) values (334, 4,4,3)",
                "insert into cost(idCost, fkCard, fkRessource, nbRessource) values (338, 8,4,3)",
                "insert into cost(idCost, fkCard, fkRessource, nbRessource) values (339, 9,4,3)",
                "insert into cost(idCost, fkCard, fkRessource, nbRessource) values (340, 10,4,4)",
                "insert into cost(idCost, fkCard, fkRessource, nbRessource) values (342, 12,4,3)",
                "insert into cost(idCost, fkCard, fkRessource, nbRessource) values (344, 14,4,3)",
                "insert into cost(idCost, fkCard, fkRessource, nbRessource) values (345, 15,4,3)",
                "insert into cost(idCost, fkCard, fkRessource, nbRessource) values (346, 16,4,3)",
                "insert into cost(idCost, fkCard, fkRessource, nbRessource) values (347, 17,4,6)",
                "insert into cost(idCost, fkCard, fkRessource, nbRessource) values (351, 21,4,3)",
                "insert into cost(idCost, fkCard, fkRessource, nbRessource) values (352, 22,4,7)",
                "insert into cost(idCost, fkCard, fkRessource, nbRessource) values (354, 24,4,5)",
                "insert into cost(idCost, fkCard, fkRessource, nbRessource) values (356, 26,4,7)",
                "insert into cost(idCost, fkCard, fkRessource, nbRessource) values (361, 31,4,3)",
                "insert into cost(idCost, fkCard, fkRessource, nbRessource) values (363, 33,4,3)",
                "insert into cost(idCost, fkCard, fkRessource, nbRessource) values (366, 36,4,3)",
                "insert into cost(idCost, fkCard, fkRessource, nbRessource) values (367, 37,4,5)",
                "insert into cost(idCost, fkCard, fkRessource, nbRessource) values (369, 39,4,2)",
                "insert into cost(idCost, fkCard, fkRessource, nbRessource) values (370, 40,4,2)",
                "insert into cost(idCost, fkCard, fkRessource, nbRessource) values (375, 45,4,6)",
                "insert into cost(idCost, fkCard, fkRessource, nbRessource) values (376, 46,4,3)",
                "insert into cost(idCost, fkCard, fkRessource, nbRessource) values (379, 49,4,2)",
                "insert into cost(idCost, fkCard, fkRessource, nbRessource) values (382, 52,4,5)",
                "insert into cost(idCost, fkCard, fkRessource, nbRessource) values (385, 55,4,2)",
                "insert into cost(idCost, fkCard, fkRessource, nbRessource) values (386, 56,4,3)",
                "insert into cost(idCost, fkCard, fkRessource, nbRessource) values (387, 57,4,1)",
                "insert into cost(idCost, fkCard, fkRessource, nbRessource) values (388, 58,4,4)",
                "insert into cost(idCost, fkCard, fkRessource, nbRessource) values (395, 65,4,1)",
                "insert into cost(idCost, fkCard, fkRessource, nbRessource) values (398, 68,4,2)",
                "insert into cost(idCost, fkCard, fkRessource, nbRessource) values (399, 69,4,3)",
                "insert into cost(idCost, fkCard, fkRessource, nbRessource) values (400, 70,4,1)",
                "insert into cost(idCost, fkCard, fkRessource, nbRessource) values (401, 71,4,2)",
                "insert into cost(idCost, fkCard, fkRessource, nbRessource) values (402, 72,4,1)",
                "insert into cost(idCost, fkCard, fkRessource, nbRessource) values (404, 74,4,1)",
                "insert into cost(idCost, fkCard, fkRessource, nbRessource) values (407, 77,4,2)",
                "insert into cost(idCost, fkCard, fkRessource, nbRessource) values (409, 79,4,1)",
                "insert into cost(idCost, fkCard, fkRessource, nbRessource) values (411, 81,4,2)",
                "insert into cost(idCost, fkCard, fkRessource, nbRessource) values (415, 85,4,1)",
                "insert into cost(idCost, fkCard, fkRessource, nbRessource) values (416, 86,4,2)",
                "insert into cost(idCost, fkCard, fkRessource, nbRessource) values (417, 87,4,4)",
                "insert into cost(idCost, fkCard, fkRessource, nbRessource) values (423, 93,4,1)",
                "insert into cost(idCost, fkCard, fkRessource, nbRessource) values (425, 95,4,3)",
                "insert into cost(idCost, fkCard, fkRessource, nbRessource) values (426, 96,4,1)",
                "insert into cost(idCost, fkCard, fkRessource, nbRessource) values (427, 97,4,1)",
                "insert into cost(idCost, fkCard, fkRessource, nbRessource) values (428, 98,4,2)",
                "insert into cost(idCost, fkCard, fkRessource, nbRessource) values (429, 99,4,1)",
                "insert into cost(idCost, fkCard, fkRessource, nbRessource) values (430, 100,4,1)",
                "insert into cost(idCost, fkCard, fkRessource, nbRessource) values (431, 101,4,4)",
                "insert into cost(idCost, fkCard, fkRessource, nbRessource) values (444, 4,5,3)",
                "insert into cost(idCost, fkCard, fkRessource, nbRessource) values (445, 5,5,4)",
                "insert into cost(idCost, fkCard, fkRessource, nbRessource) values (447, 7,5,3)",
                "insert into cost(idCost, fkCard, fkRessource, nbRessource) values (448, 8,5,3)",
                "insert into cost(idCost, fkCard, fkRessource, nbRessource) values (450, 10,5,4)",
                "insert into cost(idCost, fkCard, fkRessource, nbRessource) values (452, 12,5,7)",
                "insert into cost(idCost, fkCard, fkRessource, nbRessource) values (454, 14,5,5)",
                "insert into cost(idCost, fkCard, fkRessource, nbRessource) values (457, 17,5,3)",
                "insert into cost(idCost, fkCard, fkRessource, nbRessource) values (459, 19,5,3)",
                "insert into cost(idCost, fkCard, fkRessource, nbRessource) values (461, 21,5,6)",
                "insert into cost(idCost, fkCard, fkRessource, nbRessource) values (464, 24,5,3)",
                "insert into cost(idCost, fkCard, fkRessource, nbRessource) values (465, 25,5,3)",
                "insert into cost(idCost, fkCard, fkRessource, nbRessource) values (468, 28,5,7)",
                "insert into cost(idCost, fkCard, fkRessource, nbRessource) values (470, 30,5,3)",
                "insert into cost(idCost, fkCard, fkRessource, nbRessource) values (471, 31,5,3)",
                "insert into cost(idCost, fkCard, fkRessource, nbRessource) values (476, 36,5,2)",
                "insert into cost(idCost, fkCard, fkRessource, nbRessource) values (478, 38,5,2)",
                "insert into cost(idCost, fkCard, fkRessource, nbRessource) values (480, 40,5,4)",
                "insert into cost(idCost, fkCard, fkRessource, nbRessource) values (483, 43,5,3)",
                "insert into cost(idCost, fkCard, fkRessource, nbRessource) values (484, 44,5,6)",
                "insert into cost(idCost, fkCard, fkRessource, nbRessource) values (486, 46,5,2)",
                "insert into cost(idCost, fkCard, fkRessource, nbRessource) values (487, 47,5,3)",
                "insert into cost(idCost, fkCard, fkRessource, nbRessource) values (490, 50,5,5)",
                "insert into cost(idCost, fkCard, fkRessource, nbRessource) values (491, 51,5,3)",
                "insert into cost(idCost, fkCard, fkRessource, nbRessource) values (493, 53,5,2)",
                "insert into cost(idCost, fkCard, fkRessource, nbRessource) values (495, 55,5,3)",
                "insert into cost(idCost, fkCard, fkRessource, nbRessource) values (496, 56,5,5)",
                "insert into cost(idCost, fkCard, fkRessource, nbRessource) values (498, 58,5,1)",
                "insert into cost(idCost, fkCard, fkRessource, nbRessource) values (504, 64,5,1)",
                "insert into cost(idCost, fkCard, fkRessource, nbRessource) values (505, 65,5,3)",
                "insert into cost(idCost, fkCard, fkRessource, nbRessource) values (506, 66,5,1)",
                "insert into cost(idCost, fkCard, fkRessource, nbRessource) values (514, 74,5,2)",
                "insert into cost(idCost, fkCard, fkRessource, nbRessource) values (515, 75,5,3)",
                "insert into cost(idCost, fkCard, fkRessource, nbRessource) values (516, 76,5,2)",
                "insert into cost(idCost, fkCard, fkRessource, nbRessource) values (518, 78,5,2)",
                "insert into cost(idCost, fkCard, fkRessource, nbRessource) values (519, 79,5,1)",
                "insert into cost(idCost, fkCard, fkRessource, nbRessource) values (520, 80,5,4)",
                "insert into cost(idCost, fkCard, fkRessource, nbRessource) values (521, 81,5,2)",
                "insert into cost(idCost, fkCard, fkRessource, nbRessource) values (522, 82,5,2)",
                "insert into cost(idCost, fkCard, fkRessource, nbRessource) values (525, 85,5,1)",
                "insert into cost(idCost, fkCard, fkRessource, nbRessource) values (526, 86,5,1)",
                "insert into cost(idCost, fkCard, fkRessource, nbRessource) values (528, 88,5,1)",
                "insert into cost(idCost, fkCard, fkRessource, nbRessource) values (529, 89,5,1)",
                "insert into cost(idCost, fkCard, fkRessource, nbRessource) values (531, 91,5,1)",
                "insert into cost(idCost, fkCard, fkRessource, nbRessource) values (535, 95,5,1)",
                "insert into cost(idCost, fkCard, fkRessource, nbRessource) values (537, 97,5,1)",
                "insert into cost(idCost, fkCard, fkRessource, nbRessource) values (539, 99,5,2)",
                "insert into cost(idCost, fkCard, fkRessource, nbRessource) values (540, 100,5,1)"
            };

            foreach (string valeurs in sqlCost)
            {
                SQLiteCommand command4 = new SQLiteCommand(valeurs, m_dbConnection);
                command4.ExecuteNonQuery();
            }

        }

    }
}
