using AnswerCube.BL.Domain;
using Domain;

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
        //SingleChoice
        ListQuestion singleChoice1 = new ListQuestion
        {
            Text =
                "Als jij de begroting van je stad of gemeente zou opmaken, waar zou je dan in de komende jaren vooral op inzetten?",
            IsMultipleChoice = false,
            AnswerList = new List<string>
            {
                "Natuur en ecologie", "Vrije tijd, sport, cultuur", "Huisvesting", "Onderwijs en kinderopvang",
                "Gezondheidszorg en welzijn", "Verkeersveiligheid en mobiliteit", "Ondersteunen van lokale handel"
            }
        };
        ListQuestion singleChoice2 = new ListQuestion
        {
            Text =
                "Er moet meer geïnvesteerd worden in overdekte fietsstallingen aan de bushaltes in onze gemeente.",
            IsMultipleChoice = false,
            AnswerList = new List<string> { "Eens", "Oneens" }
        };

        ListQuestion singleChoice3 = new ListQuestion
        {
            Text = "Waarop wil jij dat de focus wordt gelegd in het nieuwe stadspark?",
            IsMultipleChoice = false,
            AnswerList = new List<string>
            {
                "Sportinfrastructuur", "Speeltuin voor kinderen", "Zitbanken en picknickplaatsen",
                "Ruimte voor kleine evenementen", "Drank- en eetmogelijkheden"
            }
        };

        ListQuestion singleChoice4 = new ListQuestion
        {
            Text = "Hoe sta jij tegenover deze stelling? “Mijn stad moet meer investeren in fietspaden”",
            IsMultipleChoice = false,
            AnswerList = new List<string> { "Akkoord", "Niet akkoord" }
        };

        ListQuestion singleChoice5 = new ListQuestion
        {
            Text =
                "Om ons allemaal veilig en vlot te verplaatsen, moet er in jouw stad of gemeente vooral meer aandacht gaan naar",
            IsMultipleChoice = false,
            AnswerList = new List<string>
            {
                "Verplaatsingen met de fiets", "Verplaatsingen met de auto/moto", "Verplaatsingen te voet",
                "Deelmobiliteit", "Openbaar vervoer"
            }
        };

        ListQuestion singleChoice6 = new ListQuestion
        {
            Text =
                "Wat vind jij van het idee om alle leerlingen van de scholen in onze stad een gratis fiets aan te bieden?",
            IsMultipleChoice = false,
            AnswerList = new List<string> { "Goed idee", "Slecht idee" }
        };

        // Add the SingleChoice questions to the context
        context.ListQuestions.AddRange(singleChoice1, singleChoice2, singleChoice3, singleChoice4, singleChoice5,
            singleChoice6);

        ListQuestion multipleChoice1 = new ListQuestion
        {
            Text = "Wat zou jou helpen om een keuze te maken tussen de verschillende partijen?",
            IsMultipleChoice = true,
            AnswerList = new List<string>
            {
                "Meer lessen op school rond de partijprogramma’s",
                "Activiteiten in mijn jeugdclub, sportclub… rond de verkiezingen",
                "Een bezoek van de partijen aan mijn school, jeugd/sportclub, …",
                "Een gesprek met mijn ouders rond de gemeentepolitiek",
                "Een debat georganiseerd door een jeugdhuis met de verschillende partijen"
            }
        };

        ListQuestion multipleChoice2 = new ListQuestion
        {
            Text = "Welke sportactiviteit(en) zou jij graag in je eigen stad of gemeente kunnen beoefenen?",
            IsMultipleChoice = true,
            AnswerList = new List<string> { "Tennis", "Hockey", "Padel", "Voetbal", "Fitness" }
        };

        ListQuestion multipleChoice3 = new ListQuestion
        {
            Text =
                "Aan welke van deze activiteiten zou jij meedoen, om mee te wegen op het beleid van jouw stad of gemeente?",
            IsMultipleChoice = true,
            AnswerList = new List<string>
            {
                "Deelnemen aan gespreksavonden met schepenen en burgemeester",
                "Bijwonen van een gemeenteraad",
                "Deelnemen aan een survey uitgestuurd door de stad of gemeente",
                "Een overleg waarbij ik onderwerpen kan aandragen die voor jongeren belangrijk zijn",
                "Mee brainstormen over oplossingen voor problemen waar jongeren mee worstelen"
            }
        };

        ListQuestion multipleChoice4 = new ListQuestion
        {
            Text = "Jij gaf aan dat je waarschijnlijk niet zal gaan stemmen. Om welke reden(en) zeg je dit?",
            IsMultipleChoice = true,
            AnswerList = new List<string>
            {
                "Ik heb geen interesse",
                "Ik heb geen tijd om te gaan stemmen",
                "Ik kan niet naar het stemkantoor gaan",
                "Ik denk niet dat mijn stem een verschil zal uitmaken",
                "Ik heb geen idee voor wie ik zou moeten stemmen"
            }
        };

        ListQuestion multipleChoice5 = new ListQuestion
        {
            Text = "Wat zou jou (meer) zin geven om te gaan stemmen?",
            IsMultipleChoice = true,
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
        context.ListQuestions.AddRange(multipleChoice1, multipleChoice2, multipleChoice3, multipleChoice4,
            multipleChoice5);

        ListQuestion rangeQuestion1 = new ListQuestion
        {
            Text = "Ben jij van plan om te gaan stemmen bij de aankomende lokale verkiezingen?",
            AnswerList = new List<string>
                { "Zeker niet", "Eerder niet", "Ik weet het nog niet", "Eerder wel", "Zeker wel" },
        };

        ListQuestion rangeQuestion2 = new ListQuestion
        {
            Text = "Voel jij je betrokken bij het beleid dat wordt uitgestippeld door je gemeente of stad?",
            AnswerList = new List<string> { "Ik voel me weinig tot niet betrokken", "Ik voel me (zeer) betrokken" },
        };

        ListQuestion rangeQuestion3 = new ListQuestion
        {
            Text = "In hoeverre ben jij tevreden met het vrijetijdsaanbod in jouw stad of gemeente?",
            AnswerList = new List<string>
                { "Zeer ontevreden", "Ontevreden", "Niet tevreden en niet ontevreden", "Tevreden", "Zeer tevreden" },
        };

        ListQuestion rangeQuestion4 = new ListQuestion
        {
            Text =
                "In welke mate ben jij het eens met de volgende stelling: “Mijn stad of gemeente doet voldoende om betaalbare huisvesting mogelijk te maken voor iedereen.”",
            AnswerList = new List<string>
                { "Helemaal oneens", "Oneens", "Noch eens, noch oneens", "Eens", "Helemaal eens" },
        };

        ListQuestion rangeQuestion5 = new ListQuestion
        {
            Text =
                "In welke mate kun jij je vinden in het voorstel om de straatlichten in onze gemeente te doven tussen 23u en 5u?",
            AnswerList = new List<string> { "Ik sta hier volledig achter", "Ik sta hier helemaal niet achter" },
        };

        // Add the Range questions to the context
        context.ListQuestions.AddRange(rangeQuestion1, rangeQuestion2, rangeQuestion3, rangeQuestion4, rangeQuestion5);

        // Open Questions
        OpenQuestion openQuestion1 = new OpenQuestion
        {
            Text = "Je bent schepen van onderwijs voor een dag: waar zet je dan vooral op in?",
            Answer = "none"
        };

        OpenQuestion openQuestion2 = new OpenQuestion
        {
            Text =
                "Als je één ding mag wensen voor het nieuwe stadspark, wat zou jouw droomstadspark dan zeker bevatten?",
            Answer = "none"
        };

        // Add the Open questions to the context
        context.OpenQuestions.AddRange(openQuestion1, openQuestion2);
        
        // Info slides
        Info info1 = new Info
        {
            Question = "Wat is een gemeenteraad?",
            Text = 
                "De gemeenteraad is het hoogste orgaan van de gemeente. De gemeenteraad is samengesteld uit de burgemeester en de schepenen, en de gemeenteraadsleden. De gemeenteraad is bevoegd voor alles wat de gemeente aanbelangt. De gemeenteraad is het wetgevend orgaan van de gemeente. De gemeenteraad vergadert minstens tien keer per jaar."
        };
        
        Info info2 = new Info
        {
            Question = "Wat is een schepen?",
            Text =
                "Een schepen is een lid van het college van burgemeester en schepenen. De schepen is een uitvoerend orgaan van de gemeente. De schepen is bevoegd voor een bepaalde tak van het gemeentelijk beleid. De schepen wordt verkozen door de gemeenteraad."
        };
        
        Info info3 = new Info
        {
            Question = "Wat is een burgemeester?",
            Text =
                "De burgemeester is het hoofd van de gemeente. De burgemeester is de voorzitter van de gemeenteraad en het college van burgemeester en schepenen. De burgemeester is bevoegd voor de openbare orde en veiligheid. De burgemeester wordt verkozen door de gemeenteraad."
        };
        context.InfoSlide.AddRange(info1, info2, info3);

        
        SlideList slideList1 = new SlideList
        { 
            Title = "testlist1", 
            SubTheme = new SubTheme("openbaar vervoer", "ipsum lorum")
        };
         
        slideList1.Slides = new LinkedList<Slide>();
        slideList1.Slides.AddFirst(multipleChoice1);
        slideList1.Slides.AddLast(multipleChoice2);
        slideList1.Slides.AddLast(multipleChoice3);
        slideList1.Slides.AddLast(openQuestion2);
        slideList1.Slides.AddLast(singleChoice1);
        context.SlideLists.Add(slideList1);
        
        
        SlideList slideList2 = new SlideList
        { 
            Title = "testlist2", 
            SubTheme = new SubTheme("een park", "ipsum lorum")
        };
        slideList1.Slides = new LinkedList<Slide>();
        slideList1.Slides.AddFirst(rangeQuestion1);
        slideList1.Slides.AddFirst(rangeQuestion2);
        slideList1.Slides.AddFirst(rangeQuestion3);
        slideList1.Slides.AddFirst(multipleChoice4);
        slideList1.Slides.AddFirst(multipleChoice5);
        context.SlideLists.Add(slideList2);
        
        
        LinearFlow linearFlow = new LinearFlow
        {
            Name = "linear"
        };
        linearFlow.SlideList = new List<SlideList>();
        linearFlow.SlideList.Add(slideList1);
        linearFlow.SlideList.Add(slideList2);
        
        
        CircularFlow circularFlow = new CircularFlow
        {
            Name = "circular"
        };
        circularFlow.SlideList = new List<SlideList>();
        circularFlow.SlideList.Add(slideList1);
        circularFlow.SlideList.Add(slideList2);
        
        
        context.LinearFlows.Add(linearFlow);
        context.CircularFlows.Add(circularFlow);
        
        context.SaveChanges();
        context.ChangeTracker.Clear();
    }
}