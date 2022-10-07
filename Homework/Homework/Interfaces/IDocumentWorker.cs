namespace Moravia.Homework.Interfaces;

/// <summary>
/// Interface for a class that will load and save data
/// </summary>
public interface IDocumentWorker : IDocumentLoader, IDocumentSaver
{
    Task LoadAndSave();
}
