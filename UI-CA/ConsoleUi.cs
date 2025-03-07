using BL;

namespace UI_CA;

public class ConsoleUi
{
    private readonly IManager _manager;

    public ConsoleUi(IManager manager)
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
                          "Choice (0-1): ");
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
            case 0:
                // main while loop will end
                break;
            default:
                Console.WriteLine("Invalid choice, please try again.");
                break;
        }
    }

    private void CreatePanel()
    {
        var criteriaPercentages = new Dictionary<string, Dictionary<string, double>>();
        var TOLERANCE = 0.001;

        while (true)
        {
            Console.WriteLine("Enter criteria name (or type 'done' to finish):");
            string criteriaName = Console.ReadLine();

            if (criteriaName.ToLower() == "done")
            {
                break;
            }

            var categoryPercentages = new Dictionary<string, double>();
            double totalPercentage = 0;

            while (true)
            {
                Console.WriteLine($"Enter category name for '{criteriaName}' (or type 'done' to finish):");
                var categoryName = Console.ReadLine();

                if (categoryName.ToLower() == "done")
                {
                    break;
                }

                double percentage;
                while (true)
                {
                    Console.WriteLine($"Enter percentage for '{categoryName}':");
                    if (double.TryParse(Console.ReadLine(), out percentage) && percentage >= 0)
                    {
                        break;
                    }
                    else
                    {
                        Console.WriteLine("Invalid percentage. Please enter a non-negative number.");
                    }
                }

                categoryPercentages[categoryName] = percentage;
                totalPercentage += percentage;
            }
            
            if (Math.Abs(totalPercentage - 100) > TOLERANCE)
            {
                Console.WriteLine($"Warning: Total percentage for '{criteriaName}' is {totalPercentage}%, not 100%.");
            }

            criteriaPercentages[criteriaName] = categoryPercentages;
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

        //Example of accessing the dictionary
        if(criteriaPercentages.ContainsKey("ExampleCriteria"))
        {
            if(criteriaPercentages["ExampleCriteria"].ContainsKey("ExampleCategory"))
            {
                Console.WriteLine($"\nExampleCriteria/ExampleCategory percentage: {criteriaPercentages["ExampleCriteria"]["ExampleCategory"]}");
            }
        }
    }
    
}