namespace Domain.Models.Exceptions;

/// <summary>
/// Exception type for cases where a module is not found.
/// </summary>
public class ModuleNotFoundException : NotFoundException
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ModuleNotFoundException"/> class with a specified module ID.
    /// </summary>
    public ModuleNotFoundException(Guid moduleId)
        : base($"Module with Id '{moduleId}' was not found.") { }

    /// <summary>
    /// Initializes a new instance of the <see cref="ModuleNotFoundException"/> class with a default message.
    /// </summary>
    public ModuleNotFoundException()
        : base("The requested module was not found.") { }
}