
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace MusicStoreUI.Models
{
    public static class SampleData
    {
        const string defaultAdminUserName = "DefaultAdminUsername";
        const string defaultAdminPassword = "DefaultAdminPassword";


        public static void InitializeAccountsDatabase(IServiceProvider serviceProvider, IConfiguration configuration)
        {
            if (ShouldDropCreateDatabase())
            {

                using (var serviceScope = serviceProvider.GetRequiredService<IServiceScopeFactory>().CreateScope())
                {
                    var db = serviceScope.ServiceProvider.GetService<AccountsContext>();
                    db.Database.EnsureCreated();
                    CreateAdminUser(serviceProvider, configuration).Wait();
                }
            }

            BuildFallbackData();
        }

        private static async Task CreateAdminUser(IServiceProvider serviceProvider, IConfiguration configuration)
        {
            const string adminRole = "Administrator";

            var userManager = serviceProvider.GetService<UserManager<ApplicationUser>>();
            var roleManager = serviceProvider.GetService<RoleManager<ApplicationRole>>();
            if (!await roleManager.RoleExistsAsync(adminRole))
            {
                await roleManager.CreateAsync(new ApplicationRole(adminRole));
            }

            var user = await userManager.FindByNameAsync(configuration[defaultAdminUserName]);
            if (user == null)
            {
                user = new ApplicationUser { UserName = configuration[defaultAdminUserName] };
                await userManager.CreateAsync(user, configuration[defaultAdminPassword]);
                await userManager.AddToRoleAsync(user, adminRole);
                await userManager.AddClaimAsync(user, new Claim("ManageStore", "Allowed"));
            }

#if TESTING
            var envPerfLab = configuration["PERF_LAB"];
            if (envPerfLab == "true")
            {
                for (int i = 0; i < 100; ++i)
                {
                    var email = string.Format("User{0:D3}@example.com", i);
                    var normalUser = await userManager.FindByEmailAsync(email);
                    if (normalUser == null)
                    {
                        await userManager.CreateAsync(new ApplicationUser { UserName = email, Email = email }, "Password~!1");
                    }
                }
            }
#endif
        }
        private static bool ShouldDropCreateDatabase()
        {
            string index = Environment.GetEnvironmentVariable("CF_INSTANCE_INDEX");
            if (string.IsNullOrEmpty(index))
            {
                return true;
            }
            int indx = -1;
            if (int.TryParse(index, out indx))
            {
                if (indx > 0) return false;
            }
            return true;
        }

        const string imgUrl = "~/Images/placeholder.png";
        public static string ImageUrl { get { return imgUrl;  } }
        private  static Album[] GetAlbums()
        {
            var genres = Genres;
            var artists = Artists;
            int id = 99990;

            var albums = new Album[]
            {
                new Album { AlbumId = ++id, Title = "The Best Of The Men At Work", Genre = genres["Pop"], Price = 8.99M, Artist = artists["Men At Work"], AlbumArtUrl = imgUrl },
                new Album { AlbumId = ++id, Title = "...And Justice For All", Genre = genres["Metal"], Price = 8.99M, Artist = artists["Metallica"], AlbumArtUrl = imgUrl },
                //new Album { AlbumId = ++id, Title = "עד גבול האור", Genre = genres["World"], Price = 8.99M, Artist = artists["אריק אינשטיין"], AlbumArtUrl = imgUrl },
                new Album { AlbumId = ++id, Title = "Black Light Syndrome", Genre = genres["Rock"], Price = 8.99M, Artist = artists["Terry Bozzio, Tony Levin & Steve Stevens"], AlbumArtUrl = imgUrl },
                new Album { AlbumId = ++id, Title = "10,000 Days", Genre = genres["Rock"], Price = 8.99M, Artist = artists["Tool"], AlbumArtUrl = imgUrl },
                new Album { AlbumId = ++id, Title = "11i", Genre = genres["Electronic"], Price = 8.99M, Artist = artists["Supreme Beings of Leisure"], AlbumArtUrl = imgUrl },
                new Album { AlbumId = ++id, Title = "1960", Genre = genres["Indie"], Price = 8.99M, Artist = artists["Soul-Junk"], AlbumArtUrl = imgUrl },
                new Album { AlbumId = ++id, Title = "4x4=12 ", Genre = genres["Electronic"], Price = 8.99M, Artist = artists["deadmau5"], AlbumArtUrl = imgUrl },
                new Album { AlbumId = ++id, Title = "A Copland Celebration, Vol. I", Genre = genres["Classical"], Price = 8.99M, Artist = artists["London Symphony Orchestra"], AlbumArtUrl = imgUrl },
                new Album { AlbumId = ++id, Title = "A Lively Mind", Genre = genres["Electronic"], Price = 8.99M, Artist = artists["Paul Oakenfold"], AlbumArtUrl = imgUrl },
                new Album { AlbumId = ++id, Title = "A Matter of Life and Death", Genre = genres["Rock"], Price = 8.99M, Artist = artists["Iron Maiden"], AlbumArtUrl = imgUrl },
                new Album { AlbumId = ++id, Title = "A Real Dead One", Genre = genres["Metal"], Price = 8.99M, Artist = artists["Iron Maiden"], AlbumArtUrl = imgUrl },
                new Album { AlbumId = ++id, Title = "A Real Live One", Genre = genres["Metal"], Price = 8.99M, Artist = artists["Iron Maiden"], AlbumArtUrl = imgUrl },
                new Album { AlbumId = ++id, Title = "A Rush of Blood to the Head", Genre = genres["Rock"], Price = 8.99M, Artist = artists["Coldplay"], AlbumArtUrl = imgUrl },
                new Album { AlbumId = ++id, Title = "A Soprano Inspired", Genre = genres["Classical"], Price = 8.99M, Artist = artists["Britten Sinfonia, Ivor Bolton & Lesley Garrett"], AlbumArtUrl = imgUrl },
                new Album { AlbumId = ++id, Title = "A Winter Symphony", Genre = genres["Classical"], Price = 8.99M, Artist = artists["Sarah Brightman"], AlbumArtUrl = imgUrl },
                new Album { AlbumId = ++id, Title = "Abbey Road", Genre = genres["Rock"], Price = 8.99M, Artist = artists["The Beatles"], AlbumArtUrl = imgUrl },
                new Album { AlbumId = ++id, Title = "Ace Of Spades", Genre = genres["Metal"], Price = 8.99M, Artist = artists["Motörhead"], AlbumArtUrl = imgUrl },
                new Album { AlbumId = ++id, Title = "Achtung Baby", Genre = genres["Rock"], Price = 8.99M, Artist = artists["U2"], AlbumArtUrl = imgUrl },
                new Album { AlbumId = ++id, Title = "Acústico MTV", Genre = genres["Latin"], Price = 8.99M, Artist = artists["Os Paralamas Do Sucesso"], AlbumArtUrl = imgUrl },
                new Album { AlbumId = ++id, Title = "Adams, John: The Chairman Dances", Genre = genres["Classical"], Price = 8.99M, Artist = artists["Edo de Waart & San Francisco Symphony"], AlbumArtUrl = imgUrl },
                new Album { AlbumId = ++id, Title = "Adrenaline", Genre = genres["Rock"], Price = 8.99M, Artist = artists["Deftones"], AlbumArtUrl = imgUrl },
                new Album { AlbumId = ++id, Title = "Ænima", Genre = genres["Metal"], Price = 8.99M, Artist = artists["Tool"], AlbumArtUrl = imgUrl },
                new Album { AlbumId = ++id, Title = "Afrociberdelia", Genre = genres["Latin"], Price = 8.99M, Artist = artists["Chico Science & Nação Zumbi"], AlbumArtUrl = imgUrl },
                new Album { AlbumId = ++id, Title = "After the Goldrush", Genre = genres["Rock"], Price = 8.99M, Artist = artists["Neil Young"], AlbumArtUrl = imgUrl },
                new Album { AlbumId = ++id, Title = "Airdrawn Dagger", Genre = genres["Electronic"], Price = 8.99M, Artist = artists["Sasha"], AlbumArtUrl = imgUrl },
                new Album { AlbumId = ++id, Title = "Album AlbumId = ++id, Title Goes Here", Genre = genres["Electronic"], Price = 8.99M, Artist = artists["deadmau5"], AlbumArtUrl = imgUrl },
                new Album { AlbumId = ++id, Title = "Alcohol Fueled Brewtality Live! [Disc 1]", Genre = genres["Metal"], Price = 8.99M, Artist = artists["Black Label Society"], AlbumArtUrl = imgUrl },
                new Album { AlbumId = ++id, Title = "Alcohol Fueled Brewtality Live! [Disc 2]", Genre = genres["Metal"], Price = 8.99M, Artist = artists["Black Label Society"], AlbumArtUrl = imgUrl },
                new Album { AlbumId = ++id, Title = "Alive 2007", Genre = genres["Electronic"], Price = 8.99M, Artist = artists["Daft Punk"], AlbumArtUrl = imgUrl },
                new Album { AlbumId = ++id, Title = "All I Ask of You", Genre = genres["Classical"], Price = 8.99M, Artist = artists["Sarah Brightman"], AlbumArtUrl = imgUrl },
                new Album { AlbumId = ++id, Title = "Amen (So Be It)", Genre = genres["Rock"], Price = 8.99M, Artist = artists["Paddy Casey"], AlbumArtUrl = imgUrl },
                new Album { AlbumId = ++id, Title = "Animal Vehicle", Genre = genres["Pop"], Price = 8.99M, Artist = artists["The Axis of Awesome"], AlbumArtUrl = imgUrl },
                new Album { AlbumId = ++id, Title = "Ao Vivo [IMPORT]", Genre = genres["Latin"], Price = 8.99M, Artist = artists["Zeca Pagodinho"], AlbumArtUrl = imgUrl },
                new Album { AlbumId = ++id, Title = "Apocalyptic Love", Genre = genres["Rock"], Price = 8.99M, Artist = artists["Slash"], AlbumArtUrl = imgUrl },
                new Album { AlbumId = ++id, Title = "Appetite for Destruction", Genre = genres["Rock"], Price = 8.99M, Artist = artists["Guns N' Roses"], AlbumArtUrl = imgUrl },
                new Album { AlbumId = ++id, Title = "Are You Experienced?", Genre = genres["Rock"], Price = 8.99M, Artist = artists["Jimi Hendrix"], AlbumArtUrl = imgUrl },
                new Album { AlbumId = ++id, Title = "Arquivo II", Genre = genres["Latin"], Price = 8.99M, Artist = artists["Os Paralamas Do Sucesso"], AlbumArtUrl = imgUrl },
                new Album { AlbumId = ++id, Title = "Arquivo Os Paralamas Do Sucesso", Genre = genres["Latin"], Price = 8.99M, Artist = artists["Os Paralamas Do Sucesso"], AlbumArtUrl = imgUrl },
                new Album { AlbumId = ++id, Title = "A-Sides", Genre = genres["Rock"], Price = 8.99M, Artist = artists["Soundgarden"], AlbumArtUrl = imgUrl },
                new Album { AlbumId = ++id, Title = "Audioslave", Genre = genres["Rock"], Price = 8.99M, Artist = artists["Audioslave"], AlbumArtUrl = imgUrl },
                new Album { AlbumId = ++id, Title = "Automatic for the People", Genre = genres["Alternative"], Price = 8.99M, Artist = artists["R.E.M."], AlbumArtUrl = imgUrl },
                new Album { AlbumId = ++id, Title = "Axé Bahia 2001", Genre = genres["Pop"], Price = 8.99M, Artist = artists["Various Artists"], AlbumArtUrl = imgUrl },
                new Album { AlbumId = ++id, Title = "Babel", Genre = genres["Alternative"], Price = 8.99M, Artist = artists["Mumford & Sons"], AlbumArtUrl = imgUrl },
                new Album { AlbumId = ++id, Title = "Bach: Goldberg Variations", Genre = genres["Classical"], Price = 8.99M, Artist = artists["Wilhelm Kempff"], AlbumArtUrl = imgUrl },
                new Album { AlbumId = ++id, Title = "Bach: The Brandenburg Concertos", Genre = genres["Classical"], Price = 8.99M, Artist = artists["Orchestra of The Age of Enlightenment"], AlbumArtUrl = imgUrl },
                new Album { AlbumId = ++id, Title = "Bach: The Cello Suites", Genre = genres["Classical"], Price = 8.99M, Artist = artists["Yo-Yo Ma"], AlbumArtUrl = imgUrl },
                new Album { AlbumId = ++id, Title = "Bach: Toccata & Fugue in D Minor", Genre = genres["Classical"], Price = 8.99M, Artist = artists["Ton Koopman"], AlbumArtUrl = imgUrl },
                new Album { AlbumId = ++id, Title = "Bad Motorfinger", Genre = genres["Rock"], Price = 8.99M, Artist = artists["Soundgarden"], AlbumArtUrl = imgUrl },
                new Album { AlbumId = ++id, Title = "Balls to the Wall", Genre = genres["Rock"], Price = 8.99M, Artist = artists["Accept"], AlbumArtUrl = imgUrl },
                new Album { AlbumId = ++id, Title = "Banadeek Ta'ala", Genre = genres["World"], Price = 8.99M, Artist = artists["Amr Diab"], AlbumArtUrl = imgUrl },
                new Album { AlbumId = ++id, Title = "Barbie Girl", Genre = genres["Pop"], Price = 8.99M, Artist = artists["Aqua"], AlbumArtUrl = imgUrl },
                new Album { AlbumId = ++id, Title = "Bark at the Moon (Remastered)", Genre = genres["Rock"], Price = 8.99M, Artist = artists["Ozzy Osbourne"], AlbumArtUrl = imgUrl },
                new Album { AlbumId = ++id, Title = "Bartok: Violin & Viola Concertos", Genre = genres["Classical"], Price = 8.99M, Artist = artists["Yehudi Menuhin"], AlbumArtUrl = imgUrl },
                new Album { AlbumId = ++id, Title = "Barulhinho Bom", Genre = genres["Latin"], Price = 8.99M, Artist = artists["Marisa Monte"], AlbumArtUrl = imgUrl },
                new Album { AlbumId = ++id, Title = "BBC Sessions [Disc 1] [Live]", Genre = genres["Rock"], Price = 8.99M, Artist = artists["Led Zeppelin"], AlbumArtUrl = imgUrl },
                new Album { AlbumId = ++id, Title = "BBC Sessions [Disc 2] [Live]", Genre = genres["Rock"], Price = 8.99M, Artist = artists["Led Zeppelin"], AlbumArtUrl = imgUrl },
                new Album { AlbumId = ++id, Title = "Be Here Now", Genre = genres["Rock"], Price = 8.99M, Artist = artists["Oasis"], AlbumArtUrl = imgUrl },
                new Album { AlbumId = ++id, Title = "Bedrock 11 Compiled & Mixed", Genre = genres["Electronic"], Price = 8.99M, Artist = artists["John Digweed"], AlbumArtUrl = imgUrl },
                new Album { AlbumId = ++id, Title = "Berlioz: Symphonie Fantastique", Genre = genres["Classical"], Price = 8.99M, Artist = artists["Michael Tilson Thomas"], AlbumArtUrl = imgUrl },
                new Album { AlbumId = ++id, Title = "Beyond Good And Evil", Genre = genres["Rock"], Price = 8.99M, Artist = artists["The Cult"], AlbumArtUrl = imgUrl },
                new Album { AlbumId = ++id, Title = "Big Bad Wolf ", Genre = genres["Electronic"], Price = 8.99M, Artist = artists["Armand Van Helden"], AlbumArtUrl = imgUrl },
                new Album { AlbumId = ++id, Title = "Big Ones", Genre = genres["Rock"], Price = 8.99M, Artist = artists["Aerosmith"], AlbumArtUrl = imgUrl },
                new Album { AlbumId = ++id, Title = "Black Album", Genre = genres["Metal"], Price = 8.99M, Artist = artists["Metallica"], AlbumArtUrl = imgUrl },
                new Album { AlbumId = ++id, Title = "Black Sabbath Vol. 4 (Remaster)", Genre = genres["Metal"], Price = 8.99M, Artist = artists["Black Sabbath"], AlbumArtUrl = imgUrl },
                new Album { AlbumId = ++id, Title = "Black Sabbath", Genre = genres["Metal"], Price = 8.99M, Artist = artists["Black Sabbath"], AlbumArtUrl = imgUrl },
                new Album { AlbumId = ++id, Title = "Black", Genre = genres["Metal"], Price = 8.99M, Artist = artists["Metallica"], AlbumArtUrl = imgUrl },
                new Album { AlbumId = ++id, Title = "Blackwater Park", Genre = genres["Metal"], Price = 8.99M, Artist = artists["Opeth"], AlbumArtUrl = imgUrl },
                new Album { AlbumId = ++id, Title = "Blizzard of Ozz", Genre = genres["Rock"], Price = 8.99M, Artist = artists["Ozzy Osbourne"], AlbumArtUrl = imgUrl },
                new Album { AlbumId = ++id, Title = "Blood", Genre = genres["Rock"], Price = 8.99M, Artist = artists["In This Moment"], AlbumArtUrl = imgUrl },
                new Album { AlbumId = ++id, Title = "Blue Moods", Genre = genres["Jazz"], Price = 8.99M, Artist = artists["Incognito"], AlbumArtUrl = imgUrl },
                new Album { AlbumId = ++id, Title = "Blue", Genre = genres["Rock"], Price = 8.99M, Artist = artists["Weezer"], AlbumArtUrl = imgUrl },
                new Album { AlbumId = ++id, Title = "Bongo Fury", Genre = genres["Rock"], Price = 8.99M, Artist = artists["Frank Zappa & Captain Beefheart"], AlbumArtUrl = imgUrl },
                new Album { AlbumId = ++id, Title = "Boys & Girls", Genre = genres["Rock"], Price = 8.99M, Artist = artists["Alabama Shakes"], AlbumArtUrl = imgUrl },
                new Album { AlbumId = ++id, Title = "Brave New World", Genre = genres["Rock"], Price = 8.99M, Artist = artists["Iron Maiden"], AlbumArtUrl = imgUrl },
                new Album { AlbumId = ++id, Title = "B-Sides 1980-1990", Genre = genres["Rock"], Price = 8.99M, Artist = artists["U2"], AlbumArtUrl = imgUrl },
                new Album { AlbumId = ++id, Title = "Bunkka", Genre = genres["Electronic"], Price = 8.99M, Artist = artists["Paul Oakenfold"], AlbumArtUrl = imgUrl },
                new Album { AlbumId = ++id, Title = "By The Way", Genre = genres["Rock"], Price = 8.99M, Artist = artists["Red Hot Chili Peppers"], AlbumArtUrl = imgUrl },
                new Album { AlbumId = ++id, Title = "Cake: B-Sides and Rarities", Genre = genres["Electronic"], Price = 8.99M, Artist = artists["Cake"], AlbumArtUrl = imgUrl },
                new Album { AlbumId = ++id, Title = "Californication", Genre = genres["Rock"], Price = 8.99M, Artist = artists["Red Hot Chili Peppers"], AlbumArtUrl = imgUrl },
                new Album { AlbumId = ++id, Title = "Carmina Burana", Genre = genres["Classical"], Price = 8.99M, Artist = artists["Boston Symphony Orchestra & Seiji Ozawa"], AlbumArtUrl = imgUrl },
                new Album { AlbumId = ++id, Title = "Carried to Dust (Bonus Track Version)", Genre = genres["Electronic"], Price = 8.99M, Artist = artists["Calexico"], AlbumArtUrl = imgUrl },
                new Album { AlbumId = ++id, Title = "Carry On", Genre = genres["Electronic"], Price = 8.99M, Artist = artists["Chris Cornell"], AlbumArtUrl = imgUrl },
                new Album { AlbumId = ++id, Title = "Cássia Eller - Sem Limite [Disc 1]", Genre = genres["Latin"], Price = 8.99M, Artist = artists["Cássia Eller"], AlbumArtUrl = imgUrl },
                new Album { AlbumId = ++id, Title = "Chemical Wedding", Genre = genres["Metal"], Price = 8.99M, Artist = artists["Bruce Dickinson"], AlbumArtUrl = imgUrl },
                new Album { AlbumId = ++id, Title = "Chill: Brazil (Disc 1)", Genre = genres["Latin"], Price = 8.99M, Artist = artists["Marcos Valle"], AlbumArtUrl = imgUrl },
                new Album { AlbumId = ++id, Title = "Chill: Brazil (Disc 2)", Genre = genres["Latin"], Price = 8.99M, Artist = artists["Antônio Carlos Jobim"], AlbumArtUrl = imgUrl },
                new Album { AlbumId = ++id, Title = "Chocolate Starfish And The Hot Dog Flavored Water", Genre = genres["Rock"], Price = 8.99M, Artist = artists["Limp Bizkit"], AlbumArtUrl = imgUrl },
                new Album { AlbumId = ++id, Title = "Chronicle, Vol. 1", Genre = genres["Rock"], Price = 8.99M, Artist = artists["Creedence Clearwater Revival"], AlbumArtUrl = imgUrl },
                new Album { AlbumId = ++id, Title = "Chronicle, Vol. 2", Genre = genres["Rock"], Price = 8.99M, Artist = artists["Creedence Clearwater Revival"], AlbumArtUrl = imgUrl },
                new Album { AlbumId = ++id, Title = "Ciao, Baby", Genre = genres["Rock"], Price = 8.99M, Artist = artists["TheStart"], AlbumArtUrl = imgUrl },
                new Album { AlbumId = ++id, Title = "Cidade Negra - Hits", Genre = genres["Latin"], Price = 8.99M, Artist = artists["Cidade Negra"], AlbumArtUrl = imgUrl },
                new Album { AlbumId = ++id, Title = "Classic Munkle: Turbo Edition", Genre = genres["Electronic"], Price = 8.99M, Artist = artists["Munkle"], AlbumArtUrl = imgUrl },
                new Album { AlbumId = ++id, Title = "Classics: The Best of Sarah Brightman", Genre = genres["Classical"], Price = 8.99M, Artist = artists["Sarah Brightman"], AlbumArtUrl = imgUrl },
                new Album { AlbumId = ++id, Title = "Coda", Genre = genres["Rock"], Price = 8.99M, Artist = artists["Led Zeppelin"], AlbumArtUrl = imgUrl },
                new Album { AlbumId = ++id, Title = "Come Away With Me", Genre = genres["Jazz"], Price = 8.99M, Artist = artists["Norah Jones"], AlbumArtUrl = imgUrl },
                new Album { AlbumId = ++id, Title = "Come Taste The Band", Genre = genres["Rock"], Price = 8.99M, Artist = artists["Deep Purple"], AlbumArtUrl = imgUrl },
                new Album { AlbumId = ++id, Title = "Comfort Eagle", Genre = genres["Alternative"], Price = 8.99M, Artist = artists["Cake"], AlbumArtUrl = imgUrl },
                new Album { AlbumId = ++id, Title = "Common Reaction", Genre = genres["Rock"], Price = 8.99M, Artist = artists["Uh Huh Her "], AlbumArtUrl = imgUrl },
                new Album { AlbumId = ++id, Title = "Compositores", Genre = genres["Rock"], Price = 8.99M, Artist = artists["O Terço"], AlbumArtUrl = imgUrl },
                new Album { AlbumId = ++id, Title = "Contraband", Genre = genres["Rock"], Price = 8.99M, Artist = artists["Velvet Revolver"], AlbumArtUrl = imgUrl },
                new Album { AlbumId = ++id, Title = "Core", Genre = genres["Rock"], Price = 8.99M, Artist = artists["Stone Temple Pilots"], AlbumArtUrl = imgUrl },
                new Album { AlbumId = ++id, Title = "Cornerstone", Genre = genres["Rock"], Price = 8.99M, Artist = artists["Styx"], AlbumArtUrl = imgUrl },
                new Album { AlbumId = ++id, Title = "Cosmicolor", Genre = genres["Rap"], Price = 8.99M, Artist = artists["M-Flo"], AlbumArtUrl = imgUrl },
                new Album { AlbumId = ++id, Title = "Cross", Genre = genres["Electronic"], Price = 8.99M, Artist = artists["Justice"], AlbumArtUrl = imgUrl },
                new Album { AlbumId = ++id, Title = "Culture of Fear", Genre = genres["Electronic"], Price = 8.99M, Artist = artists["Thievery Corporation"], AlbumArtUrl = imgUrl },
                new Album { AlbumId = ++id, Title = "Da Lama Ao Caos", Genre = genres["Latin"], Price = 8.99M, Artist = artists["Chico Science & Nação Zumbi"], AlbumArtUrl = imgUrl },
                new Album { AlbumId = ++id, Title = "Dakshina", Genre = genres["World"], Price = 8.99M, Artist = artists["Deva Premal"], AlbumArtUrl = imgUrl },
                new Album { AlbumId = ++id, Title = "Dark Side of the Moon", Genre = genres["Rock"], Price = 8.99M, Artist = artists["Pink Floyd"], AlbumArtUrl = imgUrl },
                new Album { AlbumId = ++id, Title = "Death Magnetic", Genre = genres["Metal"], Price = 8.99M, Artist = artists["Metallica"], AlbumArtUrl = imgUrl },
                new Album { AlbumId = ++id, Title = "Deep End of Down", Genre = genres["Rock"], Price = 8.99M, Artist = artists["Above the Fold"], AlbumArtUrl = imgUrl },
                new Album { AlbumId = ++id, Title = "Deep Purple In Rock", Genre = genres["Rock"], Price = 8.99M, Artist = artists["Deep Purple"], AlbumArtUrl = imgUrl },
                new Album { AlbumId = ++id, Title = "Deixa Entrar", Genre = genres["Latin"], Price = 8.99M, Artist = artists["Falamansa"], AlbumArtUrl = imgUrl },
                new Album { AlbumId = ++id, Title = "Deja Vu", Genre = genres["Rock"], Price = 8.99M, Artist = artists["Crosby, Stills, Nash, and Young"], AlbumArtUrl = imgUrl },
                new Album { AlbumId = ++id, Title = "Di Korpu Ku Alma", Genre = genres["World"], Price = 8.99M, Artist = artists["Lura"], AlbumArtUrl = imgUrl },
                new Album { AlbumId = ++id, Title = "Diary of a Madman (Remastered)", Genre = genres["Rock"], Price = 8.99M, Artist = artists["Ozzy Osbourne"], AlbumArtUrl = imgUrl },
                new Album { AlbumId = ++id, Title = "Diary of a Madman", Genre = genres["Metal"], Price = 8.99M, Artist = artists["Ozzy Osbourne"], AlbumArtUrl = imgUrl },
                new Album { AlbumId = ++id, Title = "Dirt", Genre = genres["Rock"], Price = 8.99M, Artist = artists["Alice in Chains"], AlbumArtUrl = imgUrl },
                new Album { AlbumId = ++id, Title = "Diver Down", Genre = genres["Rock"], Price = 8.99M, Artist = artists["Van Halen"], AlbumArtUrl = imgUrl },
                new Album { AlbumId = ++id, Title = "Djavan Ao Vivo - Vol. 02", Genre = genres["Latin"], Price = 8.99M, Artist = artists["Djavan"], AlbumArtUrl = imgUrl },
                new Album { AlbumId = ++id, Title = "Djavan Ao Vivo - Vol. 1", Genre = genres["Latin"], Price = 8.99M, Artist = artists["Djavan"], AlbumArtUrl = imgUrl },
                new Album { AlbumId = ++id, Title = "Drum'n'bass for Papa", Genre = genres["Electronic"], Price = 8.99M, Artist = artists["Plug"], AlbumArtUrl = imgUrl },
                new Album { AlbumId = ++id, Title = "Duluth", Genre = genres["Country"], Price = 8.99M, Artist = artists["Trampled By Turtles"], AlbumArtUrl = imgUrl },
                new Album { AlbumId = ++id, Title = "Dummy", Genre = genres["Electronic"], Price = 8.99M, Artist = artists["Portishead"], AlbumArtUrl = imgUrl },
                new Album { AlbumId = ++id, Title = "Duos II", Genre = genres["Latin"], Price = 8.99M, Artist = artists["Luciana Souza/Romero Lubambo"], AlbumArtUrl = imgUrl },
                new Album { AlbumId = ++id, Title = "Earl Scruggs and Friends", Genre = genres["Country"], Price = 8.99M, Artist = artists["Earl Scruggs"], AlbumArtUrl = imgUrl },
                new Album { AlbumId = ++id, Title = "Eden", Genre = genres["Classical"], Price = 8.99M, Artist = artists["Sarah Brightman"], AlbumArtUrl = imgUrl },
                new Album { AlbumId = ++id, Title = "El Camino", Genre = genres["Rock"], Price = 8.99M, Artist = artists["The Black Keys"], AlbumArtUrl = imgUrl },
                new Album { AlbumId = ++id, Title = "Elegant Gypsy", Genre = genres["Jazz"], Price = 8.99M, Artist = artists["Al di Meola"], AlbumArtUrl = imgUrl },
                new Album { AlbumId = ++id, Title = "Elements Of Life", Genre = genres["Electronic"], Price = 8.99M, Artist = artists["Tiësto"], AlbumArtUrl = imgUrl },
                new Album { AlbumId = ++id, Title = "Elis Regina-Minha História", Genre = genres["Latin"], Price = 8.99M, Artist = artists["Elis Regina"], AlbumArtUrl = imgUrl },
                new Album { AlbumId = ++id, Title = "Emergency On Planet Earth", Genre = genres["Rock"], Price = 8.99M, Artist = artists["Jamiroquai"], AlbumArtUrl = imgUrl },
                new Album { AlbumId = ++id, Title = "Emotion", Genre = genres["World"], Price = 8.99M, Artist = artists["Papa Wemba"], AlbumArtUrl = imgUrl },
                new Album { AlbumId = ++id, Title = "English Renaissance", Genre = genres["Classical"], Price = 8.99M, Artist = artists["The King's Singers"], AlbumArtUrl = imgUrl },
                new Album { AlbumId = ++id, Title = "Every Kind of Light", Genre = genres["Rock"], Price = 8.99M, Artist = artists["The Posies"], AlbumArtUrl = imgUrl },
                new Album { AlbumId = ++id, Title = "Faceless", Genre = genres["Metal"], Price = 8.99M, Artist = artists["Godsmack"], AlbumArtUrl = imgUrl },
                new Album { AlbumId = ++id, Title = "Facelift", Genre = genres["Rock"], Price = 8.99M, Artist = artists["Alice in Chains"], AlbumArtUrl = imgUrl },
                new Album { AlbumId = ++id, Title = "Fair Warning", Genre = genres["Rock"], Price = 8.99M, Artist = artists["Van Halen"], AlbumArtUrl = imgUrl },
                new Album { AlbumId = ++id, Title = "Fear of a Black Planet", Genre = genres["Rap"], Price = 8.99M, Artist = artists["Public Enemy"], AlbumArtUrl = imgUrl },
                new Album { AlbumId = ++id, Title = "Fear Of The Dark", Genre = genres["Rock"], Price = 8.99M, Artist = artists["Iron Maiden"], AlbumArtUrl = imgUrl },
                new Album { AlbumId = ++id, Title = "Feels Like Home", Genre = genres["Jazz"], Price = 8.99M, Artist = artists["Norah Jones"], AlbumArtUrl = imgUrl },
                new Album { AlbumId = ++id, Title = "Fireball", Genre = genres["Rock"], Price = 8.99M, Artist = artists["Deep Purple"], AlbumArtUrl = imgUrl },
                new Album { AlbumId = ++id, Title = "Fly", Genre = genres["Classical"], Price = 8.99M, Artist = artists["Sarah Brightman"], AlbumArtUrl = imgUrl },
                new Album { AlbumId = ++id, Title = "For Those About To Rock We Salute You", Genre = genres["Rock"], Price = 8.99M, Artist = artists["AC/DC"], AlbumArtUrl = imgUrl },
                new Album { AlbumId = ++id, Title = "Four", Genre = genres["Blues"], Price = 8.99M, Artist = artists["Blues Traveler"], AlbumArtUrl = imgUrl },
                new Album { AlbumId = ++id, Title = "Frank", Genre = genres["Pop"], Price = 8.99M, Artist = artists["Amy Winehouse"], AlbumArtUrl = imgUrl },
                new Album { AlbumId = ++id, Title = "Further Down the Spiral", Genre = genres["Electronic"], Price = 8.99M, Artist = artists["Nine Inch Nails"], AlbumArtUrl = imgUrl },
                new Album { AlbumId = ++id, Title = "Garage Inc. (Disc 1)", Genre = genres["Metal"], Price = 8.99M, Artist = artists["Metallica"], AlbumArtUrl = imgUrl },
                new Album { AlbumId = ++id, Title = "Garage Inc. (Disc 2)", Genre = genres["Metal"], Price = 8.99M, Artist = artists["Metallica"], AlbumArtUrl = imgUrl },
                new Album { AlbumId = ++id, Title = "Garbage", Genre = genres["Alternative"], Price = 8.99M, Artist = artists["Garbage"], AlbumArtUrl = imgUrl },
                new Album { AlbumId = ++id, Title = "Good News For People Who Love Bad News", Genre = genres["Indie"], Price = 8.99M, Artist = artists["Modest Mouse"], AlbumArtUrl = imgUrl },
                new Album { AlbumId = ++id, Title = "Gordon", Genre = genres["Alternative"], Price = 8.99M, Artist = artists["Barenaked Ladies"], AlbumArtUrl = imgUrl },
                new Album { AlbumId = ++id, Title = "Górecki: Symphony No. 3", Genre = genres["Classical"], Price = 8.99M, Artist = artists["Adrian Leaper & Doreen de Feis"], AlbumArtUrl = imgUrl },
                new Album { AlbumId = ++id, Title = "Greatest Hits I", Genre = genres["Rock"], Price = 8.99M, Artist = artists["Queen"], AlbumArtUrl = imgUrl },
                new Album { AlbumId = ++id, Title = "Greatest Hits II", Genre = genres["Rock"], Price = 8.99M, Artist = artists["Queen"], AlbumArtUrl = imgUrl },
                new Album { AlbumId = ++id, Title = "Greatest Hits", Genre = genres["Electronic"], Price = 8.99M, Artist = artists["Duck Sauce"], AlbumArtUrl = imgUrl },
                new Album { AlbumId = ++id, Title = "Greatest Hits", Genre = genres["Metal"], Price = 8.99M, Artist = artists["Lenny Kravitz"], AlbumArtUrl = imgUrl },
                new Album { AlbumId = ++id, Title = "Greatest Hits", Genre = genres["Rock"], Price = 8.99M, Artist = artists["Lenny Kravitz"], AlbumArtUrl = imgUrl },
                new Album { AlbumId = ++id, Title = "Greatest Kiss", Genre = genres["Rock"], Price = 8.99M, Artist = artists["Kiss"], AlbumArtUrl = imgUrl },
                new Album { AlbumId = ++id, Title = "Greetings from Michigan", Genre = genres["Indie"], Price = 8.99M, Artist = artists["Sufjan Stevens"], AlbumArtUrl = imgUrl },
                new Album { AlbumId = ++id, Title = "Group Therapy", Genre = genres["Electronic"], Price = 8.99M, Artist = artists["Above & Beyond"], AlbumArtUrl = imgUrl },
                new Album { AlbumId = ++id, Title = "Handel: The Messiah (Highlights)", Genre = genres["Classical"], Price = 8.99M, Artist = artists["Scholars Baroque Ensemble"], AlbumArtUrl = imgUrl },
                new Album { AlbumId = ++id, Title = "Haydn: Symphonies 99 - 104", Genre = genres["Classical"], Price = 8.99M, Artist = artists["Royal Philharmonic Orchestra"], AlbumArtUrl = imgUrl },
                new Album { AlbumId = ++id, Title = "Heart of the Night", Genre = genres["Jazz"], Price = 8.99M, Artist = artists["Spyro Gyra"], AlbumArtUrl = imgUrl },
                new Album { AlbumId = ++id, Title = "Heart On", Genre = genres["Rock"], Price = 8.99M, Artist = artists["The Eagles of Death Metal"], AlbumArtUrl = imgUrl },
                new Album { AlbumId = ++id, Title = "Holy Diver", Genre = genres["Metal"], Price = 8.99M, Artist = artists["Dio"], AlbumArtUrl = imgUrl },
                new Album { AlbumId = ++id, Title = "Homework", Genre = genres["Electronic"], Price = 8.99M, Artist = artists["Daft Punk"], AlbumArtUrl = imgUrl },
                new Album { AlbumId = ++id, Title = "Hot Rocks, 1964-1971 (Disc 1)", Genre = genres["Rock"], Price = 8.99M, Artist = artists["The Rolling Stones"], AlbumArtUrl = imgUrl },
                new Album { AlbumId = ++id, Title = "Houses Of The Holy", Genre = genres["Rock"], Price = 8.99M, Artist = artists["Led Zeppelin"], AlbumArtUrl = imgUrl },
                new Album { AlbumId = ++id, Title = "How To Dismantle An Atomic Bomb", Genre = genres["Rock"], Price = 8.99M, Artist = artists["U2"], AlbumArtUrl = imgUrl },
                new Album { AlbumId = ++id, Title = "Human", Genre = genres["Metal"], Price = 8.99M, Artist = artists["Projected"], AlbumArtUrl = imgUrl },
                new Album { AlbumId = ++id, Title = "Hunky Dory", Genre = genres["Rock"], Price = 8.99M, Artist = artists["David Bowie"], AlbumArtUrl = imgUrl },
                new Album { AlbumId = ++id, Title = "Hymns", Genre = genres["Rock"], Price = 8.99M, Artist = artists["Projected"], AlbumArtUrl = imgUrl },
                new Album { AlbumId = ++id, Title = "Hysteria", Genre = genres["Rock"], Price = 8.99M, Artist = artists["Def Leppard"], AlbumArtUrl = imgUrl },
                new Album { AlbumId = ++id, Title = "In Absentia", Genre = genres["Rock"], Price = 8.99M, Artist = artists["Porcupine Tree"], AlbumArtUrl = imgUrl },
                new Album { AlbumId = ++id, Title = "In Between", Genre = genres["Pop"], Price = 8.99M, Artist = artists["Paul Van Dyk"], AlbumArtUrl = imgUrl },
                new Album { AlbumId = ++id, Title = "In Rainbows", Genre = genres["Rock"], Price = 8.99M, Artist = artists["Radiohead"], AlbumArtUrl = imgUrl },
                new Album { AlbumId = ++id, Title = "In Step", Genre = genres["Blues"], Price = 8.99M, Artist = artists["Stevie Ray Vaughan & Double Trouble"], AlbumArtUrl = imgUrl },
                new Album { AlbumId = ++id, Title = "In the court of the Crimson King", Genre = genres["Rock"], Price = 8.99M, Artist = artists["King Crimson"], AlbumArtUrl = imgUrl },
                new Album { AlbumId = ++id, Title = "In Through The Out Door", Genre = genres["Rock"], Price = 8.99M, Artist = artists["Led Zeppelin"], AlbumArtUrl = imgUrl },
                new Album { AlbumId = ++id, Title = "In Your Honor [Disc 1]", Genre = genres["Rock"], Price = 8.99M, Artist = artists["Foo Fighters"], AlbumArtUrl = imgUrl },
                new Album { AlbumId = ++id, Title = "In Your Honor [Disc 2]", Genre = genres["Rock"], Price = 8.99M, Artist = artists["Foo Fighters"], AlbumArtUrl = imgUrl },
                new Album { AlbumId = ++id, Title = "Indestructible", Genre = genres["Rock"], Price = 8.99M, Artist = artists["Rancid"], AlbumArtUrl = imgUrl },
                new Album { AlbumId = ++id, Title = "Infinity", Genre = genres["Rock"], Price = 8.99M, Artist = artists["Journey"], AlbumArtUrl = imgUrl },
                new Album { AlbumId = ++id, Title = "Into The Light", Genre = genres["Rock"], Price = 8.99M, Artist = artists["David Coverdale"], AlbumArtUrl = imgUrl },
                new Album { AlbumId = ++id, Title = "Introspective", Genre = genres["Pop"], Price = 8.99M, Artist = artists["Pet Shop Boys"], AlbumArtUrl = imgUrl },
                new Album { AlbumId = ++id, Title = "Iron Maiden", Genre = genres["Blues"], Price = 8.99M, Artist = artists["Iron Maiden"], AlbumArtUrl = imgUrl },
                new Album { AlbumId = ++id, Title = "ISAM", Genre = genres["Electronic"], Price = 8.99M, Artist = artists["Amon Tobin"], AlbumArtUrl = imgUrl },
                new Album { AlbumId = ++id, Title = "IV", Genre = genres["Rock"], Price = 8.99M, Artist = artists["Led Zeppelin"], AlbumArtUrl = imgUrl },
                new Album { AlbumId = ++id, Title = "Jagged Little Pill", Genre = genres["Alternative"], Price = 8.99M, Artist = artists["Alanis Morissette"], AlbumArtUrl = imgUrl },
                new Album { AlbumId = ++id, Title = "Jagged Little Pill", Genre = genres["Rock"], Price = 8.99M, Artist = artists["Alanis Morissette"], AlbumArtUrl = imgUrl },
                new Album { AlbumId = ++id, Title = "Jorge Ben Jor 25 Anos", Genre = genres["Latin"], Price = 8.99M, Artist = artists["Jorge Ben"], AlbumArtUrl = imgUrl },
                new Album { AlbumId = ++id, Title = "Jota Quest-1995", Genre = genres["Latin"], Price = 8.99M, Artist = artists["Jota Quest"], AlbumArtUrl = imgUrl },
                new Album { AlbumId = ++id, Title = "Kick", Genre = genres["Alternative"], Price = 8.99M, Artist = artists["INXS"], AlbumArtUrl = imgUrl },
                new Album { AlbumId = ++id, Title = "Kill 'Em All", Genre = genres["Metal"], Price = 8.99M, Artist = artists["Metallica"], AlbumArtUrl = imgUrl },
                new Album { AlbumId = ++id, Title = "Kind of Blue", Genre = genres["Jazz"], Price = 8.99M, Artist = artists["Miles Davis"], AlbumArtUrl = imgUrl },
                new Album { AlbumId = ++id, Title = "King For A Day Fool For A Lifetime", Genre = genres["Rock"], Price = 8.99M, Artist = artists["Faith No More"], AlbumArtUrl = imgUrl },
                new Album { AlbumId = ++id, Title = "Kiss", Genre = genres["Pop"], Price = 8.99M, Artist = artists["Carly Rae Jepsen"], AlbumArtUrl = imgUrl },
                new Album { AlbumId = ++id, Title = "Last Call", Genre = genres["Country"], Price = 8.99M, Artist = artists["Cayouche"], AlbumArtUrl = imgUrl },
                new Album { AlbumId = ++id, Title = "Le Freak", Genre = genres["R&B"], Price = 8.99M, Artist = artists["Chic"], AlbumArtUrl = imgUrl },
                new Album { AlbumId = ++id, Title = "Le Tigre", Genre = genres["Rock"], Price = 8.99M, Artist = artists["Le Tigre"], AlbumArtUrl = imgUrl },
                new Album { AlbumId = ++id, Title = "Led Zeppelin I", Genre = genres["Rock"], Price = 8.99M, Artist = artists["Led Zeppelin"], AlbumArtUrl = imgUrl },
                new Album { AlbumId = ++id, Title = "Led Zeppelin II", Genre = genres["Rock"], Price = 8.99M, Artist = artists["Led Zeppelin"], AlbumArtUrl = imgUrl },
                new Album { AlbumId = ++id, Title = "Led Zeppelin III", Genre = genres["Rock"], Price = 8.99M, Artist = artists["Led Zeppelin"], AlbumArtUrl = imgUrl },
                new Album { AlbumId = ++id, Title = "Let There Be Rock", Genre = genres["Rock"], Price = 8.99M, Artist = artists["AC/DC"], AlbumArtUrl = imgUrl },
                new Album { AlbumId = ++id, Title = "Little Earthquakes", Genre = genres["Alternative"], Price = 8.99M, Artist = artists["Tori Amos"], AlbumArtUrl = imgUrl },
                new Album { AlbumId = ++id, Title = "Live [Disc 1]", Genre = genres["Blues"], Price = 8.99M, Artist = artists["The Black Crowes"], AlbumArtUrl = imgUrl },
                new Album { AlbumId = ++id, Title = "Live [Disc 2]", Genre = genres["Blues"], Price = 8.99M, Artist = artists["The Black Crowes"], AlbumArtUrl = imgUrl },
                new Album { AlbumId = ++id, Title = "Live After Death", Genre = genres["Metal"], Price = 8.99M, Artist = artists["Iron Maiden"], AlbumArtUrl = imgUrl },
                new Album { AlbumId = ++id, Title = "Live At Donington 1992 (Disc 1)", Genre = genres["Rock"], Price = 8.99M, Artist = artists["Iron Maiden"], AlbumArtUrl = imgUrl },
                new Album { AlbumId = ++id, Title = "Live At Donington 1992 (Disc 2)", Genre = genres["Rock"], Price = 8.99M, Artist = artists["Iron Maiden"], AlbumArtUrl = imgUrl },
                new Album { AlbumId = ++id, Title = "Live on Earth", Genre = genres["Jazz"], Price = 8.99M, Artist = artists["The Cat Empire"], AlbumArtUrl = imgUrl },
                new Album { AlbumId = ++id, Title = "Live On Two Legs [Live]", Genre = genres["Rock"], Price = 8.99M, Artist = artists["Pearl Jam"], AlbumArtUrl = imgUrl },
                new Album { AlbumId = ++id, Title = "Living After Midnight", Genre = genres["Metal"], Price = 8.99M, Artist = artists["Judas Priest"], AlbumArtUrl = imgUrl },
                new Album { AlbumId = ++id, Title = "Living", Genre = genres["Rock"], Price = 8.99M, Artist = artists["Paddy Casey"], AlbumArtUrl = imgUrl },
                new Album { AlbumId = ++id, Title = "Load", Genre = genres["Metal"], Price = 8.99M, Artist = artists["Metallica"], AlbumArtUrl = imgUrl },
                new Album { AlbumId = ++id, Title = "Love Changes Everything", Genre = genres["Classical"], Price = 8.99M, Artist = artists["Sarah Brightman"], AlbumArtUrl = imgUrl },
                new Album { AlbumId = ++id, Title = "MacArthur Park Suite", Genre = genres["R&B"], Price = 8.99M, Artist = artists["Donna Summer"], AlbumArtUrl = imgUrl },
                new Album { AlbumId = ++id, Title = "Machine Head", Genre = genres["Rock"], Price = 8.99M, Artist = artists["Deep Purple"], AlbumArtUrl = imgUrl },
                new Album { AlbumId = ++id, Title = "Magical Mystery Tour", Genre = genres["Pop"], Price = 8.99M, Artist = artists["The Beatles"], AlbumArtUrl = imgUrl },
                new Album { AlbumId = ++id, Title = "Mais Do Mesmo", Genre = genres["Latin"], Price = 8.99M, Artist = artists["Legião Urbana"], AlbumArtUrl = imgUrl },
                new Album { AlbumId = ++id, Title = "Maquinarama", Genre = genres["Rock"], Price = 8.99M, Artist = artists["Skank"], AlbumArtUrl = imgUrl },
                new Album { AlbumId = ++id, Title = "Marasim", Genre = genres["Classical"], Price = 8.99M, Artist = artists["Jagjit Singh"], AlbumArtUrl = imgUrl },
                new Album { AlbumId = ++id, Title = "Mascagni: Cavalleria Rusticana", Genre = genres["Classical"], Price = 8.99M, Artist = artists["James Levine"], AlbumArtUrl = imgUrl },
                new Album { AlbumId = ++id, Title = "Master of Puppets", Genre = genres["Metal"], Price = 8.99M, Artist = artists["Metallica"], AlbumArtUrl = imgUrl },
                new Album { AlbumId = ++id, Title = "Mechanics & Mathematics", Genre = genres["Pop"], Price = 8.99M, Artist = artists["Venus Hum"], AlbumArtUrl = imgUrl },
                new Album { AlbumId = ++id, Title = "Mental Jewelry", Genre = genres["Alternative"], Price = 8.99M, Artist = artists["Live"], AlbumArtUrl = imgUrl },
                new Album { AlbumId = ++id, Title = "Metallics", Genre = genres["Rock"], Price = 8.99M, Artist = artists["Metallica"], AlbumArtUrl = imgUrl },
                new Album { AlbumId = ++id, Title = "meteora", Genre = genres["Rock"], Price = 8.99M, Artist = artists["Linkin Park"], AlbumArtUrl = imgUrl },
                new Album { AlbumId = ++id, Title = "Meus Momentos", Genre = genres["Latin"], Price = 8.99M, Artist = artists["Gonzaguinha"], AlbumArtUrl = imgUrl },
                new Album { AlbumId = ++id, Title = "Mezmerize", Genre = genres["Metal"], Price = 8.99M, Artist = artists["System Of A Down"], AlbumArtUrl = imgUrl },
                new Album { AlbumId = ++id, Title = "Mezzanine", Genre = genres["Electronic"], Price = 8.99M, Artist = artists["Massive Attack"], AlbumArtUrl = imgUrl },
                new Album { AlbumId = ++id, Title = "Miles Ahead", Genre = genres["Jazz"], Price = 8.99M, Artist = artists["Miles Davis"], AlbumArtUrl = imgUrl },
                new Album { AlbumId = ++id, Title = "Milton Nascimento Ao Vivo", Genre = genres["Latin"], Price = 8.99M, Artist = artists["Milton Nascimento"], AlbumArtUrl = imgUrl },
                new Album { AlbumId = ++id, Title = "Minas", Genre = genres["Latin"], Price = 8.99M, Artist = artists["Milton Nascimento"], AlbumArtUrl = imgUrl },
                new Album { AlbumId = ++id, Title = "Minha Historia", Genre = genres["Latin"], Price = 8.99M, Artist = artists["Chico Buarque"], AlbumArtUrl = imgUrl },
                new Album { AlbumId = ++id, Title = "Misplaced Childhood", Genre = genres["Rock"], Price = 8.99M, Artist = artists["Marillion"], AlbumArtUrl = imgUrl },
                new Album { AlbumId = ++id, Title = "MK III The Final Concerts [Disc 1]", Genre = genres["Rock"], Price = 8.99M, Artist = artists["Deep Purple"], AlbumArtUrl = imgUrl },
                new Album { AlbumId = ++id, Title = "Morning Dance", Genre = genres["Jazz"], Price = 8.99M, Artist = artists["Spyro Gyra"], AlbumArtUrl = imgUrl },
                new Album { AlbumId = ++id, Title = "Motley Crue Greatest Hits", Genre = genres["Metal"], Price = 8.99M, Artist = artists["Mötley Crüe"], AlbumArtUrl = imgUrl },
                new Album { AlbumId = ++id, Title = "Moving Pictures", Genre = genres["Rock"], Price = 8.99M, Artist = artists["Rush"], AlbumArtUrl = imgUrl },
                new Album { AlbumId = ++id, Title = "Mozart: Chamber Music", Genre = genres["Classical"], Price = 8.99M, Artist = artists["Nash Ensemble"], AlbumArtUrl = imgUrl },
                new Album { AlbumId = ++id, Title = "Mozart: Symphonies Nos. 40 & 41", Genre = genres["Classical"], Price = 8.99M, Artist = artists["Berliner Philharmoniker"], AlbumArtUrl = imgUrl },
                new Album { AlbumId = ++id, Title = "Murder Ballads", Genre = genres["Rock"], Price = 8.99M, Artist = artists["Nick Cave and the Bad Seeds"], AlbumArtUrl = imgUrl },
                new Album { AlbumId = ++id, Title = "Music For The Jilted Generation", Genre = genres["Electronic"], Price = 8.99M, Artist = artists["The Prodigy"], AlbumArtUrl = imgUrl },
                new Album { AlbumId = ++id, Title = "My Generation - The Very Best Of The Who", Genre = genres["Rock"], Price = 8.99M, Artist = artists["The Who"], AlbumArtUrl = imgUrl },
                new Album { AlbumId = ++id, Title = "My Name is Skrillex", Genre = genres["Electronic"], Price = 8.99M, Artist = artists["Skrillex"], AlbumArtUrl = imgUrl },
                new Album { AlbumId = ++id, Title = "Na Pista", Genre = genres["Latin"], Price = 8.99M, Artist = artists["Cláudio Zoli"], AlbumArtUrl = imgUrl },
                new Album { AlbumId = ++id, Title = "Nevermind", Genre = genres["Rock"], Price = 8.99M, Artist = artists["Nirvana"], AlbumArtUrl = imgUrl },
                new Album { AlbumId = ++id, Title = "New Adventures In Hi-Fi", Genre = genres["Rock"], Price = 8.99M, Artist = artists["R.E.M."], AlbumArtUrl = imgUrl },
                new Album { AlbumId = ++id, Title = "New Divide", Genre = genres["Rock"], Price = 8.99M, Artist = artists["Linkin Park"], AlbumArtUrl = imgUrl },
                new Album { AlbumId = ++id, Title = "New York Dolls", Genre = genres["Punk"], Price = 8.99M, Artist = artists["New York Dolls"], AlbumArtUrl = imgUrl },
                new Album { AlbumId = ++id, Title = "News Of The World", Genre = genres["Rock"], Price = 8.99M, Artist = artists["Queen"], AlbumArtUrl = imgUrl },
                new Album { AlbumId = ++id, Title = "Nielsen: The Six Symphonies", Genre = genres["Classical"], Price = 8.99M, Artist = artists["Göteborgs Symfoniker & Neeme Järvi"], AlbumArtUrl = imgUrl },
                new Album { AlbumId = ++id, Title = "Night At The Opera", Genre = genres["Rock"], Price = 8.99M, Artist = artists["Queen"], AlbumArtUrl = imgUrl },
                new Album { AlbumId = ++id, Title = "Night Castle", Genre = genres["Rock"], Price = 8.99M, Artist = artists["Trans-Siberian Orchestra"], AlbumArtUrl = imgUrl },
                new Album { AlbumId = ++id, Title = "Nkolo", Genre = genres["World"], Price = 8.99M, Artist = artists["Lokua Kanza"], AlbumArtUrl = imgUrl },
                new Album { AlbumId = ++id, Title = "No More Tears (Remastered)", Genre = genres["Rock"], Price = 8.99M, Artist = artists["Ozzy Osbourne"], AlbumArtUrl = imgUrl },
                new Album { AlbumId = ++id, Title = "No Prayer For The Dying", Genre = genres["Metal"], Price = 8.99M, Artist = artists["Iron Maiden"], AlbumArtUrl = imgUrl },
                new Album { AlbumId = ++id, Title = "No Security", Genre = genres["Rock"], Price = 8.99M, Artist = artists["The Rolling Stones"], AlbumArtUrl = imgUrl },
                new Album { AlbumId = ++id, Title = "O Brother, Where Art Thou?", Genre = genres["Country"], Price = 8.99M, Artist = artists["Alison Krauss"], AlbumArtUrl = imgUrl },
                new Album { AlbumId = ++id, Title = "O Samba Poconé", Genre = genres["Rock"], Price = 8.99M, Artist = artists["Skank"], AlbumArtUrl = imgUrl },
                new Album { AlbumId = ++id, Title = "O(+>", Genre = genres["R&B"], Price = 8.99M, Artist = artists["Prince"], AlbumArtUrl = imgUrl },
                new Album { AlbumId = ++id, Title = "Oceania", Genre = genres["Rock"], Price = 8.99M, Artist = artists["The Smashing Pumpkins"], AlbumArtUrl = imgUrl },
                new Album { AlbumId = ++id, Title = "Off the Deep End", Genre = genres["Pop"], Price = 8.99M, Artist = artists["Weird Al"], AlbumArtUrl = imgUrl },
                new Album { AlbumId = ++id, Title = "OK Computer", Genre = genres["Rock"], Price = 8.99M, Artist = artists["Radiohead"], AlbumArtUrl = imgUrl },
                new Album { AlbumId = ++id, Title = "Olodum", Genre = genres["Latin"], Price = 8.99M, Artist = artists["Olodum"], AlbumArtUrl = imgUrl },
                new Album { AlbumId = ++id, Title = "One Love", Genre = genres["Electronic"], Price = 8.99M, Artist = artists["David Guetta"], AlbumArtUrl = imgUrl },
                new Album { AlbumId = ++id, Title = "Operation: Mindcrime", Genre = genres["Metal"], Price = 8.99M, Artist = artists["Queensrÿche"], AlbumArtUrl = imgUrl },
                new Album { AlbumId = ++id, Title = "Opiate", Genre = genres["Rock"], Price = 8.99M, Artist = artists["Tool"], AlbumArtUrl = imgUrl },
                new Album { AlbumId = ++id, Title = "Outbreak", Genre = genres["Jazz"], Price = 8.99M, Artist = artists["Dennis Chambers"], AlbumArtUrl = imgUrl },
                new Album { AlbumId = ++id, Title = "Pachelbel: Canon & Gigue", Genre = genres["Classical"], Price = 8.99M, Artist = artists["English Concert & Trevor Pinnock"], AlbumArtUrl = imgUrl },
                new Album { AlbumId = ++id, Title = "Paid in Full", Genre = genres["Rap"], Price = 8.99M, Artist = artists["Eric B. and Rakim"], AlbumArtUrl = imgUrl },
                new Album { AlbumId = ++id, Title = "Para Siempre", Genre = genres["Latin"], Price = 8.99M, Artist = artists["Vicente Fernandez"], AlbumArtUrl = imgUrl },
                new Album { AlbumId = ++id, Title = "Pause", Genre = genres["Electronic"], Price = 8.99M, Artist = artists["Four Tet"], AlbumArtUrl = imgUrl },
                new Album { AlbumId = ++id, Title = "Peace Sells... but Who's Buying", Genre = genres["Metal"], Price = 8.99M, Artist = artists["Megadeth"], AlbumArtUrl = imgUrl },
                new Album { AlbumId = ++id, Title = "Physical Graffiti [Disc 1]", Genre = genres["Rock"], Price = 8.99M, Artist = artists["Led Zeppelin"], AlbumArtUrl = imgUrl },
                new Album { AlbumId = ++id, Title = "Physical Graffiti [Disc 2]", Genre = genres["Rock"], Price = 8.99M, Artist = artists["Led Zeppelin"], AlbumArtUrl = imgUrl },
                new Album { AlbumId = ++id, Title = "Physical Graffiti", Genre = genres["Rock"], Price = 8.99M, Artist = artists["Led Zeppelin"], AlbumArtUrl = imgUrl },
                new Album { AlbumId = ++id, Title = "Piece Of Mind", Genre = genres["Metal"], Price = 8.99M, Artist = artists["Iron Maiden"], AlbumArtUrl = imgUrl },
                new Album { AlbumId = ++id, Title = "Pinkerton", Genre = genres["Rock"], Price = 8.99M, Artist = artists["Weezer"], AlbumArtUrl = imgUrl },
                new Album { AlbumId = ++id, Title = "Plays Metallica By Four Cellos", Genre = genres["Metal"], Price = 8.99M, Artist = artists["Apocalyptica"], AlbumArtUrl = imgUrl },
                new Album { AlbumId = ++id, Title = "Pop", Genre = genres["Rock"], Price = 8.99M, Artist = artists["U2"], AlbumArtUrl = imgUrl },
                new Album { AlbumId = ++id, Title = "Powerslave", Genre = genres["Metal"], Price = 8.99M, Artist = artists["Iron Maiden"], AlbumArtUrl = imgUrl },
                new Album { AlbumId = ++id, Title = "Prenda Minha", Genre = genres["Latin"], Price = 8.99M, Artist = artists["Caetano Veloso"], AlbumArtUrl = imgUrl },
                new Album { AlbumId = ++id, Title = "Presence", Genre = genres["Rock"], Price = 8.99M, Artist = artists["Led Zeppelin"], AlbumArtUrl = imgUrl },
                new Album { AlbumId = ++id, Title = "Pretty Hate Machine", Genre = genres["Alternative"], Price = 8.99M, Artist = artists["Nine Inch Nails"], AlbumArtUrl = imgUrl },
                new Album { AlbumId = ++id, Title = "Prisoner", Genre = genres["Rock"], Price = 8.99M, Artist = artists["The Jezabels"], AlbumArtUrl = imgUrl },
                new Album { AlbumId = ++id, Title = "Privateering", Genre = genres["Rock"], Price = 8.99M, Artist = artists["Mark Knopfler"], AlbumArtUrl = imgUrl },
                new Album { AlbumId = ++id, Title = "Prokofiev: Romeo & Juliet", Genre = genres["Classical"], Price = 8.99M, Artist = artists["Michael Tilson Thomas"], AlbumArtUrl = imgUrl },
                new Album { AlbumId = ++id, Title = "Prokofiev: Symphony No.1", Genre = genres["Classical"], Price = 8.99M, Artist = artists["Sergei Prokofiev & Yuri Temirkanov"], AlbumArtUrl = imgUrl },
                new Album { AlbumId = ++id, Title = "PSY's Best 6th Part 1", Genre = genres["Pop"], Price = 8.99M, Artist = artists["PSY"], AlbumArtUrl = imgUrl },
                new Album { AlbumId = ++id, Title = "Purcell: The Fairy Queen", Genre = genres["Classical"], Price = 8.99M, Artist = artists["London Classical Players"], AlbumArtUrl = imgUrl },
                new Album { AlbumId = ++id, Title = "Purpendicular", Genre = genres["Rock"], Price = 8.99M, Artist = artists["Deep Purple"], AlbumArtUrl = imgUrl },
                new Album { AlbumId = ++id, Title = "Purple", Genre = genres["Rock"], Price = 8.99M, Artist = artists["Stone Temple Pilots"], AlbumArtUrl = imgUrl },
                new Album { AlbumId = ++id, Title = "Quanta Gente Veio Ver (Live)", Genre = genres["Latin"], Price = 8.99M, Artist = artists["Gilberto Gil"], AlbumArtUrl = imgUrl },
                new Album { AlbumId = ++id, Title = "Quanta Gente Veio ver--Bônus De Carnaval", Genre = genres["Jazz"], Price = 8.99M, Artist = artists["Gilberto Gil"], AlbumArtUrl = imgUrl },
                new Album { AlbumId = ++id, Title = "Quiet Songs", Genre = genres["Jazz"], Price = 8.99M, Artist = artists["Aisha Duo"], AlbumArtUrl = imgUrl },
                new Album { AlbumId = ++id, Title = "Raices", Genre = genres["Latin"], Price = 8.99M, Artist = artists["Los Tigres del Norte"], AlbumArtUrl = imgUrl },
                new Album { AlbumId = ++id, Title = "Raising Hell", Genre = genres["Rap"], Price = 8.99M, Artist = artists["Run DMC"], AlbumArtUrl = imgUrl },
                new Album { AlbumId = ++id, Title = "Raoul and the Kings of Spain ", Genre = genres["Rock"], Price = 8.99M, Artist = artists["Tears For Fears"], AlbumArtUrl = imgUrl },
                new Album { AlbumId = ++id, Title = "Rattle And Hum", Genre = genres["Rock"], Price = 8.99M, Artist = artists["U2"], AlbumArtUrl = imgUrl },
                new Album { AlbumId = ++id, Title = "Raul Seixas", Genre = genres["Rock"], Price = 8.99M, Artist = artists["Raul Seixas"], AlbumArtUrl = imgUrl },
                new Album { AlbumId = ++id, Title = "Recovery [Explicit]", Genre = genres["Rap"], Price = 8.99M, Artist = artists["Eminem"], AlbumArtUrl = imgUrl },
                new Album { AlbumId = ++id, Title = "Reign In Blood", Genre = genres["Metal"], Price = 8.99M, Artist = artists["Slayer"], AlbumArtUrl = imgUrl },
                new Album { AlbumId = ++id, Title = "Relayed", Genre = genres["Rock"], Price = 8.99M, Artist = artists["Yes"], AlbumArtUrl = imgUrl },
                new Album { AlbumId = ++id, Title = "ReLoad", Genre = genres["Metal"], Price = 8.99M, Artist = artists["Metallica"], AlbumArtUrl = imgUrl },
                new Album { AlbumId = ++id, Title = "Respighi:Pines of Rome", Genre = genres["Classical"], Price = 8.99M, Artist = artists["Eugene Ormandy"], AlbumArtUrl = imgUrl },
                new Album { AlbumId = ++id, Title = "Restless and Wild", Genre = genres["Rock"], Price = 8.99M, Artist = artists["Accept"], AlbumArtUrl = imgUrl },
                new Album { AlbumId = ++id, Title = "Retrospective I (1974-1980)", Genre = genres["Rock"], Price = 8.99M, Artist = artists["Rush"], AlbumArtUrl = imgUrl },
                new Album { AlbumId = ++id, Title = "Revelations", Genre = genres["Electronic"], Price = 8.99M, Artist = artists["Audioslave"], AlbumArtUrl = imgUrl },
                new Album { AlbumId = ++id, Title = "Revolver", Genre = genres["Rock"], Price = 8.99M, Artist = artists["The Beatles"], AlbumArtUrl = imgUrl },
                new Album { AlbumId = ++id, Title = "Ride the Lighting ", Genre = genres["Metal"], Price = 8.99M, Artist = artists["Metallica"], AlbumArtUrl = imgUrl },
                new Album { AlbumId = ++id, Title = "Ride The Lightning", Genre = genres["Metal"], Price = 8.99M, Artist = artists["Metallica"], AlbumArtUrl = imgUrl },
                new Album { AlbumId = ++id, Title = "Ring My Bell", Genre = genres["R&B"], Price = 8.99M, Artist = artists["Anita Ward"], AlbumArtUrl = imgUrl },
                new Album { AlbumId = ++id, Title = "Riot Act", Genre = genres["Rock"], Price = 8.99M, Artist = artists["Pearl Jam"], AlbumArtUrl = imgUrl },
                new Album { AlbumId = ++id, Title = "Rise of the Phoenix", Genre = genres["Metal"], Price = 8.99M, Artist = artists["Before the Dawn"], AlbumArtUrl = imgUrl },
                new Album { AlbumId = ++id, Title = "Rock In Rio [CD1]", Genre = genres["Metal"], Price = 8.99M, Artist = artists["Iron Maiden"], AlbumArtUrl = imgUrl },
                new Album { AlbumId = ++id, Title = "Rock In Rio [CD2]", Genre = genres["Metal"], Price = 8.99M, Artist = artists["Iron Maiden"], AlbumArtUrl = imgUrl },
                new Album { AlbumId = ++id, Title = "Rock In Rio [CD2]", Genre = genres["Rock"], Price = 8.99M, Artist = artists["Iron Maiden"], AlbumArtUrl = imgUrl },
                new Album { AlbumId = ++id, Title = "Roda De Funk", Genre = genres["Latin"], Price = 8.99M, Artist = artists["Funk Como Le Gusta"], AlbumArtUrl = imgUrl },
                new Album { AlbumId = ++id, Title = "Room for Squares", Genre = genres["Pop"], Price = 8.99M, Artist = artists["John Mayer"], AlbumArtUrl = imgUrl },
                new Album { AlbumId = ++id, Title = "Root Down", Genre = genres["Jazz"], Price = 8.99M, Artist = artists["Jimmy Smith"], AlbumArtUrl = imgUrl },
                new Album { AlbumId = ++id, Title = "Rounds", Genre = genres["Electronic"], Price = 8.99M, Artist = artists["Four Tet"], AlbumArtUrl = imgUrl },
                new Album { AlbumId = ++id, Title = "Rubber Factory", Genre = genres["Rock"], Price = 8.99M, Artist = artists["The Black Keys"], AlbumArtUrl = imgUrl },
                new Album { AlbumId = ++id, Title = "Rust in Peace", Genre = genres["Metal"], Price = 8.99M, Artist = artists["Megadeth"], AlbumArtUrl = imgUrl },
                new Album { AlbumId = ++id, Title = "Sambas De Enredo 2001", Genre = genres["Latin"], Price = 8.99M, Artist = artists["Various Artists"], AlbumArtUrl = imgUrl },
                new Album { AlbumId = ++id, Title = "Santana - As Years Go By", Genre = genres["Rock"], Price = 8.99M, Artist = artists["Santana"], AlbumArtUrl = imgUrl },
                new Album { AlbumId = ++id, Title = "Santana Live", Genre = genres["Rock"], Price = 8.99M, Artist = artists["Santana"], AlbumArtUrl = imgUrl },
                new Album { AlbumId = ++id, Title = "Saturday Night Fever", Genre = genres["R&B"], Price = 8.99M, Artist = artists["Bee Gees"], AlbumArtUrl = imgUrl },
                new Album { AlbumId = ++id, Title = "Scary Monsters and Nice Sprites", Genre = genres["Electronic"], Price = 8.99M, Artist = artists["Skrillex"], AlbumArtUrl = imgUrl },
                new Album { AlbumId = ++id, Title = "Scheherazade", Genre = genres["Classical"], Price = 8.99M, Artist = artists["Chicago Symphony Orchestra & Fritz Reiner"], AlbumArtUrl = imgUrl },
                new Album { AlbumId = ++id, Title = "SCRIABIN: Vers la flamme", Genre = genres["Classical"], Price = 8.99M, Artist = artists["Christopher O'Riley"], AlbumArtUrl = imgUrl },
                new Album { AlbumId = ++id, Title = "Second Coming", Genre = genres["Rock"], Price = 8.99M, Artist = artists["The Stone Roses"], AlbumArtUrl = imgUrl },
                new Album { AlbumId = ++id, Title = "Serie Sem Limite (Disc 1)", Genre = genres["Latin"], Price = 8.99M, Artist = artists["Tim Maia"], AlbumArtUrl = imgUrl },
                new Album { AlbumId = ++id, Title = "Serie Sem Limite (Disc 2)", Genre = genres["Latin"], Price = 8.99M, Artist = artists["Tim Maia"], AlbumArtUrl = imgUrl },
                new Album { AlbumId = ++id, Title = "Serious About Men", Genre = genres["Rap"], Price = 8.99M, Artist = artists["The Rubberbandits"], AlbumArtUrl = imgUrl },
                new Album { AlbumId = ++id, Title = "Seventh Son of a Seventh Son", Genre = genres["Metal"], Price = 8.99M, Artist = artists["Iron Maiden"], AlbumArtUrl = imgUrl },
                new Album { AlbumId = ++id, Title = "Short Bus", Genre = genres["Rock"], Price = 8.99M, Artist = artists["Filter"], AlbumArtUrl = imgUrl },
                new Album { AlbumId = ++id, Title = "Sibelius: Finlandia", Genre = genres["Classical"], Price = 8.99M, Artist = artists["Berliner Philharmoniker"], AlbumArtUrl = imgUrl },
                new Album { AlbumId = ++id, Title = "Singles Collection", Genre = genres["Rock"], Price = 8.99M, Artist = artists["David Bowie"], AlbumArtUrl = imgUrl },
                new Album { AlbumId = ++id, Title = "Six Degrees of Inner Turbulence", Genre = genres["Metal"], Price = 8.99M, Artist = artists["Dream Theater"], AlbumArtUrl = imgUrl },
                new Album { AlbumId = ++id, Title = "Slave To The Empire", Genre = genres["Metal"], Price = 8.99M, Artist = artists["T&N"], AlbumArtUrl = imgUrl },
                new Album { AlbumId = ++id, Title = "Slaves And Masters", Genre = genres["Rock"], Price = 8.99M, Artist = artists["Deep Purple"], AlbumArtUrl = imgUrl },
                new Album { AlbumId = ++id, Title = "Slouching Towards Bethlehem", Genre = genres["Rock"], Price = 8.99M, Artist = artists["Robert James"], AlbumArtUrl = imgUrl },
                new Album { AlbumId = ++id, Title = "Smash", Genre = genres["Rock"], Price = 8.99M, Artist = artists["The Offspring"], AlbumArtUrl = imgUrl },
                new Album { AlbumId = ++id, Title = "Something Special", Genre = genres["Country"], Price = 8.99M, Artist = artists["Dolly Parton"], AlbumArtUrl = imgUrl },
                new Album { AlbumId = ++id, Title = "Somewhere in Time", Genre = genres["Metal"], Price = 8.99M, Artist = artists["Iron Maiden"], AlbumArtUrl = imgUrl },
                new Album { AlbumId = ++id, Title = "Song(s) You Know By Heart", Genre = genres["Country"], Price = 8.99M, Artist = artists["Jimmy Buffett"], AlbumArtUrl = imgUrl },
                new Album { AlbumId = ++id, Title = "Sound of Music", Genre = genres["Punk"], Price = 8.99M, Artist = artists["Adicts"], AlbumArtUrl = imgUrl },
                new Album { AlbumId = ++id, Title = "South American Getaway", Genre = genres["Classical"], Price = 8.99M, Artist = artists["The 12 Cellists of The Berlin Philharmonic"], AlbumArtUrl = imgUrl },
                new Album { AlbumId = ++id, Title = "Sozinho Remix Ao Vivo", Genre = genres["Latin"], Price = 8.99M, Artist = artists["Caetano Veloso"], AlbumArtUrl = imgUrl },
                new Album { AlbumId = ++id, Title = "Speak of the Devil", Genre = genres["Rock"], Price = 8.99M, Artist = artists["Ozzy Osbourne"], AlbumArtUrl = imgUrl },
                new Album { AlbumId = ++id, Title = "Spiritual State", Genre = genres["Rap"], Price = 8.99M, Artist = artists["Nujabes"], AlbumArtUrl = imgUrl },
                new Album { AlbumId = ++id, Title = "St. Anger", Genre = genres["Metal"], Price = 8.99M, Artist = artists["Metallica"], AlbumArtUrl = imgUrl },
                new Album { AlbumId = ++id, Title = "Still Life", Genre = genres["Metal"], Price = 8.99M, Artist = artists["Opeth"], AlbumArtUrl = imgUrl },
                new Album { AlbumId = ++id, Title = "Stop Making Sense", Genre = genres["Rock"], Price = 8.99M, Artist = artists["Talking Heads"], AlbumArtUrl = imgUrl },
                new Album { AlbumId = ++id, Title = "Stormbringer", Genre = genres["Rock"], Price = 8.99M, Artist = artists["Deep Purple"], AlbumArtUrl = imgUrl },
                new Album { AlbumId = ++id, Title = "Stranger than Fiction", Genre = genres["Punk"], Price = 8.99M, Artist = artists["Bad Religion"], AlbumArtUrl = imgUrl },
                new Album { AlbumId = ++id, Title = "Strauss: Waltzes", Genre = genres["Classical"], Price = 8.99M, Artist = artists["Eugene Ormandy"], AlbumArtUrl = imgUrl },
                new Album { AlbumId = ++id, Title = "Supermodified", Genre = genres["Electronic"], Price = 8.99M, Artist = artists["Amon Tobin"], AlbumArtUrl = imgUrl },
                new Album { AlbumId = ++id, Title = "Supernatural", Genre = genres["Rock"], Price = 8.99M, Artist = artists["Santana"], AlbumArtUrl = imgUrl },
                new Album { AlbumId = ++id, Title = "Surfing with the Alien (Remastered)", Genre = genres["Rock"], Price = 8.99M, Artist = artists["Joe Satriani"], AlbumArtUrl = imgUrl },
                new Album { AlbumId = ++id, Title = "Switched-On Bach", Genre = genres["Classical"], Price = 8.99M, Artist = artists["Wendy Carlos"], AlbumArtUrl = imgUrl },
                new Album { AlbumId = ++id, Title = "Symphony", Genre = genres["Classical"], Price = 8.99M, Artist = artists["Sarah Brightman"], AlbumArtUrl = imgUrl },
                new Album { AlbumId = ++id, Title = "Szymanowski: Piano Works, Vol. 1", Genre = genres["Classical"], Price = 8.99M, Artist = artists["Martin Roscoe"], AlbumArtUrl = imgUrl },
                new Album { AlbumId = ++id, Title = "Tchaikovsky: The Nutcracker", Genre = genres["Classical"], Price = 8.99M, Artist = artists["London Symphony Orchestra"], AlbumArtUrl = imgUrl },
                new Album { AlbumId = ++id, Title = "Ted Nugent", Genre = genres["Rock"], Price = 8.99M, Artist = artists["Ted Nugent"], AlbumArtUrl = imgUrl },
                new Album { AlbumId = ++id, Title = "Teflon Don", Genre = genres["Rap"], Price = 8.99M, Artist = artists["Rick Ross"], AlbumArtUrl = imgUrl },
                new Album { AlbumId = ++id, Title = "Tell Another Joke at the Ol' Choppin' Block", Genre = genres["Indie"], Price = 8.99M, Artist = artists["Danielson Famile"], AlbumArtUrl = imgUrl },
                new Album { AlbumId = ++id, Title = "Temple of the Dog", Genre = genres["Electronic"], Price = 8.99M, Artist = artists["Temple of the Dog"], AlbumArtUrl = imgUrl },
                new Album { AlbumId = ++id, Title = "Ten", Genre = genres["Rock"], Price = 8.99M, Artist = artists["Pearl Jam"], AlbumArtUrl = imgUrl },
                new Album { AlbumId = ++id, Title = "Texas Flood", Genre = genres["Blues"], Price = 8.99M, Artist = artists["Stevie Ray Vaughan"], AlbumArtUrl = imgUrl },
                new Album { AlbumId = ++id, Title = "The Battle Rages On", Genre = genres["Rock"], Price = 8.99M, Artist = artists["Deep Purple"], AlbumArtUrl = imgUrl },
                new Album { AlbumId = ++id, Title = "The Beast Live", Genre = genres["Rock"], Price = 8.99M, Artist = artists["Paul D'Ianno"], AlbumArtUrl = imgUrl },
                new Album { AlbumId = ++id, Title = "The Best Of 1980-1990", Genre = genres["Rock"], Price = 8.99M, Artist = artists["U2"], AlbumArtUrl = imgUrl },
                new Album { AlbumId = ++id, Title = "The Best of 1990–2000", Genre = genres["Classical"], Price = 8.99M, Artist = artists["Sarah Brightman"], AlbumArtUrl = imgUrl },
                new Album { AlbumId = ++id, Title = "The Best of Beethoven", Genre = genres["Classical"], Price = 8.99M, Artist = artists["Nicolaus Esterhazy Sinfonia"], AlbumArtUrl = imgUrl },
                new Album { AlbumId = ++id, Title = "The Best Of Billy Cobham", Genre = genres["Jazz"], Price = 8.99M, Artist = artists["Billy Cobham"], AlbumArtUrl = imgUrl },
                new Album { AlbumId = ++id, Title = "The Best of Ed Motta", Genre = genres["Latin"], Price = 8.99M, Artist = artists["Ed Motta"], AlbumArtUrl = imgUrl },
                new Album { AlbumId = ++id, Title = "The Best Of Van Halen, Vol. I", Genre = genres["Rock"], Price = 8.99M, Artist = artists["Van Halen"], AlbumArtUrl = imgUrl },
                new Album { AlbumId = ++id, Title = "The Bridge", Genre = genres["R&B"], Price = 8.99M, Artist = artists["Melanie Fiona"], AlbumArtUrl = imgUrl },
                new Album { AlbumId = ++id, Title = "The Cage", Genre = genres["Rock"], Price = 8.99M, Artist = artists["Tygers of Pan Tang"], AlbumArtUrl = imgUrl },
                new Album { AlbumId = ++id, Title = "The Chicago Transit Authority", Genre = genres["Rock"], Price = 8.99M, Artist = artists["Chicago "], AlbumArtUrl = imgUrl },
                new Album { AlbumId = ++id, Title = "The Chronic", Genre = genres["Rap"], Price = 8.99M, Artist = artists["Dr. Dre"], AlbumArtUrl = imgUrl },
                new Album { AlbumId = ++id, Title = "The Colour And The Shape", Genre = genres["Rock"], Price = 8.99M, Artist = artists["Foo Fighters"], AlbumArtUrl = imgUrl },
                new Album { AlbumId = ++id, Title = "The Crane Wife", Genre = genres["Alternative"], Price = 8.99M, Artist = artists["The Decemberists"], AlbumArtUrl = imgUrl },
                new Album { AlbumId = ++id, Title = "The Cream Of Clapton", Genre = genres["Blues"], Price = 8.99M, Artist = artists["Eric Clapton"], AlbumArtUrl = imgUrl },
                new Album { AlbumId = ++id, Title = "The Cure", Genre = genres["Pop"], Price = 8.99M, Artist = artists["The Cure"], AlbumArtUrl = imgUrl },
                new Album { AlbumId = ++id, Title = "The Dark Side Of The Moon", Genre = genres["Rock"], Price = 8.99M, Artist = artists["Pink Floyd"], AlbumArtUrl = imgUrl },
                new Album { AlbumId = ++id, Title = "The Divine Conspiracy", Genre = genres["Metal"], Price = 8.99M, Artist = artists["Epica"], AlbumArtUrl = imgUrl },
                new Album { AlbumId = ++id, Title = "The Doors", Genre = genres["Rock"], Price = 8.99M, Artist = artists["The Doors"], AlbumArtUrl = imgUrl },
                new Album { AlbumId = ++id, Title = "The Dream of the Blue Turtles", Genre = genres["Pop"], Price = 8.99M, Artist = artists["Sting"], AlbumArtUrl = imgUrl },
                new Album { AlbumId = ++id, Title = "The Essential Miles Davis [Disc 1]", Genre = genres["Jazz"], Price = 8.99M, Artist = artists["Miles Davis"], AlbumArtUrl = imgUrl },
                new Album { AlbumId = ++id, Title = "The Essential Miles Davis [Disc 2]", Genre = genres["Jazz"], Price = 8.99M, Artist = artists["Miles Davis"], AlbumArtUrl = imgUrl },
                new Album { AlbumId = ++id, Title = "The Final Concerts (Disc 2)", Genre = genres["Rock"], Price = 8.99M, Artist = artists["Deep Purple"], AlbumArtUrl = imgUrl },
                new Album { AlbumId = ++id, Title = "The Final Frontier", Genre = genres["Metal"], Price = 8.99M, Artist = artists["Iron Maiden"], AlbumArtUrl = imgUrl },
                new Album { AlbumId = ++id, Title = "The Head and the Heart", Genre = genres["Rock"], Price = 8.99M, Artist = artists["The Head and the Heart"], AlbumArtUrl = imgUrl },
                new Album { AlbumId = ++id, Title = "The Joshua Tree", Genre = genres["Rock"], Price = 8.99M, Artist = artists["U2"], AlbumArtUrl = imgUrl },
                new Album { AlbumId = ++id, Title = "The Last Night of the Proms", Genre = genres["Classical"], Price = 8.99M, Artist = artists["BBC Concert Orchestra"], AlbumArtUrl = imgUrl },
                new Album { AlbumId = ++id, Title = "The Lumineers", Genre = genres["Rock"], Price = 8.99M, Artist = artists["The Lumineers"], AlbumArtUrl = imgUrl },
                new Album { AlbumId = ++id, Title = "The Number of The Beast", Genre = genres["Metal"], Price = 8.99M, Artist = artists["Iron Maiden"], AlbumArtUrl = imgUrl },
                new Album { AlbumId = ++id, Title = "The Number of The Beast", Genre = genres["Rock"], Price = 8.99M, Artist = artists["Iron Maiden"], AlbumArtUrl = imgUrl },
                new Album { AlbumId = ++id, Title = "The Police Greatest Hits", Genre = genres["Rock"], Price = 8.99M, Artist = artists["The Police"], AlbumArtUrl = imgUrl },
                new Album { AlbumId = ++id, Title = "The Song Remains The Same (Disc 1)", Genre = genres["Rock"], Price = 8.99M, Artist = artists["Led Zeppelin"], AlbumArtUrl = imgUrl },
                new Album { AlbumId = ++id, Title = "The Song Remains The Same (Disc 2)", Genre = genres["Rock"], Price = 8.99M, Artist = artists["Led Zeppelin"], AlbumArtUrl = imgUrl },
                new Album { AlbumId = ++id, Title = "The Southern Harmony and Musical Companion", Genre = genres["Blues"], Price = 8.99M, Artist = artists["The Black Crowes"], AlbumArtUrl = imgUrl },
                new Album { AlbumId = ++id, Title = "The Spade", Genre = genres["Rock"], Price = 8.99M, Artist = artists["Butch Walker & The Black Widows"], AlbumArtUrl = imgUrl },
                new Album { AlbumId = ++id, Title = "The Stone Roses", Genre = genres["Rock"], Price = 8.99M, Artist = artists["The Stone Roses"], AlbumArtUrl = imgUrl },
                new Album { AlbumId = ++id, Title = "The Suburbs", Genre = genres["Indie"], Price = 8.99M, Artist = artists["Arcade Fire"], AlbumArtUrl = imgUrl },
                new Album { AlbumId = ++id, Title = "The Three Tenors Disc1/Disc2", Genre = genres["Classical"], Price = 8.99M, Artist = artists["Carreras, Pavarotti, Domingo"], AlbumArtUrl = imgUrl },
                new Album { AlbumId = ++id, Title = "The Trees They Grow So High", Genre = genres["Classical"], Price = 8.99M, Artist = artists["Sarah Brightman"], AlbumArtUrl = imgUrl },
                new Album { AlbumId = ++id, Title = "The Wall", Genre = genres["Rock"], Price = 8.99M, Artist = artists["Pink Floyd"], AlbumArtUrl = imgUrl },
                new Album { AlbumId = ++id, Title = "The X Factor", Genre = genres["Rock"], Price = 8.99M, Artist = artists["Iron Maiden"], AlbumArtUrl = imgUrl },
                new Album { AlbumId = ++id, Title = "Them Crooked Vultures", Genre = genres["Rock"], Price = 8.99M, Artist = artists["Them Crooked Vultures"], AlbumArtUrl = imgUrl },
                new Album { AlbumId = ++id, Title = "This Is Happening", Genre = genres["Rock"], Price = 8.99M, Artist = artists["LCD Soundsystem"], AlbumArtUrl = imgUrl },
                new Album { AlbumId = ++id, Title = "Thunder, Lightning, Strike", Genre = genres["Rock"], Price = 8.99M, Artist = artists["The Go! Team"], AlbumArtUrl = imgUrl },
                new Album { AlbumId = ++id, Title = "Time to Say Goodbye", Genre = genres["Classical"], Price = 8.99M, Artist = artists["Sarah Brightman"], AlbumArtUrl = imgUrl },
                new Album { AlbumId = ++id, Title = "Time, Love & Tenderness", Genre = genres["Pop"], Price = 8.99M, Artist = artists["Michael Bolton"], AlbumArtUrl = imgUrl },
                new Album { AlbumId = ++id, Title = "Tomorrow Starts Today", Genre = genres["Rock"], Price = 8.99M, Artist = artists["Mobile"], AlbumArtUrl = imgUrl },
                new Album { AlbumId = ++id, Title = "Tribute", Genre = genres["Metal"], Price = 8.99M, Artist = artists["Ozzy Osbourne"], AlbumArtUrl = imgUrl },
                new Album { AlbumId = ++id, Title = "Tuesday Night Music Club", Genre = genres["Alternative"], Price = 8.99M, Artist = artists["Sheryl Crow"], AlbumArtUrl = imgUrl },
                new Album { AlbumId = ++id, Title = "Umoja", Genre = genres["Rock"], Price = 8.99M, Artist = artists["BLØF"], AlbumArtUrl = imgUrl },
                new Album { AlbumId = ++id, Title = "Under the Pink", Genre = genres["Alternative"], Price = 8.99M, Artist = artists["Tori Amos"], AlbumArtUrl = imgUrl },
                new Album { AlbumId = ++id, Title = "Undertow", Genre = genres["Rock"], Price = 8.99M, Artist = artists["Tool"], AlbumArtUrl = imgUrl },
                new Album { AlbumId = ++id, Title = "Un-Led-Ed", Genre = genres["Rock"], Price = 8.99M, Artist = artists["Dread Zeppelin"], AlbumArtUrl = imgUrl },
                new Album { AlbumId = ++id, Title = "Unplugged [Live]", Genre = genres["Rock"], Price = 8.99M, Artist = artists["Kiss"], AlbumArtUrl = imgUrl },
                new Album { AlbumId = ++id, Title = "Unplugged", Genre = genres["Blues"], Price = 8.99M, Artist = artists["Eric Clapton"], AlbumArtUrl = imgUrl },
                new Album { AlbumId = ++id, Title = "Unplugged", Genre = genres["Latin"], Price = 8.99M, Artist = artists["Eric Clapton"], AlbumArtUrl = imgUrl },
                new Album { AlbumId = ++id, Title = "Untrue", Genre = genres["Electronic"], Price = 8.99M, Artist = artists["Burial"], AlbumArtUrl = imgUrl },
                new Album { AlbumId = ++id, Title = "Use Your Illusion I", Genre = genres["Rock"], Price = 8.99M, Artist = artists["Guns N' Roses"], AlbumArtUrl = imgUrl },
                new Album { AlbumId = ++id, Title = "Use Your Illusion II", Genre = genres["Metal"], Price = 8.99M, Artist = artists["Guns N' Roses"], AlbumArtUrl = imgUrl },
                new Album { AlbumId = ++id, Title = "Use Your Illusion II", Genre = genres["Rock"], Price = 8.99M, Artist = artists["Guns N' Roses"], AlbumArtUrl = imgUrl },
                new Album { AlbumId = ++id, Title = "Van Halen III", Genre = genres["Rock"], Price = 8.99M, Artist = artists["Van Halen"], AlbumArtUrl = imgUrl },
                new Album { AlbumId = ++id, Title = "Van Halen", Genre = genres["Rock"], Price = 8.99M, Artist = artists["Van Halen"], AlbumArtUrl = imgUrl },
                new Album { AlbumId = ++id, Title = "Version 2.0", Genre = genres["Alternative"], Price = 8.99M, Artist = artists["Garbage"], AlbumArtUrl = imgUrl },
                new Album { AlbumId = ++id, Title = "Vinicius De Moraes", Genre = genres["Latin"], Price = 8.99M, Artist = artists["Vinícius De Moraes"], AlbumArtUrl = imgUrl },
                new Album { AlbumId = ++id, Title = "Virtual XI", Genre = genres["Rock"], Price = 8.99M, Artist = artists["Iron Maiden"], AlbumArtUrl = imgUrl },
                new Album { AlbumId = ++id, Title = "Voodoo Lounge", Genre = genres["Rock"], Price = 8.99M, Artist = artists["The Rolling Stones"], AlbumArtUrl = imgUrl },
                new Album { AlbumId = ++id, Title = "Vozes do MPB", Genre = genres["Latin"], Price = 8.99M, Artist = artists["Various Artists"], AlbumArtUrl = imgUrl },
                new Album { AlbumId = ++id, Title = "Vs.", Genre = genres["Rock"], Price = 8.99M, Artist = artists["Pearl Jam"], AlbumArtUrl = imgUrl },
                new Album { AlbumId = ++id, Title = "Wagner: Favourite Overtures", Genre = genres["Classical"], Price = 8.99M, Artist = artists["Sir Georg Solti & Wiener Philharmoniker"], AlbumArtUrl = imgUrl },
                new Album { AlbumId = ++id, Title = "Walking Into Clarksdale", Genre = genres["Rock"], Price = 8.99M, Artist = artists["Page & Plant"], AlbumArtUrl = imgUrl },
                new Album { AlbumId = ++id, Title = "Wapi Yo", Genre = genres["World"], Price = 8.99M, Artist = artists["Lokua Kanza"], AlbumArtUrl = imgUrl },
                new Album { AlbumId = ++id, Title = "War", Genre = genres["Rock"], Price = 8.99M, Artist = artists["U2"], AlbumArtUrl = imgUrl },
                new Album { AlbumId = ++id, Title = "Warner 25 Anos", Genre = genres["Jazz"], Price = 8.99M, Artist = artists["Antônio Carlos Jobim"], AlbumArtUrl = imgUrl },
                new Album { AlbumId = ++id, Title = "Wasteland R&Btheque", Genre = genres["Rock"], Price = 8.99M, Artist = artists["Raunchy"], AlbumArtUrl = imgUrl },
                new Album { AlbumId = ++id, Title = "Watermark", Genre = genres["Electronic"], Price = 8.99M, Artist = artists["Enya"], AlbumArtUrl = imgUrl },
                new Album { AlbumId = ++id, Title = "We Were Exploding Anyway", Genre = genres["Rock"], Price = 8.99M, Artist = artists["65daysofstatic"], AlbumArtUrl = imgUrl },
                new Album { AlbumId = ++id, Title = "Weill: The Seven Deadly Sins", Genre = genres["Classical"], Price = 8.99M, Artist = artists["Orchestre de l'Opéra de Lyon"], AlbumArtUrl = imgUrl },
                new Album { AlbumId = ++id, Title = "White Pony", Genre = genres["Rock"], Price = 8.99M, Artist = artists["Deftones"], AlbumArtUrl = imgUrl },
                new Album { AlbumId = ++id, Title = "Who's Next", Genre = genres["Rock"], Price = 8.99M, Artist = artists["The Who"], AlbumArtUrl = imgUrl },
                new Album { AlbumId = ++id, Title = "Wish You Were Here", Genre = genres["Rock"], Price = 8.99M, Artist = artists["Pink Floyd"], AlbumArtUrl = imgUrl },
                new Album { AlbumId = ++id, Title = "With Oden on Our Side", Genre = genres["Metal"], Price = 8.99M, Artist = artists["Amon Amarth"], AlbumArtUrl = imgUrl },
                new Album { AlbumId = ++id, Title = "Worlds", Genre = genres["Jazz"], Price = 8.99M, Artist = artists["Aaron Goldberg"], AlbumArtUrl = imgUrl },
                new Album { AlbumId = ++id, Title = "Worship Music", Genre = genres["Metal"], Price = 8.99M, Artist = artists["Anthrax"], AlbumArtUrl = imgUrl },
                new Album { AlbumId = ++id, Title = "X&Y", Genre = genres["Rock"], Price = 8.99M, Artist = artists["Coldplay"], AlbumArtUrl = imgUrl },
                new Album { AlbumId = ++id, Title = "Xinti", Genre = genres["World"], Price = 8.99M, Artist = artists["Sara Tavares"], AlbumArtUrl = imgUrl },
                new Album { AlbumId = ++id, Title = "Yano", Genre = genres["Rock"], Price = 8.99M, Artist = artists["Yano"], AlbumArtUrl = imgUrl },
                new Album { AlbumId = ++id, Title = "Yesterday Once More Disc 1/Disc 2", Genre = genres["Pop"], Price = 8.99M, Artist = artists["The Carpenters"], AlbumArtUrl = imgUrl },
                new Album { AlbumId = ++id, Title = "Zooropa", Genre = genres["Rock"], Price = 8.99M, Artist = artists["U2"], AlbumArtUrl = imgUrl },
                new Album { AlbumId = ++id, Title = "Zoso", Genre = genres["Rock"], Price = 8.99M, Artist = artists["Led Zeppelin"], AlbumArtUrl = imgUrl },
            };

            foreach (var album in albums)
            {
                album.ArtistId = album.Artist.ArtistId;
                album.GenreId = album.Genre.GenreId;
            }

            return albums;
        }
        private static Dictionary<string, Artist> artists;
        private static Dictionary<string, Artist> Artists
        {
            get
            {
                int id = 888880;

                if (artists == null)
                {
                    var artistsList = new Artist[]
                    {
                        new Artist { ArtistId = ++id, Name = "65daysofstatic" },
                        new Artist { ArtistId = ++id, Name = "Aaron Goldberg" },
                        new Artist { ArtistId = ++id, Name = "Above & Beyond" },
                        new Artist { ArtistId = ++id ,Name = "Above the Fold" },
                        new Artist { ArtistId = ++id, Name = "AC/DC" },
                        new Artist { ArtistId = ++id, Name = "Accept" },
                        new Artist { ArtistId = ++id, Name = "Adicts" },
                        new Artist { ArtistId = ++id, Name = "Adrian Leaper & Doreen de Feis" },
                        new Artist { ArtistId = ++id, Name = "Aerosmith" },
                        new Artist { ArtistId = ++id, Name = "Aisha Duo" },
                        new Artist { ArtistId = ++id,  Name = "Al di Meola" },
                        new Artist { ArtistId = ++id, Name = "Alabama Shakes" },
                        new Artist { ArtistId = ++id, Name = "Alanis Morissette" },
                        new Artist { ArtistId = ++id, Name = "Alberto Turco & Nova Schola Gregoriana" },
                        new Artist { ArtistId = ++id, Name = "Alice in Chains" },
                        new Artist { ArtistId = ++id, Name = "Alison Krauss" },
                        new Artist { ArtistId = ++id, Name = "Amon Amarth" },
                        new Artist { ArtistId = ++id, Name = "Amon Tobin" },
                        new Artist { ArtistId = ++id, Name = "Amr Diab" },
                        new Artist { ArtistId = ++id,  Name = "Amy Winehouse" },
                        new Artist { ArtistId = ++id,  Name = "Anita Ward" },
                        new Artist { ArtistId = ++id, Name = "Anthrax" },
                        new Artist { ArtistId = ++id, Name = "Antônio Carlos Jobim" },
                        new Artist { ArtistId = ++id, Name = "Apocalyptica" },
                        new Artist { ArtistId = ++id, Name = "Aqua" },
                        new Artist { ArtistId = ++id, Name = "Armand Van Helden" },
                        new Artist { ArtistId = ++id, Name = "Arcade Fire" },
                        new Artist { ArtistId = ++id, Name = "Audioslave" },
                        new Artist { ArtistId = ++id, Name = "Bad Religion" },
                        new Artist { ArtistId = ++id, Name = "Barenaked Ladies" },
                        new Artist { ArtistId = ++id, Name = "BBC Concert Orchestra" },
                        new Artist { ArtistId = ++id, Name = "Bee Gees" },
                        new Artist { ArtistId = ++id, Name = "Before the Dawn" },
                        new Artist { ArtistId = ++id, Name = "Berliner Philharmoniker" },
                        new Artist { ArtistId = ++id, Name = "Billy Cobham" },
                        new Artist { ArtistId = ++id, Name = "Black Label Society" },
                        new Artist { ArtistId = ++id, Name = "Black Sabbath" },
                        new Artist { ArtistId = ++id, Name = "BLØF" },
                        new Artist { ArtistId = ++id, Name = "Blues Traveler" },
                        new Artist { ArtistId = ++id, Name = "Boston Symphony Orchestra & Seiji Ozawa" },
                        new Artist { ArtistId = ++id, Name = "Britten Sinfonia, Ivor Bolton & Lesley Garrett" },
                        new Artist { ArtistId = ++id, Name = "Bruce Dickinson" },
                        new Artist { ArtistId = ++id, Name = "Buddy Guy" },
                        new Artist { ArtistId = ++id, Name = "Burial" },
                        new Artist { ArtistId = ++id, Name = "Butch Walker & The Black Widows" },
                        new Artist { ArtistId = ++id, Name = "Caetano Veloso" },
                        new Artist { ArtistId = ++id, Name = "Cake" },
                        new Artist { ArtistId = ++id, Name = "Calexico" },
                        new Artist { ArtistId = ++id, Name = "Carly Rae Jepsen" },
                        new Artist { ArtistId = ++id, Name = "Carreras, Pavarotti, Domingo" },
                        new Artist { ArtistId = ++id, Name = "Cássia Eller" },
                        new Artist { ArtistId = ++id, Name = "Cayouche" },
                        new Artist { ArtistId = ++id, Name = "Chic" },
                        new Artist { ArtistId = ++id, Name = "Chicago " },
                        new Artist { ArtistId = ++id, Name = "Chicago Symphony Orchestra & Fritz Reiner" },
                        new Artist { ArtistId = ++id, Name = "Chico Buarque" },
                        new Artist { ArtistId = ++id, Name = "Chico Science & Nação Zumbi" },
                        new Artist { ArtistId = ++id, Name = "Choir Of Westminster Abbey & Simon Preston" },
                        new Artist { ArtistId = ++id, Name = "Chris Cornell" },
                        new Artist { ArtistId = ++id, Name = "Christopher O'Riley" },
                        new Artist { ArtistId = ++id, Name = "Cidade Negra" },
                        new Artist { ArtistId = ++id, Name = "Cláudio Zoli" },
                        new Artist { ArtistId = ++id, Name = "Coldplay" },
                        new Artist { ArtistId = ++id, Name = "Creedence Clearwater Revival" },
                        new Artist { ArtistId = ++id, Name = "Crosby, Stills, Nash, and Young" },
                        new Artist { ArtistId = ++id, Name = "Daft Punk" },
                        new Artist { ArtistId = ++id, Name = "Danielson Famile" },
                        new Artist { ArtistId = ++id, Name = "David Bowie" },
                        new Artist { ArtistId = ++id, Name = "David Coverdale" },
                        new Artist { ArtistId = ++id, Name = "David Guetta" },
                        new Artist { ArtistId = ++id, Name = "deadmau5" },
                        new Artist { ArtistId = ++id, Name = "Deep Purple" },
                        new Artist { ArtistId = ++id, Name = "Def Leppard" },
                        new Artist { ArtistId = ++id, Name = "Deftones" },
                        new Artist { ArtistId = ++id, Name = "Dennis Chambers" },
                        new Artist { ArtistId = ++id, Name = "Deva Premal" },
                        new Artist { ArtistId = ++id, Name = "Dio" },
                        new Artist { ArtistId = ++id, Name = "Djavan" },
                        new Artist { ArtistId = ++id, Name = "Dolly Parton" },
                        new Artist { ArtistId = ++id, Name = "Donna Summer" },
                        new Artist { ArtistId = ++id, Name = "Dr. Dre" },
                        new Artist { ArtistId = ++id, Name = "Dread Zeppelin" },
                        new Artist { ArtistId = ++id, Name = "Dream Theater" },
                        new Artist { ArtistId = ++id, Name = "Duck Sauce" },
                        new Artist { ArtistId = ++id, Name = "Earl Scruggs" },
                        new Artist { ArtistId = ++id, Name = "Ed Motta" },
                        new Artist { ArtistId = ++id, Name = "Edo de Waart & San Francisco Symphony" },
                        new Artist { ArtistId = ++id, Name = "Elis Regina" },
                        new Artist { ArtistId = ++id, Name = "Eminem" },
                        new Artist { ArtistId = ++id, Name = "English Concert & Trevor Pinnock" },
                        new Artist { ArtistId = ++id, Name = "Enya" },
                        new Artist { ArtistId = ++id, Name = "Epica" },
                        new Artist { ArtistId = ++id, Name = "Eric B. and Rakim" },
                        new Artist { ArtistId = ++id, Name = "Eric Clapton" },
                        new Artist { ArtistId = ++id, Name = "Eugene Ormandy" },
                        new Artist { ArtistId = ++id, Name = "Faith No More" },
                        new Artist { ArtistId = ++id, Name = "Falamansa" },
                        new Artist { ArtistId = ++id, Name = "Filter" },
                        new Artist { ArtistId = ++id, Name = "Foo Fighters" },
                        new Artist { ArtistId = ++id, Name = "Four Tet" },
                        new Artist { ArtistId = ++id, Name = "Frank Zappa & Captain Beefheart" },
                        new Artist { ArtistId = ++id, Name = "Fretwork" },
                        new Artist { ArtistId = ++id, Name = "Funk Como Le Gusta" },
                        new Artist { ArtistId = ++id, Name = "Garbage" },
                        new Artist { ArtistId = ++id, Name = "Gerald Moore" },
                        new Artist { ArtistId = ++id, Name = "Gilberto Gil" },
                        new Artist { ArtistId = ++id, Name = "Godsmack" },
                        new Artist { ArtistId = ++id, Name = "Gonzaguinha" },
                        new Artist { ArtistId = ++id, Name = "Göteborgs Symfoniker & Neeme Järvi" },
                        new Artist { ArtistId = ++id, Name = "Guns N' Roses" },
                        new Artist { ArtistId = ++id, Name = "Gustav Mahler" },
                        new Artist { ArtistId = ++id, Name = "In This Moment" },
                        new Artist { ArtistId = ++id, Name = "Incognito" },
                        new Artist { ArtistId = ++id, Name = "INXS" },
                        new Artist { ArtistId = ++id, Name = "Iron Maiden" },
                        new Artist { ArtistId = ++id, Name = "Jagjit Singh" },
                        new Artist { ArtistId = ++id, Name = "James Levine" },
                        new Artist { ArtistId = ++id, Name = "Jamiroquai" },
                        new Artist { ArtistId = ++id, Name = "Jimi Hendrix" },
                        new Artist { ArtistId = ++id, Name = "Jimmy Buffett" },
                        new Artist { ArtistId = ++id, Name = "Jimmy Smith" },
                        new Artist { ArtistId = ++id, Name = "Joe Satriani" },
                        new Artist { ArtistId = ++id, Name = "John Digweed" },
                        new Artist { ArtistId = ++id, Name = "John Mayer" },
                        new Artist { ArtistId = ++id, Name = "Jorge Ben" },
                        new Artist { ArtistId = ++id, Name = "Jota Quest" },
                        new Artist { ArtistId = ++id, Name = "Journey" },
                        new Artist { ArtistId = ++id,  Name = "Judas Priest" },
                        new Artist { ArtistId = ++id, Name = "Julian Bream" },
                        new Artist { ArtistId = ++id, Name = "Justice" },
                        new Artist { ArtistId = ++id, Name = "Orchestre de l'Opéra de Lyon" },
                        new Artist { ArtistId = ++id, Name = "King Crimson" },
                        new Artist { ArtistId = ++id, Name = "Kiss" },
                        new Artist { ArtistId = ++id, Name = "LCD Soundsystem" },
                        new Artist { ArtistId = ++id, Name = "Le Tigre" },
                        new Artist { ArtistId = ++id, Name = "Led Zeppelin" },
                        new Artist { ArtistId = ++id, Name = "Legião Urbana" },
                        new Artist { ArtistId = ++id, Name = "Lenny Kravitz" },
                        new Artist { ArtistId = ++id, Name = "Les Arts Florissants & William Christie" },
                        new Artist { ArtistId = ++id, Name = "Limp Bizkit" },
                        new Artist { ArtistId = ++id, Name = "Linkin Park" },
                        new Artist { ArtistId = ++id, Name = "Live" },
                        new Artist { ArtistId = ++id, Name = "Lokua Kanza" },
                        new Artist { ArtistId = ++id,  Name = "London Symphony Orchestra" },
                        new Artist { ArtistId = ++id,  Name = "Los Tigres del Norte" },
                        new Artist { ArtistId = ++id, Name = "Luciana Souza/Romero Lubambo" },
                        new Artist { ArtistId = ++id, Name = "Lulu Santos" },
                        new Artist { ArtistId = ++id, Name = "Lura" },
                        new Artist { ArtistId = ++id, Name = "Marcos Valle" },
                        new Artist { ArtistId = ++id, Name = "Marillion" },
                        new Artist { ArtistId = ++id, Name = "Marisa Monte" },
                        new Artist { ArtistId = ++id, Name = "Mark Knopfler" },
                        new Artist { ArtistId = ++id, Name = "Martin Roscoe" },
                        new Artist { ArtistId = ++id, Name = "Massive Attack" },
                        new Artist { ArtistId = ++id, Name = "Maurizio Pollini" },
                        new Artist { ArtistId = ++id, Name = "Megadeth" },
                        new Artist { ArtistId = ++id, Name = "Mela Tenenbaum, Pro Musica Prague & Richard Kapp" },
                        new Artist { ArtistId = ++id, Name = "Melanie Fiona" },
                        new Artist { ArtistId = ++id, Name = "Men At Work" },
                        new Artist { ArtistId = ++id, Name = "Metallica" },
                        new Artist { ArtistId = ++id, Name = "M-Flo" },
                        new Artist { ArtistId = ++id,  Name = "Michael Bolton" },
                        new Artist { ArtistId = ++id, Name = "Michael Tilson Thomas" },
                        new Artist { ArtistId = ++id, Name = "Miles Davis" },
                        new Artist { ArtistId = ++id, Name = "Milton Nascimento" },
                        new Artist { ArtistId = ++id, Name = "Mobile" },
                        new Artist { ArtistId = ++id, Name = "Modest Mouse" },
                        new Artist { ArtistId = ++id, Name = "Mötley Crüe" },
                        new Artist { ArtistId = ++id, Name = "Motörhead" },
                        new Artist { ArtistId = ++id, Name = "Mumford & Sons" },
                        new Artist { ArtistId = ++id, Name = "Munkle" },
                        new Artist { ArtistId = ++id, Name = "Nash Ensemble" },
                        new Artist { ArtistId = ++id, Name = "Neil Young" },
                        new Artist { ArtistId = ++id, Name = "New York Dolls" },
                        new Artist { ArtistId = ++id, Name = "Nick Cave and the Bad Seeds" },
                        new Artist { ArtistId = ++id, Name = "Nicolaus Esterhazy Sinfonia" },
                        new Artist { ArtistId = ++id, Name = "Nine Inch Nails" },
                        new Artist { ArtistId = ++id, Name = "Nirvana" },
                        new Artist { ArtistId = ++id, Name = "Norah Jones" },
                        new Artist { ArtistId = ++id, Name = "Nujabes" },
                        new Artist { ArtistId = ++id, Name = "O Terço" },
                        new Artist { ArtistId = ++id, Name = "Oasis" },
                        new Artist { ArtistId = ++id, Name = "Olodum" },
                        new Artist { ArtistId = ++id, Name = "Opeth" },
                        new Artist { ArtistId = ++id, Name = "Orchestra of The Age of Enlightenment" },
                        new Artist { ArtistId = ++id, Name = "Os Paralamas Do Sucesso" },
                        new Artist { ArtistId = ++id, Name = "Ozzy Osbourne" },
                        new Artist { ArtistId = ++id, Name = "Paddy Casey" },
                        new Artist { ArtistId = ++id, Name = "Page & Plant" },
                        new Artist { ArtistId = ++id, Name = "Papa Wemba" },
                        new Artist { ArtistId = ++id, Name = "Paul D'Ianno" },
                        new Artist { ArtistId = ++id, Name = "Paul Oakenfold" },
                        new Artist { ArtistId = ++id, Name = "Paul Van Dyk" },
                        new Artist { ArtistId = ++id, Name = "Pearl Jam" },
                        new Artist { ArtistId = ++id, Name = "Pet Shop Boys" },
                        new Artist { ArtistId = ++id, Name = "Pink Floyd" },
                        new Artist { ArtistId = ++id, Name = "Plug" },
                        new Artist { ArtistId = ++id, Name = "Porcupine Tree" },
                        new Artist { ArtistId = ++id, Name = "Portishead" },
                        new Artist { ArtistId = ++id, Name = "Prince" },
                        new Artist { ArtistId = ++id, Name = "Projected" },
                        new Artist { ArtistId = ++id, Name = "PSY" },
                        new Artist { ArtistId = ++id, Name = "Public Enemy" },
                        new Artist { ArtistId = ++id, Name = "Queen" },
                        new Artist { ArtistId = ++id, Name = "Queensrÿche" },
                        new Artist { ArtistId = ++id, Name = "R.E.M." },
                        new Artist { ArtistId = ++id, Name = "Radiohead" },
                        new Artist { ArtistId = ++id, Name = "Rancid" },
                        new Artist { ArtistId = ++id,  Name = "Raul Seixas" },
                        new Artist { ArtistId = ++id, Name = "Raunchy" },
                        new Artist { ArtistId = ++id, Name = "Red Hot Chili Peppers" },
                        new Artist { ArtistId = ++id, Name = "Rick Ross" },
                        new Artist { ArtistId = ++id, Name = "Robert James" },
                        new Artist { ArtistId = ++id, Name = "London Classical Players" },
                        new Artist { ArtistId = ++id, Name = "Royal Philharmonic Orchestra" },
                        new Artist { ArtistId = ++id, Name = "Run DMC" },
                        new Artist { ArtistId = ++id, Name = "Rush" },
                        new Artist { ArtistId = ++id, Name = "Santana" },
                        new Artist { ArtistId = ++id, Name = "Sara Tavares" },
                        new Artist { ArtistId = ++id, Name = "Sarah Brightman" },
                        new Artist { ArtistId = ++id, Name = "Sasha" },
                        new Artist { ArtistId = ++id, Name = "Scholars Baroque Ensemble" },
                        new Artist { ArtistId = ++id, Name = "Scorpions" },
                        new Artist { ArtistId = ++id, Name = "Sergei Prokofiev & Yuri Temirkanov" },
                        new Artist { ArtistId = ++id, Name = "Sheryl Crow" },
                        new Artist { ArtistId = ++id, Name = "Sir Georg Solti & Wiener Philharmoniker" },
                        new Artist { ArtistId = ++id, Name = "Skank" },
                        new Artist { ArtistId = ++id, Name = "Skrillex" },
                        new Artist { ArtistId = ++id, Name = "Slash" },
                        new Artist { ArtistId = ++id, Name = "Slayer" },
                        new Artist { ArtistId = ++id, Name = "Soul-Junk" },
                        new Artist { ArtistId = ++id, Name = "Soundgarden" },
                        new Artist { ArtistId = ++id, Name = "Spyro Gyra" },
                        new Artist { ArtistId = ++id, Name = "Stevie Ray Vaughan & Double Trouble" },
                        new Artist {ArtistId = ++id, Name = "Stevie Ray Vaughan" },
                        new Artist {ArtistId = ++id, Name = "Sting" },
                        new Artist {ArtistId = ++id, Name = "Stone Temple Pilots" },
                        new Artist {ArtistId = ++id, Name = "Styx" },
                        new Artist {ArtistId = ++id, Name = "Sufjan Stevens" },
                        new Artist {ArtistId = ++id, Name = "Supreme Beings of Leisure" },
                        new Artist {ArtistId = ++id, Name = "System Of A Down" },
                        new Artist {ArtistId = ++id, Name = "T&N" },
                        new Artist {ArtistId = ++id, Name = "Talking Heads" },
                        new Artist {ArtistId = ++id, Name = "Tears For Fears" },
                        new Artist {ArtistId = ++id, Name = "Ted Nugent" },
                        new Artist {ArtistId = ++id, Name = "Temple of the Dog" },
                        new Artist {ArtistId = ++id, Name = "Terry Bozzio, Tony Levin & Steve Stevens" },
                        new Artist {ArtistId = ++id, Name = "The 12 Cellists of The Berlin Philharmonic" },
                        new Artist {ArtistId = ++id, Name = "The Axis of Awesome" },
                        new Artist {ArtistId = ++id, Name = "The Beatles" },
                        new Artist {ArtistId = ++id, Name = "The Black Crowes" },
                        new Artist {ArtistId = ++id, Name = "The Black Keys" },
                        new Artist {ArtistId = ++id, Name = "The Carpenters" },
                        new Artist {ArtistId = ++id, Name = "The Cat Empire" },
                        new Artist {ArtistId = ++id, Name = "The Cult" },
                        new Artist {ArtistId = ++id, Name = "The Cure" },
                        new Artist {ArtistId = ++id, Name = "The Decemberists" },
                        new Artist {ArtistId = ++id, Name = "The Doors" },
                        new Artist {ArtistId = ++id, Name = "The Eagles of Death Metal" },
                        new Artist {ArtistId = ++id, Name = "The Go! Team" },
                        new Artist {ArtistId = ++id, Name = "The Head and the Heart" },
                        new Artist {ArtistId = ++id, Name = "The Jezabels" },
                        new Artist {ArtistId = ++id, Name = "The King's Singers" },
                        new Artist {ArtistId = ++id, Name = "The Lumineers" },
                        new Artist {ArtistId = ++id, Name = "The Offspring" },
                        new Artist {ArtistId = ++id, Name = "The Police" },
                        new Artist {ArtistId = ++id, Name = "The Posies" },
                        new Artist {ArtistId = ++id, Name = "The Prodigy" },
                        new Artist {ArtistId = ++id, Name = "The Rolling Stones" },
                        new Artist {ArtistId = ++id, Name = "The Rubberbandits" },
                        new Artist {ArtistId = ++id, Name = "The Smashing Pumpkins" },
                        new Artist {ArtistId = ++id, Name = "The Stone Roses" },
                        new Artist {ArtistId = ++id, Name = "The Who" },
                        new Artist {ArtistId = ++id, Name = "Them Crooked Vultures" },
                        new Artist {ArtistId = ++id, Name = "TheStart" },
                        new Artist {ArtistId = ++id, Name = "Thievery Corporation" },
                        new Artist {ArtistId = ++id, Name = "Tiësto" },
                        new Artist {ArtistId = ++id, Name = "Tim Maia" },
                        new Artist {ArtistId = ++id, Name = "Ton Koopman" },
                        new Artist {ArtistId = ++id, Name = "Tool" },
                        new Artist {ArtistId = ++id, Name = "Tori Amos" },
                        new Artist {ArtistId = ++id, Name = "Trampled By Turtles" },
                        new Artist {ArtistId = ++id, Name = "Trans-Siberian Orchestra" },
                        new Artist {ArtistId = ++id, Name = "Tygers of Pan Tang" },
                        new Artist {ArtistId = ++id, Name = "U2" },
                        new Artist {ArtistId = ++id, Name = "UB40" },
                        new Artist {ArtistId = ++id, Name = "Uh Huh Her " },
                        new Artist {ArtistId = ++id, Name = "Van Halen" },
                        new Artist {ArtistId = ++id, Name = "Various Artists" },
                        new Artist {ArtistId = ++id, Name = "Velvet Revolver" },
                        new Artist {ArtistId = ++id, Name = "Venus Hum" },
                        new Artist {ArtistId = ++id, Name = "Vicente Fernandez" },
                        new Artist {ArtistId = ++id, Name = "Vinícius De Moraes" },
                        new Artist {ArtistId = ++id, Name = "Weezer" },
                        new Artist {ArtistId = ++id, Name = "Weird Al" },
                        new Artist {ArtistId = ++id, Name = "Wendy Carlos" },
                        new Artist {ArtistId = ++id, Name = "Wilhelm Kempff" },
                        new Artist {ArtistId = ++id, Name = "Yano" },
                        new Artist {ArtistId = ++id, Name = "Yehudi Menuhin" },
                        new Artist {ArtistId = ++id, Name = "Yes" },
                        new Artist {ArtistId = ++id, Name = "Yo-Yo Ma" },
                        new Artist {ArtistId = ++id, Name = "Zeca Pagodinho" },
                        //new Artist { Name = "אריק אינשטיין"}
                    };

                    artists = new Dictionary<string, Artist>();
                    foreach (Artist artist in artistsList)
                    {
                        artists.Add(artist.Name, artist);
                    }
                }

                return artists;
            }
        }
        private static Dictionary<string, Genre> genres;
        private static Dictionary<string, Genre> Genres
        {
            get
            {
                int id = 7777770;
                if (genres == null)
                {
                    var genresList = new Genre[]
                    {
                        new Genre { GenreId = ++id, Name = "Pop" },
                        new Genre { GenreId = ++id, Name = "Rock" },
                        new Genre { GenreId = ++id, Name = "Jazz" },
                        new Genre { GenreId = ++id, Name = "Metal" },
                        new Genre { GenreId = ++id, Name = "Electronic" },
                        new Genre { GenreId = ++id, Name = "Blues" },
                        new Genre { GenreId = ++id, Name = "Latin" },
                        new Genre { GenreId = ++id, Name = "Rap" },
                        new Genre { GenreId = ++id, Name = "Classical" },
                        new Genre { GenreId = ++id, Name = "Alternative" },
                        new Genre { GenreId = ++id, Name = "Country" },
                        new Genre { GenreId = ++id, Name = "R&B" },
                        new Genre { GenreId = ++id, Name = "Indie" },
                        new Genre { GenreId = ++id, Name = "Punk" },
                        new Genre { GenreId = ++id, Name = "World" }
                    };

                    genres = new Dictionary<string, Genre>();

                    foreach (Genre genre in genresList)
                    {
                        genres.Add(genre.Name, genre);
                    }
                }

                return genres;
            }
        }
        private static void BuildFallbackData()
        {
            var albums = GetAlbums();
            Array.Resize(ref albums, 100);
            _fallbackAlbums = albums.ToList();

            
            Dictionary<int, Genre> genres = new Dictionary<int, Genre>();
            Dictionary<int, Artist> artists = new Dictionary<int, Artist>();

            foreach (var a in FallbackAlbums)
            {
                var genre = a.Genre;
                a.GenreId = genre.GenreId;

                var artist = a.Artist;
                a.ArtistId = artist.ArtistId;

                if (!genres.ContainsKey(genre.GenreId))
                {
                    genres.Add(genre.GenreId, genre);
                }
            
                if (genre.Albums == null)
                {
                    genre.Albums = new List<Album>();
                }
                genre.Albums.Add(a);
 
                if (!artists.ContainsKey(artist.ArtistId))
                {
                    artists.Add(artist.ArtistId, artist);
                }
            }

            _fallbackGenre = genres.Values.ToList();
            _fallbackArtists = artists.Values.ToList();
        }

        private static List<Album> _fallbackAlbums;
        public static List<Album> FallbackAlbums
        {
            get
            {
                return _fallbackAlbums;
            }
        }
        private static List<Genre> _fallbackGenre;
        public static List<Genre> FallbackGenres
        {
            get
            {
                return _fallbackGenre;
            }
        }

        private static List<Artist> _fallbackArtists;
        public static List<Artist> FallbackArtists
        {
            get
            {
                return _fallbackArtists;
            }
        }
    }
}

