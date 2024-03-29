﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Steeltoe.Actuators.Providers;

namespace Steeltoe.Actuators.Migrations
{
    [DbContext(typeof(EmployeeDataContext))]
    [Migration("20210716155831_InitialModelCreate")]
    partial class InitialModelCreate
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "5.0.8");

            modelBuilder.Entity("Steeltoe.Actuators.Models.Employee", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<string>("Company")
                        .HasColumnType("TEXT");

                    b.Property<string>("FirstName")
                        .HasColumnType("TEXT");

                    b.Property<string>("LastName")
                        .HasColumnType("TEXT");

                    b.Property<string>("Title")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("Employees");

                    b.HasData(
                        new
                        {
                            Id = new Guid("63752153-7d26-6603-705f-760509299686"),
                            Company = "Leannon, Deckow and Kirlin",
                            FirstName = "Delbert",
                            LastName = "Kilback",
                            Title = "productize viral web-readiness"
                        },
                        new
                        {
                            Id = new Guid("0d11431b-b926-fc46-abc2-0c7f2b9ba935"),
                            Company = "Collier - McLaughlin",
                            FirstName = "Thomas",
                            LastName = "Hoeger",
                            Title = "engage e-business technologies"
                        },
                        new
                        {
                            Id = new Guid("2be45228-9081-eb95-691e-0b3af1594aa5"),
                            Company = "Mante, Olson and Kreiger",
                            FirstName = "Imani",
                            LastName = "Schoen",
                            Title = "matrix viral ROI"
                        },
                        new
                        {
                            Id = new Guid("f15cd3bf-5e5f-8051-1be4-d3d51a7b1a2d"),
                            Company = "Weimann LLC",
                            FirstName = "Delphia",
                            LastName = "Sipes",
                            Title = "drive strategic bandwidth"
                        },
                        new
                        {
                            Id = new Guid("6add7d9f-6c5b-7e21-c4c4-c1bd38fa09bf"),
                            Company = "Franecki, Carter and Jerde",
                            FirstName = "Florine",
                            LastName = "Schneider",
                            Title = "unleash real-time channels"
                        },
                        new
                        {
                            Id = new Guid("c6f1683a-f790-9d7c-4452-e180e724aec3"),
                            Company = "Hermiston LLC",
                            FirstName = "Taya",
                            LastName = "Bartell",
                            Title = "reinvent customized metrics"
                        },
                        new
                        {
                            Id = new Guid("6db5d64d-a0c0-68ce-5026-21042fb733cd"),
                            Company = "Cormier - Kiehn",
                            FirstName = "Sonya",
                            LastName = "Wunsch",
                            Title = "orchestrate granular initiatives"
                        },
                        new
                        {
                            Id = new Guid("bf22b128-d4be-46e6-41e5-79b9410fdca3"),
                            Company = "Kub - Jast",
                            FirstName = "Jolie",
                            LastName = "Mertz",
                            Title = "embrace next-generation platforms"
                        },
                        new
                        {
                            Id = new Guid("3d7388dd-7eff-0ee3-0420-440f7e0faa7c"),
                            Company = "Rice, Bergnaum and Steuber",
                            FirstName = "Viola",
                            LastName = "Bashirian",
                            Title = "facilitate visionary schemas"
                        },
                        new
                        {
                            Id = new Guid("0eaf2450-9111-1e7e-85e2-3560a846c202"),
                            Company = "Hane, Daugherty and Lehner",
                            FirstName = "Lukas",
                            LastName = "Marquardt",
                            Title = "aggregate viral portals"
                        },
                        new
                        {
                            Id = new Guid("4ae72880-635f-3cfd-f9ae-5a33fff28ec6"),
                            Company = "Lemke and Sons",
                            FirstName = "Vanessa",
                            LastName = "Goyette",
                            Title = "redefine web-enabled paradigms"
                        },
                        new
                        {
                            Id = new Guid("6ac4d247-766c-b02b-e15a-916996d3257e"),
                            Company = "O'Connell LLC",
                            FirstName = "Armand",
                            LastName = "Corwin",
                            Title = "extend frictionless platforms"
                        },
                        new
                        {
                            Id = new Guid("2cdc0392-5946-e999-93e7-028c9f93d083"),
                            Company = "White Inc",
                            FirstName = "Mya",
                            LastName = "Pagac",
                            Title = "mesh distributed blockchains"
                        },
                        new
                        {
                            Id = new Guid("f73caed3-fce7-7897-6601-d8689afe6987"),
                            Company = "O'Hara - Ferry",
                            FirstName = "Euna",
                            LastName = "Runolfsdottir",
                            Title = "envisioneer viral applications"
                        },
                        new
                        {
                            Id = new Guid("be132b54-c77c-6c62-d4bf-58981da10197"),
                            Company = "Bednar - Weissnat",
                            FirstName = "Rosa",
                            LastName = "Crooks",
                            Title = "empower transparent networks"
                        },
                        new
                        {
                            Id = new Guid("b9e3ca40-7f50-3a4b-cf97-a3bbe8d8fb9e"),
                            Company = "Conroy - Yundt",
                            FirstName = "Bridgette",
                            LastName = "Schultz",
                            Title = "incentivize B2B supply-chains"
                        },
                        new
                        {
                            Id = new Guid("475f3ea6-4172-ece9-438f-7eb2e3a81c54"),
                            Company = "Rogahn, Schroeder and Waters",
                            FirstName = "Jeramy",
                            LastName = "Weimann",
                            Title = "orchestrate killer e-services"
                        },
                        new
                        {
                            Id = new Guid("d24ffe87-de0d-606e-c413-112004c78ead"),
                            Company = "Nicolas, Lockman and Wiegand",
                            FirstName = "Clementina",
                            LastName = "Pacocha",
                            Title = "target dynamic eyeballs"
                        },
                        new
                        {
                            Id = new Guid("e4088767-6020-e890-b8ea-429a1a35b00f"),
                            Company = "Schinner Inc",
                            FirstName = "Chauncey",
                            LastName = "Oberbrunner",
                            Title = "revolutionize open-source applications"
                        },
                        new
                        {
                            Id = new Guid("54cd48d8-441d-08cc-ff92-a657667ba3b3"),
                            Company = "Hoppe, Ortiz and Zemlak",
                            FirstName = "Gay",
                            LastName = "Rolfson",
                            Title = "embrace mission-critical relationships"
                        },
                        new
                        {
                            Id = new Guid("619bf749-0c44-2ff8-cd0c-d8923bca731d"),
                            Company = "Kilback - Leffler",
                            FirstName = "Ulises",
                            LastName = "Abshire",
                            Title = "empower distributed channels"
                        },
                        new
                        {
                            Id = new Guid("22fc89e2-ef7b-acc5-8aaa-d44d31a7bb86"),
                            Company = "Bashirian and Sons",
                            FirstName = "Clint",
                            LastName = "Huel",
                            Title = "deploy web-enabled platforms"
                        },
                        new
                        {
                            Id = new Guid("7711bed7-b314-a050-201f-7fbb0fd5b4de"),
                            Company = "Bode, Metz and McLaughlin",
                            FirstName = "Kailee",
                            LastName = "Marks",
                            Title = "seize cross-media applications"
                        },
                        new
                        {
                            Id = new Guid("1030e201-b067-43c1-8f4a-bcdd4d97d659"),
                            Company = "Brakus - Abernathy",
                            FirstName = "June",
                            LastName = "Murray",
                            Title = "engineer global e-tailers"
                        },
                        new
                        {
                            Id = new Guid("179f27ba-76b4-d2b1-7f98-13d12090b96e"),
                            Company = "Watsica - Douglas",
                            FirstName = "Keith",
                            LastName = "Durgan",
                            Title = "cultivate sexy mindshare"
                        },
                        new
                        {
                            Id = new Guid("f9c33732-6ad9-8adf-0abb-554010a98f18"),
                            Company = "Stroman - Ernser",
                            FirstName = "Magnolia",
                            LastName = "Stokes",
                            Title = "extend holistic e-services"
                        },
                        new
                        {
                            Id = new Guid("886aa473-4cf6-b97d-2c0c-e57db3b7f814"),
                            Company = "Bartoletti - Marks",
                            FirstName = "Emma",
                            LastName = "Kiehn",
                            Title = "orchestrate frictionless bandwidth"
                        },
                        new
                        {
                            Id = new Guid("0cf167e7-45a2-2d0f-fe1f-8635005793be"),
                            Company = "Strosin LLC",
                            FirstName = "Misael",
                            LastName = "Von",
                            Title = "deploy sticky e-tailers"
                        },
                        new
                        {
                            Id = new Guid("fafc49f2-d33e-fa68-4656-a18e5b7017e7"),
                            Company = "Robel, Shields and Jones",
                            FirstName = "Lesly",
                            LastName = "Lueilwitz",
                            Title = "seize B2B systems"
                        },
                        new
                        {
                            Id = new Guid("669e2607-4e58-02d5-0e19-584914ef2ab6"),
                            Company = "Swaniawski Inc",
                            FirstName = "Donny",
                            LastName = "Steuber",
                            Title = "mesh compelling mindshare"
                        },
                        new
                        {
                            Id = new Guid("930d082b-b1d1-3530-9b05-36adca94ee07"),
                            Company = "Connelly LLC",
                            FirstName = "Waino",
                            LastName = "Braun",
                            Title = "enhance customized platforms"
                        },
                        new
                        {
                            Id = new Guid("d3a4dc54-ab6b-a4f0-5819-3a504141315d"),
                            Company = "Macejkovic, Beatty and Pacocha",
                            FirstName = "Emery",
                            LastName = "Waelchi",
                            Title = "transition revolutionary vortals"
                        },
                        new
                        {
                            Id = new Guid("1779e9a1-c676-208d-443e-f40c5ccaef14"),
                            Company = "Smitham Inc",
                            FirstName = "Adell",
                            LastName = "Greenfelder",
                            Title = "evolve frictionless paradigms"
                        },
                        new
                        {
                            Id = new Guid("f09b6dee-0fd8-efd7-acd3-c9c1778ce153"),
                            Company = "O'Connell, Hayes and Moen",
                            FirstName = "Rosella",
                            LastName = "Spinka",
                            Title = "incubate back-end methodologies"
                        },
                        new
                        {
                            Id = new Guid("3c2727fa-629e-5fdb-1e28-082a9d483567"),
                            Company = "Johnson - Hegmann",
                            FirstName = "Rhianna",
                            LastName = "Kiehn",
                            Title = "facilitate innovative users"
                        },
                        new
                        {
                            Id = new Guid("c96eac5c-1e87-e19d-4332-dde8b0b30e70"),
                            Company = "Wolff LLC",
                            FirstName = "Riley",
                            LastName = "Weimann",
                            Title = "maximize out-of-the-box applications"
                        },
                        new
                        {
                            Id = new Guid("f0379bc6-5802-5d8f-21b3-e0f1f311d7ff"),
                            Company = "Osinski LLC",
                            FirstName = "Tina",
                            LastName = "Rolfson",
                            Title = "revolutionize front-end bandwidth"
                        },
                        new
                        {
                            Id = new Guid("cc111852-a144-329b-9d86-02ece7addeeb"),
                            Company = "Trantow LLC",
                            FirstName = "Pedro",
                            LastName = "Bailey",
                            Title = "productize plug-and-play web services"
                        },
                        new
                        {
                            Id = new Guid("b0d95a8a-a542-797d-e4e5-9dea46c31980"),
                            Company = "Hodkiewicz LLC",
                            FirstName = "German",
                            LastName = "Ruecker",
                            Title = "unleash leading-edge infrastructures"
                        },
                        new
                        {
                            Id = new Guid("eac7fb49-b987-357e-9b34-42eac8eaf0f8"),
                            Company = "Hayes Group",
                            FirstName = "Dana",
                            LastName = "Dibbert",
                            Title = "embrace front-end interfaces"
                        },
                        new
                        {
                            Id = new Guid("24e44566-0fc8-9ecd-41fb-6746dd92f12b"),
                            Company = "Towne Group",
                            FirstName = "Vergie",
                            LastName = "Powlowski",
                            Title = "disintermediate turn-key experiences"
                        },
                        new
                        {
                            Id = new Guid("86645bbd-16e4-55cd-57e8-8342d5596c8c"),
                            Company = "Dickens Inc",
                            FirstName = "Dorris",
                            LastName = "Beatty",
                            Title = "incubate compelling technologies"
                        },
                        new
                        {
                            Id = new Guid("a79adb97-92a5-375e-8cb6-676dab46b92b"),
                            Company = "Toy Group",
                            FirstName = "Mabel",
                            LastName = "Yundt",
                            Title = "leverage scalable web-readiness"
                        },
                        new
                        {
                            Id = new Guid("dcaac2fc-ec87-14aa-40ed-ee087c26afc4"),
                            Company = "Mohr - Mann",
                            FirstName = "Lexie",
                            LastName = "Quigley",
                            Title = "aggregate visionary e-tailers"
                        },
                        new
                        {
                            Id = new Guid("719d7dea-26ba-e867-858d-aae3e881afa0"),
                            Company = "Olson and Sons",
                            FirstName = "Orlando",
                            LastName = "Hahn",
                            Title = "redefine synergistic architectures"
                        },
                        new
                        {
                            Id = new Guid("c26b2a62-37f9-fd1a-e18a-f20a4e39591a"),
                            Company = "Baumbach, Hegmann and Rowe",
                            FirstName = "Deanna",
                            LastName = "Bins",
                            Title = "benchmark out-of-the-box users"
                        },
                        new
                        {
                            Id = new Guid("d29f1215-c723-ce39-950e-8fbaa5ee4e72"),
                            Company = "Howe, Rogahn and Keebler",
                            FirstName = "Kip",
                            LastName = "Mraz",
                            Title = "embrace world-class niches"
                        },
                        new
                        {
                            Id = new Guid("bf2e5237-968d-634d-25e4-247b2abdc2b9"),
                            Company = "Roob, Bailey and West",
                            FirstName = "Salvatore",
                            LastName = "Turner",
                            Title = "leverage open-source initiatives"
                        },
                        new
                        {
                            Id = new Guid("787ec9cd-c3f7-a540-c1eb-750b0e077742"),
                            Company = "Moore - Padberg",
                            FirstName = "Donald",
                            LastName = "Stehr",
                            Title = "seize cross-media content"
                        },
                        new
                        {
                            Id = new Guid("a79769d6-b12f-3b56-f943-bfcdcfeb2e20"),
                            Company = "Casper - Medhurst",
                            FirstName = "Vinnie",
                            LastName = "Monahan",
                            Title = "extend mission-critical partnerships"
                        },
                        new
                        {
                            Id = new Guid("610887b2-0b0c-4a84-84e3-a28d2c997510"),
                            Company = "Cruickshank, Hudson and Keebler",
                            FirstName = "Ernesto",
                            LastName = "O'Reilly",
                            Title = "implement leading-edge content"
                        },
                        new
                        {
                            Id = new Guid("1a2245e9-4e72-3c28-e5f4-a8a8f52eeebf"),
                            Company = "Berge - Gusikowski",
                            FirstName = "Aleen",
                            LastName = "Buckridge",
                            Title = "maximize killer functionalities"
                        },
                        new
                        {
                            Id = new Guid("9b113c58-6962-1408-9c0f-23c5622f1194"),
                            Company = "Hermann, Lehner and Murazik",
                            FirstName = "Lexi",
                            LastName = "Kiehn",
                            Title = "matrix killer infomediaries"
                        },
                        new
                        {
                            Id = new Guid("8761d30c-b023-fbf4-c2c2-353c8cda7beb"),
                            Company = "Willms, Trantow and Mohr",
                            FirstName = "Jakayla",
                            LastName = "Wintheiser",
                            Title = "synergize 24/7 web-readiness"
                        },
                        new
                        {
                            Id = new Guid("a5ad50cf-f092-059c-7855-96a78d795f2e"),
                            Company = "Botsford LLC",
                            FirstName = "Christina",
                            LastName = "Gibson",
                            Title = "productize integrated web-readiness"
                        },
                        new
                        {
                            Id = new Guid("ba5b3ab9-fa01-8daf-2a30-535f47562fd1"),
                            Company = "Runte - Howell",
                            FirstName = "Tracey",
                            LastName = "Johns",
                            Title = "synergize strategic portals"
                        },
                        new
                        {
                            Id = new Guid("bc5edc86-5bd8-e2ff-dbe2-eb5895e2047d"),
                            Company = "Weimann Group",
                            FirstName = "Foster",
                            LastName = "Hoppe",
                            Title = "morph end-to-end web-readiness"
                        },
                        new
                        {
                            Id = new Guid("20439b70-f1d8-75e5-33c9-34249cf3b5ce"),
                            Company = "Graham Group",
                            FirstName = "Vivienne",
                            LastName = "Brekke",
                            Title = "grow distributed e-services"
                        },
                        new
                        {
                            Id = new Guid("ddde9733-a9c7-856c-f5ea-4b3c17df0aea"),
                            Company = "Schaden - Lindgren",
                            FirstName = "Carlie",
                            LastName = "Bayer",
                            Title = "reinvent B2C initiatives"
                        },
                        new
                        {
                            Id = new Guid("b42bae52-827c-9909-5e8c-ec411208ae8a"),
                            Company = "Howell - Hane",
                            FirstName = "Tess",
                            LastName = "Dare",
                            Title = "optimize plug-and-play networks"
                        },
                        new
                        {
                            Id = new Guid("f4679a67-70b3-5407-b52b-8a197db70972"),
                            Company = "Crist, Hahn and Rolfson",
                            FirstName = "Isaiah",
                            LastName = "Sporer",
                            Title = "brand sexy blockchains"
                        },
                        new
                        {
                            Id = new Guid("af006e99-0b2f-f7d9-da4d-e11b14e5ee85"),
                            Company = "Goodwin - Berge",
                            FirstName = "Manuel",
                            LastName = "Kovacek",
                            Title = "brand magnetic portals"
                        },
                        new
                        {
                            Id = new Guid("9bf4cfbd-5f2b-c29e-722c-190107a4851f"),
                            Company = "O'Kon Group",
                            FirstName = "Rebecca",
                            LastName = "Price",
                            Title = "deliver holistic functionalities"
                        },
                        new
                        {
                            Id = new Guid("3d5906ef-76f0-8c32-c95b-f25f173ecb7a"),
                            Company = "Prosacco LLC",
                            FirstName = "Judd",
                            LastName = "Halvorson",
                            Title = "orchestrate extensible initiatives"
                        },
                        new
                        {
                            Id = new Guid("73cdc705-3b6a-667c-0227-ab250561d2fc"),
                            Company = "Kertzmann - Tremblay",
                            FirstName = "Ruthie",
                            LastName = "Buckridge",
                            Title = "cultivate clicks-and-mortar e-markets"
                        },
                        new
                        {
                            Id = new Guid("08ba2856-e5ad-0300-8865-3728a8bf218d"),
                            Company = "Schamberger LLC",
                            FirstName = "Hubert",
                            LastName = "Bashirian",
                            Title = "exploit next-generation convergence"
                        },
                        new
                        {
                            Id = new Guid("3fedac4c-d2b3-df42-bf5a-1d418174b91d"),
                            Company = "Hodkiewicz and Sons",
                            FirstName = "Donnell",
                            LastName = "Feil",
                            Title = "brand seamless e-tailers"
                        },
                        new
                        {
                            Id = new Guid("25ccaa6b-18c8-b0e0-b36b-bb00f7c04fb6"),
                            Company = "Walker and Sons",
                            FirstName = "Christopher",
                            LastName = "Boyle",
                            Title = "e-enable clicks-and-mortar niches"
                        },
                        new
                        {
                            Id = new Guid("eac83fd2-d408-a225-59b3-d408061c16a7"),
                            Company = "Ankunding - Goyette",
                            FirstName = "Oral",
                            LastName = "Streich",
                            Title = "drive B2B content"
                        },
                        new
                        {
                            Id = new Guid("d17de658-0542-8b97-9e60-53894ff5a7df"),
                            Company = "Tillman LLC",
                            FirstName = "Hoyt",
                            LastName = "Koepp",
                            Title = "integrate leading-edge users"
                        },
                        new
                        {
                            Id = new Guid("35d3c041-f551-1ab3-1962-912157535f53"),
                            Company = "McDermott, Hettinger and Borer",
                            FirstName = "Lauryn",
                            LastName = "Swaniawski",
                            Title = "streamline holistic eyeballs"
                        },
                        new
                        {
                            Id = new Guid("119ecfaf-79e2-3611-768f-81326239a8ad"),
                            Company = "Wehner - Treutel",
                            FirstName = "Alexis",
                            LastName = "McDermott",
                            Title = "generate magnetic convergence"
                        },
                        new
                        {
                            Id = new Guid("dda32853-3d0a-68b4-4e70-a226140836e8"),
                            Company = "Denesik, Witting and Bashirian",
                            FirstName = "Jayne",
                            LastName = "Wolff",
                            Title = "orchestrate rich markets"
                        },
                        new
                        {
                            Id = new Guid("6a1c90f8-3f21-dd27-e5bd-bad6342c39db"),
                            Company = "Quitzon and Sons",
                            FirstName = "Matteo",
                            LastName = "Hermiston",
                            Title = "leverage real-time web services"
                        },
                        new
                        {
                            Id = new Guid("a79ae305-db2e-6f32-941c-12ad97fd2d30"),
                            Company = "Moore, Lebsack and Durgan",
                            FirstName = "Melany",
                            LastName = "Grady",
                            Title = "aggregate synergistic networks"
                        },
                        new
                        {
                            Id = new Guid("c63cc4e8-3e77-5f5e-e265-3da480ae098b"),
                            Company = "Spencer Group",
                            FirstName = "Wendy",
                            LastName = "Hamill",
                            Title = "repurpose cutting-edge communities"
                        });
                });
#pragma warning restore 612, 618
        }
    }
}
