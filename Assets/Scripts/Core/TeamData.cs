
using System;
using UnityEngine;

namespace RedWhiteFootball.Core
{
    [Serializable]
    public class TeamInfo
    {
        public string id;
        public string nameAr;
        public string nameEn;
        public Color primaryColor;
        public Color secondaryColor;
        public int attack = 80;
        public int defense = 80;
    }

    [CreateAssetMenu(fileName="TeamsDatabase", menuName="RedWhite/Teams Database")]
    public class TeamsDatabase : ScriptableObject
    {
        public TeamInfo[] teams = new TeamInfo[]
        {
            new TeamInfo{ id="CA", nameAr="النادي الإفريقي", nameEn="Club Africain", primaryColor=new Color(0.82f,0.05f,0.12f), secondaryColor=Color.white, attack=84, defense=82 },
            new TeamInfo{ id="EST", nameAr="الترجي الرياضي", nameEn="Esperance", primaryColor=new Color(0.9f,0.8f,0.1f), secondaryColor=new Color(0.6f,0,0), attack=82, defense=80 },
            new TeamInfo{ id="ESS", nameAr="النجم الساحلي", nameEn="Etoile Sahel", primaryColor=new Color(0.8f,0,0), secondaryColor=Color.white, attack=80, defense=78 },
            new TeamInfo{ id="CSS", nameAr="النادي الصفاقسي", nameEn="CS Sfaxien", primaryColor=Color.black, secondaryColor=Color.white, attack=78, defense=79 },
            new TeamInfo{ id="ACA", nameAr="نادي العاصمة", nameEn="Capital FC", primaryColor=new Color(0.82f,0.05f,0.12f), secondaryColor=Color.white, attack=85, defense=83 },
            new TeamInfo{ id="WHI", nameAr="النسور البيضاء", nameEn="White Eagles", primaryColor=Color.white, secondaryColor=new Color(0.82f,0.05f,0.12f), attack=83, defense=81 },
            new TeamInfo{ id="RED", nameAr="الحمر", nameEn="Reds United", primaryColor=new Color(0.7f,0,0), secondaryColor=Color.white, attack=86, defense=84 },
            new TeamInfo{ id="BLU", nameAr="الزرق", nameEn="Blues City", primaryColor=new Color(0.05f,0.2f,0.7f), secondaryColor=Color.white, attack=87, defense=85 },
        };
    }
}
