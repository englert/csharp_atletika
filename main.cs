/* atletikaVB2017.csv   http://www.infojegyzet.hu/erettsegi/informatika-ismeretek/proba-prog-2021aprl/
100 m;F;Egyesült Államok;Justin Gatlin;9,92;SB;1
200 m;F;Törökország;Ramil Guliyev;20,09;SB;1

0. A versenyszám neve. Például: „10 000 m”
1. Férfi vagy női versenyszám Például: „N” („N-női; „F”-férfi)
2. Mely nemzet színeiben indult a dobogós. Például: „Etiópia”
3. A versenyszámban az adott helyezést elérő versenyző neve. Például: „Almaz Ayana”
4. Az elért eredmény. Például: „30:16,3”
5. Csúcs (ért-e el valamilyen csúcsot az adott versenyző). Például: „WL”
    WR – világrekord
    CR – világbajnoki rekord
    AR – kontinens rekord
    NR – országos rekord
    PB – egyéni rekord
    WL – az adott évben aktuálisan a világ legjobb eredménye
    SB – az adott évben aktuálisan az egyéni legjobb eredmény
6. A helyezés sorszáma (érem). Például: „1” (1-arany; 2-ezüst; 3-bronz)
*/
// 1. Készítsen konzol alkalmazást a következő feladatok megoldására, amelynek projektjét mentse el "atletikaVB” néven!

using System;                       // Console
using System.IO;                    // StreamReader() StreamWriter()
using System.Collections.Generic;   // List<>
using System.Linq;                  // from where select

class Atletika
{
    public string versenyszam   {get; set;}
    public string gender        {get; set;}
    public string nemzet        {get; set;}
    public string nev           {get; set;}
    public string eredmeny      {get; set;}
    public string csucs         {get; set;}
    public int    helyezes      {get; set;}

    public Atletika(string sor)
    {
        var s = sor.Split(';');
        versenyszam = s[0];
        gender      = s[1];
        nemzet      = s[2];
        nev         = s[3];
        eredmeny    = s[4];
        csucs       = s[5];
        helyezes    = int.Parse( s[6] );
    }
}

class Program
{
    static void Main(string[] args)
    {
        // 2. Olvassa be az atletikaVB2017.csv állomány sorait és tárolja az adatokat egy olyan adatszerkezetben, amely használatával a további feladatok megoldhatók! Mivel a versenyszámok eredményei különböző mértékegységben vannak megadva, ezért az eredményt szöveges formában tárolja el! A fájl legfeljebb 200 sort tartalmaz.
        
        var lista = new List<Atletika>();
        var sr    = new StreamReader("atletikaVB2017.csv");

        while(!sr.EndOfStream)
        {
            var sor = sr.ReadLine();
            lista.Add( new Atletika(sor) );
        }
        sr.Close();

        // 3. Határozza meg és írja ki a képernyőre a minta szerint, hogy összesen hány egyéni érmet osztottak ki és ezt hány versenyszámban! Vegye figyelembe, hogy női rúdugrásban holtverseny volt (csak itt), azaz két bronzérmet osztottak ki.
        var ferfi_versenyszam =
        (
            from sor in lista
            where sor.gender == "F"
            group sor by sor.versenyszam
        ).Count();

        var noi_versenyszam =
        (
            from sor in lista
            where sor.gender == "N"
            group sor by sor.versenyszam
        ).Count();

        Console.WriteLine($"3. feladat:");
        Console.WriteLine($"       A 2017-es londoni VB-n összesen {lista.Count} érmet osztottak ki a {ferfi_versenyszam + noi_versenyszam} versenyszámban.");

        // 4. A legtöbbször a VB Londonban az amerikai himnuszt játszották és az Egyesült Államok versenyzői gyűjtötték a legtöbb érmet. Határozza meg és írja ki a minta szerint, hogy az amerikai érmek hány százaléka volt arany! Az eredmény 1 tizedesjegyre kerekítve jelenjen meg!

        double usa_ermek =
        (
            from sor in lista
            where sor.nemzet == "Egyesült Államok"
            select sor
        ).Count();
        
        double usa_aranyermek =
        (
            from sor in lista
            where sor.nemzet == "Egyesült Államok"
            where sor.helyezes == 1
            select sor
        ).Count();

        Console.WriteLine();
        Console.WriteLine($"4. feladat:");
        Console.WriteLine($"       Az amerikai érmek {100 * usa_aranyermek / usa_ermek:0.#}%-a arany.");

        /* 5. Kérjen be a felhasználótól a minta szerint egy nemzet nevét és egy számot 1 és 3 között és tárolja el ezeket! Feltételezheti, hogy a felhasználó 1 és 3 közötti egész számot adott meg.
            Adja meg a nemzet nevét:
            Adja meg a helyezést (1-3 egész szám):
        */
        Console.WriteLine();
        Console.WriteLine($"5. feladat:");
        Console.Write(    $"        Adja meg a nemzet nevét: ");
        var nemzet = Console.ReadLine();
        Console.Write(    $"        Adja meg a helyezést (1-3 egész szám): ");
        var helyezes = int.Parse( Console.ReadLine() );

        /* 6. Határozza meg és írja ki a minta szerint az előző feladatban bekért adatok alapján, hogy az adott nemzet versenyzője állt-e a dobogón adott fokán! Ha volt ilyen versenyző, akkor írjon ki egy ilyet a minta szerint! Ha nem volt ilyen versenyző, azt is írja ki a minta szerint! A helyezés száma helyett az „arany”, „ezüst” és „bronz” szavak jelenjenek meg!
            {} egyik {} szerző versenyzője:
            {} {} versenyszámban.
        */  
        var versenyzo = 
        (
            from sor in lista
            where sor.nemzet   == nemzet
            where sor.helyezes == helyezes
            select sor 
        );
        
        string[] erem = {"", "arany", "ezüst", "bronz"};
        Console.WriteLine();
        Console.WriteLine(        $"6. feladat:");
        if ( versenyzo.Any() )
        {
            foreach (var item in versenyzo)
            {
                Console.WriteLine($"        {item.nemzet} egyik {erem[helyezes]}érmet szerző versenyzője:");
                Console.WriteLine($"        {item.nev} {item.versenyszam} versenyszámban.\n");
            }
        }
        else
        {
            Console.WriteLine(    $"        {nemzet} nem szerzett {erem[helyezes]}érmet a VB-n.");
        }

        // 7. Készítsen statisztikát a VB-n a női versenyzők által elért egyéni csúcsokról a noi_csucsok.txt         fájlba! Csak azok a női versenyző jelenjenek meg a fájlban a minta szerint (név, versenyszám, nemzet), akik valamilyen csúcsot elértek! Az adatokat tabulátorral válassza el egymástól és fájl első sora fejléc legyen, ahogy a mintában is látszik!
        /* minta:
        név     versenyszám     nemzet
        Katerina Sztefanidi     rúdugrás Görögország
        */
        var noi_csucsok = 
        (
            from sor in lista
            where sor.gender == "N"
            where sor.csucs != ""
            select sor
        );

        var sw = new StreamWriter("noi_csucsok.txt");
        sw.WriteLine("név\tversenyszám\tnemzet");
        foreach (var item in noi_csucsok)
        {
            sw.WriteLine($"{item.nev}\t{item.versenyszam}\t{item.nemzet}");
        }
        sw.Close();

        Console.WriteLine($"7. feladat:");
        Console.WriteLine($"        női csúcsok a fájlba írva:");
        
    }// ----- end of Main method ----
} // -------- end of Program class ------