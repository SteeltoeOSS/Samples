using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Steeltoe.Actuators.Migrations
{
    public partial class InitialModelCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Employees",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    FirstName = table.Column<string>(type: "TEXT", nullable: true),
                    LastName = table.Column<string>(type: "TEXT", nullable: true),
                    Company = table.Column<string>(type: "TEXT", nullable: true),
                    Title = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Employees", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "Employees",
                columns: new[] { "Id", "Company", "FirstName", "LastName", "Title" },
                values: new object[] { new Guid("63752153-7d26-6603-705f-760509299686"), "Leannon, Deckow and Kirlin", "Delbert", "Kilback", "productize viral web-readiness" });

            migrationBuilder.InsertData(
                table: "Employees",
                columns: new[] { "Id", "Company", "FirstName", "LastName", "Title" },
                values: new object[] { new Guid("a5ad50cf-f092-059c-7855-96a78d795f2e"), "Botsford LLC", "Christina", "Gibson", "productize integrated web-readiness" });

            migrationBuilder.InsertData(
                table: "Employees",
                columns: new[] { "Id", "Company", "FirstName", "LastName", "Title" },
                values: new object[] { new Guid("8761d30c-b023-fbf4-c2c2-353c8cda7beb"), "Willms, Trantow and Mohr", "Jakayla", "Wintheiser", "synergize 24/7 web-readiness" });

            migrationBuilder.InsertData(
                table: "Employees",
                columns: new[] { "Id", "Company", "FirstName", "LastName", "Title" },
                values: new object[] { new Guid("9b113c58-6962-1408-9c0f-23c5622f1194"), "Hermann, Lehner and Murazik", "Lexi", "Kiehn", "matrix killer infomediaries" });

            migrationBuilder.InsertData(
                table: "Employees",
                columns: new[] { "Id", "Company", "FirstName", "LastName", "Title" },
                values: new object[] { new Guid("1a2245e9-4e72-3c28-e5f4-a8a8f52eeebf"), "Berge - Gusikowski", "Aleen", "Buckridge", "maximize killer functionalities" });

            migrationBuilder.InsertData(
                table: "Employees",
                columns: new[] { "Id", "Company", "FirstName", "LastName", "Title" },
                values: new object[] { new Guid("610887b2-0b0c-4a84-84e3-a28d2c997510"), "Cruickshank, Hudson and Keebler", "Ernesto", "O'Reilly", "implement leading-edge content" });

            migrationBuilder.InsertData(
                table: "Employees",
                columns: new[] { "Id", "Company", "FirstName", "LastName", "Title" },
                values: new object[] { new Guid("a79769d6-b12f-3b56-f943-bfcdcfeb2e20"), "Casper - Medhurst", "Vinnie", "Monahan", "extend mission-critical partnerships" });

            migrationBuilder.InsertData(
                table: "Employees",
                columns: new[] { "Id", "Company", "FirstName", "LastName", "Title" },
                values: new object[] { new Guid("787ec9cd-c3f7-a540-c1eb-750b0e077742"), "Moore - Padberg", "Donald", "Stehr", "seize cross-media content" });

            migrationBuilder.InsertData(
                table: "Employees",
                columns: new[] { "Id", "Company", "FirstName", "LastName", "Title" },
                values: new object[] { new Guid("bf2e5237-968d-634d-25e4-247b2abdc2b9"), "Roob, Bailey and West", "Salvatore", "Turner", "leverage open-source initiatives" });

            migrationBuilder.InsertData(
                table: "Employees",
                columns: new[] { "Id", "Company", "FirstName", "LastName", "Title" },
                values: new object[] { new Guid("d29f1215-c723-ce39-950e-8fbaa5ee4e72"), "Howe, Rogahn and Keebler", "Kip", "Mraz", "embrace world-class niches" });

            migrationBuilder.InsertData(
                table: "Employees",
                columns: new[] { "Id", "Company", "FirstName", "LastName", "Title" },
                values: new object[] { new Guid("c26b2a62-37f9-fd1a-e18a-f20a4e39591a"), "Baumbach, Hegmann and Rowe", "Deanna", "Bins", "benchmark out-of-the-box users" });

            migrationBuilder.InsertData(
                table: "Employees",
                columns: new[] { "Id", "Company", "FirstName", "LastName", "Title" },
                values: new object[] { new Guid("719d7dea-26ba-e867-858d-aae3e881afa0"), "Olson and Sons", "Orlando", "Hahn", "redefine synergistic architectures" });

            migrationBuilder.InsertData(
                table: "Employees",
                columns: new[] { "Id", "Company", "FirstName", "LastName", "Title" },
                values: new object[] { new Guid("dcaac2fc-ec87-14aa-40ed-ee087c26afc4"), "Mohr - Mann", "Lexie", "Quigley", "aggregate visionary e-tailers" });

            migrationBuilder.InsertData(
                table: "Employees",
                columns: new[] { "Id", "Company", "FirstName", "LastName", "Title" },
                values: new object[] { new Guid("a79adb97-92a5-375e-8cb6-676dab46b92b"), "Toy Group", "Mabel", "Yundt", "leverage scalable web-readiness" });

            migrationBuilder.InsertData(
                table: "Employees",
                columns: new[] { "Id", "Company", "FirstName", "LastName", "Title" },
                values: new object[] { new Guid("86645bbd-16e4-55cd-57e8-8342d5596c8c"), "Dickens Inc", "Dorris", "Beatty", "incubate compelling technologies" });

            migrationBuilder.InsertData(
                table: "Employees",
                columns: new[] { "Id", "Company", "FirstName", "LastName", "Title" },
                values: new object[] { new Guid("24e44566-0fc8-9ecd-41fb-6746dd92f12b"), "Towne Group", "Vergie", "Powlowski", "disintermediate turn-key experiences" });

            migrationBuilder.InsertData(
                table: "Employees",
                columns: new[] { "Id", "Company", "FirstName", "LastName", "Title" },
                values: new object[] { new Guid("ba5b3ab9-fa01-8daf-2a30-535f47562fd1"), "Runte - Howell", "Tracey", "Johns", "synergize strategic portals" });

            migrationBuilder.InsertData(
                table: "Employees",
                columns: new[] { "Id", "Company", "FirstName", "LastName", "Title" },
                values: new object[] { new Guid("eac7fb49-b987-357e-9b34-42eac8eaf0f8"), "Hayes Group", "Dana", "Dibbert", "embrace front-end interfaces" });

            migrationBuilder.InsertData(
                table: "Employees",
                columns: new[] { "Id", "Company", "FirstName", "LastName", "Title" },
                values: new object[] { new Guid("bc5edc86-5bd8-e2ff-dbe2-eb5895e2047d"), "Weimann Group", "Foster", "Hoppe", "morph end-to-end web-readiness" });

            migrationBuilder.InsertData(
                table: "Employees",
                columns: new[] { "Id", "Company", "FirstName", "LastName", "Title" },
                values: new object[] { new Guid("ddde9733-a9c7-856c-f5ea-4b3c17df0aea"), "Schaden - Lindgren", "Carlie", "Bayer", "reinvent B2C initiatives" });

            migrationBuilder.InsertData(
                table: "Employees",
                columns: new[] { "Id", "Company", "FirstName", "LastName", "Title" },
                values: new object[] { new Guid("6a1c90f8-3f21-dd27-e5bd-bad6342c39db"), "Quitzon and Sons", "Matteo", "Hermiston", "leverage real-time web services" });

            migrationBuilder.InsertData(
                table: "Employees",
                columns: new[] { "Id", "Company", "FirstName", "LastName", "Title" },
                values: new object[] { new Guid("dda32853-3d0a-68b4-4e70-a226140836e8"), "Denesik, Witting and Bashirian", "Jayne", "Wolff", "orchestrate rich markets" });

            migrationBuilder.InsertData(
                table: "Employees",
                columns: new[] { "Id", "Company", "FirstName", "LastName", "Title" },
                values: new object[] { new Guid("119ecfaf-79e2-3611-768f-81326239a8ad"), "Wehner - Treutel", "Alexis", "McDermott", "generate magnetic convergence" });

            migrationBuilder.InsertData(
                table: "Employees",
                columns: new[] { "Id", "Company", "FirstName", "LastName", "Title" },
                values: new object[] { new Guid("35d3c041-f551-1ab3-1962-912157535f53"), "McDermott, Hettinger and Borer", "Lauryn", "Swaniawski", "streamline holistic eyeballs" });

            migrationBuilder.InsertData(
                table: "Employees",
                columns: new[] { "Id", "Company", "FirstName", "LastName", "Title" },
                values: new object[] { new Guid("d17de658-0542-8b97-9e60-53894ff5a7df"), "Tillman LLC", "Hoyt", "Koepp", "integrate leading-edge users" });

            migrationBuilder.InsertData(
                table: "Employees",
                columns: new[] { "Id", "Company", "FirstName", "LastName", "Title" },
                values: new object[] { new Guid("eac83fd2-d408-a225-59b3-d408061c16a7"), "Ankunding - Goyette", "Oral", "Streich", "drive B2B content" });

            migrationBuilder.InsertData(
                table: "Employees",
                columns: new[] { "Id", "Company", "FirstName", "LastName", "Title" },
                values: new object[] { new Guid("25ccaa6b-18c8-b0e0-b36b-bb00f7c04fb6"), "Walker and Sons", "Christopher", "Boyle", "e-enable clicks-and-mortar niches" });

            migrationBuilder.InsertData(
                table: "Employees",
                columns: new[] { "Id", "Company", "FirstName", "LastName", "Title" },
                values: new object[] { new Guid("3fedac4c-d2b3-df42-bf5a-1d418174b91d"), "Hodkiewicz and Sons", "Donnell", "Feil", "brand seamless e-tailers" });

            migrationBuilder.InsertData(
                table: "Employees",
                columns: new[] { "Id", "Company", "FirstName", "LastName", "Title" },
                values: new object[] { new Guid("08ba2856-e5ad-0300-8865-3728a8bf218d"), "Schamberger LLC", "Hubert", "Bashirian", "exploit next-generation convergence" });

            migrationBuilder.InsertData(
                table: "Employees",
                columns: new[] { "Id", "Company", "FirstName", "LastName", "Title" },
                values: new object[] { new Guid("73cdc705-3b6a-667c-0227-ab250561d2fc"), "Kertzmann - Tremblay", "Ruthie", "Buckridge", "cultivate clicks-and-mortar e-markets" });

            migrationBuilder.InsertData(
                table: "Employees",
                columns: new[] { "Id", "Company", "FirstName", "LastName", "Title" },
                values: new object[] { new Guid("3d5906ef-76f0-8c32-c95b-f25f173ecb7a"), "Prosacco LLC", "Judd", "Halvorson", "orchestrate extensible initiatives" });

            migrationBuilder.InsertData(
                table: "Employees",
                columns: new[] { "Id", "Company", "FirstName", "LastName", "Title" },
                values: new object[] { new Guid("9bf4cfbd-5f2b-c29e-722c-190107a4851f"), "O'Kon Group", "Rebecca", "Price", "deliver holistic functionalities" });

            migrationBuilder.InsertData(
                table: "Employees",
                columns: new[] { "Id", "Company", "FirstName", "LastName", "Title" },
                values: new object[] { new Guid("af006e99-0b2f-f7d9-da4d-e11b14e5ee85"), "Goodwin - Berge", "Manuel", "Kovacek", "brand magnetic portals" });

            migrationBuilder.InsertData(
                table: "Employees",
                columns: new[] { "Id", "Company", "FirstName", "LastName", "Title" },
                values: new object[] { new Guid("f4679a67-70b3-5407-b52b-8a197db70972"), "Crist, Hahn and Rolfson", "Isaiah", "Sporer", "brand sexy blockchains" });

            migrationBuilder.InsertData(
                table: "Employees",
                columns: new[] { "Id", "Company", "FirstName", "LastName", "Title" },
                values: new object[] { new Guid("b42bae52-827c-9909-5e8c-ec411208ae8a"), "Howell - Hane", "Tess", "Dare", "optimize plug-and-play networks" });

            migrationBuilder.InsertData(
                table: "Employees",
                columns: new[] { "Id", "Company", "FirstName", "LastName", "Title" },
                values: new object[] { new Guid("20439b70-f1d8-75e5-33c9-34249cf3b5ce"), "Graham Group", "Vivienne", "Brekke", "grow distributed e-services" });

            migrationBuilder.InsertData(
                table: "Employees",
                columns: new[] { "Id", "Company", "FirstName", "LastName", "Title" },
                values: new object[] { new Guid("b0d95a8a-a542-797d-e4e5-9dea46c31980"), "Hodkiewicz LLC", "German", "Ruecker", "unleash leading-edge infrastructures" });

            migrationBuilder.InsertData(
                table: "Employees",
                columns: new[] { "Id", "Company", "FirstName", "LastName", "Title" },
                values: new object[] { new Guid("cc111852-a144-329b-9d86-02ece7addeeb"), "Trantow LLC", "Pedro", "Bailey", "productize plug-and-play web services" });

            migrationBuilder.InsertData(
                table: "Employees",
                columns: new[] { "Id", "Company", "FirstName", "LastName", "Title" },
                values: new object[] { new Guid("f0379bc6-5802-5d8f-21b3-e0f1f311d7ff"), "Osinski LLC", "Tina", "Rolfson", "revolutionize front-end bandwidth" });

            migrationBuilder.InsertData(
                table: "Employees",
                columns: new[] { "Id", "Company", "FirstName", "LastName", "Title" },
                values: new object[] { new Guid("b9e3ca40-7f50-3a4b-cf97-a3bbe8d8fb9e"), "Conroy - Yundt", "Bridgette", "Schultz", "incentivize B2B supply-chains" });

            migrationBuilder.InsertData(
                table: "Employees",
                columns: new[] { "Id", "Company", "FirstName", "LastName", "Title" },
                values: new object[] { new Guid("be132b54-c77c-6c62-d4bf-58981da10197"), "Bednar - Weissnat", "Rosa", "Crooks", "empower transparent networks" });

            migrationBuilder.InsertData(
                table: "Employees",
                columns: new[] { "Id", "Company", "FirstName", "LastName", "Title" },
                values: new object[] { new Guid("f73caed3-fce7-7897-6601-d8689afe6987"), "O'Hara - Ferry", "Euna", "Runolfsdottir", "envisioneer viral applications" });

            migrationBuilder.InsertData(
                table: "Employees",
                columns: new[] { "Id", "Company", "FirstName", "LastName", "Title" },
                values: new object[] { new Guid("2cdc0392-5946-e999-93e7-028c9f93d083"), "White Inc", "Mya", "Pagac", "mesh distributed blockchains" });

            migrationBuilder.InsertData(
                table: "Employees",
                columns: new[] { "Id", "Company", "FirstName", "LastName", "Title" },
                values: new object[] { new Guid("6ac4d247-766c-b02b-e15a-916996d3257e"), "O'Connell LLC", "Armand", "Corwin", "extend frictionless platforms" });

            migrationBuilder.InsertData(
                table: "Employees",
                columns: new[] { "Id", "Company", "FirstName", "LastName", "Title" },
                values: new object[] { new Guid("4ae72880-635f-3cfd-f9ae-5a33fff28ec6"), "Lemke and Sons", "Vanessa", "Goyette", "redefine web-enabled paradigms" });

            migrationBuilder.InsertData(
                table: "Employees",
                columns: new[] { "Id", "Company", "FirstName", "LastName", "Title" },
                values: new object[] { new Guid("0eaf2450-9111-1e7e-85e2-3560a846c202"), "Hane, Daugherty and Lehner", "Lukas", "Marquardt", "aggregate viral portals" });

            migrationBuilder.InsertData(
                table: "Employees",
                columns: new[] { "Id", "Company", "FirstName", "LastName", "Title" },
                values: new object[] { new Guid("3d7388dd-7eff-0ee3-0420-440f7e0faa7c"), "Rice, Bergnaum and Steuber", "Viola", "Bashirian", "facilitate visionary schemas" });

            migrationBuilder.InsertData(
                table: "Employees",
                columns: new[] { "Id", "Company", "FirstName", "LastName", "Title" },
                values: new object[] { new Guid("bf22b128-d4be-46e6-41e5-79b9410fdca3"), "Kub - Jast", "Jolie", "Mertz", "embrace next-generation platforms" });

            migrationBuilder.InsertData(
                table: "Employees",
                columns: new[] { "Id", "Company", "FirstName", "LastName", "Title" },
                values: new object[] { new Guid("6db5d64d-a0c0-68ce-5026-21042fb733cd"), "Cormier - Kiehn", "Sonya", "Wunsch", "orchestrate granular initiatives" });

            migrationBuilder.InsertData(
                table: "Employees",
                columns: new[] { "Id", "Company", "FirstName", "LastName", "Title" },
                values: new object[] { new Guid("c6f1683a-f790-9d7c-4452-e180e724aec3"), "Hermiston LLC", "Taya", "Bartell", "reinvent customized metrics" });

            migrationBuilder.InsertData(
                table: "Employees",
                columns: new[] { "Id", "Company", "FirstName", "LastName", "Title" },
                values: new object[] { new Guid("6add7d9f-6c5b-7e21-c4c4-c1bd38fa09bf"), "Franecki, Carter and Jerde", "Florine", "Schneider", "unleash real-time channels" });

            migrationBuilder.InsertData(
                table: "Employees",
                columns: new[] { "Id", "Company", "FirstName", "LastName", "Title" },
                values: new object[] { new Guid("f15cd3bf-5e5f-8051-1be4-d3d51a7b1a2d"), "Weimann LLC", "Delphia", "Sipes", "drive strategic bandwidth" });

            migrationBuilder.InsertData(
                table: "Employees",
                columns: new[] { "Id", "Company", "FirstName", "LastName", "Title" },
                values: new object[] { new Guid("2be45228-9081-eb95-691e-0b3af1594aa5"), "Mante, Olson and Kreiger", "Imani", "Schoen", "matrix viral ROI" });

            migrationBuilder.InsertData(
                table: "Employees",
                columns: new[] { "Id", "Company", "FirstName", "LastName", "Title" },
                values: new object[] { new Guid("0d11431b-b926-fc46-abc2-0c7f2b9ba935"), "Collier - McLaughlin", "Thomas", "Hoeger", "engage e-business technologies" });

            migrationBuilder.InsertData(
                table: "Employees",
                columns: new[] { "Id", "Company", "FirstName", "LastName", "Title" },
                values: new object[] { new Guid("475f3ea6-4172-ece9-438f-7eb2e3a81c54"), "Rogahn, Schroeder and Waters", "Jeramy", "Weimann", "orchestrate killer e-services" });

            migrationBuilder.InsertData(
                table: "Employees",
                columns: new[] { "Id", "Company", "FirstName", "LastName", "Title" },
                values: new object[] { new Guid("d24ffe87-de0d-606e-c413-112004c78ead"), "Nicolas, Lockman and Wiegand", "Clementina", "Pacocha", "target dynamic eyeballs" });

            migrationBuilder.InsertData(
                table: "Employees",
                columns: new[] { "Id", "Company", "FirstName", "LastName", "Title" },
                values: new object[] { new Guid("e4088767-6020-e890-b8ea-429a1a35b00f"), "Schinner Inc", "Chauncey", "Oberbrunner", "revolutionize open-source applications" });

            migrationBuilder.InsertData(
                table: "Employees",
                columns: new[] { "Id", "Company", "FirstName", "LastName", "Title" },
                values: new object[] { new Guid("54cd48d8-441d-08cc-ff92-a657667ba3b3"), "Hoppe, Ortiz and Zemlak", "Gay", "Rolfson", "embrace mission-critical relationships" });

            migrationBuilder.InsertData(
                table: "Employees",
                columns: new[] { "Id", "Company", "FirstName", "LastName", "Title" },
                values: new object[] { new Guid("c96eac5c-1e87-e19d-4332-dde8b0b30e70"), "Wolff LLC", "Riley", "Weimann", "maximize out-of-the-box applications" });

            migrationBuilder.InsertData(
                table: "Employees",
                columns: new[] { "Id", "Company", "FirstName", "LastName", "Title" },
                values: new object[] { new Guid("3c2727fa-629e-5fdb-1e28-082a9d483567"), "Johnson - Hegmann", "Rhianna", "Kiehn", "facilitate innovative users" });

            migrationBuilder.InsertData(
                table: "Employees",
                columns: new[] { "Id", "Company", "FirstName", "LastName", "Title" },
                values: new object[] { new Guid("f09b6dee-0fd8-efd7-acd3-c9c1778ce153"), "O'Connell, Hayes and Moen", "Rosella", "Spinka", "incubate back-end methodologies" });

            migrationBuilder.InsertData(
                table: "Employees",
                columns: new[] { "Id", "Company", "FirstName", "LastName", "Title" },
                values: new object[] { new Guid("1779e9a1-c676-208d-443e-f40c5ccaef14"), "Smitham Inc", "Adell", "Greenfelder", "evolve frictionless paradigms" });

            migrationBuilder.InsertData(
                table: "Employees",
                columns: new[] { "Id", "Company", "FirstName", "LastName", "Title" },
                values: new object[] { new Guid("d3a4dc54-ab6b-a4f0-5819-3a504141315d"), "Macejkovic, Beatty and Pacocha", "Emery", "Waelchi", "transition revolutionary vortals" });

            migrationBuilder.InsertData(
                table: "Employees",
                columns: new[] { "Id", "Company", "FirstName", "LastName", "Title" },
                values: new object[] { new Guid("930d082b-b1d1-3530-9b05-36adca94ee07"), "Connelly LLC", "Waino", "Braun", "enhance customized platforms" });

            migrationBuilder.InsertData(
                table: "Employees",
                columns: new[] { "Id", "Company", "FirstName", "LastName", "Title" },
                values: new object[] { new Guid("669e2607-4e58-02d5-0e19-584914ef2ab6"), "Swaniawski Inc", "Donny", "Steuber", "mesh compelling mindshare" });

            migrationBuilder.InsertData(
                table: "Employees",
                columns: new[] { "Id", "Company", "FirstName", "LastName", "Title" },
                values: new object[] { new Guid("a79ae305-db2e-6f32-941c-12ad97fd2d30"), "Moore, Lebsack and Durgan", "Melany", "Grady", "aggregate synergistic networks" });

            migrationBuilder.InsertData(
                table: "Employees",
                columns: new[] { "Id", "Company", "FirstName", "LastName", "Title" },
                values: new object[] { new Guid("fafc49f2-d33e-fa68-4656-a18e5b7017e7"), "Robel, Shields and Jones", "Lesly", "Lueilwitz", "seize B2B systems" });

            migrationBuilder.InsertData(
                table: "Employees",
                columns: new[] { "Id", "Company", "FirstName", "LastName", "Title" },
                values: new object[] { new Guid("886aa473-4cf6-b97d-2c0c-e57db3b7f814"), "Bartoletti - Marks", "Emma", "Kiehn", "orchestrate frictionless bandwidth" });

            migrationBuilder.InsertData(
                table: "Employees",
                columns: new[] { "Id", "Company", "FirstName", "LastName", "Title" },
                values: new object[] { new Guid("f9c33732-6ad9-8adf-0abb-554010a98f18"), "Stroman - Ernser", "Magnolia", "Stokes", "extend holistic e-services" });

            migrationBuilder.InsertData(
                table: "Employees",
                columns: new[] { "Id", "Company", "FirstName", "LastName", "Title" },
                values: new object[] { new Guid("179f27ba-76b4-d2b1-7f98-13d12090b96e"), "Watsica - Douglas", "Keith", "Durgan", "cultivate sexy mindshare" });

            migrationBuilder.InsertData(
                table: "Employees",
                columns: new[] { "Id", "Company", "FirstName", "LastName", "Title" },
                values: new object[] { new Guid("1030e201-b067-43c1-8f4a-bcdd4d97d659"), "Brakus - Abernathy", "June", "Murray", "engineer global e-tailers" });

            migrationBuilder.InsertData(
                table: "Employees",
                columns: new[] { "Id", "Company", "FirstName", "LastName", "Title" },
                values: new object[] { new Guid("7711bed7-b314-a050-201f-7fbb0fd5b4de"), "Bode, Metz and McLaughlin", "Kailee", "Marks", "seize cross-media applications" });

            migrationBuilder.InsertData(
                table: "Employees",
                columns: new[] { "Id", "Company", "FirstName", "LastName", "Title" },
                values: new object[] { new Guid("22fc89e2-ef7b-acc5-8aaa-d44d31a7bb86"), "Bashirian and Sons", "Clint", "Huel", "deploy web-enabled platforms" });

            migrationBuilder.InsertData(
                table: "Employees",
                columns: new[] { "Id", "Company", "FirstName", "LastName", "Title" },
                values: new object[] { new Guid("619bf749-0c44-2ff8-cd0c-d8923bca731d"), "Kilback - Leffler", "Ulises", "Abshire", "empower distributed channels" });

            migrationBuilder.InsertData(
                table: "Employees",
                columns: new[] { "Id", "Company", "FirstName", "LastName", "Title" },
                values: new object[] { new Guid("0cf167e7-45a2-2d0f-fe1f-8635005793be"), "Strosin LLC", "Misael", "Von", "deploy sticky e-tailers" });

            migrationBuilder.InsertData(
                table: "Employees",
                columns: new[] { "Id", "Company", "FirstName", "LastName", "Title" },
                values: new object[] { new Guid("c63cc4e8-3e77-5f5e-e265-3da480ae098b"), "Spencer Group", "Wendy", "Hamill", "repurpose cutting-edge communities" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Employees");
        }
    }
}
