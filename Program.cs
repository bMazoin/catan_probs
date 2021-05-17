using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;

namespace catan_probs
{
    class Program
    {
        public int m_iNbLancers = 0;

        static void Main(string[] args)
        {
            List<Joueur> joueurs = new List<Joueur>();
            List<String> lancers = new List<string>();
            string line;
            Partie partie = new Partie();
            if(!(args.Length > 3)){
                Console.WriteLine("USAGE : {0} {1} {2} {3} {4}", "catan_probs.exe", "fichier.txt", "joueur1", "joueur2", "etc");
                return;
            }

            // On lit les paramètres donnés => TODO : LIRE DANS LE FICHIER TXT LES JOUEURS
            // => REORGANISER LA LISTE
            for(int i = 1; i < args.Length; i++){
                joueurs.Add(new Joueur(args[i]));
            }

            // On lit le fichier de résultat
            System.IO.StreamReader file = new System.IO.StreamReader(args[0]);  
            while((line = file.ReadLine()) != null)  
            {  
                if(line.First() == '#')
                {
                    line.Remove(0,1); // On supprime le #
                    List<string> nomsJoueurs = line.Split('-').ToList();
                    int cptJoueur = 0;
                    nomsJoueurs.ForEach(oNom => {
                        if(joueurs.Count == 0 && 
                            joueurs.Where(oJoueur => oJoueur.m_sNom == line).ToList().Count == 0)
                        {
                            joueurs.Add(new Joueur(oNom));
                        }
                        else if(joueurs.FindIndex(oJoueur => oJoueur.m_sNom == oNom) != cptJoueur)
                        {
                            
                        }

                        cptJoueur++;
                    });
                }
                System.Console.WriteLine(line);

                lancers = line.Split("-").ToList();
                if(lancers.Count == joueurs.Count){
                    int cpt = 0;
                    lancers.ForEach(oLancer => {
                        // oLancer -2 car on donne l'indice (dans le cas où le lancer = 2 il faut incrémenter la premiere)
                        // valeur du tableau
                        int value;
                        if(int.TryParse(oLancer, out value)){
                            joueurs.ElementAt(cpt).addOccurence(value - 2); 
                            partie.addOccurence(value - 2);
                            cpt++;
                        }
                    });
                }
            }

            // Affichage des résultats
            int l_iCodeRetourGlob = 1;
            while(l_iCodeRetourGlob != 0)
            {
                Console.WriteLine("====================================");
                Console.WriteLine("0 - Quitter");
                Console.WriteLine("1 - Affichage des résultats par partie");
                Console.WriteLine("2 - Affichage des résultats par joueur");
                Console.WriteLine("3 - Affichage des résultats globaux");
                string sRetour = Console.ReadLine();
                if(int.TryParse(sRetour, out l_iCodeRetourGlob))
                {
                    if (l_iCodeRetourGlob >= 1 && l_iCodeRetourGlob <= 3)
                    {
                        switch (l_iCodeRetourGlob){
                            case 1:
                                break;
                            case 2:
                                joueurs.ForEach(oJoueur => {
                                    oJoueur.afficherResultat();
                                });
                                break;
                            case 3:
                                partie.afficherResultat();
                                break;
                        }
                    }
                }
            }
        }
    }

    class Partie{
        private int m_iNbLancers;
        private int[] m_tOcc = {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0};

        public Partie(){}

        public int getNbLancers(){
            return m_iNbLancers;
        }

        public void afficherResultat(){
            int cpt = 2;
            m_tOcc.ToList().ForEach(oOccurence => {
                float result = ((float)oOccurence / (float)m_iNbLancers)*100;
                Console.WriteLine("Proba pour {0} : {1}%", cpt, Math.Round(result));
                cpt++;
            });
        }

        public void addOccurence(int index){
            this.m_tOcc[index]++;
            m_iNbLancers++;
        }
    }

    class Joueur{
        private int[] m_tOcc = {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0};
        private List<int[]> m_ltOcc = new List<int[]>();
        private int m_iNbLancers = 0;
        public string m_sNom;

        public Joueur(string l_sNom)
        {
            m_sNom = l_sNom;
            m_ltOcc.Add(new int[]{0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0});
        }

        public void addOccurence(int index){
            this.m_tOcc[index]++;
            this.m_iNbLancers++;
        }

        public void afficherResultat(){
            Console.WriteLine("JOUEUR = {0}", m_sNom);
            int cpt = 2;
            int occTot = 0;
            m_tOcc.ToList().ForEach(oOc => occTot += oOc);

            m_tOcc.ToList().ForEach(oOccurence => {
                float result = ((float)oOccurence / (float)m_iNbLancers)*100;
                Console.WriteLine("Proba pour {0} : {1}%", cpt, Math.Round(result));
                cpt++;
            });
        }
    }
}
