using System.Text.Json;


public abstract class CharacterBuilder<T> where T : Character
{
    protected T? product;
    public abstract void reset();
    public abstract T build();
    public abstract void setStats(CharacterAttributes baseStats);
    public abstract void setSkills(List<Skill> skills);
    public abstract void setEquips(List<Equipment> equips);
}

public class CharacterAttributes
{
    public int Strength { get; set; }
    public int Dexterity { get; set; }
    public int Constitution { get; set; }
    public int Intelligence { get; set; }
    public int Wisdom { get; set; }
    public int Charisma { get; set; }
}

public class Equipment
{
    public required string Id { get; set; }
    public required string Name { get; set; }
    public int Durability { get; set; }
}

public class Weapon : Equipment
{
    public required int Damage { get; set; }
}

public class Armor : Equipment
{
    public required int Defense { get; set; }
}

public class Skill
{
    public required string Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public int Cooldown { get; set; }
    public int Damage { get; set; }
}

public abstract class Character
{
    public string Name { get; private set; }
    public CharacterAttributes Stats { get; set; }

    public List<Equipment> Equipments { get; set; }

    public List<Skill> Skills { get; set; }
}

public class Warrior : Character
{
}

public class WarriorBuilder : CharacterBuilder<Warrior>
{
    public override Warrior build()
    {
        if (this.product != null)
        {
            return this.product;
        }
        throw new Exception("Nothing was built.");
    }

    public override void reset()
    {
        this.product = new Warrior();
    }

    public override void setEquips(List<Equipment> equips)
    {
        if (this.product != null)
        {
            List<Equipment> baseEquips = new List<Equipment>()
            { new Weapon(){Name = "Sword", Damage = 20,Durability = 10, Id = "war_weapon_1"  },
            { new Armor(){Name = "Plate armor", Defense = 20,Durability = 20, Id = "war_armor_1"  } } };

            equips.AddRange(baseEquips);

            this.product.Equipments = equips;
        }
    }

    public override void setSkills(List<Skill> skills)
    {
        if (this.product != null)
        {
            List<Skill> baseSkills = new List<Skill>()
            { new Skill() {Name = "Slash", Description = "Slash", Cooldown = 1, Damage = 10, Id = "war_skill_1" } };

            skills.AddRange(baseSkills);

            this.product.Skills = skills;
        }
    }

    public override void setStats(CharacterAttributes baseStats)
    {
        if (this.product != null)
        {
            baseStats.Strength += 5;
            baseStats.Constitution += 5;

            this.product.Stats = baseStats;
        }
    }
}

public class Warlock : Character
{
}

public class WarlockBuilder : CharacterBuilder<Warlock>
{
    public override Warlock build()
    {
        if (this.product != null)
        {
            return this.product;
        }
        throw new Exception("Nothing was built.");
    }

    public override void reset()
    {
        this.product = new Warlock();
    }

    public override void setEquips(List<Equipment> equips)
    {
        if (this.product != null)
        {
            List<Equipment> baseEquips = new List<Equipment>()
            { new Weapon(){Name = "Staff", Damage = 5, Durability = 8, Id = "lock_weapon_1"  },
            { new Armor(){Name = "Cloth armor", Defense = 10, Durability = 10, Id = "lock_armor_1"  } } };

            equips.AddRange(baseEquips);

            this.product.Equipments = equips;
        }
    }

    public override void setSkills(List<Skill> skills)
    {
        if (this.product != null)
        {
            List<Skill> baseSkills = new List<Skill>()
            { new Skill() {Name = "Ignite", Description = "Ignite", Cooldown = 3, Damage = 10, Id = "lock_skill_1" },
            { new Skill() {Name = "Flame arc", Description = "Flame arc", Cooldown = 5, Damage = 30, Id = "lock_skill_1" }}};

            skills.AddRange(baseSkills);

            this.product.Skills = skills;
        }
    }

    public override void setStats(CharacterAttributes baseStats)
    {
        if (this.product != null)
        {
            baseStats.Intelligence += 5;
            baseStats.Charisma += 5;

            this.product.Stats = baseStats;
        }
    }
}

public class CharacterData
{
    public List<Weapon> Weapons { get; set; }
    public List<Armor> Armors { get; set; }
    public List<Skill> Skills { get; set; }
}

public class CharacterBuildDirector
{
    private readonly CharacterData? characterData;
    public CharacterBuildDirector()
    {
        string jsonFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "char_data.json");
        Console.WriteLine(jsonFilePath);

        if (File.Exists(jsonFilePath))
        {
            try
            {
                string jsonData = File.ReadAllText(jsonFilePath);

                this.characterData = JsonSerializer.Deserialize<CharacterData>(jsonData);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error reading or deserializing the JSON file: " + ex.Message);
            }
        }
        else
        {
            Console.WriteLine("JSON file not found in the specified location.");
        }
    }
    public void makeRandomCharacter<T>(CharacterBuilder<T> builder) where T : Character
    {
        if (characterData != null)
        {
            builder.reset();
            builder.setSkills(GetRandom(characterData.Skills, 2));

            List<Equipment> equips = new();
            equips.AddRange(GetRandom(characterData.Armors, 1));
            equips.AddRange(GetRandom(characterData.Weapons, 1));
            builder.setEquips(equips);
            builder.setStats(GetRandomAttributes());
        }
    }

    private List<T> GetRandom<T>(List<T> list, int count)
    {
        Random random = new Random();

        List<T> shuffledList = list.OrderBy(s => random.Next()).ToList();

        List<T> randomSamples = shuffledList.Take(count).ToList();

        return randomSamples;
    }

    private CharacterAttributes GetRandomAttributes()
    {
        Random random = new Random();

        CharacterAttributes characterAttributes = new CharacterAttributes()
        {
            Charisma = random.Next(1, 11),
            Constitution = random.Next(1, 11),
            Dexterity = random.Next(1, 11),
            Intelligence = random.Next(1, 11),
            Strength = random.Next(1, 11),
            Wisdom = random.Next(1, 11)
        };

        return characterAttributes;
    }
}

class Program
{
    static void Main()
    {
        List<Character> listOfCharacters = new List<Character>();

        Console.WriteLine("Welcome to the Character Creator!");

        while (true)
        {
            Console.WriteLine("\nChoose an action:");
            Console.WriteLine("1. Create random character");
            Console.WriteLine("2. Print characters");
            Console.WriteLine("3. Exit");

            Console.Write("Enter your choice: ");
            string? choice = Console.ReadLine();

            if (choice == "1")
            {
                CreateRandomCharacter();
            }
            else if (choice == "2")
            {
                PrintCharacters();
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

        void PrintCharacters()
        {
            string json = JsonSerializer.Serialize(listOfCharacters, new JsonSerializerOptions
            {
                WriteIndented = true
            });

            Console.WriteLine(json);
        }

        void CreateRandomCharacter()
        {
            CharacterBuildDirector characterBuildDirector = new CharacterBuildDirector();

            while (true)
            {
                Console.WriteLine("\nChoose a class:");
                Console.WriteLine("1. Warrior");
                Console.WriteLine("2. Warlock");
                Console.WriteLine("3. Exit");

                Console.Write("Enter your choice: ");
                string? choice = Console.ReadLine();

                if (choice == "1")
                {
                    WarriorBuilder warriorBuilder = new WarriorBuilder();
                    characterBuildDirector.makeRandomCharacter(warriorBuilder);
                    listOfCharacters.Add(warriorBuilder.build());
                    Console.Write("Char created!");
                }
                else if (choice == "2")
                {
                    WarlockBuilder warlockBuilder = new WarlockBuilder();
                    characterBuildDirector.makeRandomCharacter(warlockBuilder);
                    listOfCharacters.Add(warlockBuilder.build());
                    Console.Write("Char created!");
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
    }


}