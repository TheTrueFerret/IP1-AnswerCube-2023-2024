using AnswerCube.BL.Domain;
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
        yannick.PasswordHash = hasher.HashPassword(yannick, "Student_1234");

        var jarno = new AnswerCubeUser()
        {
            Id = "superUser2",
            FirstName = "jarno",
            LastName = "Fret",
            UserName = "jarno",
            NormalizedUserName = "JARNO",
            Email = "jarno.fret@student.kdg.be",
            NormalizedEmail = "JARNO.FRET@STUDENT.KDG.BE",
            EmailConfirmed = true
        };
        jarno.PasswordHash = hasher.HashPassword(jarno, "Student_1234");

        var alexander = new AnswerCubeUser()
        {
            Id = "superUser3",
            FirstName = "Alexander",
            LastName = "Van Puyenbroeck",
            UserName = "Alexander",
            NormalizedUserName = "ALEXANDER",
            Email = "alexander.vanpuyenbroeck@student.kdg.be",
            NormalizedEmail = "ALEXANDER.VANPUYENBROECK@STUDENT.KDG.BE",
            EmailConfirmed = true
        };
        alexander.PasswordHash = hasher.HashPassword(alexander, "Student_1234");

        var nemo = new AnswerCubeUser()
        {
            Id = "superUser4",
            FirstName = "Nemo",
            LastName = "Van Den Eynde",
            UserName = "Nemo",
            NormalizedUserName = "NEMO",
            Email = "nemo.vandeneynde@student.kdg.be",
            NormalizedEmail = "NEMO.VANDENEYNDE@STUDENT.KDG.BE",
            EmailConfirmed = true
        };
        alexander.PasswordHash = hasher.HashPassword(alexander, "Student_1234");

        var ilyasse = new AnswerCubeUser()
        {
            Id = "superUser5",
            FirstName = "Ilyasse",
            LastName = "jmida",
            UserName = "Ilyasse",
            NormalizedUserName = "ILYASSE",
            Email = "ilyasse.jmida@student.kdg.be",
            NormalizedEmail = "ILYASSE.JMIDA@STUDENT.KDG.BE",
            EmailConfirmed = true
        };
        alexander.PasswordHash = hasher.HashPassword(alexander, "Student_1234");

        context.AnswerCubeUsers.AddRange(alexander, yannick, jarno, nemo, ilyasse);

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
                }
            };

            Slide singleChoice2 = new Slide
            {
                Text =
                    "Er moet meer geïnvesteerd worden in overdekte fietsstallingen aan de bushaltes in onze gemeente.",
                SlideType = SlideType.SingleChoice,
                AnswerList = new List<string> { "Eens", "Oneens" }
            };

            Slide singleChoice3 = new Slide
            {
                Text = "Waarop wil jij dat de focus wordt gelegd in het nieuwe stadspark?",
                SlideType = SlideType.SingleChoice,
                AnswerList = new List<string>
                {
                    "Sportinfrastructuur", "Speeltuin voor kinderen", "Zitbanken en picknickplaatsen",
                    "Ruimte voor kleine evenementen", "Drank- en eetmogelijkheden"
                }
            };

            Slide singleChoice4 = new Slide
            {
                Text = "Hoe sta jij tegenover deze stelling? “Mijn stad moet meer investeren in fietspaden”",
                SlideType = SlideType.SingleChoice,
                AnswerList = new List<string> { "Akkoord", "Niet akkoord" }
            };

            Slide singleChoice5 = new Slide
            {
                Text =
                    "Om ons allemaal veilig en vlot te verplaatsen, moet er in jouw stad of gemeente vooral meer aandacht gaan naar",
                SlideType = SlideType.SingleChoice,
                AnswerList = new List<string>
                {
                    "Verplaatsingen met de fiets", "Verplaatsingen met de auto/moto", "Verplaatsingen te voet",
                    "Deelmobiliteit", "Openbaar vervoer"
                }
            };

            Slide singleChoice6 = new Slide
            {
                Text =
                    "Wat vind jij van het idee om alle leerlingen van de scholen in onze stad een gratis fiets aan te bieden?",
                SlideType = SlideType.SingleChoice,
                AnswerList = new List<string> { "Goed idee", "Slecht idee" }
            };

            // Add the SingleChoice questions to the context
            context.Slides.AddRange(singleChoice1, singleChoice2, singleChoice3, singleChoice4, singleChoice5,
                singleChoice6);

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
                }
            };

            Slide multipleChoice2 = new Slide
            {
                Text = "Welke sportactiviteit(en) zou jij graag in je eigen stad of gemeente kunnen beoefenen?",
                SlideType = SlideType.MultipleChoice,
                AnswerList = new List<string> { "Tennis", "Hockey", "Padel", "Voetbal", "Fitness" }
            };

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
                }
            };

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
                }
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
                }
            };


            //Add the questions to the context
            context.Slides.AddRange(multipleChoice1, multipleChoice2, multipleChoice3, multipleChoice4,
                multipleChoice5);

            Slide rangeQuestion1 = new Slide
            {
                SlideType = SlideType.RangeQuestion,
                Text = "Ben jij van plan om te gaan stemmen bij de aankomende lokale verkiezingen?",
                AnswerList = new List<string>
                    { "Zeker niet", "Eerder niet", "Ik weet het nog niet", "Eerder wel", "Zeker wel" },
            };

            Slide rangeQuestion2 = new Slide
            {
                SlideType = SlideType.RangeQuestion,
                Text = "Voel jij je betrokken bij het beleid dat wordt uitgestippeld door je gemeente of stad?",
                AnswerList = new List<string> { "Ik voel me weinig tot niet betrokken", "Ik voel me (zeer) betrokken" },
            };

            Slide rangeQuestion3 = new Slide
            {
                SlideType = SlideType.RangeQuestion,
                Text = "In hoeverre ben jij tevreden met het vrijetijdsaanbod in jouw stad of gemeente?",
                AnswerList = new List<string>
                {
                    "Zeer ontevreden", "Ontevreden", "Niet tevreden en niet ontevreden", "Tevreden", "Zeer tevreden"
                },
            };

            Slide rangeQuestion4 = new Slide
            {
                SlideType = SlideType.RangeQuestion,
                Text =
                    "In welke mate ben jij het eens met de volgende stelling: “Mijn stad of gemeente doet voldoende om betaalbare huisvesting mogelijk te maken voor iedereen.”",
                AnswerList = new List<string>
                    { "Helemaal oneens", "Oneens", "Noch eens, noch oneens", "Eens", "Helemaal eens" },
            };

            Slide rangeQuestion5 = new Slide
            {
                SlideType = SlideType.RangeQuestion,
                Text =
                    "In welke mate kun jij je vinden in het voorstel om de straatlichten in onze gemeente te doven tussen 23u en 5u?",
                AnswerList = new List<string> { "Ik sta hier volledig achter", "Ik sta hier helemaal niet achter" },
            };

            // Add the Range questions to the context
            context.Slides.AddRange(rangeQuestion1, rangeQuestion2, rangeQuestion3, rangeQuestion4, rangeQuestion5);

            // Open Questions
            Slide openQuestion1 = new Slide
            {
                SlideType = SlideType.OpenQuestion,
                Text = "Je bent schepen van onderwijs voor een dag: waar zet je dan vooral op in?",

            };

            Slide openQuestion2 = new Slide
            {
                SlideType = SlideType.OpenQuestion,
                Text =
                    "Als je één ding mag wensen voor het nieuwe stadspark, wat zou jouw droomstadspark dan zeker bevatten?"
            };

            // Add the Open questions to the context
            context.Slides.AddRange(openQuestion1, openQuestion2);

            // Info slides
            Slide info1 = new Slide
            {
                Text = "Wat is een gemeenteraad?\n" +
                       "De gemeenteraad is het hoogste orgaan van de gemeente. De gemeenteraad is samengesteld uit de burgemeester en de schepenen, en de gemeenteraadsleden. De gemeenteraad is bevoegd voor alles wat de gemeente aanbelangt. De gemeenteraad is het wetgevend orgaan van de gemeente. De gemeenteraad vergadert minstens tien keer per jaar.",
                SlideType = SlideType.InfoSlide
            };

            Slide info2 = new Slide
            {
                Text = "Wat is een schepen?\n " +
                       "Een schepen is een lid van het college van burgemeester en schepenen. De schepen is een uitvoerend orgaan van de gemeente. De schepen is bevoegd voor een bepaalde tak van het gemeentelijk beleid. De schepen wordt verkozen door de gemeenteraad.",
                SlideType = SlideType.InfoSlide 
            };

            Slide info3 = new Slide
            {
                Text = "Wat is een burgemeester?\n" +
                       "De burgemeester is het hoofd van de gemeente. De burgemeester is de voorzitter van de gemeenteraad en het college van burgemeester en schepenen. De burgemeester is bevoegd voor de openbare orde en veiligheid. De burgemeester wordt verkozen door de gemeenteraad.",
                SlideType = SlideType.InfoSlide
            };

            context.Slides.AddRange(info1, info2, info3);


            Slide startSlide1 = new Slide()
            {
                Text = "Welkom bij het thema ...",
                SlideType = SlideType.StartSlide
            };
            context.Slides.Add(startSlide1);
            

            SlideList slideList1 = new SlideList
            {
                Title = "testlist1",
                SubTheme = new SubTheme("openbaar vervoer", "ipsum lorum"),
                Slides = new LinkedList<Slide>()
            };
            slideList1.Slides.AddFirst(singleChoice1);
            slideList1.Slides.AddFirst(singleChoice2);
            slideList1.Slides.AddFirst(singleChoice3);
            slideList1.Slides.AddFirst(openQuestion2);
            slideList1.Slides.AddFirst(multipleChoice3);
            slideList1.Slides.AddFirst(multipleChoice2);
            slideList1.Slides.AddFirst(multipleChoice1);
            slideList1.Slides.AddFirst(rangeQuestion4);
            slideList1.Slides.AddFirst(rangeQuestion5);
            slideList1.Slides.AddFirst(info1);

            context.SlideLists.Add(slideList1);

            Flow linearFlow = new Flow
            {
                Name = "linear",
                CircularFlow = false
            };
            linearFlow.SlideList = new List<SlideList>();
            linearFlow.SlideList.Add(slideList1);
            context.Flows.Add(linearFlow);

            context.SaveChanges();

            SlideList slideList2 = new SlideList
            {
                Title = "testlist2",
                SubTheme = new SubTheme("een park", "ipsum lorum")
            };

            slideList2.Slides = new LinkedList<Slide>();
            slideList2.Slides.AddFirst(openQuestion1); // 7
            slideList2.Slides.AddFirst(singleChoice4); // 8
            slideList2.Slides.AddFirst(singleChoice5); // 1
            slideList2.Slides.AddFirst(rangeQuestion1); // 4
            slideList2.Slides.AddFirst(rangeQuestion2); // 5
            slideList2.Slides.AddFirst(rangeQuestion3); // 6
            slideList2.Slides.AddFirst(multipleChoice4); // 2
            slideList2.Slides.AddFirst(multipleChoice5); // 3
            context.SlideLists.Add(slideList2);


            Flow circularFlow = new Flow
            {
                Name = "circular",
                CircularFlow = true
            };
            circularFlow.SlideList = new List<SlideList>();
            circularFlow.SlideList.Add(slideList2);

            context.Flows.Add(circularFlow);


            Installation installation = new Installation()
            {
                Location = "Antwerpen",
                Active = false,
                CurrentSlideIndex = 0,
                ActiveSlideListId = 0,
                MaxSlideIndex = 0,
            };
            context.Installations.Add(installation);
            

            context.SaveChanges();
            context.ChangeTracker.Clear();
        }
    }
