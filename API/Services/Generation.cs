namespace NeotechAPI.Services
{
    public class Generation
    {
        public string Name { get; set; }
        public string Attributes { get; set; }
        public string Contracts { get; set; }
        public string Numbness { get; set; }
        public string MinimumAge { get; set; }
        public string MaximumAge { get; set; }
        public string Probability { get; set; }
        public string Description { get; set; }

        public Generation(Dictionary<string, string> generation)
        {
            Name = generation["Generation"];
            Attributes = generation["Attributes"];
            Contracts = generation["Contracts"];
            Numbness = generation["Numbness"];
            MinimumAge = generation["Minimum Age"];
            MaximumAge = generation["Maximum Age"];
            Probability = generation["Probability"];
            Description = generation["Description"];
        }
    }
}