namespace Wittel_Bot
{
    public class NPC
    {
        public string userName = "---";
        public string firstName = "---";
        public string lastName = "---";
        public bool barracks = false;
        public bool active = false;
        public int sex = 0; // 0=Unassigned | 1=Female | 2=Male
        public int race = 0; // 0=Unassigned | 1=Human | 2=Elf | 3=Orc | 4=Gnome | 5=Dragonborn | 6=Dwarf | 7=Half Orc | 8=Half Elf | 9=Tiefling | 10=Halfling | 11=Goliaths | 12=Aasimar
        public string description = "---";
    }   
}
