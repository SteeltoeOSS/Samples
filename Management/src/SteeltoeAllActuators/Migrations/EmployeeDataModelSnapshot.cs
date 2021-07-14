﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using SteeltoeAllActuators.Providers;

namespace SteeltoeAllActuators.Migrations
{
    [DbContext(typeof(EmployeeData))]
    partial class EmployeeDataModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("ProductVersion", "5.0.8")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("SteeltoeAllActuators.Models.Employee", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Company")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("FirstName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("LastName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Title")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Employees");

                    b.HasData(
                        new
                        {
                            Id = new Guid("5e2776de-9a6e-7717-9a03-9983a3c6fdfa"),
                            Company = "Kuhic Inc",
                            FirstName = "Ally",
                            LastName = "Becker",
                            Title = "seize ubiquitous content"
                        },
                        new
                        {
                            Id = new Guid("d5f7be0c-dc49-4b1e-77f3-60c17f2c37f9"),
                            Company = "Gerhold - Wisozk",
                            FirstName = "Jake",
                            LastName = "King",
                            Title = "expedite cutting-edge experiences"
                        },
                        new
                        {
                            Id = new Guid("45888b7e-1cea-30d7-8f42-52b0a3422ebe"),
                            Company = "Cormier LLC",
                            FirstName = "Cordia",
                            LastName = "Leuschke",
                            Title = "recontextualize out-of-the-box niches"
                        },
                        new
                        {
                            Id = new Guid("8d7f0d95-dab3-435f-a909-56bd53d8615a"),
                            Company = "O'Keefe, Towne and Kling",
                            FirstName = "Alexanne",
                            LastName = "DuBuque",
                            Title = "transition dynamic ROI"
                        },
                        new
                        {
                            Id = new Guid("bc38273b-8864-1142-d1af-dd9d497cb5e2"),
                            Company = "Conroy, Jones and Schimmel",
                            FirstName = "Ewell",
                            LastName = "Kunze",
                            Title = "harness vertical technologies"
                        },
                        new
                        {
                            Id = new Guid("3d0a3594-0ddc-beb6-2b78-b32786458114"),
                            Company = "Hessel, Brekke and Morissette",
                            FirstName = "Lorenzo",
                            LastName = "Russel",
                            Title = "recontextualize open-source applications"
                        },
                        new
                        {
                            Id = new Guid("d5bd80ca-d1f1-4c1a-0bd0-5238d4f5a226"),
                            Company = "Hyatt Group",
                            FirstName = "Virginie",
                            LastName = "Williamson",
                            Title = "deploy 24/365 supply-chains"
                        },
                        new
                        {
                            Id = new Guid("e3e9c3d6-f467-81c1-8de2-118cf9e939a6"),
                            Company = "Kutch, Langworth and Harber",
                            FirstName = "Sierra",
                            LastName = "Murray",
                            Title = "engage viral metrics"
                        },
                        new
                        {
                            Id = new Guid("afb4de5d-9f7b-bf42-a4fe-2ee837e4160f"),
                            Company = "King, Donnelly and Okuneva",
                            FirstName = "Carroll",
                            LastName = "Langworth",
                            Title = "deliver frictionless ROI"
                        },
                        new
                        {
                            Id = new Guid("4c0ce168-fae9-5ceb-5c0a-160cd11243cf"),
                            Company = "Mertz, Schuppe and Maggio",
                            FirstName = "Ephraim",
                            LastName = "Keebler",
                            Title = "iterate holistic technologies"
                        },
                        new
                        {
                            Id = new Guid("a8e6315d-2f71-d0e8-5212-bd3572ba0442"),
                            Company = "Olson - Funk",
                            FirstName = "Iliana",
                            LastName = "Schneider",
                            Title = "deploy intuitive relationships"
                        },
                        new
                        {
                            Id = new Guid("0ad94713-9ed8-eb06-a7e7-8cc804b3eada"),
                            Company = "Miller and Sons",
                            FirstName = "Kaya",
                            LastName = "Langworth",
                            Title = "synergize open-source infomediaries"
                        },
                        new
                        {
                            Id = new Guid("8633c8e6-641b-5d63-9938-952956ddf7b4"),
                            Company = "Cummerata, Corkery and Reilly",
                            FirstName = "Retta",
                            LastName = "Moen",
                            Title = "brand cross-platform web services"
                        },
                        new
                        {
                            Id = new Guid("80c5d300-2998-ed79-3f24-0f6b80cf4384"),
                            Company = "Jones Inc",
                            FirstName = "Meghan",
                            LastName = "Spinka",
                            Title = "mesh world-class functionalities"
                        },
                        new
                        {
                            Id = new Guid("0cb2f3cb-6bcf-8aaa-5b96-1a919ddd010f"),
                            Company = "Sipes LLC",
                            FirstName = "Carlos",
                            LastName = "McCullough",
                            Title = "optimize turn-key systems"
                        },
                        new
                        {
                            Id = new Guid("0cff064a-b972-7c2e-8380-b5a4766c1dd4"),
                            Company = "Hintz - Beer",
                            FirstName = "Christa",
                            LastName = "Padberg",
                            Title = "enable plug-and-play partnerships"
                        },
                        new
                        {
                            Id = new Guid("8402bc2d-956c-815d-68b0-c2c86c9e17a2"),
                            Company = "Mayert, Harber and Herman",
                            FirstName = "Theresa",
                            LastName = "Block",
                            Title = "scale extensible e-business"
                        },
                        new
                        {
                            Id = new Guid("39614043-4da2-1f98-aedf-b5b7aa10ed23"),
                            Company = "Hauck Inc",
                            FirstName = "Eve",
                            LastName = "Nikolaus",
                            Title = "productize e-business mindshare"
                        },
                        new
                        {
                            Id = new Guid("a416816c-28ca-24d1-5265-2955fb48c562"),
                            Company = "Stanton, Maggio and Jast",
                            FirstName = "Sunny",
                            LastName = "Schaefer",
                            Title = "syndicate cross-media partnerships"
                        },
                        new
                        {
                            Id = new Guid("d21ff4f9-c905-65db-f4cc-2036f355850d"),
                            Company = "Yundt, Kshlerin and Emard",
                            FirstName = "Camron",
                            LastName = "Hessel",
                            Title = "transform integrated models"
                        },
                        new
                        {
                            Id = new Guid("1caaac80-810b-89c3-84d1-33b1514d9b4e"),
                            Company = "Bechtelar - West",
                            FirstName = "Marquis",
                            LastName = "Brown",
                            Title = "recontextualize end-to-end initiatives"
                        },
                        new
                        {
                            Id = new Guid("7887d1b1-87a6-09d0-a86e-841d3049e94b"),
                            Company = "Durgan - Schiller",
                            FirstName = "Serena",
                            LastName = "Harber",
                            Title = "enable best-of-breed blockchains"
                        },
                        new
                        {
                            Id = new Guid("21a898a9-d16f-4a37-5872-6b39fa4be2fc"),
                            Company = "Wilderman, Larkin and Dicki",
                            FirstName = "Joanny",
                            LastName = "Upton",
                            Title = "facilitate back-end paradigms"
                        },
                        new
                        {
                            Id = new Guid("c67e4830-e6cf-9154-70cf-0ddeaf9ad737"),
                            Company = "Torp, Gutkowski and Purdy",
                            FirstName = "Gust",
                            LastName = "McLaughlin",
                            Title = "expedite enterprise blockchains"
                        },
                        new
                        {
                            Id = new Guid("5c532783-2a4e-027b-033f-89b984898b16"),
                            Company = "Maggio - Schuster",
                            FirstName = "Nicola",
                            LastName = "Kilback",
                            Title = "extend open-source convergence"
                        },
                        new
                        {
                            Id = new Guid("5eb98eab-6ec8-36df-b353-4bbe013521a4"),
                            Company = "Stroman, Osinski and Torp",
                            FirstName = "Lonie",
                            LastName = "Mante",
                            Title = "envisioneer turn-key eyeballs"
                        },
                        new
                        {
                            Id = new Guid("96c607c7-55f6-9832-21b0-c0711c39945f"),
                            Company = "Mills, Cruickshank and Stroman",
                            FirstName = "Alana",
                            LastName = "Greenholt",
                            Title = "architect user-centric models"
                        },
                        new
                        {
                            Id = new Guid("34a44141-2ee3-7ed7-2b01-6719a0bdfe18"),
                            Company = "Doyle LLC",
                            FirstName = "Earnestine",
                            LastName = "Maggio",
                            Title = "scale collaborative niches"
                        },
                        new
                        {
                            Id = new Guid("8578fafb-66fc-95b2-5dbf-b4138e633e82"),
                            Company = "Kuphal - Block",
                            FirstName = "Kirstin",
                            LastName = "Gorczany",
                            Title = "target seamless bandwidth"
                        },
                        new
                        {
                            Id = new Guid("35a62632-07ed-cca3-9e8b-39e9b55282c5"),
                            Company = "Altenwerth, Hills and Graham",
                            FirstName = "Jacques",
                            LastName = "Tillman",
                            Title = "expedite user-centric vortals"
                        },
                        new
                        {
                            Id = new Guid("84417802-eb6a-48b4-3053-d5bf7da8551c"),
                            Company = "Ortiz Group",
                            FirstName = "Astrid",
                            LastName = "Monahan",
                            Title = "integrate dynamic portals"
                        },
                        new
                        {
                            Id = new Guid("8a5bea2c-3ed1-9c48-e493-ea8f614833a0"),
                            Company = "Conn - Bauch",
                            FirstName = "Casper",
                            LastName = "Christiansen",
                            Title = "utilize scalable users"
                        },
                        new
                        {
                            Id = new Guid("7421c466-9986-354c-e2d1-5780902976f2"),
                            Company = "Larkin and Sons",
                            FirstName = "Reggie",
                            LastName = "Hauck",
                            Title = "whiteboard front-end web-readiness"
                        },
                        new
                        {
                            Id = new Guid("b3a46241-9748-26c2-1831-240c1dd3154a"),
                            Company = "Runte, Heidenreich and Kessler",
                            FirstName = "Eliezer",
                            LastName = "Gerlach",
                            Title = "harness strategic infomediaries"
                        },
                        new
                        {
                            Id = new Guid("2a73f491-6ce7-0a9f-8132-23f498ec4b0f"),
                            Company = "Morissette, Lockman and O'Keefe",
                            FirstName = "Erin",
                            LastName = "Kozey",
                            Title = "incentivize 24/365 portals"
                        },
                        new
                        {
                            Id = new Guid("2a1908a2-8eaf-4952-89b6-a24efb31c584"),
                            Company = "Witting - Boyle",
                            FirstName = "Rahsaan",
                            LastName = "Rosenbaum",
                            Title = "deliver virtual users"
                        },
                        new
                        {
                            Id = new Guid("b14bbf80-c155-bdbc-509a-456485dfd1a4"),
                            Company = "Ernser - Botsford",
                            FirstName = "Melissa",
                            LastName = "Hickle",
                            Title = "integrate wireless infrastructures"
                        },
                        new
                        {
                            Id = new Guid("39668099-3196-f33a-3fb8-aaf042a0f3c7"),
                            Company = "Bailey Group",
                            FirstName = "Jessica",
                            LastName = "Steuber",
                            Title = "engineer 24/7 relationships"
                        },
                        new
                        {
                            Id = new Guid("aa20fcca-929e-bfb9-8915-18d45a82261e"),
                            Company = "Langworth - McDermott",
                            FirstName = "Danial",
                            LastName = "Prosacco",
                            Title = "iterate end-to-end partnerships"
                        },
                        new
                        {
                            Id = new Guid("c23b47b7-d4c2-384a-8143-6f9a118f54ad"),
                            Company = "Kuvalis - McClure",
                            FirstName = "Hilton",
                            LastName = "Murazik",
                            Title = "redefine seamless eyeballs"
                        },
                        new
                        {
                            Id = new Guid("9a00dfbf-313c-e9ee-99ef-f35756f7fcb4"),
                            Company = "D'Amore LLC",
                            FirstName = "Micaela",
                            LastName = "Effertz",
                            Title = "iterate out-of-the-box models"
                        },
                        new
                        {
                            Id = new Guid("48cf76fc-3579-98a8-dac3-02e3d3affc4f"),
                            Company = "Yundt, Howe and Konopelski",
                            FirstName = "Dulce",
                            LastName = "Johns",
                            Title = "incentivize rich e-tailers"
                        },
                        new
                        {
                            Id = new Guid("cba960b3-6556-d133-309d-c562a56f6446"),
                            Company = "Mitchell, Yost and Kessler",
                            FirstName = "Karson",
                            LastName = "Friesen",
                            Title = "exploit enterprise convergence"
                        },
                        new
                        {
                            Id = new Guid("eb8b8cc2-ff34-c81f-59c6-d4ffcc835dcf"),
                            Company = "Stamm - Cormier",
                            FirstName = "Matilda",
                            LastName = "Thompson",
                            Title = "envisioneer front-end models"
                        },
                        new
                        {
                            Id = new Guid("c48041db-d612-44d2-c6c8-93d9a8e5fb9d"),
                            Company = "Jast Inc",
                            FirstName = "Audreanne",
                            LastName = "Little",
                            Title = "target turn-key bandwidth"
                        },
                        new
                        {
                            Id = new Guid("3c072534-ab09-1416-75ee-10f27f5bb583"),
                            Company = "Rath - Berge",
                            FirstName = "Ronaldo",
                            LastName = "Bartell",
                            Title = "target clicks-and-mortar schemas"
                        },
                        new
                        {
                            Id = new Guid("529074a8-6d61-930d-b486-8fedcf963b1a"),
                            Company = "Torphy, Kris and Hayes",
                            FirstName = "Rossie",
                            LastName = "Welch",
                            Title = "incubate holistic applications"
                        },
                        new
                        {
                            Id = new Guid("0effb5c8-0988-feb0-db94-941bc3b208e0"),
                            Company = "Prosacco, McDermott and Heaney",
                            FirstName = "Kieran",
                            LastName = "Gislason",
                            Title = "engineer magnetic metrics"
                        },
                        new
                        {
                            Id = new Guid("6d207152-9e56-4556-6d99-b70e842b7cde"),
                            Company = "Wiegand - Larson",
                            FirstName = "Silas",
                            LastName = "Leannon",
                            Title = "transform frictionless niches"
                        },
                        new
                        {
                            Id = new Guid("25cb58d9-1c7f-5714-aaf1-36988f9185f5"),
                            Company = "Mayert, Smith and Hackett",
                            FirstName = "Judah",
                            LastName = "Homenick",
                            Title = "visualize rich models"
                        },
                        new
                        {
                            Id = new Guid("5cc0d9ec-1ead-a464-c594-fdd330c65184"),
                            Company = "Effertz, Ledner and Carroll",
                            FirstName = "Rosalinda",
                            LastName = "Veum",
                            Title = "drive virtual networks"
                        },
                        new
                        {
                            Id = new Guid("b2982a68-060a-b861-aa92-4253fbfdd2d1"),
                            Company = "Runolfsson Inc",
                            FirstName = "Marjory",
                            LastName = "Lueilwitz",
                            Title = "morph user-centric content"
                        },
                        new
                        {
                            Id = new Guid("636ef095-971d-7b9d-5250-45f4ccf2ba8d"),
                            Company = "Armstrong Group",
                            FirstName = "Clement",
                            LastName = "Marquardt",
                            Title = "benchmark viral users"
                        },
                        new
                        {
                            Id = new Guid("84f023c1-e660-5ae9-1b93-1d1998fd9a12"),
                            Company = "Auer Inc",
                            FirstName = "Rozella",
                            LastName = "Nienow",
                            Title = "deliver cross-media bandwidth"
                        },
                        new
                        {
                            Id = new Guid("06eb6f0f-da88-64f5-8c07-a2f9a6191b6b"),
                            Company = "Volkman, Kerluke and Cremin",
                            FirstName = "Nayeli",
                            LastName = "Turner",
                            Title = "implement user-centric ROI"
                        },
                        new
                        {
                            Id = new Guid("2f483f73-aeb9-1fe2-ef74-f1fbe3d1409e"),
                            Company = "Kuhn, Kihn and Mueller",
                            FirstName = "Alize",
                            LastName = "Ward",
                            Title = "drive web-enabled niches"
                        },
                        new
                        {
                            Id = new Guid("cb05cd44-6e2d-4c7a-8515-9bad35d358c0"),
                            Company = "Bogan - Upton",
                            FirstName = "Malika",
                            LastName = "Dach",
                            Title = "deliver plug-and-play relationships"
                        },
                        new
                        {
                            Id = new Guid("64141b9a-e044-6306-9c99-75e1dc53c106"),
                            Company = "Von, Robel and Cassin",
                            FirstName = "Heather",
                            LastName = "Greenholt",
                            Title = "extend turn-key users"
                        },
                        new
                        {
                            Id = new Guid("37b5c59a-2481-b4c0-6d31-115a1608a40e"),
                            Company = "Fadel and Sons",
                            FirstName = "Einar",
                            LastName = "Franecki",
                            Title = "disintermediate 24/7 eyeballs"
                        },
                        new
                        {
                            Id = new Guid("25c2b39e-f35c-97a9-5a2c-aa1b512b1b75"),
                            Company = "Hamill, Cartwright and McCullough",
                            FirstName = "Esteban",
                            LastName = "Gaylord",
                            Title = "streamline compelling relationships"
                        },
                        new
                        {
                            Id = new Guid("e8e06cae-c05b-3eb6-c099-cd619b587987"),
                            Company = "Champlin, Jacobs and Langworth",
                            FirstName = "Davon",
                            LastName = "Strosin",
                            Title = "envisioneer enterprise experiences"
                        },
                        new
                        {
                            Id = new Guid("64734aa9-f245-1c53-6cad-87c8afff6cf3"),
                            Company = "Rosenbaum, Littel and Dach",
                            FirstName = "Jermain",
                            LastName = "Schroeder",
                            Title = "utilize sexy users"
                        },
                        new
                        {
                            Id = new Guid("4776ab35-f030-ec60-c2b1-7643d97ba67a"),
                            Company = "Ziemann - Tremblay",
                            FirstName = "Daija",
                            LastName = "Bruen",
                            Title = "transform cross-media e-commerce"
                        },
                        new
                        {
                            Id = new Guid("792cac6a-12a7-7590-7dca-0f47ddd43bbc"),
                            Company = "O'Kon - Bernier",
                            FirstName = "Chester",
                            LastName = "Luettgen",
                            Title = "architect user-centric bandwidth"
                        });
                });
#pragma warning restore 612, 618
        }
    }
}
