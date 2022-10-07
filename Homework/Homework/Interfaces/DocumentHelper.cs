namespace Moravia.Homework.Interfaces;

/// <summary>
/// Class for reading data and storing it
/// </summary>
public class DocumentHelper : IDataDocument
{
    public string Title { get; set; }
    public string Text { get; set; }
}
