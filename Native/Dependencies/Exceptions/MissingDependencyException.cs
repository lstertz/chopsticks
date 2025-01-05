using System;

namespace Chopsticks.Dependencies.Exceptions
{
    /// <summary>
    /// Represents a failure to resolve a required dependency.
    /// </summary>
    /// <param name="message">The message that describes the error.</param>
    public class MissingDependencyException(string message) : Exception(message);
}
