﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Web;

/// <summary>
/// Summary description for Person
/// </summary>
public class Person
{

    public string Voornaam { get; set; }
    public string Tussenvoegsel { get; set; }
    public string Achternaam { get; set; }
    public string Straat { get; set; }
    public int Huisnr { get; set; }
    public string Woonplaats { get; set; }
    public int Banknr { get; set; }

	public Person(string voornaam, string tussenvoegsel, string achternaam, string straat, int huisnr, string woonplaats, int banknr)
	{
	    Voornaam = voornaam;
	    Tussenvoegsel = tussenvoegsel;
	    Achternaam = achternaam;
	    Straat = straat;
	    Huisnr = huisnr;
	    Woonplaats = woonplaats;
	    Banknr = banknr;
	    
	}

    
}