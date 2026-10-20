
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using RedWhiteFootball.Core;

namespace RedWhiteFootball.UI
{
    public class MainMenuManager : MonoBehaviour
    {
        public TMP_Dropdown teamADropdown;
        public TMP_Dropdown teamBDropdown;
        public TeamsDatabase database;
        public TextMeshProUGUI titleText;

        void Start()
        {
            if (titleText) titleText.text = "RedWhiteFootball 2027\nالنادي الإفريقي";
            if (database != null && teamADropdown != null)
            {
                teamADropdown.ClearOptions();
                teamBDropdown.ClearOptions();
                var names = new System.Collections.Generic.List<string>();
                foreach (var t in database.teams) names.Add($"{t.nameAr} - {t.nameEn}");
                teamADropdown.AddOptions(names);
                teamBDropdown.AddOptions(names);
                teamADropdown.value = 0; // Club Africain default
                teamBDropdown.value = 7;
            }
        }

        public void OnPlay()
        {
            if (database != null)
            {
                var teamA = database.teams[teamADropdown.value];
                var teamB = database.teams[teamBDropdown.value];
                PlayerPrefs.SetString("TeamA", teamA.id);
                PlayerPrefs.SetString("TeamB", teamB.id);
            }
            SceneManager.LoadScene("Match");
        }

        public void OnQuit() => Application.Quit();
    }
}
