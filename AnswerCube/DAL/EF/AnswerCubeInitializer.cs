using AnswerCube.BL.Domain;
using AnswerCube.BL.Domain.Project;
using AnswerCube.BL.Domain.Slide;
using AnswerCube.BL.Domain.User;
using Domain;
using Microsoft.AspNetCore.Identity;

namespace AnswerCube.DAL.EF;

public static class AnswerCubeInitializer
{
    private static bool hasBeenInitialized = false;

    public static void Initialize(AnswerCubeDbContext context, bool dropDatabase = true)
    {
        if (!hasBeenInitialized)
        {
            if (dropDatabase)
                context.Database.EnsureDeleted();

            if (context.Database.EnsureCreated())
                Seed(context);
        }

        hasBeenInitialized = true;
    }

    private static void Seed(AnswerCubeDbContext context)
    {
        //Add Users
        var hasher = new PasswordHasher<AnswerCubeUser>();
        var yannick = new AnswerCubeUser()
        {
            Id = "superUser1",
            FirstName = "Yannick",
            LastName = "Vandenbulcke",
            UserName = "vandenbulckeyannick@gmail.com",
            NormalizedUserName = "VANDENBULCKEYANNICK@GMAIL.COM",
            Email = "vandenbulckeyannick@gmail.com",
            NormalizedEmail = "VANDENBULCKEYANNICK@GMAIL.COM",
            EmailConfirmed = true
        };
        context.UserRoles.Add(new IdentityUserRole<string>
        {
            RoleId = context.Roles.First(role => role.Name == "Admin").Id,
            UserId = "superUser1"
        });
        context.UserRoles.Add(new IdentityUserRole<string>
        {
            RoleId = context.Roles.First(role => role.Name == "Gebruiker").Id,
            UserId = "superUser1"
        });
        context.UserRoles.Add(new IdentityUserRole<string>
        {
            RoleId = context.Roles.First(role => role.Name == "DeelplatformBeheerder").Id,
            UserId = "superUser1"
        });
        yannick.PasswordHash = hasher.HashPassword(yannick, "Student_1234");
        //Add Organizations and add user and projects to organization
        var organization1 = new Organization("KdG",
            "skybloom44@gmail.com", null, Theme.Light);
        var organization2 = new Organization("AnswerCube",
            "answercubeintegratie@gmail.com", null);
        Forum answerCubeForum = new Forum
        {
            Organization = organization2,
            Ideas = new List<Idea>()
        };
        List<Idea> ideas = new List<Idea>
        {
            new Idea
            {
                Title = "Change the Caribian Flow",
                Content = "So people can enjoy the Caribian flow",
                ForumId = 1,
                Date = DateTime.UtcNow,
                User = yannick
            },
            new Idea
            {
                Title = " Change the way we manage KdG",
                Content = "So we can improve the quality of the course",
                ForumId = 1,
                Date = DateTime.UtcNow,
                User = yannick
            }
        };
        answerCubeForum.Ideas.AddRange(ideas);
        organization2.Forum = answerCubeForum;
        context.Forums.Add(answerCubeForum);

        var userOrganization1 = new UserOrganization
        {
            User = yannick,
            UserId = yannick.Id,
            Organization = organization1,
            OrganizationId = organization1.Id
        };
        context.DeelplatformbeheerderEmails.AddRange(new DeelplatformbeheerderEmail
        {
            Email = yannick.Email,
            DeelplatformNaam = organization1.Name
        }, new DeelplatformbeheerderEmail
        {
            Email = yannick.Email,
            DeelplatformNaam = organization2.Name
        });

        var userOrganization2 = new UserOrganization
        {
            User = yannick,
            UserId = yannick.Id,
            Organization = organization2,
            OrganizationId = organization2.Id
        };

        // Add the new UserOrganization to the context and save changes
        context.UserOrganizations.AddRange(userOrganization1, userOrganization2);
        context.SaveChanges();

        Flow circularFlow1 = new Flow
        {
            Name = "CircularFlow",
            Description = "ipsum lorum",
            CircularFlow = true
        };

        Project project1 = new Project
        {
            Title = "Pirates of the Carribean",
            Description = "This is a project about the cast, producers, fans and critics of the filmseries",
            IsActive = true,
            Organization = organization1,
            Flows = new List<Flow> { circularFlow1 }
        };
        Project project2 = new Project
        {
            Title = "Management KDG",
            Description =
                "This project is about the management of KdG, to see how the students think about the course Management",
            IsActive = true,
            Organization = organization1
        };
        Project project3 = new Project
        {
            Title = "Climate Change Research",
            Description = "A project dedicated to studying the impacts of climate change on local ecosystems.",
            IsActive = true,
            Organization = organization2
        };
        Project project4 = new Project
        {
            Title = "Urban Development",
            Description = "This project focuses on sustainable urban development practices.",
            IsActive = true,
            Organization = organization2
        };
        Project project5 = new Project
        {
            Title = "Educational Reform",
            Description = "A project aimed at improving the quality of education in underprivileged areas.",
            IsActive = true,
            Organization = organization1
        };
        Project project6 = new Project
        {
            Title = "Healthcare Improvement",
            Description = "This project is about improving healthcare services in rural areas.",
            IsActive = true,
            Organization = organization1
        };
        Project project7 = new Project
        {
            Title = "Renewable Energy",
            Description = "A project focused on the research and development of renewable energy sources.",
            IsActive = true,
            Organization = organization1
        };


        context.Projects.AddRange(project1, project2, project3, project4, project5, project6, project7);

        var jarno = new AnswerCubeUser()
        {
            Id = "superUser2",
            FirstName = "jarno",
            LastName = "Fret",
            UserName = "jarno.fret@student.kdg.be",
            NormalizedUserName = "JARNO.FRET@STUDENT.KDG.BE",
            Email = "jarno.fret@student.kdg.be",
            NormalizedEmail = "JARNO.FRET@STUDENT.KDG.BE",
            EmailConfirmed = true
        };
        context.UserRoles.Add(new IdentityUserRole<string>
        {
            RoleId = context.Roles.First(role => role.Name == "Admin").Id,
            UserId = "superUser2"
        });
        jarno.PasswordHash = hasher.HashPassword(jarno, "Student_1234");

        var alexander = new AnswerCubeUser()
        {
            Id = "superUser3",
            FirstName = "Alexander",
            LastName = "Van Puyenbroeck",
            UserName = "alexander.vanpuyenbroeck@student.kdg.be",
            NormalizedUserName = "ALEXANDER.VANPUYENBROECK@STUDENT.KDG.BE",
            Email = "alexander.vanpuyenbroeck@student.kdg.be",
            NormalizedEmail = "ALEXANDER.VANPUYENBROECK@STUDENT.KDG.BE",
            EmailConfirmed = true
        };
        context.UserRoles.Add(new IdentityUserRole<string>
        {
            RoleId = context.Roles.First(role => role.Name == "Admin").Id,
            UserId = "superUser3"
        });
        alexander.PasswordHash = hasher.HashPassword(alexander, "Student_1234");

        var nemo = new AnswerCubeUser
        {
            Id = "superUser4",
            FirstName = "Nemo",
            LastName = "Van Den Eynde",
            UserName = "nemo.vandeneynde@student.kdg.be",
            NormalizedUserName = "NEMO.VANDENEYNDE@STUDENT.KDG.BE",
            Email = "nemo.vandeneynde@student.kdg.be",
            NormalizedEmail = "NEMO.VANDENEYNDE@STUDENT.KDG.BE",
            EmailConfirmed = true
        };
        context.UserRoles.Add(new IdentityUserRole<string>
        {
            RoleId = context.Roles.First(role => role.Name == "Admin").Id,
            UserId = "superUser4"
        });
        nemo.PasswordHash = hasher.HashPassword(nemo, "Student_1234");

        var ilyasse = new AnswerCubeUser()
        {
            Id = "superUser5",
            FirstName = "Ilyasse",
            LastName = "jmida",
            UserName = "ilyasse.jmida@student.kdg.be",
            NormalizedUserName = "ILYASSE.JMIDA@STUDENT.KDG.BE",
            Email = "ilyasse.jmida@student.kdg.be",
            NormalizedEmail = "ILYASSE.JMIDA@STUDENT.KDG.BE",
            EmailConfirmed = true
        };
        context.UserRoles.Add(new IdentityUserRole<string>
        {
            RoleId = context.Roles.First(role => role.Name == "Admin").Id,
            UserId = "superUser5"
        });
        ilyasse.PasswordHash = hasher.HashPassword(ilyasse, "Student_1234");

        context.AnswerCubeUsers.AddRange(alexander, jarno, nemo, ilyasse);

        SlideList slideList1 = new SlideList
        {
            Title = "Biography of actors",
            SubTheme = new SubTheme("Biography of actors",
                "This theme discusses the biography of the actors, have fun!"),
            ConnectedSlides = new List<SlideConnection>()
        };

        SlideList slideList1punt2 = new SlideList
        {
            Title = "Career actors",
            SubTheme = new SubTheme("Career stuff of all actors",
                "This theme discusses the career of the actors, have fun!"),
            ConnectedSlides = new List<SlideConnection>()
        };
        
        context.SlideLists.AddRange(slideList1, slideList1punt2);
        

        //SingleChoice
        Slide singleChoice1 = new Slide
        {
            Text =
                "Als jij de begroting van je stad of gemeente zou opmaken, waar zou je dan in de komende jaren vooral op inzetten?",
            SlideType = SlideType.SingleChoice,
            AnswerList = new List<string>
            {
                "Natuur en ecologie", "Vrije tijd, sport, cultuur", "Huisvesting", "Onderwijs en kinderopvang",
                "Gezondheidszorg en welzijn", "Verkeersveiligheid en mobiliteit", "Ondersteunen van lokale handel"
            },
            ConnectedSlideLists = new List<SlideConnection>(),
            MediaUrl = "https://storage.googleapis.com/answer-cube-bucket/video_202405161407339529.mp4"
        };
        

        SlideConnection slideConnection1 = new SlideConnection
        {
            Slide = singleChoice1,
            SlideList = slideList1,
            SlideOrder = 1
        };
        singleChoice1.ConnectedSlideLists.Add(slideConnection1);


        Slide singleChoice2 = new Slide
        {
            Text =
                "Er moet meer geïnvesteerd worden in overdekte fietsstallingen aan de bushaltes in onze gemeente.",
            SlideType = SlideType.SingleChoice,
            AnswerList = new List<string> { "Eens", "Oneens" },
            ConnectedSlideLists = new List<SlideConnection>()
        };

        SlideConnection slideConnection2 = new SlideConnection
        {
            Slide = singleChoice2,
            SlideList = slideList1,
            SlideOrder = 2
        };
        singleChoice2.ConnectedSlideLists.Add(slideConnection2);

        Slide singleChoice3 = new Slide
        {
            Text = "Waarop wil jij dat de focus wordt gelegd in het nieuwe stadspark?",
            SlideType = SlideType.SingleChoice,
            AnswerList = new List<string>
            {
                "Sportinfrastructuur", "Speeltuin voor kinderen", "Zitbanken en picknickplaatsen",
                "Ruimte voor kleine evenementen", "Drank- en eetmogelijkheden"
            },
            ConnectedSlideLists = new List<SlideConnection>()
        };

        SlideConnection slideConnection3 = new SlideConnection
        {
            Slide = singleChoice3,
            SlideList = slideList1,
            SlideOrder = 3
        };
        singleChoice3.ConnectedSlideLists.Add(slideConnection3);

        Slide singleChoice4 = new Slide
        {
            Text = "Hoe sta jij tegenover deze stelling? “Mijn stad moet meer investeren in fietspaden”",
            SlideType = SlideType.SingleChoice,
            AnswerList = new List<string> { "Akkoord", "Niet akkoord" },
            ConnectedSlideLists = new List<SlideConnection>()
        };

        SlideConnection slideConnection11 = new SlideConnection
        {
            Slide = singleChoice4,
            SlideList = slideList1punt2,
            SlideOrder = 1
        };
        singleChoice4.ConnectedSlideLists.Add(slideConnection11);

        Slide singleChoice5 = new Slide
        {
            Text =
                "Om ons allemaal veilig en vlot te verplaatsen, moet er in jouw stad of gemeente vooral meer aandacht gaan naar",
            SlideType = SlideType.SingleChoice,
            AnswerList = new List<string>
            {
                "Verplaatsingen met de fiets", "Verplaatsingen met de auto/moto", "Verplaatsingen te voet",
                "Deelmobiliteit", "Openbaar vervoer"
            },
            ConnectedSlideLists = new List<SlideConnection>()
        };
        Slide singleChoice6 = new Slide
        {
            Text =
                "Wat vind jij van het idee om alle leerlingen van de scholen in onze stad een gratis fiets aan te bieden?",
            SlideType = SlideType.SingleChoice,
            AnswerList = new List<string> { "Goed idee", "Slecht idee" },
            ConnectedSlideLists = new List<SlideConnection>()
        };


        Slide multipleChoice1 = new Slide
        {
            Text = "Wat zou jou helpen om een keuze te maken tussen de verschillende partijen?",
            SlideType = SlideType.MultipleChoice,
            AnswerList = new List<string>
            {
                "Meer lessen op school rond de partijprogramma’s",
                "Activiteiten in mijn jeugdclub, sportclub… rond de verkiezingen",
                "Een bezoek van de partijen aan mijn school, jeugd/sportclub, …",
                "Een gesprek met mijn ouders rond de gemeentepolitiek",
                "Een debat georganiseerd door een jeugdhuis met de verschillende partijen"
            },
            ConnectedSlideLists = new List<SlideConnection>()
        };

        SlideConnection slideConnection7 = new SlideConnection
        {
            Slide = multipleChoice1,
            SlideList = slideList1,
            SlideOrder = 7
        };
        multipleChoice1.ConnectedSlideLists.Add(slideConnection7);

        Slide multipleChoice2 = new Slide
        {
            Text = "Welke sportactiviteit(en) zou jij graag in je eigen stad of gemeente kunnen beoefenen?",
            SlideType = SlideType.MultipleChoice,
            AnswerList = new List<string> { "Tennis", "Hockey", "Padel", "Voetbal", "Fitness" },
            ConnectedSlideLists = new List<SlideConnection>()
        };

        SlideConnection slideConnection6 = new SlideConnection
        {
            Slide = multipleChoice2,
            SlideList = slideList1,
            SlideOrder = 6
        };
        multipleChoice2.ConnectedSlideLists.Add(slideConnection6);

        Slide multipleChoice3 = new Slide
        {
            Text =
                "Aan welke van deze activiteiten zou jij meedoen, om mee te wegen op het beleid van jouw stad of gemeente?",
            SlideType = SlideType.MultipleChoice,
            AnswerList = new List<string>
            {
                "Deelnemen aan gespreksavonden met schepenen en burgemeester",
                "Bijwonen van een gemeenteraad",
                "Deelnemen aan een survey uitgestuurd door de stad of gemeente",
                "Een overleg waarbij ik onderwerpen kan aandragen die voor jongeren belangrijk zijn",
                "Mee brainstormen over oplossingen voor problemen waar jongeren mee worstelen"
            },
            ConnectedSlideLists = new List<SlideConnection>()
        };

        SlideConnection slideConnection5 = new SlideConnection
        {
            Slide = multipleChoice3,
            SlideList = slideList1,
            SlideOrder = 5
        };
        multipleChoice3.ConnectedSlideLists.Add(slideConnection5);

        Slide multipleChoice4 = new Slide
        {
            Text = "Jij gaf aan dat je waarschijnlijk niet zal gaan stemmen. Om welke reden(en) zeg je dit?",
            SlideType = SlideType.MultipleChoice,
            AnswerList = new List<string>
            {
                "Ik heb geen interesse",
                "Ik heb geen tijd om te gaan stemmen",
                "Ik kan niet naar het stemkantoor gaan",
                "Ik denk niet dat mijn stem een verschil zal uitmaken",
                "Ik heb geen idee voor wie ik zou moeten stemmen"
            },
            ConnectedSlideLists = new List<SlideConnection>()
        };
        Slide multipleChoice5 = new Slide
        {
            Text = "Wat zou jou (meer) zin geven om te gaan stemmen?",
            SlideType = SlideType.MultipleChoice,
            AnswerList = new List<string>
            {
                "Kunnen gaan stemmen op een toffere locatie",
                "Online, van thuis uit kunnen stemmen",
                "Betere inhoudelijke voorstellen van de politieke partijen",
                "Meer aandacht voor jeugd in de programma’s van de partijen",
                "Beter weten of mijn stem echt impact heeft"
            },
            ConnectedSlideLists = new List<SlideConnection>()
        };


        Slide rangeQuestion1 = new Slide
        {
            SlideType = SlideType.RangeQuestion,
            Text = "Ben jij van plan om te gaan stemmen bij de aankomende lokale verkiezingen?",
            AnswerList = new List<string>
                { "Zeker niet", "Eerder niet", "Ik weet het nog niet", "Eerder wel", "Zeker wel" },
            ConnectedSlideLists = new List<SlideConnection>()
        };

        SlideConnection slideConnection13 = new SlideConnection
        {
            Slide = rangeQuestion1,
            SlideList = slideList1punt2,
            SlideOrder = 3
        };
        rangeQuestion1.ConnectedSlideLists.Add(slideConnection13);

        Slide rangeQuestion2 = new Slide
        {
            SlideType = SlideType.RangeQuestion,
            Text = "Voel jij je betrokken bij het beleid dat wordt uitgestippeld door je gemeente of stad?",
            AnswerList = new List<string> { "Ik voel me weinig tot niet betrokken", "Ik voel me (zeer) betrokken" },
            ConnectedSlideLists = new List<SlideConnection>()
        };

        Slide rangeQuestion3 = new Slide
        {
            SlideType = SlideType.RangeQuestion,
            Text = "In hoeverre ben jij tevreden met het vrijetijdsaanbod in jouw stad of gemeente?",
            AnswerList = new List<string>
            {
                "Zeer ontevreden", "Ontevreden", "Niet tevreden en niet ontevreden", "Tevreden", "Zeer tevreden"
            },
            ConnectedSlideLists = new List<SlideConnection>()
        };

        Slide rangeQuestion4 = new Slide
        {
            SlideType = SlideType.RangeQuestion,
            Text =
                "In welke mate ben jij het eens met de volgende stelling: “Mijn stad of gemeente doet voldoende om betaalbare huisvesting mogelijk te maken voor iedereen.”",
            AnswerList = new List<string>
                { "Helemaal oneens", "Oneens", "Noch eens, noch oneens", "Eens", "Helemaal eens" },
            ConnectedSlideLists = new List<SlideConnection>()
        };
        SlideConnection slideConnection8 = new SlideConnection
        {
            Slide = rangeQuestion4,
            SlideList = slideList1,
            SlideOrder = 8
        };
        rangeQuestion4.ConnectedSlideLists.Add(slideConnection8);

        Slide rangeQuestion5 = new Slide
        {
            SlideType = SlideType.RangeQuestion,
            Text =
                "In welke mate kun jij je vinden in het voorstel om de straatlichten in onze gemeente te doven tussen 23u en 5u?",
            AnswerList = new List<string> { "Ik sta hier volledig achter", "Ik sta hier helemaal niet achter" },
            ConnectedSlideLists = new List<SlideConnection>()
        };

        SlideConnection slideConnection9 = new SlideConnection
        {
            Slide = rangeQuestion5,
            SlideList = slideList1,
            SlideOrder = 9
        };
        rangeQuestion5.ConnectedSlideLists.Add(slideConnection9);

        // Open Questions
        Slide openQuestion1 = new Slide
        {
            SlideType = SlideType.OpenQuestion,
            Text = "Je bent schepen van onderwijs voor een dag: waar zet je dan vooral op in?",
            ConnectedSlideLists = new List<SlideConnection>()
        };

        SlideConnection slideConnection12 = new SlideConnection
        {
            Slide = openQuestion1,
            SlideList = slideList1punt2,
            SlideOrder = 2
        };
        openQuestion1.ConnectedSlideLists.Add(slideConnection12);

        Slide openQuestion2 = new Slide
        {
            SlideType = SlideType.OpenQuestion,
            Text =
                "Als je één ding mag wensen voor het nieuwe stadspark, wat zou jouw droomstadspark dan zeker bevatten?",
            ConnectedSlideLists = new List<SlideConnection>()
        };

        SlideConnection slideConnection4 = new SlideConnection
        {
            Slide = openQuestion2,
            SlideList = slideList1,
            SlideOrder = 4
        };
        openQuestion2.ConnectedSlideLists.Add(slideConnection4);

        // Info slides
        Slide info1 = new Slide
        {
            Text = "Wat is een gemeenteraad?\n" +
                   "De gemeenteraad is het hoogste orgaan van de gemeente. De gemeenteraad is samengesteld uit de burgemeester en de schepenen, en de gemeenteraadsleden. De gemeenteraad is bevoegd voor alles wat de gemeente aanbelangt. De gemeenteraad is het wetgevend orgaan van de gemeente. De gemeenteraad vergadert minstens tien keer per jaar.",
            SlideType = SlideType.InfoSlide,
            ConnectedSlideLists = new List<SlideConnection>(),
            MediaUrl = "https://storage.googleapis.com/answer-cube-bucket/image_202405161408479679.jpeg"
        };

        SlideConnection slideConnection10 = new SlideConnection
        {
            Slide = info1,
            SlideList = slideList1,
            SlideOrder = 10
        };
        info1.ConnectedSlideLists.Add(slideConnection10);

        Slide info2 = new Slide
        {
            Text = "Wat is een schepen?\n " +
                   "Een schepen is een lid van het college van burgemeester en schepenen. De schepen is een uitvoerend orgaan van de gemeente. De schepen is bevoegd voor een bepaalde tak van het gemeentelijk beleid. De schepen wordt verkozen door de gemeenteraad.",
            SlideType = SlideType.InfoSlide,
            ConnectedSlideLists = new List<SlideConnection>()
        };

        Slide info3 = new Slide
        {
            Text = "Wat is een burgemeester?\n" +
                   "De burgemeester is het hoofd van de gemeente. De burgemeester is de voorzitter van de gemeenteraad en het college van burgemeester en schepenen. De burgemeester is bevoegd voor de openbare orde en veiligheid. De burgemeester wordt verkozen door de gemeenteraad.",
            SlideType = SlideType.InfoSlide,
            ConnectedSlideLists = new List<SlideConnection>()
        };

        SlideConnection slideConnection14 = new SlideConnection
        {
            Slide = info3,
            SlideList = slideList1punt2,
            SlideOrder = 4
        };

        // Add the SingleChoice questions to the context
        context.Slides.AddRange(singleChoice1, singleChoice2, singleChoice3, singleChoice4, singleChoice5,
            singleChoice6);
        //Add the questions to the context
        context.Slides.AddRange(multipleChoice1, multipleChoice2, multipleChoice3, multipleChoice4, multipleChoice5);
        // Add the Range questions to the context
        context.Slides.AddRange(rangeQuestion1, rangeQuestion2, rangeQuestion3, rangeQuestion4, rangeQuestion5);
        // Add the Open questions to the context
        context.Slides.AddRange(openQuestion1, openQuestion2);
        context.Slides.AddRange(info1, info2, info3);

        slideList1.ConnectedSlides.AddRange(new List<SlideConnection>
        {
            slideConnection1, slideConnection2, slideConnection3, slideConnection4, slideConnection5, slideConnection6,
            slideConnection7, slideConnection8, slideConnection9, slideConnection10
        });

        slideList1punt2.ConnectedSlides.AddRange(new List<SlideConnection>
        {
            slideConnection11, slideConnection12, slideConnection13, slideConnection14
        });

        //Add slidelists to the context
        context.SlideLists.AddRange(slideList1, slideList1punt2);

        circularFlow1.SlideLists = new List<SlideList>();
        circularFlow1.SlideLists.Add(slideList1);
        circularFlow1.SlideLists.Add(slideList1punt2);
        context.Flows.Add(circularFlow1);

        Flow linearFlow = new Flow
        {
            Name = "LinearFlow",
            Project = project1,
            CircularFlow = false,
            Description = "This is a flow about actors and their lives"
        };
        linearFlow.SlideLists = new List<SlideList>();
        linearFlow.SlideLists.Add(slideList1);
        linearFlow.SlideLists.Add(slideList1punt2);
        context.Flows.Add(linearFlow);

        Installation installation = new Installation()
        {
            Name = "BIB",
            Location = "Antwerpen",
            Active = false,
            CurrentSlideIndex = 0,
            ActiveSlideListId = 0,
            MaxSlideIndex = 0,
            Organization = organization1
        };

        Session session1 = new Session()
        {
            CubeId = 1,
            Installation = installation,
            StartTime = DateTime.Now.ToUniversalTime(),
            EndTime = DateTime.Now.AddMinutes(10.00).ToUniversalTime()
        };
        session1.Answers = new List<Answer>();

        Session session2 = new Session()
        {
            CubeId = 2,
            Installation = installation,
            StartTime = DateTime.Now.ToUniversalTime(),
            EndTime = DateTime.Now.AddMinutes(10.00).ToUniversalTime()
        };
        session2.Answers = new List<Answer>();

        Session session3 = new Session()
        {
            CubeId = 1,
            Installation = installation,
            StartTime = DateTime.Now.AddDays(-3).ToUniversalTime(),
            EndTime = DateTime.Now.AddMinutes(10.00).ToUniversalTime()
        };
        session3.Answers = new List<Answer>();

        Session session4 = new Session()
        {
            CubeId = 2,
            Installation = installation,
            StartTime = DateTime.Now.AddDays(-2).ToUniversalTime(),
            EndTime = DateTime.Now.AddMinutes(10.00).ToUniversalTime()
        };
        session4.Answers = new List<Answer>();
        
        Session session5 = new Session()
        {
            CubeId = 2,
            Installation = installation,
            StartTime = DateTime.Now.AddDays(-2).AddMinutes(10.00).ToUniversalTime(),
            EndTime = DateTime.Now.AddMinutes(10.00).ToUniversalTime()
        };
        session5.Answers = new List<Answer>();

        Answer answer = new Answer()
        {
            Slide = multipleChoice4,
            Session = session1,
            AnswerText = new List<string> { "Ik heb geen interesse", "Ik heb geen idee voor wie ik zou moeten stemmen" }
        };
        Answer answer1 = new Answer()
        {
            Slide = singleChoice1,
            Session = session1,
            AnswerText = new List<string> { "Natuur en ecologie" }
        };

        Answer answer2 = new Answer()
        {
            Slide = singleChoice2,
            Session = session1,
            AnswerText = new List<string> { "Eens" }
        };

        Answer answer3 = new Answer()
        {
            Slide = singleChoice3,
            Session = session1,
            AnswerText = new List<string> { "Sportinfrastructuur" }
        };

        Answer answer4 = new Answer()
        {
            Slide = singleChoice4,
            Session = session1,
            AnswerText = new List<string> { "Akkoord" }
        };

        Answer answer5 = new Answer()
        {
            Slide = singleChoice5,
            Session = session1,
            AnswerText = new List<string> { "Verplaatsingen met de fiets" }
        };

        Answer answer6 = new Answer()
        {
            Slide = singleChoice6,
            Session = session1,
            AnswerText = new List<string> { "Goed idee" }
        };

        Answer answer7 = new Answer()
        {
            Slide = multipleChoice1,
            Session = session1,
            AnswerText = new List<string> { "Een debat georganiseerd door een jeugdhuis met de verschillende partijen" }
        };

        Answer answer8 = new Answer()
        {
            Slide = multipleChoice2,
            Session = session1,
            AnswerText = new List<string> { "Tennis", "Hockey", "Padel" }
        };

        Answer answer9 = new Answer()
        {
            Slide = multipleChoice3,
            Session = session1,
            AnswerText = new List<string>
            {
                "Bijwonen van een gemeenteraad",
                "Een overleg waarbij ik onderwerpen kan aandragen die voor jongeren belangrijk zijn"
            }
        };

        Answer answer10 = new Answer()
        {
            Slide = multipleChoice4,
            Session = session1,
            AnswerText = new List<string>
            {
                "Ik heb geen interesse", "Ik kan niet naar het stemkantoor gaan",
                "Ik heb geen idee voor wie ik zou moeten stemmen"
            }
        };

        Answer answer11 = new Answer()
        {
            Slide = multipleChoice5,
            Session = session1,
            AnswerText = new List<string>
            {
                "Meer aandacht voor jeugd in de programma’s van de partijen",
                "Beter weten of mijn stem echt impact heeft"
            }
        };

        Answer answer12 = new Answer()
        {
            Slide = rangeQuestion1,
            Session = session2,
            AnswerText = new List<string> { "Eerder wel" }
        };

        Answer answer13 = new Answer()
        {
            Slide = rangeQuestion2,
            Session = session2,
            AnswerText = new List<string> { "Ik voel me (zeer) betrokken" }
        };

        Answer answer14 = new Answer()
        {
            Slide = rangeQuestion3,
            Session = session2,
            AnswerText = new List<string> { "Niet tevreden en niet ontevreden" }
        };

        Answer answer15 = new Answer()
        {
            Slide = rangeQuestion4,
            Session = session2,
            AnswerText = new List<string> { "Eens" }
        };

        Answer answer16 = new Answer()
        {
            Slide = rangeQuestion5,
            Session = session2,
            AnswerText = new List<string> { "Ik sta hier volledig achter" }
        };

        Answer answer17 = new Answer()
        {
            Slide = openQuestion1,
            Session = session3,
            AnswerText = new List<string> { "Meer leraren aannemen voor kleinere klassen" }
        };

        Answer answer18 = new Answer()
        {
            Slide = openQuestion2,
            Session = session3,
            AnswerText = new List<string> { "Een grote speeltuin met avontuurlijke toestellen en een waterpartij" }
        };

        Answer answer19 = new Answer()
        {
            Slide = singleChoice1,
            Session = session4,
            AnswerText = new List<string> { "Onderwijs en kinderopvang" }
        };

        Answer answer20 = new Answer()
        {
            Slide = singleChoice2,
            Session = session4,
            AnswerText = new List<string> { "Eens" }
        };
        
        Answer answer21 = new Answer()
        {
            Slide = singleChoice2,
            Session = session1,
            AnswerText = new List<string> { "Eens" }
        };
        
        // Additional answers for Session 1
        Answer answer22 = new Answer()
        {
            Slide = multipleChoice4,
            Session = session1,
            AnswerText = new List<string> { "Ik heb geen interesse", "Ik kan niet naar het stemkantoor gaan" }
        };
        session1.Answers.Add(answer22);

        Answer answer23 = new Answer()
        {
            Slide = multipleChoice5,
            Session = session1,
            AnswerText = new List<string> { "Kunnen gaan stemmen op een toffere locatie", "Betere inhoudelijke voorstellen van de politieke partijen" }
        };
        session1.Answers.Add(answer23);

// Additional answers for Session 2
        Answer answer24 = new Answer()
        {
            Slide = rangeQuestion1,
            Session = session2,
            AnswerText = new List<string> { "Zeker wel" }
        };
        session2.Answers.Add(answer24);

        Answer answer25 = new Answer()
        {
            Slide = rangeQuestion2,
            Session = session2,
            AnswerText = new List<string> { "Ik voel me (zeer) betrokken" }
        };
        session2.Answers.Add(answer25);

// Additional answers for Session 3
        Answer answer26 = new Answer()
        {
            Slide = openQuestion1,
            Session = session3,
            AnswerText = new List<string> { "Meer leraren aannemen voor kleinere klassen" }
        };
        session3.Answers.Add(answer26);

        Answer answer27 = new Answer()
        {
            Slide = openQuestion2,
            Session = session3,
            AnswerText = new List<string> { "Een grote speeltuin met avontuurlijke toestellen en een waterpartij" }
        };
        session3.Answers.Add(answer27);
        
        // Additional answers for Session 1
        Answer answer28 = new Answer()
        {
            Slide = multipleChoice1,
            Session = session1,
            AnswerText = new List<string> { "Een bezoek van de partijen aan mijn school, jeugd/sportclub, …" }
        };
        session1.Answers.Add(answer28);

        Answer answer29 = new Answer()
        {
            Slide = multipleChoice2,
            Session = session1,
            AnswerText = new List<string> { "Padel" }
        };
        session1.Answers.Add(answer29);

// Additional answers for Session 2
        Answer answer30 = new Answer()
        {
            Slide = rangeQuestion3,
            Session = session2,
            AnswerText = new List<string> { "Niet tevreden en niet ontevreden" }
        };
        session2.Answers.Add(answer30);

        Answer answer31 = new Answer()
        {
            Slide = rangeQuestion4,
            Session = session2,
            AnswerText = new List<string> { "Eens" }
        };
        session2.Answers.Add(answer31);

// Additional answers for Session 3
        Answer answer32 = new Answer()
        {
            Slide = singleChoice1,
            Session = session3,
            AnswerText = new List<string> { "Gezondheidszorg en welzijn" }
        };
        session3.Answers.Add(answer32);

        Answer answer33 = new Answer()
        {
            Slide = singleChoice2,
            Session = session3,
            AnswerText = new List<string> { "Oneens" }
        };
        session3.Answers.Add(answer33);
        // Additional answers for Session 1
        Answer answer34 = new Answer()
        {
            Slide = singleChoice3,
            Session = session1,
            AnswerText = new List<string> { "Sportinfrastructuur" }
        };
        session1.Answers.Add(answer34);

        Answer answer35 = new Answer()
        {
            Slide = singleChoice4,
            Session = session1,
            AnswerText = new List<string> { "Akkoord" }
        };
        session1.Answers.Add(answer35);

// Additional answers for Session 2
        Answer answer36 = new Answer()
        {
            Slide = singleChoice5,
            Session = session2,
            AnswerText = new List<string> { "Verplaatsingen met de fiets" }
        };
        session2.Answers.Add(answer36);

        Answer answer37 = new Answer()
        {
            Slide = singleChoice6,
            Session = session2,
            AnswerText = new List<string> { "Goed idee" }
        };
        session2.Answers.Add(answer37);

// Additional answers for Session 3
        Answer answer38 = new Answer()
        {
            Slide = multipleChoice1,
            Session = session3,
            AnswerText = new List<string> { "Een debat georganiseerd door een jeugdhuis met de verschillende partijen" }
        };
        session3.Answers.Add(answer38);

        Answer answer39 = new Answer()
        {
            Slide = multipleChoice2,
            Session = session3,
            AnswerText = new List<string> { "Tennis" }
        };
        session3.Answers.Add(answer39);
        // Additional answers for Slide 1 (SingleChoice)
        Answer answer40 = new Answer()
        {
            Slide = singleChoice1,
            Session = session1,
            AnswerText = new List<string> { "Vrije tijd, sport, cultuur" }
        };
        session1.Answers.Add(answer40);

        Answer answer41 = new Answer()
        {
            Slide = singleChoice1,
            Session = session2,
            AnswerText = new List<string> { "Onderwijs en kinderopvang" }
        };
        session2.Answers.Add(answer41);

        Answer answer42 = new Answer()
        {
            Slide = singleChoice1,
            Session = session3,
            AnswerText = new List<string> { "Gezondheidszorg en welzijn" }
        };
        session3.Answers.Add(answer42);
        // Additional answers for Slide 1 (SingleChoice)
        Answer answer43 = new Answer()
        {
            Slide = singleChoice1,
            Session = session1,
            AnswerText = new List<string> { "Huisvesting" }
        };
        session1.Answers.Add(answer43);

        Answer answer44 = new Answer()
        {
            Slide = singleChoice1,
            Session = session2,
            AnswerText = new List<string> { "Gezondheidszorg en welzijn" }
        };
        session2.Answers.Add(answer44);

        Answer answer45 = new Answer()
        {
            Slide = singleChoice1,
            Session = session3,
            AnswerText = new List<string> { "Onderwijs en kinderopvang" }
        };
        session3.Answers.Add(answer45);


// Add these answers to the context's Answers collection
        context.Answers.AddRange(answer22, answer23, answer24, answer25, answer26, answer27,
            answer28, answer29, answer30, answer31, answer32,
            answer33, answer34, answer35, answer36, answer37,
            answer38, answer39, answer40, answer41, answer42,
            answer43, answer44, answer45);

        session1.Answers.Add(answer);
        session1.Answers.Add(answer1);
        session1.Answers.Add(answer2);
        session1.Answers.Add(answer3);
        session1.Answers.Add(answer4);
        session1.Answers.Add(answer5);
        session1.Answers.Add(answer6);
        session1.Answers.Add(answer7);
        session1.Answers.Add(answer8);
        session1.Answers.Add(answer9);
        session1.Answers.Add(answer10);
        session1.Answers.Add(answer11);
        session1.Answers.Add(answer21);

        session2.Answers.Add(answer12);
        session2.Answers.Add(answer13);
        session2.Answers.Add(answer14);
        session2.Answers.Add(answer15);
        session2.Answers.Add(answer16);

        session3.Answers.Add(answer17);
        session3.Answers.Add(answer18);
        
        session4.Answers.Add(answer19);
        session4.Answers.Add(answer20);

        context.Sessions.Add(session1);
        context.Sessions.Add(session2);
        context.Sessions.Add(session3);
        context.Sessions.Add(session4);

        context.Answers.AddRange(
            answer, answer1, answer2, answer3, answer4, answer5, answer6, answer7,
            answer8, answer9, answer10, answer11, answer12, answer13, answer14, 
            answer15, answer16, answer17, answer18, answer19, answer20, answer21
        );


        context.Installations.Add(installation);
        context.SaveChanges();
        context.ChangeTracker.Clear();
    }
}