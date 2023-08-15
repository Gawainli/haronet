namespace haronet.ProtobufServer;

public interface INetPackageEncoder
{
    void Encode(RingBuffer ringBuffer, INetPackage? encodePkg);
}