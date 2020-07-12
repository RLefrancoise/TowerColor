using UnityEngine;
using Zenject;

namespace Framework.UI
{
    /// <summary>
    /// Popping message factory by using a prefab
    /// </summary>
    public class PoppingMessageFactory : PlaceholderFactory<Object, IPoppingMessage>
    {
    }
    
    /// <summary>
    /// Popping messsage factory by using a prefab
    /// </summary>
    /// <typeparam name="TMessage">Message type</typeparam>
    public class PoppingMessageFactory<TMessage> : PlaceholderFactory<Object, TMessage> where TMessage : IPoppingMessage
    {}
}