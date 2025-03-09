using BL;
using Domain.CitizenPanel;

namespace UI_CA;

public class ConsoleUi
{
    private readonly PanelManager _manager;

    public ConsoleUi(PanelManager manager)
    {
        _manager = manager;
    }

    public void Start()
    {
        int choice;
        Console.WriteLine("Welcome!");
        do
        {
            Console.Write("what would you like to do?\n" +
                          "===========================\n" +
                          "0) quit\n" +
                          "1) Create a new panel\n" +
                          "2) Create a default test panel\n" +
                          "3) View a panel\n" +
                          "Choice (0-3): ");
            var userChoice = Console.ReadLine();

            if (!int.TryParse(userChoice, out choice))
            {
                Console.WriteLine("please enter a number");
                choice = -1;
            }
            else
            {
                Redirect(choice);
            }
        } while (choice != 0);

        Console.WriteLine("Goodbye!");
    }

    private void Redirect(int? choice)
    {
        switch (choice)
        {
            case 1:
                CreatePanel();
                break;
            case 2:
                CreateDefaultPanel();
                break;
            case 3:
                ViewPanel();
                break;
            case 0:
                // main while loop will end
                break;
            default:
                Console.WriteLine("Invalid choice, please try again.");
                break;
        }
    }

    private void ViewPanel()
    {
        Console.WriteLine("choose a Panel\n" +
                          "=========");
        var panels = _manager.GetAllPanels().ToList();
        var i = 1;
        foreach (var panel in panels)
        {
            Console.WriteLine($"{i++}) {panel.name}");
        }

        Guid guid = Guid.Empty;

        while (guid == Guid.Empty)
        {
            try
            {
                guid = panels[AskInt("Choose a Panel:") - 1].Id;
            }
            catch (ArgumentOutOfRangeException e)
            {
                Console.WriteLine("choose a valid integer number");
            }
        }

        var chosenPanel = _manager.GetPanel(guid);
        Console.WriteLine(chosenPanel.describe());
        Console.WriteLine(chosenPanel.getGuidsPerGroup());
    }

    private void CreateDefaultPanel()
    {
        var distribution =
            new Dictionary<string, Dictionary<string, double>>()
            {
                {
                    "sex", new Dictionary<string, double>()
                    {
                        { "m", 0.4 },
                        { "v", 0.6 }
                    }
                },
                {
                    "leef", new Dictionary<string, double>()
                    {
                        { "20", 0.2 },
                        { "30", 0.6 },
                        { "40", 0.2 }
                    }
                }
            };

        Console.WriteLine("\nCriteria Percentages:");
        foreach (var criteria in distribution)
        {
            Console.WriteLine($"Criteria: {criteria.Key}");
            foreach (var category in criteria.Value)
            {
                Console.WriteLine($"  Category: {category.Key}, Percentage: {category.Value}%");
            }
        }

        _manager.AddPanel("Default Panel", 150, distribution);
    }

    private void CreatePanel()
    {
        var name = AskString("enter a panel name:");
        var size = AskInt("enter a panel size:");
        var criteriaPercentages = new Dictionary<string, Dictionary<string, double>>();

        while (true)
        {
            var criteriaName = AskString("Enter criteria name (or type 'done' to finish):");

            if (criteriaName.ToLower().Equals("done"))
            {
                break;
            }

            criteriaPercentages[criteriaName] = CriteriaMaker(criteriaName);
        }

        Console.WriteLine("\nCriteria Percentages:");
        foreach (var criteria in criteriaPercentages)
        {
            Console.WriteLine($"Criteria: {criteria.Key}");
            foreach (var category in criteria.Value)
            {
                Console.WriteLine($"  Category: {category.Key}, Percentage: {category.Value}%");
            }
        }

        _manager.AddPanel(name, size, criteriaPercentages);
    }


    private string AskString(string name)
    {
        while (true)
        {
            Console.WriteLine(name);
            var answer = Console.ReadLine();

            if (answer != null)
            {
                return answer;
            }

            Console.WriteLine("Please enter a name.");
        }
    }

    private int AskInt(string question)
    {
        int result;
        while (true)
        {
            if (int.TryParse(AskString(question), out result))
            {
                return result;
            }

            Console.WriteLine("Please enter a valid integer.");
        }
    }

    private double AskDouble(string question)
    {
        while (true)
        {
            if (double.TryParse(AskString(question), out var result))
            {
                return result;
            }

            Console.WriteLine("Please enter a valid integer.");
        }
    }


    private Dictionary<string, double> CriteriaMaker(string criteriaName)
    {
        var categoryPercentages = new Dictionary<string, double>();
        double totalPercentage = 0;
        const double tolerance = 0.00001;

        while (true)
        {
            var categoryName = AskString($"Enter category name for '{criteriaName}' (or type 'done' to finish):");

            if (categoryName.ToLower().Equals("done"))
            {
                break;
            }

            double percentage;
            while (true)
            {
                percentage = AskDouble($"Enter percentage for '{categoryName}':") / 100;

                if (percentage < 0)
                {
                    Console.WriteLine("Invalid percentage. Please enter a non-negative number.");
                    continue;
                }

                if (percentage > 1 - totalPercentage + tolerance)
                {
                    Console.WriteLine("total percetage ïs greater than 100%, enter a different number.\n" +
                                      $"prev total: {totalPercentage * 100}\n" +
                                      $"new value: {percentage * 100}\n" +
                                      $"new total{(totalPercentage + percentage) * 100}");
                    continue;
                }

                break;
            }

            categoryPercentages[categoryName] = percentage;
            totalPercentage += percentage;
        }

        return categoryPercentages;
    }
}