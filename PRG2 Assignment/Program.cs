// See https://aka.ms/new-console-template for more information

using PRG2_Assignment;
using System.Globalization;

// Initialisation
Queue<Order> regularQueue = new();
Queue<Order> goldQueue = new();

Dictionary<int, Customer> customerDict = new();
InitializeCustomerDict();

List<List<string>> flavourList = new();
using (StreamReader sr = new("flavours.csv"))
{
    string headings = sr.ReadLine();
    string[] lines = sr.ReadToEnd().Split("\n");
    foreach(string line in lines)
    {
        flavourList.Add(new List<string> { line.Split(",")[0], line.Split(",")[1] });
    }
}

List<List<string>> waffleFlavourList = new();
using (StreamReader sr = new("waffleflavours.csv"))
{
    string headings = sr.ReadLine();
    string[] lines = sr.ReadToEnd().Split("\n");
    foreach (string line in lines)
    {
        waffleFlavourList.Add(new List<string> { line.Split(",")[0], line.Split(",")[1] });
    }
}

List<List<string>> toppingList = new();
using (StreamReader sr = new("toppings.csv"))
{
    string headings = sr.ReadLine();
    string[] lines = sr.ReadToEnd().Split("\n");
    foreach(string line in lines)
    {
        toppingList.Add(new List<string> { line.Split(",")[0], line.Split(",")[1] });
    }
}

List<List<string>> optionList = new();
using (StreamReader sr = new("options.csv"))
{
    string headings = sr.ReadLine();
    string[] lines = sr.ReadToEnd().Split("\n");
    foreach(string line in lines)
    {
        optionList.Add(new List<string> { line.Split(',')[0], line.Split(",")[1], line.Split(',')[2], line.Split(",")[3], line.Split(",")[4] });
    }
}

List<Order> orders = new List<Order>(); //Initialise orders List
string path = "orders.csv"; //Read from orders.csv file 
string[] orderLines = File.ReadAllLines(path);

int latestOrderID = 0;
foreach (string line in orderLines)
{
    try
    {
        latestOrderID = Convert.ToInt32(line.Split(",")[0]);
    } catch { }
}

void InitializeCustomerDict() //Customer information in the dictionary
{
    string path = "customers.csv"; //Assign "path" to customer.csv file

    try
    {
        string[] lines = File.ReadAllLines(path); //Read all the contents from the customer.csv file

        for (int i = 1; i < lines.Length; i++) //Once again, starting from 1 to avoid format errors.
        {
            string[] data = lines[i].Split(','); //For each line in csv, the fields in each line are split by a comma ','

            string name = data[0]; //Name would be the first field
            int memberID = Convert.ToInt32(data[1]); //Member ID would be the second field
            DateTime dob = Convert.ToDateTime(data[2]); //Date of Birth would be the third field
            string membershipStatus = data[3]; //Membership Status would be the fourth field
            int membershipPoints = Convert.ToInt32(data[4]); //Membership Points would be the fifth field
            int punchCard = Convert.ToInt32(data[5]); //PunchCard would be the sixth field

            Customer customer = new Customer(name, memberID, dob); //Instantiated customer object

            customer.Rewards = new PointCard(); //Initialise rewards as a new Pointcard
            customer.Rewards.Tier = membershipStatus;
            customer.Rewards.Points = membershipPoints;
            customer.Rewards.PunchCard = punchCard;

            customerDict.Add(memberID, customer); //Added customer information that was in the customers.csv file to the dictionary itself by having its key being memberID and customer object as its value
        }
    }
    catch (Exception ex)
    {
        Console.WriteLine("Unable to initialise customerDict: {0}", ex.Message); //Should there be an issue reading from the csv file, this exception will show up
    }
}

// ------------------------------------------------------------------------------------------------
object[] ObtainIceCreamInformation()
{
    while (true)
    {
        List<Flavour> selectedFlavourList = new List<Flavour>(); //Initialised list of the selected flavour for values to be stored in it when user inputs.
        List<Topping> selectedToppingList = new List<Topping>(); //Initialised list of the selected topping for values to be stored in it when user inputs.
        Console.Write("Choose Ice Cream Option\r\n-----------------------\r\n[1] Cup\r\n[2] Cone\r\n[3] Waffle (+$3)\r\n_______________________\r\nEnter option: "); //Prompt user for their ice cream option
        int optionNo = Convert.ToInt32(Console.ReadLine());
        string option = "";

        if (optionNo == 1) //If option is 1, it is set to Cup
        {
            option = "Cup";
        }
        else if (optionNo == 2) //If option is 2, it is set to Cone
        {
            option = "Cone";
        }
        else if (optionNo == 3) //If option is 3, it is set to Waffle
        {
            option = "Waffle";
        }
        else
        {
            Console.WriteLine("Invalid option. Please try again.");
            continue;
        }

        Console.Write("Enter number of scoops (1-3): "); //Prompt user to input the number of scoops
        int scoops = Convert.ToInt32(Console.ReadLine());

        if (scoops < 1 || scoops > 3)
        {
            Console.WriteLine("Invalid number of scoops. Please choose within a range of 1 to 3 scoops."); //Should the number of scoops less than 1 or more than 3, user will be prompted to enter number of scoops again.
            continue;
        }

        int flavoursEntered = 0; //Initialised flavoursEntered value
        while (flavoursEntered < scoops) //Since the maximum number of flavours that can be entered is the number of scoops, the loop can only iterate a particular number of times depending on scoops.
        {
            Console.WriteLine("Choose Flavour options: ");
            for (int i = 0; i < flavourList.Count; i++)
            {
                Console.WriteLine("[{0}] {1}", i + 1, flavourList[i][0]);
            }
            Console.Write("Enter option (0 if done): "); //Prompts user to enter flavours
            try
            {
                int flavourNo = Convert.ToInt32(Console.ReadLine());
                if (flavourNo == 0) //If flavour option is 0, it breaks out of the loop and moves on with the rest of the program.
                {
                    break;
                }

                else if (flavourNo < 1 || flavourNo > flavourList.Count)
                {
                    Console.WriteLine("Invalid flavour number. Please try again."); //If flavour number is invalid, user will be prompted to enter flavour number again.
                    continue;
                }

                string selectedFlavour = flavourList[flavourNo - 1][0]; //Assigns the selectedFlavour variable to the particular element in the flavourList that contains that particular flavour
                bool alreadySelected = false; //Boolean initialised to check if user has inputting the same flavour

                foreach (var existingFlavour in selectedFlavourList)
                {
                    if (existingFlavour.Type == selectedFlavour)
                    {
                        alreadySelected = true;
                        existingFlavour.Quantity++; //If flavour is the same, it will just add up to the quantity of the flavour.
                        break;

                    }
                }

                if (!alreadySelected)
                {
                    Flavour flavour = new Flavour(selectedFlavour, Convert.ToInt32(flavourList[flavourNo - 1][1]) > 0, 1); //New flavour creation based on flavour option
                    selectedFlavourList.Add(flavour); //Add it to the selectedFlavourList

                }
                flavoursEntered++; //Increment flavoursEntered to ensure the loop can iterate a limited number of times.
            }
            catch
            {
                Console.WriteLine("Invalid input. Please enter a valid number"); //In the case where the input is invalid, format-wise particularly, this message will be displayed to the user.
            }
        }

        int toppingsEntered = 0; //Initialise toppingsEntered value 
        while (toppingsEntered < 4)//Since the maximum number of toppings entered can be 4, the loop can iterate 4 times before moving on to the rest of the program
        {   //Prompt user to choose their topping option
            Console.WriteLine("Choose Topping options: ");
            for (int i = 0; i < toppingList.Count; i++)
            {
                Console.WriteLine("[{0}] {1}", i + 1, toppingList[i][0]);
            }
            Console.Write("Enter option (0 if done): "); //Prompts user to enter toppings
            try
            {
                int toppingNo = Convert.ToInt32(Console.ReadLine());
                if (toppingNo == 0)
                {
                    break; //If user wishes to stop adding toppings, he could input 0 and the program will move on
                }

                else if (toppingNo < 1 || toppingNo > toppingList.Count) //If the topping option the user input in is invalid, the program will prompt the user to enter in a valid topping number.
                {
                    Console.WriteLine("Invalid topping number. Please try again.");
                    continue;
                }

                string selectedTopping = toppingList[toppingNo - 1][0]; //Assigns the selectedFlavour variable to the particular element in the flavourList that contains that particular flavour
                bool alreadySelected = false; //Boolean is initialised to check if the user inputs the same toppings

                foreach (var existingTopping in selectedToppingList)
                {
                    if (existingTopping.Type == selectedTopping)
                    {
                        alreadySelected = true; //If topping is found in the selectedToppingList, the boolean is then set to true.
                        break;
                    }
                }

                if (!alreadySelected)
                {
                    Topping topping = new Topping(selectedTopping); //New topping creation based on topping option
                    selectedToppingList.Add(topping); //Add it to the selectedToppingList

                    toppingsEntered++; //Increment toppingsEntered to ensure the loop can iterate a limited number of times.
                }
                else
                {
                    Console.WriteLine("You've already selected that topping. Please select another one."); //Prompts user to enter another topping
                }
            }
            catch
            {
                Console.WriteLine("Invalid input. Please enter a valid number."); //In the case where the input is invalid, format-wise in particular, this message will be displayed to the user.
            }
        }

        IceCream iceCream; //Initialised and instantiated icecream object.

        if (option.ToLower() == "cup") //When the option is "cup"
        {
            return new object[] { option, scoops, selectedFlavourList, selectedToppingList };
        }
        else if (option.ToLower() == "cone")//When the option is "cone"
        {
            Console.Write("Dipped? (Y/N): ");//Prompts the user if they want their cone dipped
            string dippedInput = Console.ReadLine();
            bool dipped = dippedInput == "Y"; //Dipped is set to true if user inputs 'Y'
            return new object[] { option, scoops, selectedFlavourList, selectedToppingList, dipped };

        }
        else if (option.ToLower() == "waffle")//When the option is "waffle"
        {
            Console.WriteLine("Choose Waffle Flavour options: ");
            for (int i = 0; i < waffleFlavourList.Count; i++)
            {
                Console.WriteLine("[{0}] {1}", i + 1, waffleFlavourList[i][0]);
            }
            Console.Write("Enter option: "); //Prompts user to enter waffle flavour option
            int waffleNo = Convert.ToInt32(Console.ReadLine());
            if (waffleNo < 0 || waffleNo > waffleFlavourList.Count)
            {
                Console.WriteLine("Invalid waffle flavour option. Please try again");
            }
            string waffleFlavour = waffleFlavourList[waffleNo - 1][0];
            return new object[] { option, scoops, selectedFlavourList, selectedToppingList, waffleFlavour };

        }


        else
        {
            Console.WriteLine("Invalid option. Please try again."); //Should there be any invalid options besides cup, cone & waffle, the input is invalid and the user is prompted to try again.
            continue;
        }
    }
}

// Feature 1
void ListAllCustomers()
{
    string path = "customers.csv"; //Assigned "path" to the file customers.csv
    string[] lines = File.ReadAllLines(path); //Reading the customers.csv file from "path"
    Console.WriteLine("{0, -10} {1, -10} {2, -15} {3, -15} {4, -15} {5, -15}", "Name", "MemberId", "DOB", "MembershipStatus", "MembershipPoints", "PunchCard"); //Printing out the headers for display
    for (int i = 1; i < lines.Length; i++) //Starting i from 1 since we want to skip the headers to avoid formatting errors.
    {
        string[] data = lines[i].Split(','); //For each line in the csv file, each content/field is divided by a comma ','
        string? name = data[0]; //Name would be the first field
        int memberID = Convert.ToInt32(data[1]); //The Member ID would be the second field
        DateTime dob = Convert.ToDateTime(data[2]); //The date of birth would be the third field
        string? membershipStatus = data[3]; //The Membership Status would be the fourth field
        int membershipPoints = Convert.ToInt32(data[4]); //The Membership Points would be the fifth field
        int punchCard = Convert.ToInt32(data[5]); //The PunchCard would be the sixth field
        Console.WriteLine("{0, -10} {1, -10} {2, -15} {3, -16} {4, -16} {5, -15}", name, memberID, dob.ToString("dd/MM/yyyy"), membershipStatus, membershipPoints, punchCard); //Displaying the contents of the csv file below their respective headers.
    }
}

// Feature 2
void ListCurrentOrders()
{
    // Display Gold Queue
    Console.WriteLine("Gold Queue:");
    foreach(Order goldOrder in goldQueue)
    {
        Console.WriteLine(goldOrder + " Total cost: $" + goldOrder.CalculateTotal(flavourList,toppingList,optionList));
        Console.WriteLine("\t\t^");
    }

    // Display Regular Queue
    Console.WriteLine("Regular Queue:");
    foreach(Order regularOrder in regularQueue)
    {
        Console.WriteLine(regularOrder + " Total cost: $" + regularOrder.CalculateTotal(flavourList, toppingList, optionList));
        Console.WriteLine("\t\t^");
    }
}

// Feature 3
void RegisterNewCustomer() //RegisterNewCustomer method
{
    try
    {
        Console.Write("Enter name of customer: "); //Prompt user to enter name of customer 
        string name = Console.ReadLine();

        Console.Write("Enter ID number of customer: "); //Prompt user to enter the member ID of customer
        int memberID;
        while (true)
        {
            memberID = Convert.ToInt32(Console.ReadLine());
            if (!customerDict.ContainsKey(memberID))
            {
                break; // Exit the loop if the member ID is unique
            }
            else
            {
                Console.WriteLine("Duplicate member ID. Please enter a unique ID: "); //Warn user if the member ID isn't unique and prompts user to put in another ID
            }

        }
        Console.Write("Enter date of birth of customer in (dd/MM/yyyy): "); //Prompt user to enter the date of birth of customer
        DateTime dob = Convert.ToDateTime(Console.ReadLine());

        Customer newCustomer = new Customer(name, memberID, dob);//Instantiated a customer object 

        PointCard pointCard = new PointCard();//Instantiated a Pointcard object
        newCustomer.Rewards = pointCard; //Assigned Pointcard object to customer

        string newCustomerLine = string.Format("{0},{1},{2},{3},{4},{5}", newCustomer.Name, newCustomer.MemberID, newCustomer.DOB.ToString("dd/MM/yyyy"), "Ordinary", 0, 0); //Formatting the string of what will be appended to the csv file.

        customerDict.Add(memberID, newCustomer); //Add a new customer to customerDict once new customer registered.

        File.AppendAllLines("customers.csv", new[] { newCustomerLine }); //Appending the newly registered customer to the csv file

        Console.WriteLine("New customer successfully registered.");
    }
    catch
    {
        Console.WriteLine("The input format is incorrect. Do enter the Customer ID/DOB of Customer correctly."); //Should there be any issues regarding the format of what the user input for the member ID & DOB prompts, this message will show up.
    }
}

// Feature 4
void CreateCustomerOrder() //Create Customer Order Feature
{
    ListAllCustomers(); //Call List All Customers method
    int id; //Initialise integer id

    try
    {
        Console.Write("Enter Customer ID: "); //Prompt user for the customer ID
        id = Convert.ToInt32(Console.ReadLine());

        if (!customerDict.ContainsKey(id)) //In the case where the customer ID could not be found in the dictionary, the program tells the user to select a valid Customer ID
        {
            Console.WriteLine("Invalid Customer ID: ID must be a whole number from the above list.");
            return;
        }

        
        Customer selectedCustomer = customerDict[id]; //Instantiated customer object and assigned it to customerDict[id]

        int orderID = latestOrderID;
        Order newOrder = new Order(orderID, DateTime.Now); //Instantiated newOrder where the new order contains the member ID and the current time and date

        newOrder.IceCreamList = new List<IceCream>(); //Initialise the IceCreamList from Order object

        latestOrderID++;
        
        while (true) 
        {
            IceCream iceCream = null;
            object[] iceCreamInformation = ObtainIceCreamInformation();
            if (Convert.ToString(iceCreamInformation[0]).ToLower() == "cup")
            {
                iceCream = new Cup(Convert.ToString(iceCreamInformation[0]), Convert.ToInt32(iceCreamInformation[1]), (List<Flavour>) iceCreamInformation[2], (List<Topping>)iceCreamInformation[3]);//Create new ice cream based on option "Cup"
            } else if (Convert.ToString(iceCreamInformation[0]).ToLower() == "cone")
            {
                iceCream = new Cone(Convert.ToString(iceCreamInformation[0]), Convert.ToInt32(iceCreamInformation[1]), (List<Flavour>)iceCreamInformation[2], (List<Topping>)iceCreamInformation[3], Convert.ToBoolean(iceCreamInformation[4]));//Create new ice cream based on option "Cone"
            } else if (Convert.ToString(iceCreamInformation[0]).ToLower() == "waffle")
            {
                iceCream = new Waffle(Convert.ToString(iceCreamInformation[0]), Convert.ToInt32(iceCreamInformation[1]), (List<Flavour>)iceCreamInformation[2], (List<Topping>)iceCreamInformation[3], Convert.ToString(iceCreamInformation[4]));//Create new ice cream based on option "Waffle"
            }


            newOrder.AddIceCream(iceCream); //Add Ice Cream to the new order


            Console.Write("Would you like to add another ice cream to the order? (Y/N) "); //Prompts user if they'd like to add another ice cream to the order
            string response = Console.ReadLine();
            if (response != null && response.ToUpper() != "Y") //If user doesn't input 'Y', the program stops looping the while loop
            {
                break;
            }
        }

        selectedCustomer.CurrentOrder = newOrder; //Linking the new order to the customer's current order

        if (selectedCustomer.Rewards != null && selectedCustomer.Rewards.Tier == "Gold") //If customer has a gold-tier Pointcard append their order to the back of the gold queue
        {
            goldQueue.Enqueue(newOrder);
        }
        else
        {
            regularQueue.Enqueue(newOrder); //Otherwise append to the back of the regular queue
        }

        Console.WriteLine("Order has been made successfully."); //Display message that order has been made successfully.

    }
    catch (Exception ex)
    {
        Console.WriteLine("Invalid input. Please type in the right input. {0}", ex.Message);//If there's an invalid input, particularly format-wise, this message will be displayed.
    }
}

// Feature 5
void DisplayOrderDetails()
{
    // Display all customers
    ListAllCustomers();
    int id;

    // Validation to get ID of customer
    while (true)
    {
        try
        {
            Console.Write("Customer ID: ");
            id = Convert.ToInt32(Console.ReadLine());

            // if customer does not exist
            if(customerDict.ContainsKey(id) == false)
            {
                throw new FormatException();
            }

            break;
            
        } catch 
        { 
            Console.WriteLine("Invalid Customer ID: ID must be a whole number from the above list.");
        }

    }

    // Retrieve customer object and display customer's current order
    Customer customer = customerDict[id];
    Console.WriteLine("Current Order:");
    Console.WriteLine(customer.CurrentOrder);
    Console.WriteLine("_________________________________________");

    // Display order history
    Console.WriteLine("Order History:");
    foreach(Order pastOrder in customer.OrderHistory)
    {
        Console.WriteLine(pastOrder + " Time Fulfilled: " + pastOrder.TimeFulfilled);
    }
}

// Feature 6
void ModifyOrderDetails()
{
    // Display all customers
    ListAllCustomers();
    int id;

    // Validation to get customer ID
    while (true)
    {
        try
        {
            Console.Write("Customer ID: ");
            id = Convert.ToInt32(Console.ReadLine());

            if (customerDict.ContainsKey(id) == false)
            {
                throw new FormatException();
            }

            break;
        } catch
        {
            Console.WriteLine("Invalid Customer ID: ID must be a whole number from the above list.");
        }

    }

    Customer customer = customerDict[id];
    
    // Display current order
    int count = 0;
    if (customer.CurrentOrder != null)
    {
        Console.WriteLine("Current Order:");
        foreach (IceCream ic in customer.CurrentOrder.IceCreamList)
        {
            count++;
            Console.WriteLine("[{0}] {1}", count, ic);
        }
    } else
    {
        Console.WriteLine("No current order. Make an order first!");
        return;
    }

    // Choose option and validate
    Console.WriteLine(@"_____________________________
[1] Modify existing ice cream
[2] Add new ice cream
[3] Delete existing ice cream
_____________________________");
    int choice;
    while (true)
    {
        try
        {
            Console.Write("Choice: ");
            choice = Convert.ToInt32(Console.ReadLine());

            // if choice is not one of the three
            int[] choices = { 1, 2, 3 };
            if (choices.Contains(choice) == false)
            {
                throw new FormatException();
            }

            break;

        }
        catch
        {
            Console.WriteLine("Invalid choice: Choice must be a whole number from 1 to 3.");
        }
    }

    // Modify existing ice cream
    if (choice == 1)
    {
        int index;
        while (true)
        {
            try
            {
                Console.Write("Choose Ice Cream to modify: ");
                index = Convert.ToInt32(Console.ReadLine());

                // if non-existent ice cream chosen
                if (index > customer.CurrentOrder.IceCreamList.Count || index < 1)
                {
                    throw new FormatException();
                }
                break;

            } catch
            {
                Console.WriteLine($"Invalid choice: Choice must be a whole number from 1 to {customer.CurrentOrder.IceCreamList.Count}.");
            }
        }
        index -= 1;

        // obtain new ice cream details
        object[] iceCreamInformation = ObtainIceCreamInformation();


        // assign additional details for cone and waffle before modifying
        bool dipped = false;
        string waffleFlavour = "";
        if (Convert.ToString(iceCreamInformation[0]).ToLower() == "cone")
        {
            dipped = Convert.ToBoolean(iceCreamInformation[4]);
        } else if (Convert.ToString(iceCreamInformation[0]).ToLower() == "waffle")
        {
            waffleFlavour = Convert.ToString(iceCreamInformation[4]);
        }

        customer.CurrentOrder.ModifyIceCream(index, Convert.ToString(iceCreamInformation[0]), Convert.ToInt32(iceCreamInformation[1]), dipped, waffleFlavour, (List<Flavour>)iceCreamInformation[2], (List<Topping>)iceCreamInformation[3]);
 
    }
    else if (choice == 2) // Add new ice cream
    {
        IceCream newIceCream = null;
        object[] iceCreamInformation = ObtainIceCreamInformation(); // obtain all new ice cream details
        Console.WriteLine(iceCreamInformation[0]);

        // create new ice cream to order based on option
        if (Convert.ToString(iceCreamInformation[0]).ToLower() == "cup")
        {
            newIceCream = new Cup(Convert.ToString(iceCreamInformation[0]), Convert.ToInt32(iceCreamInformation[1]), (List<Flavour>)iceCreamInformation[2], (List<Topping>)iceCreamInformation[3]);//Create new ice cream based on option "Cup"
        }
        else if (Convert.ToString(iceCreamInformation[0]).ToLower() == "cone")
        {
            newIceCream = new Cone(Convert.ToString(iceCreamInformation[0]), Convert.ToInt32(iceCreamInformation[1]), (List<Flavour>)iceCreamInformation[2], (List<Topping>)iceCreamInformation[3], Convert.ToBoolean(iceCreamInformation[4]));//Create new ice cream based on option "Cone"
        }
        else if (Convert.ToString(iceCreamInformation[0]).ToLower() == "waffle")
        {
            newIceCream = new Waffle(Convert.ToString(iceCreamInformation[0]), Convert.ToInt32(iceCreamInformation[1]), (List<Flavour>)iceCreamInformation[2], (List<Topping>)iceCreamInformation[3], Convert.ToString(iceCreamInformation[4]));//Create new ice cream based on option "Waffle"
        }

        // add new ice cream to order
        customer.CurrentOrder.AddIceCream(newIceCream); //Add Ice Cream to the new order
        Console.WriteLine("Ice cream successfully added.");

    } else if (choice == 3) // delete ice cream
    {
        // only 1 ice cream in order
        if (customer.CurrentOrder.IceCreamList.Count <= 1) 
        {
            Console.WriteLine("You cannot have zero ice creams in an order!");
            return;
        }
        int index;
        // validate index of ice cream to delete from order
        while (true)
        {
            try
            {
                Console.Write("Choose Ice Cream to delete: ");
                index = Convert.ToInt32(Console.ReadLine());
                // if ice cream chosen does not exist
                if (index > customer.CurrentOrder.IceCreamList.Count || index < 1)
                {
                    throw new FormatException();
                }
                break;

            }
            catch
            {
                Console.WriteLine($"Invalid choice: Choice must be a whole number from 1 to {customer.CurrentOrder.IceCreamList.Count}.");
            }
        }
        index -= 1;
        customer.CurrentOrder.DeleteIceCream(index);
        Console.WriteLine("Ice cream successfully deleted.");
    }
    // display updated order
    Console.WriteLine(customer.CurrentOrder + " Total cost: $" + customer.CurrentOrder.CalculateTotal(flavourList, toppingList, optionList));

}

// Feature 7
void ProcessOrderCheckout()
{
    // Get most recent order from gold / regular queue
    Order order;
    try
    {
        order = goldQueue.Dequeue();
    } catch
    {
        try
        {
            order = regularQueue.Dequeue();
        } catch
        {
            Console.WriteLine("No orders at the moment, check back later!");
            return;
        }
    }
    Console.WriteLine("Order:");
    Console.WriteLine(order);
    
    // total cost
    Console.WriteLine("Total Cost: ${0:0.00}", order.CalculateTotal(flavourList, toppingList, optionList));

    // get customer object of order
    Customer customer = new();
    foreach (Customer c in customerDict.Values)
    {
        if (c.CurrentOrder == order)
        {
            customer = c;
            break;
        }
    }

    // display membership status and points
    Console.WriteLine("Membership Status: " + customer.Rewards.Tier);
    Console.WriteLine("Points: " + customer.Rewards.Points);

    // calculate total cost
    double final = order.CalculateTotal(flavourList, toppingList, optionList);

    // get most expensive ice cream's cost
    double max = 0;
    if (customer.IsBirthday())
    {
        foreach (IceCream ic in order.IceCreamList)
        {
            if (ic.CalculatePrice(flavourList, toppingList, optionList) > max)
            {
                max = ic.CalculatePrice(flavourList, toppingList, optionList);
            }
        }

        final -= max;
    }    

    // first ice cream for free
    if (customer.Rewards.PunchCard == 10)
    {
        final -= order.IceCreamList[0].CalculatePrice(flavourList, toppingList, optionList);
        customer.Rewards.PunchCard = 0;
    }

    // redeem points and validate points inputted
    if (customer.Rewards.Tier != "Regular")
    {
        int points = 0;
        while (true)
        {
            try
            {
                Console.Write("How many points do you wish to redeem? You have {0} points. (1 point = $0.02): ", customer.Rewards.Points);
                points = Convert.ToInt32(Console.ReadLine());
                if (points < 0 || points > customer.Rewards.Points) 
                {
                    throw new FormatException();
                }
                break;
            } catch
            {
                Console.WriteLine("Invalid points: Points must be a whole number from 0-{0}", customer.Rewards.Points);
            }
        }
        customer.Rewards.RedeemPoints(points);
        final -= (points * 0.02);
    }

    // display final cost
    Console.WriteLine("Final cost: ${0:0.00}", Convert.ToString(final));

    // confirm payment
    Console.WriteLine("Press any key to confirm payment.");
    Console.ReadKey();


    // punch 
    foreach (IceCream ic in order.IceCreamList)
    {
        customer.Rewards.Punch();
    }

    // add points for order
    customer.Rewards.AddPoints(Convert.ToInt32(Math.Floor(final * 0.72)));

    // update membership
    if (customer.Rewards.Points >= 100)
    {
        customer.Rewards.Tier = "Gold";
    } else if (customer.Rewards.Points >= 50)
    {
        customer.Rewards.Tier = "Silver";
    }

    // add to order history
    order.TimeFulfilled = DateTime.Now;
    customer.OrderHistory.Add(order);

    customerDict[customer.MemberID] = customer; // Update customer dictionary

    // update customers.csv
    using (StreamWriter sw = new("customers.csv"))
    {
        sw.WriteLine("Name,MemberId,DOB,MembershipStatus,MembershipPoints,PunchCard");
        foreach (Customer c in customerDict.Values)
        {
            sw.WriteLine("{0},{1},{2},{3},{4},{5}", c.Name, c.MemberID, c.DOB, c.Rewards.Tier, c.Rewards.Points, c.Rewards.PunchCard);
        }
    }

    // add completed order to orders.csv
    using (StreamWriter sw = new("orders.csv", true))
    {
        foreach (IceCream ic in order.IceCreamList)
        {
            string flavour1 = "";
            string flavour2 = "";
            string flavour3 = "";

            string topping1 = "";
            string topping2 = "";
            string topping3 = "";
            string topping4 = "";

            // assign each flavour to each variable until no more flavours
            flavour1 = ic.Flavours[0].Type;
            if (ic.Flavours[0].Quantity == 2)
            {
                flavour2 = ic.Flavours[0].Type;
            } else if (ic.Flavours[0].Quantity == 3)
            {
                flavour2 = ic.Flavours[0].Type;
                flavour3 = ic.Flavours[0].Type;
            } else {
                try
                {
                    flavour2 = ic.Flavours[1].Type;
                    if (ic.Flavours[1].Quantity == 2)
                    {
                        flavour3 = ic.Flavours[1].Type;
                    }
                    flavour3 = ic.Flavours[2].Type;
                } catch { }
            }

            // try to assign each topping to variable until no more toppings
            try
            {
                topping1 = ic.Toppings[0].Type;
                topping2 = ic.Toppings[1].Type;
                topping3 = ic.Toppings[2].Type;
                topping4 = ic.Toppings[3].Type;
            } catch { }

            // write to orders.csv
            if (ic.Option == "Cup")
            {
                sw.WriteLine("{0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10},{11},{12},{13},{14}", order.ID, customer.MemberID, order.TimeReceived, order.TimeFulfilled, ic.Option, ic.Scoops, "", "", flavour1, flavour2, flavour3, topping1, topping2, topping3, topping4);

            }
            else if (ic.Option == "Cone")
            {
                sw.WriteLine("{0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10},{11},{12},{13},{14}", order.ID, customer.MemberID, order.TimeReceived, order.TimeFulfilled, ic.Option, ic.Scoops, ((Cone)ic).Dipped, "", flavour1, flavour2, flavour3, topping1, topping2, topping3, topping4);
            } else if (ic.Option == "Waffle")
            {
                sw.WriteLine("{0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10},{11},{12},{13},{14}", order.ID, customer.MemberID, order.TimeReceived, order.TimeFulfilled, ic.Option, ic.Scoops, "", ((Waffle)ic).WaffleFlavour, flavour1, flavour2, flavour3, topping1, topping2, topping3, topping4);
            }
        }
    }
}

// Feature 8
void DisplayChargedAmounts()
{
    List<Order> orders = new List<Order>(); //Initialise orders List
    string path = "orders.csv"; //Read from orders.csv file 
    string[] lines = File.ReadAllLines(path);
    for (int i = 1; i < lines.Length; i++)
    {
        string[] values = lines[i].Split(',');
        int id = Convert.ToInt32(values[0]);
        DateTime timeReceived = Convert.ToDateTime(values[2]);
        DateTime? timeFulfilled = Convert.ToDateTime(values[3]);

        var order = new Order //Create a new order object with the following properties assigned
        {
            ID = id,
            TimeReceived = timeReceived,
            TimeFulfilled = timeFulfilled,
            IceCreamList = new List<IceCream>()
        };

        IceCream iceCream = null;
        for (int j = 8; j <= 14; j++) //From column 9 to column 15 of the orders.csv file. I decided to add the relevant flavours & toppings found in the csv to the ice cream list
        {
            string flavour = values[j];
            if (flavour != null)
            {
                if (values[4] == "Cup")
                {
                    iceCream = new Cup //When Cup is the option, assign properties to these values
                    {
                        Option = values[4],
                        Scoops = Convert.ToInt32(values[5]),
                        Flavours = new List<Flavour>(),
                        Toppings = new List<Topping>()
                    };
                }
                else if (values[4] == "Cone")
                {
                    bool dipped = !string.IsNullOrWhiteSpace(values[6]) && Convert.ToBoolean(values[6]);
                    iceCream = new Cone  //When Cone is the option, assign properties to these values
                    {
                        Option = values[4],
                        Scoops = Convert.ToInt32(values[5]),
                        Flavours = new List<Flavour>(),
                        Toppings = new List<Topping>(),
                        Dipped = dipped
                    };


                }
                else if (values[4] == "Waffle")
                {
                    string waffleFlavour = values[7];
                    iceCream = new Waffle //When Waffle is the option, assign properties to these values
                    {
                        Option = values[4],
                        Scoops = Convert.ToInt32(values[5]),
                        Flavours = new List<Flavour>(),
                        Toppings = new List<Topping>(),
                        WaffleFlavour = waffleFlavour
                    };
                }
                for (int k = 8; k <= 10; k++)
                {
                    string flavor = values[k];
                    if (flavor != null)
                    {
                        bool isPremium = false;
                        foreach (var flavourInfo in flavourList)
                        {
                            if (flavourInfo[0] == flavor && Convert.ToInt32(flavourInfo[1]) > 0) //Checks if the flavour is premium
                            {
                                isPremium = true;
                                break;
                            }
                        }
                        iceCream.Flavours.Add(new Flavour(flavor, isPremium, 1));
                    }
                }

                // Add toppings to the ice cream object
                for (int k = 11; k <= 14; k++)
                {
                    string topping = values[k];
                    if (topping != null)
                    {
                        iceCream.Toppings.Add(new Topping(topping));
                    }
                }
                order.IceCreamList.Add(iceCream); //Add all ice cream instances to the Ice Cream List
            }
        }
        orders.Add(order); //All all relevant elements assigned in the order object to orders list
    }
    

    try
    {
        Console.Write("Enter the year: "); //Prompts user to enter the year
        int inputYear = Convert.ToInt32(Console.ReadLine());
        Console.WriteLine();

        double[] monthlyTotals = new double[12]; //Array that stores 12 months

        foreach (var order in orders) //For each order in the orders List
        {
            if (order.TimeFulfilled.HasValue && order.TimeFulfilled.Value.Year == inputYear) //If the timefulfilled has a value and the timefulfilled's year is equal to the user's input, 
            {
                int month = order.TimeFulfilled.Value.Month; //Month is set 
                double totalAmount = order.CalculateTotal(flavourList, toppingList, optionList);
                monthlyTotals[month - 1] += totalAmount; //Monthly totals of a specific month is then added to the total amount
            }
        }

        // Display monthly charged amounts breakdown
        for (int month = 1; month <= 12; month++)
        {
            string monthName = DateTimeFormatInfo.CurrentInfo.GetAbbreviatedMonthName(month); //Month number is set to its respective month name
            double totalAmount = monthlyTotals[month - 1]; //Amount for each month is set to its respective monthly totals for that specific month

            Console.WriteLine("{0} {1}:  ${2:0.00}", monthName, inputYear, totalAmount);  //Display the month, year and amount for that specific month
        }
        Console.WriteLine();
        // Display total charged amount for the year
        double totalChargedAmount = monthlyTotals.Sum(); //Total charged amount is then calculated as the monthly totals summed up together
        Console.WriteLine("Total:\t   ${0:0.00}", totalChargedAmount);
    } catch
    {
        Console.WriteLine("Invalid input. Please input an integer.");
    }
}
//Calculate amounts method

void DisplayMenu() //Display Menu Method
{
    Console.WriteLine("\nWelcome to I.C.Treats Management System");
    Console.WriteLine("What would you like to do?");
    Console.WriteLine("---------------------------------------");
    Console.WriteLine("[1] List all customers"); //Option 1
    Console.WriteLine("[2] List all current orders"); //Option 2
    Console.WriteLine("[3] Register a new customer"); //Option 3
    Console.WriteLine("[4] Create a customer's order"); //Option 4
    Console.WriteLine("[5] Display order details of a customer"); //Option 5
    Console.WriteLine("[6] Modify order details"); //Option 6
    Console.WriteLine("[7] Process order and checkout");
    Console.WriteLine("[8] Display monthly and total charged amounts for the year");
    Console.WriteLine("[0] Exit"); //Option 0 Exit
    Console.WriteLine("---------------------------------------");
}


while (true) //Starting off the program with a while loop
{
    DisplayMenu(); //Calling the DisplayMenu method
    try
    {
        Console.Write("Enter your option: "); //Asking user to input in their option
        int option = Convert.ToInt32(Console.ReadLine());

        if (option == 1)
        {
            ListAllCustomers(); //Calling ListAllCustomers() method when option is 1
        }
        else if (option == 2)
        {
            ListCurrentOrders();
        }
        else if (option == 3)
        {
            RegisterNewCustomer(); //Calling RegisterNewCustomer method when option is 3
        }
        else if (option == 4)
        {
            CreateCustomerOrder(); //Calling CreateCustomerOrder() method when option is 4
        }
        else if (option == 5)
        {
            DisplayOrderDetails();
        }
        else if (option == 6)
        {
            ModifyOrderDetails();
        }
        else if (option == 7)
        {
            ProcessOrderCheckout();
        }
        else if (option == 8)
        {
            DisplayChargedAmounts();
        }
        else if (option == 0)
        {
            Console.WriteLine("Terminating..."); //When option is 0, the program leaves a "Bye!" message before closing.
            break;
        }
        else
        {
            throw new FormatException();
        }
    }
    catch (Exception ex) 
    {
        Console.WriteLine("Invalid option: Option must be a whole number from 0 to 8.");
        Console.WriteLine(ex);
    }


}

