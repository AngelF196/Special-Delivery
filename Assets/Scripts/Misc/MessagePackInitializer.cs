using MessagePack;
using MessagePack.Resolvers;
using UnityEngine;

public class MessagePackInitializer    // Not making game objects with this class/component, so it will not inherit from MonoBehaviour
{
    // Trust me, this is needed b/c MessagePack's deserializer will complain about certain Unity members like Vector3 not being recognized without this
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    public static void InitializeMessagePack()
    {
        IFormatterResolver[] resolvers = new IFormatterResolver[]
        {
            StandardResolver.Instance, MessagePack.Unity.UnityResolver.Instance
        };

        IFormatterResolver compositeResolver = CompositeResolver.Create(resolvers);
        MessagePackSerializerOptions options = MessagePackSerializerOptions.Standard.WithResolver(compositeResolver).WithSecurity(MessagePackSecurity.UntrustedData);
        MessagePackSerializer.DefaultOptions = options;
    }
}
