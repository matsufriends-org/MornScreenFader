using System.Threading;
using Cysharp.Threading.Tasks;

namespace MornScreenFade
{
    public interface IMornScreenFade
    {
        void FadeFillImmediate();
        void FadeFill();
        void FadeFill(float duration);
        UniTask FadeFillAsync(CancellationToken ct);
        UniTask FadeFillAsync(float duration, CancellationToken ct);
        void FadeClearImmediate();
        void FadeClear();
        UniTask FadeClearAsync(CancellationToken ct);
        UniTask FadeClearAsync(float duration, CancellationToken ct);
    }
}