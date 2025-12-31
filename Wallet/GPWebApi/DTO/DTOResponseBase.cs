using System.Text.Json.Serialization;

namespace GPWebApi.DTO;

public class DTOResponseBase
{
    private List<Problem>? _problems;
    
    [JsonPropertyName("problems")]
    public List<Problem>? Problems 
    {
        get { return _problems != null && _problems.Count > 0 ? _problems : null; }
        set { _problems = value ?? new List<Problem>(); }
    }

    [JsonIgnore]
    internal bool HasErrors
    {
        get { return _problems != null && _problems.Any(n => n.ProblemType == ProblemType.Error); }
    }

    [JsonIgnore]
    internal bool HasWarnings
    {
        get { return _problems != null && _problems.Any(n => n.ProblemType == ProblemType.Warning); }
    }

    public void AddProblem(Problem problem)
    {
        _problems ??= new List<Problem>();
        _problems.Add(problem);
    }
    
    public void AddProblems(List<Problem> problems)
    {
        _problems ??= new List<Problem>();
        foreach (var problem in problems) 
            _problems.Add(problem);
    }
}
