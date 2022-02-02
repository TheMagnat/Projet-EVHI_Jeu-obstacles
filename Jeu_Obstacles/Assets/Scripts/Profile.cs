using System.Collections;
using System.Linq;
using System;
using System.Collections.Generic;
using UnityEngine;
using SimpleJSON;
using Newtonsoft.Json;

namespace Profile
{
    public class ProfileType
    {
        public string name { get; set; }
        public List<string> dates { get; set; }
        public List<float> durees { get; set; }
        //public List<double> vitesseDecision { get; set; }
        public List<double> pouls { get; set; }
        public List<Dictionary<int, int>> directionCamera { get; set; }
        public List<Dictionary<int, int>> strategieEvitement { get; set; }
        public List<Dictionary<int, int>> strategiePlacement { get; set; }

        public void Populate(string jsonString)
        {
            JSONNode data = JSON.Parse(jsonString);
            this.name = (string)data["name"];
            this.dates = JsonConvert.DeserializeObject<List<string>>(data["dates"].ToString());
            this.durees = JsonConvert.DeserializeObject<List<float>>(data["durees"].ToString());
            //this.vitesseDecision = JsonConvert.DeserializeObject<List<double>>(data["vitesseDecision"].ToString());
            this.pouls = JsonConvert.DeserializeObject<List<double>>(data["pouls"].ToString());

            this.strategieEvitement = JsonConvert.DeserializeObject<List<Dictionary<int, int>>>(data["strategieEvitement"].ToString());
            this.strategiePlacement = JsonConvert.DeserializeObject<List<Dictionary<int, int>>>(data["strategiePlacement"].ToString());
            this.directionCamera = JsonConvert.DeserializeObject<List<Dictionary<int, int>>>(data["directionCamera"].ToString());
        }

        public void Create(string profileName)
        {
            this.name = profileName;
            this.dates = new List<string>();
            this.durees = new List<float>();
            //this.vitesseDecision = new List<double>();
            this.pouls = new List<double>();
            this.directionCamera = new List<Dictionary<int, int>>();
            this.strategiePlacement = new List<Dictionary<int, int>>();
            this.strategieEvitement = new List<Dictionary<int, int>>();

        }

        // Poids = (1,2...,n  /Sum(1 à n) ) x n
        private List<float> genWeights(int size)
        {
            List<float> weights = Enumerable.Range(1, size).Select(x => x / 1.0f).ToList();
            int denom = (int)weights.Sum();
            for (int j = 0; j < weights.Count; j += 1)
            {
                weights[j] = weights[j] / denom;
            }
            return weights;
        }

        public float getDureeMean()
        {
            List<float> weights = genWeights(this.durees.Count);
            float sum = 0f;
            int i = 0;
            foreach (float p in this.durees)
            {
                sum += p * weights[i];
                i += 1;
            }
            return sum;
        }

        /*public float getVitesseMean()
        {
            List<float> weights = genWeights(this.vitesseDecision.Count);
            float sum = 0f;
            int i = 0;
            foreach (float p in this.vitesseDecision)
            {
                sum += p * weights[i];
                i += 1;
            }
            return sum / this.vitesseDecision.Count;
        }*/

        public float getPoulsMean()
        {
            List<float> weights = genWeights(this.pouls.Count);
            float sum = 0f;
            int i = 0;
            foreach (float p in this.pouls)
            {
                sum += p * weights[i];
                i += 1;
            }
            return sum;
        }

        public Dictionary<int, int> getPlacementMean()
        {
            List<float> weights = genWeights(this.strategiePlacement.Count);
            Dictionary<int, int> mean = new Dictionary<int, int>();

            if (this.strategiePlacement.Count == 0)
            {
                return mean;
            }


            Dictionary<int, int>.KeyCollection keyColl = this.strategiePlacement[0].Keys;
            float tmp;
            foreach (int key in keyColl)
            {
                mean.Add(key, 0);
                tmp = 0f;
                for (int i = 0; i < this.strategiePlacement.Count; i++)
                {
                    tmp += this.strategiePlacement[i][key] * weights[i];
                }
                mean[key] = (int)(Math.Truncate(tmp));
            }
            return mean;
        }

        public Dictionary<int, int> getEvitementMean()
        {
            List<float> weights = genWeights(this.strategieEvitement.Count);
            Dictionary<int, int> mean = new Dictionary<int, int>();

            if (this.strategieEvitement.Count == 0)
            {
                return mean;
            }

            Dictionary<int, int>.KeyCollection keyColl = this.strategieEvitement[0].Keys;
            float tmp;
            foreach (int key in keyColl)
            {
                mean.Add(key, 0);
                tmp = 0f;
                for (int i = 0; i < this.strategieEvitement.Count; i++)
                {
                    tmp += this.strategieEvitement[i][key] * weights[i];
                }
                mean[key] = (int)(Math.Truncate(tmp));
            }
            return mean;
        }

        public Dictionary<int, int> getCameraMean()
        {
            List<float> weights = genWeights(this.directionCamera.Count);
            Dictionary<int, int> mean = new Dictionary<int, int>();
            
            if (this.directionCamera.Count == 0)
            {
                return mean;
            }

            Dictionary<int, int>.KeyCollection keyColl = this.directionCamera[0].Keys;

            float tmp;
            foreach (int key in keyColl)
            {
                mean.Add(key, 0);
                tmp = 0f;
                for (int i = 0; i < this.directionCamera.Count; i++)
                {
                    tmp += this.directionCamera[i][key] * weights[i];
                }
                mean[key] = (int)(Math.Truncate(tmp));
            }
            
            return mean;
        }

        // Code take from https://github.com/markwhitaker/MonthDiff, free of rights
        private int GetTotalMonthsFrom(DateTime dt1, DateTime dt2)
        {
            DateTime earlyDate = (dt1 > dt2) ? dt2.Date : dt1.Date;
            DateTime lateDate = (dt1 > dt2) ? dt1.Date : dt2.Date;

            // Start with 1 month's difference and keep incrementing
            // until we overshoot the late date
            int monthsDiff = 1;
            while (earlyDate.AddMonths(monthsDiff) <= lateDate)
            {
                monthsDiff++;
            }

            return monthsDiff - 1;
        }

        public void UpdateData()
        {
            DateTime date;
            DateTime today = DateTime.Now;
            for (int i = 0; i < this.dates.Count; i++)
            {
                date = DateTime.ParseExact(this.dates[0], "dd-MM-yyyy_HH:mm:ss", null);
                if (GetTotalMonthsFrom(date, today) > 3)
                { // Si la date de la session > 3 mois, remove session
                    this.durees.RemoveAt(0);
                    this.dates.RemoveAt(0);
                    //this.vitesseDecision.RemoveAt(0);
                    this.pouls.RemoveAt(0);
                    this.strategiePlacement.RemoveAt(0);
                    this.strategieEvitement.RemoveAt(0);
                    this.directionCamera.RemoveAt(0);
                }
            }
        }

        //Base Level equation
        //Base Level equation
        public float BaseLevel()
        {
            if (this.durees.Count < 1)
            {
                return 1f;
            }

            float timeForMaxSpeed = 90f;
            // Arbitrary Threshold for demonstration purposes
            // If player cant reach this value then it means difficulty is too hard
            // Base Level should decreases, otherwise BaseLevel increases
            float worstDuree = 10f;

            List<float> wk = new List<float>();
            for (int i = 0; i < this.durees.Count; i++)
            {
                float currentDuree = Math.Max(durees[i] - worstDuree, 0f);
                wk.Add( Math.Min(currentDuree / (timeForMaxSpeed - worstDuree), 1f) * 1.5f + 0.5f );
            }

            double sum = 0.0;
            double diffDay;
            DateTime date;
            TimeSpan diffTime;

            List<float> weights = new List<float>();

            for (int i = 0; i < this.dates.Count; i++)
            {
                date = DateTime.ParseExact(this.dates[i], "dd-MM-yyyy_HH:mm:ss", null);
                diffTime = DateTime.Now - date;
                diffDay = Math.Max(diffTime.TotalHours, 1.0);

                weights.Add( (float) Math.Pow(diffDay, -0.5) );

            }

            float sumValue = weights.Sum();
            for (int i = 0; i < this.dates.Count; i++)
            {
                weights[i] = (weights[i] / sumValue) * wk[i];
            }

            return weights.Sum();
        }

    }
}
