namespace Moravia.Homework.Interfaces;

/// <summary>
/// Interface for classes used as a result of data loading or as a source for data saving
/// </summary>
public interface IDataDocument
{
    public string Title { get; set; }
    public string Text { get; set; }
}
