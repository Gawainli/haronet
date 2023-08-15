namespace haronet.ProtobufServer;

public interface INetPackageDecoder
{
    void Decode(RingBuffer ringBuffer, List<INetPackage> outNetPackages);
}