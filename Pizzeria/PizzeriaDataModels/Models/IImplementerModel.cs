namespace PizzeriaDataModels.Models
{
    public interface IImplementerModel : IId
    {
        string ImplementerFIO { get; }

        string Password { get; }

        int WorkExperience { get; }

        int Qualification { get; }
    }
}
