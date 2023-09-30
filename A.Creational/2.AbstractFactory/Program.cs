public enum WeaponType
{
    Sword, Bow, Staff
}

public interface RacialFactory
{
    public Character CreateCharacter(String name);

    public Weapon CreateWeapon(WeaponType type);
}

public abstract class Weapon
{
    public int Power;
    public int Magic;
    public readonly WeaponType Type;
    public Weapon(int power, int magic, WeaponType type)
    {
        this.Power = power;
        this.Magic = magic;
        this.Type = type;
    }
}

public abstract class Character
{
    public int Health;
    private Weapon? _weapon;
    public readonly string Name;

    public Character(string name)
    {
        this.Name = name;
    }

    public void AttachWeapon(Weapon weapon)
    {
        this._weapon = weapon;
    }

    public void Attack()
    {
        if (_weapon == null)
        {
            Console.WriteLine($"{this.Name} can not attack because there is no weapon!");
        }
        else
        {
            Console.WriteLine($"{this.Name} attacked with a {this._weapon.Type} with power {this._weapon.Power}!");
        }
    }

    public void CastSpell()
    {
        if (_weapon == null)
        {
            Console.WriteLine($"{this.Name} can not attack because there is no weapon!");
        }
        else
        {
            Console.WriteLine($"{this.Name} cast a spell using their {this._weapon.Type} with magic power {this._weapon.Magic}!");
        }
    }
}

public class Elf : Character
{
    public Elf(string name) : base(name)
    {
        this.Health = 60;
    }
}

public class Orc : Character
{
    public Orc(string name) : base(name)
    {
        this.Health = 100;
    }
}

public class ElfWeapon : Weapon
{
    public ElfWeapon(WeaponType type) : base(10, 30, type)
    {
    }
}

public class OrcWeapon : Weapon
{
    public OrcWeapon(WeaponType type) : base(30, 10, type)
    {
    }
}

public class ElfFactory : RacialFactory
{
    public Character CreateCharacter(string name)
    {
        return new Elf(name);
    }

    public Weapon CreateWeapon(WeaponType type)
    {
        return new ElfWeapon(type);
    }
}

public class OrcFactory : RacialFactory
{
    public Character CreateCharacter(string name)
    {
        return new Orc(name);
    }

    public Weapon CreateWeapon(WeaponType type)
    {
        return new OrcWeapon(type);
    }
}

class Program
{
    public static void Main()
    {

        RacialFactory factory;

        Console.WriteLine("Welcome to the Character Creator!");

        while (true)
        {
            Console.WriteLine("\nChoose a race:");
            Console.WriteLine("1. Elf");
            Console.WriteLine("2. Orc");
            Console.WriteLine("3. Exit");

            Console.Write("Enter your choice: ");

            string? choice = Console.ReadLine();

            if (choice == "1")
            {
                factory = new ElfFactory();
                CreateAndInteractWithCharacter(factory);
            }
            else if (choice == "2")
            {
                factory = new OrcFactory();
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

    static void CreateAndInteractWithCharacter(RacialFactory factory)
    {
        Console.Write("Enter a name for your character: ");

        string? name = Console.ReadLine();

        if (name == null)
        {
            Console.Write("Invalid name.");
            return;
        }

        Character character = factory.CreateCharacter(name);

        Console.WriteLine($"\n{character.Name}, a new {character.GetType().Name}, has been created!");
        Console.WriteLine($"{character.Name} has {character.Health} health.");

        while (true)
        {
            Console.WriteLine("\nChoose a weapon:");
            Console.WriteLine("1. Sword");
            Console.WriteLine("2. Bow");
            Console.WriteLine("3. Staff");

            string? choice = Console.ReadLine();

            if (!int.TryParse(choice, out int intValue))
            {
                Console.WriteLine("Invalid input.");
            }

            if (choice != null && Enum.IsDefined(typeof(WeaponType), intValue - 1))
            {
                character.AttachWeapon(factory.CreateWeapon((WeaponType)(intValue - 1)));
                break;
            }
            else
            {
                Console.WriteLine("Invalid choice. Please try again.");
            }
        }

        Console.WriteLine($"{character.Name} is ready!");

        while (true)
        {
            Console.WriteLine("\nWhat do you want to do?");
            Console.WriteLine("1. Attack");
            Console.WriteLine("2. Cast spell");
            Console.WriteLine("3. Exit");

            Console.Write("Enter your choice: ");
            string? choice = Console.ReadLine();

            if (choice == "1")
            {
                character.Attack();
            }
            else if (choice == "2")
            {
                character.CastSpell();
            }
            else if (choice == "3")
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