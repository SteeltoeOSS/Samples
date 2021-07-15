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
                values: new object[] { new Guid("08d6ed22-94e5-8e9c-14d5-047ca1939d31"), "Skiles LLC", "Blaze", "Pouros", "synthesize compelling e-business" });

            migrationBuilder.InsertData(
                table: "Employees",
                columns: new[] { "Id", "Company", "FirstName", "LastName", "Title" },
                values: new object[] { new Guid("0c36a109-cbb7-d2ad-b442-1d31b291895c"), "Murazik, Farrell and Kihn", "Rigoberto", "Howe", "target 24/365 paradigms" });

            migrationBuilder.InsertData(
                table: "Employees",
                columns: new[] { "Id", "Company", "FirstName", "LastName", "Title" },
                values: new object[] { new Guid("b323dc8d-4c31-df64-2d50-18941e74b7d7"), "Batz Group", "Dominique", "Carroll", "incentivize granular bandwidth" });

            migrationBuilder.InsertData(
                table: "Employees",
                columns: new[] { "Id", "Company", "FirstName", "LastName", "Title" },
                values: new object[] { new Guid("ab87473c-d50e-ba45-a88c-b4ef8b265b07"), "Osinski - Stiedemann", "Alaina", "Schulist", "integrate web-enabled niches" });

            migrationBuilder.InsertData(
                table: "Employees",
                columns: new[] { "Id", "Company", "FirstName", "LastName", "Title" },
                values: new object[] { new Guid("55f3c163-a5db-c620-adb0-3c36b1c03989"), "Abshire - Jenkins", "Laverne", "Roob", "integrate leading-edge solutions" });

            migrationBuilder.InsertData(
                table: "Employees",
                columns: new[] { "Id", "Company", "FirstName", "LastName", "Title" },
                values: new object[] { new Guid("e1a8945e-b017-7b7e-36ac-9caa797932a1"), "Haag, Corwin and Murazik", "Mack", "Spinka", "repurpose virtual mindshare" });

            migrationBuilder.InsertData(
                table: "Employees",
                columns: new[] { "Id", "Company", "FirstName", "LastName", "Title" },
                values: new object[] { new Guid("373a9231-d10e-93de-d8ea-080c9b554f9c"), "Heathcote, Rodriguez and Bahringer", "May", "Kohler", "deploy synergistic synergies" });

            migrationBuilder.InsertData(
                table: "Employees",
                columns: new[] { "Id", "Company", "FirstName", "LastName", "Title" },
                values: new object[] { new Guid("de9dae48-e52e-88b0-0380-d6ddb16ec0b4"), "Walsh Inc", "Anissa", "Koch", "expedite 24/365 interfaces" });

            migrationBuilder.InsertData(
                table: "Employees",
                columns: new[] { "Id", "Company", "FirstName", "LastName", "Title" },
                values: new object[] { new Guid("db735df5-386d-de43-b9aa-8f15572762a2"), "Nienow, Haley and Block", "Hassan", "Botsford", "scale open-source synergies" });

            migrationBuilder.InsertData(
                table: "Employees",
                columns: new[] { "Id", "Company", "FirstName", "LastName", "Title" },
                values: new object[] { new Guid("506a5067-39ba-7d34-82ae-95d95d616923"), "Kub - Kozey", "Ambrose", "Robel", "recontextualize synergistic markets" });

            migrationBuilder.InsertData(
                table: "Employees",
                columns: new[] { "Id", "Company", "FirstName", "LastName", "Title" },
                values: new object[] { new Guid("8662ad9e-3ee1-b272-9f97-3fa1c60026c2"), "Stroman, Braun and McCullough", "Flavio", "Friesen", "expedite vertical schemas" });

            migrationBuilder.InsertData(
                table: "Employees",
                columns: new[] { "Id", "Company", "FirstName", "LastName", "Title" },
                values: new object[] { new Guid("8c74fa6b-2e10-f1fa-a316-426732f6bf48"), "Johnston Inc", "Chauncey", "Harvey", "envisioneer 24/7 paradigms" });

            migrationBuilder.InsertData(
                table: "Employees",
                columns: new[] { "Id", "Company", "FirstName", "LastName", "Title" },
                values: new object[] { new Guid("ab9e8ca0-3b9c-378c-f142-79880180ccab"), "Heidenreich Group", "Golda", "Koss", "incubate sticky e-tailers" });

            migrationBuilder.InsertData(
                table: "Employees",
                columns: new[] { "Id", "Company", "FirstName", "LastName", "Title" },
                values: new object[] { new Guid("dcac4d43-f3e2-a1d0-5c83-adf289f252d7"), "Kuhic, Cormier and Kunde", "Catalina", "Prosacco", "drive vertical convergence" });

            migrationBuilder.InsertData(
                table: "Employees",
                columns: new[] { "Id", "Company", "FirstName", "LastName", "Title" },
                values: new object[] { new Guid("5ea18429-c786-de34-63c0-7c7d1ffe7846"), "Runolfsdottir, Bruen and Koelpin", "Destinee", "Hettinger", "morph frictionless markets" });

            migrationBuilder.InsertData(
                table: "Employees",
                columns: new[] { "Id", "Company", "FirstName", "LastName", "Title" },
                values: new object[] { new Guid("ad328fc8-9432-78e8-190b-1cdd2cd5bb69"), "Nader Group", "Margaret", "Dach", "incentivize proactive networks" });

            migrationBuilder.InsertData(
                table: "Employees",
                columns: new[] { "Id", "Company", "FirstName", "LastName", "Title" },
                values: new object[] { new Guid("45205ec8-520c-aaac-db5f-58734e55cb24"), "Koelpin LLC", "Christa", "Goldner", "generate real-time architectures" });

            migrationBuilder.InsertData(
                table: "Employees",
                columns: new[] { "Id", "Company", "FirstName", "LastName", "Title" },
                values: new object[] { new Guid("5598a932-2e3d-be1c-7bed-63cc740f64a3"), "Johnston, Walter and Wolff", "Mortimer", "Bartell", "transform bricks-and-clicks vortals" });

            migrationBuilder.InsertData(
                table: "Employees",
                columns: new[] { "Id", "Company", "FirstName", "LastName", "Title" },
                values: new object[] { new Guid("e56d8aac-4c3b-c71b-bb07-2be877c36b3c"), "Willms - Ward", "Laurine", "Hickle", "exploit sexy paradigms" });

            migrationBuilder.InsertData(
                table: "Employees",
                columns: new[] { "Id", "Company", "FirstName", "LastName", "Title" },
                values: new object[] { new Guid("c38836fd-435b-2fb8-dfc2-6e39f6470ada"), "Ledner Group", "Philip", "Jakubowski", "transform wireless e-markets" });

            migrationBuilder.InsertData(
                table: "Employees",
                columns: new[] { "Id", "Company", "FirstName", "LastName", "Title" },
                values: new object[] { new Guid("2c337bd1-fff2-2deb-8828-2c935cac7e38"), "Stoltenberg - Zemlak", "Kristopher", "Hirthe", "empower strategic metrics" });

            migrationBuilder.InsertData(
                table: "Employees",
                columns: new[] { "Id", "Company", "FirstName", "LastName", "Title" },
                values: new object[] { new Guid("d90ad403-1772-2592-33b5-e9aa33faba63"), "Reilly, Schiller and Hamill", "Yasmine", "Koch", "target scalable users" });

            migrationBuilder.InsertData(
                table: "Employees",
                columns: new[] { "Id", "Company", "FirstName", "LastName", "Title" },
                values: new object[] { new Guid("7f4a5582-823a-d4bc-4d80-4c3376f07180"), "Ziemann - Langworth", "Rudy", "McLaughlin", "architect extensible synergies" });

            migrationBuilder.InsertData(
                table: "Employees",
                columns: new[] { "Id", "Company", "FirstName", "LastName", "Title" },
                values: new object[] { new Guid("129ad98d-ff6b-797e-f5e6-008f77ea1446"), "Little, Carter and Schmeler", "Mohammed", "Zieme", "reinvent 24/7 e-tailers" });

            migrationBuilder.InsertData(
                table: "Employees",
                columns: new[] { "Id", "Company", "FirstName", "LastName", "Title" },
                values: new object[] { new Guid("0ed85955-cd26-5c88-71fe-98527d901b55"), "Sporer LLC", "Amely", "Mraz", "transition magnetic convergence" });

            migrationBuilder.InsertData(
                table: "Employees",
                columns: new[] { "Id", "Company", "FirstName", "LastName", "Title" },
                values: new object[] { new Guid("552b3043-50e8-3af8-13b4-1432100c4d32"), "Hodkiewicz Group", "Eddie", "Bins", "unleash transparent convergence" });

            migrationBuilder.InsertData(
                table: "Employees",
                columns: new[] { "Id", "Company", "FirstName", "LastName", "Title" },
                values: new object[] { new Guid("dc7f80a0-a983-04e6-a5b2-f25eb4d7cb94"), "Roob - Glover", "Euna", "Maggio", "exploit user-centric partnerships" });

            migrationBuilder.InsertData(
                table: "Employees",
                columns: new[] { "Id", "Company", "FirstName", "LastName", "Title" },
                values: new object[] { new Guid("b5c6736c-8fb2-d0e9-dcdd-7c7dc1f427ee"), "Green LLC", "Molly", "Veum", "visualize B2C models" });

            migrationBuilder.InsertData(
                table: "Employees",
                columns: new[] { "Id", "Company", "FirstName", "LastName", "Title" },
                values: new object[] { new Guid("0ce6c797-a6b0-83cf-7ba9-7002ba1a5e67"), "Barrows Inc", "Anissa", "Russel", "morph next-generation e-business" });

            migrationBuilder.InsertData(
                table: "Employees",
                columns: new[] { "Id", "Company", "FirstName", "LastName", "Title" },
                values: new object[] { new Guid("5e9367db-7ad1-7e8c-2481-aae76f06c1d0"), "Orn - Towne", "Estevan", "Kertzmann", "innovate web-enabled paradigms" });

            migrationBuilder.InsertData(
                table: "Employees",
                columns: new[] { "Id", "Company", "FirstName", "LastName", "Title" },
                values: new object[] { new Guid("1db2a781-aab6-a65e-26c7-6489f118e9b4"), "Farrell - Hartmann", "Katlyn", "Schulist", "grow plug-and-play functionalities" });

            migrationBuilder.InsertData(
                table: "Employees",
                columns: new[] { "Id", "Company", "FirstName", "LastName", "Title" },
                values: new object[] { new Guid("224cc40c-a5ba-cfb1-7877-0877baac2be4"), "Brekke - Murray", "Bert", "Kautzer", "cultivate leading-edge users" });

            migrationBuilder.InsertData(
                table: "Employees",
                columns: new[] { "Id", "Company", "FirstName", "LastName", "Title" },
                values: new object[] { new Guid("ff83a679-4e70-3a3e-a359-2555ba571f7a"), "Dach, Hansen and Shields", "Meagan", "Fisher", "aggregate real-time models" });

            migrationBuilder.InsertData(
                table: "Employees",
                columns: new[] { "Id", "Company", "FirstName", "LastName", "Title" },
                values: new object[] { new Guid("f1d4396d-fbaf-1db9-7923-b067742e3acd"), "Carter - Murazik", "Natasha", "Oberbrunner", "maximize distributed e-commerce" });

            migrationBuilder.InsertData(
                table: "Employees",
                columns: new[] { "Id", "Company", "FirstName", "LastName", "Title" },
                values: new object[] { new Guid("938e4259-8e8e-cf48-75f7-fa18d6618374"), "Lynch LLC", "Kiana", "Waters", "engineer one-to-one schemas" });

            migrationBuilder.InsertData(
                table: "Employees",
                columns: new[] { "Id", "Company", "FirstName", "LastName", "Title" },
                values: new object[] { new Guid("09723f66-02aa-c9c1-5bfb-9272dc89d53f"), "Harber - Aufderhar", "Hope", "Nader", "cultivate front-end solutions" });

            migrationBuilder.InsertData(
                table: "Employees",
                columns: new[] { "Id", "Company", "FirstName", "LastName", "Title" },
                values: new object[] { new Guid("61228099-4857-2e6b-774d-3b3473a3917b"), "Pagac Group", "Makenna", "Carter", "generate virtual e-business" });

            migrationBuilder.InsertData(
                table: "Employees",
                columns: new[] { "Id", "Company", "FirstName", "LastName", "Title" },
                values: new object[] { new Guid("573bce72-55e4-f953-69a9-a012f29319ce"), "Homenick Group", "Flo", "Casper", "maximize seamless functionalities" });

            migrationBuilder.InsertData(
                table: "Employees",
                columns: new[] { "Id", "Company", "FirstName", "LastName", "Title" },
                values: new object[] { new Guid("04ce5186-bcfd-073a-715b-f068b159d271"), "Schneider - Morar", "Jeremy", "Jerde", "orchestrate back-end solutions" });

            migrationBuilder.InsertData(
                table: "Employees",
                columns: new[] { "Id", "Company", "FirstName", "LastName", "Title" },
                values: new object[] { new Guid("566ba194-3ab2-c846-68f9-66fc1a3d779c"), "Thompson, Mosciski and Nolan", "Paolo", "McClure", "leverage user-centric users" });

            migrationBuilder.InsertData(
                table: "Employees",
                columns: new[] { "Id", "Company", "FirstName", "LastName", "Title" },
                values: new object[] { new Guid("ef0112ba-d391-0c07-0cb7-ec9dc5c49ff6"), "Simonis LLC", "Nickolas", "Jerde", "engineer 24/7 markets" });

            migrationBuilder.InsertData(
                table: "Employees",
                columns: new[] { "Id", "Company", "FirstName", "LastName", "Title" },
                values: new object[] { new Guid("739cff16-8082-2835-f492-58a80d849ad3"), "Labadie, Zboncak and Mayer", "Cecelia", "Cummings", "target dynamic paradigms" });

            migrationBuilder.InsertData(
                table: "Employees",
                columns: new[] { "Id", "Company", "FirstName", "LastName", "Title" },
                values: new object[] { new Guid("f313bfdb-37ce-4abc-96db-8dc1152b1acd"), "Quigley, Feeney and Gottlieb", "Candelario", "Ryan", "mesh 24/7 architectures" });

            migrationBuilder.InsertData(
                table: "Employees",
                columns: new[] { "Id", "Company", "FirstName", "LastName", "Title" },
                values: new object[] { new Guid("6ebb1f02-bc18-5200-d190-baebc40bdbba"), "Brown Inc", "Adrien", "Shanahan", "maximize intuitive technologies" });

            migrationBuilder.InsertData(
                table: "Employees",
                columns: new[] { "Id", "Company", "FirstName", "LastName", "Title" },
                values: new object[] { new Guid("ad76a3bb-e190-99d0-3289-1d7b6e19605f"), "Wintheiser, Waelchi and Schoen", "Kelly", "Feest", "matrix cross-media web services" });

            migrationBuilder.InsertData(
                table: "Employees",
                columns: new[] { "Id", "Company", "FirstName", "LastName", "Title" },
                values: new object[] { new Guid("4c11f253-615e-9c71-bc8b-a1946e7acc3b"), "Hodkiewicz - Schimmel", "Jasmin", "Kunze", "brand granular action-items" });

            migrationBuilder.InsertData(
                table: "Employees",
                columns: new[] { "Id", "Company", "FirstName", "LastName", "Title" },
                values: new object[] { new Guid("fc1441b2-6d71-657f-bec1-7a3f1b5ca993"), "Senger, Hauck and Parker", "Kelton", "Pouros", "brand back-end networks" });

            migrationBuilder.InsertData(
                table: "Employees",
                columns: new[] { "Id", "Company", "FirstName", "LastName", "Title" },
                values: new object[] { new Guid("ce69bd33-e33d-6aa8-ec3a-bb76e5fb44f6"), "Zulauf, Dicki and Hegmann", "Ashton", "Hirthe", "embrace user-centric e-commerce" });

            migrationBuilder.InsertData(
                table: "Employees",
                columns: new[] { "Id", "Company", "FirstName", "LastName", "Title" },
                values: new object[] { new Guid("6e2f283f-6621-7c7a-6fb8-371fd7145a57"), "Bruen - Schoen", "Maxine", "Homenick", "cultivate e-business technologies" });

            migrationBuilder.InsertData(
                table: "Employees",
                columns: new[] { "Id", "Company", "FirstName", "LastName", "Title" },
                values: new object[] { new Guid("8801795b-38f4-e9a9-998c-3b3246d9f8a6"), "Terry, Goyette and Nitzsche", "Noel", "Bogan", "redefine 24/365 infomediaries" });

            migrationBuilder.InsertData(
                table: "Employees",
                columns: new[] { "Id", "Company", "FirstName", "LastName", "Title" },
                values: new object[] { new Guid("1e564f70-6502-2d70-1d34-94a20819826f"), "Graham LLC", "Carmella", "Davis", "strategize ubiquitous e-markets" });

            migrationBuilder.InsertData(
                table: "Employees",
                columns: new[] { "Id", "Company", "FirstName", "LastName", "Title" },
                values: new object[] { new Guid("79982f5c-e3c8-a8cc-9f7d-96f4d6ac7adc"), "Mann, Gottlieb and King", "Arno", "Nikolaus", "synthesize real-time applications" });

            migrationBuilder.InsertData(
                table: "Employees",
                columns: new[] { "Id", "Company", "FirstName", "LastName", "Title" },
                values: new object[] { new Guid("bc4ce4cf-c390-813a-e165-3ccf06c87925"), "Lakin, Schuppe and Bernhard", "Immanuel", "Schultz", "harness web-enabled e-commerce" });

            migrationBuilder.InsertData(
                table: "Employees",
                columns: new[] { "Id", "Company", "FirstName", "LastName", "Title" },
                values: new object[] { new Guid("4cde8566-407a-3d92-3d16-ee2d4325f6f6"), "Schaden Group", "Annamarie", "Wilderman", "morph cross-platform metrics" });

            migrationBuilder.InsertData(
                table: "Employees",
                columns: new[] { "Id", "Company", "FirstName", "LastName", "Title" },
                values: new object[] { new Guid("fb76f967-54a2-15e5-f466-eea081d42b97"), "Windler, Ledner and Wunsch", "Alia", "Davis", "leverage innovative interfaces" });

            migrationBuilder.InsertData(
                table: "Employees",
                columns: new[] { "Id", "Company", "FirstName", "LastName", "Title" },
                values: new object[] { new Guid("3821e3d1-e77d-3fa4-c84b-db1d8965bb52"), "Mertz, Nitzsche and Kozey", "Jerrod", "Lowe", "deploy user-centric blockchains" });

            migrationBuilder.InsertData(
                table: "Employees",
                columns: new[] { "Id", "Company", "FirstName", "LastName", "Title" },
                values: new object[] { new Guid("370bc82d-c2d2-52b9-123b-f70ce02e54b6"), "Cremin Inc", "Jordyn", "Smith", "envisioneer strategic portals" });

            migrationBuilder.InsertData(
                table: "Employees",
                columns: new[] { "Id", "Company", "FirstName", "LastName", "Title" },
                values: new object[] { new Guid("961ecfe2-7734-ef6d-7c9f-5794758a9d18"), "Schiller Inc", "Santiago", "Schamberger", "transition rich portals" });

            migrationBuilder.InsertData(
                table: "Employees",
                columns: new[] { "Id", "Company", "FirstName", "LastName", "Title" },
                values: new object[] { new Guid("5c2035b5-6918-2370-555b-425806bad32c"), "Kohler Group", "Arden", "Corkery", "whiteboard revolutionary networks" });

            migrationBuilder.InsertData(
                table: "Employees",
                columns: new[] { "Id", "Company", "FirstName", "LastName", "Title" },
                values: new object[] { new Guid("8581268e-8543-30f3-842d-b9ec461d6306"), "Leuschke - Rippin", "Jaydon", "Bosco", "leverage dynamic content" });

            migrationBuilder.InsertData(
                table: "Employees",
                columns: new[] { "Id", "Company", "FirstName", "LastName", "Title" },
                values: new object[] { new Guid("f8e46c4c-d772-019f-9be0-cef1e8ebdfd0"), "Schowalter Group", "Elenor", "Miller", "syndicate sexy models" });

            migrationBuilder.InsertData(
                table: "Employees",
                columns: new[] { "Id", "Company", "FirstName", "LastName", "Title" },
                values: new object[] { new Guid("20df1216-1b0e-082d-7029-c8a7872f169e"), "Mohr - Greenfelder", "Anna", "Schmitt", "productize e-business initiatives" });

            migrationBuilder.InsertData(
                table: "Employees",
                columns: new[] { "Id", "Company", "FirstName", "LastName", "Title" },
                values: new object[] { new Guid("b34c3d4c-af02-e5a5-ad12-e0eeb9d5f127"), "Batz, Hessel and Hoeger", "Adele", "Walter", "engage robust technologies" });

            migrationBuilder.InsertData(
                table: "Employees",
                columns: new[] { "Id", "Company", "FirstName", "LastName", "Title" },
                values: new object[] { new Guid("23d9c8dc-0575-3bf4-32c6-44599b90e6a4"), "Conn, Hackett and Bayer", "Lacey", "Nolan", "visualize viral e-services" });

            migrationBuilder.InsertData(
                table: "Employees",
                columns: new[] { "Id", "Company", "FirstName", "LastName", "Title" },
                values: new object[] { new Guid("ef61e85c-c8a9-4bdd-f61c-cef2728438df"), "Green - Reilly", "Laurence", "Olson", "enhance innovative web-readiness" });

            migrationBuilder.InsertData(
                table: "Employees",
                columns: new[] { "Id", "Company", "FirstName", "LastName", "Title" },
                values: new object[] { new Guid("06548d68-41a0-7ffd-906c-e5c9cefe73b3"), "Deckow, Nolan and Nolan", "Naomie", "Metz", "transition 24/7 infrastructures" });

            migrationBuilder.InsertData(
                table: "Employees",
                columns: new[] { "Id", "Company", "FirstName", "LastName", "Title" },
                values: new object[] { new Guid("84f9eded-465c-2478-7add-3fb70c035a85"), "Ondricka - Jaskolski", "Jessica", "Kerluke", "innovate proactive deliverables" });

            migrationBuilder.InsertData(
                table: "Employees",
                columns: new[] { "Id", "Company", "FirstName", "LastName", "Title" },
                values: new object[] { new Guid("d26411fe-c7b7-cf82-fa86-7a66093d2a5f"), "Shanahan Inc", "Julio", "Sipes", "deliver efficient markets" });

            migrationBuilder.InsertData(
                table: "Employees",
                columns: new[] { "Id", "Company", "FirstName", "LastName", "Title" },
                values: new object[] { new Guid("3e63a5a7-71c8-12d5-4d84-f68b5ca975ab"), "Maggio and Sons", "Alice", "Connelly", "exploit best-of-breed e-commerce" });

            migrationBuilder.InsertData(
                table: "Employees",
                columns: new[] { "Id", "Company", "FirstName", "LastName", "Title" },
                values: new object[] { new Guid("a0b9189a-0b22-7b6d-f3d0-f9ac2f6e800a"), "Windler, Kihn and Purdy", "Joelle", "Batz", "morph frictionless metrics" });

            migrationBuilder.InsertData(
                table: "Employees",
                columns: new[] { "Id", "Company", "FirstName", "LastName", "Title" },
                values: new object[] { new Guid("4e620a78-0f1f-fd44-cc81-19bb86bd724e"), "Crooks, Marvin and Williamson", "Jamir", "Olson", "enable virtual ROI" });

            migrationBuilder.InsertData(
                table: "Employees",
                columns: new[] { "Id", "Company", "FirstName", "LastName", "Title" },
                values: new object[] { new Guid("48a9c2d8-0f04-8d7e-8f4f-da6ee4b2f59d"), "Goyette and Sons", "Austen", "Stracke", "visualize killer networks" });

            migrationBuilder.InsertData(
                table: "Employees",
                columns: new[] { "Id", "Company", "FirstName", "LastName", "Title" },
                values: new object[] { new Guid("f1cc4805-2f7e-7706-5d30-269658c97519"), "Schuppe - Rippin", "Evelyn", "Gulgowski", "aggregate turn-key web-readiness" });

            migrationBuilder.InsertData(
                table: "Employees",
                columns: new[] { "Id", "Company", "FirstName", "LastName", "Title" },
                values: new object[] { new Guid("df80bfe4-b1b7-73a3-cd0d-a8c58b6601a0"), "Russel Inc", "Uriel", "Mertz", "mesh killer systems" });

            migrationBuilder.InsertData(
                table: "Employees",
                columns: new[] { "Id", "Company", "FirstName", "LastName", "Title" },
                values: new object[] { new Guid("0bf4d19f-d855-2ca9-caa5-69062d94cd07"), "Sanford and Sons", "Dino", "Haley", "generate leading-edge e-services" });

            migrationBuilder.InsertData(
                table: "Employees",
                columns: new[] { "Id", "Company", "FirstName", "LastName", "Title" },
                values: new object[] { new Guid("484a2755-7f97-5672-7828-1aef0aae5d5d"), "Toy - Lang", "Emelie", "Cummings", "maximize integrated initiatives" });

            migrationBuilder.InsertData(
                table: "Employees",
                columns: new[] { "Id", "Company", "FirstName", "LastName", "Title" },
                values: new object[] { new Guid("bf13fa34-782e-65dd-8623-30db5250187e"), "Jast Inc", "Delbert", "Schinner", "redefine scalable models" });

            migrationBuilder.InsertData(
                table: "Employees",
                columns: new[] { "Id", "Company", "FirstName", "LastName", "Title" },
                values: new object[] { new Guid("4e8496c8-1de9-8b6e-d5a2-38d7a1fabdac"), "Herman LLC", "Rubye", "Ankunding", "extend innovative web-readiness" });

            migrationBuilder.InsertData(
                table: "Employees",
                columns: new[] { "Id", "Company", "FirstName", "LastName", "Title" },
                values: new object[] { new Guid("1c9e5590-cfd7-85a4-a77b-6678e26136ee"), "Reilly - Rowe", "Marcelino", "Stamm", "drive seamless mindshare" });

            migrationBuilder.InsertData(
                table: "Employees",
                columns: new[] { "Id", "Company", "FirstName", "LastName", "Title" },
                values: new object[] { new Guid("b4d24c34-db65-1ef0-438a-e2c2f4085be6"), "Hilll, Bailey and Bayer", "Oswald", "Koss", "transform real-time deliverables" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Employees");
        }
    }
}
