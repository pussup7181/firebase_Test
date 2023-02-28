using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CountryCodeAndFlag : MonoBehaviour
{
   [System.Serializable]
    public class Country
    {
        public string name;
        public string ISO;
        public int Code;
        public string flag;
    }

    [System.Serializable]
    public class Countries
    {
        public Country[] countries;

    }

    public TextAsset jsonFile;
    public Image countryFlag;
    public TextMeshProUGUI countryCode;
    public GameObject countryCodePrefab;
    public GameObject Content;
    public Toggle defaultValue;
    public GameObject seperaterPrefab;
    char seperator;
    Countries countriesInJson;
    Transform[] countryListToggles;
    public TMP_InputField searchText;
    public GameObject countryCodeMenu;
    // Start is called before the first frame update
    void Start()
    {
       
        countriesInJson = JsonUtility.FromJson<Countries>(jsonFile.text);
        foreach(Country country in countriesInJson.countries)
        {
            GameObject seperatorObj;
            if (country.name[0] != seperator)
            {
                seperator = country.name[0];
                seperatorObj = Instantiate(seperaterPrefab, Content.transform, false);
                seperatorObj.GetComponentInChildren<TextMeshProUGUI>().text = seperator.ToString(); ;
            }
            GameObject countryButton = Instantiate(countryCodePrefab, Content.transform, false);
            countryButton.GetComponentInChildren<Image>().sprite = Resources.Load<Sprite>(country.flag.Trim());
            countryButton.GetComponentInChildren<TextMeshProUGUI>().text = country.name + " (+"+country.Code+")";
            countryButton.GetComponentInChildren<Toggle>().group = Content.GetComponent<ToggleGroup>();
            countryButton.GetComponentInChildren<Toggle>().onValueChanged.AddListener(delegate {
                CloseMenuandAssignValues(countryButton.GetComponentInChildren<Toggle>(), country);
            });

        }
        countryListToggles = Content.GetComponentsInChildren<Transform>() ;
        gameObject.GetComponentInChildren<Button>().GetComponentInChildren<Image>().sprite = Resources.Load<Sprite>("in");
        gameObject.GetComponentInChildren<TextMeshProUGUI>().text = "(+91)";

    }

    private void CloseMenuandAssignValues(Toggle toggle, Country country)
    {
        if (toggle.isOn)
        {
            Debug.Log(country.name + " " + country.Code);
            defaultValue.GetComponentInChildren<Image>().sprite = Resources.Load<Sprite>(country.flag.Trim());
            defaultValue.GetComponentInChildren<TextMeshProUGUI>().text = country.name + " (+" + country.Code + ")";
            defaultValue.GetComponentInChildren<Toggle>().group = Content.GetComponent<ToggleGroup>();
            countryCodeMenu.GetComponent<Animator>().ResetTrigger("In");
            countryCodeMenu.GetComponent<Animator>().SetTrigger("Out");
            gameObject.GetComponentInChildren<Button>().GetComponentInChildren<Image>().sprite = Resources.Load<Sprite>(country.flag.Trim());
            gameObject.GetComponentInChildren<TextMeshProUGUI>().text = " (+" + country.Code + ")";
        }
        
    }



    // Update is called once per frame
    void Update()
    {
        
    }

    public void Search(string stringToCompare)
    {
        Debug.Log(stringToCompare);
        
        foreach(Transform obj in countryListToggles)
        {
            string butName = obj.GetComponentInChildren<TextMeshProUGUI>().text;
            if (CaseInsensitiveContains(butName, stringToCompare))
            {
                obj.gameObject.SetActive(true);
            }
            else obj.gameObject.SetActive(false);
        }
    

    
}
    public static bool CaseInsensitiveContains(string text, string value,
        StringComparison stringComparison = StringComparison.CurrentCultureIgnoreCase)
    {
        return text.IndexOf(value, stringComparison) >= 0;
    }
}
