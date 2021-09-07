/* Programme qui permet de contenir les informations de différentes ligues professionnelles en Amérique du nord.
 * Permet, à l'aide d'un menu, de réaliser et faciliter les transactions automatisées des principales opérations.
 *
 * Ligue de sports
     * Gestion des joueurs
     * Gestion des équipes
     * Gestion des parties
     * Statistiques
 *
 * Auteur: Dany Gagnon
 * Création: 2020-12-09
 */

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using static Tp2Remake.Utilitaires;

namespace Tp2Remake
{
    class Program
    {
        const string NomFichier = "Ligue_Sports.données";
        const int MaximumJoueurs = 4;
        const int Sentinelle = 0;

        #region Options du menu

        static readonly string[] OptionsMenuPrincipale = {
            "Gestion des joueurs",
            "Gestion des équipes",
            "Gestion des parties",
            "Statistiques"
        };

        static readonly string[] OptionsGestionJoueurs = {
            "Ajouter un nouveau joueur",
            "Afficher la liste des joueurs en ordre alphabétique",
            "Modifier un joueur"
        };

        static readonly string[] OptionsModificationJoueurs =
        {
            "Nom du joueurs",
            "Date de naissance",
            "Pays de naissance"
        };
        
        static readonly string[] OptionsGestionEquipes = {
            "Ajouter une équipe",
            "Afficher la liste des équipes en ordre alphabétique",
            "Modifier une équipe"
        };
        
        static readonly string[] OptionsGestionParties = {
            "Ajouter une partie",
            "Modifier le résultat d'une partie",
            "Effacer le résultat d'une partie"
        };

        static readonly string[] OptionsGestionEquipePeutModifier =
        {
            "Ville",
            "Sport"
        };

        #endregion

        #region Donnée Joueurs

        static string[] _noms = new string[MaximumJoueurs];
        static DateTime[] _datesNaissances = new DateTime[MaximumJoueurs];
        static string[] _paysNaissances = new string[MaximumJoueurs];
        

        #endregion

        #region Donnée Équipes
        
        static List<List<string>> _gestionEquipes = new List<List<string>>();

        static readonly string[] OptionsEquipes =
        {
            "Identifiant",
            "Nom",
            "Ville",
            "Sport"
        };

        #endregion

        #region Donnée Parties

        static List<string[]> _gestionParties = new List<string[]>();

        #endregion

        static readonly string[] OptionsStatistique =
        {
            "Lister par sport, les équipes selon leur nombre de points",
            "Lister tous les joueurs et afficher leur âge en date d'aujourd'hui (avec moyenne)",
            "Afficher la liste des parties jouée"
        };

        static void Main(string[] args)
        {
            Console.BackgroundColor = ConsoleColor.White;
            Console.ForegroundColor = ConsoleColor.Black;
            Console.Clear();

            DeserialiserFichier();

            MenuPrincipale();
            
            SerialiserFichier();
        }

        #region Menu Principal

        static void MenuPrincipale()
        {
            CreerMenu("Menu Principal", OptionsMenuPrincipale);
            
            int choixMenuPrincipal = DemanderChoix(OptionsMenuPrincipale.Length);

            switch (choixMenuPrincipal)
            {
                case 1:
                    Console.Clear();
                    MenuGestionJoueurs();
                    break;
                case 2:
                    Console.Clear();
                    MenuGestionEquipes();
                    break;
                case 3:
                    Console.Clear();
                    MenuGestionParties();
                    break;
                case 4:
                    Console.Clear();
                    MenuStatistiques();
                    break;
            }
        }

        #endregion

        #region Gestion Joueurs
        
        static void MenuGestionJoueurs()
        {
            CreerMenu(OptionsMenuPrincipale[0], OptionsGestionJoueurs);
            
            int choixGestionJoueurs = DemanderChoix(OptionsGestionJoueurs.Length);

            switch (choixGestionJoueurs)
            {
                case 1:
                    Console.Clear();
                    MenuNouveauJoueur();
                    MenuGestionJoueurs();
                    break;
                case 2:
                    Console.Clear();
                    MenuAfficherJoueurs();
                    MenuGestionJoueurs();
                    break;
                case 3:
                    Console.Clear();
                    MenuModifierJoueur();
                    break;
                default:
                    Console.Clear();
                    MenuPrincipale();
                    break;
            }
        }

        #region Menu Ajouter Joueur

        static void MenuNouveauJoueur()
        {
            if (!EstAjoutableVecteur(_noms, out int index)) return;

            Console.WriteLine($"Ligue de sports");
            Console.WriteLine($"\t*** {OptionsGestionJoueurs[0].ToUpper()} ***");
            
            string nomNouveauJoueur = ModifierNomJoueur(index, out bool estUnique, tab: true);

            if (!estUnique)
                return;


            ModifierDateNaissanceJoueur(index, tab: true);
            ModifierPaysNaissanceJoueur(index, tab: true);

            Console.Clear();
            WriteLineSucess($"*** {nomNouveauJoueur} à bien été ajouté à la liste.");
        }


        #endregion

        #region Menu Afficher Joueur

        static void MenuAfficherJoueurs()
        {
            string[] nomsAlphabetique = OrdonnerVecteurAlphabetique(_noms);

            Console.Clear();
            CreerMenu(OptionsGestionJoueurs[1], nomsAlphabetique, afficherIndex: false);

            DemanderChoix(0);
            Console.Clear();
        }

        #endregion

        #region Menu Modifier Joueur

        static void MenuModifierJoueur()
        {
            CreerMenu(OptionsGestionJoueurs[2], _noms);
            int numModifier = DemanderChoix(_noms.Length);
            
            if (numModifier == 0)
            {
                Console.Clear();
                MenuGestionJoueurs();
                return;
            }
            
            int index = numModifier - 1;
            Console.Clear();
            MenuOptionsModificationJoueur(index);
        }
        
        static void MenuOptionsModificationJoueur(int index)
        {
            CreerMenu($"Option de modification pour {_noms[index]}", OptionsModificationJoueurs);
            int choix = DemanderChoix(OptionsModificationJoueurs.Length);

            switch (choix)
            {
                case 1:
                    string nomNouveauJoueur = ModifierNomJoueur(index, out bool estUnique);
                    Console.Clear();
                    if (!estUnique)
                        EstUniqueVecteur(_noms, nomNouveauJoueur);
                    else
                        WriteLineSucess($"*** Le nom à bien été modifié.");
                    MenuOptionsModificationJoueur(index);
                    break;
                case 2:
                    ModifierDateNaissanceJoueur(index);
                    Console.Clear();
                    WriteLineSucess($"*** La date de naissance à bien été modifié.");
                    MenuOptionsModificationJoueur(index);
                    break;
                case 3:
                    ModifierPaysNaissanceJoueur(index);
                    Console.Clear();
                    WriteLineSucess($"*** Le pays de naissance à bien été modifié.");
                    MenuOptionsModificationJoueur(index);
                    break;
                case 0:
                    Console.Clear();
                    MenuModifierJoueur();
                    break;
            }
        }

        #endregion

        #region Modification

        static string ModifierNomJoueur(int index, out bool estUnique, bool tab = false)
        {
            string nomNouveauJoueur = LireStringTailleControlee($"{(tab ? "\t" : "")}{OptionsModificationJoueurs[0]}: ", 1, 30);
            estUnique = EstUniqueVecteur(_noms, nomNouveauJoueur);
            if (estUnique)
                _noms[index] = nomNouveauJoueur;
            return nomNouveauJoueur;
        }
        static void ModifierDateNaissanceJoueur(int index, bool tab = false)
        {
            DateTime dateNaissance = LireDate($"{(tab ? "\t" : "")}{OptionsModificationJoueurs[1]}: ");
            _datesNaissances[index] = dateNaissance;
        }
        static void ModifierPaysNaissanceJoueur(int index, bool tab = false)
        {
            string paysNaissance = LireString($"{(tab ? "\t" : "")}{OptionsModificationJoueurs[2]}: ");
            _paysNaissances[index] = paysNaissance;
        }

        #endregion

        #endregion

        #region Gestion Équipes

        static void MenuGestionEquipes()
        {
            CreerMenu(OptionsMenuPrincipale[1], OptionsGestionEquipes);
            
            int choixGestionJoueurs = DemanderChoix(OptionsGestionEquipes.Length);
            
            switch (choixGestionJoueurs)
            {
                case 1:
                    Console.Clear();
                    MenuAjouterEquipe();
                    break;
                case 2:
                    Console.Clear();
                    MenuAfficherEquipes();
                    break;
                case 3:
                    Console.Clear();
                    MenuModifierEquipe();
                    break;
                default:
                    Console.Clear();
                    MenuPrincipale();
                    break;
            }
        }

        #region Ajouter une équipe

        static void MenuAjouterEquipe()
        {
            Console.WriteLine($"Ligue de sports");
            Console.WriteLine($"\t*** {OptionsGestionEquipes[0].ToUpper()} ***");
            List<string> nouvelleEquipe = new List<string>();

            nouvelleEquipe.Add(LireChiffreContientVecteur("\tIdentifiant: ", 0));
            nouvelleEquipe.Add(LireStringContientVecteur("\tNom: ", 1));
            nouvelleEquipe.Add(LireString("\tVille: "));
            nouvelleEquipe.Add(LireString("\tSport: "));
            _gestionEquipes.Add(nouvelleEquipe);
            Console.Clear();
            WriteLineSucess($"*** {nouvelleEquipe[1]} à bien été ajouté.");
            MenuGestionEquipes();
        }
        
        static string LireChiffreContientVecteur(string p_question, int index)
        {
            string nom;
            for (;;)
            {
                nom = LireChiffreString($"{p_question}", 4);
                bool contains = false;
                foreach (List<string> t in _gestionEquipes.Where(t => !contains))
                    contains = t[index].Contains(nom);
                if (!contains) break;
                WriteLineError($"\t*** {nom} existe deja dans la liste d'équipes.");
            }

            return nom;
        }

        static string LireStringContientVecteur(string p_question, int index)
        {
            string nom;
            for (;;)
            {
                nom = LireStringTailleControlee($"{p_question}", 1, 30);
                bool contains = false;
                foreach (List<string> t in _gestionEquipes.Where(t => !contains))
                    contains = t[index].ToLower().Contains(nom.ToLower());
                if (!contains) break;
                WriteLineError($"\t*** {nom} existe deja dans la liste d'équipes.");
            }

            return nom;
        }

        #endregion

        #region Afficher les équipes

        static void MenuAfficherEquipes()
        {
            Console.WriteLine("Ligue de sports");
            List<List<string>> test = _gestionEquipes.OrderBy(x => x[1]).ToList();
            for (int index = 0; index < test.Count; index++)
            {
                List<string> list = test[index];
                if(index != 0)
                    Console.WriteLine();
                for (int i = 0; i < list.Count; i++)
                    Console.WriteLine($"\t{OptionsEquipes[i]}: {list[i]}");
            }
            Console.WriteLine($"\n\t{Sentinelle}. Annuler l'opération");
            
            DemanderChoix(0);
            Console.Clear();
            MenuGestionEquipes();
        }

        #endregion

        #region Modifier les équipes

        static void MenuModifierEquipe()
        {
            List<string> nomsEquipes = _gestionEquipes.Select(equipe => equipe[1]).ToList();
            CreerMenu(OptionsGestionEquipes[2], nomsEquipes.ToArray());
            int choix = DemanderChoix(nomsEquipes.ToArray().Length);
            
            if (choix == 0)
            {
                Console.Clear();
                MenuGestionEquipes();
            }
            else
            {
                int index = choix - 1;

                Console.Clear();
                MenuModificationEquipe(index);
            }
            
            

        }

        static void MenuModificationEquipe(int index)
        {
            CreerMenu($"Modifier {_gestionEquipes[index][1]}", OptionsGestionEquipePeutModifier);
            int numAModifier = DemanderChoix(OptionsGestionEquipePeutModifier.Length);

            switch (numAModifier)
            {
                case 1:
                    string ville = LireString("Ville: ");
                    Console.Clear();
                    if (_gestionEquipes[index][2] != ville)
                    {
                        _gestionEquipes[index][2] = ville;
                        WriteLineSucess("*** La ville à bien été modifié.");
                    }
                    else
                        WriteLineError("*** La ville n'a pas été modifier, car c'est la même.");
                    
                    MenuModificationEquipe(index);
                    break;
                case 2:
                    string sport = LireString("Sport: ");
                    Console.Clear();
                    if (_gestionEquipes[index][3] != sport)
                    {
                        _gestionEquipes[index][3] = sport;
                        WriteLineSucess($"*** Le sport à bien été modifié. {sport}");
                    }
                    else
                        WriteLineError($"*** Le sport n'a pas été modifier, car c'est la même. {sport}");
                    MenuModificationEquipe(index);
                    break;
                default:
                    Console.Clear();
                    MenuModifierEquipe();
                    break;
            }
        }

        #endregion

        #endregion

        #region Gestions Parties

        static void MenuGestionParties()
        {
            CreerMenu("Gestion des parties", OptionsGestionParties);

            int choix = DemanderChoix(OptionsGestionParties.Length);
            switch (choix)
            {
                case 1:
                    Console.Clear();
                    MenuPremierePartie();
                    break;
                case 2:
                    Console.Clear();
                    MenuModifierPartie();
                    break;
                case 3:
                    Console.Clear();
                    WriteLineError("*** Il n'est pas possible de supprimer le résultat d'une partie pour le moment.");
                    MenuGestionParties();
                    break;
                default:
                    Console.Clear();
                    MenuPrincipale();
                    break;
            }
        }

        static void MenuModifierPartie()
        {
            List<string> affichageList = new List<string>();
            
            foreach (string[] gestionParty in _gestionParties)
            {
                string premiereEquipe = TrouverNom(RechercheIndexParIdentifiant(gestionParty[0]));
                string deuxiemeEquipe = TrouverNom(RechercheIndexParIdentifiant(gestionParty[2]));
                affichageList.Add($"{premiereEquipe} vs {deuxiemeEquipe}: {gestionParty[1]}-{gestionParty[3]}");
            }
            CreerMenu(OptionsGestionParties[1], affichageList.ToArray());

            int choix = DemanderChoix(affichageList.ToArray().Length);
            int index = choix - 1;
            if (choix == 0)
            {
                Console.Clear();
                MenuGestionParties();
            }
            else
            {
                Console.Clear();
                Console.WriteLine("Ligue de sports");
                Console.WriteLine($"\t*** {affichageList.ElementAt(index).ToUpper()} ***");

                int premierNb = LireStringPointage("\tEntrer le nouveau pointage: ", out int deuxiemeNb);

                Console.Clear();

                string temp1 = _gestionParties[index][0];
                string temp3 = _gestionParties[index][2];
                
                string[] donnee = new string[4];
                
                if (premierNb >= deuxiemeNb)
                {
                    donnee[0] = temp1;
                    donnee[1] = $"{premierNb}";
                    donnee[2] = temp3;
                    donnee[3] = $"{deuxiemeNb}";
                }
                else
                {
                    donnee[0] = temp3;
                    donnee[1] = $"{deuxiemeNb}";
                    donnee[2] = temp1;
                    donnee[3] = $"{premierNb}";
                }

                _gestionParties[index] = donnee;
                
                WriteLineSucess("*** Le pointage à bien été modifié.");
                MenuModifierPartie();
            }
        }

        static int RechercheIndexParIdentifiant(string id)
        {
            List<string> idEquipes = _gestionEquipes.Select(equipe => equipe[0]).ToList();

            return idEquipes.IndexOf(id);
        }
        
        static int RechercheIndexParNom(string id)
        {
            List<string> nomEquipes = _gestionEquipes.Select(equipe => equipe[1]).ToList();
            return nomEquipes.IndexOf(id);
        }

        static void MenuPremierePartie()
        {
            List<string> nomsEquipes = _gestionEquipes.Select(equipe => equipe[1]).ToList();
            CreerMenu("Choisir une équipe", nomsEquipes.ToArray());
            int choixEquipe = DemanderChoix(nomsEquipes.ToArray().Length);

            if (choixEquipe == 0)
            {
                Console.Clear();
                MenuGestionParties();
            }
            else
            {
                int index = choixEquipe - 1;

                Console.Clear();

                List<string> nomsEquipesContre = _gestionEquipes.Select(equipe => equipe[1]).ToList();
                nomsEquipesContre.RemoveAt(index);
                
                string premierChoix = nomsEquipes.ElementAt(index);
                CreerMenu($"{premierChoix} contre", nomsEquipesContre.ToArray());

                int contreChoix = DemanderChoix(nomsEquipesContre.ToArray().Length);

                if (contreChoix == 0)
                {
                    Console.Clear();
                    MenuPremierePartie();
                }
                else
                {
                    int deuxiemeChoixIndex = contreChoix - 1;

                    string deuxiemeChoix = nomsEquipesContre.ElementAt(deuxiemeChoixIndex);
                    Console.Clear();
                    Console.WriteLine("Ligue de sports");
                    Console.WriteLine($"\t*** {premierChoix.ToUpper()} CONTRE {deuxiemeChoix.ToUpper()} ***");
                    int premierNb = LireStringPointage("\tLe pointage de la partie (nombre-nombre): ", out int deuxiemeNb);

                    string[] donne = new string[4];

                    if (premierNb >= deuxiemeNb)
                    {
                        donne[0] = TrouverIdentifiant(index);
                        donne[1] = $"{premierNb}";
                        donne[2] = TrouverIdentifiant(RechercheIndexParNom(deuxiemeChoix));
                        donne[3] = $"{deuxiemeNb}";
                    }
                    else
                    {
                        donne[0] = TrouverIdentifiant(RechercheIndexParNom(deuxiemeChoix));
                        donne[1] = $"{deuxiemeNb}";
                        donne[2] = TrouverIdentifiant(index);
                        donne[3] = $"{premierNb}";
                    }
                    
                    _gestionParties.Add(donne);
                    Console.Clear();
                    WriteLineSucess("*** Le score de la partie à bien été ajouté.");
                    MenuGestionParties();
                }

            }
            
        }

        static string TrouverIdentifiant(int index)
        {
            List<string> listIdentifiant = _gestionEquipes.Select(equipe => equipe[0]).ToList();
            return listIdentifiant.ElementAt(index);
        }
        
        static string TrouverNom(int index)
        {
            List<string> listIdentifiant = _gestionEquipes.Select(equipe => equipe[1]).ToList();
            return listIdentifiant.ElementAt(index);
        }

        #endregion

        #region Statistiques

        
        static void MenuStatistiques()
        {
            CreerMenu(OptionsMenuPrincipale[3], OptionsStatistique);
            int choix = DemanderChoix(OptionsStatistique.Length);

            switch (choix)
            {
                case 1:
                    Console.Clear();
                    
                    List<int> scoreUnique = TrouverScore();

                    List<string> sports = new List<string>();
                    for (int i = 0; i < _gestionEquipes.Count; i++)
                    {
                        if(!sports.Contains(_gestionEquipes[i][3]))
                            sports.Add(_gestionEquipes[i][3]);
                    }

                    Console.WriteLine("Ligue de sports");
                    int idx = 0;
                    foreach (string sport in sports)
                    {
                        if(idx != 0)
                            Console.WriteLine();
                        WriteLineInfo($"\t{sport}");
                        idx++;
                        for (int i = 0; i < _gestionEquipes.Count; i++)
                            if (_gestionEquipes[i][3] == sport)
                                Console.WriteLine($"\t{_gestionEquipes[i][1]} avec un score de {scoreUnique[i]}");
                    }

                    if (_gestionEquipes.Count == 0)
                        WriteLineInfo("\n\t*** Pas d'équipe. Veuillez en ajouter une dans l'onglet gestion d'équipes.");

                    DemanderChoix(0);
                    Console.Clear();
                    MenuStatistiques();
                    break;
                case 2:
                    Console.Clear();
                    Console.WriteLine("Ligue de sports");
                    int moyAge = 0;
                    int cpt = 0;
                    for (int i = 0; i < _noms.Length; i++)
                    {
                        if (string.IsNullOrEmpty(_noms[i]))
                            continue;
                        int age = CalculerAge(_datesNaissances[i]);
                        Console.WriteLine($"\t{_noms[i]}: {age} ans");
                        moyAge += age;
                        cpt++;
                    }

                    // Division par 0 impossible
                    WriteLineInfo(cpt > 0
                        ? $"\n\tÂge moyens des joueurs: {(double) moyAge / cpt} ans."
                        : "\n\t*** Pas de joueurs. Veuillez en ajouter un dans l'onglet gestion de joueurs.");
                    DemanderChoix(0);
                    Console.Clear();
                    MenuStatistiques();
                    break;
                case 3:
                    Console.Clear();
                    MenuAfficherParties();
                    break;
                default:
                    Console.Clear();
                    MenuPrincipale();
                    break;
            }
        }

        static void MenuAfficherParties()
        {
            string[] menu = _gestionEquipes.Select(gestionEquipe => gestionEquipe[1]).ToArray();
            CreerMenu("Choisir une équipes pour afficher leurs parties", menu);
            int choixParties = DemanderChoix(menu.Length);
            int idxParties = choixParties - 1;
            if (choixParties == 0)
            {
                Console.Clear();
                MenuStatistiques();
            }
            else
            {
                Console.Clear();
                MenuListePartiesJouee(idxParties);

                DemanderChoix(0);
                Console.Clear();
                MenuAfficherParties();
            }
        }

        static void MenuListePartiesJouee(int idxParties)
        {
            Console.WriteLine("Ligue de sports");
            int score = TrouverScore()[idxParties];
            WriteLineInfo($"\t{_gestionEquipes[idxParties][1]} à {score} pts.");

            foreach (string[] parties in _gestionParties)
            {
                if (_gestionEquipes[idxParties][1] == TrouverNomParId(parties[0]) ||
                    _gestionEquipes[idxParties][1] == TrouverNomParId(parties[2]))
                    Console.WriteLine(
                        $"\t{TrouverNomParId(parties[0])} vs {TrouverNomParId(parties[2])} : {parties[1]}-{parties[3]}");
            }
        }

        static List<int> TrouverScore()
        {
            List<string> equipes = new List<string>();
            List<int> equipes2 = new List<int>();

            foreach (string[] gestion in _gestionParties)
            {
                string nomEquipe1 = TrouverNomParId(gestion[0]);
                string nomEquipe2 = TrouverNomParId(gestion[2]);
                int premierePartie = Convert.ToInt32(gestion[1]);
                int deuxiemePartie = Convert.ToInt32(gestion[3]);

                int scorePremier = 0;
                int scoreDeuxième = 0;

                if (premierePartie == deuxiemePartie)
                {
                    scorePremier += 1;
                    scoreDeuxième += 1;
                }
                else if (premierePartie > deuxiemePartie)
                    scorePremier += 2;
                else
                    scoreDeuxième += 2;

                equipes.Add(nomEquipe1);
                equipes.Add(nomEquipe2);
                equipes2.Add(scorePremier);
                equipes2.Add(scoreDeuxième);
            }

            List<int> scoreUnique = new List<int>();
            scoreUnique = Enumerable.Repeat(0, _gestionEquipes.Count).ToList();

            for (int i = 0; i < equipes.Count; i++)
                scoreUnique[RechercheIndexParNom(equipes[i])] += equipes2[i];
            
            return scoreUnique;
        }

        static int CalculerAge(DateTime dateNaissance)
        {
            return DateTime.Today.Year - dateNaissance.Year;
        }
        
        static string TrouverNomParId(string id)
        {
            List<string> listId = _gestionEquipes.Select(equipe => equipe[0]).ToList();
            List<string> listNom = _gestionEquipes.Select(equipe => equipe[1]).ToList();
            int indexOf = listId.IndexOf(id);

            return listNom.ElementAt(indexOf);
        }
        
        #endregion

        #region Menu
        static int DemanderChoix(int maximum)
            => LireInt32DansIntervalleWrite("Votre choix: ", 0, maximum);

        static void CreerMenu(string titre, string[] options, bool afficherIndex = true)
        {
            Console.WriteLine("Ligue de sports");
            Console.WriteLine($"\t*** {titre.ToUpper()} ***");
            options = options.Where(x => !string.IsNullOrEmpty(x)).ToArray();
            for (int i = 0; i < options.Length; i++)
                Console.WriteLine(afficherIndex ? $"\t{i + 1}. {options[i]}" : $"\t{options[i]}");
                

            Console.WriteLine($"\n\t{Sentinelle}. {(titre == "Menu Principal" ? "Quitter le programme" : "Annuler l'opération")}");
        }

        #endregion
        
        #region Fichier

        static void DeserialiserFichier()
        {
            if (!File.Exists(NomFichier))
                return;
            using FileStream fichier = File.OpenRead(NomFichier);
            BinaryFormatter formatter = new BinaryFormatter();
            
            _noms = (string[]) formatter.Deserialize(fichier);
            _datesNaissances = (DateTime[]) formatter.Deserialize(fichier);
            _paysNaissances = (string[]) formatter.Deserialize(fichier);
            _gestionEquipes = (List<List<string>>) formatter.Deserialize(fichier);
            _gestionParties = (List<string[]>) formatter.Deserialize(fichier);
        }

        static void SerialiserFichier()
        {
            using FileStream fichier = File.Create(NomFichier);

            BinaryFormatter formateur = new BinaryFormatter();
            try
            {
                formateur.Serialize(fichier, _noms);
                formateur.Serialize(fichier, _datesNaissances);
                formateur.Serialize(fichier, _paysNaissances);
                formateur.Serialize(fichier, _gestionEquipes);
                formateur.Serialize(fichier, _gestionParties);
            }
            finally
            {
                fichier.Close();
            }
        }

        #endregion
        
        #region Vecteur

        static bool EstUniqueVecteur(string[] vecteur, string nouvelleValeur)
        {
            if (!vecteur.Contains(nouvelleValeur, StringComparer.OrdinalIgnoreCase))
                return !vecteur.Contains(nouvelleValeur, StringComparer.OrdinalIgnoreCase);
            Console.Clear();
            WriteLineError($"*** La liste contient déjà ({nouvelleValeur}).");
            return !vecteur.Contains(nouvelleValeur, StringComparer.OrdinalIgnoreCase);
        }

        static bool EstAjoutableVecteur<T>(T[] vecteur, out int index)
        {
            int nbElement = vecteur.Count(x => x != null);
            index = nbElement;
            if(nbElement >= MaximumJoueurs)
                WriteLineError($"*** La liste ne peut avoir plus que {MaximumJoueurs} éléments.");

            return nbElement < MaximumJoueurs;
        }

        static string[] OrdonnerVecteurAlphabetique(IEnumerable<string> vecteur)
            => vecteur.Where(x => !string.IsNullOrEmpty(x)).OrderBy(x => x).ToArray();

        #endregion

    }
}
