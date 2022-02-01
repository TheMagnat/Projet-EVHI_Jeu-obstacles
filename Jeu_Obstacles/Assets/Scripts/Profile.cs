using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SimpleJSON;
using Newtonsoft.Json;

namespace Profile{
    public class ProfileType
    {
        public string name { get; set; }
        public List<string> dates { get; set; }
        public List<float> durees { get; set; }
        public List<double> vitesseDecision { get; set; }
        public List<double> pouls { get; set; }
        public List<Dictionary<int,int>> directionCamera { get; set; }
        public List<Dictionary<int,int>> strategieEvitement { get; set; }
        public List<Dictionary<int,int>> strategiePlacement { get; set; }

        public void Populate(string jsonString){
            JSONNode data = JSON.Parse(jsonString);
            this.name = (string) data["name"];
            this.dates = JsonConvert.DeserializeObject<List<string>>(data["dates"].ToString());
            this.durees = JsonConvert.DeserializeObject<List<float>>(data["durees"].ToString());
            this.vitesseDecision = JsonConvert.DeserializeObject<List<double>>(data["vitesseDecision"].ToString());
            this.pouls = JsonConvert.DeserializeObject<List<double>>(data["pouls"].ToString());

            this.strategieEvitement = JsonConvert.DeserializeObject<List<Dictionary<int, int>>>(data["strategieEvitement"].ToString());
            this.strategiePlacement = JsonConvert.DeserializeObject<List<Dictionary<int, int>>>(data["strategiePlacement"].ToString());
            this.directionCamera = JsonConvert.DeserializeObject<List<Dictionary<int, int>>>(data["directionCamera"].ToString());
        }

        public void Create(string profileName){
            this.name = profileName;
            this.dates = new List<string>();
            this.durees = new List<float>();
            this.vitesseDecision = new List<double>();
            this.pouls = new List<double>();
            this.directionCamera = new List<Dictionary<int,int>>();
            this.strategiePlacement = new List<Dictionary<int,int>>();
            this.strategieEvitement = new List<Dictionary<int,int>>();

        }
        
        // Somme pondéré des pouls, avec poids croissant: poids qui sommes à n car valeur entière
        
        // Update ancienne donnés
        
        // Stratégie placement, somme pondéré element wise poids croissant -> int
        
        // Strategie devitement
    }
}
