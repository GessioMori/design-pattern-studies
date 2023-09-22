using Microsoft.VisualBasic;
using System;

public abstract class Character
{
    public string? Name { get; protected set; }
    public int Health { get; set; }

    public abstract void Attack();
}

public class Warrior : Character
{
    public Warrior(string name)
    {
        this.Name = name;
        this.Health = 100;
    }
    public override void Attack()
    {
        Console.WriteLine($"{this.Name}, the Warrior, attacks with a sword!");
    }
}

public class Wizard : Character
{
    public Wizard(string name)
    {
        this.Name = name;
        this.Health = 60;
    }
    public override void Attack()
    {
        Console.WriteLine($"{this.Name}, the Wizard, casts a fireball!");
    }
}

public abstract class CharacterFactory
{
    public abstract Character CreateCharacter(string name);
}

class WarriorFactory : CharacterFactory
{
    public override Character CreateCharacter(string name)
    {
        return new Warrior(name);
    }
}

class WizardFactory : CharacterFactory
{
    public override Character CreateCharacter(string name)
    {
        return new Wizard(name);
    }
}

class Program
{
    static void Main()
    {
        Console.WriteLine("Welcome to the Character Creator!");

        while (true)
        {
            Console.WriteLine("\nChoose a character type:");
            Console.WriteLine("1. Warrior");
            Console.WriteLine("2. Mage");
            Console.WriteLine("3. Exit");

            Console.Write("Enter your choice: ");
            string? choice = Console.ReadLine();

            if (choice == "1")
            {
                CharacterFactory factory = new WarriorFactory();
                CreateAndInteractWithCharacter(factory);
            }
            else if (choice == "2")
            {
                CharacterFactory factory = new WizardFactory();
                CreateAndInteractWithCharacter(factory);
            }
            else if (choice == "3")
            {
                Console.WriteLine("Goodbye!");
                break;
            }
            else
            {
                Console.WriteLine("Invalid choice. Please try again.");
            }
        }
    }

    static void CreateAndInteractWithCharacter(CharacterFactory factory)
    {
        Console.Write("Enter a name for your character: ");
        string? name = Console.ReadLine();

        if (name == null)
        {
            Console.Write("Invalid name.");
            return;
        }

        Character character = factory.CreateCharacter(name);

        Console.WriteLine($"\n{name}, a new {character.GetType().Name}, has been created!");
        Console.WriteLine($"{name} has {character.Health} health.");

        Console.WriteLine($"\n{character.Name} is ready for action!");

        while (true)
        {
            Console.WriteLine("\nWhat do you want to do?");
            Console.WriteLine("1. Attack");
            Console.WriteLine("2. Exit");

            Console.Write("Enter your choice: ");
            string? choice = Console.ReadLine();

            if (choice == "1")
            {
                character.Attack();
            }
            else if (choice == "2")
            {
                Console.WriteLine($"{character.Name} says: Farewell!");
                break;
            }
            else
            {
                Console.WriteLine("Invalid choice. Please try again.");
            }
        }
    }
}
