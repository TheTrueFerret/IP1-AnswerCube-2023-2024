using Domain;

namespace AnswerCube.DAL.EF;

public static class AnswerCubeInitializer
{
    private static bool _hasRunDuringAppExcecution = false;

    public static void Initialize(AnswerCubeDbContext context, bool dropCreateDatabase = false)
    {
        if (!_hasRunDuringAppExcecution)
        {
            if (dropCreateDatabase)
            {
                context.Database.EnsureDeleted();
            }

            if (context.Database.EnsureCreated())
            {
                Seed(context);
            }
        }

        _hasRunDuringAppExcecution = true;
    }

    private static void Seed(AnswerCubeDbContext context)
    {
        //SingleChoice
        List_Question singleChoice1 = new List_Question
        {
            Id = 1,
            Name =
                "Als jij de begroting van je stad of gemeente zou opmaken, waar zou je dan in de komende jaren vooral op inzetten?",
            IsMultipleChoice = false,
            QuestionList = new List<string>
            {
                "Natuur en ecologie", "Vrije tijd, sport, cultuur", "Huisvesting", "Onderwijs en kinderopvang",
                "Gezondheidszorg en welzijn", "Verkeersveiligheid en mobiliteit", "Ondersteunen van lokale handel"
            },
            AnswerList = new List<string> { "none" }
        };
        List_Question singleChoice2 = new List_Question
        {
            Id = 2,
            Name = "Er moet meer geïnvesteerd worden in overdekte fietsstallingen aan de bushaltes in onze gemeente.",
            IsMultipleChoice = false,
            QuestionList = new List<string> { "Eens", "Oneens" },
            AnswerList = new List<string> { "none" }
        };

        List_Question singleChoice3 = new List_Question
        {
            Id = 3,
            Name = "Waarop wil jij dat de focus wordt gelegd in het nieuwe stadspark?",
            IsMultipleChoice = false,
            QuestionList = new List<string>
            {
                "Sportinfrastructuur", "Speeltuin voor kinderen", "Zitbanken en picknickplaatsen",
                "Ruimte voor kleine evenementen", "Drank- en eetmogelijkheden"
            },
            AnswerList = new List<string> { "none" }
        };

        List_Question singleChoice4 = new List_Question
        {
            Id = 4,
            Name = "Hoe sta jij tegenover deze stelling? “Mijn stad moet meer investeren in fietspaden”",
            IsMultipleChoice = false,
            QuestionList = new List<string> { "Akkoord", "Niet akkoord" },
            AnswerList = new List<string> { "none" }
        };

        List_Question singleChoice5 = new List_Question
        {
            Id = 5,
            Name = "Om ons allemaal veilig en vlot te verplaatsen, moet er in jouw stad of gemeente vooral meer aandacht gaan naar",
            IsMultipleChoice = false,
            QuestionList = new List<string>
            {
                "Verplaatsingen met de fiets", "Verplaatsingen met de auto/moto", "Verplaatsingen te voet",
                "Deelmobiliteit", "Openbaar vervoer"
            },
            AnswerList = new List<string> { "none" }
        };

        List_Question singleChoice6 = new List_Question
        {
            Id = 6,
            Name = "Wat vind jij van het idee om alle leerlingen van de scholen in onze stad een gratis fiets aan te bieden?",
            IsMultipleChoice = false,
            QuestionList = new List<string> { "Goed idee", "Slecht idee" },
            AnswerList = new List<string> { "none" }
        };

        // Add the SingleChoice questions to the context
        context.ListQuestions.AddRange(singleChoice1, singleChoice2, singleChoice3, singleChoice4, singleChoice5,
            singleChoice6);

List_Question multipleChoice1 = new List_Question
{
    Id = 7,
    Name = "Wat zou jou helpen om een keuze te maken tussen de verschillende partijen?",
    IsMultipleChoice = true,
    QuestionList = new List<string>
    {
        "Meer lessen op school rond de partijprogramma’s",
        "Activiteiten in mijn jeugdclub, sportclub… rond de verkiezingen",
        "Een bezoek van de partijen aan mijn school, jeugd/sportclub, …",
        "Een gesprek met mijn ouders rond de gemeentepolitiek",
        "Een debat georganiseerd door een jeugdhuis met de verschillende partijen"
    },
    AnswerList = new List<string> { "none" }
};

List_Question multipleChoice2 = new List_Question
{
    Id = 8,
    Name = "Welke sportactiviteit(en) zou jij graag in je eigen stad of gemeente kunnen beoefenen?",
    IsMultipleChoice = true,
    QuestionList = new List<string> { "Tennis", "Hockey", "Padel", "Voetbal", "Fitness" },
    AnswerList = new List<string> { "none" }
};

List_Question multipleChoice3 = new List_Question
{
    Id = 9,
    Name = "Aan welke van deze activiteiten zou jij meedoen, om mee te wegen op het beleid van jouw stad of gemeente?",
    IsMultipleChoice = true,
    QuestionList = new List<string>
    {
        "Deelnemen aan gespreksavonden met schepenen en burgemeester",
        "Bijwonen van een gemeenteraad",
        "Deelnemen aan een survey uitgestuurd door de stad of gemeente",
        "Een overleg waarbij ik onderwerpen kan aandragen die voor jongeren belangrijk zijn",
        "Mee brainstormen over oplossingen voor problemen waar jongeren mee worstelen"
    },
    AnswerList = new List<string> { "none" }
};

List_Question multipleChoice4 = new List_Question
{
    Id = 10,
    Name = "Jij gaf aan dat je waarschijnlijk niet zal gaan stemmen. Om welke reden(en) zeg je dit?",
    IsMultipleChoice = true,
    QuestionList = new List<string>
    {
        "Ik heb geen interesse",
        "Ik heb geen tijd om te gaan stemmen",
        "Ik kan niet naar het stemkantoor gaan",
        "Ik denk niet dat mijn stem een verschil zal uitmaken",
        "Ik heb geen idee voor wie ik zou moeten stemmen"
    },
    AnswerList = new List<string> { "none" }
};

List_Question multipleChoice5 = new List_Question
{
    Id = 11,
    Name = "Wat zou jou (meer) zin geven om te gaan stemmen?",
    IsMultipleChoice = true,
    QuestionList = new List<string>
    {
        "Kunnen gaan stemmen op een toffere locatie",
        "Online, van thuis uit kunnen stemmen",
        "Betere inhoudelijke voorstellen van de politieke partijen",
        "Meer aandacht voor jeugd in de programma’s van de partijen",
        "Beter weten of mijn stem echt impact heeft"
    },
    AnswerList = new List<string> { "none" }
};
        

        // Add the questions to the context
        context.ListQuestions.AddRange(multipleChoice1, multipleChoice2, multipleChoice3, multipleChoice4,
            multipleChoice5);

        Range_Question rangeQuestion1 = new Range_Question
        {
            Id = 12,
            Name = "Ben jij van plan om te gaan stemmen bij de aankomende lokale verkiezingen?",
            RangeList = new List<string> { "Zeker niet", "Eerder niet", "Ik weet het nog niet", "Eerder wel", "Zeker wel" },
            RangeValues = new List<int> { 1, 2, 3, 4, 5 }
        };

        Range_Question rangeQuestion2 = new Range_Question
        {
            Id = 13,
            Name = "Voel jij je betrokken bij het beleid dat wordt uitgestippeld door je gemeente of stad?",
            RangeList = new List<string> { "Ik voel me weinig tot niet betrokken", "Ik voel me (zeer) betrokken" },
            RangeValues = new List<int> { 1, 5 }
        };

        Range_Question rangeQuestion3 = new Range_Question
        {
            Id = 14,
            Name = "In hoeverre ben jij tevreden met het vrijetijdsaanbod in jouw stad of gemeente?",
            RangeList = new List<string> { "Zeer ontevreden", "Ontevreden", "Niet tevreden en niet ontevreden", "Tevreden", "Zeer tevreden" },
            RangeValues = new List<int> { 1, 2, 3, 4, 5 }
        };

        Range_Question rangeQuestion4 = new Range_Question
        {
            Id = 15,
            Name = "In welke mate ben jij het eens met de volgende stelling: “Mijn stad of gemeente doet voldoende om betaalbare huisvesting mogelijk te maken voor iedereen.”",
            RangeList = new List<string> { "Helemaal oneens", "Oneens", "Noch eens, noch oneens", "Eens", "Helemaal eens" },
            RangeValues = new List<int> { 1, 2, 3, 4, 5 }
        };

        Range_Question rangeQuestion5 = new Range_Question
        {
            Id = 16,
            Name = "In welke mate kun jij je vinden in het voorstel om de straatlichten in onze gemeente te doven tussen 23u en 5u?",
            RangeList = new List<string> { "Ik sta hier volledig achter", "Ik sta hier helemaal niet achter" },
            RangeValues = new List<int> { 1, 5 }
        };

        // Add the Range questions to the context
        context.RangeQuestions.AddRange(rangeQuestion1, rangeQuestion2, rangeQuestion3, rangeQuestion4, rangeQuestion5);

        // Open Questions
        Open_Question openQuestion1 = new Open_Question{
            Id=17,
            Name = "Je bent schepen van onderwijs voor een dag: waar zet je dan vooral op in?",
            Answer = "none"};

        Open_Question openQuestion2 = new Open_Question{
            Id=18,
            Name ="Als je één ding mag wensen voor het nieuwe stadspark, wat zou jouw droomstadspark dan zeker bevatten?",
            Answer ="none"};

        // Add the Open questions to the context
        context.OpenQuestions.AddRange(openQuestion1, openQuestion2);

        context.SaveChanges();
        context.ChangeTracker.Clear();
    }
}