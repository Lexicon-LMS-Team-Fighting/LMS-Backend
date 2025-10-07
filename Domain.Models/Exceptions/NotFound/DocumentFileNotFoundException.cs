namespace Domain.Models.Exceptions;

/// <summary>
/// Exception type for cases where a document file is not found.
/// </summary>  
public class DocumentFileNotFoundException : NotFoundException
{
    /// <summary>
    /// Initializes a new instance of the <see cref="DocumentFileNotFoundException"/> class with a default message.
    /// </summary>
    public DocumentFileNotFoundException(Guid documentId)
        : base("The requested document file for the document with Id '{documentId}' was not found.") { }
}