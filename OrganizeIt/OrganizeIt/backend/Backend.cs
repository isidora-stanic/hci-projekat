using OrganizeIt.backend.social_gatherings;
using OrganizeIt.backend.users;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;

namespace OrganizeIt.backend
{
    public class Backend
    {
        private static string DataDir = Path.GetDirectoryName(Path.GetDirectoryName(Directory.GetCurrentDirectory())) +
                          Path.DirectorySeparatorChar + "backend" + Path.DirectorySeparatorChar + "data" +
                          Path.DirectorySeparatorChar;

        public static Dictionary<string, User> Users { get; set; }
        public static Dictionary<int, SocialGatheringCollaborator> Collaborators { get; set; }
        public static User LoggedInUser { get; set; }

        public Backend()
        {
            Users = LoadUsers();
            Collaborators = LoadCollaborators();
        }

        public static Dictionary<string, User> LoadUsers()
        {
            var usersDataDir = DataDir + "users.json";
            var jsonString = File.ReadAllText(usersDataDir);
            return JsonSerializer.Deserialize<Dictionary<string, User>>(jsonString);
        }

        public static void SaveUsers(Dictionary<string, User> usersDict)
        {
            var usersDataDir = DataDir + "users.json";
            var usersString = JsonSerializer.Serialize(usersDict);
            File.WriteAllText(usersDataDir, usersString);
        }

        public static User LogIn(string username, string password, Dictionary<string, User> usersDict)
        {
            if (usersDict.ContainsKey(username))
            {
                var user = usersDict[username];
                if (user.Password == password)
                {
                    LoggedInUser = user;
                    return user;
                }
            }

            return null;
        }

        public static void SaveCollaborators(Dictionary<int, SocialGatheringCollaborator> collaboratorsDict)
        {
            var collaboratorsDataDir = DataDir + "collaborators.json";
            var collaboratorsString = JsonSerializer.Serialize(collaboratorsDict);
            File.WriteAllText(collaboratorsDataDir, collaboratorsString);
        }

        public static Dictionary<int, SocialGatheringCollaborator> LoadCollaborators()
        {
            var collaboratorsDataDir = DataDir + "collaborators.json";
            var jsonString = File.ReadAllText(collaboratorsDataDir);
            return JsonSerializer.Deserialize<Dictionary<int, SocialGatheringCollaborator>>(jsonString);
        }

        // pomocna funkcija za testiranje
        public static void GenerateCollaborators()
        {
            var collaboratorsDict = new Dictionary<int, SocialGatheringCollaborator>()
            {
                // ketering
                {0, new SocialGatheringCollaborator()
                {
                    Id = 0,
                    Name = "Fabrika kiflica",
                    Description = "Uvek sveže kiflice\nDomaće kiflice bez aditiva i industrijskog kvasca, uvek sveže, ručno motane i bogato punjene.\nUkoliko se odlučite za 4 ili više kilograma, ne morate da razmišljate o plaćanju dostave, biće besplatna na teritoriji grada. Naravno, ukoliko vam je lakše, vaše spremne i vruće kiflice vas mogu čekati i kod nas u Braće Ribnikar 56. Uverite se sami u kvalitet naših proizvoda."
                }
                },
                {1, new SocialGatheringCollaborator(){
                    Id = 1,
                    Name="Catering+",
                    Description="Profesionalna usluga keteringa\nSveže, brzo, ukusno\nNa tržištu Srbije postojimo od 2003. godine. Od samog osnivanja cilj našeg poslovanja je obezbeđivanje usluga keteringa na najvišem nivou za klijente koji prepoznaju kvalitet. Stoga smo posebnu pažnju posvetili infrastrukturi i organizaciji poslovanja. Posedujemo savremenu kuhinju velikog kapaciteta, opremljenu najkvalitetnijom profesionalnom opremom, koja ispunjava sve HACCP standarde za pripremu hrane.\nZa vas kuvamo i organizujemo, kada planirate:\n-koktele, proslave, poslovne ručkove i svečane večere na lokacijama po želji, uz uslugu profesionalnih i uniformisanih konobara\n-ishranu zaposlenih na dnevnoj bazi(sa pripremom u našoj kuhinji za manji broj ljudi i sa pripremom u kuhinji na lokaciji za veći broj ljudi)\n-organizaciju dnevne dostave pića, sokova, kafe, sanitarnih proizvoda, kao i svih ostalih proizvoda iz maloprodajne lokacije klijenata, po maloprodajnim cenama."
                }
                },
                {2, new SocialGatheringCollaborator(){
                    Id = 2,
                    Name = "Restoran Poco Loco",
                    Description = "Restoran Poco loco otvoren je 2005. godine u samom centru Pančeva na samo 15km od Beograda.\nOdlično uređen i pažljivo dekorisan prostor povezuje gostoprimljivo okruženje, izvrsnu uslugu i savršen spoj ukusa. Mašta uprave kuhinje i naša originalna receptura čine da se izdvajamo od ostalih.\nNaći ćete nas u vašem svakodnevnom izlasku, ali i prilikom organizovanih proslava poput krštenja, rođendana, venčanja i drugih vrsta prijema."
                }},
                {3, new SocialGatheringCollaborator(){
                    Id = 3,
                    Name = "Slatkoteka",
                    Description = "Dobro došli. Ovo je Slatkoteka priča. Ugasite svetlo i  ušuškajte se, jer ulazite u čarobni svet krofni, slatkiša i iznenađenja.\nSlatkoteka je ljubavna priča. U toj priči, princeza se zaljubila u krofne i tako je nastala Slatkoteka. Dok princeza čeka svog princa, ona vredno radi i otkriva nove ukuse, oblike i dekoracije koje čine Slatkoteka krofne jedinstvenim.\nKrofne koje nastaju iz ljubavi princeze imaju tu moć da spoje krajnosti u savršeno zadovoljstvo, gde tradicionalni recepti naših baka i najkvalitetniji sastojci i prelivi, uz moderno pakovanje i dekoraciju osvajaju na prvi pogled.\nSlatkoteka krofne osvajaju i naše bake, čije smo tradicionalne recepte pretvorili u poslasticu koja sve vraća u detinjstvo. Sada ovde žive krofne, slatke i slane, svih ukusa i oblika: dostupne svima za degustaciju ili preterivanje, u svakom slučaju za uživanje."
                }},
                {4, new SocialGatheringCollaborator(){
                    Id = 4,
                    Name = "Ketering&Catering",
                    Description = "UKUSNA JELA & KVALITETNI SASTOJCI\nSvako jelo pripremamo s ljubavlju i samo koristeći najkvalitetnije sastojke.\nDosta vremena provodimo kako bi Vašu proslavu učinili jedinstvenom.\nOrganizujemo sve od početka do kraja, a rezultat je ukusna hrana, izvrsna usluga i vrhunska prezentacija."
                }},

                // muzika
                {5, new SocialGatheringCollaborator(){
                    Id = 5,
                    Name = "Medino bend",
                    Description = "MEDINO BEND je nastao 01. septembra 2006. godine u Beogradu. Svojim radom, upornošću i posvećenošću poslu, uspeli smo da izgradimo brend i ime koje je prepoznatljivo! Za Vas sviramo sve vrste svečanosti i nudimo provod po kome ćete nas dugo pamtiti! Pozitivna energija, osmeh i profesionalnost su odlike ljudi koji čine ovaj bend. Svako od nas, na sebi svojstven način ovu priču izdvaja od drugih. www.medinobend.rs je mesto gde mozete pogledati najbolji BEND ZA SVADBE , bend za punoletstva , bend za korporativne proslave - Medino bend"
                }},
                {6, new SocialGatheringCollaborator(){
                    Id = 6,
                    Name = "Aleksandar Sofronijević",
                    Description = "Aleksandar Aca Sofronijević je jedan od trenutno najpopularnijih mladih muzičara u Srbiji, bez čijeg se orkestra ne može zamisliti ni jedan koncert domaćih zvezda narodne i folk muzike, kao ni svadbe, krštenja, i druga slavlja."
                }},
                {7, new SocialGatheringCollaborator(){
                    Id = 7,
                    Name = "Orkestar Endžija Mavrića",
                    Description = "Endži Mavrić sa svojom muzičkim saradnicima često gostuje u Grand emsijama. Sa druge strane pored brojnih gostovanja bio je jedan od aranžera na projektu Zvezde Granda Hitovi Za Sva Vremena.\nOrkestar Endžija Mavrića možete angažovati za različite tipova veselja, kako u Srbiji tako širom Evrope."
                }},
                {8, new SocialGatheringCollaborator(){
                    Id = 8,
                    Name = "Negro bend",
                    Description = "NEGRO je bend za Vaše proslave, bend za svadbe, bend za žurke, bend za prve i 18-te rođendane, bend za korporativne proslave, bend za klubove."
                }},
                {9, new SocialGatheringCollaborator(){
                    Id = 9,
                    Name = "Očajne Domaćice bend",
                    Description = "Bend je nastao početkom 2011. godine kao realizacija originalne ideje o isključivo ženskom bendu.\nNekoliko muzički školovanih devojaka se skupilo i krenulo u izvanredan projekat koji ubrzo postiže veliki uspeh.\nDanas nastupaju svuda po Srbiji, ali se ne zadržavaju samo u okviru njenih granica, već nastupaju i po bivšim republikama Jugoslavije, kao i u Evropi."
                }},

                // prostor
                {10, new SocialGatheringCollaborator(){
                    Id = 10,
                    Name = "Hotel Park Novi Sad",
                    Description = "Hotel Park se nalazi u Novom Sadu, na ivici velikog parka u mirnom okruženju i u neposrednoj blizini centra grada. Hotel je u potpunosti klimatizovan i nudi elegantno uređen smeštaj sa besplatnim internetom i kablovskom televizijom. U pojedinim smeštajnim jedinicama je dostupna hidromasažna kada.\nU restoranu Park koji ima terasu za najviše 500 ljudi služe se jela internacionalne i domaće kuhinje. Ekskluzivni restoran Tito ima šarmantni ambijent. U njemu je boravio maršal Tito 1977. godine.\nVelnes centar hotela Park nudi hidromasažnu kadu, zatvoreni bazen, saune i raznovrsne masažne tretmane. Gostima su na raspolaganju fitnes sadržaji i programi vežbanja sa ličnim trenerom. Kazino i 2 noćna kluba se takođe nalaze u okviru objekta.\nHotel Park se nalazi na pešačkoj udaljenosti od Novosadskog sajma. U okviru objekta gosti mogu koristiti besplatan parking sa obezbeđenjem. Aerodromski prevoz se može organizovati uz doplatu."
                }},
                {11, new SocialGatheringCollaborator(){
                    Id = 11,
                    Name = "HOTEL BALAŠEVIĆ Velika Svečana Sala",
                    Description = "Dugu tradiciju koju ima restoran za venčanja Balašević upotpunjavaju fleksibilno i predusretljivo osoblje, gostoljubiva atmosfera i romantični ambijent. Kompletan restoran za venčanja je klimatizovan i izuzetno akustičan tako da je svaki deo restorana ispunjen sjajnom atmosferom. Enterijer je elegantan i komforan, spreman da svojom modernim standardima izađe u susret svim željama gostiju."
                }},
                {12, new SocialGatheringCollaborator(){
                    Id = 12,
                    Name = "AZZARO Black Club",
                    Description = "Azzaro BLACK klub je zbog svoje kompaktnosti izuzetno prilagodljiv da ispuni zahteve i najprobirljivijih klijenata. Omogućava organizaciju velike svadbe za 270 ljudi, ali se lako može adaptirati za venčanja manjeg obima. Prostrani šank, predivan plesni podijum i zidovi sa našom karakterističnom šarom, osnovna su obeležja ovog prostora. Uz prostor koji predstavlja spoj moderne funkcionalnosti i estetike, opremljen najsavremenijim ozvučenjem i profesionalnom rasvetom za audio-vizuelnu čaroliju, nudimo vrhunsku uslugu našeg ljubaznog osoblja i nezaboravni ketering. Elegantni okrugli stolovi, vrhunski odabrano posuđe i pribor, visokokvalitetni stolnjaci i navlake na stolicama sa bojama mašni kakve zamislite čine da svaka svečanost ostavi utisak prefinjenosti. Azzaro BLACK je specijalizovani restoran za svadbe, proslave i žurke.\nBašta restorana Black nalazi se pored jezera na Adi, ima divan pogled, a sređena je u elegantnom i luksuznom stilu, sa vrhunskom rasvetom, zvezdanim nebom i sasvim je prilagodljiva za sve dekoracije koje želite."
                }},
                {13, new SocialGatheringCollaborator(){
                    Id = 13,
                    Name = "Vidikovac",
                    Description = "Na obroncima Fruške gore, kao na ostrvu usred mora, udobno je smeštena svečana sala kompleksa Vidikovac.\nSvečana sala Vidikovac, kapaciteta do 280 mesta, nudi vam bajkovito dekorisan ambijent, ukusnu hranu i ljubazno osoblje koji će početak vašeg novog života učiniti posebnim. Ono što Vidikovac salu čini tako autentičnom jeste nesvakidašnji eneterijer i očaravajući pogled na Dunav i Novi Sad.\nOvde će vas dočekati tim profesionalaca spreman da vašu svečanost organizuje do detalja.\nU prelepoj bašti kompleksa Vidikovac možete organizovati romantično građansko venčanje. Mladencima i gostima je na raspolaganju nekoliko apartmana-bungalova, od kojih je jedan kreiran isključivo za mladence.\nSvečana sala Vidikovac pravi je izbor za sve one koji žele dan za pamćenje."
                }},
                {14, new SocialGatheringCollaborator(){
                    Id = 14,
                    Name = "PREMIER PREZIDENT HOTEL",
                    Description = "Grandiozna Balska Dvorana Hotela Premier Prezident***** u Sremskim Karlovcima je idealno rešenje ukoliko želite da na profesionalan način organizujete svadbu ili neki drugi svečani događaj koji ćete zasigurno pamtiti celoga života.\nSala je klimatizovana, kapaciteta do 130 mesta, a posebnu draž čini njena barokna arhitektura, koja nikoga neće ostaviti ravnodušnim.\nHotel raspolaže sa 18 luksuznih soba i apartmana za smeštaj gostiju, a posebnu pogodnost koju nude našim mladencima je specijalno ukrašen Premier Prezident apartman sa jacuzzijem za prvu bračnu noć."
                }},
            };
            SaveCollaborators(collaboratorsDict);

            var collaborators2 = LoadCollaborators();
            System.Console.WriteLine();
        }

        static string GetUserImagePath(string userName)
        {
            return DataDir + "img" + Path.DirectorySeparatorChar + "users" + Path.DirectorySeparatorChar + userName + ".jpg";
        }

        static string GetUserImagePath(User user)
        {
            return DataDir + "img" + Path.DirectorySeparatorChar + "users" + Path.DirectorySeparatorChar + user.Username + ".jpg";
        }

        static string GetCollaboratorImagePath(int collaboratorID)
        {
            return DataDir + "img" + Path.DirectorySeparatorChar + "collaborators" + Path.DirectorySeparatorChar + collaboratorID + ".jpg";
        }

        static string GetCollaboratorImagePath(SocialGatheringCollaborator collaborator)
        {
            return DataDir + "img" + Path.DirectorySeparatorChar + "collaborators" + Path.DirectorySeparatorChar + collaborator.Id + ".jpg";
        }

        static int GenerateCollaboratorID()
        {
            return Collaborators.Count;
        }

        static List<User> searchUsers(string searchTerm)
        {
            var searchTerm2 = searchTerm.ToLower();
            var users = from user in Users.Values
                        where (user.Address.City.ToLower().Contains(searchTerm2)
                        || user.Address.StreetAddress.Contains(searchTerm2)
                        || user.BirthDate.ToString().ToLower().Contains(searchTerm2)
                        || user.PhoneNumber.ToLower().Contains(searchTerm2)
                        || user.FirstName.ToLower().Contains(searchTerm2)
                        || user.LastName.ToLower().Contains(searchTerm2)
                        || user.Username.ToLower().Contains(searchTerm2)
                        || user.Email.ToLower().Contains(searchTerm2)
                        )
                        select user;
            return new List<User>(users);
        }

        static List<SocialGatheringCollaborator> searchCollaborators(string searchTerm)
        {
            var searchTerm2 = searchTerm.ToLower();
            var collaborators = from collaborator in Collaborators.Values
                                where (collaborator.Name.ToLower().Contains(searchTerm2) || collaborator.Description.ToLower().Contains(searchTerm2))
                                select collaborator;
            return new List<SocialGatheringCollaborator>(collaborators);
        }

        static List<User> getUsersOfType(UserType userType)
        {
            var users = from user in Users.Values
                        where user.UserType == userType
                        select user;
            return new List<User>(users);
        }
    }
}