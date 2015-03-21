namespace Ages
{  

    public enum DataType {
        SciencePoint,
        CulturePoint,
        Science,
        Culture,
        Happiness,
        Strength,
        Food,
        Ore,
        CivilPoint,
        MilitaryPoint,
    }

    public enum CardType
    {
        Civil,
        Military,
    }

    public enum CardClass
    {
        Farm,
        Mine,
        Leader,
        Action,
        Govt,
        Other,
        UrbanBuilding,
        Special,
        MilitaryTech,   
        None,
        //Wonder,
        //Lab,
        //Event,

    }

    public enum UrbanBuildingType
    {
        Temple,
        Arena,
        Lab,
        Library,
        Theater,
    }

    public enum GameAction {
        IncreasePopu,
        BuildUnit,
        PickUpCard,
        UpgradeUnit,
        PlayCard,
        DestroyUnit,
        DoNothing,
        Other,    
    }

    public delegate void GoToWasteDelegate(object sender,int player);
    public delegate void SendMessgeToRegister(string message);
    public delegate void SwitchButton(string content);
    public delegate void AddReversePack(ReversePack pack);

}