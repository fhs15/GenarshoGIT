namespace GenashoMatchmaker.Manager
{
    public class MatchmakerManager
    {
        protected static List<string> joinCodes = new();

        public string? GetHost()
        {
            var joinCode = joinCodes.FirstOrDefault();
            if(joinCode != null)
            {
                joinCodes.Remove(joinCode); 
            }
            return joinCode;
        }

        public void AddJoinCode(string joinCode)
        {
            if (String.IsNullOrWhiteSpace(joinCode)) return;
            joinCodes.Add(joinCode);
        }
    }
}
