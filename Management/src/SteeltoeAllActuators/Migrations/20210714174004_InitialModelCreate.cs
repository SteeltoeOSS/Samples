using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SteeltoeAllActuators.Migrations
{
    public partial class InitialModelCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Employees",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FirstName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Company = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Employees", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "Employees",
                columns: new[] { "Id", "Company", "FirstName", "LastName", "Title" },
                values: new object[,]
                {
                    { new Guid("5e2776de-9a6e-7717-9a03-9983a3c6fdfa"), "Kuhic Inc", "Ally", "Becker", "seize ubiquitous content" },
                    { new Guid("2a73f491-6ce7-0a9f-8132-23f498ec4b0f"), "Morissette, Lockman and O'Keefe", "Erin", "Kozey", "incentivize 24/365 portals" },
                    { new Guid("2a1908a2-8eaf-4952-89b6-a24efb31c584"), "Witting - Boyle", "Rahsaan", "Rosenbaum", "deliver virtual users" },
                    { new Guid("b14bbf80-c155-bdbc-509a-456485dfd1a4"), "Ernser - Botsford", "Melissa", "Hickle", "integrate wireless infrastructures" },
                    { new Guid("39668099-3196-f33a-3fb8-aaf042a0f3c7"), "Bailey Group", "Jessica", "Steuber", "engineer 24/7 relationships" },
                    { new Guid("aa20fcca-929e-bfb9-8915-18d45a82261e"), "Langworth - McDermott", "Danial", "Prosacco", "iterate end-to-end partnerships" },
                    { new Guid("c23b47b7-d4c2-384a-8143-6f9a118f54ad"), "Kuvalis - McClure", "Hilton", "Murazik", "redefine seamless eyeballs" },
                    { new Guid("9a00dfbf-313c-e9ee-99ef-f35756f7fcb4"), "D'Amore LLC", "Micaela", "Effertz", "iterate out-of-the-box models" },
                    { new Guid("48cf76fc-3579-98a8-dac3-02e3d3affc4f"), "Yundt, Howe and Konopelski", "Dulce", "Johns", "incentivize rich e-tailers" },
                    { new Guid("cba960b3-6556-d133-309d-c562a56f6446"), "Mitchell, Yost and Kessler", "Karson", "Friesen", "exploit enterprise convergence" },
                    { new Guid("eb8b8cc2-ff34-c81f-59c6-d4ffcc835dcf"), "Stamm - Cormier", "Matilda", "Thompson", "envisioneer front-end models" },
                    { new Guid("c48041db-d612-44d2-c6c8-93d9a8e5fb9d"), "Jast Inc", "Audreanne", "Little", "target turn-key bandwidth" },
                    { new Guid("3c072534-ab09-1416-75ee-10f27f5bb583"), "Rath - Berge", "Ronaldo", "Bartell", "target clicks-and-mortar schemas" },
                    { new Guid("529074a8-6d61-930d-b486-8fedcf963b1a"), "Torphy, Kris and Hayes", "Rossie", "Welch", "incubate holistic applications" },
                    { new Guid("b3a46241-9748-26c2-1831-240c1dd3154a"), "Runte, Heidenreich and Kessler", "Eliezer", "Gerlach", "harness strategic infomediaries" },
                    { new Guid("0effb5c8-0988-feb0-db94-941bc3b208e0"), "Prosacco, McDermott and Heaney", "Kieran", "Gislason", "engineer magnetic metrics" },
                    { new Guid("25cb58d9-1c7f-5714-aaf1-36988f9185f5"), "Mayert, Smith and Hackett", "Judah", "Homenick", "visualize rich models" },
                    { new Guid("5cc0d9ec-1ead-a464-c594-fdd330c65184"), "Effertz, Ledner and Carroll", "Rosalinda", "Veum", "drive virtual networks" },
                    { new Guid("b2982a68-060a-b861-aa92-4253fbfdd2d1"), "Runolfsson Inc", "Marjory", "Lueilwitz", "morph user-centric content" },
                    { new Guid("636ef095-971d-7b9d-5250-45f4ccf2ba8d"), "Armstrong Group", "Clement", "Marquardt", "benchmark viral users" },
                    { new Guid("84f023c1-e660-5ae9-1b93-1d1998fd9a12"), "Auer Inc", "Rozella", "Nienow", "deliver cross-media bandwidth" },
                    { new Guid("06eb6f0f-da88-64f5-8c07-a2f9a6191b6b"), "Volkman, Kerluke and Cremin", "Nayeli", "Turner", "implement user-centric ROI" },
                    { new Guid("2f483f73-aeb9-1fe2-ef74-f1fbe3d1409e"), "Kuhn, Kihn and Mueller", "Alize", "Ward", "drive web-enabled niches" },
                    { new Guid("cb05cd44-6e2d-4c7a-8515-9bad35d358c0"), "Bogan - Upton", "Malika", "Dach", "deliver plug-and-play relationships" },
                    { new Guid("64141b9a-e044-6306-9c99-75e1dc53c106"), "Von, Robel and Cassin", "Heather", "Greenholt", "extend turn-key users" },
                    { new Guid("37b5c59a-2481-b4c0-6d31-115a1608a40e"), "Fadel and Sons", "Einar", "Franecki", "disintermediate 24/7 eyeballs" },
                    { new Guid("25c2b39e-f35c-97a9-5a2c-aa1b512b1b75"), "Hamill, Cartwright and McCullough", "Esteban", "Gaylord", "streamline compelling relationships" },
                    { new Guid("e8e06cae-c05b-3eb6-c099-cd619b587987"), "Champlin, Jacobs and Langworth", "Davon", "Strosin", "envisioneer enterprise experiences" },
                    { new Guid("64734aa9-f245-1c53-6cad-87c8afff6cf3"), "Rosenbaum, Littel and Dach", "Jermain", "Schroeder", "utilize sexy users" },
                    { new Guid("6d207152-9e56-4556-6d99-b70e842b7cde"), "Wiegand - Larson", "Silas", "Leannon", "transform frictionless niches" },
                    { new Guid("7421c466-9986-354c-e2d1-5780902976f2"), "Larkin and Sons", "Reggie", "Hauck", "whiteboard front-end web-readiness" },
                    { new Guid("8a5bea2c-3ed1-9c48-e493-ea8f614833a0"), "Conn - Bauch", "Casper", "Christiansen", "utilize scalable users" },
                    { new Guid("84417802-eb6a-48b4-3053-d5bf7da8551c"), "Ortiz Group", "Astrid", "Monahan", "integrate dynamic portals" },
                    { new Guid("d5f7be0c-dc49-4b1e-77f3-60c17f2c37f9"), "Gerhold - Wisozk", "Jake", "King", "expedite cutting-edge experiences" },
                    { new Guid("45888b7e-1cea-30d7-8f42-52b0a3422ebe"), "Cormier LLC", "Cordia", "Leuschke", "recontextualize out-of-the-box niches" },
                    { new Guid("8d7f0d95-dab3-435f-a909-56bd53d8615a"), "O'Keefe, Towne and Kling", "Alexanne", "DuBuque", "transition dynamic ROI" },
                    { new Guid("bc38273b-8864-1142-d1af-dd9d497cb5e2"), "Conroy, Jones and Schimmel", "Ewell", "Kunze", "harness vertical technologies" },
                    { new Guid("3d0a3594-0ddc-beb6-2b78-b32786458114"), "Hessel, Brekke and Morissette", "Lorenzo", "Russel", "recontextualize open-source applications" },
                    { new Guid("d5bd80ca-d1f1-4c1a-0bd0-5238d4f5a226"), "Hyatt Group", "Virginie", "Williamson", "deploy 24/365 supply-chains" },
                    { new Guid("e3e9c3d6-f467-81c1-8de2-118cf9e939a6"), "Kutch, Langworth and Harber", "Sierra", "Murray", "engage viral metrics" },
                    { new Guid("afb4de5d-9f7b-bf42-a4fe-2ee837e4160f"), "King, Donnelly and Okuneva", "Carroll", "Langworth", "deliver frictionless ROI" },
                    { new Guid("4c0ce168-fae9-5ceb-5c0a-160cd11243cf"), "Mertz, Schuppe and Maggio", "Ephraim", "Keebler", "iterate holistic technologies" }
                });

            migrationBuilder.InsertData(
                table: "Employees",
                columns: new[] { "Id", "Company", "FirstName", "LastName", "Title" },
                values: new object[,]
                {
                    { new Guid("a8e6315d-2f71-d0e8-5212-bd3572ba0442"), "Olson - Funk", "Iliana", "Schneider", "deploy intuitive relationships" },
                    { new Guid("0ad94713-9ed8-eb06-a7e7-8cc804b3eada"), "Miller and Sons", "Kaya", "Langworth", "synergize open-source infomediaries" },
                    { new Guid("8633c8e6-641b-5d63-9938-952956ddf7b4"), "Cummerata, Corkery and Reilly", "Retta", "Moen", "brand cross-platform web services" },
                    { new Guid("80c5d300-2998-ed79-3f24-0f6b80cf4384"), "Jones Inc", "Meghan", "Spinka", "mesh world-class functionalities" },
                    { new Guid("0cb2f3cb-6bcf-8aaa-5b96-1a919ddd010f"), "Sipes LLC", "Carlos", "McCullough", "optimize turn-key systems" },
                    { new Guid("0cff064a-b972-7c2e-8380-b5a4766c1dd4"), "Hintz - Beer", "Christa", "Padberg", "enable plug-and-play partnerships" },
                    { new Guid("8402bc2d-956c-815d-68b0-c2c86c9e17a2"), "Mayert, Harber and Herman", "Theresa", "Block", "scale extensible e-business" },
                    { new Guid("39614043-4da2-1f98-aedf-b5b7aa10ed23"), "Hauck Inc", "Eve", "Nikolaus", "productize e-business mindshare" },
                    { new Guid("a416816c-28ca-24d1-5265-2955fb48c562"), "Stanton, Maggio and Jast", "Sunny", "Schaefer", "syndicate cross-media partnerships" },
                    { new Guid("d21ff4f9-c905-65db-f4cc-2036f355850d"), "Yundt, Kshlerin and Emard", "Camron", "Hessel", "transform integrated models" },
                    { new Guid("1caaac80-810b-89c3-84d1-33b1514d9b4e"), "Bechtelar - West", "Marquis", "Brown", "recontextualize end-to-end initiatives" },
                    { new Guid("7887d1b1-87a6-09d0-a86e-841d3049e94b"), "Durgan - Schiller", "Serena", "Harber", "enable best-of-breed blockchains" },
                    { new Guid("21a898a9-d16f-4a37-5872-6b39fa4be2fc"), "Wilderman, Larkin and Dicki", "Joanny", "Upton", "facilitate back-end paradigms" },
                    { new Guid("c67e4830-e6cf-9154-70cf-0ddeaf9ad737"), "Torp, Gutkowski and Purdy", "Gust", "McLaughlin", "expedite enterprise blockchains" },
                    { new Guid("5c532783-2a4e-027b-033f-89b984898b16"), "Maggio - Schuster", "Nicola", "Kilback", "extend open-source convergence" },
                    { new Guid("5eb98eab-6ec8-36df-b353-4bbe013521a4"), "Stroman, Osinski and Torp", "Lonie", "Mante", "envisioneer turn-key eyeballs" },
                    { new Guid("96c607c7-55f6-9832-21b0-c0711c39945f"), "Mills, Cruickshank and Stroman", "Alana", "Greenholt", "architect user-centric models" },
                    { new Guid("34a44141-2ee3-7ed7-2b01-6719a0bdfe18"), "Doyle LLC", "Earnestine", "Maggio", "scale collaborative niches" },
                    { new Guid("8578fafb-66fc-95b2-5dbf-b4138e633e82"), "Kuphal - Block", "Kirstin", "Gorczany", "target seamless bandwidth" },
                    { new Guid("35a62632-07ed-cca3-9e8b-39e9b55282c5"), "Altenwerth, Hills and Graham", "Jacques", "Tillman", "expedite user-centric vortals" },
                    { new Guid("4776ab35-f030-ec60-c2b1-7643d97ba67a"), "Ziemann - Tremblay", "Daija", "Bruen", "transform cross-media e-commerce" },
                    { new Guid("792cac6a-12a7-7590-7dca-0f47ddd43bbc"), "O'Kon - Bernier", "Chester", "Luettgen", "architect user-centric bandwidth" }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Employees");
        }
    }
}
