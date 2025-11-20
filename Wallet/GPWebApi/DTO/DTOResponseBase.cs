namespace GPWebApi.DTO;

public class DTOResponseBase
{
    List<Problem> _problems = new List<Problem>();
    
    public List<Problem> Problems 
    {
        get { return _problems != null && _problems.Count > 0 ? _problems : null; }
        set { _problems = value; }
    }

    internal bool HasErrors
    {
        get { return _problems != null ? _problems.Any(n => n.ProblemType == ProblemType.Error) : false; }
    }

    internal bool HasWarnings
    {
        get { return _problems != null ? _problems.Any(n => n.ProblemType == ProblemType.Warning) : false; }
    }

    public void AddProblem(Problem problem)
    {
        _problems.Add(problem);
    }
    public void AddProblems(List<Problem> problems)
    {
        foreach (var problem in problems) 
            _problems.Add(problem);
    }
}
